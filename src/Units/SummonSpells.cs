// Translated from SummonSpells.pas.
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
using Aow.Units.PassiveAb;
using static Aow.Units.PassiveAb.PassiveAb;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.SummonSpells
{
    // { TSummonSpellTE (instance size 36, VMT 557E2F28): $20 FUnitIndices. }
    public partial class TSummonSpellTE : Aow.Units.AoWE.TSpellTE
    {
        public Aow.Units.AoWE.TUnitIndexList FUnitIndices;
        // SummonSpells.pas:165
        // { $20 }
        public override void Destroy()
        {
            FUnitIndices.Free();
            base.Destroy();
        }

        // SummonSpells.pas:176
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(30, FUnitIndices.LoadFromStream, FUnitIndices.SaveToStream);
        }

        // SummonSpells.pas:159
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.AoWE.TUnitIndexList.Create();
            FUnitIndices = call__1;
        }

        // SummonSpells.pas:182
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            bool Done = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            int I = default;
            Aow.Units.AoWE.TUnit U = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            base.Process(A1, A2);
            Done = false;
            A1.GetLocation(ref X, ref Y, ref L);
            {
                I = 0;
                int end__2 = FUnitIndices.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    U = Aow.Units.AoWE.TUnit.Create(null);
                    var call__3 = FUnitIndices[I];
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__3);
                    U.SetUnitResource(call__4);
                    var call__5 = U.Place(X, Y, L, unchecked((sbyte)A1.FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea));
                    Army = call__5;
                    if (Army != null)
                    {
                        var call__6 = Army.GetYhx();
                        var call__7 = Army.GetXhx();
                        var call__8 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__7, call__6);
                        if (call__8 <= 2)
                        {
                            if (U.ExpandAbility(155))
                            {
                                var call__9 = U.GetAbilityData(155);
                                Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__9).FEnchantment;
                                var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                Enchantment.SetUpkeep(call__10.FManaUpkeep);
                                Enchantment.SetSource(unchecked((sbyte)A1.FPlayer));
                                var call__11 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__12 = ((Aow.Units.SummonSpells.TSummonSpell)call__11).GetDispelResistance(U);
                                Enchantment.FDispelResistance = unchecked((sbyte)call__12);
                                Group = A1.FGroup;
                                if (Group != null)
                                {
                                    Group.AddUnit(U);
                                }
                            }

                            Done = true;
                        }
                        else
                        {
                            U.SetOwner(null);
                        }
                    }

                    U.Release();
                }
            }

            if (Done)
            {
                if (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.MakeVisible(X, Y, L, 3);
                }

                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                if (call__13)
                {
                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    var call__15 = call__14.FCastImages.ImageSequences[0];
                    if (call__15 != null)
                    {
                        Anim = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                        var call__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                        var call__17 = call__16.FCastImages.ImageSequences[0];
                        Anim.SetImageSequence(call__17);
                        Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, X, Y, L);
                    }

                    bool bool__18 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    if (!bool__18)
                    {
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                        bool__18 = call__19;
                    }

                    if (bool__18)
                    {
                        var call__20 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                        call__20.FCastSounds.Play(0);
                    }
                }

                var call__21 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__22 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__22.CastingDone(A1, call__21.FBaseManaCost);
            }
            else
            {
                if (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    var call__23 = Aow.Units.AoWE.AoWE.NoSummonLocationFoundRStr;
                    var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(call__23);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__24);
                }
            }

            Result = false;
            return Result;
        }

        // SummonSpells.pas:34
        public static new Aow.Units.SummonSpells.TSummonSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.SummonSpells.TSummonSpellTE o = new Aow.Units.SummonSpells.TSummonSpellTE();
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
            // SummonSpells.pas:171
            public override int ClassID()
            {
                int Result = default;
                Result = 135947;
                return Result;
            }

            // SummonSpells.pas:34
            public new Aow.Units.SummonSpells.TSummonSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.SummonSpells.TSummonSpellTE o = (Aow.Units.SummonSpells.TSummonSpellTE)NewInstance();
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
                where T : Aow.Units.SummonSpells.TSummonSpellTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSummonSpell (instance size 60, VMT 557E3004): $34 FAIBasePriority, $38 FUnitIndices. }
    public partial class TSummonSpell : Aow.Units.AoWE.TSpell
    {
        // Raw allocation for private tools/oracle/MovementChecks/Program.cs, GetDispelResistance fixture.
        public TSummonSpell() { }

        public int FAIBasePriority;
        // { $34 }
        public Aow.Units.AoWE.TUnitIndexList FUnitIndices;
        // SummonSpells.pas:249
        // { $38 }
        public override void Destroy()
        {
            FUnitIndices.Free();
            base.Destroy();
        }

        // SummonSpells.pas:241
        private protected TSummonSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__25 = Aow.Units.AoWE.TUnitIndexList.Create();
            FUnitIndices = call__25;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FAIBasePriority = -1;
        }

        // SummonSpells.pas:288
        public override void AIPrefetchCastSpellActions(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TAICastSpellActionList A2)
        {
            bool CanCast = default;
            Aow.Units.AoWE.TAICastSpellAction Action = default;
            Aow.Rtl.AnsiString Reason = default;
            if (FAIBasePriority != -1)
            {
                var call__26 = GetAIUpkeepPriority();
                var call__27 = AIRequestCastSpellExpense(A1, FBaseManaCost, FManaUpkeep, call__26);
                CanCast = call__27;
                bool bool__28 = CanCast;
                if (bool__28)
                {
                    var call__29 = ((Aow.Units.AoWE.THero)A1).CanCastSpell(FSpellID, ref Reason);
                    bool__28 = call__29;
                }

                CanCast = bool__28;
                if (CanCast)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(A1, Action);
                    Action.FPriority = FAIBasePriority;
                    Action.SetOwner(A2);
                }
            }
        }

        // SummonSpells.pas:308
        public override bool AIExecuteCastSpellAction(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            var hero = (Aow.Units.AoWE.THero)A1;
            var call__31 = hero.CanCastSpell(FSpellID, ref Reason);
            if (call__31)
            {
                var call__32 = GetAIUpkeepPriority();
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, call__32);
                var call__33 = hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                if (call__33)
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

        // SummonSpells.pas:266
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Units.SummonSpells.TSummonSpellTE Event = default;
            Aow.Rtl.AnsiString Reason = default;
            if (CanActivate(A1, ref Reason))
            {
                Event = Aow.Units.SummonSpells.TSummonSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                SetupSummonSpellTE(Event);
                var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__34.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // SummonSpells.pas:261
        public virtual void SetupSummonSpellTE(Aow.Units.SummonSpells.TSummonSpellTE Event)
        {
            var call__35 = Aow.Rtl.Rtl.Random(FUnitIndices.Count);
            var call__36 = FUnitIndices[call__35];
            Event.FUnitIndices.Add(call__36);
        }

        // SummonSpells.pas:283
        public int GetAIUpkeepPriority()
        {
            int Result = default;
            var call__37 = FUnitIndices[0];
            var call__38 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(155);
            var call__39 = ((Aow.Units.PassiveAb.TSummonedAbility)call__38).UnitIndexToAIUpkeepPriority(call__37);
            Result = call__39;
            return Result;
        }

        public static new Aow.Units.SummonSpells.TSummonSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonSpell(context, owner));
    }

    // { TSummonSyron (instance size 60, VMT 557E3120) }
    public partial class TSummonSyron : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:328
        private protected TSummonSyron(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__40 = Aow.Units.AoWE.AoWE.SummonSyronRStr;
            var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(call__40);
            FSpellName = call__41;
            FSpellID = 24;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FUnitIndices.Add(221);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        public static new Aow.Units.SummonSpells.TSummonSyron Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonSyron(context, owner));
    }

    // { TSummonFrog (instance size 60, VMT 557E323C) }
    public partial class TSummonFrog : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:341
        private protected TSummonFrog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__42 = Aow.Units.AoWE.AoWE.SummonFrogRStr;
            var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(call__42);
            FSpellName = call__43;
            FSpellID = 22;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FUnitIndices.Add(236);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        public static new Aow.Units.SummonSpells.TSummonFrog Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonFrog(context, owner));
    }

    // { TSummonBoar (instance size 60, VMT 557E3354) }
    public partial class TSummonBoar : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:354
        private protected TSummonBoar(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__44 = Aow.Units.AoWE.AoWE.SummonBoarRStr;
            var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(call__44);
            FSpellName = call__45;
            FSpellID = 31;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FUnitIndices.Add(235);
            FCastDuration = 5;
        }

        public static new Aow.Units.SummonSpells.TSummonBoar Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonBoar(context, owner));
    }

    // { TSummonAirElemental (instance size 60, VMT 557E346C) }
    public partial class TSummonAirElemental : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:366
        private protected TSummonAirElemental(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__46 = Aow.Units.AoWE.AoWE.SummonAirElementalRStr;
            var call__47 = Aow.Units.AoWE.AoWE.TranslateRStr(call__46);
            FSpellName = call__47;
            FSpellID = 27;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
            FUnitIndices.Add(224);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        public static new Aow.Units.SummonSpells.TSummonAirElemental Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonAirElemental(context, owner));
    }

    // { TSummonEarthElemental (instance size 60, VMT 557E3594) }
    public partial class TSummonEarthElemental : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:379
        private protected TSummonEarthElemental(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__48 = Aow.Units.AoWE.AoWE.SummonEarthElementalRStr;
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(call__48);
            FSpellName = call__49;
            FSpellID = 28;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FUnitIndices.Add(225);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        public static new Aow.Units.SummonSpells.TSummonEarthElemental Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonEarthElemental(context, owner));
    }

    // { TSummonFireElemental (instance size 60, VMT 557E36C0) }
    public partial class TSummonFireElemental : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:392
        private protected TSummonFireElemental(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__50 = Aow.Units.AoWE.AoWE.SummonFireElementalRStr;
            var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(call__50);
            FSpellName = call__51;
            FSpellID = 29;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
            FUnitIndices.Add(226);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        public static new Aow.Units.SummonSpells.TSummonFireElemental Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonFireElemental(context, owner));
    }

    // { TSummonWaterElemental (instance size 60, VMT 557E37EC) }
    public partial class TSummonWaterElemental : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:405
        private protected TSummonWaterElemental(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__52 = Aow.Units.AoWE.AoWE.SummonWaterElementalRStr;
            var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(call__52);
            FSpellName = call__53;
            FSpellID = 30;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FUnitIndices.Add(227);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        public static new Aow.Units.SummonSpells.TSummonWaterElemental Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonWaterElemental(context, owner));
    }

    // { TSummonGoldDragon (instance size 60, VMT 557E3918) }
    public partial class TSummonGoldDragon : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:418
        private protected TSummonGoldDragon(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__54 = Aow.Units.AoWE.AoWE.SummonGoldDragonRStr;
            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(call__54);
            FSpellName = call__55;
            FSpellID = 25;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
            FUnitIndices.Add(216);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        public static new Aow.Units.SummonSpells.TSummonGoldDragon Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonGoldDragon(context, owner));
    }

    // { TSummonBlackDragon (instance size 60, VMT 557E3A3C) }
    public partial class TSummonBlackDragon : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:431
        private protected TSummonBlackDragon(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__56 = Aow.Units.AoWE.AoWE.SummonBlackDragonRStr;
            var call__57 = Aow.Units.AoWE.AoWE.TranslateRStr(call__56);
            FSpellName = call__57;
            FSpellID = 26;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FUnitIndices.Add(217);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        public static new Aow.Units.SummonSpells.TSummonBlackDragon Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonBlackDragon(context, owner));
    }

    // { TSummonGreatEagle (instance size 60, VMT 557E3B64) }
    public partial class TSummonGreatEagle : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:444
        private protected TSummonGreatEagle(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__58 = Aow.Units.AoWE.AoWE.SummonGreatEagleRStr;
            var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(call__58);
            FSpellName = call__59;
            FSpellID = 23;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
            FUnitIndices.Add(234);
            FCastDuration = 10;
            FAIBasePriority = 250;
        }

        public static new Aow.Units.SummonSpells.TSummonGreatEagle Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonGreatEagle(context, owner));
    }

    // { TSummonFireSprite (instance size 60, VMT 557E3C88) }
    public partial class TSummonFireSprite : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:457
        private protected TSummonFireSprite(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__60 = Aow.Units.AoWE.AoWE.SummonFireSpriteRStr;
            var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(call__60);
            FSpellName = call__61;
            FSpellID = 21;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
            FUnitIndices.Add(228);
            FCastDuration = 10;
            FAIBasePriority = 200;
        }

        public static new Aow.Units.SummonSpells.TSummonFireSprite Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonFireSprite(context, owner));
    }

    // { TSummonBlackSpider (instance size 60, VMT 557E3DAC) }
    public partial class TSummonBlackSpider : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:475
        private protected TSummonBlackSpider(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__62 = Aow.Units.AoWE.AoWE.SummonBlackSpiderRStr;
            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(call__62);
            FSpellName = call__63;
            FSpellID = 20;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FUnitIndices.Add(222);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        // SummonSpells.pas:470
        public override void SetupSummonSpellTE(Aow.Units.SummonSpells.TSummonSpellTE Event)
        {
            base.SetupSummonSpellTE(Event);
        }

        public static new Aow.Units.SummonSpells.TSummonBlackSpider Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonBlackSpider(context, owner));
    }

    // { TSummonMermaid (instance size 60, VMT 557E3ED4) }
    public partial class TSummonMermaid : Aow.Units.SummonSpells.TSummonSpell
    {
        // SummonSpells.pas:488
        private protected TSummonMermaid(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__64 = Aow.Units.AoWE.AoWE.SummonMermaidRStr;
            var call__65 = Aow.Units.AoWE.AoWE.TranslateRStr(call__64);
            FSpellName = call__65;
            FSpellID = 78;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FUnitIndices.Add(218);
            FCastDuration = 5;
            FAIBasePriority = -1;
        }

        public static new Aow.Units.SummonSpells.TSummonMermaid Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSummonMermaid(context, owner));
    }

    public static partial class SummonSpells
    {
        // SummonSpells.pas:501
        public static void RegisterSummonSpells(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.SummonSpells.TSummonSpellTE>()]);
            var call__66 = Aow.Units.SummonSpells.TSummonSyron.Create(null);
            Control.RegisterSpell(call__66);
            var call__67 = Aow.Units.SummonSpells.TSummonFrog.Create(null);
            Control.RegisterSpell(call__67);
            var call__68 = Aow.Units.SummonSpells.TSummonBoar.Create(null);
            Control.RegisterSpell(call__68);
            var call__69 = Aow.Units.SummonSpells.TSummonAirElemental.Create(null);
            Control.RegisterSpell(call__69);
            var call__70 = Aow.Units.SummonSpells.TSummonEarthElemental.Create(null);
            Control.RegisterSpell(call__70);
            var call__71 = Aow.Units.SummonSpells.TSummonFireElemental.Create(null);
            Control.RegisterSpell(call__71);
            var call__72 = Aow.Units.SummonSpells.TSummonWaterElemental.Create(null);
            Control.RegisterSpell(call__72);
            var call__73 = Aow.Units.SummonSpells.TSummonGoldDragon.Create(null);
            Control.RegisterSpell(call__73);
            var call__74 = Aow.Units.SummonSpells.TSummonBlackDragon.Create(null);
            Control.RegisterSpell(call__74);
            var call__75 = Aow.Units.SummonSpells.TSummonGreatEagle.Create(null);
            Control.RegisterSpell(call__75);
            var call__76 = Aow.Units.SummonSpells.TSummonFireSprite.Create(null);
            Control.RegisterSpell(call__76);
            var call__77 = Aow.Units.SummonSpells.TSummonBlackSpider.Create(null);
            Control.RegisterSpell(call__77);
            var call__78 = Aow.Units.SummonSpells.TSummonMermaid.Create(null);
            Control.RegisterSpell(call__78);
        }

        // SummonSpells.pas:522
        public static void UnregisterSummonSpells(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.SummonSpells.TSummonSpellTE>()]);
            Control.UnregisterSpellID(24);
            Control.UnregisterSpellID(22);
            Control.UnregisterSpellID(31);
            Control.UnregisterSpellID(27);
            Control.UnregisterSpellID(28);
            Control.UnregisterSpellID(29);
            Control.UnregisterSpellID(30);
            Control.UnregisterSpellID(25);
            Control.UnregisterSpellID(26);
            Control.UnregisterSpellID(23);
            Control.UnregisterSpellID(21);
            Control.UnregisterSpellID(20);
            Control.UnregisterSpellID(78);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
