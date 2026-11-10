// Translated from ItemGrid.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Ui.Grids;

public partial class ItemListView : EChangeNotifyNode
{
    public ItemListView(ECustomNode owner) : base(owner)
    { }

    // ItemGrid.pas:57
    public virtual Item GetItem(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
