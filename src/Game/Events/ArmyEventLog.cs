// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public ArmyEventLog(ECustomNode owner) : base(owner)
    {
        ArmyMessages = new StringList();
        ArmyView = new ArmyView(null);
        DamageValues = new ByteList();
        DamageTypes = new ByteList();
    }

    public new const int ClassId = 131608;

    public ArmyView ArmyView { get; }

    public string ArmyText = "";

    public StringCollection ArmyMessages { get; }

    public int ArmyIcon;

    public ByteList DamageValues { get; }
    public ByteList DamageTypes { get; }

    private bool _gameOver;

    // Army.PAS:150
    public override void Dispose()
    {
        ArmyMessages?.Dispose();
        ArmyView?.Dispose();
        DamageValues?.Dispose();
        DamageTypes?.Dispose();
        base.Dispose();
    }

    // Army.PAS:162
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(ArmyView, 15);
        stream.String(ref ArmyText, 16);
        stream.Lines(ArmyMessages, 17);
        stream.Int32(ref ArmyIcon, 18);
        stream.Payload(19, DamageValues.LoadFromStream, DamageValues.SaveToStream);
        stream.Payload(20, DamageTypes.LoadFromStream, DamageTypes.SaveToStream);
        stream.Boolean(ref _gameOver, 21);
    }

    // Army.PAS:184
    public override string GetText() => ArmyText;

    // Army.PAS:174
    public override int GetLeftIcon()
    {
        int result;
        if (ArmyView.Player > 0)
        {
            result = StrategicMap.Current.Players.GetPlayers(ArmyView.Player).PlayerRace;
        }
        else
        {
            result = 12;
        }

        return result;
    }

    // Army.PAS:218
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool showEvent = default;
        bool result = false;
        switch (trigger)
        {
            case ExecuteTrigger.User:
                StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                result = true;
                break;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                showEvent = false;
                if (StrategicMap.Current.SeatedPlayer == _player || StrategicMap.Current.SeatedPlayer == ArmyView.Player)
                {
                    showEvent = showEvent || StrategicMap.Current.PlayMode == PlayMode.Turn && GameTables.GameEngine.Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.PartyDamage);
                    showEvent = showEvent || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GameTables.GameEngine.Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.PartyDamage);
                }

                showEvent = showEvent || _gameOver;
                if (showEvent)
                {
                    StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                    result = true;
                }

                break;
        }

        return result;
    }

    // Army.PAS:248
    public override void ViewLocation()
    {
        ArmyView army;
        army = ArmyView;
        if (army != null && army.X != -1)
        {
            CenterViewTo(army.X, army.Y, army.MapLevel);
        }
    }

    // Army.PAS:181
    public void SetArmyView(ArmyView value)
    {
        ArmyView.Assign(value, GameTables.GameEngine);
    }

    // Army.PAS:187
    public bool LeaderKilled()
    {
        int I;
        bool result = false;
        I = ArmyView.GetCount();
        while (!result && I != 0)
        {
            I -= 1;
            result = ArmyView.GetUnit(I) is Leader && ArmyView.GetUnit(I).GetHitPoints() <= DamageValues[I];
        }

        return result;
    }

    // Army.PAS:200
    public bool PartyDestroyed()
    {
        int I;
        bool result = true;
        I = ArmyView.GetCount();
        while (result && I != 0)
        {
            I -= 1;
            result = result && ArmyView.GetUnit(I).GetHitPoints() <= DamageValues[I];
        }

        return result;
    }

    // Army.PAS:212
    public void ValidateGameOver()
    {
        _gameOver = LeaderKilled() || PartyDestroyed() && StrategicMap.Current.Players.GetPlayers(ArmyView.Player).Armies.GetCount() == 1;
    }

}
