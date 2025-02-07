﻿using Chattoo.Application.Groups.Commands;
using Chattoo.Domain.Enums;
using Chattoo.GraphQL.Extensions;
using GraphQL.Types;

namespace Chattoo.GraphQL.Mutation
{
    public class GroupRoleMutation : ObjectGraphType
    {
        public GroupRoleMutation()
        {
            Name = "GroupRoleMutation";
            
            this.FieldAsyncWithScope<StringGraphType, string>(
                "create",
                arguments: 
                new QueryArguments
                (
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "groupId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "permission" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new AddGroupRoleCommand()
                    {
                        Name = ctx.GetString("name"),
                        Permission = (UserGroupPermission)ctx.GetInt("permission"),
                        GroupId = ctx.GetString("groupId")
                    };

                    var id = await mediator.Send(command);

                    return id;
                }
            );
            
            this.FieldAsyncWithScope<BooleanGraphType, bool>(
                "delete",
                arguments: 
                new QueryArguments
                (
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "groupId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new DeleteGroupRoleCommand()
                    {
                        GroupId = ctx.GetString("groupId"),
                        Id = ctx.GetString("id")
                    };

                    await mediator.Send(command);

                    return true;
                }
            );
            
            this.FieldAsyncWithScope<BooleanGraphType, bool>(
                "update",
                arguments: 
                new QueryArguments
                (
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "groupId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "permission" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new UpdateGroupRoleCommand()
                    {
                        Id = ctx.GetString("id"),
                        Name = ctx.GetString("name"),
                        Permission = (UserGroupPermission)ctx.GetInt("permission"),
                        GroupId = ctx.GetString("groupId")
                    };

                    await mediator.Send(command);
                    
                    return true;
                }
            );
        }
    }
}