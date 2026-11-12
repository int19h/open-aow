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
        UnitIds = new Int32List();
        JoinMessage = new StringList();
        Location.X = -1;
        Location.Y = -1;
        Location.MapLevel = -1;
        JoinIndex = -1;
    }

    public int FirstUnitId;
    public Int32List UnitIds;
    public MapCoordinate Location;
    public byte Selection;
    public int JoinIndex;
    public StringList JoinMessage;
    public int Gold;
    public JoinMode JoinMode;
    public string Text = "";

    // Diplomacy.pas:12
    public override void Dispose()
    {
        UnitIds?.Dispose();
        JoinMessage?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:19
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref FirstUnitId, 21);
        stream.rwStreamProc(22, UnitIds.LoadFromStream, UnitIds.SaveToStream);
        stream.rwByte(ref Selection, 23);
        stream.rwStrings(JoinMessage, 24);
        stream.rwInteger(ref Gold, 25);
        stream.rwInteger(ref Unsafe.As<MapCoordinate, int>(ref Location), 26);
        stream.rwData(Bytes.Of(ref JoinMode, 1), 1, 27);
        stream.rwString(ref Text, 28);
        stream.rwIntegerEx(ref JoinIndex, -1, 29);
    }

    // Diplomacy.pas:58
    public virtual bool Validate()
    {
        int I;
        Troop troop;
        MapCoordinate location = default;
        bool result = true;
        I = UnitIds.Count;
        while (result && I != 0)
        {
            I -= 1;
            var item = UnitIds[I];
            troop = AoWHSMap.Current.UnitControl.FindUnit(item);
            // { Original behavior: missing and dead units do not invalidate the offer. }
            if (troop != null && troop.Player == 0)
            {
                troop.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
                result = Location.X == location.X && Location.Y == location.Y && Location.MapLevel == location.MapLevel;
            }
        }

        return result;
    }

    // Diplomacy.pas:80
    public virtual void ListJoinUnits(UnitList units, ref byte selection)
    {
        Troop troop;
        for (int I = 0, last = UnitIds.Count - 1; I <= last; I++)
        {
            var item = UnitIds[I];
            troop = AoWHSMap.Current.UnitControl.FindUnit(item);
            units.AddUnit(troop);
        }

        selection = Selection;
    }

    // Diplomacy.pas:93
    public virtual bool Accept(ref string errorText)
    {
        bool result;
        if (AoWHSMap.Current.Players.GetPlayers(_player).Gems >= Gold)
        {
            result = true;
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GameText.s_tributeNotEnoughGoldText);
        }

        return result;
    }

    // Diplomacy.pas:105
    public virtual bool Cancel() => true;

    // Diplomacy.pas:110
    public virtual bool Reject(ref string errorText) => true;

    // Diplomacy.pas:33
    public void SetupJoinArmy(Army army, sbyte player, JoinMode joinMode)
    {
        FirstUnitId = 0;
        _player = (sbyte)(byte)player;
        Selection = army.GetJoinSelection(player);
        Gold = army.JoinAmount(player, Selection);
        JoinMode = joinMode;
        UnitIds.Clear();
        for (int I = 0, last = army.Count - 1; I <= last; I++)
        {
            if (FirstUnitId == 0 && ((1 << I) & Selection) != 0)
            {
                FirstUnitId = army.GetUnit(I).UnitId;
                var selectionCount = army.SelectionCount(Selection);
                army.GetUnit(I).GetJoinMessage(player, selectionCount - 1, Gold, joinMode, JoinMessage);
            }

            var troop = army.GetUnit(I);
            UnitIds.Add(troop.UnitId);
        }

        Location = HexGeometry.PackMapCoordinate(army.X, army.Y, army.MapLevel);
    }

}
