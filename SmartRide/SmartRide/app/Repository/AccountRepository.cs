using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class AccountRepository
    {
        private readonly MainDbContext _context;

        public AccountRepository(MainDbContext context)
        {
            _context = context;
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Accounts?.ToList();
        }
        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }
        public async Task AddAccountAsync(RegAccount account)
        {
            var existingAccount = await GetAccountByEmailAsync(account.Email);
            if (existingAccount != null)
            {
                throw new Exception("Account with this email already exists.");
            }

            var newAccount = new Account
            {
                AccountId = Guid.NewGuid(),
                Email = account.Email,
                UserName = account.UserName,
                Password = account.Password, // Assuming password is already hashed
                Role = account.Role ?? "customer" // Default role if not specified
            };

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                AccountId = newAccount.AccountId,
                CustomerName = account.Customer?.CustomerName,
                Phone = account.Customer?.Phone,
                Email = account.Customer?.Email
            };

            _context.Accounts.Add(newAccount);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

    }
}
