
namespace Aow.Game.Draw;

public partial class BlendTable5Bit
{
    public override void Assign(BlendTable source)
    {
        if (source is BlendTable5Bit table) Array.Copy(table.Table, Table, 32 * 32);
    }
}
