using AutoMapper;
using Capstone.Application.Interfaces;
using Capstone.Domain.Entities;
using Capstone.Infrastructure.Persistence.Contexts;
using Capstone.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Application.Interfaces.Repositories.Dictionaries;

namespace Capstone.Infrastructure.Persistence.Repositories
{
    public class CityRepository : GenericRepositoryAsync<City>, ICity
    {
        private readonly DbSet<City> _cities;

        private readonly IMapper _mapper;
        private readonly IAuthenticatedUser _authenticatedUser;

        public CityRepository(ApplicationDbContext dbContext, IMapper mapper, IAuthenticatedUser authenticatedUser) : base(dbContext)
        {
            _cities = dbContext.Set<City>();
            _mapper = mapper;
            _authenticatedUser = authenticatedUser;
        }

    }
}
