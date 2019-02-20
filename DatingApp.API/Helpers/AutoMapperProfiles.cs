using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Extensions;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>().ForMember(destination => destination.PhotoUrl,
                    options => { options.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url); })
                .ForMember(destination => destination.Age,
                    options =>
                    {
                        options.ResolveUsing(date => date.DateOfBirth.CalculateAge());

                    });

            CreateMap<User, UserForDetailedDto>().ForMember(destination => destination.PhotoUrl,
                options =>
                {
                    options.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url);

                })
                .ForMember(destination => destination.Age,
                    options =>
                    {
                        options.ResolveUsing(date => date.DateOfBirth.CalculateAge());

                    });

            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}
