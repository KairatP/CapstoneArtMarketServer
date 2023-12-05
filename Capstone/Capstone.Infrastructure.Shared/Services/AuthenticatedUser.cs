using Capstone.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Infrastructure.Shared.Services
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
        {
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

            string userId = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userId)) UserId = Guid.Parse(userId);
        }

        public string UserName { get; set; }
        public Guid UserId { get; set; }
    }
}
