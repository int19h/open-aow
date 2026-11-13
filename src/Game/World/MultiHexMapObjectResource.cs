// Translated from HSEngine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

namespace Aow.Game.World;

// FdXImage, FdYImage (the image offset, ids 12 and 13), FdXmainP, FdYmainP (the main point, ids
// 14 and 15, part of the show priority offset)
public partial class MultiHexMapObjectResource : MapObjectResource
{
    public new const int ClassId = 65877;

    public MultiHexMapObjectResource(ECustomNode owner) : base(owner)
    {
        SetMainPointOffsetX(16);
        SetMainPointOffsetY(32);
    }

    protected internal int _imageOffsetX;
    protected internal int _imageOffsetY;
    private int _mainPointOffsetX;
    private int _mainPointOffsetY;

    // MAPOBJ.PAS:1256
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _imageOffsetX, 12);
        stream.Int32(ref _imageOffsetY, 13);
        stream.Int32(ref _mainPointOffsetX, 14);
        stream.Int32(ref _mainPointOffsetY, 15);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _imageOffsetX = persister.Int32("ImageOffsetX", value: _imageOffsetX);
        _imageOffsetY = persister.Int32("ImageOffsetY", value: _imageOffsetY);
        _mainPointOffsetX = persister.Int32("MainPointOffsetX", value: _mainPointOffsetX);
        _mainPointOffsetY = persister.Int32("MainPointOffsetY", value: _mainPointOffsetY);
    }

    // MAPOBJ.PAS:1232

    // MAPOBJ.PAS:1244
    public virtual void SetMainPointOffsetX(int value)
    {
        _mainPointOffsetX = value;
        CalculateShowPriorityOffset(_mainPointOffsetX, _mainPointOffsetY, _level);
    }

    // MAPOBJ.PAS:1250
    public virtual void SetMainPointOffsetY(int value)
    {
        _mainPointOffsetY = value;
        CalculateShowPriorityOffset(_mainPointOffsetX, _mainPointOffsetY, _level);
    }

    public int ImageOffsetX { get => _imageOffsetX; set => _imageOffsetX = value; }
    public int ImageOffsetY { get => _imageOffsetY; set => _imageOffsetY = value; }
    public int MainPointOffsetX { get => _mainPointOffsetX; set => SetMainPointOffsetX(value); }
    public int MainPointOffsetY { get => _mainPointOffsetY; set => SetMainPointOffsetY(value); }

}
