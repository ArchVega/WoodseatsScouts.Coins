using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Routing.Tree;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.Tests.Integration;

public static class TestDataFactory
{
    public static Troop Troop1 ;
    public static Troop Troop2 ;
    public static Troop Troop3 ;
    public static Troop Troop4 ;

    public static Member Troop1Member1;
    public static Member Troop1Member2;
    public static Member Troop1Member3;
    public static Member Troop1Member4;

    public static Member Troop2Member1;
    public static Member Troop2Member2;
    public static Member Troop2Member3;
    public static Member Troop2Member4;

    public static Member Troop3Member1;
    public static Member Troop3Member2;
    public static Member Troop3Member3;
    public static Member Troop3Member4;

    public static Member Troop4Member1;
    public static Member Troop4Member2;
    public static Member Troop4Member3;
    public static Member Troop4Member4;

    static TestDataFactory()
    {
        Troop1 = CreateTroop(1);
        Troop2 = CreateTroop(2);
        Troop3 = CreateTroop(3);
        Troop4 = CreateTroop(4);

        Troop1Member1 = CreateTroopMember(1, Troop1, 'A');
        Troop1Member2 = CreateTroopMember(2, Troop1, 'A');
        Troop1Member3 = CreateTroopMember(3, Troop1, 'A');
        Troop1Member4 = CreateTroopMember(4, Troop1, 'A');
        Troop2Member1 = CreateTroopMember(5, Troop2, 'A');
        Troop2Member2 = CreateTroopMember(6, Troop2, 'A');
        Troop2Member3 = CreateTroopMember(7, Troop2, 'A');
        Troop2Member4 = CreateTroopMember(8, Troop2, 'A');
        Troop3Member1 = CreateTroopMember(9, Troop3, 'A');
        Troop3Member2 = CreateTroopMember(10, Troop3, 'A');
        Troop3Member3 = CreateTroopMember(11, Troop3, 'A');
        Troop3Member4 = CreateTroopMember(12, Troop3, 'A');
        Troop4Member1 = CreateTroopMember(13, Troop4, 'A');
        Troop4Member2 = CreateTroopMember(14, Troop4, 'A');
        Troop4Member3 = CreateTroopMember(15, Troop4, 'A');
        Troop4Member4 = CreateTroopMember(16, Troop4, 'A');
    }

    public static Troop CreateTroop(int number)
    {
        return new Troop
        {
            Name = "Troop" + number
        };
    }

    public static Member CreateTroopMember(int number, Troop troop, char section) =>
        new()
        {
            FirstName = "Member" + number,
            LastName = "Member" + number,
            Number = number,
            Troop = troop,
            Section = section.ToString()
        };

    public static List<Troop> GetKnownTroops()
    {
        return new List<Troop>
        {
            Troop1,
            Troop2,
            Troop3,
            Troop4
        };
    }

    public static List<Member> GetKnownMembers()
    {
        var members = new List<Member>
        {
            Troop1Member1,
            Troop1Member2,
            Troop1Member3,
            Troop1Member4,
            Troop2Member1,
            Troop2Member2,
            Troop2Member3,
            Troop2Member4,
            Troop3Member1,
            Troop3Member2,
            Troop3Member3,
            Troop3Member4,
            Troop4Member1,
            Troop4Member2,
            Troop4Member3,
            Troop4Member4,
        };

        return members;
    }
}