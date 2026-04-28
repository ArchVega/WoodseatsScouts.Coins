using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    // dotcover disable
    /// <summary>
    /// Member photos are stored on disk in the ClientApp/public/member-images directory and the files must be a jpg file whose name
    /// matches the Number field
    /// </summary>
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class ScoutMember
    {
        public int Id { get; set; }

        /// <summary>
        /// Unlike Coins which has a static value for Code (as Coins are generated before going live), Members require a computed value in case
        /// members are added to the application after it is live.  
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Code { get; set; } = null!;
        
        public int Number { get; set; }
        
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }
        
        public int ScoutGroupId { get; set; }
        
        public ScoutGroup ScoutGroup { get; init; } = null!;
        
        [MaxLength(1)]
        public string ScoutSectionCode { get; set; } = null!;
        
        public ScoutSection ScoutSection { get; set; } = null!;
        
        public string? Clue1State { get; init; }

        public string? Clue2State { get; init; }

        public string? Clue3State { get; init; }
        
        public bool IsDayVisitor { get; set; }
        
        public bool HasImage { get; set; }
        
        public List<ScanSession> ScanSessions { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Code)}: {Code}, {nameof(FullName)}: {FullName}";
        }
    }
}
