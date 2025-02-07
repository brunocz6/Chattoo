﻿using FluentValidation;

namespace Chattoo.Application.Groups.Commands
{
    /// <summary>
    /// Validátor příkazu <see cref="DeleteGroupCommand"/>.
    /// </summary>
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty()
                    .WithMessage("Je nutno určit Id skupiny.");
        }
    }
}