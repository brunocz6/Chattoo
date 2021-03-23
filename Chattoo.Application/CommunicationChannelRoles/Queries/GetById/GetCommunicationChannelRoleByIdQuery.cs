﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Chattoo.Application.Common.Exceptions;
using Chattoo.Application.CommunicationChannelRoles.DTOs;
using Chattoo.Domain.Entities;
using Chattoo.Domain.Repositories;
using MediatR;

namespace Chattoo.Application.CommunicationChannelRoles.Queries.GetById
{
    /// <summary>
    /// Dotaz na uživatelskou roli (z komunikačního kanálu) s daným Id.
    /// </summary>
    public class GetCommunicationChannelRoleByIdQuery : IRequest<CommunicationChannelRoleDto>
    {
        /// <summary>
        /// Vrací nebo nastavuje Id uživatelské role z komunikačního kanálu.
        /// </summary>
        public string Id { get; set; }
    }
    
    public class GetCommunicationChannelRoleByIdQueryHandler : IRequestHandler<GetCommunicationChannelRoleByIdQuery, CommunicationChannelRoleDto>
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationChannelRoleRepository _communicationChannelRoleRepository;

        public GetCommunicationChannelRoleByIdQueryHandler(IMapper mapper, ICommunicationChannelRoleRepository communicationChannelRoleRepository)
        {
            _mapper = mapper;
            _communicationChannelRoleRepository = communicationChannelRoleRepository;
        }

        public async Task<CommunicationChannelRoleDto> Handle(GetCommunicationChannelRoleByIdQuery request, CancellationToken cancellationToken)
        {
            // Načtu uživatelskou roli z datového zdroje.
            var role = await _communicationChannelRoleRepository.GetByIdAsync(request.Id);

            // Pokud se uživatelskou roli s daným Id nepodařilo dohledat, vracím chybu.
            if (role is null)
            {
                throw new NotFoundException(nameof(CommunicationChannelRole), request.Id);
            }

            // Převedu entitu na dto.
            var result = _mapper.Map<CommunicationChannelRoleDto>(role);

            return result;
        }
    }
}