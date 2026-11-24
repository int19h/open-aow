#nullable disable warnings
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

// ArmyView. ArmyText. ArmyMessages. ArmyIcon. DamageValues. DamageTypes. GameOver
public partial class ArmyEventLog : EventLog
{
    // Army.PAS:141
    public ArmyEventLog(CustomNode? owner) : base(owner)
    {
        ArmyMessages = new DelphiStringList();
        ArmyView = new ArmyView(null) { Holder = CreationHolder() };
        DamageValues = [];
        DamageTypes = [];
    }

    public new const int ClassId = 131_608;

    public ArmyView ArmyView { get; }

    public string ArmyText { get; set; } = "";

    public DelphiStringCollection ArmyMessages { get; }

    public int ArmyIcon { get; set; }

    public ByteList DamageValues { get; }
    public ByteList DamageTypes { get; }

    private bool _gameOver;

    // Army.PAS:150
    public override void Dispose()
    {
        ArmyView?.Dispose();
        base.Dispose();
    }

    // Army.PAS:162
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("ArmyView", objectValue: ArmyView);
        ArmyText = persister.String("ArmyText", value: ArmyText);
        persister.Lines(name: "ArmyMessages", lines: ArmyMessages);
        ArmyIcon = persister.Int32("ArmyIcon", value: ArmyIcon);
        persister.ByteList(name: "DamageValues", list: DamageValues);
        persister.ByteList(name: "DamageTypes", list: DamageTypes);
        _gameOver = persister.Boolean("GameOver", value: _gameOver);
    }

    // Army.PAS:184
    public override string GetText() => ArmyText;

    // Army.PAS:174
    public override int LeftIcon => ArmyView.Player > 0 ? StrategicMap.Current.Players.GetPlayers(ArmyView.Player).PlayerRace : 12;

    // Army.PAS:218
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent? done)
    {
        switch (trigger)
        {
            case ExecuteTrigger.User:
                StrategicMap.Current.TriggerExecuteEventLog(this, immediate: false, done);
                return true;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                bool showEvent = false;
                if (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.SeatedPlayer == ArmyView.Player)
                {
                    showEvent = showEvent || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.PartyDamage));
                    showEvent = showEvent || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.PartyDamage));
                }

                showEvent = showEvent || _gameOver;
                if (showEvent)
                {
                    StrategicMap.Current.TriggerExecuteEventLog(this, immediate: false, done);
                    return true;
                }

                break;
            default:
                break;
        }

        return false;
    }

    // Army.PAS:248
    public override void ViewLocation()
    {
        ArmyView army = ArmyView;
        if (army is not null && army.Location.X != -1)
        {
            CenterViewTo(army.Location.X, army.Location.Y, L: army.Location.MapLevel);
        }
    }

    // Army.PAS:181
    public void SetArmyView(ArmyView value) => ArmyView.Assign(value, engine: GetEngine());

    // Army.PAS:187
    public bool LeaderKilled()
    {
        bool result = false;
        int i = ArmyView.Units.Count;
        while (!result && i != 0)
        {
            i -= 1;
            result = ArmyView.Units.GetUnit(i) is Leader && ArmyView.Units.GetUnit(i).GetHitPoints() <= DamageValues[i];
        }

        return result;
    }

    // Army.PAS:200
    public bool PartyDestroyed()
    {
        bool result = true;
        int i = ArmyView.Units.Count;
        while (result && i != 0)
        {
            i -= 1;
            result = result && ArmyView.Units.GetUnit(i).GetHitPoints() <= DamageValues[i];
        }

        return result;
    }

    // Army.PAS:212
    public void ValidateGameOver() => _gameOver = LeaderKilled() || (PartyDestroyed() && StrategicMap.Current.Players.GetPlayers(ArmyView.Player).Armies.GetCount() == 1);

}
