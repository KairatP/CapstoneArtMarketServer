using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Interfaces
{
    public interface IAuthenticatedUser
    {
        string UserName { get; }
        Guid UserId { get; }
    }
}
