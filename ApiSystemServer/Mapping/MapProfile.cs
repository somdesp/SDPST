using AutoMapper;
using Domain.Entity;
using Domain.ViewModel;

namespace ApiSystemServer.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User,UserViewModel > (MemberList.Destination);
           // CreateMap<UserViewModel, User>().ReverseMap();

        }
    }
}
