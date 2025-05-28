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

        public async Task<Account?> CreateAccountAsync(Account regAccount)
        {
            if (regAccount == null)
                return null;

            var username = regAccount.UserName;
            var plainPassword = regAccount.Password;
            var customerName = regAccount?.UserName;
            var phone = regAccount?.Phone;
            var email = regAccount?.Email;

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
            regAccount.UserName = customerName;
            regAccount.Email = email;
            regAccount.Phone = phone;
            regAccount.Password = _passwordHasher.HashPassword(null!, plainPassword);
            regAccount.Role = "customer";

            await _accountRepository.AddAccountAsync(regAccount);
            return regAccount;
        }

    }
}