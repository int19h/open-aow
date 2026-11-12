// Translated from CenterViewLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World;

public partial class CenterViewLocation : MapObject
{
    public CenterViewLocation(ECustomNode owner) : base(owner)
    { }

    // CenterViewLoc.pas:46
    public new const int ClassId = 2233640;

    // CenterViewLoc.pas:80
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // CenterViewLoc.pas:63
    public override void Activate()
    {
        base.Activate();
        if (!GetEditMode())
        {
            if (CombatMap.Current.CenterViewLocation != null)
            {
                CombatMap.Current.CenterViewLocation?.Dispose();
            }

            CombatMap.Current.CenterViewLocation = this;
        }
    }

    // CenterViewLoc.pas:73
    public override void Deactivate()
    {
        base.Deactivate();
        if (!GetEditMode())
        {
            if (CombatMap.Current.CenterViewLocation == this)
            {
                CombatMap.Current.CenterViewLocation = null;
            }
        }
    }

    // CenterViewLoc.pas:55
    public override bool CanSelect(int pixelX, int pixelY) => true;

    // CenterViewLoc.pas:49
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // CenterViewLoc.pas:52
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => true;

    // CenterViewLoc.pas:58
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => base.PlaceHex(mapContainer, hexX, hexY, hexLevel);

    public new partial class Resource : MapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(27);
        }

        // CenterViewLoc.pas:91
        public new const int ClassId = 2233641;

        // CenterViewLoc.pas:85

        // CenterViewLoc.pas:102
        public override int ResourceUserClassId() => 2233640;

        // CenterViewLoc.pas:94
        public override void SetLevel(byte value)
        {
            base.SetLevel(27);
        }

        // CenterViewLoc.pas:34

        // CenterViewLoc.pas:99
        public override string ResourceEditName() => "Center View Location";

    }

}
