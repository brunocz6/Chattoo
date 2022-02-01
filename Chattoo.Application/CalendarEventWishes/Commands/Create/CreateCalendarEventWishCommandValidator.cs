using System.Linq;
using Chattoo.Application.Common.Services;
using Chattoo.Domain.Extensions;
using Chattoo.Domain.ValueObjects;
using FluentValidation;

namespace Chattoo.Application.CalendarEventWishes.Commands.Create
{
    public class CreateCalendarEventWishCommandValidator : AbstractValidator<CreateCalendarEventWishCommand>
    {
        public CreateCalendarEventWishCommandValidator(DateIntervalService dateIntervalService)
        {
            RuleFor(x => x.DateIntervals)
                .Must(x => x.Count > 0).WithMessage("Alespoň 1 časový blok musí být určen.")
                .Must(x =>
                {
                    var intervals = x.Select(DateInterval.Create).ToList();
                    var isOverlapping = dateIntervalService.GetOverlapOfIntervals(intervals);

                    return !isOverlapping;
                }).WithMessage("Časové bloky se překrývají.");

            RuleFor(x => x.GroupId)
                .NotEmpty()
                .When(t => t.CommunicationChannelId.IsNullOrEmpty())
                .WithMessage("Id komunikačního kanálu nebo Id skupiny musí být vyplněno");
        }
    }
}