﻿using Chattoo.Application.Common.Exceptions;
using Chattoo.Application.Common.Interfaces;
using Chattoo.Application.Common.Security;
using MediatR;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Chattoo.Domain.Exceptions;
using Chattoo.Domain.Interfaces;

namespace Chattoo.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserIdService _currentUserIdService;
        private readonly IIdentityService _identityService;

        public AuthorizationBehaviour(
            ICurrentUserIdService currentUserIdService,
            IIdentityService identityService)
        {
            _currentUserIdService = currentUserIdService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                // Must be authenticated user
                if (_currentUserIdService.UserId is null)
                {
                    throw new UnauthorizedAccessException();
                }

                // Role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        var authorized = false;
                        foreach (var role in roles)
                        {
                            var isInRole = await _identityService.IsInRoleAsync(_currentUserIdService.UserId, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }

                        // Must be a member of at least one role in roles
                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }
                    }
                }

                // Policy-based authorization
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach(var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        var authorized = await _identityService.AuthorizeAsync(_currentUserIdService.UserId, policy);

                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }
                    }
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
    }
}
