// Translated from GlobalEnchantmentSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.GlobalEnchantmentSpells
{
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
        public override bool Validate(THero A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            Result = base.Validate(A1);
            bool bool__1 = Result;
            if (bool__1)
            {
                var call__2 = FEnchantment.CanCast(A1.FPlayer, ref Reason);
                bool__1 = call__2;
            }

            Result = bool__1;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:408
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Result = base.Process(A1, A2);
            return Result;
        }

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
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            if (A3 == 0)
            {
                ((TGlobalEnchantmentSpellTE)A1).FEnchantment.Cast(A2.FPlayer);
            }

            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:494
        public override void AIPrefetchCastSpellActions(TAbstractUnit A1, TAICastSpellActionList A2)
        {
            TAICastSpellAction Action = default;
            var call__3 = AICanCastSpell(A1);
            bool bool__4 = call__3;
            if (bool__4)
            {
                var call__5 = AIUpkeepPriority();
                var call__6 = AIRequestCastSpellExpense(A1, FBaseManaCost, FManaUpkeep, call__5);
                bool__4 = call__6;
            }

            if (bool__4)
            {
                Action = CreateCastSpellAction();
                SetupCastSpellAction(A1, Action);
                Action.FPriority = FAIBasePriority;
                Action.SetOwner(A2);
            }
        }

        // GlobalEnchantmentSpells.pas:507
        public override bool AIExecuteCastSpellAction(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            if (AICanCastSpell(A1))
            {
                var call__7 = AIUpkeepPriority();
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, call__7);
                var hero = (THero)A1;
                var call__9 = hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                if (call__9)
                {
                    Result = this.Activate(A1);
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
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            TGlobalEnchantment Enchantment = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result)
            {
                if (!AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(TGlobalEnchantmentType.getSpellWard))
                {
                    Enchantment = CreateEnchantment();
                    if (Enchantment != null)
                    {
                        SetupEnchantment(A1, Enchantment);
                        var call__10 = Enchantment.CanCast(A1.FPlayer, ref A2);
                        Result = call__10;
                        Enchantment.Release();
                    }
                }
                else
                {
                    Result = false;
                    var call__11 = CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr;
                    var call__12 = TranslateRStr(call__11);
                    A2 = call__12;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:458
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            TGlobalEnchantmentSpellTE Event = default;
            AnsiString Reason = default;
            var call__13 = CanActivate(A1, ref Reason);
            bool bool__14 = call__13;
            if (bool__14)
            {
                var call__15 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__16 = call__15.GetBusy();
                bool__14 = !call__16;
            }

            if (bool__14)
            {
                Event = new TGlobalEnchantmentSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__17 = CreateEnchantment();
                Event.FEnchantment = call__17;
                SetupEnchantment(A1, Event.FEnchantment);
                var call__18 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__18.AddEvent(Event);
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
            bool Result = default;
            AnsiString Reason = default;
            bool bool__19 = FAIBasePriority != -1;
            if (bool__19)
            {
                var call__20 = ((THero)Source).CanCastSpell(FSpellID, ref Reason);
                bool__19 = call__20;
            }

            Result = bool__19;
            Result = Result && CanActivate(Source, ref Reason);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:419
        public virtual TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = null;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:424
        public virtual void SetupEnchantment(TAbstractUnit Source, TGlobalEnchantment Enchantment)
        {
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
        }

        // GlobalEnchantmentSpells.pas:484
        public int AIUpkeepPriority()
        {
            int Result = default;
            TGlobalEnchantment Enchantment = default;
            // { Original creates the base enchantment here. }
            Enchantment = new TGlobalEnchantment(null);
            Result = Enchantment.GetAIUpkeepPriority();
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
            var call__21 = EnchantedRoadsRStr;
            var call__22 = TranslateRStr(call__21);
            FName = call__22;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:540
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 900;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:563
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            base.Deactivate(Player);
            var call__23 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__24 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__24.FMagicControl.SetActiveEnchantments(call__23.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEnchantedRoads));
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:545
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            bool bool__25 = Result;
            if (bool__25)
            {
                var call__26 = AoWHSMap.FPlayers.GetPlayers(Player);
                bool__25 = call__26.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petEnchantedRoads);
            }

            if (bool__25)
            {
                var call__27 = XIsAlreadyActiveRStr;
                var call__28 = TranslateRStr(call__27);
                var call__29 = GetName();
                var call__30 = Rtl.Rtl.Format(call__28, [call__29]);
                Reason = call__30;
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:555
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            var call__31 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__32 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__32.FMagicControl.SetActiveEnchantments(call__31.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEnchantedRoads));
            Result = true;
            return Result;
        }

    }

    public partial class TEnchantedRoads : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:571
        public TEnchantedRoads(TECustomNode owner) : base(owner)
        {
            var call__33 = EnchantedRoadsRStr;
            var call__34 = TranslateRStr(call__33);
            FSpellName = call__34;
            FSpellID = 44;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msEarth;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:613
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TAnimation Anim = default;
            TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            Anim = new TAnimation(null);
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap;
            Anim.Activate();
            Twinkle = new TTwinkleAnimation(null);
            var call__35 = AoWHSSet.FSpellControl.GetSpell(44);
            var call__36 = call__35.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__36, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:636
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TEnchantedRoadsEnchantment(null);
            return Result;
        }
    }

    public partial class TAirMasteryEnchantment : TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:649
        public new const int ClassID = 135966;

        // GlobalEnchantmentSpells.pas:641
        public TAirMasteryEnchantment(TECustomNode owner) : base(owner)
        {
            var call__37 = AirMasteryRStr;
            var call__38 = TranslateRStr(call__37);
            FName = call__38;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:654
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:679
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            base.Deactivate(Player);
            var call__39 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__40 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__40.FMagicControl.SetActiveEnchantments(call__39.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petAirMastery));
            // { Original uses Earth. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msEarth);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:659
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                var call__41 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__41.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petAirMastery))
                {
                    var call__42 = XIsAlreadyActiveRStr;
                    var call__43 = TranslateRStr(call__42);
                    var call__44 = GetName();
                    var call__45 = Rtl.Rtl.Format(call__43, [call__44]);
                    Reason = call__45;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:670
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            var call__46 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__47 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__47.FMagicControl.SetActiveEnchantments(call__46.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petAirMastery));
            // { Original uses Earth. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msEarth);
            Result = true;
            return Result;
        }

    }

    public partial class TAirMastery : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:688
        public TAirMastery(TECustomNode owner) : base(owner)
        {
            var call__48 = AirMasteryRStr;
            var call__49 = TranslateRStr(call__48);
            FSpellName = call__49;
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
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TAnimation Anim = default;
            base.ExecuteTE(A1, A2, A3);
            GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msAir);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            Anim = new TAnimation(null);
            Anim.FAnimationLevel = 28;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap;
            Anim.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
            Anim.Activate();
            var call__50 = AoWHSSet.FSpellControl.GetSpell(60);
            call__50.FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:747
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TAirMasteryEnchantment(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:701
        public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int I = default;
            int Count = default;
            int X = default;
            int Y = default;
            int Speed = default;
            int Image = default;
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

                {
                    I = 0;
                    int end__51 = Count - 1;
                    for (; I <= end__51; I += 1)
                    {
                        var call__52 = Random(Anim.FTag & 65535);
                        X = call__52;
                        var call__53 = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
                        Y = call__53;
                        Speed = Random(15) + 5;
                        Image = Random(3);
                        X += (Anim.FFrameCounter - 45) * Speed * 2;
                        Y -= (Anim.FFrameCounter - 45) * Speed;
                        FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
                    }
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
            var call__54 = WaterMasteryRStr;
            var call__55 = TranslateRStr(call__54);
            FName = call__55;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:765
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:791
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            base.Deactivate(Player);
            var call__56 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__57 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__57.FMagicControl.SetActiveEnchantments(call__56.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petWaterMastery));
            AoWHSMap.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msFire);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:770
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                var call__58 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__58.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petWaterMastery))
                {
                    var call__59 = XIsAlreadyActiveRStr;
                    var call__60 = TranslateRStr(call__59);
                    var call__61 = GetName();
                    var call__62 = Rtl.Rtl.Format(call__60, [call__61]);
                    Reason = call__62;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:781
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            var call__63 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__64 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__64.FMagicControl.SetActiveEnchantments(call__63.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petWaterMastery));
            AoWHSMap.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msFire);
            Result = true;
            return Result;
        }

    }

    public partial class TWaterMastery : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:806
        public TWaterMastery(TECustomNode owner) : base(owner)
        {
            var call__65 = WaterMasteryRStr;
            var call__66 = TranslateRStr(call__65);
            FSpellName = call__66;
            FSpellID = 45;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msWater;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:819
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            // { Original ignores the event step. }
            base.ExecuteTE(A1, A2, 0);
            A2.GetLocation(ref X, ref Y, ref L);
            GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msWater);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            FCastSounds.Play(0);
            AoWHSMap.AddEffect(TworldFXType.fxWaterMastery);
            CastingDone(A2, FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:801
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TWaterMasteryEnchantment(null);
            return Result;
        }
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

        // GlobalEnchantmentSpells.pas:833
        public THatredEnchantment(TECustomNode owner) : base(owner)
        {
            var call__67 = HatredRStr;
            var call__68 = TranslateRStr(call__67);
            FName = call__68;
            FEnchantmentType = TGlobalEnchantmentType.getHatred;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:863
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Value == FPlayerIndex)
            {
                NewDay();
            }
        }

        // GlobalEnchantmentSpells.pas:846
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 500;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:876
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            base.Deactivate(Player);
            if (FDaysActive > 10)
            {
                AoWHSMap.FRaces.FRelationOffset += 40;
            }
            else
            {
                AoWHSMap.FRaces.FRelationOffset += FDaysActive * 4;
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:869
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            NewDay();
            Result = true;
            return Result;
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
            var call__69 = HatredRStr;
            var call__70 = TranslateRStr(call__69);
            FSpellName = call__70;
            FSpellID = 46;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 20;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msDeath;
        }

        // GlobalEnchantmentSpells.pas:900
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            AoWHSMap.AddEffect(TworldFXType.fxHatred);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:884
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new THatredEnchantment(null);
            return Result;
        }
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

        // GlobalEnchantmentSpells.pas:913
        public THarmonyEnchantment(TECustomNode owner) : base(owner)
        {
            var call__71 = HarmonyRStr;
            var call__72 = TranslateRStr(call__71);
            FName = call__72;
            FEnchantmentType = TGlobalEnchantmentType.getHarmony;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:943
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Value == FPlayerIndex)
            {
                NewDay();
            }
        }

        // GlobalEnchantmentSpells.pas:926
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 500;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:956
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            base.Deactivate(Player);
            if (FDaysActive > 10)
            {
                AoWHSMap.FRaces.FRelationOffset -= 40;
            }
            else
            {
                AoWHSMap.FRaces.FRelationOffset -= FDaysActive * 4;
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:949
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            NewDay();
            Result = true;
            return Result;
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
            var call__73 = HarmonyRStr;
            var call__74 = TranslateRStr(call__73);
            FSpellName = call__74;
            FSpellID = 47;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 15;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msLife;
        }

        // GlobalEnchantmentSpells.pas:980
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            AoWHSMap.AddEffect(TworldFXType.fxTranquility);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:964
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new THarmonyEnchantment(null);
            return Result;
        }
    }

    public partial class TPowerLeakEnchantment : TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1001
        public new const int ClassID = 135959;

        // GlobalEnchantmentSpells.pas:993
        public TPowerLeakEnchantment(TECustomNode owner) : base(owner)
        {
            var call__75 = PowerLeakRStr;
            var call__76 = TranslateRStr(call__75);
            FName = call__76;
            FEnchantmentType = TGlobalEnchantmentType.getPowerLeak;
            FDispelMana = 25;
        }

        // GlobalEnchantmentSpells.pas:1006
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 750;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1021
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            int I = default;
            base.Deactivate(Player);
            {
                I = AoWHSMap.FPlayers.Count - 1;
                byte end__77 = 0;
                for (; I >= end__77; I -= 1)
                {
                    var call__78 = AoWHSMap.FPlayers.GetPlayers(I);
                    call__78.FMagicControl.Update();
                }
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1011
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            int I = default;
            base.Activate(Player);
            {
                I = AoWHSMap.FPlayers.Count - 1;
                byte end__79 = 0;
                for (; I >= end__79; I -= 1)
                {
                    var call__80 = AoWHSMap.FPlayers.GetPlayers(I);
                    call__80.FMagicControl.Update();
                }
            }

            Result = true;
            return Result;
        }

    }

    public partial class TPowerLeak : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1036
        public TPowerLeak(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
            var call__81 = PowerLeakRStr;
            var call__82 = TranslateRStr(call__81);
            FSpellName = call__82;
            FSpellID = 48;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msCosmos;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:1049
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TTwinkleAnimation Anim = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            Anim = new TTwinkleAnimation(null);
            var call__83 = AoWHSSet.FSpellControl.GetSpell(48);
            var call__84 = call__83.FCastImages.ImageSequences[0];
            Anim.Setup(call__84, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmImplode), 100, 64, 50, 30);
            AoWHSMap.PlaceHX(Anim, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1031
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TPowerLeakEnchantment(null);
            return Result;
        }
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
            int X = default;
            int Y = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            TMapField MapF = default;
            int Index = default;
            THexagonSprite RoadHS = default;
            sbyte TerrainType = default;
            AoWHSMap.LockMapChangedEvent();
            var call__85 = AoWHSMap.MapContainer.GetMapLevel(0);
            MapLevel = call__85;
            // { Original includes the last row when walking neighbouring hexagons. }
            {
                Y = 1;
                int end__86 = MapLevel.Height - 1;
                for (; Y <= end__86; Y += 1)
                {
                    {
                        X = 1;
                        int end__87 = MapLevel.Width - 2;
                        for (; X <= end__87; X += 1)
                        {
                            var call__88 = MapLevel.GetField(X, Y);
                            if (call__88.TerrainType == 0)
                            {
                                InitHNWalk(ref Walk, X, Y, 1);
                                while (Walk.Radius == 1)
                                {
                                    var call__89 = MapLevel.GetField(Walk.X, Walk.Y);
                                    MapF = call__89;
                                    if ((!TSet<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || (MapF.TerrainType == 0 && MapF.OverlayType == 5))
                                    {
                                        Index = (FWidth * Walk.Y + Walk.X) * 2;
                                        bool bool__90 = FOriginalTerrain[Index] == 0 && FOriginalTerrain[Index + 1] == 0;
                                        if (bool__90)
                                        {
                                            var call__91 = MapF.FindNoneTransparentHS(typeof(TStructure));
                                            bool__90 = call__91 == null;
                                        }

                                        if (bool__90)
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
                }
            }

            {
                Y = 1;
                int end__92 = MapLevel.Height - 1;
                for (; Y <= end__92; Y += 1)
                {
                    Index = (FWidth * Y + 1) * 2;
                    {
                        X = 1;
                        int end__93 = MapLevel.Width - 2;
                        for (; X <= end__93; X += 1)
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
                }
            }

            AoWHSMap.UnlockMapChangedEvent();
            AoWHSMap.InvalidateMap();
        }

        // GlobalEnchantmentSpells.pas:1160
        public void Restore()
        {
            TEObjectList Resources = default;
            TList Candidates = default;
            int X = default;
            int Y = default;
            int I = default;
            int Index = default;
            TMapField MapF = default;
            TMapLevel MapLevel = default;
            sbyte Overlay = default;
            TMapObjectResource Resource = default;
            AoWHSMap.LockMapChangedEvent();
            var call__94 = AoWHSMap.MapContainer.GetMapLevel(0);
            MapLevel = call__94;
            {
                Y = 1;
                int end__95 = MapLevel.Height - 1;
                for (; Y <= end__95; Y += 1)
                {
                    Index = (FWidth * Y + 1) * 2;
                    {
                        X = 1;
                        int end__96 = MapLevel.Width - 2;
                        for (; X <= end__96; X += 1)
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
                }
            }

            Resources = AoWHSSet.MapObjectResourceList.FChildren;
            Candidates = new TList();
            {
                Y = 1;
                int end__97 = MapLevel.Height - 1;
                for (; Y <= end__97; Y += 1)
                {
                    Index = (FWidth * Y + 1) * 2;
                    {
                        X = 1;
                        int end__98 = MapLevel.Width - 2;
                        for (; X <= end__98; X += 1)
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
                                    {
                                        I = 0;
                                        int end__99 = Resources.Count - 1;
                                        for (; I <= end__99; I += 1)
                                        {
                                            Resource = (TMapObjectResource)Resources.List[I];
                                            var call__100 = Resource.EClass.ClassID;
                                            bool bool__101 = call__100 == 131431;
                                            if (bool__101)
                                            {
                                                var call__102 = Resource.TerrainList.UsedTerrain();
                                                bool__101 = call__102 == 1;
                                            }

                                            bool bool__103 = bool__101;
                                            if (bool__103)
                                            {
                                                var call__104 = Resource.TerrainList.Terrain[0];
                                                bool__103 = call__104 == MapF.TerrainType;
                                            }

                                            bool bool__105 = bool__103;
                                            if (bool__105)
                                            {
                                                var call__106 = Resource.TerrainList.Overlay[0];
                                                bool__105 = Overlay == call__106;
                                            }

                                            if (bool__105)
                                            {
                                                Candidates.Add(Resource);
                                            }
                                        }
                                    }

                                    if (Candidates.Count != 0)
                                    {
                                        var call__107 = AoWHSMap.Random(Candidates.Count);
                                        var call__108 = Candidates[call__107];
                                        ((TMapObjectResource)call__108).PlaceResourceUser(AoWHSMap, (sbyte)X, (sbyte)Y, 0);
                                    }
                                }
                            }

                            Index += 2;
                        }
                    }
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

        // GlobalEnchantmentSpells.pas:1224
        public TFloodEnchantment(TECustomNode owner) : base(owner)
        {
            var call__109 = FloodRStr;
            var call__110 = TranslateRStr(call__109);
            FName = call__110;
            FEnchantmentType = TGlobalEnchantmentType.getFlood;
            var call__111 = new TFloodControl(null);
            FFloodControl = call__111;
            FDispelMana = 100;
            FRestoreDays = -1;
        }

        // GlobalEnchantmentSpells.pas:1302
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if (Msg.Value == FPlayerIndex && !FRemoved)
                {
                    NewTurn();
                }

                if (Msg.Value == FDispelPlayer && FRemoved)
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
        public override AnsiString GetName()
        {
            AnsiString Result = default;
            Result = base.GetName();
            if (FRemoved)
            {
                var call__112 = FmtDaysLeftStr(FRestoreDays);
                Result = Result + (AnsiString)" (" + LowerCase(call__112) + AnsiString.FromBytes(41);
            }
            else
            {
                if (DaysBeforeActive() != 0)
                {
                    var call__113 = DaysBeforeActive();
                    var call__114 = XinYdaysRStr;
                    var call__115 = Rtl.Rtl.Format(call__114, [Result, call__113]);
                    Result = call__115;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1276
        public override int GetUpkeep()
        {
            int Result = default;
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
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 500;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1328
        public override bool CanDeactivate(byte Player, ref AnsiString Reason)
        {
            bool Result = default;
            if (!FRemoved)
            {
                Result = base.CanDeactivate(Player, ref Reason);
            }
            else
            {
                var call__116 = XAlreadyRemovedRStr;
                var call__117 = TranslateRStr(call__116);
                var call__118 = Rtl.Rtl.Format(call__117, [FName]);
                Reason = call__118;
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1338
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
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

                var call__119 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
                call__119.FMagicControl.Update();
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
            bool Result = default;
            base.Activate(Player);
            FRemoved = false;
            FFloodControl.Setup();
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1260
        public int DaysBeforeActive()
        {
            int Result = default;
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
                            var call__120 = AoWHSMap.Random(2);
                            if (call__120 == 1)
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
                                var call__121 = AoWHSMap.Random(2);
                                if (call__121 == 1)
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
            var call__122 = FloodRStr;
            var call__123 = TranslateRStr(call__122);
            FSpellName = call__123;
            FSpellID = 50;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msWater;
        }

        // GlobalEnchantmentSpells.pas:1370
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            Twinkle = new TTwinkleAnimation(null);
            var call__124 = AoWHSSet.FSpellControl.GetSpell(50);
            var call__125 = call__124.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__125, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1353
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TFloodEnchantment(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1367
        public void ShowAnimation(int A1, int A2, int A3, int A4)
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
            var call__126 = LifeMasteryRStr;
            var call__127 = TranslateRStr(call__126);
            FName = call__127;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1398
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1428
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            TEMsg Msg = default;
            base.Deactivate(Player);
            // { Original doubles Death. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msDeath);
            var call__128 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__129 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__129.FMagicControl.SetActiveEnchantments(call__128.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petLifeMastery));
            InitMsg(ref Msg, 537002240, this);
            AoWHSMap.SendMsg(ref Msg, this);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1401
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                var call__130 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__130.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petLifeMastery))
                {
                    var call__131 = XIsAlreadyActiveRStr;
                    var call__132 = TranslateRStr(call__131);
                    var call__133 = GetName();
                    var call__134 = Rtl.Rtl.Format(call__132, [call__133]);
                    Reason = call__134;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1412
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(Player);
            // { Original doubles Death. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msDeath);
            var call__135 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__135;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petLifeMastery));

            {
                I = APlayer.FArmies.Count - 1;
                byte end__137 = 0;
                for (; I >= end__137; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__138 = 0;
                        for (; J >= end__138; J -= 1)
                        {
                            var call__139 = APlayer.FArmies.GetArmy(I);
                            var call__140 = call__139.GetUnit(J);
                            call__140.SetLifeMasteryBlessing();
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

    }

    public partial class TLifeMastery : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1441
        public TLifeMastery(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
            var call__141 = LifeMasteryRStr;
            var call__142 = TranslateRStr(call__141);
            FSpellName = call__142;
            FSpellID = 61;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Life Mastery. }
            FMagicSphere = TMagicSphere.msCosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1457
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msLife);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            Twinkle = new TTwinkleAnimation(null);
            var call__143 = AoWHSSet.FSpellControl.GetSpell(61);
            var call__144 = call__143.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__144, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
            AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1454
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TLifeMasteryEnchantment(null);
            return Result;
        }
    }

    public partial class TDeathMasteryEnchantment : TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1483
        public new const int ClassID = 135968;

        // GlobalEnchantmentSpells.pas:1475
        public TDeathMasteryEnchantment(TECustomNode owner) : base(owner)
        {
            var call__145 = DeathMasteryRStr;
            var call__146 = TranslateRStr(call__145);
            FName = call__146;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1568
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                if (Msg.Value != 0)
                {
                    var call__147 = AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__147.FMagicControl.FEnemyEnchantments.Contains(TPlayerEnchantmentType.petDeathMastery))
                    {
                        NewTurn((sbyte)Msg.Value);
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1486
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1509
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            TEMsg Msg = default;
            base.Deactivate(Player);
            // { Original doubles Life. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msLife);
            var call__148 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__149 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__149.FMagicControl.SetActiveEnchantments(call__148.FMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petDeathMastery));
            InitMsg(ref Msg, 537002241, this);
            AoWHSMap.SendMsg(ref Msg, this);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1489
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                var call__150 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__150.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petDeathMastery))
                {
                    var call__151 = XIsAlreadyActiveRStr;
                    var call__152 = TranslateRStr(call__151);
                    var call__153 = GetName();
                    var call__154 = Rtl.Rtl.Format(call__152, [call__153]);
                    Reason = call__154;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1500
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(Player);
            // { Original doubles Life. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msLife);
            var call__155 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__156 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__156.FMagicControl.SetActiveEnchantments(call__155.FMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petDeathMastery));
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1522
        public void NewTurn(sbyte Player)
        {
            int X = default;
            int Y = default;
            int L = default;
            int Index = default;
            byte[] Flags = [];
            int Mask = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            bool Border = default;
            Flags = [];
            Mask = 1 << (Player - 1);
            {
                L = 0;
                int end__157 = AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__157; L += 1)
                {
                    var call__158 = AoWHSMap.MapContainer.GetMapLevel(L);
                    MapLevel = call__158;
                    Index = 0;
                    Array.Resize(ref Flags, MapLevel.Width * MapLevel.Height);
                    Array.Clear(Flags);
                    {
                        Y = 1;
                        int end__159 = MapLevel.Height - 1;
                        for (; Y <= end__159; Y += 1)
                        {
                            {
                                X = 1;
                                int end__160 = MapLevel.Width - 2;
                                for (; X <= end__160; X += 1)
                                {
                                    var call__161 = MapLevel.GetField(X, Y);
                                    if ((((TAoWMapField)call__161).FFieldFlags & Mask) != 0)
                                    {
                                        Border = false;
                                        InitHNWalk(ref Walk, X, Y, 1);
                                        while (Walk.Radius == 1 && !Border)
                                        {
                                            if (Walk.X >= 0 && Walk.X < MapLevel.Width && Walk.Y >= 0 && Walk.Y < MapLevel.Height)
                                            {
                                                var call__162 = MapLevel.GetField(Walk.X, Walk.Y);
                                                Border = (((TAoWMapField)call__162).FFieldFlags & Mask) == 0;
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
                        }
                    }

                    Index = 0;
                    {
                        Y = 1;
                        int end__163 = MapLevel.Height - 1;
                        for (; Y <= end__163; Y += 1)
                        {
                            {
                                X = 1;
                                int end__164 = MapLevel.Width - 2;
                                for (; X <= end__164; X += 1)
                                {
                                    if (Flags[Index] != 0)
                                    {
                                        var call__165 = MapLevel.GetField(X, Y);
                                        ((TAoWMapField)call__165).SetExplored(Player, false);
                                    }

                                    Index += 1;
                                }
                            }
                        }
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
            var call__166 = DeathMasteryRStr;
            var call__167 = TranslateRStr(call__166);
            FSpellName = call__167;
            FSpellID = 62;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Death Mastery. }
            FMagicSphere = TMagicSphere.msCosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1618
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msDeath);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            var animation = new TAnimation(null);
            animation.FAnimationLevel = 28;
            animation.FOnShow = ShowAnimation;
            animation.FAnimationMap = AoWHSMap;
            animation.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
            animation.Activate();

            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1590
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TDeathMasteryEnchantment(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1593
        public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int I = default;
            int Count = default;
            int X = default;
            int Y = default;
            int Speed = default;
            int D = default;
            int Image = default;
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

            {
                I = 0;
                int end__169 = Count - 1;
                for (; I <= end__169; I += 1)
                {
                    var call__170 = Random(Anim.FTag & 65535);
                    X = call__170;
                    var call__171 = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
                    Y = call__171;
                    Speed = Random(15) + 5;
                    Image = Random(3);
                    // { Preserve the original image/displacement register lifetimes. }
                    ;
                    D = (Anim.FFrameCounter - 45) * Speed;
                    X -= D * 2;
                    Y += D;
                    FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
                }
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
            var call__172 = FireMasteryRStr;
            var call__173 = TranslateRStr(call__172);
            FName = call__173;
            FEnchantmentType = TGlobalEnchantmentType.getFireMastery;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1724
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if (Msg.Value != 0)
                {
                    var call__174 = AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__174.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petFireMastery))
                    {
                        UpdateFire(2, !AoWHSMap.FRequestCombatType);
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1650
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1680
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Deactivate(Player);
            var call__175 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__175;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petFireMastery));

            {
                I = APlayer.FArmies.Count - 1;
                byte end__177 = 0;
                for (; I >= end__177; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__178 = 0;
                        for (; J >= end__178; J -= 1)
                        {
                            var call__179 = APlayer.FArmies.GetArmy(I);
                            var call__180 = call__179.GetUnit(J);
                            call__180.ResetFireMasteryProtection();
                        }
                    }
                }
            }

            // { Original doubles Water. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msWater);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1653
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            var call__181 = AoWHSMap.FPlayers.GetPlayers(Player);
            if (call__181.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petFireMastery))
            {
                var call__182 = XIsAlreadyActiveRStr;
                var call__183 = TranslateRStr(call__182);
                var call__184 = GetName();
                var call__185 = Rtl.Rtl.Format(call__183, [call__184]);
                Reason = call__185;
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
            bool Result = default;
            TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(Player);
            // { Original doubles Water. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msWater);
            var call__186 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__186;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petFireMastery));

            {
                I = APlayer.FArmies.Count - 1;
                byte end__188 = 0;
                for (; I >= end__188; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__189 = 0;
                        for (; J >= end__189; J -= 1)
                        {
                            var call__190 = APlayer.FArmies.GetArmy(I);
                            var call__191 = call__190.GetUnit(J);
                            call__191.SetFireMasteryProtection();
                        }
                    }
                }
            }

            UpdateFire(15, true);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1696
        public void UpdateFire(int Factor, bool Explosions)
        {
            int X = default;
            int Y = default;
            int L = default;
            int Chance = default;
            TMapLevel MapLevel = default;
            TFire Fire = default;
            var call__192 = AoWHSMap.Random(16777215);
            RandSeed = call__192;
            {
                L = 0;
                int end__193 = AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__193; L += 1)
                {
                    var call__194 = AoWHSMap.MapContainer.GetMapLevel(L);
                    MapLevel = call__194;
                    {
                        Y = 1;
                        int end__195 = MapLevel.Height - 1;
                        for (; Y <= end__195; Y += 1)
                        {
                            {
                                X = 1;
                                int end__196 = MapLevel.Width - 2;
                                for (; X <= end__196; X += 1)
                                {
                                    var call__197 = MapLevel.GetField(X, Y);
                                    var call__198 = IgnitionChance((TAoWMapField)call__197);
                                    Chance = call__198;
                                    if (Chance > 0)
                                    {
                                        var call__199 = AoWHSMap.Random(10000);
                                        if (call__199 < Chance * Factor)
                                        {
                                            var call__200 = AoWHSSet.FFireControl.PlaceFire(X, Y, L, TFireType.ftLarge);
                                            Fire = call__200;
                                            if (Fire != null)
                                            {
                                                if (Explosions)
                                                {
                                                    var call__201 = Random(30);
                                                    Fire.SetupExplosion(call__201);
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
            }
        }

    }

    public partial class TFireMastery : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1733
        public TFireMastery(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
            var call__202 = FireMasteryRStr;
            var call__203 = TranslateRStr(call__202);
            FSpellName = call__203;
            FSpellID = 63;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msFire;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1792
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref L);
                GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msFire);
                AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
                var animation = new TAnimation(null);
                animation.FAnimationLevel = 27;
                animation.FOnShow = ShowAnimation;
                animation.FAnimationMap = AoWHSMap;
                animation.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
                animation.Activate();

                FCastSounds.Play(0);
                CastingDone(A2, A1.FEventMana);
            }

            if (A3 == 20)
            {
                base.ExecuteTE(A1, A2, 0);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1746
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TFireMasteryEnchantment(null);
            return Result;
        }
    }

    public partial class TEarthMasteryEnchantment : TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1828
        public new const int ClassID = 135970;

        // GlobalEnchantmentSpells.pas:1820
        public TEarthMasteryEnchantment(TECustomNode owner) : base(owner)
        {
            var call__205 = EarthMasteryRStr;
            var call__206 = TranslateRStr(call__205);
            FName = call__206;
            FEnchantmentType = TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1831
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 1250;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1861
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Deactivate(Player);
            var call__207 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__207;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEarthMastery));

            {
                I = APlayer.FArmies.Count - 1;
                byte end__209 = 0;
                for (; I >= end__209; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__210 = 0;
                        for (; J >= end__210; J -= 1)
                        {
                            var call__211 = APlayer.FArmies.GetArmy(I);
                            var call__212 = call__211.GetUnit(J);
                            call__212.ResetEarthMasteryStoneSkin();
                        }
                    }
                }
            }

            // { Original doubles Air. }
            AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(TMagicSphere.msAir);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1834
        public override bool CanActivate(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                var call__213 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__213.FMagicControl.FActiveEnchantments.Contains(TPlayerEnchantmentType.petEarthMastery))
                {
                    var call__214 = XIsAlreadyActiveRStr;
                    var call__215 = TranslateRStr(call__214);
                    var call__216 = Rtl.Rtl.Format(call__215, [FName]);
                    Reason = call__216;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1845
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(Player);
            // { Original doubles Air. }
            AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(TMagicSphere.msAir);
            var call__217 = AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__217;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<TPlayerEnchantmentType>.Of(TPlayerEnchantmentType.petEarthMastery));

            {
                I = APlayer.FArmies.Count - 1;
                byte end__219 = 0;
                for (; I >= end__219; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__220 = 0;
                        for (; J >= end__220; J -= 1)
                        {
                            var call__221 = APlayer.FArmies.GetArmy(I);
                            var call__222 = call__221.GetUnit(J);
                            call__222.SetEarthMasteryStoneSkin();
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

    }

    public partial class TEarthMastery : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1877
        public TEarthMastery(TECustomNode owner) : base(owner)
        {
            var call__223 = EarthMasteryRStr;
            var call__224 = TranslateRStr(call__223);
            FSpellName = call__224;
            FSpellID = 64;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msEarth;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1918
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            GlobalEnchantmentSpells.NotifySpellSphere(A2.FPlayer, TMagicSphere.msEarth);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            AoWHSMap.AddEffect(TworldFXType.fxEarthMastery);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1932
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TEarthMasteryEnchantment(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1890
        public void ShowAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int MaxX = default;
            int Y = default;
            int MaxY = default;
            int Xhp = default;
            int Yhp = default;
            int FieldStep = default;
            TMapLevel MapLevel = default;
            TAoWMapField MapF = default;
            var call__225 = AoWHSMap.MapContainer.GetMapLevel(AoWHSMap.ViewL);
            MapLevel = call__225;
            FieldStep = MapLevel.FHNOffsets[0][4];
            MaxX = AoWHSMap.ViewXhx + AoWHSMap.ViewWidthHX - 1;
            MaxY = AoWHSMap.ViewYhx + AoWHSMap.ViewHeightHX - 1;
            {
                X = AoWHSMap.ViewXhx;
                int end__226 = MaxX;
                for (; X <= end__226; X += 1)
                {
                    HXtoHP(X, AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
                    Xhp = Xhp - XhpSurf + 16;
                    Yhp = Yhp - YhpSurf + 16;
                    var call__227 = MapLevel.GetField(X, AoWHSMap.ViewYhx);
                    MapF = (TAoWMapField)call__227;
                    {
                        Y = AoWHSMap.ViewYhx;
                        int end__228 = MaxY;
                        for (; Y <= end__228; Y += 1)
                        {
                            if (MapF.OverlayType == 4)
                            {
                                FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - (((int)(((uint)ObjectToInt(MapF)) >> 8)) & 15), SurfaceDesc);
                            }

                            // { Original uses the field address. }
                            /* TODO: Implement managed object pool traversal. */
                            throw new NotImplementedException();
                            Yhp += 32;
                        }
                    }
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
            var call__229 = SpellWardRStr;
            var call__230 = TranslateRStr(call__229);
            FName = call__230;
            FEnchantmentType = TGlobalEnchantmentType.getSpellWard;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:1946
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 500;
            return Result;
        }

    }

    public partial class TSpellWard : TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1949
        public TSpellWard(TECustomNode owner) : base(owner)
        {
            var call__231 = SpellWardRStr;
            var call__232 = TranslateRStr(call__231);
            FSpellName = call__232;
            FSpellID = 65;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msEarth;
        }

        // GlobalEnchantmentSpells.pas:1960
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
            AoWHSMap.AddEffect(TworldFXType.fxSpellWard);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1973
        public override TGlobalEnchantment CreateEnchantment()
        {
            TGlobalEnchantment Result = default;
            Result = new TSpellWardEnchantment(null);
            return Result;
        }
    }

    public static partial class GlobalEnchantmentSpells
    {
        // GlobalEnchantmentSpells.pas:1976
        public static void RegisterGlobalEnchantmentSpells(TAoWEngine AEngine)
        {
            TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([EClass.Of<TEnchantedRoadsEnchantment>(), EClass.Of<TGlobalEnchantmentSpellTE>(), EClass.Of<TGlobalEnchantmentEventLog>()]);
            AEngine.RegisterEClasses([EClass.Of<TWaterMasteryEnchantment>(), EClass.Of<THatredEnchantment>(), EClass.Of<THarmonyEnchantment>(), EClass.Of<TPowerLeakEnchantment>(), EClass.Of<TFloodEnchantment>(), EClass.Of<TAirMasteryEnchantment>()]);
            AEngine.RegisterEClasses([EClass.Of<TLifeMasteryEnchantment>(), EClass.Of<TDeathMasteryEnchantment>(), EClass.Of<TFireMasteryEnchantment>(), EClass.Of<TEarthMasteryEnchantment>(), EClass.Of<TSpellWardEnchantment>()]);
            var call__233 = new TEnchantedRoads(null);
            Control.RegisterSpell(call__233);
            var call__234 = new THatred(null);
            Control.RegisterSpell(call__234);
            var call__235 = new THarmony(null);
            Control.RegisterSpell(call__235);
            var call__236 = new TPowerLeak(null);
            Control.RegisterSpell(call__236);
            var call__237 = new TFlood(null);
            Control.RegisterSpell(call__237);
            var call__238 = new TAirMastery(null);
            Control.RegisterSpell(call__238);
            var call__239 = new TLifeMastery(null);
            Control.RegisterSpell(call__239);
            var call__240 = new TDeathMastery(null);
            Control.RegisterSpell(call__240);
            var call__241 = new TFireMastery(null);
            Control.RegisterSpell(call__241);
            var call__242 = new TEarthMastery(null);
            Control.RegisterSpell(call__242);
            var call__243 = new TWaterMastery(null);
            Control.RegisterSpell(call__243);
            var call__244 = new TSpellWard(null);
            Control.RegisterSpell(call__244);
        }

        // GlobalEnchantmentSpells.pas:1998
        public static void UnregisterGlobalEnchantmentSpells(TAoWEngine AEngine)
        {
            TSpellControl Control = default;
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
}
