using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Core.Models;
using DatingApp.API.Core.Repositories;
using DatingApp.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Persistence.Repositories
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T: class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recipientId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.FirstOrDefaultAsync(u => u.UserId == userId && u.IsMain);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(x => x.Sender).ThenInclude(p => p.Photos)
            .Include(x => x.Recipient).ThenInclude(p => p.Photos).AsQueryable();

            switch(messageParams.MessageContainer)
            {
                case "Inbox":
                {
                    messages = messages.Where(x => x.RecipientId == messageParams.UserId && !x.RecipientDeleted);
                    break;
                }

                case "Outbox":
                {
                    messages = messages.Where(x => x.SenderId == messageParams.UserId && !x.SenderDeleted);
                    break;
                }

                default:
                {
                    messages = messages.Where(x => x.RecipientId == messageParams.UserId && !x.IsRead && !x.RecipientDeleted);
                    break;
                }
            }

            messages = messages.OrderByDescending(x => x.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
            .Include(x => x.Sender).ThenInclude(p => p.Photos)
            .Include(x => x.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.RecipientId == userId && m.SenderId == recipientId && !m.RecipientDeleted || 
                m.RecipientId == recipientId && m.SenderId == userId && !m.SenderDeleted)
            .OrderByDescending(o => o.MessageSent)
            .ToListAsync();

            return messages;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.Include(p => p.Photos).AsQueryable();

            if (isCurrentUser)
            {
                query = query.IgnoreQueryFilters();
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(x => x.LastActive).AsQueryable();

            users = users.Where(x => x.Id != userParams.UserId);

            users = users.Where(x => x.Gender == userParams.Gender);

            if(userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(x => userLikers.Contains(x.Id));
            }

            if(userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(x => userLikees.Contains(x.Id));
            }

            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            }

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created":
                    {
                        users = users.OrderByDescending(x => x.Created);
                        break;
                    }

                    default:
                    {
                        users = users.OrderByDescending(x => x.LastActive);
                        break;
                    }
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var users = await _context.Users.Include(x => x.Likers).Include(x => x.Likees).FirstOrDefaultAsync(x => x.Id == id);

            if(likers)
            {
                return users.Likers.Where(x => x.LikeeId == id).Select(x => x.LikerId);
            }
            else
            {
                return users.Likees.Where(x => x.LikerId == id).Select(x => x.LikeeId);
            }
        }
    }
}