// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// (multiple targets, range, dispel resistance, AI value)
public partial class UnitSpell : Spell
{

    // Spells.PAS:1210
    public UnitSpell(ECustomNode owner) : base(owner)
    {
        AIValue = -1;
        SpellType = SpellType.Unit;
        AllowMultipleTargets = true;
        TargetRange = 0;
    }

    public bool AllowMultipleTargets { get; protected set; }
    public int TargetRange { get; protected set; }
    public byte UnitDispelResistance { get; protected set; }
    public int AIValue { get; protected set; }

    // Spells.PAS:1444
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        AICastSpellAction action;
        string reason = "";
        int priority = default;
        Troop target = default;
        bool valid;
        if (AIValue != -1 && source.Owner is Army)
        {
            valid = AIRequestCastSpellExpense(source, BaseManaCost, ManaUpkeep, 0);
            valid = valid && source.Owner is Army;
            valid = valid && ((Hero)source).CanCastSpell(SpellId, ref reason);
            if (valid && AIRetrieveOptimalTargetUnit(source, source.UnitListOwner, ref target, ref priority))
            {
                action = CreateCastSpellAction();
                SetupCastSpellAction(source, action);
                action.Priority = priority;
                action.SetOwner(actions);
            }
        }
    }

    // Spells.PAS:1466
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        bool result;
        string reason = "";
        int priority = default;
        Troop target = default;
        UnitSpellTokenEvent eventValue;
        if (((Hero)source).CanCastSpell(SpellId, ref reason))
        {
            AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, 0);
            if (((Hero)source).CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                if (source.Owner is HeroList)
                {
                    result = false;
                }
                else if (AIRetrieveOptimalTargetUnit(source, source.UnitListOwner, ref target, ref priority))
                {
                    eventValue = new UnitSpellTokenEvent(null);
                    eventValue.Initialize(SpellId, source, null, BaseManaCost);
                    eventValue.TargetPlayer = (byte)source.Player;
                    eventValue.TargetUnitIds.Add(target.UnitId);
                    AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                    eventValue.Release();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (((Hero)source).CastingSpellId != SpellId)
                {
                    ((Hero)source).ExecuteStartCasting(SpellId);
                    result = true;
                }
                else
                {
                    result = true;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1355
    public override bool Activate(Troop source)
    {
        bool result;
        UnitSpellCaster caster;
        string reason = "";
        base.Activate(source);
        if (CanActivate(source, ref reason))
        {
            if (TargetRange == 0)
            {
                caster = new UnitSpellCaster(null);
                if (source.Owner is HeroList)
                {
                    var empty = new UnitUserList(null);
                    SetupUnitSelectionControl(caster, source, empty);
                    empty.Dispose();
                }
                else
                {
                    SetupUnitSelectionControl(caster, source, source.UnitListOwner);
                }

                AoWHSMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
                caster.Release();
            }
            else
            {
                SetupTargetSelection(source, AoWHSMap.Current.TargetSelection);
                AoWHSMap.Current.TargetSelection.Activate();
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1379
    public override bool CanActivateCombat(Troop source, ref string reason)
    {
        bool result;
        if (!SpellControlTypes.Contains(SpellControlTypeElement.Combat) || !base.CanActivateCombat(source, ref reason))
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Spells.PAS:1385
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        CombatAction result;
        UnitSpellCombatAction action;
        if (SpellControlTypes.Contains(SpellControlTypeElement.Combat))
        {
            action = new UnitSpellCombatAction(null);
            action.SpellId = SpellId;
            action.SourceId = source.Id;
            action.TargetId = target.Id;
            if (chargeMana)
            {
                action.Mana = BaseManaCost;
            }

            result = action;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Spells.PAS:1218
    public override void GetCombatInfo(Troop source, ref SpellCombatInfo info)
    {
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
        info.Range = -1;
    }

    // Spells.PAS:1226
    public virtual bool ValidTargetUnit(Troop source, Troop target) => true;

    // Spells.PAS:1228
    public virtual void ValidateUnitSelection(Troop source, UnitList targets, ref byte selection)
    {
        var count = targets.GetCount();
        selection = (byte)(selection & SelectionMasks[count]);
        for (int I = 0, last = targets.GetCount() - 1; I <= last; I++)
        {
            if ((selection & (1 << I)) != 0)
            {
                if (!ValidTargetUnit(source, targets.GetUnit(I)))
                {
                    selection = (byte)(selection & ~(1 << I));
                }
            }
        }
    }

    // Spells.PAS:1238
    public virtual void SetupUnitSelectionControl(UnitSpellCaster caster, Troop source, UnitList targets)
    {
        caster.CasterSourceId = source.UnitId;
        caster.CasterSpellId = SpellId;
        for (int I = 0, last = targets.GetCount() - 1; I <= last; I++)
        {
            if (!((Army)targets).GetUnitConcealedForPlayer(I, source.Player))
            {
                var troop = targets.GetUnit(I);
                caster.TargetUnits.AddUnit(troop);
            }
        }

        caster.TargetUnits.SortUnits();
        caster.AllowMultipleTargets = AllowMultipleTargets;
        caster.ValidSelection = 0;
        for (int I = caster.TargetUnits.GetCount() - 1; I >= 0; I--)
        {
            if (ValidTargetUnit(source, caster.TargetUnits.GetUnit(I)))
            {
                caster.ValidSelection = (byte)(caster.ValidSelection | (1 << I));
            }
        }
    }

    // Spells.PAS:1254
    public virtual bool CastSpell(Troop source, UnitList targets)
    {
        bool result;
        UnitSpellTokenEvent eventValue;
        ResetTargetSelection();
        if (!AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new UnitSpellTokenEvent(null);
            var castingMana = ((Hero)source).CastingMana(this);
            eventValue.Initialize(SpellId, source, targets, castingMana);
            AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1344
    public virtual bool ValidTargetParty(Troop source, ArmyHexSprite party) => !party.Army.Status.Contains(ArmyStatusElement.FullyConcealed);

    // Spells.PAS:1346
    public virtual void TargetPartySelected(Troop source, ArmyHexSprite party)
    {
        UnitSpellCaster caster;
        caster = new UnitSpellCaster(null);
        SetupUnitSelectionControl(caster, source, party.Army);
        AoWHSMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
        caster.Release();
    }

    // Spells.PAS:1224
    public virtual bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data) => false;

    // Spells.PAS:1400
    public virtual UnitSpellCombatAction CreateCombatAction(CombatObject source, CombatObject target, int mana)
    {
        UnitSpellCombatAction result;
        if (SpellControlTypes.Contains(SpellControlTypeElement.Combat))
        {
            result = new UnitSpellCombatAction(null);
            result.SpellId = SpellId;
            result.SourceId = source.Id;
            result.TargetId = target.Id;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Spells.PAS:1411
    public virtual void ExecuteCombatAction(Battle combat, UnitSpellCombatAction action)
    {
        CombatObject source;
        CombatObject target;
        source = combat.CombatData.FindId(action.SourceId);
        target = combat.CombatData.FindId(action.TargetId);
        if (source is CombatUnit && target is CombatUnit)
        {
            if (ExecuteSpell(((CombatUnit)source).Troop, ((CombatUnit)target).Troop, action.Mana, action.SpellData))
            {
                if (action.Mana != 0 && (((byte)combat.Status.Image) & 1) != 0)
                {
                    AoWHSSet.Current.SpellControl.GetSpell(SpellId).CombatCastingDone(source, action.Mana);
                }
            }
        }
    }

    // Spells.PAS:1422
    public virtual bool AIRetrieveOptimalTargetUnit(Troop source, UnitList targets, ref Troop target, ref int priority)
    {
        bool result;
        DoubleItemList<Troop> list;
        int value;
        list = new DoubleItemList<Troop>();
        for (int I = 0, last = targets.GetCount() - 1; I <= last; I++)
        {
            if (ValidTargetUnit(source, targets.GetUnit(I)) && targets.GetUnit(I).GetUnitType() != UnitType.Machine)
            {
                value = targets.GetUnit(I).GetUnitLevel() + AIValue;
                list.AddAbility(targets.GetUnit(I), value);
            }
        }

        if (list.Count != 0)
        {
            list.SortDescendingItemB();
            target = list[0].First;
            priority = list[0].Second;
            result = true;
        }
        else
        {
            result = false;
        }

        list?.Dispose();
        return result;
    }

    // Spells.PAS:1269
    public void OnValidateTarget(TargetSelectionControl control, sbyte X, sbyte Y, sbyte L, ref bool valid)
    {
        Troop source;
        ArmyHexSprite party;
        source = AoWHSMap.Current.UnitControl.FindUnit(control.SourceUnitId);
        if (source != null)
        {
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                party = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHexSprite();
                if (party != null && (party.Army.Player == source.Player || !((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetMagicShieldActive()))
                {
                    valid = ValidTargetParty(source, party);
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
        }
        else
        {
            ResetTargetSelection();
        }
    }

    // Spells.PAS:1289
    public void OnTargetSelectionChange(TargetSelectionControl control)
    {
        Troop source;
        ArmyHexSprite party;
        source = AoWHSMap.Current.UnitControl.FindUnit(control.SourceUnitId);
        if (source != null)
        {
            switch (control.Change)
            {
                case TargetSelectionChange.Deactivated:
                    ResetTargetSelection();
                    break;
                case TargetSelectionChange.Selected:
                    if (control.TargetX != -1)
                    {
                        party = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(control.TargetX, control.TargetY, control.TargetL)).GetArmyHexSprite();
                        if (party != null && ValidTargetParty(source, party))
                        {
                            TargetPartySelected(source, party);
                        }
                    }

                    break;
            }
        }
        else
        {
            ResetTargetSelection();
        }
    }

    // Spells.PAS:1310
    public void ResetTargetSelection()
    {
        var targetSelectionControl = AoWHSMap.Current.TargetSelection;
        targetSelectionControl.FOnChange -= OnTargetSelectionChange;
        targetSelectionControl.Deactivate();
    }

    // Spells.PAS:1318
    public void SetupTargetSelection(Troop source, TargetSelectionControl control)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        control.Initialize(source);
        control.TargetMode = control.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ExternalFloater);
        if (TargetRange != -1)
        {
            control.TargetMode = control.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance);
            control.Range = TargetRange;
        }
        else
        {
            control.Range = 0;
        }

        source.GetLocation(ref X, ref Y, ref L);
        control.SourceX = X;
        control.SourceY = Y;
        control.SourceL = L;
        control.FOnChange += OnTargetSelectionChange;
        control.OnValidateTarget = OnValidateTarget;
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_selectTargetPartyFormat), [SpellName]);
        control.Text.Text = format;
        control.Title = SpellName;
        control.CancelText = AbilityRules.TranslateResourceString(GameText.CancelText);
        control.SourceUnitId = source.UnitId;
        control.SourceImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(20).GetPreviewImage();
        control.ValidTargetImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(21).GetPreviewImage();
        control.InvalidTargetImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(22).GetPreviewImage();
    }

    private static ReadOnlySpan<byte> SelectionMasks => [0, 1, 3, 7, 15, 31, 63, 127, 255];

    private static readonly string s_selectTargetPartyFormat = "Select target party for %s on map";
}
