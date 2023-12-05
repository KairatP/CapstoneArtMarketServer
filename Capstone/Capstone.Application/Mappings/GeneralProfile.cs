using AutoMapper;
using Capstone.Application.Features.Accounts.Commands;
using Capstone.Application.Features.Accounts.Queries.GetProfile;
using Capstone.Application.Features.Posts.Commands;
using Capstone.Application.Features.Posts.Queries;
using Capstone.Domain.Entities;
using System.Linq;

namespace Capstone.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            AccountMapping();
            PostMapping();
        }

        public void AccountMapping()
        {
            CreateMap<CreateAccountCommand, User>();

            CreateMap<User, GetProfileDTO>()
                .ForMember(dest => dest.Id, src => src.MapFrom(o => o.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(o => o.Name))
                .ForMember(dest => dest.Email, src => src.MapFrom(o => o.Email))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(o => o.PhoneNumber))
                .ForMember(dest => dest.Country, src => src.MapFrom(o => o.Country != null ? o.Country.Name : string.Empty))
                .ForMember(dest => dest.City, src => src.MapFrom(o => o.City != null ? o.City.Name : string.Empty))
                .ForMember(dest => dest.AvaUrl, src => src.MapFrom(o => o.Avatar != null ? o.Avatar.Path : string.Empty));
        }

        public void PostMapping()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(o => o.Id))
               .ForMember(dest => dest.Price, src => src.MapFrom(o => o.Price))
               .ForMember(dest => dest.Description, src => src.MapFrom(o => o.Description))
               .ForMember(dest => dest.Height, src => src.MapFrom(o => o.Height))
               .ForMember(dest => dest.Width, src => src.MapFrom(o => o.Width))
               .ForMember(dest => dest.Color, src => src.MapFrom(o => o.Color))
               .ForMember(dest => dest.Pano, src => src.MapFrom(o => o.Pano))
               .ForMember(dest => dest.Urls, src => src.MapFrom(o => o.Pictures.Select(y => y.File.Path)))
               .ForMember(dest => dest.Author, src => src.MapFrom(o => o.Author));
        }
    }
}