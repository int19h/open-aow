#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

public class CastSpellAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // Spells.PAS:1543
    // state, casting heroes, idle heroes, pending hero IDs, candidate actions
    public CastSpellAIPlayerAction(CustomNode owner) : base(owner)
    {
        _castingHeroes = [];
        _idleHeroes = [];
        _pendingHeroes = [];
        _castSpellActions = new AICastSpellActionList(null) { Holder = CreationHolder() };
    }

    private int _state;
    private readonly Int32List _castingHeroes;
    private readonly Int32List _idleHeroes;
    private readonly Int32List _pendingHeroes;
    private readonly AICastSpellActionList _castSpellActions;

    // Spells.PAS:1551
    public override void Dispose()
    {
        _castSpellActions?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1561
    public override bool Activate()
    {
        bool result = base.Activate();
        Player player = Control.Player;
        if (player.PlayerIndex != PlayerId.Independent)
        {
            _state = 0;
            for (int i = 0, last = player.Heroes.GetCount() - 1; i <= last; i++)
            {
                Hero hero = player.Heroes.GetHero(i);
                if (hero.GetAbilityEnabled(52) && hero.Owner is not null)
                {
                    _ = hero.CastingSpellId == 0 ? _idleHeroes.Add(hero.UnitId) : _castingHeroes.Add(hero.UnitId);
                }
            }
        }

        return result;
    }

    // Spells.PAS:1678
    public override bool Process()
    {
        Player player = Control.Player;
        if (player.PlayerIndex != PlayerId.Independent)
        {
            while (_state != -1 && StrategicMap.Current.AIExecutor.ProcessTimeLeft > 0 && !player.GetBusy())
            {
                ProcessStates();
            }

            return _state != -1;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:1581
    public void PrefetchCastSpellActions(Hero hero)
    {
        SpellList spells = new();
        Control.Player.MagicControl.ListSpells(spells, spellTypes: SpellTypes.Unit | SpellTypes.Global);
        for (int i = 0, last = spells.GetCount() - 1; i <= last; i++)
        {
            if (hero.CastingTurns(spells[i]) < 8)
            {
                spells[i].AIPrefetchCastSpellActions(hero, actions: _castSpellActions);
            }
        }
    }

    // Spells.PAS:1593
    public int CastingHeroesMax()
    {
        Player player = Control.Player;
        return (player.MagicControl.Mana / 250) + 1;
    }

    // Spells.PAS:1600
    public void ProcessStates()
    {
        Hero hero;
        bool done;
        switch (_state)
        {
            case 0:
                if (_castingHeroes.Count != 0)
                {
                    _pendingHeroes.Assign(_castingHeroes);
                    _state += 1;
                }
                else
                {
                    _state = 2;
                }

                break;
            case 1:
                if (_pendingHeroes.Count != 0)
                {
                    done = false;
                    int item = _pendingHeroes[0];
                    hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero is not null && hero.Owner is not null && (((byte)hero.UnitStatus) & 16) == 0)
                    {
                        if (hero.CastingSpellId != 0)
                        {
                            done = GamePack.Current.SpellControl[hero.CastingSpellId].AIExecuteCastSpellAction(hero);
                        }

                        if (!done)
                        {
                            _ = _idleHeroes.Add(hero.UnitId);
                            _ = _castingHeroes.Remove(hero.UnitId);
                            _ = hero.ExecuteCancelCasting();
                        }
                    }

                    _pendingHeroes.Delete(0);
                }
                else
                {
                    _state += 1;
                }

                break;
            case 2:
                int castingHeroesMax = CastingHeroesMax();
                if (_castingHeroes.Count < castingHeroesMax && _idleHeroes.Count != 0)
                {
                    _pendingHeroes.Assign(_idleHeroes);
                    _castSpellActions.Clear();
                    _state += 1;
                }
                else
                {
                    _state = -1;
                }

                break;
            case 3:
                if (_pendingHeroes.Count != 0)
                {
                    int item = _pendingHeroes[0];
                    hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero is not null && (((byte)hero.UnitStatus) & 16) == 0)
                    {
                        PrefetchCastSpellActions(hero);
                    }

                    _pendingHeroes.Delete(0);
                }
                else
                {
                    _state += 1;
                }

                break;
            case 4:
                if (_castSpellActions.GetCount() != 0)
                {
                    _castSpellActions.SortOnPriority();
                    done = false;
                    while (!done && _castSpellActions.GetCount() != 0)
                    {
                        int i = 0;
                        while (true)
                        {
                            bool condition = i < _castSpellActions.GetCount();
                            if (condition)
                            {
                                AICastSpellAction castSpellActions = _castSpellActions.GetCastSpellActions(i);
                                AICastSpellAction fCastSpellActionsCastSpellActions = _castSpellActions.GetCastSpellActions(0);
                                condition = castSpellActions.Priority >= fCastSpellActionsCastSpellActions.Priority * 0.75;
                            }

                            if (!condition)
                            {
                                break;
                            }

                            i += 1;
                        }

                        int index = StrategicMap.Current.Random(i);
                        AICastSpellAction castSpellActions2 = _castSpellActions.GetCastSpellActions(index);
                        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(castSpellActions2.SourceUnitId);
                        if (hero is not null && (((byte)hero.UnitStatus) & 16) == 0)
                        {
                            AICastSpellAction castSpellActions = _castSpellActions.GetCastSpellActions(index);
                            if (GamePack.Current.SpellControl[castSpellActions.SpellId].AIExecuteCastSpellAction(hero))
                            {
                                _ = _castingHeroes.Add(hero.UnitId);
                                _ = _idleHeroes.Remove(hero.UnitId);
                                done = true;
                            }
                        }

                        _castSpellActions.GetCastSpellActions(index)?.Dispose();
                    }

                    _state = done ? 2 : -1;
                }
                else
                {
                    _state = -1;
                }

                break;
            default:
                break;
        }
    }

    // Spells.PAS:1559
    public static new int Priority => 23;

}
