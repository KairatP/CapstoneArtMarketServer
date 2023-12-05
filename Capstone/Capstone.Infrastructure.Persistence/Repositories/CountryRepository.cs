using AutoMapper;
using Capstone.Application.Interfaces.Repositories.Dictionaries;
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

namespace Capstone.Infrastructure.Persistence.Repositories
{
    public class CountryRepository : GenericRepositoryAsync<Country>, ICountry
    {
        private readonly DbSet<Country> _countires;

        private readonly IMapper _mapper;
        private readonly IAuthenticatedUser _authenticatedUser;

        public CountryRepository(ApplicationDbContext dbContext, IMapper mapper, IAuthenticatedUser authenticatedUser) : base(dbContext)
        {
            _countires = dbContext.Set<Country>();
            _mapper = mapper;
            _authenticatedUser = authenticatedUser;
        }
    }
}
