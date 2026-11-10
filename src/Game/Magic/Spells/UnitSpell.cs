// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Ai;

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
        FAIValue = -1;
        FSpellType = SpellType.Unit;
        FAllowMultipleTargets = true;
        FTargetRange = 0;
    }

    public bool FAllowMultipleTargets;
    public int FTargetRange;
    public byte FUnitDispelResistance;
    public int FAIValue;

    // Spells.PAS:1444
    public override void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
    {
        AICastSpellAction Action;
        string Reason = "";
        int Priority = default;
        AbstractUnit Target = default;
        bool Valid;
        if (FAIValue != -1 && Source.Owner is Army)
        {
            Valid = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, 0);
            Valid = Valid && Source.Owner is Army;
            Valid = Valid && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
            if (Valid && AIRetrieveOptimalTargetUnit(Source, Source.UnitListOwner, ref Target, ref Priority))
            {
                Action = CreateCastSpellAction();
                SetupCastSpellAction(Source, Action);
                Action.FPriority = Priority;
                Action.SetOwner(Actions);
            }
        }
    }

    // Spells.PAS:1466
    public override bool AIExecuteCastSpellAction(AbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        int Priority = default;
        AbstractUnit Target = default;
        UnitSpellTE Event;
        if (((Hero)Source).CanCastSpell(FSpellID, ref Reason))
        {
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, 0);
            if (((Hero)Source).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                if (Source.Owner is HeroList)
                {
                    Result = false;
                }
                else if (AIRetrieveOptimalTargetUnit(Source, Source.UnitListOwner, ref Target, ref Priority))
                {
                    Event = new UnitSpellTE(null);
                    Event.Initialize(FSpellID, Source, null, FBaseManaCost);
                    Event.FTargetPlayer = (byte)Source.FPlayer;
                    Event.FTargetUnitIDs.Add(Target.FUnitID);
                    AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                    Event.Release();
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                if (((Hero)Source).FCastingSpellID != FSpellID)
                {
                    ((Hero)Source).ExecuteStartCasting(FSpellID);
                    Result = true;
                }
                else
                {
                    Result = true;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1355
    public override bool Activate(AbstractUnit Source)
    {
        bool Result;
        UnitSpellCaster Caster;
        string Reason = "";
        base.Activate(Source);
        if (CanActivate(Source, ref Reason))
        {
            if (FTargetRange == 0)
            {
                Caster = new UnitSpellCaster(null);
                if (Source.Owner is HeroList)
                {
                    var empty = new UnitUserList(null);
                    SetupUSC(Caster, Source, empty);
                    empty.Dispose();
                }
                else
                {
                    SetupUSC(Caster, Source, Source.UnitListOwner);
                }

                AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
                Caster.Release();
            }
            else
            {
                SetupTargetSelection(Source, AoWHSMap.Current.FTargetSelection);
                AoWHSMap.Current.FTargetSelection.Activate();
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1379
    public override bool CanActivateCombat(AbstractUnit source, ref string reason)
    {
        bool Result;
        if (!FSpellControlTypes.Contains(SpellControlTypeElement.Combat) || !base.CanActivateCombat(source, ref reason))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:1385
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        CombatAction Result;
        UnitSpellCA Action;
        if (FSpellControlTypes.Contains(SpellControlTypeElement.Combat))
        {
            Action = new UnitSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            if (ChargeMana)
            {
                Action.FMana = FBaseManaCost;
            }

            Result = Action;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Spells.PAS:1218
    public override void GetCombatInfo(AbstractUnit Source, ref SpellCombatInfo Info)
    {
        base.GetCombatInfo(Source, ref Info);
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
        Info.Range = -1;
    }

    // Spells.PAS:1226
    public virtual bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target) => true;

    // Spells.PAS:1228
    public virtual void ValidateUnitSelection(AbstractUnit Source, UnitList Targets, ref byte Selection)
    {
        var count = Targets.GetCount();
        Selection = (byte)(Selection & P4SelectionMasks[count]);
        for (int I = 0, last = Targets.GetCount() - 1; I <= last; I++)
        {
            if ((Selection & (1 << I)) != 0)
            {
                if (!ValidTargetUnit(Source, Targets.GetUnit(I)))
                {
                    Selection = (byte)(Selection & ~(1 << I));
                }
            }
        }
    }

    // Spells.PAS:1238
    public virtual void SetupUSC(UnitSpellCaster Caster, AbstractUnit Source, UnitList Targets)
    {
        Caster.FCasterSourceID = Source.FUnitID;
        Caster.FCasterSpellID = FSpellID;
        for (int I = 0, last = Targets.GetCount() - 1; I <= last; I++)
        {
            if (!((Army)Targets).GetUnitConcealedForPlayer(I, Source.FPlayer))
            {
                var unit = Targets.GetUnit(I);
                Caster.FTargetUnits.AddUnit(unit);
            }
        }

        Caster.FTargetUnits.SortUnits();
        Caster.FAllowMultipleTargets = FAllowMultipleTargets;
        Caster.FValidSelection = 0;
        for (int I = Caster.FTargetUnits.GetCount() - 1; I >= 0; I--)
        {
            if (ValidTargetUnit(Source, Caster.FTargetUnits.GetUnit(I)))
            {
                Caster.FValidSelection = (byte)(Caster.FValidSelection | (1 << I));
            }
        }
    }

    // Spells.PAS:1254
    public virtual bool CastSpell(AbstractUnit Source, UnitList Targets)
    {
        bool Result;
        UnitSpellTE Event;
        ResetTargetSelection();
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new UnitSpellTE(null);
            var castingMana = ((Hero)Source).CastingMana(this);
            Event.Initialize(FSpellID, Source, Targets, castingMana);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1344
    public virtual bool ValidTargetParty(AbstractUnit Source, ArmyHS Party) => !Party.FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed);

    // Spells.PAS:1346
    public virtual void TargetPartySelected(AbstractUnit Source, ArmyHS Party)
    {
        UnitSpellCaster Caster;
        Caster = new UnitSpellCaster(null);
        SetupUSC(Caster, Source, Party.FArmy);
        AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
        Caster.Release();
    }

    // Spells.PAS:1224
    public virtual bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data) => false;

    // Spells.PAS:1400
    public virtual UnitSpellCA CreateCAEx(CombatObject Source, CombatObject Target, int Mana)
    {
        UnitSpellCA Result;
        if (FSpellControlTypes.Contains(SpellControlTypeElement.Combat))
        {
            Result = new UnitSpellCA(null);
            Result.FSpellID = FSpellID;
            Result.FSourceID = Source.FID;
            Result.FTargetID = Target.FID;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Spells.PAS:1411
    public virtual void ExecuteCA(Combat.Battle Combat, UnitSpellCA Action)
    {
        CombatObject Source;
        CombatObject Target;
        Source = Combat.FCombatData.FindID(Action.FSourceID);
        Target = Combat.FCombatData.FindID(Action.FTargetID);
        if (Source is CombatUnit && Target is CombatUnit)
        {
            if (ExecuteSpell(((CombatUnit)Source).FUnit, ((CombatUnit)Target).FUnit, Action.FMana, ref Action.FSpellData))
            {
                if (Action.FMana != 0 && (((byte)Combat.FStatus.Image) & 1) != 0)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, Action.FMana);
                }
            }
        }
    }

    // Spells.PAS:1422
    public virtual bool AIRetrieveOptimalTargetUnit(AbstractUnit Source, UnitList Targets, ref AbstractUnit Target, ref int Priority)
    {
        bool Result;
        DoubleItemList<AbstractUnit> List;
        int Value;
        List = new DoubleItemList<AbstractUnit>();
        for (int I = 0, last = Targets.GetCount() - 1; I <= last; I++)
        {
            if (ValidTargetUnit(Source, Targets.GetUnit(I)) && Targets.GetUnit(I).GetUnitType() != UnitType.Machine)
            {
                Value = Targets.GetUnit(I).GetUnitLevel() + FAIValue;
                List.AddAB(Targets.GetUnit(I), Value);
            }
        }

        if (List.Count != 0)
        {
            List.SortDescendingItemB();
            Target = List[0].A;
            Priority = List[0].B;
            Result = true;
        }
        else
        {
            Result = false;
        }

        List?.Dispose();
        return Result;
    }

    // Spells.PAS:1269
    public void OnValidateTarget(TargetSelectionControl Control, sbyte X, sbyte Y, sbyte L, ref bool Valid)
    {
        AbstractUnit Source;
        ArmyHS Party;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(Control.FSourceUnitID);
        if (Source != null)
        {
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                Party = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Party != null && (Party.FArmy.FPlayer == Source.FPlayer || !((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetMagicShieldActive()))
                {
                    Valid = ValidTargetParty(Source, Party);
                }
                else
                {
                    Valid = false;
                }
            }
            else
            {
                Valid = false;
            }
        }
        else
        {
            ResetTargetSelection();
        }
    }

    // Spells.PAS:1289
    public void OnTargetSelectionChange(TargetSelectionControl Control)
    {
        AbstractUnit Source;
        ArmyHS Party;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(Control.FSourceUnitID);
        if (Source != null)
        {
            switch (Control.FChange)
            {
                case TargetSelectionChange.Deactivated:
                    ResetTargetSelection();
                    break;
                case TargetSelectionChange.Selected:
                    if (Control.FTargetX != -1)
                    {
                        Party = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(Control.FTargetX, Control.FTargetY, Control.FTargetL)).GetArmyHS();
                        if (Party != null && ValidTargetParty(Source, Party))
                        {
                            TargetPartySelected(Source, Party);
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
        var targetSelectionControl = AoWHSMap.Current.FTargetSelection;
        targetSelectionControl.FOnChange -= OnTargetSelectionChange;
        targetSelectionControl.Deactivate();
    }

    // Spells.PAS:1318
    public void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl Control)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Control.Initialize(Source);
        Control.FTargetMode = Control.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ExternalFloater);
        if (FTargetRange != -1)
        {
            Control.FTargetMode = Control.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance);
            Control.FRange = FTargetRange;
        }
        else
        {
            Control.FRange = 0;
        }

        Source.GetLocation(ref X, ref Y, ref L);
        Control.FSourceX = X;
        Control.FSourceY = Y;
        Control.FSourceL = L;
        Control.FOnChange += OnTargetSelectionChange;
        Control.FOnValidateTarget = OnValidateTarget;
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(P4SelectTargetPartyForXRStr), [FSpellName]);
        Control.FText.Text = format;
        Control.FTitle = FSpellName;
        Control.FCancelText = AbilityRules.TranslateRStr(GameText.CancelRStr);
        Control.FSourceUnitID = Source.FUnitID;
        Control.FSourceImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(20).GetPreviewImage();
        Control.FValidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(21).GetPreviewImage();
        Control.FInvalidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(22).GetPreviewImage();
    }

    private static FixedArray_byte_9 P4SelectionMasks = FixedArray_byte_9.From((ReadOnlySpan<byte>)new byte[9] { 0, 1, 3, 7, 15, 31, 63, 127, 255 });
    private static readonly string P4SelectTargetPartyForXRStr = "Select target party for %s on map";
}
