using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Core.Models;
using DatingApp.API.Helpers;

namespace DatingApp.API.Core.Repositories
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
    }
}