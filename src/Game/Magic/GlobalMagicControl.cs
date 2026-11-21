#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic;

// next ID, update count, active types, enchantments, double-mana counters, changed events
public sealed class GlobalMagicControl : EngineObject
{

    // Spells.PAS:2673
    public GlobalMagicControl(CustomNode? owner) : base(owner)
    {
        Enchantments = new GlobalEnchantmentList(null) { Holder = CreationHolder() };
        _changedEventsStore = new();
        NextEnchantmentId = 1;
    }

    public int NextEnchantmentId { get; set; }

    private int _updateCount;

    public GlobalEnchantmentTypes ActiveEnchantments { get; set; }
    public GlobalEnchantmentList Enchantments { get; internal set; }

    public int[] DoubleSphereManaCount { get; } = new int[9];

    private readonly EventStore<GlobalMagicControl> _changedEventsStore;

    public event NotifyEvent<GlobalMagicControl> Changed
    {
        add => _changedEventsStore.Add(value);
        remove => _changedEventsStore.Remove(value);
    }

    // Spells.PAS:2675
    public override void Dispose()
    {
        base.Dispose();
        Enchantments?.Dispose();
        _changedEventsStore.Clear();
    }

    // Spells.PAS:2677
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Enchantments", Enchantments);
        NextEnchantmentId = persister.Int32("NextEnchantmentId", NextEnchantmentId);
        persister.Int32Array(name: "DoubleSphereManaCount", values: DoubleSphereManaCount);
        UpdateActiveEnchantments();
    }

    // Spells.PAS:2690
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        Enchantments.MessageProc(ref message, sender);
    }

    // Spells.PAS:2679
    public void IncrementDoubleSphereManaCount(MagicSphere sphere) => DoubleSphereManaCount[(int)sphere] += 1;

    // Spells.PAS:2681
    public void DecrementDoubleSphereManaCount(MagicSphere sphere) => DoubleSphereManaCount[(int)sphere] -= 1;

    // Spells.PAS:2683
    public bool GetSphereManaDoubled(MagicSphere sphere) => DoubleSphereManaCount[(int)sphere] != 0;

    // Spells.PAS:2685
    public void OutOfMana(sbyte player, int mana)
    {
        int index = 0;
        while (index != -1 && mana > 0 && Enchantments.GetCount() != 0)
        {
            int i = Enchantments.GetCount();
            // { Original may select the last enchantment of another player. }
            index = i - 1;
            while (i != 0)
            {
                i -= 1;
                if (Enchantments.GetEnchantments(i).PlayerIndex == player && Enchantments.GetEnchantments(i).GetUpkeep() < Enchantments.GetEnchantments(index).GetUpkeep())
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                GlobalEnchantment enchantment = Enchantments.GetEnchantments(index);
                mana -= enchantment.GetUpkeep();
                PlayerMagicEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    Player = player,
                    Mode = PlayerMagicEventLog.EventMode.OutOfMana,
                    EnchantmentName = Enchantments.GetEnchantments(index).GetName()
                };
                EventLog.DistributeEventLog(log, execute: false);
                _ = log.Release();
                _ = enchantment.AddRef();
                _ = enchantment.Deactivate((byte)player);
                _ = enchantment.Release();
            }
        }
    }

    // Spells.PAS:2688
    public void RemovePlayerEnchantments(sbyte player)
    {
        for (int i = Enchantments.GetCount() - 1; i >= 0; i--)
        {
            if (player == Enchantments.GetEnchantments(i).PlayerIndex)
            {
                _ = Enchantments.GetEnchantments(i).Remove((byte)player);
            }
        }
    }

    // Spells.PAS:2692
    public void BeginUpdate() => _updateCount += 1;

    // Spells.PAS:2694
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            Update();
        }
    }

    // Spells.PAS:2696
    public void TriggerChangedEvent()
    {
        if (_updateCount == 0)
        {
            _changedEventsStore.Raise(this);
        }
    }

    // Spells.PAS:2698
    public void Update()
    {
        UpdateActiveEnchantments();
        _changedEventsStore.Raise(this);
    }

    // Spells.PAS:2700
    public int GetEnchantmentUpkeep(sbyte player)
    {
        int result = 0;
        for (int i = Enchantments.GetCount() - 1; i >= 0; i--)
        {
            if (player == Enchantments.GetEnchantments(i).PlayerIndex)
            {
                result += Enchantments.GetEnchantments(i).GetUpkeep();
            }
        }

        return result;
    }

    // Spells.PAS:2702
    public void UpdateActiveEnchantments()
    {
        ActiveEnchantments = GlobalEnchantmentTypes.None;
        for (int i = Enchantments.GetCount() - 1; i >= 0; i--)
        {
            GlobalEnchantment enchantments = Enchantments.GetEnchantments(i);
            ActiveEnchantments |= enchantments.EnchantmentType.ToFlag();
        }
    }

    // Spells.PAS:2704
    public int IndexOfId(int id)
    {
        int i = Enchantments.GetCount();
        while (i != 0)
        {
            i -= 1;
            if (id == Enchantments.GetEnchantments(i).EnchantmentId)
            {
                return i;
            }
        }

        return -1;
    }

    // Spells.PAS:2706
    public bool ValidateEnchantmentId(GlobalEnchantment enchantment)
    {
        int id = enchantment.EnchantmentId;
        if (id > 0)
        {
            enchantment.EnchantmentId = 0;
            if (IndexOfId(id) != -1)
            {
                id = 0;
            }
        }

        if (id <= 0)
        {
            id = NextEnchantmentId;
            NextEnchantmentId += 1;
            if (NextEnchantmentId > 268435455)
            {
                NextEnchantmentId = 1;
            }

            while (IndexOfId(id) != -1)
            {
                id = NextEnchantmentId;
                NextEnchantmentId += 1;
                if (NextEnchantmentId > 268435455)
                {
                    NextEnchantmentId = 1;
                }
            }
        }

        enchantment.EnchantmentId = id;
        return true;
    }

    // Spells.PAS:2708
    public ChangeGlobalMagicTokenEvent CreateChangeMagicTokenEvent() => new(null) { Holder = CreationHolder() };

    // Spells.PAS:2710
    public bool ExecuteDispelEnchantment(sbyte player, int enchantmentId)
    {
        int index = IndexOfId(enchantmentId);
        return index != -1 && Enchantments.GetEnchantments(index).Dispel((byte)player);
    }

    // Spells.PAS:2712
    public bool DispelEnchantment(sbyte player, GlobalEnchantment enchantment)
    {
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy() && IndexOfId(enchantment.EnchantmentId) != -1)
        {
            string reason = "";
            if (enchantment.CanDispel(player, ref reason))
            {
                ChangeGlobalMagicTokenEvent eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangeGlobalMagicTokenEventMode.DispelEnchantment;
                eventValue.EnchantmentId = enchantment.EnchantmentId;
                eventValue.PlayerIndex = (byte)player;
                StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                _ = eventValue.Release();
                return true;
            }
            else
            {
                if (player == StrategicMap.Current.SeatedPlayer)
                {
                    StrategicMap.Current.ShowWarning(reason);
                }

                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:2714
    public bool ExecuteRemoveEnchantment(sbyte player, int enchantmentId)
    {
        int index = IndexOfId(enchantmentId);
        return index != -1 && Enchantments.GetEnchantments(index).Remove((byte)player);
    }

    // Spells.PAS:2716
    public bool RemoveEnchantment(sbyte player, GlobalEnchantment? enchantment)
    {
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy() && IndexOfId(enchantment.EnchantmentId) != -1)
        {
            string reason = "";
            if (enchantment.CanRemove(player, ref reason))
            {
                ChangeGlobalMagicTokenEvent eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangeGlobalMagicTokenEventMode.RemoveEnchantment;
                eventValue.EnchantmentId = enchantment.EnchantmentId;
                eventValue.PlayerIndex = (byte)player;
                StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                _ = eventValue.Release();
                return true;
            }
            else
            {
                if (player == StrategicMap.Current.SeatedPlayer)
                {
                    StrategicMap.Current.ShowWarning(reason);
                }

                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
