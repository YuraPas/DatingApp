using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adding entity. Class only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        /// <summary>
        /// Deleting entity. Could be class only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        /// <summary>
        /// Fetching all users
        /// </summary>
        /// <returns>List of entities in Users table</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            //Using include for displaying main profile photo and for other tasks with photos
            var users = await _context.Users.Include(photos => photos.Photos).ToListAsync();

            return users;
        }

        /// <summary>
        /// Fetching specific user and corresponding photos by user Id  number
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Fetched user</returns>
        public async Task<User> GetUser(int id)
        {
            //Adding photos for fetched user via Include
            var user = await _context.Users.Include(photos => photos.Photos).
                                    FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        /// <summary>
        /// Saving changes to database
        /// </summary>
        /// <returns>
        /// True if any entries have been written to database.
        /// False if none were made.
        /// </returns>
        public async Task<bool> SaveAll()
        {
            //According to documentation SaveChangesAsync returns the number of entries written to the database.
            //If any have been made return true, else false
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
