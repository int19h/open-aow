
namespace Aow.Game.Draw;

public partial class BlendTable5Bit
{
    public override void Assign(BlendTable Source)
    {
        if (Source is BlendTable5Bit table) Array.Copy(table.FTable, FTable, 32 * 32);
    }
}
