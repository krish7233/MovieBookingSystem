using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.Repositories
{
    public class UserRepository
    {
        private readonly MovieBookingDBContext _context;

        public UserRepository(MovieBookingDBContext context)
        {
            _context = context;
        }

        public async Task SaveUser(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        public User GetUser(string userName) 
        { 
            var user = _context.users.SingleOrDefault(x => x.Email == userName);

            return user;
        }

    }
}
