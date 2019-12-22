using System.Linq;
using AutoMapper;
using CoreAPI.Dto;
using CoreAPI.Models;

namespace CoreAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //set for age and dob property as they can't match each other
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.Age, opt =>
                     {
                         opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                     })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.Age, opt =>
                     {
                         opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                     })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                });

             CreateMap<Photo, PhotosForDetailedDto>();
             CreateMap<UserForUpdateDto,User>();
        }
    }
}