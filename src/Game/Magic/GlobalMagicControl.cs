// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic;

// next ID, update count, active types, enchantments, double-mana counters, changed events
public partial class GlobalMagicControl : EngineObject
{

    // Spells.PAS:2673
    public GlobalMagicControl(ECustomNode owner) : base(owner)
    {
        Enchantments = new GlobalEnchantmentList(null);
        _changedEventsStore = new();
        NextEnchantmentId = 1;
    }

    public int NextEnchantmentId;
    private int _updateCount;

    public Set<GlobalEnchantmentType> ActiveEnchantments { get; set; }
    public GlobalEnchantmentList Enchantments { get; set; }

    public int[] DoubleSphereManaCount { get; } = new int[9];

    private EventStore<GlobalMagicControl> _changedEventsStore;

    public event NotifyEvent<GlobalMagicControl> FChangedEvents
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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Enchantments, 10);
        stream.rwInteger(ref NextEnchantmentId, 11);
        stream.rwData(MemoryMarshal.AsBytes((Span<int>)DoubleSphereManaCount).Slice(0, 36), 36, 0);
        UpdateActiveEnchantments();
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Enchantments", Enchantments);
        persister.Int32("NextEnchantmentID", ref NextEnchantmentId);
        PersistSupport.Int32Array(persister, "DoubleSphereManaCount", DoubleSphereManaCount);
        UpdateActiveEnchantments();
    }

    // Spells.PAS:2690
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        Enchantments.MessageProc(ref message, sender);
    }

    // Spells.PAS:2679
    public void IncrementDoubleSphereManaCount(MagicSphere sphere)
    {
        DoubleSphereManaCount[(int)sphere] += 1;
    }

    // Spells.PAS:2681
    public void DecrementDoubleSphereManaCount(MagicSphere sphere)
    {
        DoubleSphereManaCount[(int)sphere] -= 1;
    }

    // Spells.PAS:2683
    public bool GetSphereManaDoubled(MagicSphere sphere) => DoubleSphereManaCount[(int)sphere] != 0;

    // Spells.PAS:2685
    public void OutOfMana(sbyte player, int mana)
    {
        int index;
        int I;
        GlobalEnchantment enchantment;
        PlayerMagicEventLog log;
        index = 0;
        while (index != -1 && mana > 0 && Enchantments.GetCount() != 0)
        {
            I = Enchantments.GetCount();
            // { Original may select the last enchantment of another player. }
            index = I - 1;
            while (I != 0)
            {
                I -= 1;
                if (Enchantments.GetEnchantments(I).PlayerIndex == player && Enchantments.GetEnchantments(I).GetUpkeep() < Enchantments.GetEnchantments(index).GetUpkeep())
                {
                    index = I;
                }
            }

            if (index != -1)
            {
                enchantment = Enchantments.GetEnchantments(index);
                mana -= enchantment.GetUpkeep();
                log = new PlayerMagicEventLog(null);
                log._player = player;
                log.Mode = PlayerMagicEventLog.EventMode.OutOfMana;
                log.EnchantmentName = Enchantments.GetEnchantments(index).GetName();
                EventLog.DistributeEventLog(log, false);
                log.Release();
                enchantment.AddRef();
                enchantment.Deactivate((byte)player);
                enchantment.Release();
            }
        }
    }

    // Spells.PAS:2688
    public void RemovePlayerEnchantments(sbyte player)
    {
        for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
        {
            if (player == Enchantments.GetEnchantments(I).PlayerIndex)
            {
                Enchantments.GetEnchantments(I).Remove((byte)player);
            }
        }
    }

    // Spells.PAS:2692
    public void BeginUpdate()
    {
        _updateCount += 1;
    }

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
        for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
        {
            if (player == Enchantments.GetEnchantments(I).PlayerIndex)
            {
                result += Enchantments.GetEnchantments(I).GetUpkeep();
            }
        }

        return result;
    }

    // Spells.PAS:2702
    public void UpdateActiveEnchantments()
    {
        ActiveEnchantments = Set<GlobalEnchantmentType>.Of();
        for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
        {
            var enchantments = Enchantments.GetEnchantments(I);
            ActiveEnchantments = ActiveEnchantments + Set<GlobalEnchantmentType>.Of(enchantments.EnchantmentType);
        }
    }

    // Spells.PAS:2704
    public int IndexOfId(int id)
    {
        int I;
        I = Enchantments.GetCount();
        while (I != 0)
        {
            I -= 1;
            if (id == Enchantments.GetEnchantments(I)._enchantmentId)
            {
                return I;
            }
        }

        return -1;
    }

    // Spells.PAS:2706
    public bool ValidateEnchantmentId(GlobalEnchantment enchantment)
    {
        int id;
        id = enchantment._enchantmentId;
        if (id > 0)
        {
            enchantment._enchantmentId = 0;
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

        enchantment._enchantmentId = id;
        return true;
    }

    // Spells.PAS:2708
    public ChangeGlobalMagicTokenEvent CreateChangeMagicTokenEvent() => new ChangeGlobalMagicTokenEvent(null);

    // Spells.PAS:2710
    public bool ExecuteDispelEnchantment(sbyte player, int enchantmentId)
    {
        bool result;
        int index;
        index = IndexOfId(enchantmentId);
        if (index != -1)
        {
            result = Enchantments.GetEnchantments(index).Dispel((byte)player);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2712
    public bool DispelEnchantment(sbyte player, GlobalEnchantment enchantment)
    {
        bool result;
        ChangeGlobalMagicTokenEvent eventValue;
        string reason = "";
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy() && IndexOfId(enchantment._enchantmentId) != -1)
        {
            if (enchantment.CanDispel(player, ref reason))
            {
                eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangeGlobalMagicTokenEventMode.DispelEnchantment;
                eventValue.EnchantmentId = enchantment._enchantmentId;
                eventValue.PlayerIndex = (byte)player;
                StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                eventValue.Release();
                result = true;
            }
            else
            {
                if (player == StrategicMap.Current.SeatedPlayer)
                {
                    StrategicMap.Current.ShowWarning(reason);
                }

                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2714
    public bool ExecuteRemoveEnchantment(sbyte player, int enchantmentId)
    {
        bool result;
        int index;
        index = IndexOfId(enchantmentId);
        if (index != -1)
        {
            result = Enchantments.GetEnchantments(index).Remove((byte)player);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2716
    public bool RemoveEnchantment(sbyte player, GlobalEnchantment enchantment)
    {
        bool result;
        ChangeGlobalMagicTokenEvent eventValue;
        string reason = "";
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy() && IndexOfId(enchantment._enchantmentId) != -1)
        {
            if (enchantment.CanRemove(player, ref reason))
            {
                eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangeGlobalMagicTokenEventMode.RemoveEnchantment;
                eventValue.EnchantmentId = enchantment._enchantmentId;
                eventValue.PlayerIndex = (byte)player;
                StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                eventValue.Release();
                result = true;
            }
            else
            {
                if (player == StrategicMap.Current.SeatedPlayer)
                {
                    StrategicMap.Current.ShowWarning(reason);
                }

                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // intf_P4_01.inc:258

}
