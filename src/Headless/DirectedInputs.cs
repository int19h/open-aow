#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Headless;

// The directed inputs mirror equip_test_items and cast_test_global in playtest.py.
internal sealed class DirectedInputs(StrategicMap map, bool items, int spell)
{
    public List<object> ItemActions { get; } = [];
    public List<object> GlobalRequests { get; } = [];

    private int _caster = -1;
    private bool _pending;

    public void Initial()
    {
        if (items)
        {
            EquipItems();
        }

        if (spell != 0)
        {
            _caster = Enumerable.Range(1, count: map.Players.GetCount() - 1)
                .Where(i => map.Players.GetPlayers(i).Leader is not null)
                .OrderBy(i => (byte)map.Players.GetPlayers(i).Leader.Level).ThenBy(i => i).Last();
            _ = map.Players.GetPlayers(_caster).MagicControl.ExecuteSpellResearched(spell);
            CastGlobal();
        }
    }

    public void DaySaved() => _pending = spell != 0;

    public void BeforeFrame()
    {
        if (_pending && map.CurrentPlayer == _caster)
        {
            _pending = false;
            CastGlobal();
        }
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
}
