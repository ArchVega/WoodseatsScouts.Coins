using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    [PrimaryKey(nameof(MemberId), nameof(CountryId))]
    public class MemberCountryVote
    {
        public int MemberId { get; set; }

        public int CountryId { get; set; }

        public DateTime VotedAt { get; set; }

        public Member Member { get; set; }
        
        public Country Country { get; set; }
    }
}
