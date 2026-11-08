// Translated from CitySpells.pas.
using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.City;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.Sound;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.EventLog.EventLogUnit;
using static Aow.Units.HSEngine.HSEngineUnit;
using static Aow.Units.Sound.SoundUnit;
using Exception = Aow.Rtl.Exception;

namespace Aow.Units.CitySpells;

public enum TCityTargetModeElement : byte
{
    ctmOwn = 0,
    ctmEnemy = 1,
    ctmSkipSource = 2,
    ctmRazed = 3
}

public enum TCityEnchantmentCastSpellSituationElement : byte
{
    ccsLeaderInCity = 0,
    ccs4HexCity = 1,
    ccs3HexCity = 2
}

// { ot=1 }
// packed target coordinates. target owner
public partial class TCityTargetSpellTE : TSpellTE
{
    public TCityTargetSpellTE(TECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:170
    public new const int ClassID = 135949;

    public TXYL FTargetLocation;
    public sbyte FTargetPlayer;

    // CitySpells.pas:175
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTargetLocation.XYL, 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 21);
    }

    // CitySpells.pas:189
    public override bool Validate(THero source)
    {
        TCity TargetCity;
        bool Result = base.Validate(source);
        if (Result)
        {
            TargetCity = GetSelectedCity();
            if (TargetCity == null || TargetCity.FPlayer != FTargetPlayer)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:182
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        FTargetLocation.X = X;
        FTargetLocation.Y = Y;
        FTargetLocation.L = L;
    }

    // CitySpells.pas:204
    public TCity GetSelectedCity() => (TCity)AoWHSMap.MapContainer.GetFieldXYL(FTargetLocation.XYL).FindNoneTransparentHS(typeof(TCity));

    // NetworkE.pas:230

}

// source unit ID. city target modes
public partial class TCityTargetSpell : TSpell
{
    public int FSourceID;
    public TSet<TCityTargetModeElement> FCityTargetMode;

    // CitySpells.pas:209
    public TCityTargetSpell(TECustomNode owner) : base(owner)
    {
        FSpellType = TSpellType.stGlobal;
    }

    // CitySpells.pas:377
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        if (CanActivate(Source, ref Reason))
        {
            FSourceID = Source.FUnitID;
            SetupTargetSelection(Source, AoWHSMap.FTargetSelection);
            AoWHSMap.FTargetSelection.Activate();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CitySpells.pas:288
    public virtual void OnTargetSelectionChange(TObject Sender)
    {
        TTargetSelectionControl TargetSelection = default;
        TAbstractUnit Source = default;
        bool Valid = default;
        TargetSelection = (TTargetSelectionControl)Sender;
        Source = AoWHSMap.FUnitControl.FindUnit(FSourceID);
        if (Source != null)
        {
            switch (TargetSelection.FChange)
            {
                case TTargetSelectionChange.tscDeactivated:
                    TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                    TargetSelection.Deactivate();
                    break;
                case TTargetSelectionChange.tscSelected:
                    ValidateTarget(TargetSelection, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL, ref Valid);
                    if (Valid)
                    {
                        if (TargetSelected(Source, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL))
                        {
                            TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                            TargetSelection.Deactivate();
                        }
                    }

                    break;
            }
        }
        else
        {
            TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
            TargetSelection.Deactivate();
        }
    }

    // CitySpells.pas:215
    public virtual void PlayDefaultCityEffects(TAbstractUnit Source, TCity TargetCity)
    {
        TMapLevel Level;
        TImageSequenceAnimation A;
        THNWalkData Walk = default;
        bool PlaySound;
        PlaySound = false;
        var lhx = TargetCity.GetLhx();
        Level = AoWHSMap.MapContainer.GetMapLevel(lhx);
        var yhx = TargetCity.GetYhx();
        var xhx = TargetCity.GetXhx();
        InitHNWalk(ref Walk, xhx, yhx, 0);
        while (Walk.HN < TargetCity.Resource.TerrainList.Count)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height && TargetCity.Resource.TerrainList.Used[Walk.HN])
            {
                bool condition = Source.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!condition)
                {
                    var targetCityLhx = TargetCity.GetLhx();
                    condition = AoWHSMap.TerrainVisible((sbyte)Walk.X, (sbyte)Walk.Y, targetCityLhx, 0);
                }

                if (condition)
                {
                    // { Original passes the city layer before the radius. }
                    A = new TImageSequenceAnimation(null);
                    A.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                    A.FFrameCounter = Random(5);
                    A.FAnimationLevel = 27;
                    var targetCityLhx = TargetCity.GetLhx();
                    AoWHSMap.PlaceHX(A, Walk.X, Walk.Y, targetCityLhx);
                    bool test = Source.FPlayer == AoWHSMap.FSeatedPlayer;
                    if (!test)
                    {
                        var lhx2 = TargetCity.GetLhx();
                        test = AoWHSMap.WatchingTerrain((sbyte)Walk.X, (sbyte)Walk.Y, lhx2, 0);
                    }

                    if (test)
                    {
                        PlaySound = true;
                    }
                }
            }

            WalkToNextHN(ref Walk);
        }

        if (PlaySound)
        {
            AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }
    }

    // CitySpells.pas:323
    public virtual void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TargetSelection.Initialize(Source);
        TargetSelection.FTargetMode = TargetSelection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
        var format = RtlUnit.Format(TranslateRStr(SelectTargetCityForXOnMapRStr), [FSpellName]);
        TargetSelection.FText.Text = format;
        TargetSelection.FTitle = FSpellName;
        TargetSelection.FCancelText = TranslateRStr(CancelRStr);
        TargetSelection.FSourceImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
        TargetSelection.FValidTargetImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
        TargetSelection.FInvalidTargetImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
        Source.GetLocation(ref X, ref Y, ref L);
        TargetSelection.FSourceX = X;
        TargetSelection.FSourceY = Y;
        TargetSelection.FSourceL = L;
        TargetSelection.FRange = 12;
        if (TargetSelection.FOnChange.Count > 1)
        {
            // { Original rejects two existing listeners. }
            throw Exception.Create("oohps");
        }

        TargetSelection.FOnChange.Add(OnTargetSelectionChange);
        TargetSelection.FOnValidateTarget = ValidateTarget;
    }

    // CitySpells.pas:359
    public virtual bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
    {
        bool Result;
        TCityTargetSpellTE Event;
        if (!AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = CreateTE();
            if (SetupTE(Event, Source, X, Y, L))
            {
                AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Result = true;
            }
            else
            {
                Result = false;
            }

            Event.Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CitySpells.pas:248
    public virtual bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TCity Found;
        bool Result = (FCityTargetMode.Contains(TCityTargetModeElement.ctmOwn) && TargetCity.FPlayer == Source.FPlayer) || (FCityTargetMode.Contains(TCityTargetModeElement.ctmEnemy) && TargetCity.FPlayer != Source.FPlayer);
        Result = Result && (!TargetCity.GetRazed() || FCityTargetMode.Contains(TCityTargetModeElement.ctmRazed));
        if (Result && FCityTargetMode.Contains(TCityTargetModeElement.ctmSkipSource))
        {
            Source.GetLocation(ref X, ref Y, ref L);
            Found = (TCity)AoWHSMap.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(TCity));
            Result = TargetCity != Found;
        }

        return Result;
    }

    // CitySpells.pas:346
    public virtual TCityTargetSpellTE CreateTE() => new TCityTargetSpellTE(null);

    // CitySpells.pas:351
    public virtual bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L)
    {
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        Event.FTargetPlayer = Event.GetSelectedCity().FPlayer;
        return true;
    }

    // CitySpells.pas:262
    public void ValidateTarget(TTargetSelectionControl TargetSelection, sbyte X, sbyte Y, sbyte L, ref bool Valid)
    {
        TAbstractUnit Source;
        TMapLevel Level;
        TAoWMapField Field;
        TCity TargetCity;
        Level = AoWHSMap.MapContainer.GetMapLevel(L);
        Source = AoWHSMap.FUnitControl.FindUnit(FSourceID);
        if (Source != null && X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
        {
            Field = (TAoWMapField)Level.GetField(X, Y);
            Valid = !AoWHSMap.FExplorationEnabled || Field.GetExplored(Source.FPlayer);
            if (Valid)
            {
                TargetCity = (TCity)Field.FindNoneTransparentHS(typeof(TCity));
                if (TargetCity != null)
                {
                    Valid = ValidTargetCity(Source, TargetCity);
                }
                else
                {
                    Valid = false;
                }
            }
        }
        else
        {
            Valid = false;
        }
    }
}

// army ID. destination coordinates
public partial class TTownGateTE : TCityTargetSpellTE
{
    public TTownGateTE(TECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:397
    public new const int ClassID = 135950;

    public TArmyID FArmyID;
    public TXYL FTeleportLocation;

    // CitySpells.pas:391
    public override void Destroy()
    {
        FArmyID.Free();
        base.Destroy();
    }

    // CitySpells.pas:400
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTeleportLocation.XYL, 30);
        Stream.rwCreateEObject(ref FArmyID, null, 31);
    }

    // CitySpells.pas:407
    public override bool Validate(THero source)
    {
        byte Selection = default;
        TArmyHS Army = default;
        bool Result = base.Validate(source) && FArmyID.Valid();
        if (Result)
        {
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            Result = ((TTownGate)AoWHSSet.FSpellControl.GetSpell(FEventSpellID)).ValidTeleportLocation(Army, Selection, FTeleportLocation.X, FTeleportLocation.Y, FTeleportLocation.L);
        }

        return Result;
    }

    // NetworkE.pas:230

}

public partial class TTownGate : TCityTargetSpell
{
    // CitySpells.pas:421
    public TTownGate(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(TownGateRStr);
        FSpellID = 38;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn, TCityTargetModeElement.ctmSkipSource);
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = TMagicSphere.msCosmos;
    }

    // CitySpells.pas:486
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool ShowSource;
        bool ShowTarget;
        byte Selection = default;
        TArmyHS Army = default;
        TAoWMapField Field;
        TArmyHS NewArmy;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TImageSequenceAnimation A;
        Source.GetLocation(ref X, ref Y, ref L);
        ((TTownGateTE)Event).FArmyID.RetrieveArmy(ref Army, ref Selection);
        Field = (TAoWMapField)AoWHSMap.MapContainer.GetFieldXYL(((TTownGateTE)Event).FTeleportLocation.XYL);
        Army.FArmy.ResetDestinationPos();
        if (AoWHSMap.FSelectedArmy.GetArmyHS() == Army)
        {
            AoWHSMap.FSelectedArmy.Select(Army);
        }

        if (Army.FArmy.ValidateSelection(Selection) != Army.FArmy.ValidateSelection(255))
        {
            NewArmy = new TArmyHS(null);
            NewArmy.FArmy.SetPlayer(Army.FArmy.FPlayer);
            NewArmy.FArmy.AddUnitSelection(Army.FArmy, Selection);
            AoWHSMap.PlaceHX(NewArmy, ((TTownGateTE)Event).FTeleportLocation.X, ((TTownGateTE)Event).FTeleportLocation.Y, ((TTownGateTE)Event).FTeleportLocation.L);
        }
        else
        {
            if (Field.GetArmyHS() != null)
            {
                Field.GetArmyHS().FArmy.AddUnitSelection(Army.FArmy, Selection);
            }
            else
            {
                Army.MoveTo(Field, 0);
            }
        }

        ShowSource = AoWHSMap.FSeatedPlayer == Source.FPlayer || AoWHSMap.Visible(X, Y, L, 0);
        ShowTarget = AoWHSMap.FSeatedPlayer == Source.FPlayer || AoWHSMap.Visible(Field.Xhx, Field.Yhx, Field.Lhx, 0);
        if (ShowSource)
        {
            if (AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
            {
                A = new TImageSequenceAnimation(null);
                A.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.PlaceHX(A, X, Y, L);
            }
        }

        if (ShowTarget)
        {
            if (AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
            {
                A = new TImageSequenceAnimation(null);
                A.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.PlaceHX(A, Field.Xhx, Field.Yhx, Field.Lhx);
            }
        }

        if (AoWHSMap.FSeatedPlayer == Source.FPlayer)
        {
            AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
        }
        else
        {
            if (!AoWHSMap.ValidateObservation(Source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0))
            {
                AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            }
        }

        if (AoWHSMap.FSeatedPlayer == Source.FPlayer || AoWHSMap.Watching(X, Y, L, 0) || AoWHSMap.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
        {
            AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }

        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // CitySpells.pas:540
    public override bool CanActivate(TAbstractUnit Source, ref string MessageText) => base.CanActivate(Source, ref MessageText);

    // CitySpells.pas:545
    public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FTargetMode = TargetSelection.FTargetMode - TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmFixedLevel, TTargetSelectionModeElement.tmValidateDistance);
    }

    // CitySpells.pas:445
    public override TCityTargetSpellTE CreateTE() => new TTownGateTE(null);

    // CitySpells.pas:450
    public override bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L)
    {
        byte Selection;
        TArmyHS Army;
        bool Found;
        TMapLevel Level;
        THNWalkData Walk = default;
        bool Result = base.SetupTE(Event, Source, X, Y, L);
        if (Result)
        {
            Army = ((TArmy)Source.Owner).GetArmyHS();
            if (Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer && AoWHSMap.FSelectedArmy.GetArmyHS() == Army)
            {
                Selection = AoWHSMap.FSelectedArmy.GetSelection();
            }
            else
            {
                Selection = 255;
            }

            InitHNWalk(ref Walk, X, Y, 0);
            Found = false;
            Level = AoWHSMap.MapContainer.GetMapLevel(L);
            while (Walk.Radius < 4 && !Found)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Found = ValidTeleportLocation(Army, Selection, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                }

                if (!Found)
                {
                    WalkToNextHN(ref Walk);
                }
            }

            if (Found)
            {
                var id = Army.FArmy.CreateID(Selection);
                ((TTownGateTE)Event).FArmyID = id;
                var packXYL = PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                ((TTownGateTE)Event).FTeleportLocation = packXYL;
                Result = true;
            }
            else
            {
                Result = false;
            }
        }

        return Result;
    }

    // CitySpells.pas:432
    public bool ValidTeleportLocation(TArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        TAoWMapField Field;
        Field = (TAoWMapField)AoWHSMap.MapContainer.GetField(X, Y, L);
        if (Field.GetPlayer() != -1 && Field.GetPlayer() != Army.FArmy.FPlayer)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        if (Result)
        {
            Result = Army.FArmy.ValidTerrainEx(Selection, Field.TerrainType, Field.OverlayType);
            // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
            Result = Result && (Field.GetArmyHS() == null || Field.GetArmyHS().FArmy.CanAddUnitSelection(Army.FArmy, Selection) == TAddUnitsResult.arOK);
        }

        return Result;
    }
}

public partial class TCityEnchantmentSpellTE : TCityTargetSpellTE
{
    public TCityEnchantmentSpellTE(TECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:551
    public new const int ClassID = 131821;

    // NetworkE.pas:230

}

// enchantment ID, enchantment setting, AI priority, AI budget, AI cast situations
public partial class TCityEnchantmentSpell : TCityTargetSpell
{
    public TCityEnchantmentID FEnchantmentID;
    public int FEnchantmentSpellID;
    public int FAIPriority;
    public int FAIBudget;
    public TSet<TCityEnchantmentCastSpellSituationElement> FAICastSituation;

    // CitySpells.pas:554
    public TCityEnchantmentSpell(TECustomNode owner) : base(owner)
    {
        FAIPriority = -1;
    }

    // CitySpells.pas:576
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        TCity TargetCity;
        TCityEnchantment Enchantment;
        TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
        Enchantment = new TCityEnchantment(null);
        Enchantment.FEnchantmentID = FEnchantmentID;
        Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
        var xyl = ((TMapField)TargetCity.Owner).GetXYL();
        Enchantment.FEnchantmentLocation.XYL = xyl;
        Enchantment.FDispelMana = FEnchantmentSpellID;
        Enchantment.FPlayerIndex = Source.FPlayer;
        TargetCity.ActivateEnchantment(Enchantment);
        PlayDefaultCityEffects(Event.GetSource(), TargetCity);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // CitySpells.pas:594
    public override void AIPrefetchCastSpellActions(TAbstractUnit Source, TAICastSpellActionList Actions)
    {
        bool Valid;
        TAICastSpellAction Action;
        string Reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TCity TargetCity;
        if (FAIPriority != -1)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            TargetCity = (TCity)AoWHSMap.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(TCity));
            bool condition = TargetCity != null;
            if (condition)
            {
                var aiCityEnchantmentUpkeepPriority = TargetCity.AICityEnchantmentUpkeepPriority(FEnchantmentID);
                condition = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiCityEnchantmentUpkeepPriority);
            }

            Valid = condition;
            // { Original uses mana upkeep for mana and city priority for upkeep. }
            Valid = Valid && ValidTargetCity(Source, TargetCity);
            Valid = Valid && ((THero)Source).CanCastSpell(FSpellID, ref Reason);
            if (Valid)
            {
                if (FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity) && Source is TLeader || FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccs4HexCity) && TargetCity.GetSize() == 4 || FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccs3HexCity) && TargetCity.GetSize() == 3)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(Source, Action);
                    Action.FPriority = FAIPriority;
                    Action.SetOwner(Actions);
                }
            }
        }
    }

    // CitySpells.pas:623
    public override bool AIExecuteCastSpellAction(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TCity TargetCity;
        Source.GetLocation(ref X, ref Y, ref L);
        TargetCity = (TCity)AoWHSMap.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(TCity));
        if (TargetCity != null && ValidTargetCity(Source, TargetCity) && ((THero)Source).CanCastSpell(FSpellID, ref Reason))
        {
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, FAIBudget);
            var hero = (THero)Source;
            if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Result = TargetSelected(Source, X, Y, L);
            }
            else
            {
                if (hero.FCastingSpellID != FSpellID)
                {
                    hero.ExecuteStartCasting(FSpellID);
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

    // CitySpells.pas:570
    public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity) => base.ValidTargetCity(Source, TargetCity) && !TargetCity.FEnchantments.Contains(FEnchantmentID);

    // CitySpells.pas:560
    public override TCityTargetSpellTE CreateTE() => new TCityEnchantmentSpellTE(null);

    // CitySpells.pas:565
    public override bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L) => base.SetupTE(Event, Source, X, Y, L);
}

public partial class TFountainOfLife : TCityEnchantmentSpell
{
    // CitySpells.pas:649
    public TFountainOfLife(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(FountainOfLifeRStr);
        FSpellID = 52;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msWater;
        FEnchantmentID = TCityEnchantmentID.ceiFountainOfLife;
        FCastDuration = 20;
        FAIPriority = 500;
        FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
        FAIBudget = 600;
        FEnchantmentSpellID = 50;
    }

    // CitySpells.pas:692
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        TCity TargetCity = default;
        TAnimation A = default;
        Result = base.ExecuteTE(Event, Source, ExecuteCount);
        if (ExecuteCount == 0)
        {
            TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
            A = new TAnimation(null);
            A.FAnimationLevel = 27;
            A.FOnShow = ShowFountain;
            A.FAnimationMap = AoWHSMap;
            A.FTag = (int)(((uint)(int)TargetCity.GetXhx()) | ((uint)(TargetCity.GetYhx() << 8)));
            A.Activate();
        }

        return Result;
    }

    // CitySpells.pas:666
    public void ShowFountain(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
    {
        int DX;
        int VY;
        int Offset;
        int CY = default;
        int CX = default;
        int VX;
        int T;
        int DY;
        HXtoHP(Animation.FTag & 255, (int)(((uint)Animation.FTag) >> 8), ref CX, ref CY);
        CX = CX + 10 - X;
        CY = CY - Y + 10;
        // { Original resets the process-wide random seed for every draw. }
        RandSeed = 0;
        for (int I = 0; I <= 100; I++)
        {
            VX = Random(100) - 50;
            VY = Random(5) + 7;
            Offset = Round((double)VX * 0.1);
            T = Random(30);
            if (T < Animation.FFrameCounter && Animation.FFrameCounter - T < 90)
            {
                T = (Animation.FFrameCounter - T) % 30;
                DX = Round((double)(T * VX) / 30);
                DY = VY * T - Round((double)((double)T * 0.25) * T);
                if (T > 0)
                {
                    AoWHSSet.FSpellControl.GetSpell(52).FCastImages.Show(0, CX + DX + Offset, CY - DY, 0, SD);
                }
            }
        }

        if (Animation.FFrameCounter == 120)
        {
            Animation.Free();
        }
    }
}

public partial class TGoldRush : TCityEnchantmentSpell
{
    // CitySpells.pas:710
    public TGoldRush(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(GoldRushRStr);
        FSpellID = 53;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msWater;
        FEnchantmentID = TCityEnchantmentID.ceiGoldRush;
        FCastDuration = 20;
        FAIPriority = 500;
        FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccs4HexCity);
        FAIBudget = 600;
        FEnchantmentSpellID = 25;
    }
}

public partial class TAntiMagicShell : TCityEnchantmentSpell
{
    // CitySpells.pas:727
    public TAntiMagicShell(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(AntiMagicShellRStr);
        FSpellID = 54;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msCosmos;
        FEnchantmentID = TCityEnchantmentID.ceiAntiMagicShell;
        FCastDuration = 40;
        FAIPriority = 500;
        FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
        FEnchantmentSpellID = 100;
    }

    // CitySpells.pas:743
    public override void PlayDefaultCityEffects(TAbstractUnit Source, TCity TargetCity)
    {
        base.PlayDefaultCityEffects(Source, TargetCity);
        bool condition = Source.FPlayer == AoWHSMap.FSeatedPlayer;
        if (!condition)
        {
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            var xhx = TargetCity.GetXhx();
            condition = AoWHSMap.WatchingTerrain(xhx, yhx, lhx, 1);
        }

        if (condition)
        {
            AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(1, -1, false, 30, TSamplePanning.spCenter, null);
        }
    }
}

public partial class TWarmonger : TCityEnchantmentSpell
{
    // CitySpells.pas:751
    public TWarmonger(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(WarmongerRStr);
        FSpellID = 58;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
        FEnchantmentID = TCityEnchantmentID.ceiWarmonger;
        FCastDuration = 30;
        FAIPriority = 600;
        FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccs4HexCity, TCityEnchantmentCastSpellSituationElement.ccs3HexCity);
        FEnchantmentSpellID = 50;
    }
}

public partial class TTownQuake : TCityTargetSpell
{
    // CitySpells.pas:764
    public TTownQuake(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(TownQuakeRStr);
        FSpellID = 71;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy);
    }

    // CitySpells.pas:820
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result;
        TCity TargetCity;
        TArmyHSList Armies;
        if (ExecuteCount == 0)
        {
            TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.StartTownQuake();
            TargetCity.ValidateObservation(Source.FPlayer);
            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
                TargetCity.StopTownQuake();
                Armies = new TArmyHSList(null);
                if (AoWHSMap.Random(10) < 7)
                {
                    TargetCity.SetWallType(0);
                }

                TargetCity.ListArmies(Armies);
                for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                {
                    TriggerArmyDamage(Source, Armies.GetArmyHS(I));
                }

                Armies.Free();
                CastingDone(Source, Event.FEventMana);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:812
    public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        if (!Result || TargetCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CitySpells.pas:773
    public void TriggerArmyDamage(TAbstractUnit Source, TArmyHS Army)
    {
        TArmyEventLog Log;
        bool Damaged;
        int Damage;
        TAbstractUnit AUnit;
        Log = new TArmyEventLog(null);
        Log.FArmyText = TranslateRStr(TownQuakeDamageRStr);
        Log.FPlayer = Source.FPlayer;
        var townQuake = TranslateRStr(TownQuakeRStr);
        Log.FArmyMessages.Add(townQuake);
        Log.FArmyIcon = 2;
        Log.FArmyView.Setup(Army.FArmy, 255);
        Army.FArmy.BeginUpdate();
        Damaged = false;
        for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
        {
            AUnit = Army.FArmy.GetUnit(I);
            Damage = AUnit.ExecuteDamageRole(7, 5, TSet<TDamageType>.Of(TDamageType.dtPhysical));
            if (Damage > AUnit.GetHitPoints())
            {
                Damage = AUnit.GetHitPoints();
            }

            if (Damage > 0)
            {
                AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - Damage));
                Damaged = true;
            }

            Log.FDamageValues.Add((byte)Damage);
            Log.FDamageTypes.Add(7);
        }

        if (Damaged)
        {
            Log.ValidateGameOver();
            var yhx = Army.GetYhx();
            var lhx = Army.GetLhx();
            var xhx = Army.GetXhx();
            DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, lhx, true);
        }

        Log.Release();
        Army.FArmy.Validate();
        Army.FArmy.EndUpdate();
    }
}

public partial class TAnarchy : TCityTargetSpell
{
    // CitySpells.pas:849
    public TAnarchy(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(AnarchyRStr);
        FSpellID = 72;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy);
    }

    // CitySpells.pas:875
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        TCity TargetCity = default;
        TLocationTextEventLog Log = default;
        TImageSequenceAnimation A = default;
        if (ExecuteCount == 0)
        {
            TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
            Log = new TLocationTextEventLog(null);
            Log.FPlayer = Source.FPlayer;
            Log.FText = RtlUnit.Format(TranslateRStr(XCastRStr), [FSpellName]);
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
            AoWHSMap.FPlayers.GetPlayers(TargetCity.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.ValidateObservation(Source.FPlayer);
            if (TargetCity.Resource.ImageSequenceList.ImageSequences[16] != null)
            {
                A = new TImageSequenceAnimation(null);
                A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[16]);
                A.SetXhpOffset(A.FXhpOffset - 16);
                A.SetYhpOffset(A.FYhpOffset - 16);
                A.FOnShow = ShowAnimation;
                A.FLooped = true;
                A.FFrameCount = 120;
                var targetCityLhx = TargetCity.GetLhx();
                var targetCityYhx = TargetCity.GetYhx();
                var xhx = TargetCity.GetXhx();
                AoWHSMap.PlaceHX(A, xhx, targetCityYhx, targetCityLhx);
            }

            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
                TargetCity.ExecuteAnarchy();
                CastingDone(Source, Event.FEventMana);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:858
    public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        if (!Result || TargetCity.FPlayer == 0 || TargetCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CitySpells.pas:867
    public void ShowAnimation(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
    {
        var imageSequenceAnimation = (TImageSequenceAnimation)Animation;
        if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && AoWEngine.FAnimationDetail != 0)
        {
            var random = Random(2);
            PlaySample(ExplosionSFX[random]);
        }
    }
}

public partial class TAnimateRuins : TCityTargetSpell
{
    // CitySpells.pas:915
    public TAnimateRuins(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(AnimateRuinsRStr);
        FSpellID = 76;
        FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy, TCityTargetModeElement.ctmRazed);
    }

    // CitySpells.pas:938
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        TCity TargetCity = default;
        TLocationTextEventLog Log = default;
        TImageSequenceAnimation A = default;
        int Count = default;
        TUnit AUnit = default;
        if (ExecuteCount == 0)
        {
            TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
            Log = new TLocationTextEventLog(null);
            Log.FPlayer = Source.FPlayer;
            var animateRuins = TranslateRStr(AnimateRuinsRStr);
            Log.FText = RtlUnit.Format(TranslateRStr(XCastRStr), [animateRuins]);
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
            var targetCityYhx = TargetCity.GetYhx();
            var targetCityLhx = TargetCity.GetLhx();
            var xhx = TargetCity.GetXhx();
            DistributeLocationEventLog(Log, Source.FPlayer, xhx, targetCityYhx, targetCityLhx, false);
            Log.Release();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.ValidateObservation(Source.FPlayer);
            if (TargetCity.Resource.ImageSequenceList.ImageSequences[20] != null)
            {
                A = new TImageSequenceAnimation(null);
                A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[20]);
                A.SetXhpOffset(A.FXhpOffset - 16);
                A.SetYhpOffset(A.FYhpOffset - 16);
                A.FOnShow = ShowAnimation;
                var lhx2 = TargetCity.GetLhx();
                var yhx2 = TargetCity.GetYhx();
                var targetCityXhx = TargetCity.GetXhx();
                AoWHSMap.PlaceHX(A, targetCityXhx, yhx2, lhx2);
            }

            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((TCityTargetSpellTE)Event).GetSelectedCity();
                TargetCity.SetRace(AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FPlayerRace);
                TargetCity.SetRazed(false);
                var size = TargetCity.GetSize();
                Count = AoWHSMap.Random(size * 2) + 1;
                for (int I = 0; I <= Count; I++)
                {
                    AUnit = new TUnit(null);
                    var random = AoWHSMap.Random(2);
                    AUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(CitySpellsUnit.TAnimateRuins_ExecuteTE__UndeadUnits[random]));
                    var lhx = TargetCity.GetLhx();
                    var yhx = TargetCity.GetYhx();
                    AUnit.Place(TargetCity.GetXhx(), yhx, lhx, Source.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                    AUnit.Release();
                }

                CastingDone(Source, Event.FEventMana);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:924
    public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        return Result && TargetCity.GetRazed();
    }

    // CitySpells.pas:930
    public void ShowAnimation(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
    {
        var imageSequenceAnimation = (TImageSequenceAnimation)Animation;
        if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && AoWEngine.FAnimationDetail != 0)
        {
            var random = Random(2);
            PlaySample(ExplosionSFX[random]);
        }
    }
}

public static partial class CitySpellsUnit
{
    public static FixedArray_int_2 TAnimateRuins_ExecuteTE__UndeadUnits = FixedArray_int_2.From([198, 201]);

    // CitySpells.pas:988
    public static void RegisterCitySpells(TAoWEngine Engine)
    {
        Engine.RegisterEClasses([EClass.Of<TCityEnchantmentSpellTE>(), EClass.Of<TCityTargetSpellTE>(), EClass.Of<TTownGateTE>()]);
        var spellControl = Engine.FGameSet.FSpellControl;
        spellControl.RegisterSpell(new TAnarchy(null));
        spellControl.RegisterSpell(new TTownQuake(null));
        spellControl.RegisterSpell(new TTownGate(null));
        spellControl.RegisterSpell(new TFountainOfLife(null));
        spellControl.RegisterSpell(new TGoldRush(null));
        spellControl.RegisterSpell(new TAntiMagicShell(null));
        spellControl.RegisterSpell(new TWarmonger(null));
        spellControl.RegisterSpell(new TAnimateRuins(null));
    }

    // CitySpells.pas:1004
    public static void UnregisterCitySpells(TAoWEngine Engine)
    {
        Engine.UnregisterEClasses([EClass.Of<TCityEnchantmentSpellTE>(), EClass.Of<TCityTargetSpellTE>(), EClass.Of<TTownGateTE>()]);
        var spellControl = Engine.FGameSet.FSpellControl;
        spellControl.UnregisterSpellID(72);
        // { Original leaves Town Quake ($47) registered. }
        spellControl.UnregisterSpellID(38);
        spellControl.UnregisterSpellID(52);
        spellControl.UnregisterSpellID(53);
        spellControl.UnregisterSpellID(54);
        spellControl.UnregisterSpellID(58);
        spellControl.UnregisterSpellID(76);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
