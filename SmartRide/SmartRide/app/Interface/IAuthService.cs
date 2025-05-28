using Models;

namespace Interface
{
    public interface IAuthService
    {
        Task<Account> CheckAccountAsync(string email, string password);
        Task<Account> CreateAccountAsync(Account account);
    }
}
