using System;
using System.Threading.Tasks;
using AutoMapper;
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

        [HttpGet("{messageId}")]
        public Task<IActionResult> GetMessage(Guid messageId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{messageId}/replies")]
        public Task<IActionResult> GetReplies(Guid messageId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("news/{draftId}")]
        public Task<IActionResult> GetCommentsByDraft(Guid draftId)
        {
            throw new NotImplementedException();
        }

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
