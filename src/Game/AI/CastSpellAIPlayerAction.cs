// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI;

public partial class CastSpellAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // Spells.PAS:1543
    // state, casting heroes, idle heroes, pending hero IDs, candidate actions
    public CastSpellAIPlayerAction(ECustomNode owner) : base(owner)
    {
        CastingHeroes = new Int32List();
        IdleHeroes = new Int32List();
        PendingHeroes = new Int32List();
        CastSpellActions = new AICastSpellActionList(null);
    }

    public int State;
    public Int32List CastingHeroes;
    public Int32List IdleHeroes;
    public Int32List PendingHeroes;
    public AICastSpellActionList CastSpellActions;

    // Spells.PAS:1551
    public override void Dispose()
    {
        IdleHeroes?.Dispose();
        CastingHeroes?.Dispose();
        PendingHeroes?.Dispose();
        CastSpellActions?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1561
    public override bool Activate()
    {
        Player player;
        Hero hero;
        bool result = base.Activate();
        player = Control.Player;
        if (player.PlayerIndex != 0)
        {
            State = 0;
            for (int I = 0, last = player.Heroes.GetCount() - 1; I <= last; I++)
            {
                hero = player.Heroes.GetHero(I);
                if (hero.GetAbilityEnabled(52) && hero.Owner != null)
                {
                    if (hero.CastingSpellId == 0)
                    {
                        IdleHeroes.Add(hero.UnitId);
                    }
                    else
                    {
                        CastingHeroes.Add(hero.UnitId);
                    }
                }
            }
        }

        return result;
    }

    // Spells.PAS:1678
    public override bool Process()
    {
        bool result;
        Player player;
        player = Control.Player;
        if (player.PlayerIndex != 0)
        {
            while (State != -1 && AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && !player.GetBusy())
            {
                ProcessStates();
            }

            result = State != -1;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1581
    public void PrefetchCastSpellActions(Hero hero)
    {
        SpellList spells;
        spells = new SpellList();
        Control.Player.MagicControl.ListSpells(ref spells, Set<SpellType>.Of(SpellType.Unit, SpellType.Global));
        for (int I = 0, last = spells.GetCount() - 1; I <= last; I++)
        {
            if (hero.CastingTurns(spells.GetSpell(I)) < 8)
            {
                spells.GetSpell(I).AIPrefetchCastSpellActions(hero, CastSpellActions);
            }
        }

        spells?.Dispose();
    }

    // Spells.PAS:1593
    public int CastingHeroesMax()
    {
        Player player;
        player = Control.Player;
        return player.MagicControl.Mana / 250 + 1;
    }

    // Spells.PAS:1600
    public void ProcessStates()
    {
        Hero hero;
        bool done;
        int I;
        int index;
        switch (State)
        {
            case 0:
                if (CastingHeroes.Count != 0)
                {
                    PendingHeroes.Assign(CastingHeroes);
                    State += 1;
                }
                else
                {
                    State = 2;
                }

                break;
            case 1:
                if (PendingHeroes.Count != 0)
                {
                    done = false;
                    var item = PendingHeroes[0];
                    hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero != null && hero.Owner != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
                    {
                        if (hero.CastingSpellId != 0)
                        {
                            done = AoWHSSet.Current.SpellControl.GetSpell(hero.CastingSpellId).AIExecuteCastSpellAction(hero);
                        }

                        if (!done)
                        {
                            IdleHeroes.Add(hero.UnitId);
                            CastingHeroes.Remove(hero.UnitId);
                            hero.ExecuteCancelCasting();
                        }
                    }

                    PendingHeroes.Delete(0);
                }
                else
                {
                    State += 1;
                }

                break;
            case 2:
                var castingHeroesMax = CastingHeroesMax();
                if (CastingHeroes.Count < castingHeroesMax && IdleHeroes.Count != 0)
                {
                    PendingHeroes.Assign(IdleHeroes);
                    CastSpellActions.Clear();
                    State += 1;
                }
                else
                {
                    State = -1;
                }

                break;
            case 3:
                if (PendingHeroes.Count != 0)
                {
                    var item = PendingHeroes[0];
                    hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
                    {
                        PrefetchCastSpellActions(hero);
                    }

                    PendingHeroes.Delete(0);
                }
                else
                {
                    State += 1;
                }

                break;
            case 4:
                if (CastSpellActions.GetCount() != 0)
                {
                    CastSpellActions.SortOnPriority();
                    done = false;
                    while (!done && CastSpellActions.GetCount() != 0)
                    {
                        I = 0;
                        while (true)
                        {
                            bool condition = I < CastSpellActions.GetCount();
                            if (condition)
                            {
                                var castSpellActions = CastSpellActions.GetCastSpellActions(I);
                                var fCastSpellActionsCastSpellActions = CastSpellActions.GetCastSpellActions(0);
                                condition = (double)castSpellActions.Priority >= (double)fCastSpellActionsCastSpellActions.Priority * 0.75;
                            }

                            if (!condition)
                                break;
                            I += 1;
                        }

                        index = AoWHSMap.Current.Random(I);
                        var castSpellActions2 = CastSpellActions.GetCastSpellActions(index);
                        hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(castSpellActions2.SourceUnitId);
                        if (hero != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
                        {
                            var castSpellActions = CastSpellActions.GetCastSpellActions(index);
                            if (AoWHSSet.Current.SpellControl.GetSpell(castSpellActions.SpellId).AIExecuteCastSpellAction(hero))
                            {
                                CastingHeroes.Add(hero.UnitId);
                                IdleHeroes.Remove(hero.UnitId);
                                done = true;
                            }
                        }

                        CastSpellActions.GetCastSpellActions(index)?.Dispose();
                    }

                    if (done)
                    {
                        State = 2;
                    }
                    else
                    {
                        State = -1;
                    }
                }
                else
                {
                    State = -1;
                }

                break;
        }
    }

    // intf_P4_02.inc:1147

    // Spells.PAS:1559
    public static new int Priority => 23;

}
