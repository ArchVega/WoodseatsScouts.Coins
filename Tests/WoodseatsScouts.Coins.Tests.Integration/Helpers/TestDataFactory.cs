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
        public Member CharcoalCrimson => appDbContext.Members!.Single(x => x.FirstName == "Charcoal" && x.LastName == "Crimson");
        public Member OlivineCrimson => appDbContext.Members!.Single(x => x.FirstName == "Olivine" && x.LastName == "Crimson");
        public Member IcterineCrimson => appDbContext.Members!.Single(x => x.FirstName == "Icterine" && x.LastName == "Crimson");
        public Member TurquoiseCrimson => appDbContext.Members!.Single(x => x.FirstName == "Turquoise" && x.LastName == "Crimson");
        public Member PumpkinJet => appDbContext.Members!.Single(x => x.FirstName == "Pumpkin" && x.LastName == "Jet");
        public Member GlaucousJet => appDbContext.Members!.Single(x => x.FirstName == "Glaucous" && x.LastName == "Jet");
        public Member PistachioJet => appDbContext.Members!.Single(x => x.FirstName == "Pistachio" && x.LastName == "Jet");
        public Member RedJet => appDbContext.Members!.Single(x => x.FirstName == "Red" && x.LastName == "Jet");
        public Member AsparagusRoyal => appDbContext.Members!.Single(x => x.FirstName == "Asparagus" && x.LastName == "Royal");
        public Member JasperRoyal => appDbContext.Members!.Single(x => x.FirstName == "Jasper" && x.LastName == "Royal");
        public Member GhostRoyal => appDbContext.Members!.Single(x => x.FirstName == "Ghost" && x.LastName == "Royal");
        public Member CeriseRoyal => appDbContext.Members!.Single(x => x.FirstName == "Cerise" && x.LastName == "Royal");
        public Member HunterSaffron => appDbContext.Members!.Single(x => x.FirstName == "Hunter" && x.LastName == "Saffron");
        public Member OxfordSaffron => appDbContext.Members!.Single(x => x.FirstName == "Oxford" && x.LastName == "Saffron");
        public Member RosewoodSaffron => appDbContext.Members!.Single(x => x.FirstName == "Rosewood" && x.LastName == "Saffron");
        public Member VioletSaffron => appDbContext.Members!.Single(x => x.FirstName == "Violet" && x.LastName == "Saffron");

        public List<Member> GetKnownMembers()
        {
            var members = new List<Member>
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

    public static Member CreateScoutGroupMember(int number, ScoutGroup scoutGroup, char section) =>
        new()
        {
            FirstName = "Member" + number,
            LastName = "Member" + number,
            Number = number,
            ScoutGroup = scoutGroup,
            SectionId = section.ToString()
        };
}