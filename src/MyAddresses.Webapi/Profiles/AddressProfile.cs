using AutoMapper;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain.Models;

namespace MyAddresses.Webapi.Profiles
{
    public class AddressProfile: Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressModel>()
                .ReverseMap()
                .ForMember(m => m.Id, s => s.Ignore())
                .ForMember(m => m.User, s => s.Ignore())
                .ForMember(m => m.UserId, s => s.Ignore());
        }
    }
}