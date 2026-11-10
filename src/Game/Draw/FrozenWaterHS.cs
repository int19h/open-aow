// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw;

// signed days remaining, caster player
public partial class FrozenWaterHS : SingleHS
{
    public FrozenWaterHS(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131436;

    public sbyte FDaysRemaining;
    public sbyte FCasterPlayer;

    // Structur.PAS:4487
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwShortint(ref FDaysRemaining, 30);
        Stream.rwShortint(ref FCasterPlayer, 31);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.SByte("DaysRemaining", ref FDaysRemaining);
        persister.SByte("CasterPlayer", ref FCasterPlayer);
    }

    // Structur.PAS:4518
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
        }
    }

    // Structur.PAS:4482
    public override byte GetLevel() => 28;

    // Structur.PAS:4503
    public override void TerrainChanged(in HSTerrainInfo Info)
    {
        base.TerrainChanged(Info);
        if (Info.HN == 0 && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)Info.TerrainType))
        {
            Dispose();
        }
    }

    // Structur.PAS:4524
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        var yhx = Yhx;
        HexGeometry.HXtoHP(Xhx, yhx, ref X, ref Y);
        X = X - XhpSurf + 16;
        Y = Y - YhpSurf + 16;
        var xhx = Xhx;
        var yhx2 = Yhx;
        var yhx3 = Yhx;
        AoWHSSet.Current.FImageSequences.ShowLoopedEx(70, X, Y, AoWHSMap.Current.Frame, xhx + yhx2 + yhx3 * 2, SurfaceDesc);
    }

    // Structur.PAS:4494
    public void MeltIce()
    {
        if (((MapField)Owner).TerrainType == 6)
        {
            var yhx = Yhx;
            var xhx = Xhx;
            AoWHSMap.Current.ChangeTerrain(this, xhx, yhx, Lhx, 0, 0);
        }
        else
        {
            if (((MapField)Owner).TerrainType == 13)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                AoWHSMap.Current.ChangeTerrain(this, xhx, yhx, Lhx, 0, 10);
            }
            else
            {
                Dispose();
            }
        }
    }

    // Structur.PAS:4509
    public void NewTurn(sbyte Player)
    {
        if (Player == FCasterPlayer)
        {
            FDaysRemaining = (sbyte)(FDaysRemaining - 1);
            if (FDaysRemaining <= 0)
            {
                MeltIce();
            }
        }
    }

    // Engine.pas:524

}
