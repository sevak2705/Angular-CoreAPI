using System;
using System.Threading.Tasks;
using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
         public async Task<bool> UserExist(string userName)
        {
            //AnyAsyc check for  existance
            if (await _context.Users.AnyAsync(a => a.UserName == userName))
                return true;
            return false;
        }
        public async Task<User> Login(string userName, string password)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(a => a.UserName == userName);
            if (userData == null)
                return null;
            if (!VerifyPasswordHash(password, userData.PasswordHash, userData.PasswordSalt))
                return null;
            return userData;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var Hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
           {
               var computedHash =  Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++){
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
           }
           return true;

        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash , passwordSalt;
            CreatePasswordHash(password,out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        private void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] passwordSalt)
        {
           using(var Hmac = new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt = Hmac.Key;
               passwordHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           }
        }

    }
}