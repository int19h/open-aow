// Generated from GlobalEnchantmentSpells.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.GlobalEnchantmentSpells
{
    // { TGlobalEnchantmentSpellTE (instance size 36, VMT 557ED91C): $20 FEnchantment. }
    public partial class TGlobalEnchantmentSpellTE : Aow.Units.AoWE.TSpellTE
    {
        public Aow.Units.AoWE.TGlobalEnchantment FEnchantment;
        // GlobalEnchantmentSpells.pas:379
        // { $20 }
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
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FEnchantment, null, 25);
        }

        // GlobalEnchantmentSpells.pas:400
        public override bool Validate(Aow.Units.AoWE.THero A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Result = base.Validate(A1);
            bool bool__1 = Result;
            if (bool__1)
            {
                var call__2 = FEnchantment.CanCast(unchecked((sbyte)A1.FPlayer), ref Reason);
                bool__1 = call__2;
            }

            Result = bool__1;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:408
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Result = base.Process(A1, A2);
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE o = new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // GlobalEnchantmentSpells.pas:389
            public override int ClassID()
            {
                int Result = default;
                Result = 135954;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE o = (Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TGlobalEnchantmentSpell (instance size 56, VMT 557EDA18): $34 FAIBasePriority. }
    public partial class TGlobalEnchantmentSpell : Aow.Units.AoWE.TSpell
    {
        public int FAIBasePriority;
        // GlobalEnchantmentSpells.pas:413
        // { $34 }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FAIBasePriority = -1;
        }

        // GlobalEnchantmentSpells.pas:429
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            if (A3 == 0)
            {
                ((Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE)A1).FEnchantment.Cast(unchecked((sbyte)A2.FPlayer));
            }

            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:494
        public override void AIPrefetchCastSpellActions(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TAICastSpellActionList A2)
        {
            Aow.Units.AoWE.TAICastSpellAction Action = default;
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
        public override bool AIExecuteCastSpellAction(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            if (AICanCastSpell(A1))
            {
                var call__7 = AIUpkeepPriority();
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, call__7);
                {
                    var w__8 = (Aow.Units.AoWE.THero)A1;
                    var call__9 = w__8.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                    if (call__9)
                    {
                        Result = this.Activate(A1);
                    }
                    else
                    {
                        if (w__8.FCastingSpellID != FSpellID)
                        {
                            w__8.ExecuteStartCasting(FSpellID);
                            Result = true;
                        }
                        else
                        {
                            Result = true;
                        }
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
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
        {
            bool Result = default;
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result)
            {
                if (!Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TGlobalEnchantmentType.getSpellWard))
                {
                    Enchantment = CreateEnchantment();
                    if (Enchantment != null)
                    {
                        SetupEnchantment(A1, Enchantment);
                        var call__10 = Enchantment.CanCast(unchecked((sbyte)A1.FPlayer), ref A2);
                        Result = call__10;
                        Enchantment.Release();
                    }
                }
                else
                {
                    Result = false;
                    var call__11 = Aow.Units.AoWE.AoWE.CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr;
                    var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(call__11);
                    A2 = call__12;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:458
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE Event = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__13 = CanActivate(A1, ref Reason);
            bool bool__14 = call__13;
            if (bool__14)
            {
                var call__15 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__16 = call__15.GetBusy();
                bool__14 = !call__16;
            }

            if (bool__14)
            {
                Event = Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__17 = CreateEnchantment();
                Event.FEnchantment = call__17;
                SetupEnchantment(A1, Event.FEnchantment);
                var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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
        public virtual bool AICanCastSpell(Aow.Units.AoWE.TAbstractUnit Source)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            bool bool__19 = (FAIBasePriority != -1);
            if (bool__19)
            {
                var call__20 = ((Aow.Units.AoWE.THero)Source).CanCastSpell(FSpellID, ref Reason);
                bool__19 = call__20;
            }

            Result = bool__19;
            Result = Result && CanActivate(Source, ref Reason);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:419
        // { +09C }
        public virtual Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = null;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:424
        // { +0A0 }
        public virtual void SetupEnchantment(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TGlobalEnchantment Enchantment)
        {
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
        }

        // GlobalEnchantmentSpells.pas:484
        // { +0A4 }
        public int AIUpkeepPriority()
        {
            int Result = default;
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            // { Original creates the base enchantment here. }
            Enchantment = Aow.Units.AoWE.TGlobalEnchantment.Create(null);
            Result = Enchantment.GetAIUpkeepPriority();
            Enchantment.Release();
            return Result;
        }

        // GlobalEnchantmentSpells.pas:57
        public static new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell o = new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TEnchantedRoadsEnchantment (instance size 36, VMT 557EDB5C) }
    public partial class TEnchantedRoadsEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:527
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__21 = Aow.Units.AoWE.AoWE.EnchantedRoadsRStr;
            var call__22 = Aow.Units.AoWE.AoWE.TranslateRStr(call__21);
            FName = call__22;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
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
            base.Deactivate(unchecked((byte)Player));
            var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__24.FMagicControl.SetActiveEnchantments(call__23.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEnchantedRoads));
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:545
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            bool bool__25 = Result;
            if (bool__25)
            {
                var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                bool__25 = call__26.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petEnchantedRoads);
            }

            if (bool__25)
            {
                var call__27 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(call__27);
                var call__29 = GetName();
                var call__30 = Aow.Rtl.Rtl.Format(call__28, [call__29]);
                Reason = call__30;
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:555
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
            var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__32.FMagicControl.SetActiveEnchantments(call__31.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEnchantedRoads));
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:72
        public static new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:535
            public override int ClassID()
            {
                int Result = default;
                Result = 135955;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:72
            public new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEnchantedRoads (instance size 56, VMT 557EDC88) }
    public partial class TEnchantedRoads : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:571
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__33 = Aow.Units.AoWE.AoWE.EnchantedRoadsRStr;
            var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(call__33);
            FSpellName = call__34;
            FSpellID = 44;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:613
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TAnimation Anim = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Anim = Aow.Units.Animation.TAnimation.Create(null);
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
            Anim.Activate();
            Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
            var call__35 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(44);
            var call__36 = call__35.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__36, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:636
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:584
        public void ShowAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int MaxX = default;
            int Y = default;
            int MaxY = default;
            int Xhp = default;
            int Yhp = default;
            int FieldStep = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWE.AoWE.AoWHSMap.ViewL);
            MapLevel = call__37;
            FieldStep = MapLevel.FHNOffsets[0][4];
            MaxX = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidthHX) - 1;
            MaxY = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeightHX) - 1;
            {
                X = Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx;
                int end__38 = MaxX;
                for (; X <= end__38; X += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
                    Xhp = (Xhp - XhpSurf) + 16;
                    Yhp = (Yhp - YhpSurf) + 16;
                    var call__39 = MapLevel.GetField(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx);
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__39;
                    {
                        Y = Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx;
                        int end__40 = MaxY;
                        for (; Y <= end__40; Y += 1)
                        {
                            bool bool__41 = (MapF.OverlayType == 4) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1);
                            if (bool__41)
                            {
                                var call__42 = MapF.GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                bool__41 = call__42;
                            }

                            if (bool__41)
                            {
                                FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - (((MapF.Xhx + (MapF.Yhx * 8)) + Unsafe.As<ushort, Aow.Rtl.WordRec>(ref Unsafe.As<int, Aow.Rtl.LongRec>(ref MapF.FFieldFlags).Hi).Lo) & 15), SurfaceDesc);
                            }

                            /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                            throw new System.NotImplementedException();
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

        // GlobalEnchantmentSpells.pas:83
        public static new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads o = new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAirMasteryEnchantment (instance size 36, VMT 557EDDBC) }
    public partial class TAirMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:641
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__43 = Aow.Units.AoWE.AoWE.AirMasteryRStr;
            var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(call__43);
            FName = call__44;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
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
            base.Deactivate(unchecked((byte)Player));
            var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__46.FMagicControl.SetActiveEnchantments(call__45.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery));
            // { Original uses Earth. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msEarth);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:659
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            if (Result)
            {
                var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__47.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery))
                {
                    var call__48 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(call__48);
                    var call__50 = GetName();
                    var call__51 = Aow.Rtl.Rtl.Format(call__49, [call__50]);
                    Reason = call__51;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:670
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
            var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__53.FMagicControl.SetActiveEnchantments(call__52.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery));
            // { Original uses Earth. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msEarth);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:92
        public static new Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:649
            public override int ClassID()
            {
                int Result = default;
                Result = 135966;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:92
            public new Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TAirMastery (instance size 56, VMT 557EDEE0) }
    public partial class TAirMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:688
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__54 = Aow.Units.AoWE.AoWE.AirMasteryRStr;
            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(call__54);
            FSpellName = call__55;
            FSpellID = 60;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            // { Original also assigns Earth to Air Mastery. }
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:727
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TAnimation Anim = default;
            base.ExecuteTE(A1, A2, A3);
            Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msAir);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Anim = Aow.Units.Animation.TAnimation.Create(null);
            Anim.FAnimationLevel = 28;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
            Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
            Anim.Activate();
            var call__56 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(60);
            call__56.FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:747
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:701
        public void ShowAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int I = default;
            int Count = default;
            int X = default;
            int Y = default;
            int Speed = default;
            int Image = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == 0)
            {
                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.Frame - Anim.FFrameCounter;
                switch ((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail)
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
                    int end__57 = Count - 1;
                    for (; I <= end__57; I += 1)
                    {
                        var call__58 = Aow.Rtl.Rtl.Random(Anim.FTag & 65535);
                        X = call__58;
                        var call__59 = Aow.Rtl.Rtl.Random((int)(((uint)((int)unchecked((uint)Anim.FTag))) >> 16));
                        Y = call__59;
                        Speed = Aow.Rtl.Rtl.Random(15) + 5;
                        Image = Aow.Rtl.Rtl.Random(3);
                        X += ((Anim.FFrameCounter - 45) * Speed) * 2;
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

        // GlobalEnchantmentSpells.pas:103
        public static new Aow.Units.GlobalEnchantmentSpells.TAirMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TAirMastery o = new Aow.Units.GlobalEnchantmentSpells.TAirMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TWaterMasteryEnchantment (instance size 36, VMT 557EE00C) }
    public partial class TWaterMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:752
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__60 = Aow.Units.AoWE.AoWE.WaterMasteryRStr;
            var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(call__60);
            FName = call__61;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
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
            base.Deactivate(unchecked((byte)Player));
            var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__63 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__63.FMagicControl.SetActiveEnchantments(call__62.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery));
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msFire);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:770
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            if (Result)
            {
                var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__64.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery))
                {
                    var call__65 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(call__65);
                    var call__67 = GetName();
                    var call__68 = Aow.Rtl.Rtl.Format(call__66, [call__67]);
                    Reason = call__68;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:781
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
            var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__70.FMagicControl.SetActiveEnchantments(call__69.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery));
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msFire);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:112
        public static new Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:760
            public override int ClassID()
            {
                int Result = default;
                Result = 135956;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:112
            public new Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWaterMastery (instance size 56, VMT 557EE138) }
    public partial class TWaterMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:806
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__71 = Aow.Units.AoWE.AoWE.WaterMasteryRStr;
            var call__72 = Aow.Units.AoWE.AoWE.TranslateRStr(call__71);
            FSpellName = call__72;
            FSpellID = 45;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:819
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            // { Original ignores the event step. }
            base.ExecuteTE(A1, A2, 0);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msWater);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            FCastSounds.Play(0);
            Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxWaterMastery);
            CastingDone(A2, FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:801
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:123
        public static new Aow.Units.GlobalEnchantmentSpells.TWaterMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TWaterMastery o = new Aow.Units.GlobalEnchantmentSpells.TWaterMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THatredEnchantment (instance size 40, VMT 557EE268): $24 FDaysActive. }
    public partial class THatredEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public int FDaysActive;
        // GlobalEnchantmentSpells.pas:851
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        // GlobalEnchantmentSpells.pas:833
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__73 = Aow.Units.AoWE.AoWE.HatredRStr;
            var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(call__73);
            FName = call__74;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getHatred;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:863
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if ((Msg.MsgID == 537001986) && (Msg.Value == FPlayerIndex))
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
            base.Deactivate(unchecked((byte)Player));
            if (FDaysActive > 10)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset += 40;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset += FDaysActive * 4;
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:869
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
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
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset -= 4;
            }
        }

        // GlobalEnchantmentSpells.pas:133
        public static new Aow.Units.GlobalEnchantmentSpells.THatredEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.THatredEnchantment o = new Aow.Units.GlobalEnchantmentSpells.THatredEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:841
            public override int ClassID()
            {
                int Result = default;
                Result = 135957;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:133
            public new Aow.Units.GlobalEnchantmentSpells.THatredEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.THatredEnchantment o = (Aow.Units.GlobalEnchantmentSpells.THatredEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.THatredEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { THatred (instance size 56, VMT 557EE384) }
    public partial class THatred : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:889
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__75 = Aow.Units.AoWE.AoWE.HatredRStr;
            var call__76 = Aow.Units.AoWE.AoWE.TranslateRStr(call__75);
            FSpellName = call__76;
            FSpellID = 46;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 20;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
        }

        // GlobalEnchantmentSpells.pas:900
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxHatred);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:884
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.THatredEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:145
        public static new Aow.Units.GlobalEnchantmentSpells.THatred Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.THatred o = new Aow.Units.GlobalEnchantmentSpells.THatred();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THarmonyEnchantment (instance size 40, VMT 557EE4A8): $24 FDaysActive. }
    public partial class THarmonyEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public int FDaysActive;
        // GlobalEnchantmentSpells.pas:931
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        // GlobalEnchantmentSpells.pas:913
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__77 = Aow.Units.AoWE.AoWE.HarmonyRStr;
            var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(call__77);
            FName = call__78;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getHarmony;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:943
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if ((Msg.MsgID == 537001986) && (Msg.Value == FPlayerIndex))
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
            base.Deactivate(unchecked((byte)Player));
            if (FDaysActive > 10)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset -= 40;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset -= FDaysActive * 4;
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:949
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
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
                Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.FRelationOffset += 4;
            }
        }

        // GlobalEnchantmentSpells.pas:155
        public static new Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment o = new Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:921
            public override int ClassID()
            {
                int Result = default;
                Result = 135958;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:155
            public new Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment o = (Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { THarmony (instance size 56, VMT 557EE5C8) }
    public partial class THarmony : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:969
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__79 = Aow.Units.AoWE.AoWE.HarmonyRStr;
            var call__80 = Aow.Units.AoWE.AoWE.TranslateRStr(call__79);
            FSpellName = call__80;
            FSpellID = 47;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 15;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
        }

        // GlobalEnchantmentSpells.pas:980
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxTranquility);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:964
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:167
        public static new Aow.Units.GlobalEnchantmentSpells.THarmony Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.THarmony o = new Aow.Units.GlobalEnchantmentSpells.THarmony();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TPowerLeakEnchantment (instance size 36, VMT 557EE6F0) }
    public partial class TPowerLeakEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:993
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__81 = Aow.Units.AoWE.AoWE.PowerLeakRStr;
            var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(call__81);
            FName = call__82;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getPowerLeak;
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
            base.Deactivate(unchecked((byte)Player));
            {
                I = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                byte end__83 = 0;
                for (; I >= end__83; I -= 1)
                {
                    var call__84 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__84.FMagicControl.Update();
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
            base.Activate(unchecked((sbyte)Player));
            {
                I = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                byte end__85 = 0;
                for (; I >= end__85; I -= 1)
                {
                    var call__86 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__86.FMagicControl.Update();
                }
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:175
        public static new Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1001
            public override int ClassID()
            {
                int Result = default;
                Result = 135959;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:175
            public new Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPowerLeak (instance size 56, VMT 557EE814) }
    public partial class TPowerLeak : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1036
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__87 = Aow.Units.AoWE.AoWE.PowerLeakRStr;
            var call__88 = Aow.Units.AoWE.AoWE.TranslateRStr(call__87);
            FSpellName = call__88;
            FSpellID = 48;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:1049
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TTwinkleAnimation Anim = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Anim = Aow.Units.Animation.TTwinkleAnimation.Create(null);
            var call__89 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(48);
            var call__90 = call__89.FCastImages.ImageSequences[0];
            Anim.Setup(call__90, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmImplode), 100, 64, 50, 30);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1031
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:185
        public static new Aow.Units.GlobalEnchantmentSpells.TPowerLeak Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TPowerLeak o = new Aow.Units.GlobalEnchantmentSpells.TPowerLeak();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFloodControl (instance size 20, VMT 557EE93C): $08 FWidth, $0C FHeight, $10 FOriginalTerrain. }
    public partial class TFloodControl : Aow.Units.Engine.TEObject
    {
        public int FWidth;
        // { $08 }
        public int FHeight;
        // { $0C }
        public Ptr<sbyte> FOriginalTerrain;
        // GlobalEnchantmentSpells.pas:1072
        // { $10 }
        public override void Destroy()
        {
            if (!FOriginalTerrain.IsNull)
            {
                FOriginalTerrain = FOriginalTerrain.Realloc(0);
            }

            base.Destroy();
        }

        // GlobalEnchantmentSpells.pas:1078
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FWidth, 20);
            Stream.rwInteger(ref FHeight, 21);
            if (FWidth != 0)
            {
                if (Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smRead))
                {
                    FOriginalTerrain = FOriginalTerrain.Realloc((FWidth * FHeight) * 2);
                }

                Stream.rwData(MemoryMarshal.AsBytes(FOriginalTerrain.Array.AsSpan(FOriginalTerrain.Index, (FWidth * FHeight) * 2)), (FWidth * FHeight) * 2, 22);
            }
        }

        // GlobalEnchantmentSpells.pas:1066
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FOriginalTerrain = default;
        }

        // GlobalEnchantmentSpells.pas:1090
        public void Setup()
        {
            FWidth = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Width;
            FHeight = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Height;
            FOriginalTerrain = FOriginalTerrain.Realloc((FWidth * FHeight) * 2);
            Aow.Rtl.Rtl.FillBytes(MemoryMarshal.AsBytes(FOriginalTerrain.Array.AsSpan(FOriginalTerrain.Index, (FWidth * FHeight) * 2)), 0);
        }

        // GlobalEnchantmentSpells.pas:1098
        public void Flood()
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            int Index = default;
            Aow.Units.HSEngine.THexagonSprite RoadHS = default;
            sbyte TerrainType = default;
            Aow.Units.AoWE.AoWE.AoWHSMap.LockMapChangedEvent();
            var call__91 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(0);
            MapLevel = call__91;
            // { Original includes the last row when walking neighbouring hexagons. }
            {
                Y = 1;
                int end__92 = MapLevel.Height - 1;
                for (; Y <= end__92; Y += 1)
                {
                    {
                        X = 1;
                        int end__93 = MapLevel.Width - 2;
                        for (; X <= end__93; X += 1)
                        {
                            var call__94 = MapLevel.GetField(X, Y);
                            if (call__94.TerrainType == 0)
                            {
                                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 1);
                                while (Walk.Radius == 1)
                                {
                                    var call__95 = MapLevel.GetField(Walk.X, Walk.Y);
                                    MapF = call__95;
                                    if ((!TSet<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || ((MapF.TerrainType == 0) && (MapF.OverlayType == 5)))
                                    {
                                        Index = ((FWidth * Walk.Y) + Walk.X) * 2;
                                        bool bool__96 = (FOriginalTerrain[Index] == 0) && (FOriginalTerrain[Index + 1] == 0);
                                        if (bool__96)
                                        {
                                            var call__97 = MapF.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                                            bool__96 = (call__97 == null);
                                        }

                                        if (bool__96)
                                        {
                                            FOriginalTerrain[Index] = unchecked((sbyte)MapF.TerrainType);
                                            FOriginalTerrain[Index + 1] = unchecked((sbyte)MapF.OverlayType);
                                        }

                                        if (TSet<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)MapF.OverlayType))
                                        {
                                            RoadHS = MapF.FindOwnedHS(typeof(Aow.Units.HSEngine.TAbstractRoad));
                                            if (RoadHS != null)
                                            {
                                                RoadHS.Destroy();
                                            }
                                        }
                                    }

                                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                                }
                            }
                        }
                    }
                }
            }

            {
                Y = 1;
                int end__98 = MapLevel.Height - 1;
                for (; Y <= end__98; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
                    {
                        X = 1;
                        int end__99 = MapLevel.Width - 2;
                        for (; X <= end__99; X += 1)
                        {
                            if (FOriginalTerrain[Index] != 0)
                            {
                                MapF = MapLevel.GetField(X, Y);
                                TerrainType = unchecked((sbyte)MapF.TerrainType);
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

            Aow.Units.AoWE.AoWE.AoWHSMap.UnlockMapChangedEvent();
            Aow.Units.AoWE.AoWE.AoWHSMap.InvalidateMap();
        }

        // GlobalEnchantmentSpells.pas:1160
        public void Restore()
        {
            Aow.Units.Engine.TEObjectList Resources = default;
            Aow.Rtl.TList Candidates = default;
            int X = default;
            int Y = default;
            int I = default;
            int Index = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            sbyte Overlay = default;
            Aow.Units.HSEngine.TMapObjectResource Resource = default;
            Aow.Units.AoWE.AoWE.AoWHSMap.LockMapChangedEvent();
            var call__100 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(0);
            MapLevel = call__100;
            {
                Y = 1;
                int end__101 = MapLevel.Height - 1;
                for (; Y <= end__101; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
                    {
                        X = 1;
                        int end__102 = MapLevel.Width - 2;
                        for (; X <= end__102; X += 1)
                        {
                            if (FOriginalTerrain[Index] != 0)
                            {
                                MapF = MapLevel.GetField(X, Y);
                                if (TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)MapF.TerrainType))
                                {
                                    MapF.TerrainType = unchecked((sbyte)FOriginalTerrain[Index]);
                                    MapF.TerrainChanged();
                                }
                            }

                            Index += 2;
                        }
                    }
                }
            }

            Resources = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FChildren;
            Candidates = new Aow.Rtl.TList();
            {
                Y = 1;
                int end__103 = MapLevel.Height - 1;
                for (; Y <= end__103; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
                    {
                        X = 1;
                        int end__104 = MapLevel.Width - 2;
                        for (; X <= end__104; X += 1)
                        {
                            Overlay = unchecked((sbyte)FOriginalTerrain[Index + 1]);
                            if (Overlay > 0)
                            {
                                MapF = MapLevel.GetField(X, Y);
                                if (TSet<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)Overlay))
                                {
                                    Aow.Units.AoWE.AoWE.AoWHSSet.RoadControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, unchecked((sbyte)MapF.TerrainType));
                                }
                                else
                                {
                                    Candidates.Clear();
                                    {
                                        I = 0;
                                        int end__105 = Resources.Count - 1;
                                        for (; I <= end__105; I += 1)
                                        {
                                            Resource = (Aow.Units.HSEngine.TMapObjectResource)Resources.List[I];
                                            var call__106 = Resource.ClassType.ClassID();
                                            bool bool__107 = (call__106 == 131431);
                                            if (bool__107)
                                            {
                                                var call__108 = Resource.TerrainList.UsedTerrain();
                                                bool__107 = (call__108 == 1);
                                            }

                                            bool bool__109 = bool__107;
                                            if (bool__109)
                                            {
                                                var call__110 = Resource.TerrainList.Terrain[0];
                                                bool__109 = (call__110 == MapF.TerrainType);
                                            }

                                            bool bool__111 = bool__109;
                                            if (bool__111)
                                            {
                                                var call__112 = Resource.TerrainList.Overlay[0];
                                                bool__111 = (Overlay == call__112);
                                            }

                                            if (bool__111)
                                            {
                                                Candidates.Add(Resource);
                                            }
                                        }
                                    }

                                    if (Candidates.Count != 0)
                                    {
                                        var call__113 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Candidates.Count);
                                        var call__114 = Candidates[call__113];
                                        ((Aow.Units.HSEngine.TMapObjectResource)call__114).PlaceResourceUser(Aow.Units.AoWE.AoWE.AoWHSMap, unchecked((sbyte)X), unchecked((sbyte)Y), 0);
                                    }
                                }
                            }

                            Index += 2;
                        }
                    }
                }
            }

            Candidates.Free();
            Aow.Units.AoWE.AoWE.AoWHSMap.UnlockMapChangedEvent();
            Aow.Units.AoWE.AoWE.AoWHSMap.InvalidateMap();
        }

        // GlobalEnchantmentSpells.pas:201
        public static new Aow.Units.GlobalEnchantmentSpells.TFloodControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TFloodControl o = new Aow.Units.GlobalEnchantmentSpells.TFloodControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFloodEnchantment (instance size 48, VMT 557EEA10): $24 FRemoved, $25 FRestoreDays, $26 FDispelPlayer, $28 FTurnsActive, $2C FFloodControl. }
    public partial class TFloodEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public bool FRemoved;
        // { $24 }
        public sbyte FRestoreDays;
        // { $25 }
        public sbyte FDispelPlayer;
        // { $26 }
        public byte FReserved;
        // { $27 alignment }
        public int FTurnsActive;
        // { $28 }
        public Aow.Units.GlobalEnchantmentSpells.TFloodControl FFloodControl;
        // GlobalEnchantmentSpells.pas:1234
        // { $2C }
        public override void Destroy()
        {
            FFloodControl.Free();
            base.Destroy();
        }

        // GlobalEnchantmentSpells.pas:1250
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTurnsActive, 20);
            Stream.rwEObject(FFloodControl, 21);
            Stream.rwBool(ref FRemoved, 22);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDispelPlayer), 23);
            Stream.rwShortintEx(ref FRestoreDays, unchecked((sbyte)-1), 24);
        }

        // GlobalEnchantmentSpells.pas:1224
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__115 = Aow.Units.AoWE.AoWE.FloodRStr;
            var call__116 = Aow.Units.AoWE.AoWE.TranslateRStr(call__115);
            FName = call__116;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getFlood;
            var call__117 = Aow.Units.GlobalEnchantmentSpells.TFloodControl.Create(null);
            FFloodControl = call__117;
            FDispelMana = 100;
            FRestoreDays = unchecked((sbyte)-1);
        }

        // GlobalEnchantmentSpells.pas:1302
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if ((Msg.Value == FPlayerIndex) && !FRemoved)
                {
                    NewTurn();
                }

                if ((Msg.Value == FDispelPlayer) && FRemoved)
                {
                    FRestoreDays = (sbyte)(((int)FRestoreDays) - 1);
                    if (FRestoreDays <= 0)
                    {
                        FFloodControl.Restore();
                        base.Deactivate(unchecked((byte)FDispelPlayer));
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1268
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = base.GetName();
            if (FRemoved)
            {
                var call__118 = Aow.Units.AoWE.AoWE.FmtDaysLeftStr(FRestoreDays);
                Result = ((Result + ((AnsiString)" (")) + Aow.Rtl.Rtl.LowerCase(call__118)) + AnsiString.FromBytes(41);
            }
            else
            {
                if (DaysBeforeActive() != 0)
                {
                    var call__119 = DaysBeforeActive();
                    var call__120 = Aow.Units.AoWE.AoWE.XinYdaysRStr;
                    var call__121 = Aow.Rtl.Rtl.Format(call__120, [Result, (int)call__119]);
                    Result = call__121;
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
        public override bool CanDeactivate(byte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            if (!FRemoved)
            {
                Result = base.CanDeactivate(unchecked((byte)Player), ref Reason);
            }
            else
            {
                var call__122 = Aow.Units.AoWE.AoWE.XAlreadyRemovedRStr;
                var call__123 = Aow.Units.AoWE.AoWE.TranslateRStr(call__122);
                var call__124 = Aow.Rtl.Rtl.Format(call__123, [FName]);
                Reason = call__124;
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
                FDispelPlayer = unchecked((sbyte)Player);
                FRemoved = true;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous)
                {
                    FRestoreDays = 2;
                }
                else
                {
                    FRestoreDays = 1;
                }

                var call__125 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
                call__125.FMagicControl.Update();
                Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.Update();
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
            base.Activate(unchecked((sbyte)Player));
            FRemoved = false;
            FFloodControl.Setup();
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1260
        public int DaysBeforeActive()
        {
            int Result = default;
            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous) && (FTurnsActive < 2))
            {
                Result = 2 - FTurnsActive;
            }
            else
            {
                if ((Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn) && (FTurnsActive == 0))
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
            // { Unmatched: tried stage play mode and independent turn-count reloads; stage first mode read and retain second map-field expression. }
            if (!FRemoved)
            {
                FTurnsActive += 1;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous)
                {
                    if ((FTurnsActive > 1) && (FTurnsActive < 4))
                    {
                        FFloodControl.Flood();
                    }
                    else
                    {
                        if (FTurnsActive == 4)
                        {
                            var call__126 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
                            if (call__126 == 1)
                            {
                                FFloodControl.Flood();
                            }
                        }
                    }
                }
                else
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn)
                    {
                        if (FTurnsActive < 3)
                        {
                            FFloodControl.Flood();
                        }
                        else
                        {
                            if (FTurnsActive == 3)
                            {
                                var call__127 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
                                if (call__127 == 1)
                                {
                                    FFloodControl.Flood();
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:218
        public static new Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1240
            public override int ClassID()
            {
                int Result = default;
                Result = 135960;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:218
            public new Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFlood (instance size 56, VMT 557EEB2C) }
    public partial class TFlood : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1356
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__128 = Aow.Units.AoWE.AoWE.FloodRStr;
            var call__129 = Aow.Units.AoWE.AoWE.TranslateRStr(call__128);
            FSpellName = call__129;
            FSpellID = 50;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
        }

        // GlobalEnchantmentSpells.pas:1370
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
            var call__130 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(50);
            var call__131 = call__130.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__131, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1353
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1367
        public void ShowAnimation(int A1, int A2, int A3, int A4)
        {
        }

        // GlobalEnchantmentSpells.pas:234
        public static new Aow.Units.GlobalEnchantmentSpells.TFlood Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TFlood o = new Aow.Units.GlobalEnchantmentSpells.TFlood();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { draft }
    // { TLifeMasteryEnchantment (instance size 36, VMT 557EEC4C) }
    public partial class TLifeMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1387
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__132 = Aow.Units.AoWE.AoWE.LifeMasteryRStr;
            var call__133 = Aow.Units.AoWE.AoWE.TranslateRStr(call__132);
            FName = call__133;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
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
            Aow.Units.Engine.TEMsg Msg = default;
            base.Deactivate(unchecked((byte)Player));
            // { Original doubles Death. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msDeath);
            var call__134 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__135.FMagicControl.SetActiveEnchantments(call__134.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery));
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537002240, this);
            Aow.Units.AoWE.AoWE.AoWHSMap.SendMsg(ref Msg, this);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1401
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            if (Result)
            {
                var call__136 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__136.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery))
                {
                    var call__137 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__138 = Aow.Units.AoWE.AoWE.TranslateRStr(call__137);
                    var call__139 = GetName();
                    var call__140 = Aow.Rtl.Rtl.Format(call__138, [call__139]);
                    Reason = call__140;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1412
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Death. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msDeath);
            var call__141 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__141;
            {
                var w__142 = APlayer.FMagicControl;
                w__142.SetActiveEnchantments(w__142.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery));
            }

            {
                I = APlayer.FArmies.Count - 1;
                byte end__143 = 0;
                for (; I >= end__143; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__144 = 0;
                        for (; J >= end__144; J -= 1)
                        {
                            var call__145 = APlayer.FArmies.GetArmy(I);
                            var call__146 = call__145.GetUnit(J);
                            call__146.SetLifeMasteryBlessing();
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:243
        public static new Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1395
            public override int ClassID()
            {
                int Result = default;
                Result = 135967;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:243
            public new Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TLifeMastery (instance size 56, VMT 557EED74) }
    public partial class TLifeMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1441
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__147 = Aow.Units.AoWE.AoWE.LifeMasteryRStr;
            var call__148 = Aow.Units.AoWE.AoWE.TranslateRStr(call__147);
            FSpellName = call__148;
            FSpellID = 61;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Life Mastery. }
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1457
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.ExecuteTE(A1, A2, A3);
            Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msLife);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
            var call__149 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(61);
            var call__150 = call__149.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__150, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1454
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:254
        public static new Aow.Units.GlobalEnchantmentSpells.TLifeMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TLifeMastery o = new Aow.Units.GlobalEnchantmentSpells.TLifeMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TDeathMasteryEnchantment (instance size 36, VMT 557EEEA4) }
    public partial class TDeathMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1475
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__151 = Aow.Units.AoWE.AoWE.DeathMasteryRStr;
            var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(call__151);
            FName = call__152;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1568
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                if (Msg.Value != 0)
                {
                    var call__153 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__153.FMagicControl.FEnemyEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery))
                    {
                        NewTurn(unchecked((sbyte)Msg.Value));
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
            Aow.Units.Engine.TEMsg Msg = default;
            base.Deactivate(unchecked((byte)Player));
            // { Original doubles Life. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msLife);
            var call__154 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__155 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__155.FMagicControl.SetActiveEnchantments(call__154.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery));
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537002241, this);
            Aow.Units.AoWE.AoWE.AoWHSMap.SendMsg(ref Msg, this);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1489
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            if (Result)
            {
                var call__156 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__156.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery))
                {
                    var call__157 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__158 = Aow.Units.AoWE.AoWE.TranslateRStr(call__157);
                    var call__159 = GetName();
                    var call__160 = Aow.Rtl.Rtl.Format(call__158, [call__159]);
                    Reason = call__160;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1500
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Life. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msLife);
            var call__161 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__162 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__162.FMagicControl.SetActiveEnchantments(call__161.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery));
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
            Ptr<byte> Flags = default;
            int Mask = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            bool Border = default;
            Flags = default;
            Mask = 1 << (Player - 1);
            {
                L = 0;
                int end__163 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__163; L += 1)
                {
                    var call__164 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
                    MapLevel = call__164;
                    Index = 0;
                    Flags = Flags.Realloc(MapLevel.Width * MapLevel.Height);
                    Aow.Rtl.Rtl.FillBytes(Flags.Array.AsSpan(Flags.Index, MapLevel.Width * MapLevel.Height), 0);
                    {
                        Y = 1;
                        int end__165 = MapLevel.Height - 1;
                        for (; Y <= end__165; Y += 1)
                        {
                            {
                                X = 1;
                                int end__166 = MapLevel.Width - 2;
                                for (; X <= end__166; X += 1)
                                {
                                    var call__167 = MapLevel.GetField(X, Y);
                                    if ((((Aow.Units.AoWE.TAoWMapField)call__167).FFieldFlags & Mask) != 0)
                                    {
                                        Border = false;
                                        Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 1);
                                        while ((Walk.Radius == 1) && !Border)
                                        {
                                            if ((((Walk.X >= 0) && (Walk.X < MapLevel.Width)) && (Walk.Y >= 0)) && (Walk.Y < MapLevel.Height))
                                            {
                                                var call__168 = MapLevel.GetField(Walk.X, Walk.Y);
                                                Border = (((Aow.Units.AoWE.TAoWMapField)call__168).FFieldFlags & Mask) == 0;
                                            }

                                            Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
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
                        int end__169 = MapLevel.Height - 1;
                        for (; Y <= end__169; Y += 1)
                        {
                            {
                                X = 1;
                                int end__170 = MapLevel.Width - 2;
                                for (; X <= end__170; X += 1)
                                {
                                    if (Flags[Index] != 0)
                                    {
                                        var call__171 = MapLevel.GetField(X, Y);
                                        ((Aow.Units.AoWE.TAoWMapField)call__171).SetExplored(Player, false);
                                    }

                                    Index += 1;
                                }
                            }
                        }
                    }
                }
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateVisibility();
            Flags = Flags.Realloc(0);
        }

        // GlobalEnchantmentSpells.pas:262
        public static new Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1483
            public override int ClassID()
            {
                int Result = default;
                Result = 135968;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:262
            public new Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TDeathMastery (instance size 56, VMT 557EEFD0) }
    public partial class TDeathMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1577
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__172 = Aow.Units.AoWE.AoWE.DeathMasteryRStr;
            var call__173 = Aow.Units.AoWE.AoWE.TranslateRStr(call__172);
            FSpellName = call__173;
            FSpellID = 62;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Death Mastery. }
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1618
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msDeath);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            {
                var w__174 = Aow.Units.Animation.TAnimation.Create(null);
                w__174.FAnimationLevel = 28;
                w__174.FOnShow = ShowAnimation;
                w__174.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                w__174.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                w__174.Activate();
            }

            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1590
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1593
        public void ShowAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int I = default;
            int Count = default;
            int X = default;
            int Y = default;
            int Speed = default;
            int D = default;
            int Image = default;
            Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.Frame - Anim.FFrameCounter;
            switch (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail)
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
                int end__175 = Count - 1;
                for (; I <= end__175; I += 1)
                {
                    var call__176 = Aow.Rtl.Rtl.Random(Anim.FTag & 65535);
                    X = call__176;
                    var call__177 = Aow.Rtl.Rtl.Random((int)(((uint)((int)unchecked((uint)Anim.FTag))) >> 16));
                    Y = call__177;
                    Speed = Aow.Rtl.Rtl.Random(15) + 5;
                    Image = Aow.Rtl.Rtl.Random(3);
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

        // GlobalEnchantmentSpells.pas:275
        public static new Aow.Units.GlobalEnchantmentSpells.TDeathMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TDeathMastery o = new Aow.Units.GlobalEnchantmentSpells.TDeathMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFireMasteryEnchantment (instance size 36, VMT 557EF100) }
    public partial class TFireMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1639
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__178 = Aow.Units.AoWE.AoWE.FireMasteryRStr;
            var call__179 = Aow.Units.AoWE.AoWE.TranslateRStr(call__178);
            FName = call__179;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getFireMastery;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1724
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if (Msg.Value != 0)
                {
                    var call__180 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__180.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery))
                    {
                        UpdateFire(2, !Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType);
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Deactivate(unchecked((byte)Player));
            var call__181 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__181;
            {
                var w__182 = APlayer.FMagicControl;
                w__182.SetActiveEnchantments(w__182.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery));
            }

            {
                I = APlayer.FArmies.Count - 1;
                byte end__183 = 0;
                for (; I >= end__183; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__184 = 0;
                        for (; J >= end__184; J -= 1)
                        {
                            var call__185 = APlayer.FArmies.GetArmy(I);
                            var call__186 = call__185.GetUnit(J);
                            call__186.ResetFireMasteryProtection();
                        }
                    }
                }
            }

            // { Original doubles Water. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msWater);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1653
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__187 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            if (call__187.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery))
            {
                var call__188 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                var call__189 = Aow.Units.AoWE.AoWE.TranslateRStr(call__188);
                var call__190 = GetName();
                var call__191 = Aow.Rtl.Rtl.Format(call__189, [call__190]);
                Reason = call__191;
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Water. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msWater);
            var call__192 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__192;
            {
                var w__193 = APlayer.FMagicControl;
                w__193.SetActiveEnchantments(w__193.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery));
            }

            {
                I = APlayer.FArmies.Count - 1;
                byte end__194 = 0;
                for (; I >= end__194; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__195 = 0;
                        for (; J >= end__195; J -= 1)
                        {
                            var call__196 = APlayer.FArmies.GetArmy(I);
                            var call__197 = call__196.GetUnit(J);
                            call__197.SetFireMasteryProtection();
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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.AoWE.TFire Fire = default;
            var call__198 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__198;
            {
                L = 0;
                int end__199 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__199; L += 1)
                {
                    var call__200 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
                    MapLevel = call__200;
                    {
                        Y = 1;
                        int end__201 = MapLevel.Height - 1;
                        for (; Y <= end__201; Y += 1)
                        {
                            {
                                X = 1;
                                int end__202 = MapLevel.Width - 2;
                                for (; X <= end__202; X += 1)
                                {
                                    var call__203 = MapLevel.GetField(X, Y);
                                    var call__204 = Aow.Units.AoWE.AoWE.IgnitionChance((Aow.Units.AoWE.TAoWMapField)call__203);
                                    Chance = call__204;
                                    if (Chance > 0)
                                    {
                                        var call__205 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(10000);
                                        if (call__205 < (Chance * Factor))
                                        {
                                            var call__206 = Aow.Units.AoWE.AoWE.AoWHSSet.FFireControl.PlaceFire(X, Y, L, Aow.Units.AoWE.TFireType.ftLarge);
                                            Fire = call__206;
                                            if (Fire != null)
                                            {
                                                if (Explosions)
                                                {
                                                    var call__207 = Aow.Rtl.Rtl.Random(30);
                                                    Fire.SetupExplosion(call__207);
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

        // GlobalEnchantmentSpells.pas:284
        public static new Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1647
            public override int ClassID()
            {
                int Result = default;
                Result = 135969;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:284
            public new Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFireMastery (instance size 56, VMT 557EF228) }
    public partial class TFireMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1733
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__208 = Aow.Units.AoWE.AoWE.FireMasteryRStr;
            var call__209 = Aow.Units.AoWE.AoWE.TranslateRStr(call__208);
            FSpellName = call__209;
            FSpellID = 63;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1792
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref L);
                Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msFire);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                {
                    var w__210 = Aow.Units.Animation.TAnimation.Create(null);
                    w__210.FAnimationLevel = 27;
                    w__210.FOnShow = ShowAnimation;
                    w__210.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                    w__210.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                    w__210.Activate();
                }

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
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1749
        public void ShowAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int FieldStep = default;
            int Chance = default;
            int Delay = default;
            int X = default;
            int Y = default;
            int MaxY = default;
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int MaxX = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            switch (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail)
            {
                case 0:
                    Chance = 2;
                    break;
                case 2:
                    Chance = 5;
                    break;
                default:
                    Chance = 3;
                    break;
            }

            var call__211 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWE.AoWE.AoWHSMap.ViewL);
            MapLevel = call__211;
            FieldStep = MapLevel.FHNOffsets[0][4];
            MaxX = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidthHX) - 1;
            MaxY = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeightHX) - 1;
            {
                X = Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx;
                int end__212 = MaxX;
                for (; X <= end__212; X += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
                    Xhp = (Xhp - XhpSurf) + 16;
                    Yhp = (Yhp - YhpSurf) + 16;
                    var call__213 = MapLevel.GetField(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx);
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__213;
                    {
                        Y = Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx;
                        int end__214 = MaxY;
                        for (; Y <= end__214; Y += 1)
                        {
                            // { Original excludes border terrain. }
                            bool bool__215 = !TSet<byte>.WithBounds(0, 255).With(7).With(8).With(15).Contains((byte)MapF.TerrainType) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1);
                            if (bool__215)
                            {
                                bool bool__216 = !Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
                                if (!bool__216)
                                {
                                    var call__217 = MapF.GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                    bool__216 = call__217;
                                }

                                bool__215 = bool__216;
                            }

                            if (bool__215)
                            {
                                Aow.Rtl.Rtl.RandSeed = (MapF.Xhx + (MapF.Yhx * 8)) + (Ptr<byte>.Unbox(MapF)[26] << 16);
                                if (Aow.Rtl.Rtl.Random(100) < Chance)
                                {
                                    Delay = Aow.Rtl.Rtl.Random(40);
                                    FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - Delay, SurfaceDesc);
                                    if ((Anim.FFrameCounter == Delay) && (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0))
                                    {
                                        Aow.Units.Sound.Sound.PlaySample(Aow.Units.AoWE.AoWE.ExplosionSFX[X % 2]);
                                    }
                                }
                            }

                            /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                            throw new System.NotImplementedException();
                            Yhp += 32;
                        }
                    }
                }
            }

            if (Anim.FFrameCounter == 90)
            {
                Anim.Destroy();
            }
        }

        // GlobalEnchantmentSpells.pas:297
        public static new Aow.Units.GlobalEnchantmentSpells.TFireMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TFireMastery o = new Aow.Units.GlobalEnchantmentSpells.TFireMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TEarthMasteryEnchantment (instance size 36, VMT 557EF358) }
    public partial class TEarthMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1820
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__218 = Aow.Units.AoWE.AoWE.EarthMasteryRStr;
            var call__219 = Aow.Units.AoWE.AoWE.TranslateRStr(call__218);
            FName = call__219;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getNone;
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Deactivate(unchecked((byte)Player));
            var call__220 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__220;
            {
                var w__221 = APlayer.FMagicControl;
                w__221.SetActiveEnchantments(w__221.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery));
            }

            {
                I = APlayer.FArmies.Count - 1;
                byte end__222 = 0;
                for (; I >= end__222; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__223 = 0;
                        for (; J >= end__223; J -= 1)
                        {
                            var call__224 = APlayer.FArmies.GetArmy(I);
                            var call__225 = call__224.GetUnit(J);
                            call__225.ResetEarthMasteryStoneSkin();
                        }
                    }
                }
            }

            // { Original doubles Air. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msAir);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1834
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanActivate(unchecked((sbyte)Player), ref Reason);
            if (Result)
            {
                var call__226 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__226.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery))
                {
                    var call__227 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__228 = Aow.Units.AoWE.AoWE.TranslateRStr(call__227);
                    var call__229 = Aow.Rtl.Rtl.Format(call__228, [FName]);
                    Reason = call__229;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1845
        public override bool Activate(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Air. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msAir);
            var call__230 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__230;
            {
                var w__231 = APlayer.FMagicControl;
                w__231.SetActiveEnchantments(w__231.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery));
            }

            {
                I = APlayer.FArmies.Count - 1;
                byte end__232 = 0;
                for (; I >= end__232; I -= 1)
                {
                    {
                        J = APlayer.FArmies.GetArmy(I).Count - 1;
                        byte end__233 = 0;
                        for (; J >= end__233; J -= 1)
                        {
                            var call__234 = APlayer.FArmies.GetArmy(I);
                            var call__235 = call__234.GetUnit(J);
                            call__235.SetEarthMasteryStoneSkin();
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:306
        public static new Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1828
            public override int ClassID()
            {
                int Result = default;
                Result = 135970;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:306
            public new Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEarthMastery (instance size 56, VMT 557EF484) }
    public partial class TEarthMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1877
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__236 = Aow.Units.AoWE.AoWE.EarthMasteryRStr;
            var call__237 = Aow.Units.AoWE.AoWE.TranslateRStr(call__236);
            FSpellName = call__237;
            FSpellID = 64;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1918
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            Aow.Units.GlobalEnchantmentSpells.GlobalEnchantmentSpells.NotifySpellSphere(unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.TMagicSphere.msEarth);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxEarthMastery);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1932
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1890
        public void ShowAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int MaxX = default;
            int Y = default;
            int MaxY = default;
            int Xhp = default;
            int Yhp = default;
            int FieldStep = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            var call__238 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWE.AoWE.AoWHSMap.ViewL);
            MapLevel = call__238;
            FieldStep = MapLevel.FHNOffsets[0][4];
            MaxX = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidthHX) - 1;
            MaxY = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeightHX) - 1;
            {
                X = Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx;
                int end__239 = MaxX;
                for (; X <= end__239; X += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
                    Xhp = (Xhp - XhpSurf) + 16;
                    Yhp = (Yhp - YhpSurf) + 16;
                    var call__240 = MapLevel.GetField(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx);
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__240;
                    {
                        Y = Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx;
                        int end__241 = MaxY;
                        for (; Y <= end__241; Y += 1)
                        {
                            if (unchecked((short)MapF.OverlayType) == 4)
                            {
                                FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - (((int)(((uint)((int)Aow.Rtl.Rtl.ObjectToInt(MapF))) >> 8)) & 15), SurfaceDesc);
                            }

                            // { Original uses the field address. }
                            /* TODO(T-A3) Object pool address increment requires managed pool traversal */
                            throw new System.NotImplementedException();
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

        // GlobalEnchantmentSpells.pas:317
        public static new Aow.Units.GlobalEnchantmentSpells.TEarthMastery Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TEarthMastery o = new Aow.Units.GlobalEnchantmentSpells.TEarthMastery();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TSpellWardEnchantment (instance size 36, VMT 557EF5B4) }
    public partial class TSpellWardEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1935
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__242 = Aow.Units.AoWE.AoWE.SpellWardRStr;
            var call__243 = Aow.Units.AoWE.AoWE.TranslateRStr(call__242);
            FName = call__243;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getSpellWard;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:1946
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Result = 500;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:326
        public static new Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment o = new Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1943
            public override int ClassID()
            {
                int Result = default;
                Result = 135971;
                return Result;
            }

            // GlobalEnchantmentSpells.pas:326
            public new Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment o = (Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSpellWard (instance size 56, VMT 557EF6D8) }
    public partial class TSpellWard : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1949
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__244 = Aow.Units.AoWE.AoWE.SpellWardRStr;
            var call__245 = Aow.Units.AoWE.AoWE.TranslateRStr(call__244);
            FSpellName = call__245;
            FSpellID = 65;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
        }

        // GlobalEnchantmentSpells.pas:1960
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref L);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
            Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxSpellWard);
            FCastSounds.Play(0);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1973
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment.Create(null);
            return Result;
        }

        // GlobalEnchantmentSpells.pas:334
        public static new Aow.Units.GlobalEnchantmentSpells.TSpellWard Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalEnchantmentSpells.TSpellWard o = new Aow.Units.GlobalEnchantmentSpells.TSpellWard();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class GlobalEnchantmentSpells
    {
        // GlobalEnchantmentSpells.pas:1976
        public static void RegisterGlobalEnchantmentSpells(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWE.TGlobalEnchantmentEventLog>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.THatredEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment>()]);
            var call__246 = Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads.Create(null);
            Control.RegisterSpell(call__246);
            var call__247 = Aow.Units.GlobalEnchantmentSpells.THatred.Create(null);
            Control.RegisterSpell(call__247);
            var call__248 = Aow.Units.GlobalEnchantmentSpells.THarmony.Create(null);
            Control.RegisterSpell(call__248);
            var call__249 = Aow.Units.GlobalEnchantmentSpells.TPowerLeak.Create(null);
            Control.RegisterSpell(call__249);
            var call__250 = Aow.Units.GlobalEnchantmentSpells.TFlood.Create(null);
            Control.RegisterSpell(call__250);
            var call__251 = Aow.Units.GlobalEnchantmentSpells.TAirMastery.Create(null);
            Control.RegisterSpell(call__251);
            var call__252 = Aow.Units.GlobalEnchantmentSpells.TLifeMastery.Create(null);
            Control.RegisterSpell(call__252);
            var call__253 = Aow.Units.GlobalEnchantmentSpells.TDeathMastery.Create(null);
            Control.RegisterSpell(call__253);
            var call__254 = Aow.Units.GlobalEnchantmentSpells.TFireMastery.Create(null);
            Control.RegisterSpell(call__254);
            var call__255 = Aow.Units.GlobalEnchantmentSpells.TEarthMastery.Create(null);
            Control.RegisterSpell(call__255);
            var call__256 = Aow.Units.GlobalEnchantmentSpells.TWaterMastery.Create(null);
            Control.RegisterSpell(call__256);
            var call__257 = Aow.Units.GlobalEnchantmentSpells.TSpellWard.Create(null);
            Control.RegisterSpell(call__257);
        }

        // GlobalEnchantmentSpells.pas:1998
        public static void UnregisterGlobalEnchantmentSpells(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
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
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWE.TGlobalEnchantmentEventLog>()]);
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.THatredEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment>()]);
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment>()]);
        }

        // GlobalEnchantmentSpells.pas:369
        public static void NotifySpellSphere(sbyte Player, Aow.Units.AoWE.TMagicSphere Sphere)
        {
            Aow.Units.Engine.TEMsg Msg = default;
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537002242, null);
            Msg.Param[0] = Player;
            Msg.Param[1] = (int)Sphere;
            Aow.Units.AoWE.AoWE.AoWHSMap.SendMsg(ref Msg, null);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
