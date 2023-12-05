using AutoMapper;
using Capstone.Application.Interfaces.Repositories.Users;
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
using Capstone.Application.Interfaces.Repositories.Files;

namespace Capstone.Infrastructure.Persistence.Repositories
{
    public class FileRepository : GenericRepositoryAsync<File>, IFile
    {
        private readonly DbSet<File> _files;

        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IAuthenticatedUser _authenticatedUser;

        public FileRepository(ApplicationDbContext dbContext, IMapper mapper, IUser user, IAuthenticatedUser authenticatedUser) : base(dbContext)
        {
            _files = dbContext.Set<File>();
            _mapper = mapper;
            _user = user;
            _authenticatedUser = authenticatedUser;
        }
    }
}
