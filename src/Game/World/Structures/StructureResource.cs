// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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

    public bool TerrainBound { get; protected set; }
    public bool ConnectRoads { get; protected set; }
    public byte FlagType { get; private set; }

    public string SelectSfx { get; private set; } = "";

    private bool _selectSfxLooped;

    public StringCollection Description { get; }

    public int ScannerIcon { get; private set; }

    public bool Razeable { get; protected set; }
    public bool CastsShadow { get; protected set; }
    public byte RebuildGold { get; protected set; }
    public byte RebuildDays { get; protected set; }
    public UnitIndexListCollection Defenders { get; }
    public int RazeDefenderBudget { get; protected set; }

    // Structur.PAS:1904
    public override void Dispose()
    {
        Description?.Dispose();
        Defenders?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:1929
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _selectSfxLooped = persister.Boolean("SelectSfxLooped", value: _selectSfxLooped);
        // The original leaves the sound out under text only, which the translate stream's mode holds.
        if (!PersistSupport.Translating(persister))
        {
            SelectSfx = persister.String("SelectSfx", value: SelectSfx);
        }

        PersistSupport.Lines(persister, name: "Description", lines: Description);
        ScannerIcon = persister.Int32("ScannerIcon", value: ScannerIcon);
        PersistSupport.UnitIndexListCollection(persister, name: "Defenders", collection: Defenders);
        // The translate stream's mode holds read, so the original calculates the index there too.
        if (persister.Loading || PersistSupport.Translating(persister))
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
        if (structure.GetSelected() && (StrategicMap.Current.Frame & 6) == 6)
        {
            index = structure.ImageIndex;
            offset = structure.Owner.HexX + structure.Owner.HexY * 16;
            if (ImageSequenceList != null)
            {
                sequence = ImageSequenceList.List[index];
                while (sequence != null)
                {
                    frameIndex = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset);
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
                ImageLibrary[index].ShowTransparent(X + ImageOffsetX, Y + ImageOffsetY, lockedSurface);
            }
        }
        else
        {
            if (ImageSequenceList == null)
            {
                ImageLibrary[structure.ImageIndex].Show(X, Y, lockedSurface);
            }
            else
            {
                ShowComplexImage(X, Y, lockedSurface, structure.ImageIndex, (uint)(structure.Owner.HexX + structure.Owner.HexY * 16));
            }
        }
    }

    // Structur.PAS:1920
    public void CalculatePadIndex()
    {
        FlagType = 0;
        for (int I = 0, last = TerrainList.Count - 1; I <= last; I++)
        {
            if (TerrainList.Terrain[I] >= 0)
            {
                FlagType = (byte)(FlagType + 1);
            }
        }
    }

    // intf_P1_09.inc:200

    // Structur.PAS:1914
    public override string ResourceEditName() => "";

}
