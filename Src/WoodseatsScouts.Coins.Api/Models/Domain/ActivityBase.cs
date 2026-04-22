namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ActivityBase
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    /// <summary>
    /// An activity base can be the name of a Scout Group, though we do not need to track this relationship at present. The nullable key can be used just in case we want to
    /// perform an order by where we show ScoutGroup names first and then Activity names after in a list view, for example.
    /// </summary>
    public int? ScoutGroupId { get; set; }
    
    public ScoutGroup ScoutGroup { get; set; }
}