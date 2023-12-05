using Capstone.Application.Features.Posts.Commands;
using Capstone.Application.Features.Posts.Queries;
using Capstone.Application.Wrappers;
using Capstone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Interfaces.Repositories.Posts
{
    public interface IPost : IGenericRepositoryAsync<Post>
    {
        Task<PagedResponse<IEnumerable<PostDto>>> GetAllPosts(GetPostsQuery request);
        Task<PagedResponse<IEnumerable<PostDto>>> GetMyPosts(GetMyPostsQuery request);

        Task<Post> Add(CreatePostCommand request);
        Task<Post> Update(UpdatePostCommand request);
    }
}
