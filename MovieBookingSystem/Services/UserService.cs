using MovieBookingSystem.Models;
using MovieBookingSystem.Repositories;

namespace MovieBookingSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }

        public async Task AddUser(User user)
        {
            await _userRepository.SaveUser(user);
        }
    }
}
