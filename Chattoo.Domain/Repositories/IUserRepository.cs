﻿using System.Collections.Generic;
using System.Linq;
using Chattoo.Domain.Entities;

namespace Chattoo.Domain.Repositories
{
    /// <summary>
    /// Rozhraní repozitáře uživatelů aplikace.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Vrací všechny uživatele, kteří jsou součástí daného komunikačního kanálu.
        /// </summary>
        /// <param name="channelId">Id komunikačního kanálu, jehož uživatelé se mají načíst.</param>
        /// <returns>Kolekci <see cref="User"/> uživatelů, kteří jsou součástí daného komunikačního kanálu.</returns>
        public IQueryable<User> GetByChannelId(string channelId);
        
        /// <summary>
        /// Vrací všechny uživatele, kteří jsou součástí dané skupiny uživatelů.
        /// </summary>
        /// <param name="groupId">Id uživatelské skupiny, jejíž uživatelé se mají načíst.</param>
        /// <returns>Kolekci <see cref="User"/> uživatelů, kteřé jsou součástí dané uživatelské skupiny.</returns>
        public IQueryable<User> GetByGroupId(string groupId);
        
        public IQueryable<User> GetByCalendarEventId(string calendarEventId);

        /// <summary>
        /// Vrací všechny uživatele, kteří odpovídají hledanému výrazu.
        /// </summary>
        /// <param name="searchTerm">Výraz, dle kterého se mají dohledat uživatelé.</param>
        /// <param name="excludedUserIds">List Id uživatelů, kteří jsou mimo vyhledávání..</param>
        /// <param name="channelId">Id komunikačního kanálu.</param>
        /// <param name="groupId">Id skupiny.</param>
        public IQueryable<User> GetBySearchTerm(string searchTerm, List<string> excludedUserIds, string channelId, string groupId);
    }
}