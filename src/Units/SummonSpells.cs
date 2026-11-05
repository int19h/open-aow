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
            FUnitIndices = new TUnitIndexList();
        }

        // SummonSpells.pas:182
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Done;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TUnit U;
            TArmyHS Army;
            TUnitEnchantment Enchantment;
            TAIGroup Group;
            TImageSequenceAnimation Anim;
            base.Process(A1, A2);
            Done = false;
            A1.GetLocation(ref X, ref Y, ref L);
            for (int I = 0, last = FUnitIndices.Count - 1; I <= last; I++)
            {
                U = new TUnit(null);
                var unitIndice = FUnitIndices[I];
                U.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(unitIndice));
                Army = U.Place(X, Y, L, A1.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                if (Army != null)
                {
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    if (dHXtoRad(X, Y, xhx, yhx) <= 2)
                    {
                        if (U.ExpandAbility(155))
                        {
                            Enchantment = ((TUnitEnchantmentAbilityData)U.GetAbilityData(155)).FEnchantment;
                            Enchantment.SetUpkeep(AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FManaUpkeep);
                            Enchantment.SetSource(A1.FPlayer);
                            Enchantment.FDispelResistance = (sbyte)((TSummonSpell)AoWHSSet.FSpellControl.GetSpell(FEventSpellID)).GetDispelResistance(U);
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

            if (Done)
            {
                if (A1.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    AoWHSMap.MakeVisible(X, Y, L, 3);
                }

                AoWHSMap.ValidateObservation(A1.FPlayer, X, Y, L, 0);
                if (AoWHSMap.Visible(X, Y, L, 0))
                {
                    if (AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
                    {
                        Anim = new TImageSequenceAnimation(null);
                        Anim.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                        AoWHSMap.PlaceHX(Anim, X, Y, L);
                    }

                    if (A1.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, L, 0))
                    {
                        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                    }
                }

                var spell = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(A1, spell.FBaseManaCost);
            }
            else
            {
                if (A1.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    var noSummonLocationFound = TranslateRStr(NoSummonLocationFoundRStr);
                    AoWHSMap.ShowMessage(noSummonLocationFound);
                }
            }

            return false;
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
            FUnitIndices = new TUnitIndexList();
            FSpellType = TSpellType.stGlobal;
            FAIBasePriority = -1;
        }

        // SummonSpells.pas:288
        public override void AIPrefetchCastSpellActions(TAbstractUnit A1, TAICastSpellActionList A2)
        {
            bool CanCast;
            TAICastSpellAction Action;
            string Reason = "";
            if (FAIBasePriority != -1)
            {
                var aiUpkeepPriority = GetAIUpkeepPriority();
                CanCast = AIRequestCastSpellExpense(A1, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
                CanCast = CanCast && ((THero)A1).CanCastSpell(FSpellID, ref Reason);
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
            bool Result;
            string Reason = "";
            var hero = (THero)A1;
            if (hero.CanCastSpell(FSpellID, ref Reason))
            {
                var aiUpkeepPriority = GetAIUpkeepPriority();
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
                if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
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
            bool Result;
            TSummonSpellTE Event;
            string Reason = "";
            if (CanActivate(A1, ref Reason))
            {
                Event = new TSummonSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                SetupSummonSpellTE(Event);
                AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer).AddEvent(Event);
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
            var random = Random(FUnitIndices.Count);
            var unitIndice = FUnitIndices[random];
            Event.FUnitIndices.Add(unitIndice);
        }

        // SummonSpells.pas:283
        public int GetAIUpkeepPriority()
        {
            var unitIndice = FUnitIndices[0];
            return ((TSummonedAbility)AoWHSSet.FAbilityControl.GetAbility(155)).UnitIndexToAIUpkeepPriority(unitIndice);
        }
    }

    public partial class TSummonSyron : TSummonSpell
    {
        // SummonSpells.pas:328
        public TSummonSyron(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(SummonSyronRStr);
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
            FSpellName = TranslateRStr(SummonFrogRStr);
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
            FSpellName = TranslateRStr(SummonBoarRStr);
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
            FSpellName = TranslateRStr(SummonAirElementalRStr);
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
            FSpellName = TranslateRStr(SummonEarthElementalRStr);
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
            FSpellName = TranslateRStr(SummonFireElementalRStr);
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
            FSpellName = TranslateRStr(SummonWaterElementalRStr);
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
            FSpellName = TranslateRStr(SummonGoldDragonRStr);
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
            FSpellName = TranslateRStr(SummonBlackDragonRStr);
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
            FSpellName = TranslateRStr(SummonGreatEagleRStr);
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
            FSpellName = TranslateRStr(SummonFireSpriteRStr);
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
            FSpellName = TranslateRStr(SummonBlackSpiderRStr);
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
            FSpellName = TranslateRStr(SummonMermaidRStr);
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
            TSpellControl Control;
            Control = AEngine.FGameSet.FSpellControl;
            AEngine.RegisterEClasses([EClass.Of<TSummonSpellTE>()]);
            Control.RegisterSpell(new TSummonSyron(null));
            Control.RegisterSpell(new TSummonFrog(null));
            Control.RegisterSpell(new TSummonBoar(null));
            Control.RegisterSpell(new TSummonAirElemental(null));
            Control.RegisterSpell(new TSummonEarthElemental(null));
            Control.RegisterSpell(new TSummonFireElemental(null));
            Control.RegisterSpell(new TSummonWaterElemental(null));
            Control.RegisterSpell(new TSummonGoldDragon(null));
            Control.RegisterSpell(new TSummonBlackDragon(null));
            Control.RegisterSpell(new TSummonGreatEagle(null));
            Control.RegisterSpell(new TSummonFireSprite(null));
            Control.RegisterSpell(new TSummonBlackSpider(null));
            Control.RegisterSpell(new TSummonMermaid(null));
        }

        // SummonSpells.pas:522
        public static void UnregisterSummonSpells(TAoWEngine AEngine)
        {
            TSpellControl Control;
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
