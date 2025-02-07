﻿using System.Collections.Generic;
using Chattoo.Domain.Common;
using Chattoo.Domain.Enums;
using Chattoo.Domain.Interfaces;

namespace Chattoo.Domain.Entities
{
    /// <summary>
    /// Entita uživatelské role v kontextu skupiny.
    /// </summary>
    public class GroupRole : AuditableEntity, IAuditableEntity
    {
        private List<User> _users;
        
        protected GroupRole()
        {
            _users = new List<User>();
        }
        
        /// <summary>
        /// Vrací nebo nastavuje Id skupiny, do které tato role patří.
        /// </summary>
        public string GroupId { get; private set; }
        
        /// <summary>
        /// Vrací nebo nastavuje název uživatelské role.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Vrací nebo nastavuje oprávnění, které uživatelům poskytuje tato role.
        /// </summary>
        public UserGroupPermission Permission { get; private set; }

        public virtual IReadOnlyCollection<User> Users => _users.AsReadOnly();

        public static GroupRole Create(string groupId, string name, UserGroupPermission permission)
        {
            var entity = new GroupRole()
            {
                GroupId = groupId
            };
            
            entity.SetName(name);
            entity.SetPermission(permission);

            return entity;
        }

        public void SetName(string name)
        {
            Name = name;
        }
        
        public void SetPermission(UserGroupPermission permission)
        {
            Permission = permission;
        }
    }
}
