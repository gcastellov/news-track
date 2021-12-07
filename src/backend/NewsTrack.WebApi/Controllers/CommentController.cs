using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly ICommentRepository _commentRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public CommentController(
            ICommentService commentService,
            ICommentRepository commentRepository,
            IIdentityHelper identityHelper,
            IMapper mapper)
        {
            _commentService = commentService;
            _commentRepository = commentRepository;
            _identityHelper = identityHelper;
            _mapper = mapper;
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> Get(Guid commentId)
        {
            return await Execute(async () =>
            {
                var comment = await _commentRepository.Get(commentId);
                return _mapper.Map<CommentDto>(comment);
            });
        }

        [HttpGet("{commentId}/replies")]
        public async Task<IActionResult> GetReplies(Guid commentId, [FromQuery] uint page, [FromQuery] uint count)
        {
            if (count == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var comments = await _commentRepository.GetReplies(commentId, (int)page, (int)count);
                return _mapper.Map<IEnumerable<CommentDto>>(comments);
            });
        }

        [HttpGet("news/{draftId}")]
        public async Task<IActionResult> GetByDraft(Guid draftId, [FromQuery] uint page, [FromQuery] uint count)
        {
            if (count == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var comments = await _commentRepository.GetByDraftId(draftId, (int)page, (int)count);
                return _mapper.Map<IEnumerable<CommentDto>>(comments);
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            return await Execute(async () => 
            {
                var user = new User
                {
                    Username = _identityHelper.Username,
                    Id = _identityHelper.Id
                };

                var comment = Comment.Create(
                    commentDto.DraftId,
                    commentDto.Content,
                    user);

                comment.ReplyingTo = commentDto.ReplyingTo;

                await _commentService.Save(comment);
                return _mapper.Map<CommentDto>(comment);
            });
        }
    }
}
