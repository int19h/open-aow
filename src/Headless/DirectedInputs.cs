#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Headless;

// The directed inputs mirror equip_test_items, cast_test_global, dispel_test_global,
// end_test_global, leader_attack_turn and capture_locations_turn in playtest.py. One
// player can own more than one input. The pending set is re-armed after each day save,
// and each input runs once on that player's turn.
internal sealed class DirectedInputs(StrategicMap map, bool items, int spell, int dispel, string endEnchantment, LeaderAttackTarget? leaderAttack, int captureLocations)
{
    public List<object> ItemActions { get; } = [];
    public List<object> GlobalRequests { get; } = [];
    public List<object> DispelRequests { get; } = [];
    public List<object> EndRequests { get; } = [];
    public List<object> AttackRequests { get; } = [];
    public List<object> CaptureRequests { get; } = [];

    private int _caster = -1;
    private int _dispeller = -1;
    private int _dispellerLevel;
    private int _baseMana;
    private int _researchCost;
    private bool _pendingGlobal;
    private bool _pendingDispel;
    private bool _pendingEnd;
    private bool _dispelled;
    private bool _dispellerDead;
    private int _dispelAttempts;
    private int _castEnchantment;
    private bool _watchMana;
    private int _watchPlayer;
    private int _manaBefore;
    private int _manaDrop;
    private int _watchEnchantment;
    private int _castDay = -1;
    private bool _drawFailed;
    private bool _endQueued;
    private bool _endRemoved;
    private bool _casterDead;
    private int _endQueuedDay = -1;
    private int _endEnchantmentId = -1;
    private bool _pendingAttack;
    private bool _attackDone;
    private bool _pendingCapture;
    private bool _captureDone;
    private bool _attackerDied;
    private readonly List<(int Player, int Leader)> _livingLeaders = [];

    public object DispelRecord => dispel == 0 ? null : new
    {
        spell_id = dispel,
        player = _dispeller,
        level = _dispellerLevel,
        base_mana = _baseMana,
        research_cost = _researchCost,
        global_magic_control_offset = "0x188",
        dispelled = _dispelled,
        draw_failed = _drawFailed,
        requests = DispelRequests
    };

    public object EndRecord => endEnchantment is null ? null : new
    {
        mode = endEnchantment,
        player = _caster,
        done = _endQueued,
        removed = _endRemoved,
        enchantment_id = _endEnchantmentId,
        requests = EndRequests
    };

    public object AttackRecord => leaderAttack is not { } target ? null : new
    {
        player = target.Player,
        target_unit = target.UnitId,
        whole_army = target.WholeArmy,
        done = _attackDone,
        attacking_leader_died = _attackerDied,
        leader_deaths = LeaderDeaths(),
        requests = AttackRequests
    };

    public object CaptureRecord => captureLocations == 0 ? null : new
    {
        player = captureLocations,
        done = _captureDone,
        requests = CaptureRequests
    };

    public bool HasTurnInputs => spell != 0 || dispel != 0 || endEnchantment is not null || leaderAttack is not null || captureLocations != 0;

    public void Initial()
    {
        if (leaderAttack is not null)
        {
            for (int i = 1; i < map.Players.GetCount(); i++)
            {
                if (map.Players.GetPlayers(i).Leader is Leader hero && !hero.HeroStatus.Contains(HeroStatuses.Died))
                {
                    _livingLeaders.Add((i, hero.UnitId));
                }
            }
        }

        if (items)
        {
            EquipItems();
        }

        if (spell != 0 || dispel != 0 || endEnchantment is not null)
        {
            _caster = SelectCaster();
        }

        if (spell != 0)
        {
            _ = map.Players.GetPlayers(_caster).MagicControl.ExecuteSpellResearched(spell);
            CastGlobal();
        }

        if (dispel != 0)
        {
            if (GamePack.Current is null || GamePack.Current.SpellControl[dispel] is not Spell spellObject)
            {
                throw new InvalidOperationException("Disjunction is not registered");
            }

            _dispeller = SelectDispeller();
            _dispellerLevel = (byte)map.Players.GetPlayers(_dispeller).Leader.Level;
            _baseMana = spellObject.BaseManaCost;
            _researchCost = spellObject.ResearchCost;
            _ = map.Players.GetPlayers(_dispeller).MagicControl.ExecuteSpellResearched(dispel);
        }
    }

    public void DaySaved()
    {
        // NewTurn adds income before the dispeller acts again. The day save is the
        // last point where mana still equals the cast snapshot minus the drop.
        if (_dispelled && HasEnchantment(_watchEnchantment))
        {
            throw new InvalidOperationException($"enchantment {_watchEnchantment} is active on day {map.Day} after dispel removed it");
        }

        if (_watchMana && map.Day > _castDay)
        {
            ObserveDispel();
        }

        if (_endQueued && !_endRemoved && map.Day > _endQueuedDay)
        {
            if (HasEnchantment(_endEnchantmentId))
            {
                throw new InvalidOperationException($"enchantment {_endEnchantmentId} is still active on day {map.Day} after {endEnchantment}");
            }

            _endRemoved = true;
        }

        _pendingGlobal = spell != 0;
        _pendingDispel = dispel != 0 && !_dispelled && !_dispellerDead;
        _pendingEnd = endEnchantment is not null && !_endQueued && !_casterDead;
        _pendingAttack = leaderAttack is not null && !_attackDone;
        _pendingCapture = captureLocations != 0 && !_captureDone;
    }

    public void BeforeFrame()
    {
        int current = PlayerIdExtensions.ToOriginal(map.CurrentPlayer);
        if (_pendingGlobal && current == _caster)
        {
            _pendingGlobal = false;
            CastGlobal();
        }

        if (_pendingEnd && current == _caster)
        {
            _pendingEnd = false;
            EndEnchantment();
        }

        if (_pendingDispel && current == _dispeller)
        {
            _pendingDispel = false;
            Dispel();
        }

        if (_pendingAttack && leaderAttack is { } target && current == target.Player)
        {
            _pendingAttack = false;
            AttackWithLeader(target);
        }

        if (_pendingCapture && current == captureLocations)
        {
            _pendingCapture = false;
            CaptureLocations();
        }
    }

    public void AssertKnownResults(int turns)
    {
        if (turns <= 0)
        {
            return;
        }

        if (dispel != 0 && _dispelAttempts == 0)
        {
            throw new InvalidOperationException("dispel never cast");
        }

        if (endEnchantment is not null && !_endRemoved)
        {
            throw new InvalidOperationException(_endQueued
                ? $"end enchantment did not remove enchantment {_endEnchantmentId}"
                : "end enchantment was not accepted");
        }

        if (captureLocations != 0 && !_captureDone)
        {
            throw new InvalidOperationException("capture locations did not run");
        }
    }

    private void ObserveDispel()
    {
        _watchMana = false;
        int after = map.Players.GetPlayers(_watchPlayer).MagicControl.Mana;
        bool gone = !HasEnchantment(_watchEnchantment);
        if (gone)
        {
            _dispelled = true;
            _drawFailed = false;
        }
        else
        {
            _drawFailed = true;
        }

        DispelRequests.Add(new
        {
            observed = true,
            mana_before = _manaBefore,
            mana_after = after,
            mana_drop = _manaDrop,
            enchantment_gone = gone,
            draw_failed = !gone
        });
        int expected = _manaBefore - _manaDrop;
        if (after != expected)
        {
            throw new InvalidOperationException($"dispel mana is {after} on day {map.Day}, expected {expected} (before {_manaBefore}, drop {_manaDrop})");
        }
    }

    private void Dispel()
    {
        Player player = map.Players.GetPlayers(_dispeller);
        Leader hero = player.Leader;
        if (hero is null || hero.HeroStatus.Contains(HeroStatuses.Died))
        {
            _dispellerDead = true;
            DispelRequests.Add(new { dead = true });
            return;
        }

        if (_castEnchantment != 0 && !HasEnchantment(_castEnchantment))
        {
            _dispelled = true;
            DispelRequests.Add(new { enchantment_gone = true, enchantment_id = _castEnchantment });
            return;
        }

        GlobalEnchantment target = ForeignEnchantment(_dispeller);
        if (target is null)
        {
            DispelRequests.Add(new { waiting = true });
            return;
        }

        Spell spellObject = GamePack.Current.SpellControl[dispel];
        bool channelReady = hero.CastingSpellId == dispel && hero.CastingReady();
        int manaBefore = player.MagicControl.Mana;
        string reason = "";
        bool canActivate = spellObject.CanActivate(hero, messageText: ref reason);
        bool activateReason = reason.Length != 0;
        bool canCast = false;
        bool cast = false;
        bool busy = player.GetBusy();
        int dispelMana = -1;
        if (canActivate)
        {
            DisjunctionSpellCaster caster = new(null)
            {
                Holder = spellObject.CreationHolder(),
                CasterSpellId = dispel,
                CasterSourceId = hero.UnitId
            };
            caster.SetEnchantmentId(target.EnchantmentId);
            caster.SetDispelChance(75);
            dispelMana = caster.GetMana();
            reason = "";
            canCast = caster.CanCast(ref reason);
            if (canCast)
            {
                cast = caster.Cast();
            }

            _ = caster.Release();
        }

        if (cast)
        {
            _dispelAttempts += 1;
            _castEnchantment = target.EnchantmentId;
            _watchMana = true;
            _watchPlayer = _dispeller;
            _manaBefore = manaBefore;
            _manaDrop = dispelMana + (channelReady ? 0 : spellObject.BaseManaCost);
            _watchEnchantment = target.EnchantmentId;
            _castDay = map.Day;
        }

        DispelRequests.Add(new
        {
            enchantment_id = target.EnchantmentId,
            owner = PlayerIdExtensions.ToOriginal(target.PlayerIndex),
            chance = 75,
            mana_before = manaBefore,
            base_mana = spellObject.BaseManaCost,
            research_cost = spellObject.ResearchCost,
            can_activate = canActivate,
            activate_reason = activateReason,
            dispel_mana = dispelMana,
            can_cast = canCast,
            cast_reason = reason.Length != 0,
            player_busy = busy,
            cast,
            prepaid = cast && channelReady
        });
    }

    // MoveT.PAS:3181 MoveArmyXYL, the map UI's move order, with the selection bit of the
    // leader's slot, or the bits of every slot for the whole army. It plans the path and queues the move token, an attack when the
    // target is in reach. The input stops when the leader is dead or the target is gone.
    private void AttackWithLeader(LeaderAttackTarget target)
    {
        Player player = map.Players.GetPlayers(target.Player);
        Leader hero = player.Leader;
        if (hero is null || hero.HeroStatus.Contains(HeroStatuses.Died))
        {
            _attackDone = true;
            _attackerDied = true;
            AttackRequests.Add(new { dead = true });
            return;
        }

        if (map.UnitControl.FindUnit(target.UnitId) is not Troop unit)
        {
            _attackDone = true;
            AttackRequests.Add(new { target_gone = true });
            return;
        }

        // The army comes from the owner's army list, as the native driver finds it: the army
        // whose units hold the unit.
        if (ArmyOf(hero, owner: player) is not Army army)
        {
            _attackDone = true;
            AttackRequests.Add(new { off_map = true });
            return;
        }

        if (ArmyOf(unit, owner: map.Players.GetPlayers(unit.Player)) is not Army targetArmy)
        {
            _attackDone = true;
            AttackRequests.Add(new { target_off_map = true });
            return;
        }

        int slot = army.Units.IndexOf(hero);
        ArmyHexSprite sprite = army.GetArmyHexSprite();
        MapCoordinate here = army.Location;
        MapCoordinate there = targetArmy.Location;
        bool busy = player.GetBusy();
        int selection = target.WholeArmy ? (1 << army.Units.Count) - 1 : 1 << slot;
        bool moved = sprite is not null && slot < 8 && army.Units.Count <= 8 && !busy
            && ArmyHexSprite.MoveArmyMapCoordinate(sprite, selection: selection, there.X, there.Y, L: there.MapLevel, force: false);
        AttackRequests.Add(new
        {
            leader = hero.UnitId,
            level = (byte)hero.Level,
            slot,
            selection,
            army_units = army.Units.Count,
            from = new[] { here.X, here.Y, here.MapLevel },
            to = new[] { there.X, there.Y, there.MapLevel },
            target_player = PlayerIdExtensions.ToOriginal(targetArmy.Player),
            target_units = targetArmy.Units.Count,
            player_busy = busy,
            moved
        });
    }

    // Every leader alive on the first day whose player has since lost or whose leader has died,
    // whichever leader the directed combat killed.
    private object[] LeaderDeaths() => [.. _livingLeaders
        .Where(entry => map.Players.GetPlayers(entry.Player) is not { } player
            || player.GameOverStatus == GameOverStatus.Lost
            || player.Leader is not { } hero
            || hero.HeroStatus.Contains(HeroStatuses.Died))
        .Select(static entry => (object)new { player = entry.Player, leader = entry.Leader })];

    private static Army? ArmyOf(Troop troop, Player owner)
    {
        ArmyCollection armies = owner.Armies;
        for (int i = 0; i < armies.Count; i++)
        {
            Army army = armies.GetArmy(i);
            if (army.Units.IndexOf(troop) >= 0)
            {
                return army;
            }
        }

        return null;
    }

    private void EndEnchantment()
    {
        Player player = map.Players.GetPlayers(_caster);
        Leader hero = player.Leader;
        if (hero is null || hero.HeroStatus.Contains(HeroStatuses.Died))
        {
            _casterDead = true;
            EndRequests.Add(new { dead = true });
            return;
        }

        GlobalEnchantment enchantment = OwnedEnchantment(_caster);
        if (enchantment is null)
        {
            EndRequests.Add(new { waiting = true });
            return;
        }

        bool busy = player.GetBusy();
        bool accepted = false;
        if (!busy && endEnchantment == "remove")
        {
            accepted = map.GlobalMagicControl.RemoveEnchantment(PlayerId.FromOriginal((byte)_caster), enchantment: enchantment);
        }

        if (!busy && endEnchantment == "upkeep")
        {
            accepted = player.MagicControl.ChangePowerSettings(100);
        }

        if (accepted)
        {
            _endQueued = true;
            _endQueuedDay = map.Day;
            _endEnchantmentId = enchantment.EnchantmentId;
        }

        EndRequests.Add(new
        {
            mode = endEnchantment,
            enchantment_id = enchantment.EnchantmentId,
            player_busy = busy,
            accepted,
            mana = player.MagicControl.Mana,
            research = player.MagicControl.ResearchPercentage
        });
    }

    private int SelectCaster() => Enumerable.Range(1, count: map.Players.GetCount() - 1)
        .Where(i => map.Players.GetPlayers(i).Leader is not null)
        .OrderBy(i => (byte)map.Players.GetPlayers(i).Leader.Level).ThenBy(i => i).Last();

    private int SelectDispeller() => Enumerable.Range(1, count: map.Players.GetCount() - 1)
        .Where(i => i != _caster && map.Players.GetPlayers(i).Leader is Leader hero && !hero.HeroStatus.Contains(HeroStatuses.Died))
        .OrderBy(i => (byte)map.Players.GetPlayers(i).Leader.Level).ThenBy(i => -i).Last();

    private GlobalEnchantment ForeignEnchantment(int player)
    {
        PlayerId? id = PlayerId.FromOriginal((byte)player);
        GlobalEnchantmentList list = map.GlobalMagicControl.Enchantments;
        for (int i = 0; i < list.Count; i++)
        {
            GlobalEnchantment entry = list.GetEnchantments(i);
            if (entry.PlayerIndex != id)
            {
                return entry;
            }
        }

        return null;
    }

    private GlobalEnchantment OwnedEnchantment(int player)
    {
        PlayerId? id = PlayerId.FromOriginal((byte)player);
        GlobalEnchantmentList list = map.GlobalMagicControl.Enchantments;
        for (int i = 0; i < list.Count; i++)
        {
            GlobalEnchantment entry = list.GetEnchantments(i);
            if (entry.PlayerIndex == id)
            {
                return entry;
            }
        }

        return null;
    }

    private bool HasEnchantment(int id)
    {
        GlobalEnchantmentList list = map.GlobalMagicControl.Enchantments;
        for (int i = 0; i < list.Count; i++)
        {
            if (list.GetEnchantments(i).EnchantmentId == id)
            {
                return true;
            }
        }

        return false;
    }

    private void CastGlobal()
    {
        Player player = map.Players.GetPlayers(_caster);
        Leader? hero = player.Leader;
        if (hero is null || hero.HeroStatus.Contains(HeroStatuses.Died))
        {
            GlobalRequests.Add(new { dead = true });
            return;
        }
        int before = hero.CastingSpellId, required = hero.CastingProgress;
        string reason = "";
        bool accepted = hero.CastSpell(spell, ref reason);
        bool started = !accepted && reason.Length == 0 && before == 0 && hero.ExecuteStartCasting(spell);
        GlobalRequests.Add(new
        {
            accepted,
            started,
            reason = reason.Length != 0,
            player = _caster,
            player_busy = player.GetBusy(),
            current_player = map.CurrentPlayer,
            casting_points = (sbyte)hero.CastingPoints,
            casting_max = (sbyte)hero.GetCastingPointsMax(),
            casting_before = before,
            required_before = required,
            casting_after = hero.CastingSpellId,
            required_after = hero.CastingProgress,
            casting_mana = hero.CastingManaValue
        });
    }

    private void EquipItems()
    {
        ItemControl control = map.ItemControl;
        for (int index = 1; index < map.Players.GetCount(); index++)
        {
            Leader? hero = map.Players.GetPlayers(index).Leader;
            if (hero is null)
            {
                continue;
            }

            int? firstRing = null;
            foreach (int kind in new[] { 0, 1, 2, 3, 4, 4, 5, 6 })
            {
                Item? item = control.GenerateItem(((ItemType)kind).ToFlag(), 1, 100000);
                if (item is null)
                {
                    ItemActions.Add(new { player = index, type = kind, available = false });
                    continue;
                }
                int id = item.ItemId;
                bool placed = hero.AutoPlaceItem(item);
                bool? stored = null, reequipped = null, swapped = null, queued = null, executed = null, consumed = null;
                if (placed && kind <= 3)
                {
                    stored = hero.ExecutePlaceItem(id, position: (HeroItemPosition)6);
                    reequipped = hero.AutoPlaceItem(item);
                }
                if (placed && kind == 4)
                {
                    if (firstRing is null)
                    {
                        firstRing = id;
                    }
                    else
                    {
                        swapped = hero.ExecuteSwapItem(firstRing.Value, id);
                    }
                }
                if (placed && kind == 5)
                {
                    queued = item.Use(hero);
                    if (!queued.Value)
                    {
                        executed = item.ExecuteUse(hero);
                        consumed = control.FindItem(id) is null;
                    }
                }
                ItemActions.Add(new
                {
                    player = index,
                    type = kind,
                    item_id = id,
                    placed,
                    stored,
                    reequipped,
                    swapped,
                    use_queued = queued,
                    use_executed = executed,
                    consumed
                });
            }
        }
    }

    // Structur.PAS:2130. On the player's turn, each structure that is still a victory location
    // for that player changes owner through the structure's own SetPlayer, in structure-control
    // order. The victory check inside SetPlayer then runs when the new owner has that location.
    // The native driver calls the same method from the same turn boundary
    // (playtest.py --capture-locations).
    private void CaptureLocations()
    {
        PlayerId playerId = PlayerId.FromOriginal(captureLocations) ?? throw new InvalidOperationException("capture locations names no player");

        StructureControl control = map.StructureControl ?? throw new InvalidOperationException("the map has no structure control");
        int locationCount = 0;
        for (int index = 0; index < control.GetCount(); index++)
        {
            if (control[index] is not PlayerStructure structure || !structure.GetVictoryLocation(playerId))
            {
                continue;
            }

            int owner = PlayerIdExtensions.ToOriginal(structure.Player);
            bool heldByAlly = owner > 0
                && map.AlliedVictory
                && map.Players.GetDiplomaticRelation(structure.Player, otherPlayer: playerId) == DiplomaticRelation.Alliance;
            if (structure.Player == playerId || heldByAlly)
            {
                continue;
            }

            locationCount++;
            structure.SetPlayer(playerId);
            CaptureRequests.Add(new
            {
                structure_id = structure.StructureId,
                name = structure.GetName(),
                previous_player = owner,
                captured = structure.Player == playerId,
            });
            if (map.GameStatus == StrategicMapGameStatus.Terminated)
            {
                break;
            }
        }

        if (locationCount == 0)
        {
            throw new InvalidOperationException($"player {captureLocations} has no victory location left to capture");
        }

        _captureDone = true;
    }
}

/// <summary>
/// The leader to send and the unit whose army it attacks, as leader_target.py prints them. With
/// <see cref="WholeArmy"/> (PLAYER:UNIT:army) the leader's whole army moves, not the leader alone.
/// </summary>
internal readonly record struct LeaderAttackTarget(int Player, int UnitId, bool WholeArmy = false)
{
    public static LeaderAttackTarget Parse(string text)
    {
        string[] parts = text.Split(':');
        return parts.Length is 2 or 3
            && (parts.Length == 2 || parts[2] == "army")
            && int.TryParse(parts[0], style: System.Globalization.NumberStyles.None, provider: System.Globalization.CultureInfo.InvariantCulture, result: out int player)
            && int.TryParse(parts[1], style: System.Globalization.NumberStyles.None, provider: System.Globalization.CultureInfo.InvariantCulture, result: out int unitId)
            && player > 0
            && unitId > 0
            ? new LeaderAttackTarget(player, UnitId: unitId, WholeArmy: parts.Length == 3)
            : throw new ArgumentException("--leader-attack takes PLAYER:UNIT or PLAYER:UNIT:army");
    }
}
