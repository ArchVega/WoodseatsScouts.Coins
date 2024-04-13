using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CountriesController(IAppDbContext appDbContext) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCountries()
    {
        return Ok(appDbContext.Countries!.ToList());
    }
}
