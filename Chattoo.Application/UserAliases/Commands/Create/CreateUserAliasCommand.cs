﻿using System.Threading;
using System.Threading.Tasks;
using Chattoo.Application.Common.Exceptions;
using Chattoo.Domain.Entities;
using Chattoo.Domain.Repositories;
using MediatR;

namespace Chattoo.Application.UserAliases.Commands.Create
{
    /// <summary>
    /// Příkaz pro vytvoření uživatelské přezdívky.
    /// </summary>
    public class CreateUserAliasCommand : IRequest<string>
    {
        /// <summary>
        /// Vrací nebo nastavuje Id uživatele, kterému se má vytvořit nová přezdívka.
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Vrací nebo nastavuje přezdívku uživatele.
        /// </summary>
        public string Alias { get; set; }
    }

    public class CreateUserAliasCommandHandler : IRequestHandler<CreateUserAliasCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserAliasRepository _userAliasRepository;

        public CreateUserAliasCommandHandler(IUnitOfWork unitOfWork, IUserAliasRepository userAliasRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userAliasRepository = userAliasRepository;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateUserAliasCommand request, CancellationToken cancellationToken)
        {
            // Pokusím se z datového zdroje vytáhnout uživatele, kterému se má přidat přezdívka.
            var user = await _userRepository.GetByIdAsync(request.UserId);

            // Pokud se uživatele nepodařilo dohledat, vracím výjimku.
            if (user is null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            
            // Vytvořím entitu naplněnou daty z příkazu.
            var entity = new UserAlias()
            {
                Alias = request.Alias
            };

            // Přidám záznam do datového zdroje a uložím.`
            await _userAliasRepository.AddOrUpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Vrátím Id vytvořeného záznamu.
            return entity.Id;
        }
    }
}