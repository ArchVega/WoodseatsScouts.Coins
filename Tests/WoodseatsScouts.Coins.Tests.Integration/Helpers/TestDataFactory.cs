using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Tests.Integration.Helpers;

public class TestDataFactory(IAppDbContext appDbContext)
{
    public ScoutGroupsCollection ScoutGroups => new ScoutGroupsCollection(appDbContext);

    public MembersCollection Members => new MembersCollection(appDbContext);

    public class ScoutGroupsCollection(IAppDbContext appDbContext)
    {
        public ScoutGroup Crimson => appDbContext.ScoutGroups!.Single(x => x.Name == "Crimson");
        public ScoutGroup Jet => appDbContext.ScoutGroups!.Single(x => x.Name == "Jet");
        public ScoutGroup Royal => appDbContext.ScoutGroups!.Single(x => x.Name == "Royal");
        public ScoutGroup Saffron => appDbContext.ScoutGroups!.Single(x => x.Name == "Saffron");

        public List<ScoutGroup> GetKnownScoutGroups()
        {
            return
            [
                Crimson,
                Jet,
                Royal,
                Saffron
            ];
        }
    }

    public class MembersCollection(IAppDbContext appDbContext)
    {
        public ScoutMember CharcoalCrimson => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Charcoal" && x.LastName == "Crimson");
        public ScoutMember OlivineCrimson => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Olivine" && x.LastName == "Crimson");
        public ScoutMember IcterineCrimson => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Icterine" && x.LastName == "Crimson");
        public ScoutMember TurquoiseCrimson => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Turquoise" && x.LastName == "Crimson");
        public ScoutMember PumpkinJet => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Pumpkin" && x.LastName == "Jet");
        public ScoutMember GlaucousJet => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Glaucous" && x.LastName == "Jet");
        public ScoutMember PistachioJet => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Pistachio" && x.LastName == "Jet");
        public ScoutMember RedJet => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Red" && x.LastName == "Jet");
        public ScoutMember AsparagusRoyal => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Asparagus" && x.LastName == "Royal");
        public ScoutMember JasperRoyal => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Jasper" && x.LastName == "Royal");
        public ScoutMember GhostRoyal => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Ghost" && x.LastName == "Royal");
        public ScoutMember CeriseRoyal => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Cerise" && x.LastName == "Royal");
        public ScoutMember HunterSaffron => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Hunter" && x.LastName == "Saffron");
        public ScoutMember OxfordSaffron => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Oxford" && x.LastName == "Saffron");
        public ScoutMember RosewoodSaffron => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Rosewood" && x.LastName == "Saffron");
        public ScoutMember VioletSaffron => appDbContext.ScoutMembers!.Single(x => x.FirstName == "Violet" && x.LastName == "Saffron");

        public List<ScoutMember> GetKnownMembers()
        {
            var members = new List<ScoutMember>
            {
                CharcoalCrimson,
                OlivineCrimson,
                IcterineCrimson,
                TurquoiseCrimson,
                PumpkinJet,
                GlaucousJet,
                PistachioJet,
                RedJet,
                AsparagusRoyal,
                JasperRoyal,
                GhostRoyal,
                CeriseRoyal,
                HunterSaffron,
                OxfordSaffron,
                RosewoodSaffron,
                VioletSaffron
            };

            return members;
        }
    }

    public static ScoutMember CreateScoutGroupMember(int number, ScoutGroup scoutGroup, char section) =>
        new()
        {
            FirstName = "Member" + number,
            LastName = "Member" + number,
            Number = number,
            ScoutGroup = scoutGroup,
            ScoutSectionCode = section.ToString()
        };
}