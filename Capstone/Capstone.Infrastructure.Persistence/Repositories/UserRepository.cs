using Capstone.Application.Exceptions;
using Capstone.Application.Interfaces.Repositories.Users;
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
    public class UserRepository : GenericRepositoryAsync<User>, IUser
    {
        private readonly DbSet<User> _entities;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _entities = dbContext.Set<User>();
        }

        public async Task<User> GetByUserName(string username)
        {
            var user = await _entities
                .Include(x => x.City)
                .Include(x => x.Country)
                .Include(x => x.Avatar)
                .Where(x => x.UserName == username).FirstOrDefaultAsync();

            if (user == null) throw new ApiException("User is not found");

            return user;
        }
    }
}
