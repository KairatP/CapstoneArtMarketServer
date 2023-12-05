using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Wrappers;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Posts.Commands
{
    public class UpdatePostCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Color { get; set; }
        public string Pano { get; set; }

        public List<IFormFile> Pictures { get; set; }
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Response<bool>>
    {
        private readonly IPost _postService;

        public UpdatePostCommandHandler(IPost postService)
        {
            _postService = postService;
        }

        public async Task<Response<bool>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            await _postService.Update(request);

            return new Response<bool>(true);
        }
    }
}
