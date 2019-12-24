using System.Collections.Generic;
using System.Threading.Tasks;
using CoreAPI.Models;

namespace CoreAPI.Repositories
{
    public interface IDatingRepository
    {
        //generic for add and delete
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);

        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}