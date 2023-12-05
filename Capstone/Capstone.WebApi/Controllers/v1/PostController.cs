using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Capstone.Application.Features.Posts.Commands;
using Capstone.Application.Wrappers;
using Capstone.Application.Features.Posts.Queries;
using Capstone.Domain.Entities;
using System.Collections.Generic;

namespace Capstone.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("/api/v{version:ApiVersion}/post")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class PostController : BaseApiController
    {
        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] CreatePostCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("update")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] UpdatePostCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("delete")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(DeletePostCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("list")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<PostDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromBody]GetPostsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("myPosts/list")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<PostDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyPostsList([FromBody]GetMyPostsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
