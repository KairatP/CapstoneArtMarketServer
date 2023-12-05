using AutoMapper;
using Capstone.Application.Interfaces.Repositories.Users;
using Capstone.Application.Interfaces;
using Capstone.Infrastructure.Persistence.Contexts;
using Capstone.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Domain.Entities;
using Capstone.Application.Interfaces.Repositories.Posts;

namespace Capstone.Infrastructure.Persistence.Repositories
{
    public class PostFileRepository : GenericRepositoryAsync<PostFile>, IPostFile
    {
        private readonly DbSet<PostFile> _postFiles;

        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IAuthenticatedUser _authenticatedUser;

        public PostFileRepository(ApplicationDbContext dbContext, IMapper mapper, IUser user, IAuthenticatedUser authenticatedUser) : base(dbContext)
        {
            _postFiles = dbContext.Set<PostFile>();
            _mapper = mapper;
            _user = user;
            _authenticatedUser = authenticatedUser;
        }
    }
}
