﻿using Chattoo.Application.CommunicationChannels.Commands;
using Chattoo.Domain.Enums;
using Chattoo.Domain.Extensions;
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
                    var command = new AddMessageAttachmentCommand()
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
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "messageId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new DeleteMessageAttachmentCommand()
                    {
                        ChannelId = ctx.GetString("channelId"),
                        MessageId = ctx.GetString("messageId"),
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
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "messageId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: async (ctx, mediator) =>
                {
                    var command = new UpdateMessageAttachmentCommand()
                    {
                        ChannelId = ctx.GetString("channelId"),
                        MessageId = ctx.GetString("messageId"),
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