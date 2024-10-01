using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> SaveUser(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int id) 
        { 
            var user = await _context.users.SingleOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<User> DeleteUser(int id)
        {
            User user = await _context.users.FirstOrDefaultAsync(x => x.Id==id);

            if (user != null) {
                _context.users.Remove(user);
                _context.SaveChanges();

                return user;
            }

            return user;
        }

    }
}
