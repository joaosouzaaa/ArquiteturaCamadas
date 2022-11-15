using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class TagService : BaseService<Tag>, ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository, IValidator<Tag> validator, INotificationHandler notification) 
                          : base(validator, notification)
        {
            _tagRepository = tagRepository;
        }

        public async Task<bool> AddAsync(TagSaveRequest tagSaveRequest)
        {
            var tag = tagSaveRequest.MapTo<TagSaveRequest, Tag>();

            if (!await ValidateAsync(tag))
                return false;

            return await _tagRepository.AddAsync(tag);
        }

        public async Task<bool> UpdateAsync(TagUpdateRequest tagUpdateRequest)
        {
            if (!await _tagRepository.HaveObjectInDbAsync(t => t.Id == tagUpdateRequest.Id))
                return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Tag"));

            var tag = tagUpdateRequest.MapTo<TagUpdateRequest, Tag>();

            if (!await ValidateAsync(tag))
                return false;

            return await _tagRepository.UpdateAsync(tag);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(!await _tagRepository.HaveObjectInDbAsync(t => t.Id == id))
                return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Tag"));

            return await _tagRepository.DeleteAsync(id);
        }

        public async Task<TagResponse> FindByIdAsync(int id)
        {
            var tag = await _tagRepository.FindByIdAsync(id);

            return tag.MapTo<Tag, TagResponse>();
        }

        public async Task<PageList<TagPostsResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams)
        {
            var tagsPageList = await _tagRepository.FindAllEntitiesWithPaginationAsync(pageParams, t => t.Include(t => t.Posts));

            return tagsPageList.MapTo<PageList<Tag>, PageList<TagPostsResponse>>();
        }

        public async Task<Tag> FindByIdAsyncAsNoTrackingReturnsDomainObject(int id) =>
            await _tagRepository.FindByIdAsync(id, null, false);

        public async Task<List<TagPostsResponse>> FindAllEntitiesAsync()
        {
            var tagsList = await _tagRepository.FindAllEntitiesAsync(t => t.Include(t => t.Posts));

            return tagsList.MapTo<List<Tag>, List<TagPostsResponse>>();
        }
    }
}
