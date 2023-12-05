﻿using Capstone.Application.Exceptions;
using Capstone.Application.Interfaces.Repositories.Users;
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
    public class ChangePasswordCommand : IRequest<Response<bool>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Response<bool>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUser _userRespository;

        public ChangePasswordCommandHandler(UserManager<User> userManager, IUser userRespository)
        {
            _userManager = userManager;
            _userRespository = userRespository;
        }

        public async Task<Response<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetByUserName(request.Email);
            if (user == null)
                throw new ApiException("User is not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (result == null)
                throw new ApiException("Error");
            //await _userManager.ChangePasswordAsync(user, user.PasswordHash, request.Password);

            return new Response<bool>(true);
        }
    }
}
