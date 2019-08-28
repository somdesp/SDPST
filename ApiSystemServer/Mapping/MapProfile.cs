using ApiSystemServer.ViewModel;
using AutoMapper;
using Domain.Entity;

namespace ApiSystemServer.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserViewModel, User>(MemberList.Source);
            CreateMap<UserViewModel, User>().ReverseMap();

        }
    }
}
