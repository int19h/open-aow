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
        ConnectRoads = true;
        TerrainBound = true;
        Description = new StringList();
        Defenders = new UnitIndexListCollection();
        RazeDefenderBudget = 50;
        Razeable = false;
    }

    public new const int ClassId = 131915;

    public bool TerrainBound;
    public bool ConnectRoads;
    public byte FlagType;
    public string SelectSfx = "";
    public bool SelectSfxLooped;
    public StringCollection Description;
    public int ScannerIcon;
    public bool Razeable;
    public bool CastsShadow;
    public byte RebuildGold;
    public byte RebuildDays;
    public UnitIndexListCollection Defenders;
    public int RazeDefenderBudget;

    // Structur.PAS:1904
    public override void Dispose()
    {
        Description?.Dispose();
        Defenders?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:1929
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwBool(ref SelectSfxLooped, 20);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref SelectSfx, 21);
        }

        stream.rwStrings(Description, 22);
        stream.rwInteger(ref ScannerIcon, 23);
        stream.rwStreamProc(28, Defenders.LoadFromStream, Defenders.SaveToStream);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            CalculatePadIndex();
        }
    }

    // Structur.PAS:1892

    // Structur.PAS:1917
    public override int ResourceUserClassId() => 131914;

    // Structur.PAS:1940
    public virtual void Show(int X, int Y, in LockedSurface lockedSurface, Structure structure)
    {
        int index;
        int offset;
        int frameIndex;
        ImageSequence sequence;
        if (structure.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
        {
            index = structure.ImageIndex;
            offset = ((MapField)structure.Owner).HexX + ((MapField)structure.Owner).HexY * 16;
            if (_imageSequenceList != null)
            {
                sequence = (ImageSequence)_imageSequenceList.List[index];
                while (sequence != null)
                {
                    frameIndex = sequence.GetFrameIndex(AoWHSMap.Current.Frame, offset);
                    if (frameIndex != -1)
                    {
                        if (sequence.ImageLibrary[frameIndex].ShowInfo.ShowMode == ShowMode.Opaque)
                        {
                            sequence.ImageLibrary[frameIndex].ShowTransparent(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                        }
                        else
                        {
                            sequence.ImageLibrary[frameIndex].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                        }
                    }

                    sequence = sequence.Next;
                }
            }
            else
            {
                _imageLibrary[index].ShowTransparent(X + _imageOffsetX, Y + _imageOffsetY, lockedSurface);
            }
        }
        else
        {
            if (_imageSequenceList == null)
            {
                _imageLibrary[structure.ImageIndex].Show(X, Y, lockedSurface);
            }
            else
            {
                ShowComplexImage(X, Y, lockedSurface, structure.ImageIndex, (uint)(((MapField)structure.Owner).HexX + ((MapField)structure.Owner).HexY * 16));
            }
        }
    }

    // Structur.PAS:1920
    public void CalculatePadIndex()
    {
        FlagType = 0;
        for (int I = 0, last = _terrainList.Count - 1; I <= last; I++)
        {
            if (_terrainList.Terrain[I] >= 0)
            {
                FlagType = (byte)(FlagType + 1);
            }
        }
    }

    // intf_P1_09.inc:200

    // Structur.PAS:1914
    public override string ResourceEditName() => "";

}
