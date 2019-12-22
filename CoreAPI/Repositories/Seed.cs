using System.Collections.Generic;
using System.Linq;
using CoreAPI.Models;
using Newtonsoft.Json;

namespace CoreAPI.Repositories
{
    public class Seed
    {
     //Create a static method as we will never use it more than once, hence we also keep it sync.
        public static void SeedUser(DataContext dataContext)
        {
            if (!dataContext.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Repositories/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    //Modify or add those values which are not part of json file but we need to have that in our DB like password salt and password hash
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    dataContext.Users.Add(user);
                }
                dataContext.SaveChanges();
            }
        }
         private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using(var Hmac = new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt = Hmac.Key;
               passwordHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           }
        }
    }
}