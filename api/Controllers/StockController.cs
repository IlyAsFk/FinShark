using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Stock;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Helpers;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepository;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepository = stockRepo;   
            _context = context;
        }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            var stocks = await _stockRepository.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
    
    [HttpPost]
    public async Task<IActionResult>Create([FromBody] CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stock);
            return CreatedAtAction(nameof(GetById),new {id = stock.Id}, stock.ToStockDto());
        }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepository.UpdateAsync(id, stockDto);
            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepository.DeleteAsync(id);
            if(stock == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
