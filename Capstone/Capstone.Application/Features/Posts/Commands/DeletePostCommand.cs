using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Posts.Commands
{
    public class DeletePostCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Response<bool>>
    {
        private readonly IPost _postService;

        public DeletePostCommandHandler(IPost postService)
        {
            _postService = postService;
        }

        public async Task<Response<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postService.GetByIdAsync(request.Id);
                await _postService.DeleteAsync(post);
            }
            catch (Exception)
            {
                return new Response<bool>(false);
            }

            return new Response<bool>(true);
        }
    }
}
