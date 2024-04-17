namespace WoodseatsScouts.QRCodes.Classes;

public class Coin(int i)
{
    public int Id { get; set; } = i;

    public int BaseValueId { get; set; }

    public int Base { get; set; }

    public int Value { get; set; }
    public string Code { get; set; }

    public override string ToString()
    {
        return Code;
    }
}