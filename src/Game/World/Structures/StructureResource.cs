// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// selection Sfx, description, scanner icon. . other recovered resource fields below
public partial class StructureResource : FixedImageLibraryTerrainMapObjectResource
{
    public StructureResource(ECustomNode owner) : base(owner)
    {
        SetLevel(26);
        FConnectRoads = true;
        FTerrainBound = true;
        FDescription = new StringList();
        FDefenders = new UnitIndexListCollection();
        FRazeDefenderBudget = 50;
        FRazeable = false;
    }

    public new const int ClassID = 131915;

    public bool FTerrainBound;
    public bool FConnectRoads;
    public byte FFlagType;
    public string FSelectSFX = "";
    public bool FSelectSFXLooped;
    public StringCollection FDescription;
    public int FScannerIcon;
    public bool FRazeable;
    public bool FCastsShadow;
    public byte FRebuildGold;
    public byte FRebuildDays;
    public UnitIndexListCollection FDefenders;
    public int FRazeDefenderBudget;

    // Structur.PAS:1904
    public override void Dispose()
    {
        FDescription?.Dispose();
        FDefenders?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:1929
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FSelectSFXLooped, 20);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSelectSFX, 21);
        }

        Stream.rwStrings(FDescription, 22);
        Stream.rwInteger(ref FScannerIcon, 23);
        Stream.rwStreamProc(28, FDefenders.LoadFromStream, FDefenders.SaveToStream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            CalculatePadIndex();
        }
    }

    // Structur.PAS:1892

    // Structur.PAS:1917
    public override int ResourceUserClassID() => 131914;

    // Structur.PAS:1940
    public virtual void Show(int X, int Y, in LockedSurface SurfaceDesc, Structure Structure)
    {
        int Index;
        int Offset;
        int FrameIndex;
        ImageSequence Sequence;
        if (Structure.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
        {
            Index = Structure.ImageIndex;
            Offset = ((MapField)Structure.Owner).Xhx + ((MapField)Structure.Owner).Yhx * 16;
            if (FImageSequenceList != null)
            {
                Sequence = (ImageSequence)FImageSequenceList.List[Index];
                while (Sequence != null)
                {
                    FrameIndex = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, Offset);
                    if (FrameIndex != -1)
                    {
                        if (Sequence.ImageLibrary[FrameIndex].ShowInfo.ShowMode == ShowMode.Opaque)
                        {
                            Sequence.ImageLibrary[FrameIndex].ShowTransparent(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                        }
                        else
                        {
                            Sequence.ImageLibrary[FrameIndex].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }
            else
            {
                FImageLibrary[Index].ShowTransparent(X + FdXImage, Y + FdYImage, SurfaceDesc);
            }
        }
        else
        {
            if (FImageSequenceList == null)
            {
                FImageLibrary[Structure.ImageIndex].Show(X, Y, SurfaceDesc);
            }
            else
            {
                ShowComplexImage(X, Y, SurfaceDesc, Structure.ImageIndex, (uint)(((MapField)Structure.Owner).Xhx + ((MapField)Structure.Owner).Yhx * 16));
            }
        }
    }

    // Structur.PAS:1920
    public void CalculatePadIndex()
    {
        FFlagType = 0;
        for (int I = 0, last = FTerrainList.Count - 1; I <= last; I++)
        {
            if (FTerrainList.Terrain[I] >= 0)
            {
                FFlagType = (byte)(FFlagType + 1);
            }
        }
    }

    // intf_P1_09.inc:200

    // Structur.PAS:1914
    public override string ResourceEditName() => "";

}
