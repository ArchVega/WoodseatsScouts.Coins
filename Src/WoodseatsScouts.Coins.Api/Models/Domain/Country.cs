// dotcover disable
namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}
