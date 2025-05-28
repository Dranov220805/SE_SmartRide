using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class HomeRepository
    {
        private readonly MainDbContext _context;
        public HomeRepository(MainDbContext context)
        {
            _context = context;
        }
    }
}
