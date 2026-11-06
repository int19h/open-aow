using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.AoWReg;
using System.Runtime.CompilerServices;

namespace Aow.Units.AoWE;

public partial class TArmyID
{
    public override void ReadWrite(TEStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 10);
        int location = (byte)FX | ((byte)FY << 8) | ((byte)FL << 16) | (F_ArmyID_13 << 24);
        stream.rwInteger(ref location, 11);
        FX = (sbyte)location;
        FY = (sbyte)(location >> 8);
        FL = (sbyte)(location >> 16);
        F_ArmyID_13 = (byte)(location >> 24);
        stream.rwStreamProc(12, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
    }
}

public partial class TCombatPredictorUnit
{
    public void Setup(TAbstractUnit unit, TCombatPredictor predictor, byte side)
    {
        FUnit = unit;
        unit.AddRef();
        FPredictor = predictor;
        FCPUID = predictor.CreateUniqueCPUID();
        FSide = side;
        FInitialHitPoints = unit.GetHitPoints() * 100;
        FEnabled = 1;
        FTargetsInvalid = 1;
        FWallTargetsInvalid = 1;
        FOpponents = predictor.FSides[side == 0 ? 1 : 0];
    }
}

public partial class TWallCrushingAbility
{
    // AoWAb.PAS:2081
    public override bool fcExecuteCombatCommand(TCombatObject source, TFastCombatCommand command)
    {
        var target = command.FTarget;
        if (target.GetEnabled())
        {
            var action = AoWEUnit.CreateDamageCA(source, target, 6, 6, TSet<TDamageType>.Of(TDamageType.dtWall));
            source.FCombat.ExecuteCombatAction(action);
            action.Release();
        }
        return true;
    }
}

public static partial class AoWEUnit
{
    public static bool P5DiplomaticExecute(TDiplomaticActionEventLog log, bool immediate, TExecuteTrigger trigger, object data, TExecuteDoneEvent done)
    {
        bool result = trigger == TExecuteTrigger.etUser;
        if (log.FAction.FTarget == AoWHSMap.FSeatedPlayer || log.FAction.FSource == AoWHSMap.FSeatedPlayer)
        {
            result = result || AoWHSMap.FPlayMode == TPlayMode.pmTurn && AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(TWindowPopupEvent.wpDiplomacy) && trigger == TExecuteTrigger.etNewTurn;
            result = result || AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous && AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(TWindowPopupEvent.wpDiplomacy);
        }
        result = result && (trigger == TExecuteTrigger.etUser || log.FAction.FReaction != TDiplomaticActionReaction.darIgnore);
        if (!result) return false;
        AoWHSMap.TriggerExecuteEventLog(log, immediate, done);
        return true;
    }
}
