using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.EventLog;

namespace Aow.Units.AoWE;

public partial class TRaceList
{
    // TEObject.ReadWrite is empty. The original bypasses TEControlStyleNode.ReadWrite.
    public override void ReadWrite(TEStorageStream stream) => stream.rwInteger(ref FRelationOffset, 20);
}

public partial class TUnitSelector
{
    public virtual bool Select() { FOnSelect?.Invoke(this); return true; }
}

public partial class TCustomAbilityList
{
    public virtual void SetAbSet(int index, byte value)
    {
        if ((uint)index >= (uint)FAbilityCount) SetAbCount(unchecked(index + 1));
        var bits = FAbilityBits;
        byte mask = (byte)(1 << (index & 7));
        if (value != 0) bits[index >> 3] |= mask;
        else bits[index >> 3] &= (byte)~mask;
    }
    public virtual bool GetAbSet(int index) => (uint)index < (uint)FAbilityCount && (FAbilityBits[index >> 3] & (1 << (index & 7))) != 0;
}

public partial class TAoWMapField
{
    public bool GetExplored(int player)
    {
        int count = player & 31;
        return count == 0 || ((uint)FFieldFlags & (1u << (count - 1))) != 0;
    }
    public void SetExplored(int player, bool value) => SetExploredRaw(player, value ? 1 : 0);
    // The original tests ECX, including the bits above the Boolean byte.
    public void SetExploredRaw(int player, int value)
    {
        int mask = 1 << ((player - 1) & 31);
        if (value != 0) FFieldFlags |= mask;
        else FFieldFlags &= ~mask;
    }
}

public partial class TAbility
{
    public virtual bool CanExpand(TAbilityOwner owner) => !owner.GetAbSet(FAbilityID) && (owner.GetAbilitySelectionTypes() & FSelectionTypes) != default(TSet<TAbilitySelectionType>);
    public virtual int ExpandCost(TAbilityOwner owner) => FSkillPointCost;
    public virtual bool Remove(TAbilityOwner owner)
    {
        if (!owner.GetAbSet(FAbilityID)) return false;
        owner.SetAbSet(FAbilityID, 0);
        return true;
    }
    public virtual bool GetInherent(TAbilityOwner owner) => owner.GetAbSet(FAbilityID);
    public virtual int GetSkillPoints(TAbilityOwner owner) => owner.GetAbSet(FAbilityID) ? FSkillPointCost : 0;
    public virtual bool GetEnabled(TAbilityOwner owner) => owner.GetAbSet(FAbilityID);
}

public partial class TDurationAbility
{
    public override bool Remove(TAbilityOwner owner)
    {
        var data = owner.GetAbilityData(FAbilityID);
        SetAb(owner, FAbilityID, 0);
        if (data == null) return false;
        owner.RemoveAbilityData(data);
        return true;
    }
}

public partial class TMultiLevelAbility
{
    public override int GetLevel(TAbilityOwner owner) => owner == null ? 0 : ((TMultiLevelAbilityData)owner.GetAbilityData(FAbilityID))?.FLevel ?? 0;
    public override int GetInherentLevel(TAbilityOwner owner) => owner == null ? 0 : ((TMultiLevelAbilityData)owner.GetAbilityData(FAbilityID))?.FLevel ?? 0;
    public override AnsiString GetName(TAbilityOwner owner) => GetLevelName(GetLevel(owner));
    public override AnsiString ExpandName(TAbilityOwner owner) => GetLevelName(unchecked(GetLevel(owner) + 1));
    public override int ExpandCost(TAbilityOwner owner)
    {
        int level = GetInherentLevel(owner);
        return level >= FMaxLevel ? 0 : FLevelCosts.Get(level + 1);
    }
    public override bool CanExpand(TAbilityOwner owner) => (owner.GetAbilitySelectionTypes() & FSelectionTypes) != default(TSet<TAbilitySelectionType>) && GetInherentLevel(owner) < FMaxLevel;
    public override int GetSkillPoints(TAbilityOwner owner)
    {
        int level = GetInherentLevel(owner), sum = 0;
        // The original reads the current level cost on every iteration.
        for (int i = level; i >= 1; --i) sum = unchecked(sum + FLevelCosts.Get(level));
        return sum;
    }
    public override bool Expand(TAbilityOwner owner)
    {
        var data = (TMultiLevelAbilityData)owner.GetAbilityData(FAbilityID);
        if (data == null)
        {
            data = (TMultiLevelAbilityData)ClassType.AbilityDataClass().Create(null);
            data.FLevelAbilityID = unchecked((ushort)FAbilityID);
            data.FLevel = 1;
            SetAb(owner, FAbilityID, 1);
            owner.AddAbilityData(data);
            return true;
        }
        if (data.FLevel == FMaxLevel) return false;
        data.FLevel = unchecked((byte)(data.FLevel + 1));
        return true;
    }
    public override bool Remove(TAbilityOwner owner)
    {
        var data = (TMultiLevelAbilityData)owner.GetAbilityData(FAbilityID);
        if (data == null) { SetAb(owner, FAbilityID, 0); return false; }
        if (data.FLevel == 1) { SetAb(owner, FAbilityID, 0); owner.RemoveAbilityData(data); }
        else data.FLevel = unchecked((byte)(data.FLevel - 1));
        return true;
    }
}

public partial class TCombatEventLog
{
    public override void ViewLocation()
    {
        var data = FLogbook.FFinalData;
        CenterViewTo(unchecked((sbyte)data.FX), unchecked((sbyte)data.FY), unchecked((sbyte)data.FL));
    }
}
public partial class TDamageEffectCA
{
    public override bool Play(TCombatViewer viewer)
    {
        if (viewer.FDelay * 2 == viewer.FTime) Execute(viewer.FCombat);
        return viewer.FDelay * 2 > viewer.FTime;
    }
}
public partial class TUpdateHeaderCA { public override bool Play(TCombatViewer viewer) { Execute(viewer.FCombat); return false; } }
public partial class TNewCombatRoundCA { public override bool Play(TCombatViewer viewer) { if (viewer.FTime == 0) Execute(viewer.FCombat); return viewer.FDelay > viewer.FTime; } }
public partial class TCombatObjectTurnDoneCA { public override bool Play(TCombatViewer viewer) { if (viewer.FTime == 0) Execute(viewer.FCombat); return viewer.FDelay > viewer.FTime; } }
public partial class TMoveWallCA { public override bool Play(TCombatViewer viewer) { Execute(viewer.FCombat); return false; } }
public partial class TFleeCA { public override bool Play(TCombatViewer viewer) { Execute(viewer.FCombat); return false; } }
public partial class TNegotiateRequestEventLog
{
    public override bool Execute(bool mode, TExecuteTrigger trigger, TExecuteDoneEvent done) => (byte)trigger == 0 && AoWE.AoWHSMap.FSeatedPlayer == FPlayer && base.Execute(mode, trigger, done);
}
public partial class TDiplomaticActionEventLog
{
    public override int GetMiddleIcon() => (byte)FAction.FActionType == 0 || (byte)FAction.FActionType == 9 && (byte)FAction.FReaction == 1 ? 0x15 : 0x16;
}

public partial class TSpell
{
    public virtual bool CanActivateCombat(TAbstractUnit source, ref AnsiString reason) => AoWE.AoWHSMap.FCombat != null;
    public virtual void GetCombatInfo(TAbstractUnit source, ref TSpellCombatInfo info)
    {
        info.Attack = 0; info.Damage = 0; info.DamageTypes = default;
        info.RepeatCount = 1; info.Range = 0; info.Radius = 0; info.TargetTypes = default;
    }
    public virtual TAICastSpellAction CreateCastSpellAction()
    {
        var action = new TAICastSpellAction(null);
        // TEObject.Create only calls SetOwner. It bypasses descendant constructor bodies.
        try { action.SetOwner(null); } catch { action.Destroy(); throw; }
        return action;
    }
    public virtual void SetupCastSpellAction(TAbstractUnit source, TAICastSpellAction action)
    { action.FSpellID = FSpellID; action.FSourceUnitID = source.FUnitID; }
}
public partial class TSpellTE
{
    public TAbstractUnit GetSource() => AoWE.AoWHSMap.FUnitControl.FindUnit(FEventSourceID);
    public virtual bool Validate(THero source)
    {
        AnsiString reason = default;
        try { return unchecked((byte)source.FPlayer) == FEventPlayerIndex && source.CanCastSpellInstantly(FEventSpellID, FEventMana, ref reason); }
        finally { reason = default; }
    }
    public virtual bool Process(TAbstractUnit source, int value) => AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).ExecuteTE(this, source, value);
    public virtual void PlayDefaultCastEffects(int unused1, int unused2)
    {
        sbyte x = 0, y = 0, l = 0;
        GetSource().GetLocation(ref x, ref y, ref l);
        var spell = AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
        spell.PlayDefaultCastEffects(GetSource().FPlayer, x, y, l);
    }
}
public partial class TSpellCaster
{
    public TAbstractUnit GetSource() => AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
    public virtual bool CanCast(ref AnsiString reason) => AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID) != null;
}
public partial class TUnitSpellCaster
{
    public void SetSelection(byte selection)
    {
        byte value = selection;
        var source = AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
        ((TUnitSpell)AoWE.AoWHSSet.FSpellControl.GetSpell(FCasterSpellID)).ValidateUnitSelection(source, FTargetUnits, ref value);
        FSelectionMask = value;
    }
}
public partial class TExternalPowerSource { public override int Power() => FExternalPowerControl.FExternalPower; }
public partial class TAbstractUnit
{
    public virtual bool GetAbilitySet(int index) => (uint)index < (uint)FAbilityCount && (FAbilityBits[index >> 3] & (1 << (index & 7))) != 0;
    public override bool RemoveAbility(int ability)
    {
        AddRef();
        bool result = base.RemoveAbility(ability);
        Release();
        return result;
    }
}
