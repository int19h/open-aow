// Translated from Mountain.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Terrain;

// FMode, FPlayer, FDuration, FOriginalOverlay
public partial class RaisedMountain : ILTerrainMO
{
    public RaisedMountain(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:178
    public new const int ClassID = 131434;

    public Set<AnonymousEnum_rmRaising> FMode;
    public sbyte FPlayer;
    public sbyte FDuration;
    public sbyte FOriginalOverlay;

    // Mountain.pas:181
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwShortintEx(ref FPlayer, -1, 40);
        Stream.rwShortintEx(ref FDuration, -1, 41);
        Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
    }

    // Mountain.pas:210
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
        }
    }

    // Mountain.pas:223
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (!FMode.Contains(AnonymousEnum_rmRaising.Raising))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // Mountain.pas:216
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4402)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:189
    public void RestoreTerrain()
    {
        MapField Field;
        sbyte Overlay;
        Field = (MapField)Owner;
        Overlay = FOriginalOverlay;
        this.Dispose();
        if (Overlay > 0)
        {
            AoWHSMap.Current.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
        }
    }

    // Mountain.pas:201
    public void NewTurn(sbyte Player)
    {
        if (Player == FPlayer && FDuration > 0)
        {
            FDuration = (sbyte)(FDuration - 1);
            if (FDuration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:228
    public void ShowRaiseTerrainAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        MapField Field;
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        Field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
        // { Original tests the base mountain ID, although this object is raised terrain. }
        if (this == Field.FindOwnedChild(131432) && Anim.FFrameCounter == 0)
        {
            bool condition = AoWHSMap.Current.FSeatedPlayer == Anim.FTag;
            if (!condition)
            {
                var lhx2 = GetLhx();
                var yhx2 = GetYhx();
                var xhx2 = GetXhx();
                condition = AoWHSMap.Current.WatchingTerrain(xhx2, yhx2, lhx2, 0);
            }

            if (condition)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(19).FCastSounds.Play(0);
            }
        }
    }

    // Mountain.pas:240
    public void NewFrameRaiseTerrainAnimation(Animation Anim, int FrameCounter)
    {
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        var field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
        if (field.FindOwnedChild(131434) == this && FrameCounter == 9)
        {
            FMode = FMode - Set<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.Raising);
        }
    }

    public new partial class Resource : ILTerrainMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Mountain.pas:249
        public new const int ClassID = 131435;

        // Mountain.pas:255
        public override int ResourceUserClassID() => 131434;

        // Mountain.pas:252
        public override string ResourceEditName() => AbilityRules.TranslateRStr(RaisedMountainRStr);

        private static readonly string RaisedMountainRStr = "Raised Mountain";

    }

}
