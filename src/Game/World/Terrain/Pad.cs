// Translated from Pad.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

// PadActive
public class Pad(CustomNode owner) : FixedImageLibraryTerrainMapObject(owner)
{

    // Pad.pas:52
    public new const int ClassId = 131334;

    // Pad.pas:55
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // Pad.pas:58
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => mapLevel.HexLevel != 0 ? (sbyte)12 : (sbyte)-1;

    // PadSize
    public new class Resource : FixedImageLibraryTerrainMapObjectResource
    {
        public Resource(CustomNode owner) : base(owner) => SetLevel(10);

        // Pad.pas:76
        public new const int ClassId = 131335;

        private byte _padSize;

        // Pad.pas:69
        public override void Dispose()
        {
            if (Engine is not null && !Engine.HexSpriteSet.Status.Contains(HexSpriteSetStatusElement.Destroying))
            {
                Engine.HexSpriteSet.PadControl.UnregisterPadResource(this, size: _padSize);
            }

            base.Dispose();
        }

        // Pad.pas:101
        public override EditorClass Editor() => Engine.Editors.FindEditor(131335) ?? base.Editor();

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
            _padSize = 0;
            for (int i = 0, last = TerrainList.Count - 1; i <= last; i++)
            {
                if (TerrainList.GetTerrain(i) >= -1)
                {
                    _padSize = (byte)(_padSize + 1);
                }
            }

            Engine.HexSpriteSet.PadControl.RegisterPadResource(this, size: _padSize);
        }

        // Pad.pas:79
        public override string ResourceEditName() => "Pad";

    }
}
