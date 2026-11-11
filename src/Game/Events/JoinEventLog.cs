// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// { Interface declaration run owned by P5. }
// FirstUnitID, UnitIDs, Location, Selection, JoinIndex, JoinMessage, Gold, JoinMode, Text
public partial class JoinEventLog : EventLog
{
    // Diplomacy.pas:1
    public JoinEventLog(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new Int32List();
        FJoinMessage = new StringList();
        FLocation.X = -1;
        FLocation.Y = -1;
        FLocation.L = -1;
        FJoinIndex = -1;
    }

    public int FFirstUnitID;
    public Int32List FUnitIDs;
    public MapCoordinate FLocation;
    public byte FSelection;
    public int FJoinIndex;
    public StringList FJoinMessage;
    public int FGold;
    public JoinMode FJoinMode;
    public string FText = "";

    // Diplomacy.pas:12
    public override void Dispose()
    {
        FUnitIDs?.Dispose();
        FJoinMessage?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:19
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FFirstUnitID, 21);
        Stream.rwStreamProc(22, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        Stream.rwByte(ref FSelection, 23);
        Stream.rwStrings(FJoinMessage, 24);
        Stream.rwInteger(ref FGold, 25);
        Stream.rwInteger(ref Unsafe.As<MapCoordinate, int>(ref FLocation), 26);
        Stream.rwData(Bytes.Of(ref FJoinMode, 1), 1, 27);
        Stream.rwString(ref FText, 28);
        Stream.rwIntegerEx(ref FJoinIndex, -1, 29);
    }

    // Diplomacy.pas:58
    public virtual bool Validate()
    {
        int I;
        Troop AUnit;
        MapCoordinate Location = default;
        bool Result = true;
        I = FUnitIDs.Count;
        while (Result && I != 0)
        {
            I -= 1;
            var item = FUnitIDs[I];
            AUnit = AoWHSMap.Current.FUnitControl.FindUnit(item);
            // { Original behavior: missing and dead units do not invalidate the offer. }
            if (AUnit != null && AUnit.FPlayer == 0)
            {
                AUnit.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
                Result = FLocation.X == Location.X && FLocation.Y == Location.Y && FLocation.L == Location.L;
            }
        }

        return Result;
    }

    // Diplomacy.pas:80
    public virtual void ListJoinUnits(UnitList Units, ref byte Selection)
    {
        Troop AUnit;
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            var item = FUnitIDs[I];
            AUnit = AoWHSMap.Current.FUnitControl.FindUnit(item);
            Units.AddUnit(AUnit);
        }

        Selection = FSelection;
    }

    // Diplomacy.pas:93
    public virtual bool Accept(ref string ErrorText)
    {
        bool Result;
        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGems >= FGold)
        {
            Result = true;
        }
        else
        {
            Result = false;
            ErrorText = AbilityRules.TranslateRStr(GameText.P5NotEnoughGoldRStr);
        }

        return Result;
    }

    // Diplomacy.pas:105
    public virtual bool Cancel() => true;

    // Diplomacy.pas:110
    public virtual bool Reject(ref string ErrorText) => true;

    // Diplomacy.pas:33
    public void SetupJoinArmy(Army Army, sbyte Player, JoinMode JoinMode)
    {
        FFirstUnitID = 0;
        FPlayer = (sbyte)(byte)Player;
        FSelection = Army.GetJoinSelection(Player);
        FGold = Army.JoinAmount(Player, FSelection);
        FJoinMode = JoinMode;
        FUnitIDs.Clear();
        for (int I = 0, last = Army.Count - 1; I <= last; I++)
        {
            if (FFirstUnitID == 0 && ((1 << I) & FSelection) != 0)
            {
                FFirstUnitID = Army.GetUnit(I).FUnitID;
                var selectionCount = Army.SelectionCount(FSelection);
                Army.GetUnit(I).GetJoinMessage(Player, selectionCount - 1, FGold, JoinMode, FJoinMessage);
            }

            var unit = Army.GetUnit(I);
            FUnitIDs.Add(unit.FUnitID);
        }

        FLocation = HexGeometry.PackXYL(Army.FX, Army.FY, Army.FL);
    }

}
