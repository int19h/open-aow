// Translated from SummonSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.PassiveAb;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.SummonSpells
{
    // FUnitIndices
    public partial class TSummonSpellTE : TSpellTE
    {
        // SummonSpells.pas:171
        public new const int ClassID = 135947;

        public TUnitIndexList FUnitIndices;
        // SummonSpells.pas:165
        public override void Destroy()
        {
            FUnitIndices.Free();
            base.Destroy();
        }

        // SummonSpells.pas:176
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(30, FUnitIndices.LoadFromStream, FUnitIndices.SaveToStream);
        }

        // SummonSpells.pas:159
        public TSummonSpellTE(TECustomNode owner) : base(owner)
        {
            var call__1 = new TUnitIndexList();
            FUnitIndices = call__1;
        }

        // SummonSpells.pas:182
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            bool Done = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            int I = default;
            TUnit U = default;
            TArmyHS Army = default;
            TUnitEnchantment Enchantment = default;
            TAIGroup Group = default;
            TImageSequenceAnimation Anim = default;
            base.Process(A1, A2);
            Done = false;
            A1.GetLocation(ref X, ref Y, ref L);
            {
                I = 0;
                int end__2 = FUnitIndices.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    U = new TUnit(null);
                    var call__3 = FUnitIndices[I];
                    var call__4 = AoWHSSet.FUnitResources.GetUnitResource(call__3);
                    U.SetUnitResource(call__4);
                    var call__5 = U.Place(X, Y, L, A1.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                    Army = call__5;
                    if (Army != null)
                    {
                        var call__6 = Army.GetYhx();
                        var call__7 = Army.GetXhx();
                        var call__8 = dHXtoRad(X, Y, call__7, call__6);
                        if (call__8 <= 2)
                        {
                            if (U.ExpandAbility(155))
                            {
                                var call__9 = U.GetAbilityData(155);
                                Enchantment = ((TUnitEnchantmentAbilityData)call__9).FEnchantment;
                                var call__10 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                Enchantment.SetUpkeep(call__10.FManaUpkeep);
                                Enchantment.SetSource(A1.FPlayer);
                                var call__11 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__12 = ((TSummonSpell)call__11).GetDispelResistance(U);
                                Enchantment.FDispelResistance = (sbyte)call__12;
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
                if (A1.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    AoWHSMap.MakeVisible(X, Y, L, 3);
                }

                AoWHSMap.ValidateObservation(A1.FPlayer, X, Y, L, 0);
                var call__13 = AoWHSMap.Visible(X, Y, L, 0);
                if (call__13)
                {
                    var call__14 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    var call__15 = call__14.FCastImages.ImageSequences[0];
                    if (call__15 != null)
                    {
                        Anim = new TImageSequenceAnimation(null);
                        var call__16 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                        var call__17 = call__16.FCastImages.ImageSequences[0];
                        Anim.SetImageSequence(call__17);
                        AoWHSMap.PlaceHX(Anim, X, Y, L);
                    }

                    bool bool__18 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
                    if (!bool__18)
                    {
                        var call__19 = AoWHSMap.Watching(X, Y, L, 0);
                        bool__18 = call__19;
                    }

                    if (bool__18)
                    {
                        var call__20 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                        call__20.FCastSounds.Play(0);
                    }
                }

                var call__21 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__22 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__22.CastingDone(A1, call__21.FBaseManaCost);
            }
            else
            {
                if (A1.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    var call__23 = NoSummonLocationFoundRStr;
                    var call__24 = TranslateRStr(call__23);
                    AoWHSMap.ShowMessage(call__24);
                }
            }

            Result = false;
            return Result;
        }

        // SummonSpells.pas:34


    }

    // FAIBasePriority, FUnitIndices
    public partial class TSummonSpell : TSpell
    {

        public int FAIBasePriority;
        public TUnitIndexList FUnitIndices;
        // SummonSpells.pas:249
        public override void Destroy()
        {
            FUnitIndices.Free();
            base.Destroy();
        }

        // SummonSpells.pas:241
        public TSummonSpell(TECustomNode owner) : base(owner)
        {
            var call__25 = new TUnitIndexList();
            FUnitIndices = call__25;
            FSpellType = TSpellType.stGlobal;
            FAIBasePriority = -1;
        }

        // SummonSpells.pas:288
        public override void AIPrefetchCastSpellActions(TAbstractUnit A1, TAICastSpellActionList A2)
        {
            bool CanCast = default;
            TAICastSpellAction Action = default;
            AnsiString Reason = default;
            if (FAIBasePriority != -1)
            {
                var call__26 = GetAIUpkeepPriority();
                var call__27 = AIRequestCastSpellExpense(A1, FBaseManaCost, FManaUpkeep, call__26);
                CanCast = call__27;
                bool bool__28 = CanCast;
                if (bool__28)
                {
                    var call__29 = ((THero)A1).CanCastSpell(FSpellID, ref Reason);
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
        public override bool AIExecuteCastSpellAction(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            var hero = (THero)A1;
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
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            TSummonSpellTE Event = default;
            AnsiString Reason = default;
            if (CanActivate(A1, ref Reason))
            {
                Event = new TSummonSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                SetupSummonSpellTE(Event);
                var call__34 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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
        public virtual void SetupSummonSpellTE(TSummonSpellTE Event)
        {
            var call__35 = Random(FUnitIndices.Count);
            var call__36 = FUnitIndices[call__35];
            Event.FUnitIndices.Add(call__36);
        }

        // SummonSpells.pas:283
        public int GetAIUpkeepPriority()
        {
            int Result = default;
            var call__37 = FUnitIndices[0];
            var call__38 = AoWHSSet.FAbilityControl.GetAbility(155);
            var call__39 = ((TSummonedAbility)call__38).UnitIndexToAIUpkeepPriority(call__37);
            Result = call__39;
            return Result;
        }
    }

    public partial class TSummonSyron : TSummonSpell
    {
        // SummonSpells.pas:328
        public TSummonSyron(TECustomNode owner) : base(owner)
        {
            var call__40 = SummonSyronRStr;
            var call__41 = TranslateRStr(call__40);
            FSpellName = call__41;
            FSpellID = 24;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msCosmos;
            FUnitIndices.Add(221);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }
    }

    public partial class TSummonFrog : TSummonSpell
    {
        // SummonSpells.pas:341
        public TSummonFrog(TECustomNode owner) : base(owner)
        {
            var call__42 = SummonFrogRStr;
            var call__43 = TranslateRStr(call__42);
            FSpellName = call__43;
            FSpellID = 22;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msWater;
            FUnitIndices.Add(236);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }
    }

    public partial class TSummonBoar : TSummonSpell
    {
        // SummonSpells.pas:354
        public TSummonBoar(TECustomNode owner) : base(owner)
        {
            var call__44 = SummonBoarRStr;
            var call__45 = TranslateRStr(call__44);
            FSpellName = call__45;
            FSpellID = 31;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msEarth;
            FUnitIndices.Add(235);
            FCastDuration = 5;
        }
    }

    public partial class TSummonAirElemental : TSummonSpell
    {
        // SummonSpells.pas:366
        public TSummonAirElemental(TECustomNode owner) : base(owner)
        {
            var call__46 = SummonAirElementalRStr;
            var call__47 = TranslateRStr(call__46);
            FSpellName = call__47;
            FSpellID = 27;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msAir;
            FUnitIndices.Add(224);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }
    }

    public partial class TSummonEarthElemental : TSummonSpell
    {
        // SummonSpells.pas:379
        public TSummonEarthElemental(TECustomNode owner) : base(owner)
        {
            var call__48 = SummonEarthElementalRStr;
            var call__49 = TranslateRStr(call__48);
            FSpellName = call__49;
            FSpellID = 28;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msEarth;
            FUnitIndices.Add(225);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }
    }

    public partial class TSummonFireElemental : TSummonSpell
    {
        // SummonSpells.pas:392
        public TSummonFireElemental(TECustomNode owner) : base(owner)
        {
            var call__50 = SummonFireElementalRStr;
            var call__51 = TranslateRStr(call__50);
            FSpellName = call__51;
            FSpellID = 29;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msFire;
            FUnitIndices.Add(226);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }
    }

    public partial class TSummonWaterElemental : TSummonSpell
    {
        // SummonSpells.pas:405
        public TSummonWaterElemental(TECustomNode owner) : base(owner)
        {
            var call__52 = SummonWaterElementalRStr;
            var call__53 = TranslateRStr(call__52);
            FSpellName = call__53;
            FSpellID = 30;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msWater;
            FUnitIndices.Add(227);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }
    }

    public partial class TSummonGoldDragon : TSummonSpell
    {
        // SummonSpells.pas:418
        public TSummonGoldDragon(TECustomNode owner) : base(owner)
        {
            var call__54 = SummonGoldDragonRStr;
            var call__55 = TranslateRStr(call__54);
            FSpellName = call__55;
            FSpellID = 25;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msLife;
            FUnitIndices.Add(216);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }
    }

    public partial class TSummonBlackDragon : TSummonSpell
    {
        // SummonSpells.pas:431
        public TSummonBlackDragon(TECustomNode owner) : base(owner)
        {
            var call__56 = SummonBlackDragonRStr;
            var call__57 = TranslateRStr(call__56);
            FSpellName = call__57;
            FSpellID = 26;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msDeath;
            FUnitIndices.Add(217);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }
    }

    public partial class TSummonGreatEagle : TSummonSpell
    {
        // SummonSpells.pas:444
        public TSummonGreatEagle(TECustomNode owner) : base(owner)
        {
            var call__58 = SummonGreatEagleRStr;
            var call__59 = TranslateRStr(call__58);
            FSpellName = call__59;
            FSpellID = 23;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msAir;
            FUnitIndices.Add(234);
            FCastDuration = 10;
            FAIBasePriority = 250;
        }
    }

    public partial class TSummonFireSprite : TSummonSpell
    {
        // SummonSpells.pas:457
        public TSummonFireSprite(TECustomNode owner) : base(owner)
        {
            var call__60 = SummonFireSpriteRStr;
            var call__61 = TranslateRStr(call__60);
            FSpellName = call__61;
            FSpellID = 21;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msFire;
            FUnitIndices.Add(228);
            FCastDuration = 10;
            FAIBasePriority = 200;
        }
    }

    public partial class TSummonBlackSpider : TSummonSpell
    {
        // SummonSpells.pas:475
        public TSummonBlackSpider(TECustomNode owner) : base(owner)
        {
            var call__62 = SummonBlackSpiderRStr;
            var call__63 = TranslateRStr(call__62);
            FSpellName = call__63;
            FSpellID = 20;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msDeath;
            FUnitIndices.Add(222);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        // SummonSpells.pas:470
        public override void SetupSummonSpellTE(TSummonSpellTE Event)
        {
            base.SetupSummonSpellTE(Event);
        }
    }

    public partial class TSummonMermaid : TSummonSpell
    {
        // SummonSpells.pas:488
        public TSummonMermaid(TECustomNode owner) : base(owner)
        {
            var call__64 = SummonMermaidRStr;
            var call__65 = TranslateRStr(call__64);
            FSpellName = call__65;
            FSpellID = 78;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msDeath;
            FUnitIndices.Add(218);
            FCastDuration = 5;
            FAIBasePriority = -1;
        }
    }

    public static partial class SummonSpells
    {
        // SummonSpells.pas:501
        public static void RegisterSummonSpells(TAoWEngine AEngine)
        {
            TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([EClass.Of<TSummonSpellTE>()]);
            var call__66 = new TSummonSyron(null);
            Control.RegisterSpell(call__66);
            var call__67 = new TSummonFrog(null);
            Control.RegisterSpell(call__67);
            var call__68 = new TSummonBoar(null);
            Control.RegisterSpell(call__68);
            var call__69 = new TSummonAirElemental(null);
            Control.RegisterSpell(call__69);
            var call__70 = new TSummonEarthElemental(null);
            Control.RegisterSpell(call__70);
            var call__71 = new TSummonFireElemental(null);
            Control.RegisterSpell(call__71);
            var call__72 = new TSummonWaterElemental(null);
            Control.RegisterSpell(call__72);
            var call__73 = new TSummonGoldDragon(null);
            Control.RegisterSpell(call__73);
            var call__74 = new TSummonBlackDragon(null);
            Control.RegisterSpell(call__74);
            var call__75 = new TSummonGreatEagle(null);
            Control.RegisterSpell(call__75);
            var call__76 = new TSummonFireSprite(null);
            Control.RegisterSpell(call__76);
            var call__77 = new TSummonBlackSpider(null);
            Control.RegisterSpell(call__77);
            var call__78 = new TSummonMermaid(null);
            Control.RegisterSpell(call__78);
        }

        // SummonSpells.pas:522
        public static void UnregisterSummonSpells(TAoWEngine AEngine)
        {
            TSpellControl Control = default;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.UnregisterEClasses([EClass.Of<TSummonSpellTE>()]);
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
