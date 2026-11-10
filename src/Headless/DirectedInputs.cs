using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Headless;

// The directed inputs mirror equip_test_items and cast_test_global in playtest.py.
internal sealed class DirectedInputs(AoWHSMap map, bool items, int spell)
{
    public List<object> ItemActions { get; } = [];
    public List<object> GlobalRequests { get; } = [];

    private int caster = -1;
    private bool pending;

    public void Initial()
    {
        if (items) EquipItems();
        if (spell != 0)
        {
            caster = Enumerable.Range(1, map.FPlayers.GetCount() - 1)
                .Where(i => map.FPlayers.GetPlayers(i).FLeader is not null)
                .OrderBy(i => map.FPlayers.GetPlayers(i).FLeader.FLevel).ThenBy(i => i).Last();
            map.FPlayers.GetPlayers(caster).FMagicControl.ExecuteSpellResearched(spell);
            CastGlobal();
        }
    }

    public void DaySaved() => pending = spell != 0;

    public void BeforeFrame()
    {
        if (pending && map.FCurrentPlayer == caster)
        {
            pending = false;
            CastGlobal();
        }
    }

    private void CastGlobal()
    {
        var player = map.FPlayers.GetPlayers(caster);
        var hero = player.FLeader;
        if (hero is null || hero.FHeroStatus.Contains(HeroStatusElement.Died))
        {
            GlobalRequests.Add(new { dead = true });
            return;
        }
        int before = hero.FCastingSpellID, required = hero.FCastingProgress;
        string reason = "";
        bool accepted = hero.CastSpell(spell, ref reason);
        bool started = !accepted && reason.Length == 0 && before == 0 && hero.ExecuteStartCasting(spell);
        GlobalRequests.Add(new
        {
            accepted,
            started,
            reason = reason.Length != 0,
            player = caster,
            player_busy = player.GetBusy(),
            current_player = map.FCurrentPlayer,
            casting_points = hero.FCastingPoints,
            casting_max = hero.GetCastingPointsMax(),
            casting_before = before,
            required_before = required,
            casting_after = hero.FCastingSpellID,
            required_after = hero.FCastingProgress,
            casting_mana = hero.FCastingMana
        });
    }

    private void EquipItems()
    {
        var control = map.FItemControl;
        for (int index = 1; index < map.FPlayers.GetCount(); index++)
        {
            var hero = map.FPlayers.GetPlayers(index).FLeader;
            if (hero is null) continue;
            int? firstRing = null;
            foreach (int kind in new[] { 0, 1, 2, 3, 4, 4, 5, 6 })
            {
                var item = control.GenerateItem(Set<ItemType>.Of((ItemType)kind), 1, 100000);
                if (item is null)
                {
                    ItemActions.Add(new { player = index, type = kind, available = false });
                    continue;
                }
                int id = item.FItemID;
                bool placed = hero.AutoPlaceItem(item);
                bool? stored = null, reequipped = null, swapped = null, queued = null, executed = null, consumed = null;
                if (placed && kind <= 3)
                {
                    stored = hero.ExecutePlaceItem(id, (HeroItemPosition)6);
                    reequipped = hero.AutoPlaceItem(item);
                }
                if (placed && kind == 4)
                {
                    if (firstRing is null) firstRing = id;
                    else swapped = hero.ExecuteSwapItem(firstRing.Value, id);
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
