namespace WoodseatsScouts.QRCodes.Classes;

public class Coin(int i)
{
    public int Id { get; set; } = i;

    public int Base { get; set; }

    public int Value { get; set; }

    public override string ToString()
    {
        return $"C{Id:0000}{Base:000}{Value:000}";
    }
}