﻿using Chattoo.Application.Common.Interfaces;
using Chattoo.Domain.Repositories;
using Chattoo.Infrastructure.Identity;
using Chattoo.Infrastructure.Persistence;
using Chattoo.Infrastructure.Persistence.Repositories;
using Chattoo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chattoo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Připojení k databázi (v paměti/k SQL serveru).
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("inMemoryDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            // Přidání repozitářů.
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(ICommunicationChannelRepository), typeof(CommunicationChannelRepository));
            services.AddScoped(typeof(IGroupRepository), typeof(GroupRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ICalendarEventRepository), typeof(CalendarEventRepository));
            services.AddScoped(typeof(ICalendarEventWishRepository), typeof(CalendarEventWishRepository));
            services.AddScoped(typeof(IChannelMessageRepository), typeof(ChannelMessageRepository));

            // Jednotka pro správu/manipulaci datového zdroje.
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<ApplicationDbContext>());

            // Služba pro publish událostí na doménových entitách.
            services.AddScoped<IDomainEventService, DomainEventService>();

            // Služby pro autentizaci/správu uživatelů aplikace.
            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            // Služba pro zjištění aktuálního času.
            services.AddTransient<IDateTime, DateTimeService>();

            services.AddTransient<IIdentityService, IdentityService>();
            
            services.AddAuthentication()
                .AddJwtBearer()
                .AddIdentityServerJwt();

            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });*/

            return services;
        }
    }
}
