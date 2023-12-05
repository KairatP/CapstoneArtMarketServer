using Capstone.Application.Interfaces;
using Capstone.Application.Interfaces.Repositories.Dictionaries;
using Capstone.Application.Interfaces.Repositories.Files;
using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Interfaces.Repositories.Users;
using Capstone.Identity.Services;
using Capstone.Infrastructure.Persistence.Contexts;
using Capstone.Infrastructure.Persistence.Repositories;
using Capstone.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Capstone.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            #region Repositories

            #region General

            services.AddTransient<ITokenService, TokenService>();

            #endregion

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            services.AddTransient<IUser, UserRepository>();
            services.AddTransient<IPost, PostRepository>();
            services.AddTransient<ICountry, CountryRepository>();
            services.AddTransient<ICity, CityRepository>();
            services.AddTransient<IFile, FileRepository>();
            services.AddTransient<IPostFile, PostFileRepository>();

            #endregion Repositories
        }
    }
}