// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// FPositions: packed XYL entries, eight per army position
public partial class UnitPositionControl : EObject
{
    public XYLList FPositions;

    // AoWTC.pas:4943
    public override void Dispose()
    {
        FPositions?.Dispose();
        FPositions = null;
        base.Dispose();
    }

    // AoWTC.pas:4936
    public UnitPositionControl(ECustomNode owner) : base(owner)
    {
        FPositions = new XYLList();
        var packXYL = HexGeometry.PackXYL(-1, -1, -1);
        FPositions.Default = packXYL;
    }

    // AoWTC.pas:4990
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        UnitPositionHS HS;
        int X = default;
        int Y = default;
        int L;
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65541)
        {
            L = Msg.Data[1];
            HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref X, ref Y);
            if (AoWCombatMap.Current.MapContainer.GetField(X, Y, L).TerrainType != 15)
            {
                HS = (UnitPositionHS)AoWCombatMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(2228483);
                if (HS == null)
                {
                    HS = new UnitPositionHS(null);
                    if (!AoWCombatMap.Current.PlaceHX(HS, X, Y, L))
                    {
                        HS?.Dispose();
                        HS = null;
                    }
                }

                if (HS != null)
                {
                }
            }
        }
    }

    // AoWTC.pas:4950
    public XYL GetUnitPosition(int Position, int Index)
    {
        XYL Result = default;
        int I;
        I = Position * 8 + Index;
        if (I < FPositions.Count)
        {
            Result = FPositions[I];
        }
        else
        {
            Result.X = -1;
            Result.Y = -1;
            Result.L = -1;
        }

        return Result;
    }

    // AoWTC.pas:4963
    public void SetUnitPosition(int Position, int Index, XYL Value)
    {
        if (Position < 7 && Index < 8)
        {
            FPositions[Position * 8 + Index] = Value;
        }
    }

    // AoWTC.pas:4969
    public bool ValidatePosition(ref sbyte Position, ref sbyte Index)
    {
        bool Result = FPositions[Position * 8 + Index].X == -1;
        if (!Result)
        {
            Index = 0;
            while (FPositions[Position * 8 + Index].X != -1 && Index != 8)
            {
                Index = (sbyte)(Index + 1);
            }

            Result = Index != 8;
        }

        if (!Result)
        {
            Position = 0;
            while (!Result && Position < 7)
            {
                while (FPositions[Position * 8 + Index].X != -1 && Index != 8)
                {
                    Index = (sbyte)(Index + 1);
                }

                Result = Index != 8;
                if (!Result)
                {
                    Index = 0;
                    Position = (sbyte)(Position + 1);
                }
            }
        }

        return Result;
    }

}
