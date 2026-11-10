// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands
{

    // FUnitIndices
    public partial class SummonSpellTE : SpellTE
    {
        // SummonSpells.pas:171
        public new const int ClassID = 135947;

        public UnitIndexList FUnitIndices;

        // SummonSpells.pas:165
        public override void Dispose()
        {
            FUnitIndices?.Dispose();
            base.Dispose();
        }

        // SummonSpells.pas:176
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(30, FUnitIndices.LoadFromStream, FUnitIndices.SaveToStream);
        }

        // SummonSpells.pas:159
        public SummonSpellTE(ECustomNode owner) : base(owner)
        {
            FUnitIndices = new UnitIndexList();
        }

        // SummonSpells.pas:182
        public override bool Process(AbstractUnit source, int value)
        {
            bool Done;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Unit U;
            ArmyHS Army;
            UnitEnchantment Enchantment;
            AIGroup Group;
            ImageSequenceAnimation Anim;
            base.Process(source, value);
            Done = false;
            source.GetLocation(ref X, ref Y, ref L);
            for (int I = 0, last = FUnitIndices.Count - 1; I <= last; I++)
            {
                U = new Unit(null);
                var unitIndice = FUnitIndices[I];
                U.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(unitIndice));
                Army = U.Place(X, Y, L, source.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                if (Army != null)
                {
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    if (HexGeometry.dHXtoRad(X, Y, xhx, yhx) <= 2)
                    {
                        if (U.ExpandAbility(155))
                        {
                            Enchantment = ((UnitEnchantmentAbilityData)U.GetAbilityData(155)).FEnchantment;
                            Enchantment.SetUpkeep(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FManaUpkeep);
                            Enchantment.SetSource(source.FPlayer);
                            Enchantment.FDispelResistance = (sbyte)((SummonSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).GetDispelResistance(U);
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
                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    AoWHSMap.Current.MakeVisible(X, Y, L, 3);
                }

                AoWHSMap.Current.ValidateObservation(source.FPlayer, X, Y, L, 0);
                if (AoWHSMap.Current.Visible(X, Y, L, 0))
                {
                    if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
                    {
                        Anim = new ImageSequenceAnimation(null);
                        Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                        AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
                    }

                    if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, L, 0))
                    {
                        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                    }
                }

                var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            }
            else
            {
                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    var noSummonLocationFound = AbilityRules.TranslateRStr(NoSummonLocationFoundRStr);
                    AoWHSMap.Current.ShowMessage(noSummonLocationFound);
                }
            }

            return false;
        }

        private static readonly string NoSummonLocationFoundRStr = "No nearby summon location found";

        // SummonSpells.pas:34

        // SummonSpells.pas:34

    }
}

namespace Aow.Game.Magic.Spells
{

    // FAIBasePriority, FUnitIndices
    public partial class SummonSpell : Spell
    {

        public int FAIBasePriority;
        public UnitIndexList FUnitIndices;

        // SummonSpells.pas:249
        public override void Dispose()
        {
            FUnitIndices?.Dispose();
            base.Dispose();
        }

        // SummonSpells.pas:241
        public SummonSpell(ECustomNode owner) : base(owner)
        {
            FUnitIndices = new UnitIndexList();
            FSpellType = SpellType.Global;
            FAIBasePriority = -1;
        }

        // SummonSpells.pas:288
        public override void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
        {
            bool CanCast;
            AICastSpellAction Action;
            string Reason = "";
            if (FAIBasePriority != -1)
            {
                var aiUpkeepPriority = GetAIUpkeepPriority();
                CanCast = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
                CanCast = CanCast && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
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
        public override bool AIExecuteCastSpellAction(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            var hero = (Hero)Source;
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
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            SummonSpellTE Event;
            string Reason = "";
            if (CanActivate(Source, ref Reason))
            {
                Event = new SummonSpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                SetupSummonSpellTE(Event);
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

        // SummonSpells.pas:261
        public virtual void SetupSummonSpellTE(SummonSpellTE Event)
        {
            var random = Random(FUnitIndices.Count);
            var unitIndice = FUnitIndices[random];
            Event.FUnitIndices.Add(unitIndice);
        }

        // SummonSpells.pas:283
        public int GetAIUpkeepPriority()
        {
            var unitIndice = FUnitIndices[0];
            return ((SummonedAbility)AoWHSSet.Current.FAbilityControl.GetAbility(155)).UnitIndexToAIUpkeepPriority(unitIndice);
        }

        // SummonSpells.pas:255
        public int GetDispelResistance(AbstractUnit unit) => SummonedAbility.GetDefaultDispelResistance(unit);
    }

    public partial class SummonSyron : SummonSpell
    {
        // SummonSpells.pas:328
        public SummonSyron(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonSyronRStr);
            FSpellID = 24;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Cosmos;
            FUnitIndices.Add(221);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonSyronRStr = "Summon Syron";
    }

    public partial class SummonFrog : SummonSpell
    {
        // SummonSpells.pas:341
        public SummonFrog(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonFrogRStr);
            FSpellID = 22;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Water;
            FUnitIndices.Add(236);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        private static readonly string SummonFrogRStr = "Summon Frog";
    }

    public partial class SummonBoar : SummonSpell
    {
        // SummonSpells.pas:354
        public SummonBoar(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonBoarRStr);
            FSpellID = 31;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Earth;
            FUnitIndices.Add(235);
            FCastDuration = 5;
        }

        private static readonly string SummonBoarRStr = "Summon Boar";
    }

    public partial class SummonAirElemental : SummonSpell
    {
        // SummonSpells.pas:366
        public SummonAirElemental(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonAirElementalRStr);
            FSpellID = 27;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Air;
            FUnitIndices.Add(224);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonAirElementalRStr = "Summon Air Elemental";
    }

    public partial class SummonEarthElemental : SummonSpell
    {
        // SummonSpells.pas:379
        public SummonEarthElemental(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonEarthElementalRStr);
            FSpellID = 28;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Earth;
            FUnitIndices.Add(225);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonEarthElementalRStr = "Summon Earth Elemental";
    }

    public partial class SummonFireElemental : SummonSpell
    {
        // SummonSpells.pas:392
        public SummonFireElemental(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonFireElementalRStr);
            FSpellID = 29;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Fire;
            FUnitIndices.Add(226);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonFireElementalRStr = "Summon Fire Elemental";
    }

    public partial class SummonWaterElemental : SummonSpell
    {
        // SummonSpells.pas:405
        public SummonWaterElemental(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonWaterElementalRStr);
            FSpellID = 30;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Water;
            FUnitIndices.Add(227);
            FCastDuration = 40;
            FAIBasePriority = 500;
        }

        private static readonly string SummonWaterElementalRStr = "Summon Water Elemental";
    }

    public partial class SummonGoldDragon : SummonSpell
    {
        // SummonSpells.pas:418
        public SummonGoldDragon(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonGoldDragonRStr);
            FSpellID = 25;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Life;
            FUnitIndices.Add(216);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonGoldDragonRStr = "Summon Gold Dragon";
    }

    public partial class SummonBlackDragon : SummonSpell
    {
        // SummonSpells.pas:431
        public SummonBlackDragon(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonBlackDragonRStr);
            FSpellID = 26;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Death;
            FUnitIndices.Add(217);
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        private static readonly string SummonBlackDragonRStr = "Summon Black Dragon";
    }

    public partial class SummonGreatEagle : SummonSpell
    {
        // SummonSpells.pas:444
        public SummonGreatEagle(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonGreatEagleRStr);
            FSpellID = 23;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Air;
            FUnitIndices.Add(234);
            FCastDuration = 10;
            FAIBasePriority = 250;
        }

        private static readonly string SummonGreatEagleRStr = "Summon Great Eagle";
    }

    public partial class SummonFireSprite : SummonSpell
    {
        // SummonSpells.pas:457
        public SummonFireSprite(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonFireSpriteRStr);
            FSpellID = 21;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Fire;
            FUnitIndices.Add(228);
            FCastDuration = 10;
            FAIBasePriority = 200;
        }

        private static readonly string SummonFireSpriteRStr = "Summon Fire Sprite";
    }

    public partial class SummonBlackSpider : SummonSpell
    {
        // SummonSpells.pas:475
        public SummonBlackSpider(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonBlackSpiderRStr);
            FSpellID = 20;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Death;
            FUnitIndices.Add(222);
            FCastDuration = 5;
            FAIBasePriority = 200;
        }

        // SummonSpells.pas:470
        public override void SetupSummonSpellTE(SummonSpellTE Event)
        {
            base.SetupSummonSpellTE(Event);
        }

        private static readonly string SummonBlackSpiderRStr = "Summon Black Spider";
    }

    public partial class SummonMermaid : SummonSpell
    {
        // SummonSpells.pas:488
        public SummonMermaid(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(SummonMermaidRStr);
            FSpellID = 78;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Death;
            FUnitIndices.Add(218);
            FCastDuration = 5;
            FAIBasePriority = -1;
        }

        private static readonly string SummonMermaidRStr = "Summon Mermaid";
    }
}
