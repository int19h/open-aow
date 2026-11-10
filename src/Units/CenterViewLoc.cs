// Translated from CenterViewLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public partial class CenterViewLocation : MapObject
{
    public CenterViewLocation(ECustomNode owner) : base(owner)
    { }

    // CenterViewLoc.pas:46
    public new const int ClassID = 2233640;

    // CenterViewLoc.pas:80
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // CenterViewLoc.pas:63
    public override void Activate()
    {
        base.Activate();
        if (!GetEditMode())
        {
            if (AoWCombatMap.Current.FCenterViewLocation != null)
            {
                AoWCombatMap.Current.FCenterViewLocation?.Dispose();
            }

            AoWCombatMap.Current.FCenterViewLocation = this;
        }
    }

    // CenterViewLoc.pas:73
    public override void Deactivate()
    {
        base.Deactivate();
        if (!GetEditMode())
        {
            if (AoWCombatMap.Current.FCenterViewLocation == this)
            {
                AoWCombatMap.Current.FCenterViewLocation = null;
            }
        }
    }

    // CenterViewLoc.pas:55
    public override bool CanSelect(int Xhp, int Yhp) => true;

    // CenterViewLoc.pas:49
    public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

    // CenterViewLoc.pas:52
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

    // CenterViewLoc.pas:58
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);

    public new partial class Resource : MapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(27);
        }

        // CenterViewLoc.pas:91
        public new const int ClassID = 2233641;

        // CenterViewLoc.pas:85

        // CenterViewLoc.pas:102
        public override int ResourceUserClassID() => 2233640;

        // CenterViewLoc.pas:94
        public override void SetLevel(byte Value)
        {
            base.SetLevel(27);
        }

        // CenterViewLoc.pas:34

        // CenterViewLoc.pas:99
        public override string ResourceEditName() => "Center View Location";

    }

}
