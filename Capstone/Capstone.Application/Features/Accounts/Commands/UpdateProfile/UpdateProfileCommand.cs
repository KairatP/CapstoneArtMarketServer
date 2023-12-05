using Capstone.Application.Interfaces;
using Capstone.Application.Interfaces.Repositories.Dictionaries;
using Capstone.Application.Interfaces.Repositories.Files;
using Capstone.Application.Interfaces.Repositories.Posts;
using Capstone.Application.Wrappers;
using Capstone.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Accounts.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public IFormFile Avatar { get; set; }

        public class GetProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Response<bool>>
        {
            private readonly IHttpContextAccessor _context;
            private readonly UserManager<User> _userManager;
            private readonly ICountry _country;
            private readonly ICity _city;
            private readonly IFile _fileService;

            private readonly IAuthenticatedUser _authenticatedUser;

            public GetProfileCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ICountry country, ICity city, IFile fileService, IAuthenticatedUser authenticatedUser)
            {
                _context = httpContextAccessor;
                _userManager = userManager;
                _country = country;
                _city = city;
                _fileService = fileService;
                _authenticatedUser = authenticatedUser;
            }

            public async Task<Response<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_authenticatedUser.UserName);

                Country country = null;
                if (!string.IsNullOrEmpty(request.Country))
                {
                    country = await _country.AddAsync(new Country { Name = request.Country });
                }

                City city = null;
                if (!string.IsNullOrEmpty(request.City))
                {
                    city = await _city.AddAsync(new City { Name = request.City });
                }

                if (!string.IsNullOrEmpty(request.Name))
                {
                    user.Name = request.Name;
                }

                if (!string.IsNullOrEmpty(request.Name))
                {
                    user.PhoneNumber = request.PhoneNumber;
                }
                    
                user.CountryId = country != null ? country.Id : null;
                user.CityId = city != null ? city.Id : null;

                if( request.Avatar != null )
                {
                    var file = await SaveAvatar(request.Avatar, user);
                    user.AvatarId = file.Id;
                }
                
                await _userManager.UpdateAsync(user);

                return new Response<bool>(true);
            }


            private async Task<Domain.Entities.File> SaveAvatar(IFormFile file, User user)
            {
                var baseUri = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";

                string rootFolder = "wwwroot";
                string folderPath = $"images\\{user.UserName}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), rootFolder, folderPath);

                //create folder if not exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //get file extension
                FileInfo fileInfo = new FileInfo(file.FileName);

                string fileNameWithPath = Path.Combine(path, file.FileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var fileEntity = new Domain.Entities.File { Name = file.FileName, Path = Path.Combine(baseUri, folderPath, file.FileName) };
                await _fileService.AddAsync(fileEntity);

                return fileEntity;
            }
        }
    }
}
