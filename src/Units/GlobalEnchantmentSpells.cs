// Translated from GlobalEnchantmentSpells.pas.
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
        // GlobalEnchantmentSpells.pas:389
        public new const int ClassID = 135954;

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
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE)base.Create(AOwner);
        }
    }

    // { TGlobalEnchantmentSpell (instance size 56, VMT 557EDA18): $34 FAIBasePriority. }
    public partial class TGlobalEnchantmentSpell : Aow.Units.AoWE.TSpell
    {
        public int FAIBasePriority;
        // GlobalEnchantmentSpells.pas:413
        // { $34 }
        private protected TGlobalEnchantmentSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
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
                var hero = (Aow.Units.AoWE.THero)A1;
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
            Enchantment = new Aow.Units.AoWE.TGlobalEnchantment(null);
            Result = Enchantment.GetAIUpkeepPriority();
            Enchantment.Release();
            return Result;
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TGlobalEnchantmentSpell(context, owner));
    }

    // { TEnchantedRoadsEnchantment (instance size 36, VMT 557EDB5C) }
    public partial class TEnchantedRoadsEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:535
        public new const int ClassID = 135955;

        // GlobalEnchantmentSpells.pas:527
        private protected TEnchantedRoadsEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
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

        public static new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TEnchantedRoadsEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:535
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEnchantedRoads (instance size 56, VMT 557EDC88) }
    public partial class TEnchantedRoads : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:571
        private protected TEnchantedRoads(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
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

        public static new Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TEnchantedRoads(context, owner));
    }

    // { TAirMasteryEnchantment (instance size 36, VMT 557EDDBC) }
    public partial class TAirMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:649
        public new const int ClassID = 135966;

        // GlobalEnchantmentSpells.pas:641
        private protected TAirMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__37 = Aow.Units.AoWE.AoWE.AirMasteryRStr;
            var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(call__37);
            FName = call__38;
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
            var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__40.FMagicControl.SetActiveEnchantments(call__39.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery));
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
                var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__41.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery))
                {
                    var call__42 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(call__42);
                    var call__44 = GetName();
                    var call__45 = Aow.Rtl.Rtl.Format(call__43, [call__44]);
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
            base.Activate(unchecked((sbyte)Player));
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__47.FMagicControl.SetActiveEnchantments(call__46.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petAirMastery));
            // { Original uses Earth. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msEarth);
            Result = true;
            return Result;
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAirMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:649
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TAirMastery (instance size 56, VMT 557EDEE0) }
    public partial class TAirMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:688
        private protected TAirMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__48 = Aow.Units.AoWE.AoWE.AirMasteryRStr;
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(call__48);
            FSpellName = call__49;
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
            var call__50 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(60);
            call__50.FCastSounds.Play(0);
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
                    int end__51 = Count - 1;
                    for (; I <= end__51; I += 1)
                    {
                        var call__52 = Aow.Rtl.Rtl.Random(Anim.FTag & 65535);
                        X = call__52;
                        var call__53 = Aow.Rtl.Rtl.Random((int)(((uint)((int)unchecked((uint)Anim.FTag))) >> 16));
                        Y = call__53;
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

        public static new Aow.Units.GlobalEnchantmentSpells.TAirMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAirMastery(context, owner));
    }

    // { TWaterMasteryEnchantment (instance size 36, VMT 557EE00C) }
    public partial class TWaterMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:760
        public new const int ClassID = 135956;

        // GlobalEnchantmentSpells.pas:752
        private protected TWaterMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__54 = Aow.Units.AoWE.AoWE.WaterMasteryRStr;
            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(call__54);
            FName = call__55;
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
            var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__57 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__57.FMagicControl.SetActiveEnchantments(call__56.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery));
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
                var call__58 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__58.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery))
                {
                    var call__59 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(call__59);
                    var call__61 = GetName();
                    var call__62 = Aow.Rtl.Rtl.Format(call__60, [call__61]);
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
            base.Activate(unchecked((sbyte)Player));
            var call__63 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__64.FMagicControl.SetActiveEnchantments(call__63.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery));
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msFire);
            Result = true;
            return Result;
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TWaterMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:760
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWaterMastery (instance size 56, VMT 557EE138) }
    public partial class TWaterMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:806
        private protected TWaterMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__65 = Aow.Units.AoWE.AoWE.WaterMasteryRStr;
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(call__65);
            FSpellName = call__66;
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

        public static new Aow.Units.GlobalEnchantmentSpells.TWaterMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TWaterMastery(context, owner));
    }

    // { THatredEnchantment (instance size 40, VMT 557EE268): $24 FDaysActive. }
    public partial class THatredEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:841
        public new const int ClassID = 135957;

        public int FDaysActive;
        // GlobalEnchantmentSpells.pas:851
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        // GlobalEnchantmentSpells.pas:833
        private protected THatredEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__67 = Aow.Units.AoWE.AoWE.HatredRStr;
            var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(call__67);
            FName = call__68;
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

        public static new Aow.Units.GlobalEnchantmentSpells.THatredEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THatredEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:841
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.THatredEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.THatredEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.THatredEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.THatredEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { THatred (instance size 56, VMT 557EE384) }
    public partial class THatred : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:889
        private protected THatred(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__69 = Aow.Units.AoWE.AoWE.HatredRStr;
            var call__70 = Aow.Units.AoWE.AoWE.TranslateRStr(call__69);
            FSpellName = call__70;
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

        public static new Aow.Units.GlobalEnchantmentSpells.THatred Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THatred(context, owner));
    }

    // { THarmonyEnchantment (instance size 40, VMT 557EE4A8): $24 FDaysActive. }
    public partial class THarmonyEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:921
        public new const int ClassID = 135958;

        public int FDaysActive;
        // GlobalEnchantmentSpells.pas:931
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        // GlobalEnchantmentSpells.pas:913
        private protected THarmonyEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__71 = Aow.Units.AoWE.AoWE.HarmonyRStr;
            var call__72 = Aow.Units.AoWE.AoWE.TranslateRStr(call__71);
            FName = call__72;
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

        public static new Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THarmonyEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:921
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { THarmony (instance size 56, VMT 557EE5C8) }
    public partial class THarmony : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:969
        private protected THarmony(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__73 = Aow.Units.AoWE.AoWE.HarmonyRStr;
            var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(call__73);
            FSpellName = call__74;
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

        public static new Aow.Units.GlobalEnchantmentSpells.THarmony Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THarmony(context, owner));
    }

    // { TPowerLeakEnchantment (instance size 36, VMT 557EE6F0) }
    public partial class TPowerLeakEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1001
        public new const int ClassID = 135959;

        // GlobalEnchantmentSpells.pas:993
        private protected TPowerLeakEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__75 = Aow.Units.AoWE.AoWE.PowerLeakRStr;
            var call__76 = Aow.Units.AoWE.AoWE.TranslateRStr(call__75);
            FName = call__76;
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
                byte end__77 = 0;
                for (; I >= end__77; I -= 1)
                {
                    var call__78 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
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
            base.Activate(unchecked((sbyte)Player));
            {
                I = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                byte end__79 = 0;
                for (; I >= end__79; I -= 1)
                {
                    var call__80 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    call__80.FMagicControl.Update();
                }
            }

            Result = true;
            return Result;
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TPowerLeakEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1001
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPowerLeak (instance size 56, VMT 557EE814) }
    public partial class TPowerLeak : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1036
        private protected TPowerLeak(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__81 = Aow.Units.AoWE.AoWE.PowerLeakRStr;
            var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(call__81);
            FSpellName = call__82;
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
            var call__83 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(48);
            var call__84 = call__83.FCastImages.ImageSequences[0];
            Anim.Setup(call__84, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmImplode), 100, 64, 50, 30);
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

        public static new Aow.Units.GlobalEnchantmentSpells.TPowerLeak Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TPowerLeak(context, owner));
    }

    // { TFloodControl (instance size 20, VMT 557EE93C): $08 FWidth, $0C FHeight, $10 FOriginalTerrain. }
    public partial class TFloodControl : Aow.Units.Engine.TEObject
    {
        public int FWidth;
        // { $08 }
        public int FHeight;
        // { $0C }
        public sbyte[] FOriginalTerrain = [];
        // GlobalEnchantmentSpells.pas:1072
        // { $10 }
        public override void Destroy()
        {
            if (FOriginalTerrain.Length != 0)
            {
                Array.Resize(ref FOriginalTerrain, 0);
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
                    Array.Resize(ref FOriginalTerrain, (FWidth * FHeight) * 2);
                }

                Stream.rwData(MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, (FWidth * FHeight) * 2)), (FWidth * FHeight) * 2, 22);
            }
        }

        // GlobalEnchantmentSpells.pas:1066
        public TFloodControl(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FOriginalTerrain = [];
        }

        // GlobalEnchantmentSpells.pas:1090
        public void Setup()
        {
            FWidth = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Width;
            FHeight = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Height;
            Array.Resize(ref FOriginalTerrain, (FWidth * FHeight) * 2);
            Array.Clear(FOriginalTerrain);
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
            var call__85 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(0);
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
                                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 1);
                                while (Walk.Radius == 1)
                                {
                                    var call__89 = MapLevel.GetField(Walk.X, Walk.Y);
                                    MapF = call__89;
                                    if ((!TSet<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || ((MapF.TerrainType == 0) && (MapF.OverlayType == 5)))
                                    {
                                        Index = ((FWidth * Walk.Y) + Walk.X) * 2;
                                        bool bool__90 = (FOriginalTerrain[Index] == 0) && (FOriginalTerrain[Index + 1] == 0);
                                        if (bool__90)
                                        {
                                            var call__91 = MapF.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                                            bool__90 = (call__91 == null);
                                        }

                                        if (bool__90)
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
                int end__92 = MapLevel.Height - 1;
                for (; Y <= end__92; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
                    {
                        X = 1;
                        int end__93 = MapLevel.Width - 2;
                        for (; X <= end__93; X += 1)
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
            var call__94 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(0);
            MapLevel = call__94;
            {
                Y = 1;
                int end__95 = MapLevel.Height - 1;
                for (; Y <= end__95; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
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
                int end__97 = MapLevel.Height - 1;
                for (; Y <= end__97; Y += 1)
                {
                    Index = ((FWidth * Y) + 1) * 2;
                    {
                        X = 1;
                        int end__98 = MapLevel.Width - 2;
                        for (; X <= end__98; X += 1)
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
                                        int end__99 = Resources.Count - 1;
                                        for (; I <= end__99; I += 1)
                                        {
                                            Resource = (Aow.Units.HSEngine.TMapObjectResource)Resources.List[I];
                                            var call__100 = Resource.EClass.ClassID;
                                            bool bool__101 = (call__100 == 131431);
                                            if (bool__101)
                                            {
                                                var call__102 = Resource.TerrainList.UsedTerrain();
                                                bool__101 = (call__102 == 1);
                                            }

                                            bool bool__103 = bool__101;
                                            if (bool__103)
                                            {
                                                var call__104 = Resource.TerrainList.Terrain[0];
                                                bool__103 = (call__104 == MapF.TerrainType);
                                            }

                                            bool bool__105 = bool__103;
                                            if (bool__105)
                                            {
                                                var call__106 = Resource.TerrainList.Overlay[0];
                                                bool__105 = (Overlay == call__106);
                                            }

                                            if (bool__105)
                                            {
                                                Candidates.Add(Resource);
                                            }
                                        }
                                    }

                                    if (Candidates.Count != 0)
                                    {
                                        var call__107 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Candidates.Count);
                                        var call__108 = Candidates[call__107];
                                        ((Aow.Units.HSEngine.TMapObjectResource)call__108).PlaceResourceUser(Aow.Units.AoWE.AoWE.AoWHSMap, unchecked((sbyte)X), unchecked((sbyte)Y), 0);
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

    }

    // { TFloodEnchantment (instance size 48, VMT 557EEA10): $24 FRemoved, $25 FRestoreDays, $26 FDispelPlayer, $28 FTurnsActive, $2C FFloodControl. }
    public partial class TFloodEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1240
        public new const int ClassID = 135960;

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
        private protected TFloodEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__109 = Aow.Units.AoWE.AoWE.FloodRStr;
            var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(call__109);
            FName = call__110;
            FEnchantmentType = Aow.Units.AoWE.TGlobalEnchantmentType.getFlood;
            var call__111 = new Aow.Units.GlobalEnchantmentSpells.TFloodControl(null);
            FFloodControl = call__111;
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
                var call__112 = Aow.Units.AoWE.AoWE.FmtDaysLeftStr(FRestoreDays);
                Result = ((Result + ((AnsiString)" (")) + Aow.Rtl.Rtl.LowerCase(call__112)) + AnsiString.FromBytes(41);
            }
            else
            {
                if (DaysBeforeActive() != 0)
                {
                    var call__113 = DaysBeforeActive();
                    var call__114 = Aow.Units.AoWE.AoWE.XinYdaysRStr;
                    var call__115 = Aow.Rtl.Rtl.Format(call__114, [Result, (int)call__113]);
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
        public override bool CanDeactivate(byte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            if (!FRemoved)
            {
                Result = base.CanDeactivate(unchecked((byte)Player), ref Reason);
            }
            else
            {
                var call__116 = Aow.Units.AoWE.AoWE.XAlreadyRemovedRStr;
                var call__117 = Aow.Units.AoWE.AoWE.TranslateRStr(call__116);
                var call__118 = Aow.Rtl.Rtl.Format(call__117, [FName]);
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

                var call__119 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
                call__119.FMagicControl.Update();
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
                            var call__120 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
                            if (call__120 == 1)
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
                                var call__121 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
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

        public static new Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFloodEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1240
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFlood (instance size 56, VMT 557EEB2C) }
    public partial class TFlood : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1356
        private protected TFlood(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__122 = Aow.Units.AoWE.AoWE.FloodRStr;
            var call__123 = Aow.Units.AoWE.AoWE.TranslateRStr(call__122);
            FSpellName = call__123;
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
            var call__124 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(50);
            var call__125 = call__124.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__125, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
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

        public static new Aow.Units.GlobalEnchantmentSpells.TFlood Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFlood(context, owner));
    }

    // { draft }
    // { TLifeMasteryEnchantment (instance size 36, VMT 557EEC4C) }
    public partial class TLifeMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1395
        public new const int ClassID = 135967;

        // GlobalEnchantmentSpells.pas:1387
        private protected TLifeMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__126 = Aow.Units.AoWE.AoWE.LifeMasteryRStr;
            var call__127 = Aow.Units.AoWE.AoWE.TranslateRStr(call__126);
            FName = call__127;
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
            var call__128 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__129 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__129.FMagicControl.SetActiveEnchantments(call__128.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery));
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
                var call__130 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__130.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery))
                {
                    var call__131 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__132 = Aow.Units.AoWE.AoWE.TranslateRStr(call__131);
                    var call__133 = GetName();
                    var call__134 = Aow.Rtl.Rtl.Format(call__132, [call__133]);
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Death. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msDeath);
            var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__135;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petLifeMastery));

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

        public static new Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TLifeMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1395
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TLifeMastery (instance size 56, VMT 557EED74) }
    public partial class TLifeMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1441
        private protected TLifeMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__141 = Aow.Units.AoWE.AoWE.LifeMasteryRStr;
            var call__142 = Aow.Units.AoWE.AoWE.TranslateRStr(call__141);
            FSpellName = call__142;
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
            var call__143 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(61);
            var call__144 = call__143.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__144, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
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

        public static new Aow.Units.GlobalEnchantmentSpells.TLifeMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TLifeMastery(context, owner));
    }

    // { TDeathMasteryEnchantment (instance size 36, VMT 557EEEA4) }
    public partial class TDeathMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1483
        public new const int ClassID = 135968;

        // GlobalEnchantmentSpells.pas:1475
        private protected TDeathMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__145 = Aow.Units.AoWE.AoWE.DeathMasteryRStr;
            var call__146 = Aow.Units.AoWE.AoWE.TranslateRStr(call__145);
            FName = call__146;
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
                    var call__147 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__147.FMagicControl.FEnemyEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery))
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
            var call__148 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__149 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__149.FMagicControl.SetActiveEnchantments(call__148.FMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery));
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
                var call__150 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__150.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery))
                {
                    var call__151 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(call__151);
                    var call__153 = GetName();
                    var call__154 = Aow.Rtl.Rtl.Format(call__152, [call__153]);
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
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Life. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msLife);
            var call__155 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            var call__156 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            call__156.FMagicControl.SetActiveEnchantments(call__155.FMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petDeathMastery));
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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            bool Border = default;
            Flags = [];
            Mask = 1 << (Player - 1);
            {
                L = 0;
                int end__157 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__157; L += 1)
                {
                    var call__158 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
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
                                    if ((((Aow.Units.AoWE.TAoWMapField)call__161).FFieldFlags & Mask) != 0)
                                    {
                                        Border = false;
                                        Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 1);
                                        while ((Walk.Radius == 1) && !Border)
                                        {
                                            if ((((Walk.X >= 0) && (Walk.X < MapLevel.Width)) && (Walk.Y >= 0)) && (Walk.Y < MapLevel.Height))
                                            {
                                                var call__162 = MapLevel.GetField(Walk.X, Walk.Y);
                                                Border = (((Aow.Units.AoWE.TAoWMapField)call__162).FFieldFlags & Mask) == 0;
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
                                        ((Aow.Units.AoWE.TAoWMapField)call__165).SetExplored(Player, false);
                                    }

                                    Index += 1;
                                }
                            }
                        }
                    }
                }
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateVisibility();
            Array.Resize(ref Flags, 0);
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDeathMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1483
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TDeathMastery (instance size 56, VMT 557EEFD0) }
    public partial class TDeathMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1577
        private protected TDeathMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__166 = Aow.Units.AoWE.AoWE.DeathMasteryRStr;
            var call__167 = Aow.Units.AoWE.AoWE.TranslateRStr(call__166);
            FSpellName = call__167;
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
            var animation = Aow.Units.Animation.TAnimation.Create(null);
            animation.FAnimationLevel = 28;
            animation.FOnShow = ShowAnimation;
            animation.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
            animation.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
            animation.Activate();

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
                int end__169 = Count - 1;
                for (; I <= end__169; I += 1)
                {
                    var call__170 = Aow.Rtl.Rtl.Random(Anim.FTag & 65535);
                    X = call__170;
                    var call__171 = Aow.Rtl.Rtl.Random((int)(((uint)((int)unchecked((uint)Anim.FTag))) >> 16));
                    Y = call__171;
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

        public static new Aow.Units.GlobalEnchantmentSpells.TDeathMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDeathMastery(context, owner));
    }

    // { TFireMasteryEnchantment (instance size 36, VMT 557EF100) }
    public partial class TFireMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1647
        public new const int ClassID = 135969;

        // GlobalEnchantmentSpells.pas:1639
        private protected TFireMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__172 = Aow.Units.AoWE.AoWE.FireMasteryRStr;
            var call__173 = Aow.Units.AoWE.AoWE.TranslateRStr(call__172);
            FName = call__173;
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
                    var call__174 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Msg.Value);
                    if (call__174.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery))
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
            var call__175 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__175;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery));

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
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.DecDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msWater);
            Result = true;
            return Result;
        }

        // GlobalEnchantmentSpells.pas:1653
        public override bool CanActivate(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__181 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            if (call__181.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery))
            {
                var call__182 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                var call__183 = Aow.Units.AoWE.AoWE.TranslateRStr(call__182);
                var call__184 = GetName();
                var call__185 = Aow.Rtl.Rtl.Format(call__183, [call__184]);
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Water. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msWater);
            var call__186 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__186;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petFireMastery));

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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.AoWE.TFire Fire = default;
            var call__192 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__192;
            {
                L = 0;
                int end__193 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__193; L += 1)
                {
                    var call__194 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
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
                                    var call__198 = Aow.Units.AoWE.AoWE.IgnitionChance((Aow.Units.AoWE.TAoWMapField)call__197);
                                    Chance = call__198;
                                    if (Chance > 0)
                                    {
                                        var call__199 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(10000);
                                        if (call__199 < (Chance * Factor))
                                        {
                                            var call__200 = Aow.Units.AoWE.AoWE.AoWHSSet.FFireControl.PlaceFire(X, Y, L, Aow.Units.AoWE.TFireType.ftLarge);
                                            Fire = call__200;
                                            if (Fire != null)
                                            {
                                                if (Explosions)
                                                {
                                                    var call__201 = Aow.Rtl.Rtl.Random(30);
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

        public static new Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1647
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFireMastery (instance size 56, VMT 557EF228) }
    public partial class TFireMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1733
        private protected TFireMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            var call__202 = Aow.Units.AoWE.AoWE.FireMasteryRStr;
            var call__203 = Aow.Units.AoWE.AoWE.TranslateRStr(call__202);
            FSpellName = call__203;
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
                var animation = Aow.Units.Animation.TAnimation.Create(null);
                animation.FAnimationLevel = 27;
                animation.FOnShow = ShowAnimation;
                animation.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                animation.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
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
        public override Aow.Units.AoWE.TGlobalEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            Result = Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment.Create(null);
            return Result;
        }

        public static new Aow.Units.GlobalEnchantmentSpells.TFireMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireMastery(context, owner));
    }

    // { TEarthMasteryEnchantment (instance size 36, VMT 557EF358) }
    public partial class TEarthMasteryEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1828
        public new const int ClassID = 135970;

        // GlobalEnchantmentSpells.pas:1820
        private protected TEarthMasteryEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__205 = Aow.Units.AoWE.AoWE.EarthMasteryRStr;
            var call__206 = Aow.Units.AoWE.AoWE.TranslateRStr(call__205);
            FName = call__206;
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
            var call__207 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__207;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery));

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
                var call__213 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__213.FMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery))
                {
                    var call__214 = Aow.Units.AoWE.AoWE.XIsAlreadyActiveRStr;
                    var call__215 = Aow.Units.AoWE.AoWE.TranslateRStr(call__214);
                    var call__216 = Aow.Rtl.Rtl.Format(call__215, [FName]);
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
            Aow.Units.AoWE.TPlayer APlayer = default;
            int I = default;
            int J = default;
            base.Activate(unchecked((sbyte)Player));
            // { Original doubles Air. }
            Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IncDoubleSphereManaCount(Aow.Units.AoWE.TMagicSphere.msAir);
            var call__217 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayerIndex);
            APlayer = call__217;
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + TSet<Aow.Units.AoWE.TPlayerEnchantmentType>.Of(Aow.Units.AoWE.TPlayerEnchantmentType.petEarthMastery));

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

        public static new Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TEarthMasteryEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1828
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEarthMastery (instance size 56, VMT 557EF484) }
    public partial class TEarthMastery : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1877
        private protected TEarthMastery(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__223 = Aow.Units.AoWE.AoWE.EarthMasteryRStr;
            var call__224 = Aow.Units.AoWE.AoWE.TranslateRStr(call__223);
            FSpellName = call__224;
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
            var call__225 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWE.AoWE.AoWHSMap.ViewL);
            MapLevel = call__225;
            FieldStep = MapLevel.FHNOffsets[0][4];
            MaxX = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidthHX) - 1;
            MaxY = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx + Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeightHX) - 1;
            {
                X = Aow.Units.AoWE.AoWE.AoWHSMap.ViewXhx;
                int end__226 = MaxX;
                for (; X <= end__226; X += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx, ref Xhp, ref Yhp);
                    Xhp = (Xhp - XhpSurf) + 16;
                    Yhp = (Yhp - YhpSurf) + 16;
                    var call__227 = MapLevel.GetField(X, Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx);
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__227;
                    {
                        Y = Aow.Units.AoWE.AoWE.AoWHSMap.ViewYhx;
                        int end__228 = MaxY;
                        for (; Y <= end__228; Y += 1)
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

        public static new Aow.Units.GlobalEnchantmentSpells.TEarthMastery Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TEarthMastery(context, owner));
    }

    // { TSpellWardEnchantment (instance size 36, VMT 557EF5B4) }
    public partial class TSpellWardEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1943
        public new const int ClassID = 135971;

        // GlobalEnchantmentSpells.pas:1935
        private protected TSpellWardEnchantment(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__229 = Aow.Units.AoWE.AoWE.SpellWardRStr;
            var call__230 = Aow.Units.AoWE.AoWE.TranslateRStr(call__229);
            FName = call__230;
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

        public static new Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSpellWardEnchantment(context, owner));

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // GlobalEnchantmentSpells.pas:1943
            public override int ClassID() => Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment.ClassID;

            public new Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
                => (Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSpellWard (instance size 56, VMT 557EF6D8) }
    public partial class TSpellWard : Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1949
        private protected TSpellWard(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__231 = Aow.Units.AoWE.AoWE.SpellWardRStr;
            var call__232 = Aow.Units.AoWE.AoWE.TranslateRStr(call__231);
            FSpellName = call__232;
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

        public static new Aow.Units.GlobalEnchantmentSpells.TSpellWard Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSpellWard(context, owner));
    }

    public static partial class GlobalEnchantmentSpells
    {
        // GlobalEnchantmentSpells.pas:1976
        public static void RegisterGlobalEnchantmentSpells(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWE.TGlobalEnchantmentEventLog>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.THatredEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment>()]);
            var call__233 = Aow.Units.GlobalEnchantmentSpells.TEnchantedRoads.Create(null);
            Control.RegisterSpell(call__233);
            var call__234 = Aow.Units.GlobalEnchantmentSpells.THatred.Create(null);
            Control.RegisterSpell(call__234);
            var call__235 = Aow.Units.GlobalEnchantmentSpells.THarmony.Create(null);
            Control.RegisterSpell(call__235);
            var call__236 = Aow.Units.GlobalEnchantmentSpells.TPowerLeak.Create(null);
            Control.RegisterSpell(call__236);
            var call__237 = Aow.Units.GlobalEnchantmentSpells.TFlood.Create(null);
            Control.RegisterSpell(call__237);
            var call__238 = Aow.Units.GlobalEnchantmentSpells.TAirMastery.Create(null);
            Control.RegisterSpell(call__238);
            var call__239 = Aow.Units.GlobalEnchantmentSpells.TLifeMastery.Create(null);
            Control.RegisterSpell(call__239);
            var call__240 = Aow.Units.GlobalEnchantmentSpells.TDeathMastery.Create(null);
            Control.RegisterSpell(call__240);
            var call__241 = Aow.Units.GlobalEnchantmentSpells.TFireMastery.Create(null);
            Control.RegisterSpell(call__241);
            var call__242 = Aow.Units.GlobalEnchantmentSpells.TEarthMastery.Create(null);
            Control.RegisterSpell(call__242);
            var call__243 = Aow.Units.GlobalEnchantmentSpells.TWaterMastery.Create(null);
            Control.RegisterSpell(call__243);
            var call__244 = Aow.Units.GlobalEnchantmentSpells.TSpellWard.Create(null);
            Control.RegisterSpell(call__244);
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
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TEnchantedRoadsEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TGlobalEnchantmentSpellTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWE.TGlobalEnchantmentEventLog>()]);
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TWaterMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.THatredEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.THarmonyEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TPowerLeakEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TFloodEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TAirMasteryEnchantment>()]);
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TLifeMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TDeathMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TFireMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TEarthMasteryEnchantment>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalEnchantmentSpells.TSpellWardEnchantment>()]);
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
