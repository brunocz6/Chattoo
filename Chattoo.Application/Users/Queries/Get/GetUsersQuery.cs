﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chattoo.Application.Common.Mappings;
using Chattoo.Application.Common.Models;
using Chattoo.Application.Common.Queries;
using Chattoo.Application.Users.DTOs;
using Chattoo.Domain.Repositories;

namespace Chattoo.Application.Users.Queries
{
    /// <summary>
    /// Dotaz na uživatele (dle filtru).
    /// </summary>
    public class GetUsersQuery : PaginatedQuery<UserDto>
    {
        /// <summary>
        /// Vrací nebo nastavuje hledaný výraz, podle kterého se mají dohledat uživatelé.
        /// </summary>
        public string SearchTerm { get; set; }

        public List<string> ExcludedUserIds { get; set; }
        
        public string ChannelId { get; set; }
        
        public string GroupId { get; set; }
    }

    public class GetUsersQueryHandler : PaginatedQueryHandler<GetUsersQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            // Načtu kolekci uživatelů v dané skupině a zpracuju na stránkovanou kolekci.
            var user = _userRepository.GetBySearchTerm(
                    request.SearchTerm,
                    request.ExcludedUserIds,
                    request.ChannelId,
                    request.GroupId
            );
            
            var result = await user
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

            return result;
        }
    }
}
