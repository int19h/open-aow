// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

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
        ArmyView = new ArmyView(null) { Holder = CreationHolder() };
        Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event, ExecuteTrigger.User, ExecuteTrigger.NewTurn);
    }

    public new const int ClassId = 131817;

    public ArmyView ArmyView { get; }

    public byte Selection;

    public StringCollection Messages { get; }

    public string Title = "";
    public string Text = "";
    public string Description = "";

    public Set<ExecuteTrigger> Triggers { get; set; }

    // Army.PAS:263
    public override void Dispose()
    {
        Messages?.Dispose();
        ArmyView?.Dispose();
        base.Dispose();
    }

    // Army.PAS:273
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(ArmyView, 15);
        stream.Lines(Messages, 16);
        stream.Bytes(Bytes.Of(ref Selection, 1), 1, 17);
        stream.String(ref Title, 18);
        stream.String(ref Text, 19);
        stream.String(ref Description, 20);
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
                    StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                    result = true;
                    break;
                case ExecuteTrigger.NewTurn:
                    if (StrategicMap.Current.SeatedPlayer == _player || StrategicMap.Current.SeatedPlayer == ArmyView.Player)
                    {
                        StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
                        result = true;
                    }

                    break;
                case ExecuteTrigger.Event:
                    if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && (StrategicMap.Current.SeatedPlayer == _player || StrategicMap.Current.SeatedPlayer == ArmyView.Player))
                    {
                        StrategicMap.Current.TriggerExecuteEventLog(this, false, done);
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
        ArmyView.Assign(value, GetEngine());
    }

}
