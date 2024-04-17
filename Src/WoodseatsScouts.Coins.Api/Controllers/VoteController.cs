using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VoteController(
    IAppDbContext appDbContext,
    SystemDateTimeProvider systemDateTimeProvider) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public IActionResult GetCountries(string code, string memberCode)
    {
        return Ok();
    }

    [HttpPut]
    [Route("{memberId:int}/RegisterVote")]
    public IActionResult RegisterVoteForMember(int memberId, [FromQuery] int countryId)
    {
        var memberCountryVote = new MemberCountryVote
        {
            MemberId = memberId,
            CountryId = countryId,
            VotedAt = systemDateTimeProvider.Now
        };

        appDbContext.MemberCountryVotes!.Add(memberCountryVote);
        appDbContext.SaveChanges();

        var persistedMemberCountryVote = appDbContext.MemberCountryVotes
            .Include(x => x.Member)
            .Include(x => x.Country)
            .Single(x => x.MemberId == memberId && x.CountryId == countryId);

        var voteResultViewModel = new VoteResultViewModel
        {
            MemberName = persistedMemberCountryVote.Member.FirstName,
            CountryName = persistedMemberCountryVote.Country.Name
        };

        return Ok(voteResultViewModel);
    }

    [HttpGet]
    [Route("Results")]
    public ActionResult VoteResults()
    {
        var memberCountryVotes = appDbContext.MemberCountryVotes!
            .Include(x => x.Country)
            .ToList();


        var countriesGroupedByVoteCount = memberCountryVotes
            .GroupBy(x => x.CountryId)
            .Select(x => new
            {
                CountryId = x.Key,
                CountryName = x.First().Country.Name,
                TotalVotes = x.Count()
            })
            .ToList();


        var allCountriesWithVotes = new List<CountryVoteViewModel>();

        var countries = appDbContext.Countries!.ToList();

        foreach (var country in countries)
        {
            var countryVoteViewModel = new CountryVoteViewModel
            {
                CountryId = country.Id,
                CountryName = country.Name
            };

            var hasVotes = countriesGroupedByVoteCount.SingleOrDefault(x => x.CountryId == country.Id);

            countryVoteViewModel.TotalVotes = hasVotes?.TotalVotes ?? 0;  
            
            allCountriesWithVotes.Add(countryVoteViewModel);
        }

        return Ok(
            allCountriesWithVotes
                .OrderByDescending(x => x.TotalVotes)
                .ThenBy(x => x.CountryName)
                .ToList());
    }
}

public class CountryVoteViewModel
{
    public int TotalVotes { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}

public class VoteResultViewModel
{
    public string MemberName { get; set; }
    public string CountryName { get; set; }
}