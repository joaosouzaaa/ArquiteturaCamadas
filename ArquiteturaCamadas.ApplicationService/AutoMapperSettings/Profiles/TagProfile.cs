using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagSaveRequest, Tag>();

            CreateMap<Tag, TagResponse>()
                .ReverseMap();

            CreateMap<Tag, TagPostsResponse>()
                .ForMember(tr => tr.Posts, map => map.MapFrom(t => t.Posts));

            CreateMap<TagUpdateRequest, Tag>();

            CreateMap<PageList<Tag>, PageList<TagPostsResponse>>();
        }
    }
}
