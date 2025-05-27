using Interface;
using Models;
using Repository;

namespace Services
{
    public class HomeService : IHomeService
    {
        private readonly AccountRepository _accountRepository;

        public HomeService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public List<Account> GetAllAccounts()
        {
            return _accountRepository.GetAllAccounts();
        }
    }
}
