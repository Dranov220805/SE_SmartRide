using Microsoft.AspNetCore.Identity;
using Interface;
using Models;
using Repository;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly AccountRepository _accountRepository;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AuthService(AccountRepository accountRepository, IPasswordHasher<Account> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Account?> CheckAccountAsync(string email, string password)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(email);
            if (account == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(account, account.Password, password);
            return result == PasswordVerificationResult.Success ? account : null;
        }

        public async Task<RegAccount?> CreateAccountAsync(RegAccount regAccount)
        {
            if (regAccount == null)
                return null;

            var username = regAccount.UserName;
            var plainPassword = regAccount.Password;
            var customerName = regAccount.Customer?.CustomerName;
            var phone = regAccount.Customer?.Phone;
            var email = regAccount.Customer?.Email;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword) ||
                string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var existing = await _accountRepository.GetAccountByEmailAsync(email);
            if (existing != null)
                return null;

            var accountId = Guid.NewGuid();

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                CustomerName = customerName,
                Phone = phone,
                Email = regAccount.Email
            };

            regAccount.AccountId = accountId;
            regAccount.Email = email;
            regAccount.Password = _passwordHasher.HashPassword(null!, plainPassword);
            regAccount.Role = "customer";
            regAccount.Customer = customer;

            await _accountRepository.AddAccountAsync(regAccount);
            return regAccount;
        }

    }
}