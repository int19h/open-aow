// Translated from GlobalEnchantmentSpells.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Rtl;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.Sound;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.Engine.EngineUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.GlobalEnchantmentSpells;

// FEnchantment
public partial class TGlobalEnchantmentSpellTE : TSpellTE
{
    public TGlobalEnchantmentSpellTE(TECustomNode owner) : base(owner)
    { }

    // GlobalEnchantmentSpells.pas:389
    public new const int ClassID = 135954;

    public TGlobalEnchantment FEnchantment;

    // GlobalEnchantmentSpells.pas:379
    public override void Destroy()
    {
        if (FEnchantment != null)
        {
            FEnchantment.Release();
            FEnchantment = null;
        }

        base.Destroy();
    }

    // GlobalEnchantmentSpells.pas:394
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FEnchantment, null, 25);
    }

    // GlobalEnchantmentSpells.pas:400
    public override bool Validate(THero source)
    {
        string Reason = "";
        bool Result = base.Validate(source);
        return Result && FEnchantment.CanCast(source.FPlayer, ref Reason);
    }

    // GlobalEnchantmentSpells.pas:408
    public override bool Process(TAbstractUnit source, int value) => base.Process(source, value);

    // NetworkE.pas:230

}

// FAIBasePriority
public partial class TGlobalEnchantmentSpell : TSpell
{
    public int FAIBasePriority;

    // GlobalEnchantmentSpells.pas:413
    public TGlobalEnchantmentSpell(TECustomNode owner) : base(owner)
    {
        FAIBasePriority = -1;
    }

    // GlobalEnchantmentSpells.pas:429
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        if (ExecuteCount == 0)
        {
            ((TGlobalEnchantmentSpellTE)Event).FEnchantment.Cast(Source.FPlayer);
        }

        return false;
    }

    // GlobalEnchantmentSpells.pas:494
    public override void AIPrefetchCastSpellActions(TAbstractUnit Source, TAICastSpellActionList Actions)
    {
        TAICastSpellAction Action;
        bool aiCanCastSpell = AICanCastSpell(Source);
        if (aiCanCastSpell)
        {
            var aiUpkeepPriority = AIUpkeepPriority();
            aiCanCastSpell = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
        }

        if (aiCanCastSpell)
        {
            Action = CreateCastSpellAction();
            SetupCastSpellAction(Source, Action);
            Action.FPriority = FAIBasePriority;
            Action.SetOwner(Actions);
        }
    }

    // GlobalEnchantmentSpells.pas:507
    public override bool AIExecuteCastSpellAction(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        if (AICanCastSpell(Source))
        {
            var aiUpkeepPriority = AIUpkeepPriority();
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
            var hero = (THero)Source;
            if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Result = this.Activate(Source);
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

    // GlobalEnchantmentSpells.pas:435
    public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
    {
        TGlobalEnchantment Enchantment;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            if (!AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(TGlobalEnchantmentType.getSpellWard))
            {
                Enchantment = CreateEnchantment();
                if (Enchantment != null)
                {
                    SetupEnchantment(Source, Enchantment);
                    Result = Enchantment.CanCast(Source.FPlayer, ref MessageText);
                    Enchantment.Release();
                }
            }
            else
            {
                Result = false;
                MessageText = TranslateRStr(CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr);
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:458
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        TGlobalEnchantmentSpellTE Event;
        string Reason = "";
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TGlobalEnchantmentSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.FEnchantment = CreateEnchantment();
            SetupEnchantment(Source, Event.FEnchantment);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:476
    public virtual bool AICanCastSpell(TAbstractUnit Source)
    {
        string Reason = "";
        bool Result = FAIBasePriority != -1 && ((THero)Source).CanCastSpell(FSpellID, ref Reason);
        return Result && CanActivate(Source, ref Reason);
    }

    // GlobalEnchantmentSpells.pas:419
    public virtual TGlobalEnchantment CreateEnchantment() => null;

    // GlobalEnchantmentSpells.pas:424
    public virtual void SetupEnchantment(TAbstractUnit Source, TGlobalEnchantment Enchantment)
    {
        Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
    }

    // GlobalEnchantmentSpells.pas:484
    public int AIUpkeepPriority()
    {
        TGlobalEnchantment Enchantment;
        // { Original creates the base enchantment here. }
        Enchantment = new TGlobalEnchantment(null);
        int Result = Enchantment.GetAIUpkeepPriority();
        Enchantment.Release();
        return Result;
    }
}

public partial class TEnchantedRoadsEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:535
    public new const int ClassID = 135955;

    // GlobalEnchantmentSpells.pas:527
    public TEnchantedRoadsEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(EnchantedRoadsRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:540
    public override int GetAIUpkeepPriority() => 900;

    // GlobalEnchantmentSpells.pas:563
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEnchantedRoads));
        return true;
    }

    // GlobalEnchantmentSpells.pas:545
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result && AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petEnchantedRoads))
        {
            var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
            var name = GetName();
            Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:555
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEnchantedRoads));
        return true;
    }

}

public partial class TEnchantedRoads : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:571
    public TEnchantedRoads(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(EnchantedRoadsRStr);
        FSpellID = 44;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = TMagicSphere.msEarth;
        FCastDuration = 20;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:613
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TAnimation Anim = default;
        TTwinkleAnimation Twinkle = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new TAnimation(null);
        Anim.FAnimationLevel = 27;
        Anim.FOnShow = ShowAnimation;
        Anim.FAnimationMap = AoWHSMap;
        Anim.Activate();
        Twinkle = new TTwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.FSpellControl.GetSpell(44).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
        AoWHSMap.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:636
    public override TGlobalEnchantment CreateEnchantment() => new TEnchantedRoadsEnchantment(null);

    public void ShowAnimation(TAnimation anim, int xhpSurface, int yhpSurface, in TSurfaceDesc surface)
    {
        var map = AoWHSMap;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
        for (int x = map.ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
            xhp = xhp - xhpSurface + 16;
            yhp = yhp - yhpSurface + 16;
            for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
            {
                var field = (TAoWMapField)level.GetField(x, y);
                if (field.OverlayType == 4 && map.FSeatedPlayer != -1 && field.GetExplored(map.FSeatedPlayer))
                    FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - ((field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255)) & 15), surface);
            }
        }
        if (anim.FFrameCounter == 50) anim.Destroy();
    }
}

public partial class TAirMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:649
    public new const int ClassID = 135966;

    // GlobalEnchantmentSpells.pas:641
    public TAirMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(AirMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:654
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:679
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petAirMastery));
        // { Original uses Earth. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msEarth);
        return true;
    }

    // GlobalEnchantmentSpells.pas:659
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petAirMastery))
            {
                var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:670
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petAirMastery));
        // { Original uses Earth. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msEarth);
        return true;
    }

}

public partial class TAirMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:688
    public TAirMastery(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(AirMasteryRStr);
        FSpellID = 60;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        // { Original also assigns Earth to Air Mastery. }
        FMagicSphere = TMagicSphere.msEarth;
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:727
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TAnimation Anim = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msAir);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new TAnimation(null);
        Anim.FAnimationLevel = 28;
        Anim.FOnShow = ShowAnimation;
        Anim.FAnimationMap = AoWHSMap;
        Anim.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
        Anim.Activate();
        AoWHSSet.FSpellControl.GetSpell(60).FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:747
    public override TGlobalEnchantment CreateEnchantment() => new TAirMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:701
    public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Count;
        int X;
        int Y;
        int Speed;
        int Image;
        if (AoWHSMap.ViewL == 0)
        {
            RandSeed = AoWHSMap.Frame - Anim.FFrameCounter;
            switch ((int)AoWEngine.FAnimationDetail)
            {
                case 0:
                    Count = 5;
                    break;
                case 2:
                    Count = 20;
                    break;
                default:
                    Count = 10;
                    break;
            }

            for (int I = 0; I <= Count - 1; I++)
            {
                X = Random(Anim.FTag & 65535);
                Y = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
                Speed = Random(15) + 5;
                Image = Random(3);
                X += (Anim.FFrameCounter - 45) * Speed * 2;
                Y -= (Anim.FFrameCounter - 45) * Speed;
                FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
            }
        }

        if (Anim.FFrameCounter == 90)
        {
            Anim.Destroy();
        }
    }
}

public partial class TWaterMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:760
    public new const int ClassID = 135956;

    // GlobalEnchantmentSpells.pas:752
    public TWaterMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(WaterMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:765
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:791
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petWaterMastery));
        AoWHSMap.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msFire);
        return true;
    }

    // GlobalEnchantmentSpells.pas:770
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petWaterMastery))
            {
                var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:781
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petWaterMastery));
        AoWHSMap.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msFire);
        return true;
    }

}

public partial class TWaterMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:806
    public TWaterMastery(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(WaterMasteryRStr);
        FSpellID = 45;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msWater;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:819
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        // { Original ignores the event step. }
        base.ExecuteTE(Event, Source, 0);
        Source.GetLocation(ref X, ref Y, ref L);
        GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msWater);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        FCastSounds.Play(0);
        AoWHSMap.AddEffect(TworldFXType.fxWaterMastery);
        CastingDone(Source, FBaseManaCost);
        return false;
    }

    // GlobalEnchantmentSpells.pas:801
    public override TGlobalEnchantment CreateEnchantment() => new TWaterMasteryEnchantment(null);
}

// FDaysActive
public partial class THatredEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:841
    public new const int ClassID = 135957;

    public int FDaysActive;

    // GlobalEnchantmentSpells.pas:851
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDaysActive, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("DaysActive", ref FDaysActive);
    }

    // GlobalEnchantmentSpells.pas:833
    public THatredEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(HatredRStr);
        FEnchantmentType = TGlobalEnchantmentType.getHatred;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:863
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
        {
            NewDay();
        }
    }

    // GlobalEnchantmentSpells.pas:846
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:876
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        if (FDaysActive > 10)
        {
            AoWHSMap.FRaces.FRelationOffset += 40;
        }
        else
        {
            AoWHSMap.FRaces.FRelationOffset += FDaysActive * 4;
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:869
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        NewDay();
        return true;
    }

    // GlobalEnchantmentSpells.pas:857
    public void NewDay()
    {
        FDaysActive += 1;
        if (FDaysActive <= 10)
        {
            AoWHSMap.FRaces.FRelationOffset -= 4;
        }
    }

}

public partial class THatred : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:889
    public THatred(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(HatredRStr);
        FSpellID = 46;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 20;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msDeath;
    }

    // GlobalEnchantmentSpells.pas:900
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.AddEffect(TworldFXType.fxHatred);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:884
    public override TGlobalEnchantment CreateEnchantment() => new THatredEnchantment(null);
}

// FDaysActive
public partial class THarmonyEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:921
    public new const int ClassID = 135958;

    public int FDaysActive;

    // GlobalEnchantmentSpells.pas:931
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDaysActive, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("DaysActive", ref FDaysActive);
    }

    // GlobalEnchantmentSpells.pas:913
    public THarmonyEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(HarmonyRStr);
        FEnchantmentType = TGlobalEnchantmentType.getHarmony;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:943
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
        {
            NewDay();
        }
    }

    // GlobalEnchantmentSpells.pas:926
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:956
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        if (FDaysActive > 10)
        {
            AoWHSMap.FRaces.FRelationOffset -= 40;
        }
        else
        {
            AoWHSMap.FRaces.FRelationOffset -= FDaysActive * 4;
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:949
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        NewDay();
        return true;
    }

    // GlobalEnchantmentSpells.pas:937
    public void NewDay()
    {
        FDaysActive += 1;
        if (FDaysActive <= 10)
        {
            AoWHSMap.FRaces.FRelationOffset += 4;
        }
    }

}

public partial class THarmony : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:969
    public THarmony(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(HarmonyRStr);
        FSpellID = 47;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 15;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msLife;
    }

    // GlobalEnchantmentSpells.pas:980
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.AddEffect(TworldFXType.fxTranquility);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:964
    public override TGlobalEnchantment CreateEnchantment() => new THarmonyEnchantment(null);
}

public partial class TPowerLeakEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1001
    public new const int ClassID = 135959;

    // GlobalEnchantmentSpells.pas:993
    public TPowerLeakEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(PowerLeakRStr);
        FEnchantmentType = TGlobalEnchantmentType.getPowerLeak;
        FDispelMana = 25;
    }

    // GlobalEnchantmentSpells.pas:1006
    public override int GetAIUpkeepPriority() => 750;

    // GlobalEnchantmentSpells.pas:1021
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        for (int I = AoWHSMap.FPlayers.Count - 1; I >= 0; I--)
        {
            AoWHSMap.FPlayers.GetPlayers(I).FMagicControl.Update();
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:1011
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        for (int I = AoWHSMap.FPlayers.Count - 1; I >= 0; I--)
        {
            AoWHSMap.FPlayers.GetPlayers(I).FMagicControl.Update();
        }

        return true;
    }

}

public partial class TPowerLeak : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1036
    public TPowerLeak(TECustomNode owner) : base(owner)
    {
        FSpellType = TSpellType.stGlobal;
        FSpellName = TranslateRStr(PowerLeakRStr);
        FSpellID = 48;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msCosmos;
        FCastDuration = 20;
        FAIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:1049
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TTwinkleAnimation Anim;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Anim = new TTwinkleAnimation(null);
        Anim.Setup(AoWHSSet.FSpellControl.GetSpell(48).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmImplode), 100, 64, 50, 30);
        AoWHSMap.PlaceHX(Anim, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1031
    public override TGlobalEnchantment CreateEnchantment() => new TPowerLeakEnchantment(null);
}

// FWidth, FHeight, FOriginalTerrain
public partial class TFloodControl : TEObject
{
    public int FWidth;
    public int FHeight;
    public sbyte[] FOriginalTerrain = [];

    // GlobalEnchantmentSpells.pas:1072
    public override void Destroy()
    {
        if (FOriginalTerrain.Length != 0)
        {
            Array.Resize(ref FOriginalTerrain, 0);
        }

        base.Destroy();
    }

    // GlobalEnchantmentSpells.pas:1078
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FWidth, 20);
        Stream.rwInteger(ref FHeight, 21);
        if (FWidth != 0)
        {
            if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
            {
                Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
            }

            Stream.rwData(MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, FWidth * FHeight * 2)), FWidth * FHeight * 2, 22);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("Width", ref FWidth);
        persister.Int("Height", ref FHeight);
        if (FWidth != 0)
        {
            int count = FWidth * FHeight * 2;
            if (persister.Loading) Array.Resize(ref FOriginalTerrain, count);
            persister.Bytes("OriginalTerrain", MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, count)));
        }
    }

    // GlobalEnchantmentSpells.pas:1066
    public TFloodControl(TECustomNode owner) : base(owner)
    {
        FOriginalTerrain = [];
    }

    // GlobalEnchantmentSpells.pas:1090
    public void Setup()
    {
        FWidth = AoWHSMap.MapContainer.Width;
        FHeight = AoWHSMap.MapContainer.Height;
        Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
        Array.Clear(FOriginalTerrain);
    }

    // GlobalEnchantmentSpells.pas:1098
    public void Flood()
    {
        TMapLevel MapLevel;
        THNWalkData Walk = default;
        TMapField MapF;
        int Index;
        THexagonSprite RoadHS;
        sbyte TerrainType;
        AoWHSMap.LockMapChangedEvent();
        MapLevel = AoWHSMap.MapContainer.GetMapLevel(0);
        // { Original includes the last row when walking neighbouring hexagons. }
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (MapLevel.GetField(X, Y).TerrainType == 0)
                {
                    InitHNWalk(ref Walk, X, Y, 1);
                    while (Walk.Radius == 1)
                    {
                        MapF = MapLevel.GetField(Walk.X, Walk.Y);
                        if ((!TSet<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || (MapF.TerrainType == 0 && MapF.OverlayType == 5))
                        {
                            Index = (FWidth * Walk.Y + Walk.X) * 2;
                            if (FOriginalTerrain[Index] == 0 && FOriginalTerrain[Index + 1] == 0 && MapF.FindNoneTransparentHS(typeof(TStructure)) == null)
                            {
                                FOriginalTerrain[Index] = MapF.TerrainType;
                                FOriginalTerrain[Index + 1] = MapF.OverlayType;
                            }

                            if (TSet<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)MapF.OverlayType))
                            {
                                RoadHS = MapF.FindOwnedHS(typeof(TAbstractRoad));
                                if (RoadHS != null)
                                {
                                    RoadHS.Destroy();
                                }
                            }
                        }

                        WalkToNextHN(ref Walk);
                    }
                }
            }
        }

        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (FOriginalTerrain[Index] != 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    TerrainType = MapF.TerrainType;
                    if (TerrainType != 0)
                    {
                        MapF.TerrainType = 0;
                        MapF.TerrainChanged();
                    }
                }

                Index += 2;
            }
        }

        AoWHSMap.UnlockMapChangedEvent();
        AoWHSMap.InvalidateMap();
    }

    // GlobalEnchantmentSpells.pas:1160
    public void Restore()
    {
        TEObjectList Resources;
        TList Candidates;
        int Index;
        TMapField MapF;
        TMapLevel MapLevel;
        sbyte Overlay;
        TMapObjectResource Resource;
        AoWHSMap.LockMapChangedEvent();
        MapLevel = AoWHSMap.MapContainer.GetMapLevel(0);
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                if (FOriginalTerrain[Index] != 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    if (TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)MapF.TerrainType))
                    {
                        MapF.TerrainType = FOriginalTerrain[Index];
                        MapF.TerrainChanged();
                    }
                }

                Index += 2;
            }
        }

        Resources = AoWHSSet.MapObjectResourceList.FChildren;
        Candidates = new TList();
        for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
        {
            Index = (FWidth * Y + 1) * 2;
            for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
            {
                Overlay = FOriginalTerrain[Index + 1];
                if (Overlay > 0)
                {
                    MapF = MapLevel.GetField(X, Y);
                    if (TSet<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)Overlay))
                    {
                        AoWHSSet.RoadControl.Place(AoWHSMap.MapContainer, (sbyte)X, (sbyte)Y, 0, MapF.TerrainType);
                    }
                    else
                    {
                        Candidates.Clear();
                        for (int I = 0, lastI = Resources.Count - 1; I <= lastI; I++)
                        {
                            Resource = (TMapObjectResource)Resources.List[I];
                            if (Resource.EClass.ClassID == 131431 && Resource.TerrainList.UsedTerrain() == 1 && Resource.TerrainList.Terrain[0] == MapF.TerrainType && Overlay == Resource.TerrainList.Overlay[0])
                            {
                                Candidates.Add(Resource);
                            }
                        }

                        if (Candidates.Count != 0)
                        {
                            ((TMapObjectResource)Candidates[AoWHSMap.Random(Candidates.Count)]).PlaceResourceUser(AoWHSMap, (sbyte)X, (sbyte)Y, 0);
                        }
                    }
                }

                Index += 2;
            }
        }

        Candidates.Free();
        AoWHSMap.UnlockMapChangedEvent();
        AoWHSMap.InvalidateMap();
    }

}

// FRemoved, FRestoreDays, FDispelPlayer, FTurnsActive, FFloodControl
public partial class TFloodEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1240
    public new const int ClassID = 135960;

    public bool FRemoved;
    public sbyte FRestoreDays;
    public sbyte FDispelPlayer;
    // alignment
    public int FTurnsActive;
    public TFloodControl FFloodControl;

    // GlobalEnchantmentSpells.pas:1234
    public override void Destroy()
    {
        FFloodControl.Free();
        base.Destroy();
    }

    // GlobalEnchantmentSpells.pas:1250
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTurnsActive, 20);
        Stream.rwEObject(FFloodControl, 21);
        Stream.rwBool(ref FRemoved, 22);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDispelPlayer), 23);
        Stream.rwShortintEx(ref FRestoreDays, -1, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("TurnsActive", ref FTurnsActive);
        persister.Object("FloodControl", FFloodControl);
        persister.Bool("Removed", ref FRemoved);
        PersistSupport.UnsignedByte(persister, "DispelPlayer", ref FDispelPlayer);
        persister.SByte("RestoreDays", ref FRestoreDays, -1);
    }

    // GlobalEnchantmentSpells.pas:1224
    public TFloodEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(FloodRStr);
        FEnchantmentType = TGlobalEnchantmentType.getFlood;
        FFloodControl = new TFloodControl(null);
        FDispelMana = 100;
        FRestoreDays = -1;
    }

    // GlobalEnchantmentSpells.pas:1302
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001987)
        {
            if (Msg.Payload.Int == FPlayerIndex && !FRemoved)
            {
                NewTurn();
            }

            if (Msg.Payload.Int == FDispelPlayer && FRemoved)
            {
                FRestoreDays = (sbyte)(FRestoreDays - 1);
                if (FRestoreDays <= 0)
                {
                    FFloodControl.Restore();
                    base.Deactivate((byte)FDispelPlayer);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1268
    public override string GetName()
    {
        string Result = base.GetName();
        if (FRemoved)
        {
            Result = Result + " (" + LowerCase(FmtDaysLeftStr(FRestoreDays)) + ")";
        }
        else
        {
            if (DaysBeforeActive() != 0)
            {
                var daysBeforeActive = DaysBeforeActive();
                Result = RtlUnit.Format(XinYdaysRStr, [Result, daysBeforeActive]);
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1276
    public override int GetUpkeep()
    {
        int Result;
        if (!FRemoved)
        {
            Result = base.GetUpkeep();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1245
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:1328
    public override bool CanDeactivate(byte Player, ref string Reason)
    {
        bool Result;
        if (!FRemoved)
        {
            Result = base.CanDeactivate(Player, ref Reason);
        }
        else
        {
            Reason = RtlUnit.Format(TranslateRStr(XAlreadyRemovedRStr), [FName]);
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1338
    public override bool Deactivate(byte Player)
    {
        bool Result;
        if (!FRemoved)
        {
            FDispelPlayer = (sbyte)Player;
            FRemoved = true;
            if (AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous)
            {
                FRestoreDays = 2;
            }
            else
            {
                FRestoreDays = 1;
            }

            AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.Update();
            AoWHSMap.FGlobalMagicControl.Update();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1320
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        FRemoved = false;
        FFloodControl.Setup();
        return true;
    }

    // GlobalEnchantmentSpells.pas:1260
    public int DaysBeforeActive()
    {
        int Result;
        if (AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous && FTurnsActive < 2)
        {
            Result = 2 - FTurnsActive;
        }
        else
        {
            if (AoWHSMap.FPlayMode == TPlayMode.pmTurn && FTurnsActive == 0)
            {
                Result = 1;
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1282
    public void NewTurn()
    {
        if (!FRemoved)
        {
            FTurnsActive += 1;
            if (AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous)
            {
                if (FTurnsActive > 1 && FTurnsActive < 4)
                {
                    FFloodControl.Flood();
                }
                else
                {
                    if (FTurnsActive == 4)
                    {
                        if (AoWHSMap.Random(2) == 1)
                        {
                            FFloodControl.Flood();
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.FPlayMode == TPlayMode.pmTurn)
                {
                    if (FTurnsActive < 3)
                    {
                        FFloodControl.Flood();
                    }
                    else
                    {
                        if (FTurnsActive == 3)
                        {
                            if (AoWHSMap.Random(2) == 1)
                            {
                                FFloodControl.Flood();
                            }
                        }
                    }
                }
            }
        }
    }

}

public partial class TFlood : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1356
    public TFlood(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(FloodRStr);
        FSpellID = 50;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msWater;
    }

    // GlobalEnchantmentSpells.pas:1370
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TTwinkleAnimation Twinkle;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Twinkle = new TTwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.FSpellControl.GetSpell(50).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
        AoWHSMap.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1353
    public override TGlobalEnchantment CreateEnchantment() => new TFloodEnchantment(null);

    // GlobalEnchantmentSpells.pas:1367
    public void ShowAnimation(int Anim, int XhpSurf, int YhpSurf, int SurfaceDesc)
    {
    }
}

public partial class TLifeMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1395
    public new const int ClassID = 135967;

    // GlobalEnchantmentSpells.pas:1387
    public TLifeMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(LifeMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1398
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1428
    public override bool Deactivate(byte Player)
    {
        TEMsg Msg = default;
        base.Deactivate(Player);
        // { Original doubles Death. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msDeath);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petLifeMastery));
        InitMsg(ref Msg, 537002240, this);
        AoWHSMap.SendMsg(ref Msg, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1401
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petLifeMastery))
            {
                var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1412
    public override bool Activate(sbyte Player)
    {
        TPlayer APlayer;
        base.Activate(Player);
        // { Original doubles Death. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msDeath);
        APlayer = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petLifeMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetLifeMasteryBlessing();
            }
        }

        return true;
    }

}

public partial class TLifeMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1441
    public TLifeMastery(TECustomNode owner) : base(owner)
    {
        FSpellType = TSpellType.stGlobal;
        FSpellName = TranslateRStr(LifeMasteryRStr);
        FSpellID = 61;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        // { Original assigns Cosmos to Life Mastery. }
        FMagicSphere = TMagicSphere.msCosmos;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1457
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TTwinkleAnimation Twinkle;
        base.ExecuteTE(Event, Source, ExecuteCount);
        GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msLife);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Twinkle = new TTwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.FSpellControl.GetSpell(61).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
        AoWHSMap.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1454
    public override TGlobalEnchantment CreateEnchantment() => new TLifeMasteryEnchantment(null);
}

public partial class TDeathMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1483
    public new const int ClassID = 135968;

    // GlobalEnchantmentSpells.pas:1475
    public TDeathMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(DeathMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1568
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            if (Msg.Payload.Int != 0)
            {
                if (AoWHSMap.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FEnemyEnchantments.Contains(TPlayerEnchantmentType.petDeathMastery))
                {
                    NewTurn((sbyte)Msg.Payload.Int);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1486
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1509
    public override bool Deactivate(byte Player)
    {
        TEMsg Msg = default;
        base.Deactivate(Player);
        // { Original doubles Life. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msLife);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petDeathMastery));
        InitMsg(ref Msg, 537002241, this);
        AoWHSMap.SendMsg(ref Msg, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1489
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petDeathMastery))
            {
                var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1500
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        // { Original doubles Life. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msLife);
        var player = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petDeathMastery));
        return true;
    }

    // GlobalEnchantmentSpells.pas:1522
    public void NewTurn(sbyte Player)
    {
        int Index;
        byte[] Flags;
        int Mask;
        TMapLevel MapLevel;
        THNWalkData Walk = default;
        bool Border;
        Flags = [];
        Mask = 1 << (Player - 1);
        for (int L = 0, last = AoWHSMap.MapContainer.LevelCount - 1; L <= last; L++)
        {
            MapLevel = AoWHSMap.MapContainer.GetMapLevel(L);
            Index = 0;
            Array.Resize(ref Flags, MapLevel.Width * MapLevel.Height);
            Array.Clear(Flags);
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if ((((TAoWMapField)MapLevel.GetField(X, Y)).FFieldFlags & Mask) != 0)
                    {
                        Border = false;
                        InitHNWalk(ref Walk, X, Y, 1);
                        while (Walk.Radius == 1 && !Border)
                        {
                            if (Walk.X >= 0 && Walk.X < MapLevel.Width && Walk.Y >= 0 && Walk.Y < MapLevel.Height)
                            {
                                Border = (((TAoWMapField)MapLevel.GetField(Walk.X, Walk.Y)).FFieldFlags & Mask) == 0;
                            }

                            WalkToNextHN(ref Walk);
                        }

                        if (Border)
                        {
                            Flags[Index] = 1;
                        }
                    }

                    Index += 1;
                }
            }

            Index = 0;
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if (Flags[Index] != 0)
                    {
                        ((TAoWMapField)MapLevel.GetField(X, Y)).SetExplored(Player, false);
                    }

                    Index += 1;
                }
            }
        }

        AoWHSMap.UpdateVisibility();
        Array.Resize(ref Flags, 0);
    }

}

public partial class TDeathMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1577
    public TDeathMastery(TECustomNode owner) : base(owner)
    {
        FSpellType = TSpellType.stGlobal;
        FSpellName = TranslateRStr(DeathMasteryRStr);
        FSpellID = 62;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        // { Original assigns Cosmos to Death Mastery. }
        FMagicSphere = TMagicSphere.msCosmos;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1618
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msDeath);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        var animation = new TAnimation(null);
        animation.FAnimationLevel = 28;
        animation.FOnShow = ShowAnimation;
        animation.FAnimationMap = AoWHSMap;
        animation.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
        animation.Activate();

        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1590
    public override TGlobalEnchantment CreateEnchantment() => new TDeathMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1593
    public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Count;
        int X;
        int Y;
        int Speed;
        int D;
        int Image;
        RandSeed = AoWHSMap.Frame - Anim.FFrameCounter;
        switch (AoWEngine.FAnimationDetail)
        {
            case 0:
                Count = 5;
                break;
            case 2:
                Count = 20;
                break;
            default:
                Count = 10;
                break;
        }

        for (int I = 0; I <= Count - 1; I++)
        {
            X = Random(Anim.FTag & 65535);
            Y = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
            Speed = Random(15) + 5;
            Image = Random(3);
            // { Preserve the original image/displacement register lifetimes. }
            D = (Anim.FFrameCounter - 45) * Speed;
            X -= D * 2;
            Y += D;
            FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
        }

        if (Anim.FFrameCounter == 90)
        {
            Anim.Destroy();
        }
    }
}

public partial class TFireMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1647
    public new const int ClassID = 135969;

    // GlobalEnchantmentSpells.pas:1639
    public TFireMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(FireMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getFireMastery;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1724
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001987)
        {
            if (Msg.Payload.Int != 0)
            {
                if (AoWHSMap.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petFireMastery))
                {
                    UpdateFire(2, !AoWHSMap.FRequestCombatType);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1650
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1680
    public override bool Deactivate(byte Player)
    {
        TPlayer APlayer;
        base.Deactivate(Player);
        APlayer = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petFireMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).ResetFireMasteryProtection();
            }
        }

        // { Original doubles Water. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msWater);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1653
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result;
        if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petFireMastery))
        {
            var xIsAlreadyActive = TranslateRStr(XIsAlreadyActiveRStr);
            var name = GetName();
            Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1663
    public override bool Activate(sbyte Player)
    {
        TPlayer APlayer;
        base.Activate(Player);
        // { Original doubles Water. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msWater);
        APlayer = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petFireMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetFireMasteryProtection();
            }
        }

        UpdateFire(15, true);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1696
    public void UpdateFire(int Factor, bool Explosions)
    {
        int Chance;
        TMapLevel MapLevel;
        TFire Fire;
        RandSeed = AoWHSMap.Random(16777215);
        for (int L = 0, last = AoWHSMap.MapContainer.LevelCount - 1; L <= last; L++)
        {
            MapLevel = AoWHSMap.MapContainer.GetMapLevel(L);
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    Chance = IgnitionChance((TAoWMapField)MapLevel.GetField(X, Y));
                    if (Chance > 0)
                    {
                        if (AoWHSMap.Random(10000) < Chance * Factor)
                        {
                            Fire = AoWHSSet.FFireControl.PlaceFire(X, Y, L, TFireType.ftLarge);
                            if (Fire != null)
                            {
                                if (Explosions)
                                {
                                    Fire.SetupExplosion(Random(30));
                                }

                                Fire.FTurnsLeft = 3;
                            }
                        }
                    }
                }
            }
        }
    }

}

public partial class TFireMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1733
    public TFireMastery(TECustomNode owner) : base(owner)
    {
        FSpellType = TSpellType.stGlobal;
        FSpellName = TranslateRStr(FireMasteryRStr);
        FSpellID = 63;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msFire;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1792
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msFire);
            AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            var animation = new TAnimation(null);
            animation.FAnimationLevel = 27;
            animation.FOnShow = ShowAnimation;
            animation.FAnimationMap = AoWHSMap;
            animation.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
            animation.Activate();

            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
        }

        if (ExecuteCount == 20)
        {
            base.ExecuteTE(Event, Source, 0);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1746
    public override TGlobalEnchantment CreateEnchantment() => new TFireMasteryEnchantment(null);

    public void ShowAnimation(TAnimation anim, int xhpSurface, int yhpSurface, in TSurfaceDesc surface)
    {
        int chance = AoWEngine.FAnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
        var map = AoWHSMap;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
        for (int x = map.ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
            xhp = xhp - xhpSurface + 16;
            yhp = yhp - yhpSurface + 16;
            for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
            {
                var field = (TAoWMapField)level.GetField(x, y);
                if (field.TerrainType is 7 or 8 or 15 || map.FSeatedPlayer == -1 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) continue;
                RandSeed = field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255) * 65536;
                if (Random(100) >= chance) continue;
                int delay = Random(40);
                FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - delay, surface);
                if (anim.FFrameCounter == delay && AoWEngine.FAnimationDetail != 0)
                    SoundUnit.PlaySample(ExplosionSFX[x % 2]);
            }
        }
        if (anim.FFrameCounter == 90) anim.Destroy();
    }
}

public partial class TEarthMasteryEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1828
    public new const int ClassID = 135970;

    // GlobalEnchantmentSpells.pas:1820
    public TEarthMasteryEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(EarthMasteryRStr);
        FEnchantmentType = TGlobalEnchantmentType.getNone;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1831
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1861
    public override bool Deactivate(byte Player)
    {
        TPlayer APlayer;
        base.Deactivate(Player);
        APlayer = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEarthMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).ResetEarthMasteryStoneSkin();
            }
        }

        // { Original doubles Air. }
        AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msAir);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1834
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petEarthMastery))
            {
                Reason = RtlUnit.Format(TranslateRStr(XIsAlreadyActiveRStr), [FName]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1845
    public override bool Activate(sbyte Player)
    {
        TPlayer APlayer;
        base.Activate(Player);
        // { Original doubles Air. }
        AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msAir);
        APlayer = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEarthMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetEarthMasteryStoneSkin();
            }
        }

        return true;
    }

}

public partial class TEarthMastery : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1877
    public TEarthMastery(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(EarthMasteryRStr);
        FSpellID = 64;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = TMagicSphere.msEarth;
        FCastDuration = 40;
        FAIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1918
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        GlobalEnchantmentSpellsUnit.NotifySpellSphere(Source.FPlayer, TMagicSphere.msEarth);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.AddEffect(TworldFXType.fxEarthMastery);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1932
    public override TGlobalEnchantment CreateEnchantment() => new TEarthMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1890
    public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int MaxX;
        int MaxY;
        int Xhp = default;
        int Yhp = default;
        TMapLevel MapLevel;
        TAoWMapField MapF;
        MapLevel = AoWHSMap.MapContainer.GetMapLevel(AoWHSMap.ViewL);
        MaxX = AoWHSMap.ViewXhx + AoWHSMap.ViewWidthHX - 1;
        MaxY = AoWHSMap.ViewYhx + AoWHSMap.ViewHeightHX - 1;
        for (int X = AoWHSMap.ViewXhx; X <= MaxX; X++)
        {
            HXtoHP(X, AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf + 16;
            Yhp = Yhp - YhpSurf + 16;
            MapF = (TAoWMapField)MapLevel.GetField(X, AoWHSMap.ViewYhx);
            for (int Y = AoWHSMap.ViewYhx; Y <= MaxY; Y++)
            {
                // The original frame offset uses the field address.
                if (MapF.OverlayType == 4)
                {
                    FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - FrameFixtureInputs.FieldFrameOffset(MapF), SurfaceDesc);
                }

                // The original steps the field pointer by FHNOffsets[0, 4], one row down. The step
                // after the last row is never read there, and the guard keeps it from reading here.
                if (Y < MaxY)
                {
                    MapF = (TAoWMapField)MapLevel.GetField(X, Y + 1);
                }

                Yhp += 32;
            }
        }

        if (Anim.FFrameCounter == 50)
        {
            Anim.Destroy();
        }
    }
}

public partial class TSpellWardEnchantment : TGlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1943
    public new const int ClassID = 135971;

    // GlobalEnchantmentSpells.pas:1935
    public TSpellWardEnchantment(TECustomNode owner) : base(owner)
    {
        FName = TranslateRStr(SpellWardRStr);
        FEnchantmentType = TGlobalEnchantmentType.getSpellWard;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:1946
    public override int GetAIUpkeepPriority() => 500;

}

public partial class TSpellWard : TGlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1949
    public TSpellWard(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(SpellWardRStr);
        FSpellID = 65;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 3;
        FMagicSphere = TMagicSphere.msEarth;
    }

    // GlobalEnchantmentSpells.pas:1960
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        AoWHSMap.AddEffect(TworldFXType.fxSpellWard);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1973
    public override TGlobalEnchantment CreateEnchantment() => new TSpellWardEnchantment(null);
}

public static partial class GlobalEnchantmentSpellsUnit
{
    // GlobalEnchantmentSpells.pas:1976
    public static void RegisterGlobalEnchantmentSpells(TAoWEngine AEngine)
    {
        TSpellControl Control;
        Control = AEngine.FGameSet.FSpellControl;
        AEngine.RegisterEClasses([EClass.Of<TEnchantedRoadsEnchantment>(), EClass.Of<TGlobalEnchantmentSpellTE>(), EClass.Of<TGlobalEnchantmentEventLog>()]);
        AEngine.RegisterEClasses([EClass.Of<TWaterMasteryEnchantment>(), EClass.Of<THatredEnchantment>(), EClass.Of<THarmonyEnchantment>(), EClass.Of<TPowerLeakEnchantment>(), EClass.Of<TFloodEnchantment>(), EClass.Of<TAirMasteryEnchantment>()]);
        AEngine.RegisterEClasses([EClass.Of<TLifeMasteryEnchantment>(), EClass.Of<TDeathMasteryEnchantment>(), EClass.Of<TFireMasteryEnchantment>(), EClass.Of<TEarthMasteryEnchantment>(), EClass.Of<TSpellWardEnchantment>()]);
        Control.RegisterSpell(new TEnchantedRoads(null));
        Control.RegisterSpell(new THatred(null));
        Control.RegisterSpell(new THarmony(null));
        Control.RegisterSpell(new TPowerLeak(null));
        Control.RegisterSpell(new TFlood(null));
        Control.RegisterSpell(new TAirMastery(null));
        Control.RegisterSpell(new TLifeMastery(null));
        Control.RegisterSpell(new TDeathMastery(null));
        Control.RegisterSpell(new TFireMastery(null));
        Control.RegisterSpell(new TEarthMastery(null));
        Control.RegisterSpell(new TWaterMastery(null));
        Control.RegisterSpell(new TSpellWard(null));
    }

    // GlobalEnchantmentSpells.pas:1998
    public static void UnregisterGlobalEnchantmentSpells(TAoWEngine AEngine)
    {
        TSpellControl Control;
        Control = AEngine.FGameSet.FSpellControl;
        Control.UnregisterSpellID(44);
        Control.UnregisterSpellID(46);
        Control.UnregisterSpellID(47);
        Control.UnregisterSpellID(48);
        Control.UnregisterSpellID(50);
        Control.UnregisterSpellID(60);
        Control.UnregisterSpellID(61);
        Control.UnregisterSpellID(62);
        Control.UnregisterSpellID(63);
        Control.UnregisterSpellID(64);
        Control.UnregisterSpellID(45);
        Control.UnregisterSpellID(65);
        AEngine.UnregisterEClasses([EClass.Of<TEnchantedRoadsEnchantment>(), EClass.Of<TGlobalEnchantmentSpellTE>(), EClass.Of<TGlobalEnchantmentEventLog>()]);
        AEngine.UnregisterEClasses([EClass.Of<TWaterMasteryEnchantment>(), EClass.Of<THatredEnchantment>(), EClass.Of<THarmonyEnchantment>(), EClass.Of<TPowerLeakEnchantment>(), EClass.Of<TFloodEnchantment>(), EClass.Of<TAirMasteryEnchantment>()]);
        AEngine.UnregisterEClasses([EClass.Of<TLifeMasteryEnchantment>(), EClass.Of<TDeathMasteryEnchantment>(), EClass.Of<TFireMasteryEnchantment>(), EClass.Of<TEarthMasteryEnchantment>(), EClass.Of<TSpellWardEnchantment>()]);
    }

    // GlobalEnchantmentSpells.pas:369
    public static void NotifySpellSphere(sbyte Player, TMagicSphere Sphere)
    {
        TEMsg Msg = default;
        InitMsg(ref Msg, 537002242, null);
        Msg.Param[0] = Player;
        Msg.Param[1] = (int)Sphere;
        AoWHSMap.SendMsg(ref Msg, null);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
