using AutoMapper;
using Capstone.Application.Interfaces;
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

namespace Capstone.Application.Features.Accounts.Queries.GetProfile
{
    public class GetProfileCommand : IRequest<Response<GetProfileDTO>>
    {
        public class GetProfileCommandHandler : IRequestHandler<GetProfileCommand, Response<GetProfileDTO>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;
            private readonly IUser _userRespository;
            private readonly IAuthenticatedUser _authenticatedUser;

            public GetProfileCommandHandler(UserManager<User> userManager, IMapper mapper, IUser userRespository, IAuthenticatedUser authenticatedUser)
            {
                _userManager = userManager;
                _mapper = mapper;
                _userRespository = userRespository;
                _authenticatedUser = authenticatedUser;
            }

            public async Task<Response<GetProfileDTO>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRespository.GetByUserName(_authenticatedUser.UserName);

                var profileDto = new GetProfileDTO();
                profileDto.Id = user.Id;
                profileDto.Name = user.Name ?? string.Empty;
                profileDto.Email = user.Email ?? string.Empty;
                profileDto.PhoneNumber = user.PhoneNumber ?? string.Empty;
                profileDto.Country = user.Country?.Name ?? string.Empty;
                profileDto.City = user.City?.Name ?? string.Empty;
                profileDto.AvaUrl = user.Avatar?.Path ?? string.Empty;

                return new Response<GetProfileDTO>(profileDto);
            }
        }
    }
}
