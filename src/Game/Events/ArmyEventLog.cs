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

// FArmyView. FArmyText. FArmyMessages. FArmyIcon. FDamageValues. FDamageTypes. FGameOver
public partial class ArmyEventLog : EventLog
{
    // Army.PAS:141
    public ArmyEventLog(ECustomNode owner) : base(owner)
    {
        FArmyMessages = new StringList();
        FArmyView = new ArmyView(null);
        FDamageValues = new ByteList();
        FDamageTypes = new ByteList();
    }

    public new const int ClassID = 131608;

    public ArmyView FArmyView;
    public string FArmyText = "";
    public StringCollection FArmyMessages;
    public int FArmyIcon;
    public ByteList FDamageValues;
    public ByteList FDamageTypes;
    public bool FGameOver;

    // Army.PAS:150
    public override void Dispose()
    {
        FArmyMessages?.Dispose();
        FArmyView?.Dispose();
        FDamageValues?.Dispose();
        FDamageTypes?.Dispose();
        base.Dispose();
    }

    // Army.PAS:162
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FArmyView, 15);
        Stream.rwString(ref FArmyText, 16);
        Stream.rwStrings(FArmyMessages, 17);
        Stream.rwInteger(ref FArmyIcon, 18);
        Stream.rwStreamProc(19, FDamageValues.LoadFromStream, FDamageValues.SaveToStream);
        Stream.rwStreamProc(20, FDamageTypes.LoadFromStream, FDamageTypes.SaveToStream);
        Stream.rwBool(ref FGameOver, 21);
    }

    // Army.PAS:184
    public override string GetText() => FArmyText;

    // Army.PAS:174
    public override int GetLeftIcon()
    {
        int Result;
        if (FArmyView.FPlayer > 0)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FArmyView.FPlayer).FPlayerRace;
        }
        else
        {
            Result = 12;
        }

        return Result;
    }

    // Army.PAS:218
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool ShowEvent = default;
        bool Result = false;
        switch (Trigger)
        {
            case ExecuteTrigger.User:
                AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                Result = true;
                break;
            case ExecuteTrigger.Event:
            case ExecuteTrigger.NewTurn:
                ShowEvent = false;
                if (AoWHSMap.Current.FSeatedPlayer == FPlayer || AoWHSMap.Current.FSeatedPlayer == FArmyView.FPlayer)
                {
                    ShowEvent = ShowEvent || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.PartyDamage);
                    ShowEvent = ShowEvent || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.PartyDamage);
                }

                ShowEvent = ShowEvent || FGameOver;
                if (ShowEvent)
                {
                    AoWHSMap.Current.TriggerExecuteEventLog(this, false, Done);
                    Result = true;
                }

                break;
        }

        return Result;
    }

    // Army.PAS:248
    public override void ViewLocation()
    {
        ArmyView Army;
        Army = FArmyView;
        if (Army != null && Army.FX != -1)
        {
            CenterViewTo(Army.FX, Army.FY, Army.FL);
        }
    }

    // Army.PAS:181
    public void SetArmyView(ArmyView Value)
    {
        FArmyView.Assign(Value, GameTables.AoWEngine);
    }

    // Army.PAS:187
    public bool LeaderKilled()
    {
        int I;
        bool Result = false;
        I = FArmyView.GetCount();
        while (!Result && I != 0)
        {
            I -= 1;
            Result = FArmyView.GetUnit(I) is Leader && FArmyView.GetUnit(I).GetHitPoints() <= FDamageValues[I];
        }

        return Result;
    }

    // Army.PAS:200
    public bool PartyDestroyed()
    {
        int I;
        bool Result = true;
        I = FArmyView.GetCount();
        while (Result && I != 0)
        {
            I -= 1;
            Result = Result && FArmyView.GetUnit(I).GetHitPoints() <= FDamageValues[I];
        }

        return Result;
    }

    // Army.PAS:212
    public void ValidateGameOver()
    {
        FGameOver = LeaderKilled() || PartyDestroyed() && AoWHSMap.Current.FPlayers.GetPlayers(FArmyView.FPlayer).FArmies.GetCount() == 1;
    }

}
