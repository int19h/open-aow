// Translated from HSEngine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.World;

// FdXImage, FdYImage (the image offset, ids 12 and 13), FdXmainP, FdYmainP (the main point, ids
// 14 and 15, part of the show priority offset)
public partial class MultiHexMOR : MapObjectResource
{
    public new const int ClassID = 65877;

    public MultiHexMOR(ECustomNode owner) : base(owner)
    {
        SetdXmainP(16);
        SetdYmainP(32);
    }

    protected internal int FdXImage;
    protected internal int FdYImage;
    protected internal int FdXmainP;
    protected internal int FdYmainP;

    // MAPOBJ.PAS:1256
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FdXImage, 12);
        Stream.rwInteger(ref FdYImage, 13);
        Stream.rwInteger(ref FdXmainP, 14);
        Stream.rwInteger(ref FdYmainP, 15);
    }

    // MAPOBJ.PAS:1232

    // MAPOBJ.PAS:1244
    public virtual void SetdXmainP(int Value)
    {
        FdXmainP = Value;
        CalcShowPriorityOffset(FdXmainP, FdYmainP, FLevel);
    }

    // MAPOBJ.PAS:1250
    public virtual void SetdYmainP(int Value)
    {
        FdYmainP = Value;
        CalcShowPriorityOffset(FdXmainP, FdYmainP, FLevel);
    }

    public int dXImage { get => FdXImage; set => FdXImage = value; }
    public int dYImage { get => FdYImage; set => FdYImage = value; }
    public int dXmainP { get => FdXmainP; set => SetdXmainP(value); }
    public int dYmainP { get => FdYmainP; set => SetdYmainP(value); }

}
