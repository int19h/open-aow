using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// A list box whose items each carry a T, where the original kept a TObject in Items.Objects and
// cast it back. The items are a string list of T, so every object stored is a T.
public class ListBox<T> : ListBox
    where T : class
{
    public ListBox(Component owner) : this(owner, items: new StringList<T>())
    {
    }

    private ListBox(Component owner, StringList<T> items) : base(owner, items)
    {
        Items = items;
    }

    public new StringList<T> Items { get; }
}
