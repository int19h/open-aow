// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Ai;

public partial class CastSpellAIPA : AIPlayerAction, IAIPlayerActionClass
{
    // Spells.PAS:1543
    // state, casting heroes, idle heroes, pending hero IDs, candidate actions
    public CastSpellAIPA(ECustomNode owner) : base(owner)
    {
        FCastingHeroes = new IntegerList();
        FIdleHeroes = new IntegerList();
        FPendingHeroes = new IntegerList();
        FCastSpellActions = new AICastSpellActionList(null);
    }

    public int FState;
    public IntegerList FCastingHeroes;
    public IntegerList FIdleHeroes;
    public IntegerList FPendingHeroes;
    public AICastSpellActionList FCastSpellActions;

    // Spells.PAS:1551
    public override void Dispose()
    {
        FIdleHeroes?.Dispose();
        FCastingHeroes?.Dispose();
        FPendingHeroes?.Dispose();
        FCastSpellActions?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1561
    public override bool Activate()
    {
        Player Player;
        Hero Hero;
        bool Result = base.Activate();
        Player = FControl.FPlayer;
        if (Player.FPlayerIndex != 0)
        {
            FState = 0;
            for (int I = 0, last = Player.FHeroes.GetCount() - 1; I <= last; I++)
            {
                Hero = Player.FHeroes.GetHero(I);
                if (Hero.GetAbilityEnabled(52) && Hero.Owner != null)
                {
                    if (Hero.FCastingSpellID == 0)
                    {
                        FIdleHeroes.Add(Hero.FUnitID);
                    }
                    else
                    {
                        FCastingHeroes.Add(Hero.FUnitID);
                    }
                }
            }
        }

        return Result;
    }

    // Spells.PAS:1678
    public override bool Process()
    {
        bool Result;
        Player Player;
        Player = FControl.FPlayer;
        if (Player.FPlayerIndex != 0)
        {
            while (FState != -1 && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0 && !Player.GetBusy())
            {
                ProcessStates();
            }

            Result = FState != -1;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1581
    public void PrefetchCastSpellActions(Hero Hero)
    {
        SpellList Spells;
        Spells = new SpellList();
        FControl.FPlayer.FMagicControl.ListSpells(ref Spells, Set<SpellType>.Of(SpellType.Unit, SpellType.Global));
        for (int I = 0, last = Spells.GetCount() - 1; I <= last; I++)
        {
            if (Hero.CastingTurns(Spells.GetSpell(I)) < 8)
            {
                Spells.GetSpell(I).AIPrefetchCastSpellActions(Hero, FCastSpellActions);
            }
        }

        Spells?.Dispose();
    }

    // Spells.PAS:1593
    public int CastingHeroesMax()
    {
        Player Player;
        Player = FControl.FPlayer;
        return Player.FMagicControl.FMana / 250 + 1;
    }

    // Spells.PAS:1600
    public void ProcessStates()
    {
        Hero Hero;
        bool Done;
        int I;
        int Index;
        switch (FState)
        {
            case 0:
                if (FCastingHeroes.Count != 0)
                {
                    FPendingHeroes.Assign(FCastingHeroes);
                    FState += 1;
                }
                else
                {
                    FState = 2;
                }

                break;
            case 1:
                if (FPendingHeroes.Count != 0)
                {
                    Done = false;
                    var item = FPendingHeroes[0];
                    Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(item);
                    if (Hero != null && Hero.Owner != null && (((byte)Hero.FUnitStatus.Image) & 16) == 0)
                    {
                        if (Hero.FCastingSpellID != 0)
                        {
                            Done = AoWHSSet.Current.FSpellControl.GetSpell(Hero.FCastingSpellID).AIExecuteCastSpellAction(Hero);
                        }

                        if (!Done)
                        {
                            FIdleHeroes.Add(Hero.FUnitID);
                            FCastingHeroes.Remove(Hero.FUnitID);
                            Hero.ExecuteCancelCasting();
                        }
                    }

                    FPendingHeroes.Delete(0);
                }
                else
                {
                    FState += 1;
                }

                break;
            case 2:
                var castingHeroesMax = CastingHeroesMax();
                if (FCastingHeroes.Count < castingHeroesMax && FIdleHeroes.Count != 0)
                {
                    FPendingHeroes.Assign(FIdleHeroes);
                    FCastSpellActions.Clear();
                    FState += 1;
                }
                else
                {
                    FState = -1;
                }

                break;
            case 3:
                if (FPendingHeroes.Count != 0)
                {
                    var item = FPendingHeroes[0];
                    Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(item);
                    if (Hero != null && (((byte)Hero.FUnitStatus.Image) & 16) == 0)
                    {
                        PrefetchCastSpellActions(Hero);
                    }

                    FPendingHeroes.Delete(0);
                }
                else
                {
                    FState += 1;
                }

                break;
            case 4:
                if (FCastSpellActions.GetCount() != 0)
                {
                    FCastSpellActions.SortOnPriority();
                    Done = false;
                    while (!Done && FCastSpellActions.GetCount() != 0)
                    {
                        I = 0;
                        while (true)
                        {
                            bool condition = I < FCastSpellActions.GetCount();
                            if (condition)
                            {
                                var castSpellActions = FCastSpellActions.GetCastSpellActions(I);
                                var fCastSpellActionsCastSpellActions = FCastSpellActions.GetCastSpellActions(0);
                                condition = (double)castSpellActions.FPriority >= (double)fCastSpellActionsCastSpellActions.FPriority * 0.75;
                            }

                            if (!condition)
                                break;
                            I += 1;
                        }

                        Index = AoWHSMap.Current.Random(I);
                        var castSpellActions2 = FCastSpellActions.GetCastSpellActions(Index);
                        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(castSpellActions2.FSourceUnitID);
                        if (Hero != null && (((byte)Hero.FUnitStatus.Image) & 16) == 0)
                        {
                            var castSpellActions = FCastSpellActions.GetCastSpellActions(Index);
                            if (AoWHSSet.Current.FSpellControl.GetSpell(castSpellActions.FSpellID).AIExecuteCastSpellAction(Hero))
                            {
                                FCastingHeroes.Add(Hero.FUnitID);
                                FIdleHeroes.Remove(Hero.FUnitID);
                                Done = true;
                            }
                        }

                        FCastSpellActions.GetCastSpellActions(Index)?.Dispose();
                    }

                    if (Done)
                    {
                        FState = 2;
                    }
                    else
                    {
                        FState = -1;
                    }
                }
                else
                {
                    FState = -1;
                }

                break;
        }
    }

    // intf_P4_02.inc:1147

    // Spells.PAS:1559
    public static new int Priority => 23;

}
