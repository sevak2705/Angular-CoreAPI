using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Repositories
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
             _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
             _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
             var user = await _context.Users.Include(a =>a.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users =  await _context.Users.Include(a =>a.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll() // this is called to save data to DB
        {
           return await _context.SaveChangesAsync() > 0;
        }
          public async Task<Photo> GetPhoto(int id)
        {
             var photo = await _context.Photos.FirstOrDefaultAsync(u => u.Id == id);

            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
           return await _context.Photos.Where(a => a.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}