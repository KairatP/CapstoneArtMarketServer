using AutoMapper;
using Capstone.Application.Exceptions;
using Capstone.Application.Interfaces;
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
    public class CreateAccountCommand : IRequest<Response<bool>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response<bool>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public CreateAccountCommandHandler(UserManager<User> userManager, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Response<bool>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //await _emailService.SendAsync(new DTOs.Email.EmailRequest { To = "yersa.kz@gmail.com", Body ="1212", Subject = "Verification Code" });

                var account = _mapper.Map<User>(request);
                account.UserName = request.Email;

                var result = await _userManager.CreateAsync(account, request.Password);
                if (!result.Succeeded)

                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.Code}, {error.Description}");
                    }

                    throw new ApiException("Error happened during the registration");
                }

                // emailService.Send(code)

            }
            catch (Exception)
            {

                throw;
            }

            return new Response<bool>(true);
        }
    }
}
