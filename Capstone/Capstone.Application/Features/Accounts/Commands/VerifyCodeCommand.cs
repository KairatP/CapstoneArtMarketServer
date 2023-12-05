using AutoMapper;
using Capstone.Application.Exceptions;
using Capstone.Application.Wrappers;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Accounts.Commands
{
    public class VerifyCodeCommand : IRequest<Response<bool>>
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }

    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeCommand, Response<bool>>
    {
        private readonly UserManager<User> _userManager;

        public VerifyCodeCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<bool>> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User is not found");
            }

            if (user.Code != request.Code)
            {
                throw new ApiException("Verification code is incorrect");
            }

            user.Code = request.Code;
            await _userManager.UpdateAsync(user);

            return new Response<bool>(true);
        }
    }
}
