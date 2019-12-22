using System.Threading.Tasks;
using CoreAPI.Models;

namespace CoreAPI.Repositories
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string userName, string password);
         Task<bool> UserExist(string userName);

    }
}