using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class PostService : BaseService<Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagService _tagService;

        public PostService(IPostRepository postRepository, ITagService tagService,
                           IValidator<Post> validator, INotificationHandler notification)
                           : base(validator, notification)
        {
            _postRepository = postRepository;
            _tagService = tagService;
        }

        public async Task<bool> AddAsync(PostSaveRequest postSaveRequest)
        {
            if (postSaveRequest.Image is not null)
                if (!postSaveRequest.Image.FileName.ValidateFileFormat())
                    return _notification.AddDomainNotification("Not Found", EMessage.InvalidImageFormat.Description());

            var post = postSaveRequest.MapTo<PostSaveRequest, Post>();

            if (!await ValidateAsync(post))
                return false;

            post.Tags = new List<Tag>();

            if(postSaveRequest.TagsIds != null)
            {
                foreach (var tagId in postSaveRequest.TagsIds)
                {
                    var tag = await _tagService.FindByIdAsyncAsNoTrackingReturnsDomainObject(tagId);

                    if (tag is null)
                        return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Tag"));

                    post.Tags.Add(tag);
                }
            }

            return await _postRepository.AddAsync(post);
        }

        public async Task<bool> UpdateUnreapeatTagsSearchAsync(PostUpdateRequest postUpdateRequest)
        {
            var post = await _postRepository.FindByIdAsync(postUpdateRequest.Id, p => p.Include(p => p.Tags), false);

            if (post is null)
                return _notification.AddDomainNotification("Not found", EMessage.NotFound.Description().FormatTo("Post"));

            if (postUpdateRequest.Image is not null)
            {
                if (!postUpdateRequest.Image.FileName.ValidateFileFormat())
                    return _notification.AddDomainNotification("Image Format", EMessage.InvalidImageFormat.Description());

                post.ImageBytes = GetImageBytes(postUpdateRequest.Image);
            }
            else
                post.ImageBytes = null;

            post.Message = postUpdateRequest.Message;

            if (!await ValidateAsync(post))
                return false;

            if (postUpdateRequest.TagsIds != null)
            {
                foreach (var tag in post.Tags.ToList())
                {
                    if (!postUpdateRequest.TagsIds.Contains(tag.Id))
                        post.Tags.Remove(tag);
                }
            }

            var removeUpdateResult = await _postRepository.UpdateAsync(post);

            if(postUpdateRequest.TagsIds != null)
            {
                var tagsIds = post.Tags.Select(t => t.Id);
                var comparableIds = postUpdateRequest.TagsIds.Except(tagsIds).ToList();

                if (removeUpdateResult is true && comparableIds.Any())
                    return await AddNewTagsAsync(postUpdateRequest, post, comparableIds);
            }

            return removeUpdateResult;
        }

        public async Task<bool> UpdateManyToManyAsync(PostUpdateRequest postUpdateRequest)
        {
            var post = await _postRepository.FindByIdAsync(postUpdateRequest.Id, p => p.Include(p => p.Tags), false);

            if (post is null)
                return _notification.AddDomainNotification("Not found", EMessage.NotFound.Description().FormatTo("Post"));

            if (postUpdateRequest.Image is not null)
            {
                if (!postUpdateRequest.Image.FileName.ValidateFileFormat())
                    return _notification.AddDomainNotification("Image Format", EMessage.InvalidImageFormat.Description());

                post.ImageBytes = GetImageBytes(postUpdateRequest.Image);
            }
            else
                post.ImageBytes = null;

            post.Message = postUpdateRequest.Message;

            if (!await ValidateAsync(post))
                return false;

            post.Tags.Clear();

            if(postUpdateRequest.TagsIds != null)
            {
                foreach (var tagId in postUpdateRequest.TagsIds)
                {
                    var tag = await _tagService.FindByIdAsyncAsNoTrackingReturnsDomainObject(tagId);

                    if (tag is null)
                        return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Tag"));

                    post.Tags.Add(tag);
                }
            }

            return await _postRepository.UpdateAsync(post);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _postRepository.HaveObjectInDbAsync(p => p.Id == id))
                return _notification.AddDomainNotification("Not found", EMessage.NotFound.Description().FormatTo("Post"));

            return await _postRepository.DeleteAsync(id);
        }

        public async Task<PageList<PostTagsResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams)
        {
            var postsPageList = await _postRepository.FindAllEntitiesWithPaginationAsync(pageParams, p => p.Include(p => p.Tags));

            return postsPageList.MapTo<PageList<Post>, PageList<PostTagsResponse>>();
        }

        public async Task<List<PostTagsResponse>> FindAllEntitiesAsync()
        {
            var postsList = await _postRepository.FindAllEntitiesAsync(p => p.Include(p => p.Tags));

            return postsList.MapTo<List<Post>, List<PostTagsResponse>>();
        }

        public async Task<PostTagsResponse> FindByIdAsync(int id)
        {
            var post = await _postRepository.FindByIdAsync(id, p => p.Include(p => p.Tags));

            return post.MapTo<Post, PostTagsResponse>();
        }

        private byte[] GetImageBytes(IFormFile image)
        {
            using (var stream = new MemoryStream())
            {
                image.CopyTo(stream);

                return stream.ToArray();
            }
        }

        private async Task<bool> AddNewTagsAsync(PostUpdateRequest postUpdateRequest, Post post, List<int> differentIds)
        {
            foreach (var tagId in differentIds)
            {
                var tag = await _tagService.FindByIdAsyncAsNoTrackingReturnsDomainObject(tagId);

                if (tag is null)
                    return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Tag"));

                post.Tags.Add(tag);
            }

            var mappedPost = new Post()
            {
                Id = postUpdateRequest.Id,
                Message = postUpdateRequest.Message,
                Tags = post.Tags
            };

            return await _postRepository.DetachEntityAndSaveChangesAsync(mappedPost);
        }
    }
}
