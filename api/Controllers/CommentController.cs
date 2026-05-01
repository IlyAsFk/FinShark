using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepo;   
        _stockRepository = stockRepo;
        _userManager = userManager;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comments = await _commentRepository.GetAllAsync(query);
        var CommentDto = comments.Select(c => c.ToCommentDto());
        return Ok(CommentDto);
    }   
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById( int id) // [FromRoute] is optional for simple types like int
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        var commentDto = comment.ToCommentDto();
        return Ok(commentDto);
    }
    [HttpPost("{stockId:int}")]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!await _stockRepository.stockExists(stockId))
        {
            return BadRequest($"Stock with id {stockId} does not exist.");
        }
        var username = User.GetUsername();
        var user = await _userManager.FindByNameAsync(username);
        var comment = commentDto.ToCommentFromCreateDto(stockId);
        comment.AppUserId = user.Id;
        await _commentRepository.CreateAsync(comment);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.UpdateAsync(id, commentDto.ToCommentFromUpdateDto());
        if (comment == null)
        {
            return NotFound("Comment not found.");
        }
        return Ok(comment.ToCommentDto());
    }
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.DeleteAsync(id);
        if (comment == null)
        {
            return NotFound("Comment not found.");
        }
        return NoContent();
    }
    }
}