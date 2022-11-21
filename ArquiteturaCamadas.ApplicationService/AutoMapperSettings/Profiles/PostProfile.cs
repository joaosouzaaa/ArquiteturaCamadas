using ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Converters;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostSaveRequest, Post>()
                .ForMember(p => p.ImageBytes, map => map.ConvertUsing(new FormFileToBytes(), ps => ps.Image));
            
            CreateMap<Post, PostResponse>();

            CreateMap<Post, PostTagsResponse>()
                .ForMember(pr => pr.Tags, map => map.MapFrom(p => p.Tags));

            CreateMap<PageList<Post>, PageList<PostTagsResponse>>();
        }
    }
}
