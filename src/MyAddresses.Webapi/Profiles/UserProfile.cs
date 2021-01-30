using AutoMapper;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain.Models;

namespace MyAddresses.Webapi.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(m => m.Username, s => s.MapFrom(f => f.Username))
                .ReverseMap()
                .ForMember(m => m.Id, s => s.Ignore())
                .ForMember(m => m.Username, s => s.MapFrom(f => f.Username));
        }
    }
}