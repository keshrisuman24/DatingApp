using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
     public AutoMapperProfiles()
     {
         CreateMap<User,UserListDtos>()
         .ForMember(dest=>dest.PhotoUrl,opt => {
             opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url);
         })
         .ForMember(destinationMember=>destinationMember.Age,opt=>
         {
           opt.ResolveUsing(d=>d.DateOfBirth.CalculateAge()); 
         });
         CreateMap<User,UserForDetailDto>().ForMember(dest=>dest.PhotoUrl,opt => {
             opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url);
         }).ForMember(destinationMember=>destinationMember.Age,opt=>
         {
           opt.ResolveUsing(d=>d.DateOfBirth.CalculateAge()); 
         });
         CreateMap<Photo,PhotoForDetailDto>();
     }   
    }
}