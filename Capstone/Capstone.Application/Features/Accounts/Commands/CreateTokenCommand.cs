using AutoMapper;
using Capstone.Application.DTOs.Account;
using Capstone.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Accounts.Commands
{
    public class CreateTokenCommand : TokenRequest, IRequest<TokenResponse>
    {
        public class CommandHandler : IRequestHandler<CreateTokenCommand, TokenResponse>
        {
            private readonly ITokenService _tokenService;
            private readonly HttpContext _httpContext;

            private readonly IMapper _mapper;

            public CommandHandler(
                ITokenService tokenService,
                                  IMapper mapper,
                                  IHttpContextAccessor httpContextAccessor
                )
            {
                this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

            }

            public async Task<TokenResponse> Handle(CreateTokenCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    string ipAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                    TokenResponse tokenResponse = await _tokenService.Authenticate(command, ipAddress);
                    if (tokenResponse == null)
                    {
                        //throw new InvalidCredentialsException();
                    }

                    return tokenResponse;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
