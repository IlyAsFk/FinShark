using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
    private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepository = commentRepo;   
        }
    [HttpGet]
    public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var CommentDto = comments.Select(c => c.ToCommentDto());
            return Ok(CommentDto);
        }   
    }
}