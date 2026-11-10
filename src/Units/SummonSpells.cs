// Translated from SummonSpells.pas.
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Summon;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands
{

    // FUnitIndices
    public partial class TSummonSpellTE : TSpellTE
    {
        // SummonSpells.pas:171
        public new const int ClassID = 135947;

        public TUnitIndexList FUnitIndices;

        // SummonSpells.pas:165
        public override void Dispose()
        {
            FUnitIndices?.Dispose();
            base.Dispose();
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
        public override bool Process(TAbstractUnit source, int value)
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
            base.Process(source, value);
            Done = false;
            source.GetLocation(ref X, ref Y, ref L);
            for (int I = 0, last = FUnitIndices.Count - 1; I <= last; I++)
            {
                U = new TUnit(null);
                var unitIndice = FUnitIndices[I];
                U.SetUnitResource(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(unitIndice));
                Army = U.Place(X, Y, L, source.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                if (Army != null)
                {
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    if (Aow.Game.World.Map.HexGeometry.dHXtoRad(X, Y, xhx, yhx) <= 2)
                    {
                        if (U.ExpandAbility(155))
                        {
                            Enchantment = ((TUnitEnchantmentAbilityData)U.GetAbilityData(155)).FEnchantment;
                            Enchantment.SetUpkeep(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FManaUpkeep);
                            Enchantment.SetSource(source.FPlayer);
                            Enchantment.FDispelResistance = (sbyte)((TSummonSpell)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID)).GetDispelResistance(U);
                            Group = source.FGroup;
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
                if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MakeVisible(X, Y, L, 3);
                }

                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateObservation(source.FPlayer, X, Y, L, 0);
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Visible(X, Y, L, 0))
                {
                    if (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
                    {
                        Anim = new TImageSequenceAnimation(null);
                        Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, X, Y, L);
                    }

                    if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(X, Y, L, 0))
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                    }
                }

                var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            }
            else
            {
                if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
                {
                    var noSummonLocationFound = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Commands.TSummonSpellTE.NoSummonLocationFoundRStr);
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(noSummonLocationFound);
                }
            }

            return false;
        }

        private static readonly string NoSummonLocationFoundRStr = "No nearby summon location found";

        // SummonSpells.pas:34

        // SummonSpells.pas:34

    }
}

namespace Aow.Game.Magic.Spells.Summon
{

    // FAIBasePriority, FUnitIndices
    public partial class TSummonSpell : Aow.Game.Magic.Spells.TSpell
    {

        public int FAIBasePriority;
        public TUnitIndexList FUnitIndices;

        // SummonSpells.pas:249
        public override void Dispose()
        {
            FUnitIndices?.Dispose();
            base.Dispose();
        }

        // SummonSpells.pas:241
        public TSummonSpell(TECustomNode owner) : base(owner)
        {
            FUnitIndices = new TUnitIndexList();
            FSpellType = Aow.Game.Magic.Spells.TSpellType.stGlobal;
            FAIBasePriority = -1;
        }

        // SummonSpells.pas:288
        public override void AIPrefetchCastSpellActions(TAbstractUnit Source, TAICastSpellActionList Actions)
        {
            bool CanCast;
            TAICastSpellAction Action;
            string Reason = "";
            if (FAIBasePriority != -1)
            {
                var aiUpkeepPriority = GetAIUpkeepPriority();
                CanCast = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
                CanCast = CanCast && ((THero)Source).CanCastSpell(FSpellID, ref Reason);
                if (CanCast)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(Source, Action);
                    Action.FPriority = FAIBasePriority;
                    Action.SetOwner(Actions);
                }
            }
        }

        // SummonSpells.pas:308
        public override bool AIExecuteCastSpellAction(TAbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            var hero = (THero)Source;
            if (hero.CanCastSpell(FSpellID, ref Reason))
            {
                var aiUpkeepPriority = GetAIUpkeepPriority();
                AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
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

        // SummonSpells.pas:266
        public override bool Activate(TAbstractUnit Source)
        {
            bool Result;
            TSummonSpellTE Event;
            string Reason = "";
            if (CanActivate(Source, ref Reason))
            {
                Event = new TSummonSpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                SetupSummonSpellTE(Event);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
            return ((TSummonedAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(155)).UnitIndexToAIUpkeepPriority(unitIndice);
        }

        // SummonSpells.pas:255
        public int GetDispelResistance(TAbstractUnit unit) => TSummonedAbility.GetDefaultDispelResistance(unit);
    }

    public partial class TSummonSyron : TSummonSpell
    {
        // SummonSpells.pas:328
        public TSummonSyron(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonSyron.SummonSyronRStr);
            FSpellID = 24;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msCosmos;
            FUnitIndices.Add(221);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonSyronRStr = "Summon Syron";
    }

    public partial class TSummonFrog : TSummonSpell
    {
        // SummonSpells.pas:341
        public TSummonFrog(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonFrog.SummonFrogRStr);
            FSpellID = 22;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msWater;
            FUnitIndices.Add(236);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        private static readonly string SummonFrogRStr = "Summon Frog";
    }

    public partial class TSummonBoar : TSummonSpell
    {
        // SummonSpells.pas:354
        public TSummonBoar(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonBoar.SummonBoarRStr);
            FSpellID = 31;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msEarth;
            FUnitIndices.Add(235);
            FCastDuration = 5;
        }

        private static readonly string SummonBoarRStr = "Summon Boar";
    }

    public partial class TSummonAirElemental : TSummonSpell
    {
        // SummonSpells.pas:366
        public TSummonAirElemental(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonAirElemental.SummonAirElementalRStr);
            FSpellID = 27;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msAir;
            FUnitIndices.Add(224);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonAirElementalRStr = "Summon Air Elemental";
    }

    public partial class TSummonEarthElemental : TSummonSpell
    {
        // SummonSpells.pas:379
        public TSummonEarthElemental(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonEarthElemental.SummonEarthElementalRStr);
            FSpellID = 28;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msEarth;
            FUnitIndices.Add(225);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonEarthElementalRStr = "Summon Earth Elemental";
    }

    public partial class TSummonFireElemental : TSummonSpell
    {
        // SummonSpells.pas:392
        public TSummonFireElemental(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonFireElemental.SummonFireElementalRStr);
            FSpellID = 29;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msFire;
            FUnitIndices.Add(226);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonFireElementalRStr = "Summon Fire Elemental";
    }

    public partial class TSummonWaterElemental : TSummonSpell
    {
        // SummonSpells.pas:405
        public TSummonWaterElemental(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonWaterElemental.SummonWaterElementalRStr);
            FSpellID = 30;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msWater;
            FUnitIndices.Add(227);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonWaterElementalRStr = "Summon Water Elemental";
    }

    public partial class TSummonGoldDragon : TSummonSpell
    {
        // SummonSpells.pas:418
        public TSummonGoldDragon(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonGoldDragon.SummonGoldDragonRStr);
            FSpellID = 25;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msLife;
            FUnitIndices.Add(216);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonGoldDragonRStr = "Summon Gold Dragon";
    }

    public partial class TSummonBlackDragon : TSummonSpell
    {
        // SummonSpells.pas:431
        public TSummonBlackDragon(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonBlackDragon.SummonBlackDragonRStr);
            FSpellID = 26;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
            FUnitIndices.Add(217);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonBlackDragonRStr = "Summon Black Dragon";
    }

    public partial class TSummonGreatEagle : TSummonSpell
    {
        // SummonSpells.pas:444
        public TSummonGreatEagle(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonGreatEagle.SummonGreatEagleRStr);
            FSpellID = 23;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msAir;
            FUnitIndices.Add(234);
            FCastDuration = 10;
            FAIBasePriority = 250;
        }

        private static readonly string SummonGreatEagleRStr = "Summon Great Eagle";
    }

    public partial class TSummonFireSprite : TSummonSpell
    {
        // SummonSpells.pas:457
        public TSummonFireSprite(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonFireSprite.SummonFireSpriteRStr);
            FSpellID = 21;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msFire;
            FUnitIndices.Add(228);
            FCastDuration = 10;
            FAIBasePriority = 200;
        }

        private static readonly string SummonFireSpriteRStr = "Summon Fire Sprite";
    }

    public partial class TSummonBlackSpider : TSummonSpell
    {
        // SummonSpells.pas:475
        public TSummonBlackSpider(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonBlackSpider.SummonBlackSpiderRStr);
            FSpellID = 20;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
            FUnitIndices.Add(222);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        // SummonSpells.pas:470
        public override void SetupSummonSpellTE(TSummonSpellTE Event)
        {
            base.SetupSummonSpellTE(Event);
        }

        private static readonly string SummonBlackSpiderRStr = "Summon Black Spider";
    }

    public partial class TSummonMermaid : TSummonSpell
    {
        // SummonSpells.pas:488
        public TSummonMermaid(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Summon.TSummonMermaid.SummonMermaidRStr);
            FSpellID = 78;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
            FUnitIndices.Add(218);
            FCastDuration = 5;
            FAIBasePriority = -1;
        }

        private static readonly string SummonMermaidRStr = "Summon Mermaid";
    }
}
