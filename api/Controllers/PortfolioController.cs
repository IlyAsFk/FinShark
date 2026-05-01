using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager, 
        IStockRepository stockRepository,
        IPortfolioRepository portfolioRepository)
        {
         _userManager = userManager;
         _stockRepository = stockRepository;
         _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var stocks = await _portfolioRepository.GetUserPortfolio(user);
            return Ok(stocks);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var stockItem = await _stockRepository.GetStockBySymbolAsync(symbol);
            if(stockItem == null)
            {
                return NotFound("Stock not found");
            }
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            if(userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already in portfolio");
            }
            var UserStock = await _portfolioRepository.createAsync(new Portfolio
            {
                AppUserId = user.Id,
                StockId = stockItem.Id
            });
            if(UserStock == null)
            {
                return StatusCode(500, "Failed to add stock to portfolio");
            }
            else
            {
                return Created();
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveFromPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var stockItem = await _stockRepository.GetStockBySymbolAsync(symbol);
            if(stockItem == null)
            {
                return NotFound("Stock not found");
            }
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            var FilteredStock = userPortfolio.FirstOrDefault(s => s.Symbol.ToLower() == symbol.ToLower());
            if(FilteredStock == null)
            {
                return NotFound("Stock not in portfolio");
            }
            var result = await _portfolioRepository.DeleteAsync(FilteredStock);
            if(result == null)
            {
                return StatusCode(500, "Failed to remove stock from portfolio");
            }
            else
            {
                return Ok();
            }
        }
    }
}