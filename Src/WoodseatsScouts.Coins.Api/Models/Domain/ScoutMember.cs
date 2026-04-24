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
        
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string? LastName { get; set; }
        
        public int ScoutGroupId { get; init; }
        
        public ScoutGroup ScoutGroup { get; init; } = null!;
        
        [MaxLength(1)]
        public string ScoutSectionId { get; init; } = null!;
        
        public ScoutSection ScoutSection { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Clue1State { get; init; }

        [MaxLength(100)]
        public string? Clue2State { get; init; }

        [MaxLength(100)]
        public string? Clue3State { get; init; }
        
        public bool IsDayVisitor { get; set; }
        
        public bool HasImage { get; set; }
        
        public List<ScanSession> ScavengeResults { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Code)}: {Code}, {nameof(FullName)}: {FullName}";
        }
    }
}
