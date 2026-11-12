// Translated from HSEngine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _imageOffsetX, 12);
        stream.rwInteger(ref _imageOffsetY, 13);
        stream.rwInteger(ref _mainPointOffsetX, 14);
        stream.rwInteger(ref _mainPointOffsetY, 15);
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
