document.addEventListener("DOMContentLoaded", function () {
    const userCredential = document.getElementById("userPreferencesModalLabel");
    if (userCredential) {
        const user = JSON.parse(localStorage.getItem("user"));
        if (user) {
            document.getElementById("userName").value = `${user.username}`;
            document.getElementById("userAddress").value = `${user.email}`;
            document.getElementById("userPhone").value = `${user.phone}`;
        }
    };

    const overlay = document.getElementById("loadingOverlay");

    const addDriverbtn = document.getElementById("add-driver__btn");

    if (addDriverbtn) {
        addDriverbtn.addEventListener("click", function (e) {
            e.preventDefault();
            const userAddress = document.getElementById('userAddress');
            if (!userAddress || !userAddress.value) {
                alert("No email address.");
                return;
            }
            confirmDriver(userAddress.value);
        });

        function confirmDriver(userEmail) {
            const overlay = document.getElementById("loadingOverlay");

            if (confirm("Are you sure you want to become a driver?")) {
                overlay && (overlay.style.display = "flex");

                fetch('/Driver/AddDriver', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ email: userEmail })
                })
                    .then(response => {
                        overlay && (overlay.style.display = "none");

                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error("Failed to become a driver.");
                        }
                    })
                    .then(data => {
                        if (data.success && data.redirectUrl) {
                            alert("You are now a driver!");
                            window.location.href = data.redirectUrl;
                        } else {
                            alert("An error occurred. Please try again.");
                        }
                    })
                    .catch(error => {
                        overlay && (overlay.style.display = "none");
                        alert("An error occurred: " + error.message);
                    });
            }
        }
    }

    const form = document.getElementById("loginForm");
    if (form) {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();

            overlay && (overlay.style.display = "flex");

            const formData = new FormData(form);
            const data = {
                Email: formData.get("Email"),
                Password: formData.get("Password")
            };

            try {
                const response = await fetch('/Log/Login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const result = await response.json();
                    const { id, username, email, phone } = result.user;
                    console.log("Logged in as:", username, email, phone, id);

                    // You can save it to localStorage, update UI, etc.
                    localStorage.setItem("user", JSON.stringify(result.user));
                    window.location.href = result.redirectUrl;
                } else {
                    const error = await response.json();
                    showError(error.error || "Login failed");
                }
            } catch (err) {
                console.error(err);
                showError("Something went wrong.");
            } finally {
                overlay && (overlay.style.display = "none");
            }
        });

        function showError(message) {
            const container = document.querySelector(".error-message-container");
            if (container) container.remove();

            const div = document.createElement("div");
            div.className = "error-message-container";
            div.innerHTML = `<p class="text-danger">${message}</p>`;
            form.appendChild(div);
        }
    }

    const mapContainer = document.getElementById('map');
    const useMyLocationBtn = document.getElementById("useMyLocationBtn");
    const pickupAddressInput = document.getElementById("pickupAddress");
    const pickupLatInput = document.getElementById("pickupLatitude");
    const pickupLngInput = document.getElementById("pickupLongitude");

    if (useMyLocationBtn) {
        useMyLocationBtn.addEventListener("click", function () {
            if (!navigator.geolocation) {
                alert("Geolocation is not supported by your browser.");
                return;
            }

            overlay && (overlay.style.display = "flex");

            navigator.geolocation.getCurrentPosition(async (position) => {
                const lat = position.coords.latitude;
                const lng = position.coords.longitude;

                // Set hidden inputs
                pickupLatInput.value = lat;
                pickupLngInput.value = lng;

                // Center map if available
                if (window.map) {
                    window.map.setView([lat, lng], 15);
                    if (window.pickupMarker) {
                        window.map.removeLayer(window.pickupMarker);
                    }
                    window.pickupMarker = L.marker([lat, lng]).addTo(window.map);
                }

                try {
                    const res = await fetch(`https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`);
                    if (!res.ok) throw new Error("Failed to reverse geocode location");
                    const data = await res.json();
                    const address = data.display_name;
                    document.getElementById('pickupAddress').value = address;
                    pickupAddressInput.value = data.display_name || "Current Location";
                } catch {
                    pickupAddressInput.value = "Unable to get address";
                } finally {
                    overlay && (overlay.style.display = "none");
                }
            }, (error) => {
                alert("Could not get your location: " + error.message);
                overlay && (overlay.style.display = "none");
            });
        });
    }

    if (pickupAddressInput) {
        let searchTimeout;
        pickupAddressInput.addEventListener('input', function () {
            const query = pickupAddressInput.value.trim();
            if (searchTimeout) clearTimeout(searchTimeout);

            if (query.length < 3) {
                // You can optionally clear lat/lng if address is too short
                return;
            }

            // Debounce input so we don't spam API on every keystroke
            searchTimeout = setTimeout(async () => {
                try {
                    const response = await fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}`);
                    if (!response.ok) throw new Error("Failed to search address");
                    const results = await response.json();

                    if (results && results.length > 0) {
                        const place = results[0];
                        const lat = parseFloat(place.lat);
                        const lon = parseFloat(place.lon);

                        // Update hidden inputs
                        pickupLatInput.value = lat;
                        pickupLngInput.value = lon;

                        // Center and add marker on map
                        if (window.map) {
                            window.map.setView([lat, lon], 15);
                            if (window.pickupMarker) {
                                window.map.removeLayer(window.pickupMarker);
                            }
                            window.pickupMarker = L.marker([lat, lon]).addTo(window.map);
                        }
                    }
                } catch (err) {
                    console.error("Address search error:", err);
                }
            }, 300); // wait 500ms after typing stops
        });
    }

    if (mapContainer) {
        const mode = mapContainer.dataset.mode; // view or select
        window.map = L.map('map').setView([10.762622, 106.660172], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(map);

        if (mode === "view") {
            L.marker([10.762622, 106.660172])
                .addTo(map)
                .bindPopup("Driver location")
                .openPopup();
        } else if (mode === "select") {
            let marker;
            map.on('click', async function (e) {
                const { lat, lng } = e.latlng;
                if (marker) map.removeLayer(marker);
                marker = L.marker([lat, lng]).addTo(map);

                document.getElementById('latitude').value = lat;
                document.getElementById('longitude').value = lng;

                try {
                    const response = await fetch(`https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`);
                    if (!response.ok) throw new Error("Reverse geocoding failed");
                    const data = await response.json();
                    const address = data.display_name;
                    document.getElementById('destinationAddress').value = address;
                } catch (err) {
                    console.error(err);
                    document.getElementById('destinationAddress').value = "Address not found.";
                }
            });
        }
    }

    const registerForm = document.getElementById("registerForm");

    if (registerForm) {
        registerForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            overlay && (overlay.style.display = "flex");

            const formData = new FormData(registerForm);
            const confirmPasswordInput = document.getElementById("password-confirm");

            const data = {
                UserName: formData.get("UserName"),
                Email: formData.get("Email"),
                Password: formData.get("Password"),
                Phone: formData.get("Phone"),
            };

            const confirmPassword = confirmPasswordInput?.value;

            if (data.Password !== confirmPassword) {
                showError("Passwords do not match.");
                overlay && (overlay.style.display = "none");
                return;
            }

            try {
                const response = await fetch('/Log/Register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const result = await response.json();
                    const { id, username, email, phone } = result.user;
                    window.location.href = result.redirectUrl;
                } else {
                    const error = await response.json();
                    showError(error.error || "Register failed");
                }
            } catch (err) {
                console.error(err);
                showError("Something went wrong.");
            } finally {
                overlay && (overlay.style.display = "none");
            }
        });
    }

    const bookingRideForm = document.getElementById("bookingRideForm");

    if (bookingRideForm) {
        bookingRideForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            overlay && (overlay.style.display = "flex");
            const userEmail = document.getElementById('userAddress').value;
            const pickupLatitude = document.getElementById('pickupLatitude').value;
            const pickupLongitude = document.getElementById('pickupLongitude').value;
            const destinationLatitude = document.getElementById('latitude').value;
            const destinationLongitude = document.getElementById('longitude').value;

            const formData = new FormData(bookingRideForm);
            if (formData.get("pickupAddress") === null || formData.get("destinationAddress") === null) {
                showError("Pick-up Address is empty!");
                return;
            }

            // Adjust the keys here according to your booking form's input names
            const data = {
                userEmail: userEmail, // or however you want to pass the user's email
                PickupLocation: formData.get("pickupAddress"),
                pickupLatitude: pickupLatitude,
                pickupLongitude: pickupLongitude,
                DropoffLocation: formData.get("destinationAddress"),
                destinationLatitude: destinationLatitude,
                destinationLongitude: destinationLongitude,
                pickupDate: formData.get("pickupDate"),
            };

            try {
                const response = await fetch('/Ride/BookRide', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const result = await response.json();
                    if (result.success === false) {
                        showError(result.error || "Booking failed");
                        return;
                    }
                    // Do something on success, e.g. redirect or alert
                    console.log(result);
                    alert("Ride booked successfully!");
                    bookingRideForm.reset();  // clear form if you want
                    // maybe redirect if backend sends redirectUrl:
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                } else {
                    const error = await response.json();
                    showError(error.error || "Booking failed");
                }
            } catch (err) {
                console.error(err);
                showError("Something went wrong.");
            } finally {
                overlay && (overlay.style.display = "none");
            }
        });

        // Reuse your showError but append error inside bookingRideForm
        function showError(message) {
            const container = bookingRideForm.querySelector(".error-message-container");
            if (container) container.remove();

            const div = document.createElement("div");
            div.className = "error-message-container";
            div.innerHTML = `<p class="text-danger">${message}</p>`;
            bookingRideForm.appendChild(div);
        }
    }

    const acceptButtons = document.querySelectorAll(".accept-button");
    acceptButtons.forEach(button => {
        button.addEventListener("click", async function (e) {
            e.preventDefault();

            const rideId = this.dataset.rideId;
            const userEmail = this.dataset.userEmail;

            if (!rideId || !userEmail) {
                alert("Missing ride or user information.");
                return;
            }

            overlay && (overlay.style.display = "flex");

            try {
                const response = await fetch("/Driver/TakeRide", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        RideId: rideId,
                        UserEmail: userEmail
                    })
                });

                overlay && (overlay.style.display = "none");

                const result = await response.json();

                if (response.ok && result.success) {
                    if (result.success == true) {
                        alert("Ride successfully assigned to you!");
                        window.location.reload();
                    } else {
                        console.error(result.results);
                    }
                } else {
                    alert(result.message || "Failed to assign ride.");
                }

            } catch (err) {
                overlay && (overlay.style.display = "none");
                console.error(err);
                alert("An error occurred: " + err.message);
            }
        });
    });

    function showError(message) {
        const container = document.querySelector(".error-message-container");
        if (container) container.remove();

        const div = document.createElement("div");
        div.className = "error-message-container";
        div.innerHTML = `<p class="text-danger">${message}</p>`;
        registerForm.appendChild(div);
    }
});