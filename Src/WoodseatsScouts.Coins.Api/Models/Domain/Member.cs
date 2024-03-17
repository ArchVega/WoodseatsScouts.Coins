using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    // dotcover disable
    /// <summary>
    /// Member photos are stored on disk in the ClientApp/public/member-images directory and the files must be a jpg file whose name
    /// matches the Number field
    /// </summary>
    public class Member
    {
        public int Id { get; set; }

        /// <summary>
        /// Unlike Coins which has a static value for Code (as Coins are generated before going live), Members require a computed value in case
        /// members are added to the application after it is live.  
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Code { get; set; }
        
        public int Number { get; set; }
        
        public string FirstName { get; set; }

        public string? LastName { get; set; }
        
        public int TroopId { get; init; }
        
        public Troop Troop { get; init; }
        
        public string SectionId { get; init; }
        
        public Section Section { get; set; }
        
        public string? Clue1State { get; init; }

        public string? Clue2State { get; init; }

        public string? Clue3State { get; init; }
        
        public bool IsDayVisitor { get; set; }
        
        public bool HasImage { get; set; }
        
        public List<ScavengeResult> ScavengeResults { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Code)}: {Code}, {nameof(FullName)}: {FullName}";
        }
    }
}
