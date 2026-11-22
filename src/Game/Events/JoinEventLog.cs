#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Events;

// { Interface declaration run owned by P5. }
// FirstUnitID, UnitIDs, Location, Selection, JoinIndex, JoinMessage, Gold, JoinMode, Text
public class JoinEventLog(CustomNode owner) : EventLog(owner)
{

    public int FirstUnitId { get; set; }

    public Int32List UnitIds { get; } = [];

    private MapCoordinate _location = new(-1, -1, -1);

    public int Selection { get; set; }
    public int JoinIndex { get; set; } = -1;

    public DelphiStringList JoinMessage { get; } = [];

    public int Gold { get; set; }
    public JoinMode JoinMode { get; set; }
    public string Text { get; set; } = "";

    // Diplomacy.pas:12
    public override void Dispose()
    {
        UnitIds?.Dispose();
        JoinMessage?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:19
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        FirstUnitId = persister.Int32("FirstUnitId", value: FirstUnitId);
        persister.Int32List(name: "UnitIds", list: UnitIds);
        Selection = persister.Byte("Selection", value: Selection.TruncateToByte());
        persister.Lines(name: "JoinMessage", lines: JoinMessage);
        Gold = persister.Int32("Gold", value: Gold);
        int location = persister.Int32("Location", value: StoredCoordinateImage.From(_location).ToInt32());
        _location = StoredCoordinateImage.FromInt32(location).Location;
        JoinMode = persister.Enum("JoinMode", value: JoinMode);
        Text = persister.String("Text", value: Text);
        JoinIndex = persister.Int32("JoinIndex", value: JoinIndex, fallback: -1);
    }

    // Diplomacy.pas:58
    public virtual bool Validate()
    {
        bool result = true;
        int i = UnitIds.Count;
        while (result && i != 0)
        {
            i -= 1;
            int item = UnitIds[i];
            Troop troop = StrategicMap.Current.UnitControl.FindUnit(item);
            // { Original behavior: missing and dead units do not invalidate the offer. }
            if (troop is not null && troop.Player == 0)
            {
                MapCoordinate coordinate20 = troop.GetLocation();
                int locationX = coordinate20.X;
                int locationY = coordinate20.Y;
                int locationMapLevel = coordinate20.MapLevel;
                MapCoordinate location = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
                result = _location.X == location.X && _location.Y == location.Y && _location.MapLevel == location.MapLevel;
            }
        }

        return result;
    }

    // Diplomacy.pas:80
    public virtual int ListJoinUnits(UnitUserList units)
    {
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            int item = UnitIds[i];
            Troop troop = StrategicMap.Current.UnitControl.FindUnit(item);
            _ = units.AddUnit(troop);
        }

        int selection = Selection;
        return selection;
    }

    // Diplomacy.pas:93
    public virtual bool Accept(ref string errorText)
    {
        if (StrategicMap.Current.Players.GetPlayers(Player).Gems >= Gold)
        {
            return true;
        }
        else
        {
            errorText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TributeNotEnoughGoldText);
        }

        return false;
    }

    // Diplomacy.pas:105
    public virtual bool Cancel() => true;

    // Diplomacy.pas:110
    public virtual bool Reject(ref string errorText) => true;

    // Diplomacy.pas:33
    public void SetupJoinArmy(Army army, sbyte player, JoinMode joinMode)
    {
        FirstUnitId = 0;
        Player = (sbyte)(byte)player;
        Selection = army.GetJoinSelection(player);
        Gold = army.JoinAmount(player, Selection);
        JoinMode = joinMode;
        UnitIds.Clear();
        for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
        {
            if (FirstUnitId == 0 && ((1 << i) & Selection) != 0)
            {
                FirstUnitId = army.Units.GetUnit(i).UnitId;
                int selectionCount = army.SelectionCount(Selection);
                army.Units.GetUnit(i).GetJoinMessage(player, count: selectionCount - 1, amount: Gold, mode: joinMode, messages: JoinMessage);
            }

            Troop troop = army.Units.GetUnit(i);
            _ = UnitIds.Add(troop.UnitId);
        }

        _location = new MapCoordinate(army.Location.X, army.Location.Y, army.Location.MapLevel);
    }
}
