// Translated from OverlayObject.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Terrain;

public partial class OverlayObject : FixedImageLibraryTerrainMapObject
{
    public OverlayObject(ECustomNode owner) : base(owner)
    { }

    // OverlayObject.pas:36
    public new const int ClassId = 131228;

    // OverlayType
    public new partial class Resource : FixedImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // OverlayObject.pas:42
        public new const int ClassId = 131229;

        private int _overlayType;

        // OverlayObject.pas:48
        public override void ReadWrite(StorageStream stream)
        {
            base.ReadWrite(stream);
            stream.Int32(ref _overlayType, 40);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            _overlayType = persister.Int32("OverlayType", value: _overlayType);
        }

        // OverlayObject.pas:45
        public override int ResourceUserClassId() => 131228;

        // OverlayObject.pas:39
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_overlayObjectText);

        private static readonly string s_overlayObjectText = "Overlay Object";

    }

}
