// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// ArmyView. Selection. Messages. Title. Text. Description. Triggers
public partial class ArmyInfoEventLog : EventLog
{
    // Army.PAS:255
    public ArmyInfoEventLog(ECustomNode owner) : base(owner)
    {
        Messages = new StringList();
        ArmyView = new ArmyView(null);
        Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event, ExecuteTrigger.User, ExecuteTrigger.NewTurn);
    }

    public new const int ClassId = 131817;

    public ArmyView ArmyView;
    public byte Selection;
    public StringCollection Messages;
    public string Title = "";
    public string Text = "";
    public string Description = "";
    public Set<ExecuteTrigger> Triggers;

    // Army.PAS:263
    public override void Dispose()
    {
        Messages?.Dispose();
        ArmyView?.Dispose();
        base.Dispose();
    }

    // Army.PAS:273
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(ArmyView, 15);
        stream.rwStrings(Messages, 16);
        stream.rwData(Bytes.Of(ref Selection, 1), 1, 17);
        stream.rwString(ref Title, 18);
        stream.rwString(ref Text, 19);
        stream.rwString(ref Description, 20);
    }

    // Army.PAS:287
    public override string GetText() => Text;

    // Army.PAS:297
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = false;
        if (Triggers.Contains(trigger))
        {
            switch (trigger)
            {
                case ExecuteTrigger.User:
                    AoWHSMap.Current.TriggerExecuteEventLog(this, false, done);
                    result = true;
                    break;
                case ExecuteTrigger.NewTurn:
                    if (AoWHSMap.Current.SeatedPlayer == _player || AoWHSMap.Current.SeatedPlayer == ArmyView.Player)
                    {
                        AoWHSMap.Current.TriggerExecuteEventLog(this, false, done);
                        result = true;
                    }

                    break;
                case ExecuteTrigger.Event:
                    if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local && (AoWHSMap.Current.SeatedPlayer == _player || AoWHSMap.Current.SeatedPlayer == ArmyView.Player))
                    {
                        AoWHSMap.Current.TriggerExecuteEventLog(this, false, done);
                        result = true;
                    }
                    else
                    {
                        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
                    }

                    break;
            }
        }

        return result;
    }

    // Army.PAS:290
    public override void ViewLocation()
    {
        ArmyView army;
        army = ArmyView;
        if (army != null && army.X != -1)
        {
            CenterViewTo(army.X, army.Y, army.MapLevel);
        }
    }

    // Army.PAS:284
    public void SetArmyView(ArmyView value)
    {
        ArmyView.Assign(value, GameTables.GameEngine);
    }

}
