// Translated from Pad.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

// PadActive
public partial class Pad : FixedImageLibraryTerrainMapObject
{
    public Pad(ECustomNode owner) : base(owner)
    {
        PadActive = true;
    }

    // Pad.pas:52
    public new const int ClassId = 131334;

    public bool PadActive;

    // Pad.pas:46

    // Pad.pas:55
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // Pad.pas:58
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY)
    {
        sbyte result;
        if (mapLevel.HexLevel != 0)
        {
            result = 12;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // PadSize
    public new partial class Resource : FixedImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(10);
        }

        // Pad.pas:76
        public new const int ClassId = 131335;

        public byte PadSize;

        // Pad.pas:69
        public override void Dispose()
        {
            if (Engine != null && !Engine.HexSpriteSet.Status.Contains(HexSpriteSetStatusElement.Destroying))
            {
                ((AoWHSSet)Engine.HexSpriteSet).PadControl.UnregisterPadResource(this, PadSize);
            }

            base.Dispose();
        }

        // Pad.pas:63

        // Pad.pas:101
        public override EditorClass Editor()
        {
            EditorClass result = Engine.Editors.FindEditor(131335);
            if (result == null)
            {
                result = base.Editor();
            }

            return result;
        }

        // Pad.pas:95
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            base.MessageProc(ref message, sender);
            if (message.MessageId == 536870920)
            {
                ActivateMessage();
            }
        }

        // Pad.pas:82
        public override int ResourceUserClassId() => 131334;

        // Pad.pas:85
        public void ActivateMessage()
        {
            PadSize = 0;
            for (int I = 0, last = TerrainList.Count - 1; I <= last; I++)
            {
                if (TerrainList.GetTerrain(I) >= -1)
                {
                    PadSize = (byte)(PadSize + 1);
                }
            }

            ((AoWHSSet)Engine.HexSpriteSet).PadControl.RegisterPadResource(this, PadSize);
        }

        // Pad.pas:32

        // Pad.pas:79
        public override string ResourceEditName() => "Pad";

    }

    // Pad.pas:20

}
