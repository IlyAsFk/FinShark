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
    private readonly IStockRepository _stockRepository;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepository = commentRepo;   
        _stockRepository = stockRepo;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();
        var CommentDto = comments.Select(c => c.ToCommentDto());
        return Ok(CommentDto);
    }   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById( int id) // [FromRoute] is optional for simple types like int
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        var commentDto = comment.ToCommentDto();
        return Ok(commentDto);
    }
    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
        if (!await _stockRepository.stockExists(stockId))
        {
            return BadRequest($"Stock with id {stockId} does not exist.");
        }
        var comment = commentDto.ToCommentFromCreateDto(stockId);
        await _commentRepository.CreateAsync(comment);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
    {
        var comment = await _commentRepository.UpdateAsync(id, commentDto.ToCommentFromUpdateDto());
        if (comment == null)
        {
            return NotFound("Comment not found.");
        }
        return Ok(comment.ToCommentDto());
    }
}
}