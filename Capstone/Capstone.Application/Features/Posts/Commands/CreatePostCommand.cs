using AutoMapper;
using Capstone.Application.Features.Posts.Queries;
using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Wrappers;
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
    public class CreatePostCommand : IRequest<Response<PostDto>>
    {
        public int Price { get; set; }
        public string Description { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Color { get; set; }
        public string Pano { get; set; }

        public List<IFormFile> Pictures { get; set; } = new List<IFormFile>();
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Response<PostDto>>
    {
        private readonly IPost _postService;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPost postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<Response<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.Add(request);
            var result = _mapper.Map<PostDto>(post);

            return new Response<PostDto>(result);
        }
    }
}
