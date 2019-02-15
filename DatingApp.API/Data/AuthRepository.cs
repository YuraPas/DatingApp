using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Logins user based on username and password. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Null if there is no matches.
        /// User if a match was found
        /// </returns>
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);  // returns null if user was not found

            //checking if the user was found
            if (user == null)
                return null;

            //verifying password
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        /// <summary>
        /// Registers and adds a user to a database. Creates Hash and Salt property for user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>Registered user</returns>
        public async Task<User> Register(User user, string password)
        {
            //Inline variable declaration could be added
            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Initialize the keyed hash object. 
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                // Compute the hash of the password.
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // compare the computed hash with the password hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                // Salt property is assigned to random key
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        /// <summary>
        /// Checks if user exists in database based on username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// True is user with the same username exists. False if there is no match
        /// </returns>
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(user => user.Username == username))
                return true;

            return false;
        }
    }
}
