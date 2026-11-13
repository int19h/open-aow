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
        _castingHeroes = new Int32List();
        _idleHeroes = new Int32List();
        _pendingHeroes = new Int32List();
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
        _idleHeroes?.Dispose();
        _castingHeroes?.Dispose();
        _pendingHeroes?.Dispose();
        _castSpellActions?.Dispose();
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
            _state = 0;
            for (int I = 0, last = player.Heroes.GetCount() - 1; I <= last; I++)
            {
                hero = player.Heroes.GetHero(I);
                if (hero.GetAbilityEnabled(52) && hero.Owner != null)
                {
                    if (hero.CastingSpellId == 0)
                    {
                        _idleHeroes.Add(hero.UnitId);
                    }
                    else
                    {
                        _castingHeroes.Add(hero.UnitId);
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
            while (_state != -1 && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && !player.GetBusy())
            {
                ProcessStates();
            }

            result = _state != -1;
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
                spells.GetSpell(I).AIPrefetchCastSpellActions(hero, _castSpellActions);
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
                    var item = _pendingHeroes[0];
                    hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero != null && hero.Owner != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
                    {
                        if (hero.CastingSpellId != 0)
                        {
                            done = GamePack.Current.SpellControl.GetSpell(hero.CastingSpellId).AIExecuteCastSpellAction(hero);
                        }

                        if (!done)
                        {
                            _idleHeroes.Add(hero.UnitId);
                            _castingHeroes.Remove(hero.UnitId);
                            hero.ExecuteCancelCasting();
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
                var castingHeroesMax = CastingHeroesMax();
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
                    var item = _pendingHeroes[0];
                    hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(item);
                    if (hero != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
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
                        I = 0;
                        while (true)
                        {
                            bool condition = I < _castSpellActions.GetCount();
                            if (condition)
                            {
                                var castSpellActions = _castSpellActions.GetCastSpellActions(I);
                                var fCastSpellActionsCastSpellActions = _castSpellActions.GetCastSpellActions(0);
                                condition = (double)castSpellActions.Priority >= (double)fCastSpellActionsCastSpellActions.Priority * 0.75;
                            }

                            if (!condition)
                                break;
                            I += 1;
                        }

                        index = StrategicMap.Current.Random(I);
                        var castSpellActions2 = _castSpellActions.GetCastSpellActions(index);
                        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(castSpellActions2.SourceUnitId);
                        if (hero != null && (((byte)hero.UnitStatus.Image) & 16) == 0)
                        {
                            var castSpellActions = _castSpellActions.GetCastSpellActions(index);
                            if (GamePack.Current.SpellControl.GetSpell(castSpellActions.SpellId).AIExecuteCastSpellAction(hero))
                            {
                                _castingHeroes.Add(hero.UnitId);
                                _idleHeroes.Remove(hero.UnitId);
                                done = true;
                            }
                        }

                        _castSpellActions.GetCastSpellActions(index)?.Dispose();
                    }

                    if (done)
                    {
                        _state = 2;
                    }
                    else
                    {
                        _state = -1;
                    }
                }
                else
                {
                    _state = -1;
                }

                break;
        }
    }

    // intf_P4_02.inc:1147

    // Spells.PAS:1559
    public static new int Priority => 23;

}
