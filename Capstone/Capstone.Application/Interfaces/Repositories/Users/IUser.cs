using Capstone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Interfaces.Repositories.Users
{
    public interface IUser : IGenericRepositoryAsync<User>
    {
        Task<User> GetByUserName(string username);
    }
}
