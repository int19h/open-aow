#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Pad.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Terrain;

// PadActive
public class Pad(CustomNode owner) : FixedImageLibraryTerrainMapObject(owner)
{

    // Pad.pas:52
    public new const int ClassId = 131_334;

    // Pad.pas:55
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // Pad.pas:58
    public override int ForceTerrainType(MapLevel mapLevel, int hexX, int hexY) => mapLevel.HexLevel != 0 ? 12 : -1;

    // PadSize
    public new class Resource : FixedImageLibraryTerrainMapObjectResource
    {
        public Resource(CustomNode owner) : base(owner) => SetLevel(10);

        // Pad.pas:76
        public new const int ClassId = 131_335;

        private int _padSize;

        // Pad.pas:69
        public override void Dispose()
        {
            if (Engine is not null && !Engine.GamePack.Status.Contains(HexSpriteSetStatuses.Destroying))
            {
                Engine.GamePack.PadControl.UnregisterPadResource(this, size: _padSize);
            }

            base.Dispose();
        }

        // Pad.pas:101
        public override EditorClass Editor() => Engine.Editors.FindEditor(ClassId) ?? base.Editor();

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
        public override int ResourceUserClassId() => Pad.ClassId;

        // Pad.pas:85
        public void ActivateMessage()
        {
            _padSize = 0;
            for (int i = 0, last = TerrainList.Count - 1; i <= last; i++)
            {
                if (TerrainList.GetTerrain(i) >= -1)
                {
                    _padSize = (_padSize + 1).TruncateToByte();
                }
            }

            Engine.GamePack.PadControl.RegisterPadResource(this, size: _padSize);
        }

        // Pad.pas:79
        public override string ResourceEditName() => "Pad";

    }
}
