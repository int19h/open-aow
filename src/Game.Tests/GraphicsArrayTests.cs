using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

// The blocks of records the original grows with ReallocMem and shifts with Move: the image items
// of a library and of an image node, the frame table list and the invalidation matrix of DisplayC.
// The port holds each in an array with the original's count and capacity. The expected counts,
// capacities and orders follow the Pascal of ImageLib.pas and DisplayC.pas at the line each test names.
public sealed class GraphicsArrayTests
{
    // ImageLib.pas 9699 to 9783: the capacity is rounded up to a multiple of 8 less one, new slots
    // hold no image and the reference -1, InsertItem moves the tail up and clears the slot (its
    // reference 0), DeleteItem moves the tail down and clears the last slot.
    [Fact]
    public void LibraryItemsGrowInBlocksOfEightAndShift()
    {
        using var library = new ImageLibrary(null!);
        library.SetImageReference(0, 5);
        Assert.Equal(1, library._count);
        Assert.Equal(7, library._capacity);
        Assert.Equal(7, library._images.Length);
        Assert.All(library._images.Skip(1), item => Assert.Equal(-1, item.Reference));
        library.SetImageReference(1, 6);
        library.SetImageReference(2, 7);
        library.InsertItem(1);
        Assert.Equal(4, library._count);
        Assert.Equal([5, 0, 6, 7, -1, -1, -1], library._images.Select(item => item.Reference));
        library.SetImageReference(8, 9);
        Assert.Equal(9, library._count);
        Assert.Equal(15, library._capacity);
        Assert.Equal([5, 0, 6, 7, -1, -1, -1, -1, 9, -1, -1, -1, -1, -1, -1], library._images.Select(item => item.Reference));
        library.DeleteItem(0);
        Assert.Equal(8, library._count);
        Assert.Equal([0, 6, 7, -1, -1, -1, -1, 9, 0, -1, -1, -1, -1, -1, -1], library._images.Select(item => item.Reference));
        library.SetCapacity(0);
        Assert.Null(library._images);
        Assert.Equal(0, library._count);
    }

    // ImageLib.pas 1313 to 1397: Add grows the node to its count, Insert by one, both move the tail
    // and Delete moves it down.
    [Fact]
    public void ImageNodeShiftsItsImages()
    {
        var node = new ImageNode(null!);
        var images = Enumerable.Range(0, 4).Select(_ => new ImageNode(null!)).ToArray();
        try
        {
            node.Add(images[0]);
            node.Add(images[1]);
            Assert.Equal((2, 2), (node._count, node._capacity));
            node.Insert(1, images[2]);
            Assert.Equal((3, 3), (node._count, node._capacity));
            Assert.Equal(new LibraryImage[] { images[0], images[2], images[1] }, node._images);
            node.Insert(0, images[3]);
            Assert.Equal(new LibraryImage[] { images[3], images[0], images[2], images[1] }, node._images);
            node.Delete(1);
            Assert.Equal(3, node._count);
            Assert.Equal(new LibraryImage[] { images[3], images[2], images[1] }, node._images.Take(3));
            Assert.Equal(1, node.IndexOf(images[2]));
            Assert.Equal(-1, node.IndexOf(images[0]));
            Assert.Same(images[1], node.GetImage(2));
            Assert.Null(node.GetSafeImage(3));
            node.SetCapacity(0);
            Assert.Null(node._images);
        }
        finally
        {
            node._count = 0;
            node.Dispose();
            foreach (var image in images) image.Dispose();
        }
    }

    // ImageLib.pas 9434 to 9586: Grow adds 4 up to a capacity of 8, 16 up to 64 and a quarter
    // above. Put past the end sets the count and fills the new items with the bytes of the default
    // value. Insert and Delete move the tail, Assign copies the items with a capacity of the count.
    [Fact]
    public void FrameTableListGrowsFillsAndShifts()
    {
        var list = new FrameTableList { DefaultValue = 7 };
        var copy = new FrameTableList();
        try
        {
            Assert.Equal(0, list.Add(5));
            Assert.Equal(4, list.Capacity);
            list[6] = 9;
            Assert.Equal((7, 7), (list.Count, list.Capacity));
            Assert.Equal([5, 0x07070707, 0x07070707, 0x07070707, 0x07070707, 0x07070707, 9], list.List);
            list.Insert(0, 1);
            Assert.Equal((8, 11), (list.Count, list.Capacity));
            Assert.Equal([1, 5, 0x07070707, 0x07070707, 0x07070707, 0x07070707, 0x07070707, 9], list.List.Take(8));
            list.Delete(1);
            list.Delete(1);
            Assert.Equal([1, 0x07070707, 0x07070707, 0x07070707, 0x07070707, 9], list.List.Take(list.Count));
            for (int i = 0; i < 6; i++) list.Add(i);
            Assert.Equal((12, 27), (list.Count, list.Capacity));
            copy.Assign(list);
            Assert.Equal((12, 12), (copy.Count, copy.Capacity));
            Assert.True(copy.Compare(list));
            copy.Sort((a, b) => a.CompareTo(b));
            Assert.Equal([0, 1, 1, 2, 3, 4, 5, 9, 0x07070707, 0x07070707, 0x07070707, 0x07070707], copy.List);
            Assert.False(copy.Compare(list));
            Assert.Equal(1, list.IndexOf(0x07070707));
            list.Clear();
            Assert.Equal((0, 0), (list.Count, list.Capacity));
            Assert.Null(list.List);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Delete(0));
        }
        finally
        {
            list.Dispose();
            copy.Dispose();
        }
    }

    // DisplayC.pas 3145 to 3178: the matrix starts at zero, GetVal clips at 2, SetVal outside does
    // nothing, GetVal outside raises.
    [Fact]
    public void MatrixKeepsItsValuesRowByRow()
    {
        var matrix = new Matrix(4, 3);
        Assert.Equal(12, matrix._data.Length);
        Assert.All(matrix._data, value => Assert.Equal(0, value));
        matrix.SetValue(1, 2, 5);
        matrix.SetValue(3, 0, 1);
        matrix.SetValue(4, 0, 1);
        Assert.Equal(5, matrix._data[1 + 2 * 4]);
        Assert.Equal(1, matrix._data[3]);
        Assert.Equal(2, matrix.GetValue(1, 2));
        Assert.Equal(1, matrix.GetValue(3, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => matrix.GetValue(4, 0));
        matrix.Dispose();
        Assert.Null(matrix._data);
        var empty = new Matrix(0, 3);
        Assert.Null(empty._data);
        Assert.Throws<ArgumentOutOfRangeException>(() => empty.GetValue(0, 0));
    }

    // DisplayC.pas 894, worked by hand. With
    //   1 1 0 0
    //   1 1 1 0
    //   0 1 1 0
    //   1 0 0 0
    // the block at (0, 0) grows rows first to 2 by 2 (area 4) and columns first to 2 by 2 (area 4),
    // so the rows win the tie. At (1, 1) both give 2 by 2. At (0, 2) the cell is clear and the
    // area 0. A column of three at (0, 0) of a second matrix gives 1 by 3 both ways.
    [Fact]
    public void InvalidBlockSizeFindsTheLargestBlock()
    {
        using var owner = new Component(null!);
        var displayController = new DisplayController(owner);
        var matrix = new Matrix(4, 4);
        foreach (var (x, y) in new[] { (0, 0), (1, 0), (0, 1), (1, 1), (2, 1), (1, 2), (2, 2), (0, 3) }) matrix.SetValue((byte)x, (byte)y, 1);
        int bw = -1, bh = -1;
        Assert.Equal(4, displayController.GetInverseBlockSize(0, 0, matrix, ref bw, ref bh));
        Assert.Equal((2, 2), (bw, bh));
        Assert.Equal(4, displayController.GetInverseBlockSize(1, 1, matrix, ref bw, ref bh));
        Assert.Equal((2, 2), (bw, bh));
        bw = bh = -1;
        Assert.Equal(0, displayController.GetInverseBlockSize(0, 2, matrix, ref bw, ref bh));
        Assert.Equal((-1, -1), (bw, bh));
        var column = new Matrix(4, 4);
        foreach (int y in new[] { 0, 1, 2 }) column.SetValue(0, (byte)y, 1);
        Assert.Equal(3, displayController.GetInverseBlockSize(0, 0, column, ref bw, ref bh));
        Assert.Equal((1, 3), (bw, bh));
    }
}
