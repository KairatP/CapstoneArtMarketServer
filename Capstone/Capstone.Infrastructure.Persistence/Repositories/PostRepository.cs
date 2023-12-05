using AutoMapper;
using Azure.Core;
using Capstone.Application.Features.Posts.Commands;
using Capstone.Application.Features.Posts.Queries;
using Capstone.Application.Interfaces;
using Capstone.Application.Interfaces.Repositories.Files;
using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Interfaces.Repositories.Users;
using Capstone.Application.Wrappers;
using Capstone.Domain.Entities;
using Capstone.Infrastructure.Persistence.Contexts;
using Capstone.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepositoryAsync<Post>, IPost
    {
        private readonly DbSet<Post> _posts;

        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        private readonly IUser _userService;
        private readonly IFile _fileService;
        private readonly IPostFile _postFileService;
        private readonly IAuthenticatedUser _authenticatedUser;

        public PostRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, IMapper mapper, IFile fileService, IPostFile postFileService, IUser userService, IAuthenticatedUser authenticatedUser) : base(dbContext)
        {
            _context = httpContextAccessor;
            _posts = dbContext.Set<Post>();
            _mapper = mapper;
            _fileService = fileService;
            _postFileService = postFileService;
            _userService = userService;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Post> Add(CreatePostCommand request)
        {
            var post = new Post();
            post.Price = request.Price;
            post.Description = request.Description;
            post.Width = request.Width;
            post.Height = request.Height;
            post.Color = request.Color;
            post.Pano = request.Pano;
            await SetCountryCity(post);
            await AddAsync(post);

            await SaveFiles(request.Pictures, post);

            return post;
        }

        public async Task<Post> Update(UpdatePostCommand request)
        {
            var post = await _posts
                .Include(x => x.Pictures)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (post != null)
            {
                post.Price = request.Price;
                post.Description = request.Description;
                post.Width = request.Width;
                post.Height = request.Height;
                post.Color = request.Color;
                post.Pano = request.Pano;
                await SetCountryCity(post);
                await UpdateAsync(post);

                await SaveFiles(request.Pictures, post);
            }

            return post;
        }

        public async Task<PagedResponse<IEnumerable<PostDto>>> GetAllPosts(GetPostsQuery request)
        {
            var searchBy = request.SearchText != null ? request.SearchText : string.Empty;
            var sortOrder = request.Sorting;

            var data = _posts
                .Include(x => x.Author).ThenInclude(y => y.Avatar)
                .Include(x => x.Author).ThenInclude(y => y.Country)
                .Include(x => x.Author).ThenInclude(y => y.City)
                .Include(x => x.Pictures).ThenInclude(y => y.File)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchBy))
            {
                data = data.Where(r => (!string.IsNullOrEmpty(r.Description) && r.Description.ToUpper().Contains(searchBy.ToUpper())) ||
                                       (!string.IsNullOrEmpty(r.Color) && r.Color.ToUpper().Contains(searchBy.ToUpper())) ||
                                       (!string.IsNullOrEmpty(r.Pano) && r.Pano.ToUpper().Contains(searchBy.ToUpper())));
            }

            if (request.Countries != null)
            {
                if (request.Countries.Any())
                {
                    data = data.Where(x => x.Author.Country != null && !string.IsNullOrEmpty(x.Author.Country.Name) && request.Countries.Contains(x.Author.Country.Name));
                }
            }

            var filteredResultsCount = await data.CountAsync();
            var totalResultsCount = await _posts.CountAsync();
            var filteredData = data
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<IEnumerable<PostDto>>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = _mapper.Map<IEnumerable<PostDto>>(filteredData)
            };
        }

        public async Task<PagedResponse<IEnumerable<PostDto>>> GetMyPosts(GetMyPostsQuery request)
        {
            var searchBy = request.SearchText != null ? request.SearchText : string.Empty;
            var sortOrder = request.Sorting;

            var data = _posts
                        .Include(x => x.Author).ThenInclude(y => y.Avatar)
                        .Include(x => x.Author).ThenInclude(y => y.Country)
                        .Include(x => x.Author).ThenInclude(y => y.City)
                        .Include(x => x.Pictures).ThenInclude(y => y.File)
                        .Where(x => x.CreatedBy == _authenticatedUser.UserId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchBy))
            {
                data = data.Where(r => (!string.IsNullOrEmpty(r.Description) && r.Description.ToUpper().Contains(searchBy.ToUpper())) ||
                                       (!string.IsNullOrEmpty(r.Color) && r.Color.ToUpper().Contains(searchBy.ToUpper())) ||
                                       (!string.IsNullOrEmpty(r.Pano) && r.Pano.ToUpper().Contains(searchBy.ToUpper())));
            }

            if (request.Countries != null)
            {
                if (request.Countries.Any())
                {
                    data = data.Where(x => x.Author.Country != null && !string.IsNullOrEmpty(x.Author.Country.Name) && request.Countries.Contains(x.Author.Country.Name));
                }
            }

            var filteredResultsCount = await data.CountAsync();
            var totalResultsCount = await _posts.CountAsync();
            var filteredData = data
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<IEnumerable<PostDto>>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = _mapper.Map<IEnumerable<PostDto>>(filteredData)
            };
        }

        private async Task<Post> SetCountryCity(Post post)
        {
            var user = await _userService.GetByUserName(_authenticatedUser.UserName);

            post.CountryId = user.CountryId;
            post.CityId = user.CityId;

            return post;
        }

        private async Task RemoveFiles(List<PostFile> files)
        {
            foreach (var file in files)
            {
               await _postFileService.DeleteAsync(file);
            }
        }

        private async Task SaveFiles(List<IFormFile> files, Post post)
        {
            if (files != null)
            {
                if (files.Any())
                {
                    await RemoveFiles(post.Pictures.ToList());

                    var baseUri = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";

                    string rootFolder = "wwwroot";
                    string folderPath = $"images\\{post.Id}";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), rootFolder, folderPath);

                    //create folder if not exist
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (var file in files)
                    {
                        //get file extension
                        FileInfo fileInfo = new FileInfo(file.FileName);

                        string fileNameWithPath = Path.Combine(path, file.FileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        var fileEntity = new Domain.Entities.File { Name = file.FileName, Path = Path.Combine(baseUri, folderPath, file.FileName) };
                        await _fileService.AddAsync(fileEntity);

                        var postFile = new PostFile { PostId = post.Id, FileId = fileEntity.Id };
                        await _postFileService.AddAsync(postFile);
                    }
                }
            }
        }
    }
}
