// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

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
        FEnchantments = new GlobalEnchantmentList(null);
        FChangedEventsStore = new();
        FNextEnchantmentID = 1;
    }

    public int FNextEnchantmentID;
    public int FUpdateCount;
    public Set<GlobalEnchantmentType> FActiveEnchantments;
    public GlobalEnchantmentList FEnchantments;
    public Int32Array9 FDoubleSphereManaCount;

    internal EventStore<GlobalMagicControl> FChangedEventsStore;

    public event NotifyEvent<GlobalMagicControl> FChangedEvents
    {
        add => FChangedEventsStore.Add(value);
        remove => FChangedEventsStore.Remove(value);
    }

    // Spells.PAS:2675
    public override void Dispose()
    {
        base.Dispose();
        FEnchantments?.Dispose();
        FChangedEventsStore.Clear();
    }

    // Spells.PAS:2677
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FEnchantments, 10);
        Stream.rwInteger(ref FNextEnchantmentID, 11);
        Stream.rwData(MemoryMarshal.AsBytes((Span<int>)FDoubleSphereManaCount).Slice(0, 36), 36, 0);
        UpdateActiveEnchantments();
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Enchantments", FEnchantments);
        persister.Int("NextEnchantmentID", ref FNextEnchantmentID);
        PersistSupport.Int32Array(persister, "DoubleSphereManaCount", FDoubleSphereManaCount);
        UpdateActiveEnchantments();
    }

    // Spells.PAS:2690
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FEnchantments.MsgProc(ref Msg, Sender);
    }

    // Spells.PAS:2679
    public void IncDoubleSphereManaCount(MagicSphere Sphere)
    {
        FDoubleSphereManaCount[(int)Sphere] += 1;
    }

    // Spells.PAS:2681
    public void DecDoubleSphereManaCount(MagicSphere Sphere)
    {
        FDoubleSphereManaCount[(int)Sphere] -= 1;
    }

    // Spells.PAS:2683
    public bool GetSphereManaDoubled(MagicSphere Sphere) => FDoubleSphereManaCount[(int)Sphere] != 0;

    // Spells.PAS:2685
    public void OutOfMana(sbyte Player, int Mana)
    {
        int Index;
        int I;
        GlobalEnchantment Enchantment;
        PlayerMagicEventLog Log;
        Index = 0;
        while (Index != -1 && Mana > 0 && FEnchantments.GetCount() != 0)
        {
            I = FEnchantments.GetCount();
            // { Original may select the last enchantment of another player. }
            Index = I - 1;
            while (I != 0)
            {
                I -= 1;
                if (FEnchantments.GetEnchantments(I).FPlayerIndex == Player && FEnchantments.GetEnchantments(I).GetUpkeep() < FEnchantments.GetEnchantments(Index).GetUpkeep())
                {
                    Index = I;
                }
            }

            if (Index != -1)
            {
                Enchantment = FEnchantments.GetEnchantments(Index);
                Mana -= Enchantment.GetUpkeep();
                Log = new PlayerMagicEventLog(null);
                Log.FPlayer = Player;
                Log.FMode = PlayerMagicEventLog.EventMode.OutOfMana;
                Log.FEnchantmentName = FEnchantments.GetEnchantments(Index).GetName();
                EventLog.DistributeEventLog(Log, false);
                Log.Release();
                Enchantment.AddRef();
                Enchantment.Deactivate((byte)Player);
                Enchantment.Release();
            }
        }
    }

    // Spells.PAS:2688
    public void RemovePlayerEnchantments(sbyte Player)
    {
        for (int I = FEnchantments.GetCount() - 1; I >= 0; I--)
        {
            if (Player == FEnchantments.GetEnchantments(I).FPlayerIndex)
            {
                FEnchantments.GetEnchantments(I).Remove((byte)Player);
            }
        }
    }

    // Spells.PAS:2692
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Spells.PAS:2694
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Update();
        }
    }

    // Spells.PAS:2696
    public void TriggerChangedEvent()
    {
        if (FUpdateCount == 0)
        {
            FChangedEventsStore.Raise(this);
        }
    }

    // Spells.PAS:2698
    public void Update()
    {
        UpdateActiveEnchantments();
        FChangedEventsStore.Raise(this);
    }

    // Spells.PAS:2700
    public int GetEnchantmentUpkeep(sbyte Player)
    {
        int Result = 0;
        for (int I = FEnchantments.GetCount() - 1; I >= 0; I--)
        {
            if (Player == FEnchantments.GetEnchantments(I).FPlayerIndex)
            {
                Result += FEnchantments.GetEnchantments(I).GetUpkeep();
            }
        }

        return Result;
    }

    // Spells.PAS:2702
    public void UpdateActiveEnchantments()
    {
        FActiveEnchantments = Set<GlobalEnchantmentType>.Of();
        for (int I = FEnchantments.GetCount() - 1; I >= 0; I--)
        {
            var enchantments = FEnchantments.GetEnchantments(I);
            FActiveEnchantments = FActiveEnchantments + Set<GlobalEnchantmentType>.Of(enchantments.FEnchantmentType);
        }
    }

    // Spells.PAS:2704
    public int IndexOfID(int ID)
    {
        int I;
        I = FEnchantments.GetCount();
        while (I != 0)
        {
            I -= 1;
            if (ID == FEnchantments.GetEnchantments(I).FEnchantmentID)
            {
                return I;
            }
        }

        return -1;
    }

    // Spells.PAS:2706
    public bool ValidateEnchantmentID(GlobalEnchantment Enchantment)
    {
        int ID;
        ID = Enchantment.FEnchantmentID;
        if (ID > 0)
        {
            Enchantment.FEnchantmentID = 0;
            if (IndexOfID(ID) != -1)
            {
                ID = 0;
            }
        }

        if (ID <= 0)
        {
            ID = FNextEnchantmentID;
            FNextEnchantmentID += 1;
            if (FNextEnchantmentID > 268435455)
            {
                FNextEnchantmentID = 1;
            }

            while (IndexOfID(ID) != -1)
            {
                ID = FNextEnchantmentID;
                FNextEnchantmentID += 1;
                if (FNextEnchantmentID > 268435455)
                {
                    FNextEnchantmentID = 1;
                }
            }
        }

        Enchantment.FEnchantmentID = ID;
        return true;
    }

    // Spells.PAS:2708
    public ChangeGlobalMagicTokenEvent CreateChangeMagicTE() => new ChangeGlobalMagicTokenEvent(null);

    // Spells.PAS:2710
    public bool ExecuteDispelEnchantment(sbyte Player, int EnchantmentID)
    {
        bool Result;
        int Index;
        Index = IndexOfID(EnchantmentID);
        if (Index != -1)
        {
            Result = FEnchantments.GetEnchantments(Index).Dispel((byte)Player);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2712
    public bool DispelEnchantment(sbyte Player, GlobalEnchantment Enchantment)
    {
        bool Result;
        ChangeGlobalMagicTokenEvent Event;
        string Reason = "";
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy() && IndexOfID(Enchantment.FEnchantmentID) != -1)
        {
            if (Enchantment.CanDispel(Player, ref Reason))
            {
                Event = CreateChangeMagicTE();
                Event.FMode = ChangeGlobalMagicTokenEvent.Mode.DispelEnchantment;
                Event.FEnchantmentID = Enchantment.FEnchantmentID;
                Event.FPlayerIndex = (byte)Player;
                AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                if (Player == AoWHSMap.Current.FSeatedPlayer)
                {
                    AoWHSMap.Current.ShowWarning(Reason);
                }

                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2714
    public bool ExecuteRemoveEnchantment(sbyte Player, int EnchantmentID)
    {
        bool Result;
        int Index;
        Index = IndexOfID(EnchantmentID);
        if (Index != -1)
        {
            Result = FEnchantments.GetEnchantments(Index).Remove((byte)Player);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2716
    public bool RemoveEnchantment(sbyte Player, GlobalEnchantment Enchantment)
    {
        bool Result;
        ChangeGlobalMagicTokenEvent Event;
        string Reason = "";
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy() && IndexOfID(Enchantment.FEnchantmentID) != -1)
        {
            if (Enchantment.CanRemove(Player, ref Reason))
            {
                Event = CreateChangeMagicTE();
                Event.FMode = ChangeGlobalMagicTokenEvent.Mode.RemoveEnchantment;
                Event.FEnchantmentID = Enchantment.FEnchantmentID;
                Event.FPlayerIndex = (byte)Player;
                AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                if (Player == AoWHSMap.Current.FSeatedPlayer)
                {
                    AoWHSMap.Current.ShowWarning(Reason);
                }

                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // intf_P4_01.inc:258

}
