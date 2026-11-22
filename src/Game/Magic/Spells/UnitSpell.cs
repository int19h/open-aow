#nullable disable warnings
// Translated from AoWE.pas.
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
public class UnitSpell : Spell
{

    // Spells.PAS:1210
    public UnitSpell(CustomNode? owner) : base(owner)
    {
        AIValue = -1;
        SpellType = SpellType.Unit;
        AllowMultipleTargets = true;
        TargetRange = 0;
    }

    public bool AllowMultipleTargets { get; protected init; }
    public int TargetRange { get; protected init; }
    public int UnitDispelResistance { get; protected init; }
    public int AIValue { get; protected init; }

    // Spells.PAS:1444
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        if (AIValue != -1 && source.Owner is Army)
        {
            bool valid = AIRequestCastSpellExpense(source, budget: BaseManaCost, mana: ManaUpkeep, upkeep: 0);
            valid = valid && source.Owner is Army;
            string reason = "";
            valid = valid && source.CanCastSpell(SpellId, ref reason);
            int priority = default;
            Troop? target = default;
            if (valid && AIRetrieveOptimalTargetUnit(source, targets: source.OwnerUnits, ref target, ref priority))
            {
                AICastSpellAction action = CreateCastSpellAction();
                SetupCastSpellAction(source, action);
                action.Priority = priority;
                action.SetOwner(actions);
            }
        }
    }

    // Spells.PAS:1466
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        string reason = "";
        if (source.CanCastSpell(SpellId, ref reason))
        {
            _ = AICastSpellExpenseMade(source, budget: BaseManaCost, mana: ManaUpkeep, upkeep: 0);
            if (source.CanCastSpellInstantly(SpellId, mana: BaseManaCost, ref reason))
            {
                int priority = default;
                Troop? target = default;
                if (source.Owner is IHeroHolder)
                {
                    return false;
                }
                else if (AIRetrieveOptimalTargetUnit(source, targets: source.OwnerUnits, ref target, ref priority))
                {
                    UnitSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
                    eventValue.Initialize(SpellId, source, targets: null, mana: BaseManaCost);
                    eventValue.TargetPlayer = (byte)source.Player;
                    _ = eventValue.TargetUnitIds.Add(target.UnitId);
                    StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                    _ = eventValue.Release();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (source.GetCastingSpellId() != SpellId)
                {
                    _ = source.ExecuteStartCasting(SpellId);
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:1355
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, messageText: ref reason))
        {
            if (TargetRange == 0)
            {
                UnitSpellCaster caster = new(null) { Holder = CreationHolder() };
                if (source.Owner is IHeroHolder)
                {
                    using UnitUserList empty = new(null) { Holder = CreationHolder() };
                    SetupUnitSelectionControl(caster, source, targets: empty.Units);
                }
                else
                {
                    SetupUnitSelectionControl(caster, source, targets: source.OwnerUnits);
                }

                StrategicMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
                _ = caster.Release();
            }
            else
            {
                SetupTargetSelection(source, control: StrategicMap.Current.TargetSelection);
                StrategicMap.Current.TargetSelection.Activate();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:1379
    public override bool CanActivateCombat(Troop source, ref string reason) => SpellControlTypes.Contains(SpellControlTypes.Combat) && base.CanActivateCombat(source, ref reason);

    // Spells.PAS:1385
    public override CombatAction? CreateCombatAction(CombatObject? source, CombatObject target, bool chargeMana)
    {
        if (SpellControlTypes.Contains(SpellControlTypes.Combat))
        {
            UnitSpellCombatAction action = new(null)
            {
                Holder = CreationHolder(),
                SpellId = SpellId,
                SourceId = source.Id,
                TargetId = target.Id
            };
            if (chargeMana)
            {
                action.Mana = BaseManaCost;
            }

            return action;
        }
        else
        {
            return null;
        }
    }

    // Spells.PAS:1218
    public override void GetCombatInfo(Troop? source, ref SpellCombatInfo info)
    {
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = CombatTargetTypes.Friend;
        info.Range = -1;
    }

    // Spells.PAS:1226
    public virtual bool ValidTargetUnit(Troop? source, Troop? target) => true;

    // Spells.PAS:1228
    public virtual int ValidateUnitSelection(Troop? source, UnitUserList targets, int selection)
    {
        int count = targets.GetCount();
        selection &= SelectionMasks[count];
        for (int i = 0, last = targets.GetCount() - 1; i <= last; i++)
        {
            if ((selection & (1 << i)) != 0)
            {
                if (!ValidTargetUnit(source, target: targets.GetUnit(i)))
                {
                    selection &= ~(1 << i);
                }
            }
        }
        return selection;
    }

    // Spells.PAS:1238
    public virtual void SetupUnitSelectionControl(UnitSpellCaster caster, Troop source, UnitCollection targets)
    {
        caster.CasterSourceId = source.UnitId;
        caster.CasterSpellId = SpellId;
        for (int i = 0, last = targets.Count - 1; i <= last; i++)
        {
            // Concealment lives on an army. Another holder keeps no such record, so none of its
            // troops is concealed.
            if (targets.Holder is not Army holder || !holder.GetUnitConcealedForPlayer(i, source.Player))
            {
                Troop? troop = targets.GetUnit(i);
                _ = caster.TargetUnits.AddUnit(troop);
            }
        }

        caster.TargetUnits.SortUnits();
        caster.AllowMultipleTargets = AllowMultipleTargets;
        caster.ValidSelection = 0;
        for (int i = caster.TargetUnits.GetCount() - 1; i >= 0; i--)
        {
            if (ValidTargetUnit(source, target: caster.TargetUnits.GetUnit(i)))
            {
                caster.ValidSelection = (caster.ValidSelection | (1 << i)).TruncateToByte();
            }
        }
    }

    // Spells.PAS:1254
    public virtual bool CastSpell(Troop source, UnitUserList targets)
    {
        ResetTargetSelection();
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            UnitSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(mana: source.CastingMana(this), spellId: SpellId, source: source, targets: targets);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:1344
    public virtual bool ValidTargetParty(Troop source, ArmyHexSprite party) => !party.Army.Status.Contains(ArmyStatuses.FullyConcealed);

    // Spells.PAS:1346
    public virtual void TargetPartySelected(Troop source, ArmyHexSprite party)
    {
        UnitSpellCaster caster = new(null) { Holder = CreationHolder() };
        SetupUnitSelectionControl(caster, source, targets: party.Army.Units);
        StrategicMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
        _ = caster.Release();
    }

    // Spells.PAS:1224
    public virtual bool ExecuteSpell(Troop? source, Troop? target, int mana, Span<int> data) => false;

    // Spells.PAS:1400
    public virtual UnitSpellCombatAction? CreateCombatAction(CombatObject source, CombatObject target, int mana)
    {
        return SpellControlTypes.Contains(SpellControlTypes.Combat)
            ? new UnitSpellCombatAction(null)
            {
                Holder = CreationHolder(),
                SpellId = SpellId,
                SourceId = source.Id,
                TargetId = target.Id
            }
            : null;
    }

    // Spells.PAS:1411
    public virtual void ExecuteCombatAction(Battle combat, UnitSpellCombatAction action)
    {
        CombatObject? source = combat.CombatData.FindId(action.SourceId);
        CombatObject? target = combat.CombatData.FindId(action.TargetId);
        if (source is CombatUnit sourceUnit && target is CombatUnit targetUnit)
        {
            if (ExecuteSpell(sourceUnit.Troop, target: targetUnit.Troop, action.Mana, data: action.SpellData))
            {
                if (action.Mana != 0 && (((byte)combat.Status) & 1) != 0)
                {
                    GamePack.Current.SpellControl[SpellId].CombatCastingDone(source, action.Mana);
                }
            }
        }
    }

    // Spells.PAS:1422
    public virtual bool AIRetrieveOptimalTargetUnit(Troop source, UnitCollection targets, ref Troop? target, ref int priority)
    {
        DoubleItemList<Troop?> list = new();
        for (int i = 0, last = targets.Count - 1; i <= last; i++)
        {
            if (ValidTargetUnit(source, target: targets.GetUnit(i)) && targets.GetUnit(i).GetUnitType() != UnitType.Machine)
            {
                int value = targets.GetUnit(i).UnitLevel + AIValue;
                _ = list.AddAbility(targets.GetUnit(i), value);
            }
        }

        bool result;
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
        Troop? source = StrategicMap.Current.UnitControl.FindUnit(control.SourceUnitId);
        if (source is not null)
        {
            if (StrategicMap.Current.Visible(X, Y, L, radius: 0))
            {
                ArmyHexSprite party = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
                valid = party is not null && (party.Army.Player == source.Player || !StrategicMap.Current.MapContainer.GetField(X, Y, L).GetMagicShieldActive()) && ValidTargetParty(source, party);
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
        Troop? source = StrategicMap.Current.UnitControl.FindUnit(control.SourceUnitId);
        if (source is not null)
        {
            switch (control.Change)
            {
                case TargetSelectionChange.Deactivated:
                    ResetTargetSelection();
                    break;
                case TargetSelectionChange.Selected:
                    if (control.Target.X != -1)
                    {
                        ArmyHexSprite party = StrategicMap.Current.MapContainer.GetField(control.Target.X, control.Target.Y, control.Target.MapLevel).GetArmyHexSprite();
                        if (party is not null && ValidTargetParty(source, party))
                        {
                            TargetPartySelected(source, party);
                        }
                    }

                    break;
                case TargetSelectionChange.None:
                case TargetSelectionChange.Activated:
                default:
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
        TargetSelectionControl targetSelectionControl = StrategicMap.Current.TargetSelection;
        targetSelectionControl.Changed -= OnTargetSelectionChange;
        targetSelectionControl.Deactivate();
    }

    // Spells.PAS:1318
    public void SetupTargetSelection(Troop source, TargetSelectionControl control)
    {
        control.Initialize(source);
        control.TargetMode |= TargetSelectionModes.ExternalFloater;
        if (TargetRange != -1)
        {
            control.TargetMode |= TargetSelectionModes.ValidateDistance;
            control.Range = TargetRange;
        }
        else
        {
            control.Range = 0;
        }

        MapCoordinate coordinate85 = source.GetLocation();
        int x = coordinate85.X;
        int y = coordinate85.Y;
        int l = coordinate85.MapLevel;
        control.SourceLocation = new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)l);
        control.Changed += OnTargetSelectionChange;
        control.SetValidateTargetHandler(OnValidateTarget);
        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_selectTargetPartyFormat), arguments: [SpellName]);
        control.Text.Text = format;
        control.Title = SpellName;
        control.CancelText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CancelText);
        control.SourceUnitId = source.UnitId;
        control.SourceImage = GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(20).GetPreviewImage();
        control.ValidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(21).GetPreviewImage();
        control.InvalidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(22).GetPreviewImage();
    }

    private static ReadOnlySpan<byte> SelectionMasks => [0, 1, 3, 7, 15, 31, 63, 127, 255];

    private static readonly string s_selectTargetPartyFormat = "Select target party for %s on map";
}
