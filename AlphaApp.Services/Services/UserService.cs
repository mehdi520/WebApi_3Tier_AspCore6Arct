using AlphaApp.ApplicationServices.Contracts;
using AlphaApp.Repositories.Contracts;
using AlphaApp.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaApp.ApplicationServices.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        { 
            _userRepository = userRepository;
        }
        public string getSomething()
        {
           return _userRepository.getSomething();
        }
    }
}
