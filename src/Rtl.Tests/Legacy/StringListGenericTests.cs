namespace Aow.Rtl.Tests;

public sealed class StringListGenericTests
{
    private sealed record Tagged(int Id);

    [Fact]
    public void ObjectsAreTypedAndOtherClassesAreRefused()
    {
        var first = new Tagged(1);
        var second = new Tagged(2);
        var list = new StringList<Tagged>();
        Assert.Equal(0, list.AddObject("b", first));
        list.InsertObject(0, "a", second);
        list.Add("c");
        Tagged? read = list.Objects[1];
        Assert.Same(first, read);
        Assert.Same(second, list.Objects[0]);
        Assert.Null(list.Objects[2]);
        Assert.Equal(1, list.IndexOfObject(first));

        StringCollection untyped = list;
        Assert.Throws<ArgumentException>(() => untyped.Objects[2] = new object());
        // The base adds the string before it stores the object, so the string stays with an empty slot.
        Assert.Throws<ArgumentException>(() => untyped.AddObject("d", "text"));
        Assert.Null(list.Objects[3]);
        list.Delete(3);
        untyped.Objects[2] = first;
        Assert.Same(first, list.Objects[2]);

        list.Exchange(0, 1);
        list.Move(2, 0);
        Assert.Equal(["c", "b", "a"], Enumerable.Range(0, list.Count).Select(i => list[i]));
        Assert.Equal([first, first, second], Enumerable.Range(0, list.Count).Select(i => list.Objects[i]));

        var copy = new StringList<Tagged>();
        copy.AddStrings(list);
        Assert.Same(second, copy.Objects[2]);
        var mixed = new StringList();
        mixed.AddObject("x", new object());
        Assert.Throws<ArgumentException>(() => copy.AddStrings(mixed));
    }
}
