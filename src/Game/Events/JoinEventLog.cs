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
public partial class JoinEventLog : EventLog
{
    // Diplomacy.pas:1
    public JoinEventLog(ECustomNode owner) : base(owner)
    {
        UnitIds = new Int32List();
        JoinMessage = new StringList();
        _location = new MapCoordinate(X: -1, Y: -1, MapLevel: -1);
        JoinIndex = -1;
    }

    public int FirstUnitId { get; set; }

    public Int32List UnitIds { get; }

    private MapCoordinate _location;

    // The fourth byte of the packed location stored at id 26, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    public byte Selection { get; set; }
    public int JoinIndex { get; set; }

    public StringList JoinMessage { get; }

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
        PersistSupport.Int32List(persister, name: "UnitIds", list: UnitIds);
        Selection = persister.Byte("Selection", value: Selection);
        PersistSupport.Lines(persister, name: "JoinMessage", lines: JoinMessage);
        Gold = persister.Int32("Gold", value: Gold);
        int location = StoredCoordinateImage.From(_location, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("Location", value: location);
        (_location, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
        JoinMode = persister.Enum("JoinMode", value: JoinMode);
        Text = persister.String("Text", value: Text);
        JoinIndex = persister.Int32("JoinIndex", value: JoinIndex, fallback: -1);
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
            troop = StrategicMap.Current.UnitControl.FindUnit(item);
            // { Original behavior: missing and dead units do not invalidate the offer. }
            if (troop != null && troop.Player == 0)
            {
                troop.GetLocation(out var locationX, out var locationY, out var locationMapLevel);
                location = new MapCoordinate(X: locationX, Y: locationY, MapLevel: locationMapLevel);
                result = _location.X == location.X && _location.Y == location.Y && _location.MapLevel == location.MapLevel;
            }
        }

        return result;
    }

    // Diplomacy.pas:80
    public virtual void ListJoinUnits(UnitList units, out byte selection)
    {
        Troop troop;
        for (int I = 0, last = UnitIds.Count - 1; I <= last; I++)
        {
            var item = UnitIds[I];
            troop = StrategicMap.Current.UnitControl.FindUnit(item);
            units.AddUnit(troop);
        }

        selection = Selection;
    }

    // Diplomacy.pas:93
    public virtual bool Accept(ref string errorText)
    {
        bool result;
        if (StrategicMap.Current.Players.GetPlayers(_player).Gems >= Gold)
        {
            result = true;
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GetEngine(), GameText.TributeNotEnoughGoldText);
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
        for (int I = 0, last = army.Units.Count - 1; I <= last; I++)
        {
            if (FirstUnitId == 0 && ((1 << I) & Selection) != 0)
            {
                FirstUnitId = army.Units.GetUnit(I).UnitId;
                var selectionCount = army.SelectionCount(Selection);
                army.Units.GetUnit(I).GetJoinMessage(player, selectionCount - 1, Gold, joinMode, JoinMessage);
            }

            var troop = army.Units.GetUnit(I);
            UnitIds.Add(troop.UnitId);
        }

        _location = HexGeometry.PackMapCoordinate(army.X, army.Y, army.MapLevel);
    }

}
