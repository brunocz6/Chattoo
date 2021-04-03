﻿using Chattoo.Application.CommunicationChannelRoles.DTOs;
using GraphQL.Types;

namespace Chattoo.GraphQL.Types
{
    public sealed class CommunicationChannelRoleType : ObjectGraphType<CommunicationChannelRoleDto>
    {
        public CommunicationChannelRoleType()
        {
            Field(o => o.Id);
            Field(o => o.Name);
            // TODO: graphQL neumí EnumerationGraphType<flags enum>, takže musím vytvořit vlastní typ
            //Field(o => o.Permission);
            Field(o => o.ChannelId);
            Field(o => o.CreatedAt);
            Field(o => o.ModifiedAt, nullable: true);
        }
    }
}