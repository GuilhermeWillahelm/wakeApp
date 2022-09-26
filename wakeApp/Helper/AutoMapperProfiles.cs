using AutoMapper;
using wakeApp.Dtos;
using wakeApp.Models;

namespace wakeApp.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PostVideo, PostVideoDto>().ReverseMap();
        }
    }
}
