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

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
    [HttpGet]
    public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
    
    [HttpPost]
    public async Task<IActionResult>Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById),new {id = stock.Id}, stock.ToStockDto());
        }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stock == null)
            {
                return NotFound();
            }
            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.Industry = stockDto.Industry;
            stock.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();
            return Ok(stock.ToStockDto());
        }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
