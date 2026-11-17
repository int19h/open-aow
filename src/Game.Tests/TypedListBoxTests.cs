using Aow.Game.UI.Controls;

namespace Aow.Game.Tests;

[Collection("Engine")]
public sealed class TypedListBoxTests
{
    [Fact]
    public void ItemsAreOneTypedStringListThatRefusesOtherClasses()
    {
        ListBox<FileTime> box = new(null!);
        FileTime stamp = new() { Time = 2 };
        _ = box.Items.AddObject("a", stamp);
        ListBox plain = box;
        Assert.Same(box.Items, plain.Items);
        Assert.Same(stamp, box.Items.Objects[0]);
        _ = Assert.Throws<ArgumentException>(() => plain.Items.Objects[0] = new object());
        Assert.Same(stamp, box.Items.Objects[0]);
        _ = plain.Items.Add("b");
        Assert.Null(box.Items.Objects[1]);
        box.Selected = 1;
        Assert.Equal(1, plain.Selected);
    }
}
