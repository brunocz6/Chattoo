﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chattoo.Application.CalendarEvents.DTOs;
using Chattoo.Application.Common.Mappings;
using Chattoo.Application.Common.Models;
using Chattoo.Application.Common.Queries;
using Chattoo.Application.Common.Services;
using Chattoo.Domain.Repositories;

namespace Chattoo.Application.CommunicationChannels.Queries
{
    /// <summary>
    /// Dotaz na kalendářní události z komunikačního kanálu.
    /// </summary>
    public class GetCalendarEventsForCommunicationChannelQuery : PaginatedQuery<CalendarEventDto>
    {
        /// <summary>
        /// Vrací nebo nastavuje Id komunikačního kanálu, jehož kalendářní události se mají vrátit.
        /// </summary>
        public string ChannelId { get; set; }
    }

    public class GetCalendarEventsForCommunicationChannelQueryHandler : PaginatedQueryHandler<GetCalendarEventsForCommunicationChannelQuery, CalendarEventDto>
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationChannelRepository _communicationChannelRepository;
        private readonly ICalendarEventRepository _calendarEventRepository;
        private readonly GetByIdUserSafeService _getByIdUserSafeService;

        public GetCalendarEventsForCommunicationChannelQueryHandler(IMapper mapper, ICalendarEventRepository calendarEventRepository, ICommunicationChannelRepository communicationChannelRepository, GetByIdUserSafeService getByIdUserSafeService)
        {
            _mapper = mapper;
            _calendarEventRepository = calendarEventRepository;
            _communicationChannelRepository = communicationChannelRepository;
            _getByIdUserSafeService = getByIdUserSafeService;
        }

        public override async Task<PaginatedList<CalendarEventDto>> Handle(GetCalendarEventsForCommunicationChannelQuery request, CancellationToken cancellationToken)
        {
            // Pokusím se načíst kanál.
            var channel = await _getByIdUserSafeService.GetAsync(_communicationChannelRepository, request.ChannelId);

            var events = _calendarEventRepository.GetByCommunicationChannelId(channel.Id);
            
            // Načtu kolekci kalendářních událostí komunikačního kanálu a zpracuju na stránkovanou kolekci.
            var result = await events
                .ProjectTo<CalendarEventDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
            
            return result;
        }
    }
}