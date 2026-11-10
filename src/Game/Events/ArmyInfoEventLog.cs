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

// FArmyView. FSelection. FMessages. FTitle. FText. FDescription. FTriggers
public partial class ArmyInfoEventLog : EventLog
{
    // Army.PAS:255
    public ArmyInfoEventLog(ECustomNode owner) : base(owner)
    {
        FMessages = new StringList();
        FArmyView = new ArmyView(null);
        FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event, ExecuteTrigger.User, ExecuteTrigger.NewTurn);
    }

    public new const int ClassID = 131817;

    public ArmyView FArmyView;
    public byte FSelection;
    public StringCollection FMessages;
    public string FTitle = "";
    public string FText = "";
    public string FDescription = "";
    public Set<ExecuteTrigger> FTriggers;

    // Army.PAS:263
    public override void Dispose()
    {
        FMessages?.Dispose();
        FArmyView?.Dispose();
        base.Dispose();
    }

    // Army.PAS:273
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FArmyView, 15);
        Stream.rwStrings(FMessages, 16);
        Stream.rwData(Bytes.Of(ref FSelection, 1), 1, 17);
        Stream.rwString(ref FTitle, 18);
        Stream.rwString(ref FText, 19);
        Stream.rwString(ref FDescription, 20);
    }

    // Army.PAS:287
    public override string GetText() => FText;

    // Army.PAS:297
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = false;
        if (FTriggers.Contains(Trigger))
        {
            switch (Trigger)
            {
                case ExecuteTrigger.User:
                    AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                    Result = true;
                    break;
                case ExecuteTrigger.NewTurn:
                    if (AoWHSMap.Current.FSeatedPlayer == FPlayer || AoWHSMap.Current.FSeatedPlayer == FArmyView.FPlayer)
                    {
                        AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                        Result = true;
                    }

                    break;
                case ExecuteTrigger.Event:
                    if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local && (AoWHSMap.Current.FSeatedPlayer == FPlayer || AoWHSMap.Current.FSeatedPlayer == FArmyView.FPlayer))
                    {
                        AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                        Result = true;
                    }
                    else
                    {
                        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWEVENT.WAV");
                    }

                    break;
            }
        }

        return Result;
    }

    // Army.PAS:290
    public override void ViewLocation()
    {
        ArmyView Army;
        Army = FArmyView;
        if (Army != null && Army.FX != -1)
        {
            CenterViewTo(Army.FX, Army.FY, Army.FL);
        }
    }

    // Army.PAS:284
    public void SetArmyView(ArmyView Value)
    {
        FArmyView.Assign(Value, GameTables.AoWEngine);
    }

}
