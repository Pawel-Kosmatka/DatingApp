using System;
using System.Linq;
using API.Entities;

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
    }
}