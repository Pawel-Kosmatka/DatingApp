using System;
using System.Linq;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Message> MarkUnreadAsRead(this IQueryable<Message> query, string currentUserName)
        {
            var unreadMessages = query.Where(m => m.DateRead == null && m.RecipientUserName == currentUserName);

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }
            return query;
        }
        public static IQueryable<AppUser> IgnoreQueryFilterForLoggedUser(this IQueryable<AppUser> query, string currentUserName, bool isCurrentUser)
        {
            if (isCurrentUser) query = query.IgnoreQueryFilters();

            return query;
        }
    }
}