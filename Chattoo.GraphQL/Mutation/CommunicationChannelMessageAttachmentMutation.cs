﻿using Chattoo.Application.Common.Extensions;
using Chattoo.Application.CommunicationChannelMessageAttachments.Commands.Create;
using Chattoo.Application.CommunicationChannelMessageAttachments.Commands.Delete;
using Chattoo.Application.CommunicationChannelMessageAttachments.Commands.Update;
using Chattoo.Domain.Enums;
using Chattoo.GraphQL.Extensions;
using GraphQL.Types;

namespace Chattoo.GraphQL.Mutation
{
    public class CommunicationChannelMessageAttachmentMutation : ObjectGraphType
    {
        public CommunicationChannelMessageAttachmentMutation()
        {
            Name = "CommunicationChannelMessageAttachmentMutation";
            
            this.FieldAsyncWithScope<StringGraphType, string>(
                "create",
                arguments: 
                new QueryArguments
                (
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "content" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "permission" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "messageId" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new CreateCommunicationChannelMessageAttachmentCommand()
                    {
                        Name = ctx.GetString("name"),
                        Content = ctx.GetString("content").GetBytes(),
                        Type = (CommunicationChannelMessageAttachmentType)ctx.GetInt("type"),
                        MessageId = ctx.GetString("messageId")
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
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new DeleteCommunicationChannelMessageAttachmentCommand()
                    {
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
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new UpdateCommunicationChannelMessageAttachmentCommand()
                    {
                        Id = ctx.GetString("id"),
                        Name = ctx.GetString("name")
                    };

                    await mediator.Send(command);

                    return true;
                }
            );
        }
    }
}