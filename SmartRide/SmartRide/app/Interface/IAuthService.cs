using Models;

namespace Interface
{
    public interface IAuthService
    {
        Task<Account> CheckAccountAsync(string email, string password);
        Task<RegAccount> CreateAccountAsync(RegAccount account);
    }
}
