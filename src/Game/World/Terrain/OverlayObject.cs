// Translated from OverlayObject.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Terrain;

public partial class OverlayObject : FixedImageLibraryTerrainMapObject
{
    public OverlayObject(ECustomNode owner) : base(owner)
    { }

    // OverlayObject.pas:36
    public new const int ClassID = 131228;

    // FOverlayType
    public new partial class Resource : FixedImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // OverlayObject.pas:42
        public new const int ClassID = 131229;

        public int FOverlayType;

        // OverlayObject.pas:48
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FOverlayType, 40);
        }

        // OverlayObject.pas:45
        public override int ResourceUserClassID() => 131228;

        // OverlayObject.pas:39
        public override string ResourceEditName() => AbilityRules.TranslateRStr(OverlayObjectRStr);

        private static readonly string OverlayObjectRStr = "Overlay Object";

    }

}
