using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Posts.Queries
{
    public class GetPostsQuery : IRequest<PagedResponse<IEnumerable<PostDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SearchText { get; set; }
        public List<string> Countries { get; set; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PagedResponse<IEnumerable<PostDto>>>
    {
        private readonly IPost _postService;

        public GetPostsQueryHandler(IPost postService)
        {
            _postService = postService;
        }

        public async Task<PagedResponse<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetAllPosts(request);
        }
    }
}