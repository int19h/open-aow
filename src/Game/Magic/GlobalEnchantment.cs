// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic;

// (ID, references, status, mode, dispel mana, type, player, name, upkeep)
public partial class GlobalEnchantment : EObject
{

    // Spells.PAS:2471
    public GlobalEnchantment(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FEnchantmentMode = Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
    }

    public int FEnchantmentID;
    public int FRefCount;
    public Set<GlobalEnchantmentStatusElement> FEnchantmentStatus;
    public Set<GlobalEnchantmentModeElement> FEnchantmentMode;
    public int FDispelMana;
    public GlobalEnchantmentType FEnchantmentType;
    public sbyte FPlayerIndex;
    public string FName = "";
    public int FEnchantmentManaUpkeep;

    // Spells.PAS:2520
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FEnchantmentID, 10);
        Stream.rwData(Bytes.Of(ref FEnchantmentType, 1), 1, 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayerIndex), 12);
        Stream.rwString(ref FName, 13);
        Stream.rwInteger(ref FEnchantmentManaUpkeep, 14);
        Stream.rwInteger(ref FDispelMana, 15);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("TGlobalEnchantment.EnchantmentID", ref FEnchantmentID);
        persister.Enum("EnchantmentType", ref FEnchantmentType);
        PersistSupport.UnsignedByte(persister, "PlayerIndex", ref FPlayerIndex);
        persister.String("Name", ref FName);
        persister.Int("EnchantmentManaUpkeep", ref FEnchantmentManaUpkeep);
        persister.Int("DispelMana", ref FDispelMana);
    }

    // Spells.PAS:2477
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Spells.PAS:2482
    public override int Release()
    {
        int Count;
        FRefCount -= 1;
        Count = FRefCount;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Spells.PAS:2503
    public virtual string GetName() => FName;

    // Spells.PAS:2505
    public virtual int GetUpkeep() => FEnchantmentManaUpkeep;

    // Spells.PAS:2490
    public virtual XYL GetLocation()
    {
        XYL Result = default;
        Result.Packed = 16777215;
        return Result;
    }

    // Spells.PAS:2488
    public virtual int GetAIUpkeepPriority() => 0;

    // Spells.PAS:2563
    public virtual bool CanDeactivate(byte Player, ref string Reason) => true;

    // Spells.PAS:2565
    public virtual bool Deactivate(byte Player)
    {
        FEnchantmentStatus = FEnchantmentStatus - Set<GlobalEnchantmentStatusElement>.Of(GlobalEnchantmentStatusElement.Activated);
        if (FEnchantmentMode.Contains(GlobalEnchantmentModeElement.Owned))
        {
            Owner = null;
        }
        else
        {
            AoWHSMap.Current.FGlobalMagicControl.FEnchantments.RemoveChild(this);
        }

        AoWHSMap.Current.FGlobalMagicControl.Update();
        if (FEnchantmentManaUpkeep != 0)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.Update();
        }

        Release();
        return true;
    }

    // Spells.PAS:2536
    public virtual bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result;
        if (FEnchantmentType != GlobalEnchantmentType.None && AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(FEnchantmentType))
        {
            Reason = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr), [FName]);
            Result = false;
        }
        else
        {
            Reason = "";
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:2550
    public virtual bool Activate(sbyte Player)
    {
        FPlayerIndex = Player;
        FEnchantmentStatus = FEnchantmentStatus + Set<GlobalEnchantmentStatusElement>.Of(GlobalEnchantmentStatusElement.Activated);
        AoWHSMap.Current.FGlobalMagicControl.ValidateEnchantmentID(this);
        if (FEnchantmentMode.Contains(GlobalEnchantmentModeElement.Owned))
        {
            Owner = AoWHSMap.Current.FGlobalMagicControl.FEnchantments;
        }
        else
        {
            AoWHSMap.Current.FGlobalMagicControl.FEnchantments.AddChild(this);
        }

        AddRef();
        AoWHSMap.Current.FGlobalMagicControl.Update();
        if (FEnchantmentManaUpkeep != 0)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.Update();
        }

        return true;
    }

    // Spells.PAS:2575
    public virtual bool CanCast(sbyte Player, ref string Reason) => CanActivate(Player, ref Reason);

    // Spells.PAS:2577
    public virtual bool Cast(sbyte Player)
    {
        bool Result;
        string Reason = "";
        GlobalEnchantmentEventLog Log;
        if (CanCast(Player, ref Reason))
        {
            Result = Activate(Player);
            if (Result)
            {
                Log = new GlobalEnchantmentEventLog(null);
                Log.FEnchantmentPlayer = Player;
                Log.FPlayer = Player;
                Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [FName]);
                EventLog.DistributeEventLog(Log, false);
                Log.Release();
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2597
    public virtual bool CanDispel(sbyte Player, ref string Reason) => CanDeactivate((byte)Player, ref Reason);

    // Spells.PAS:2599
    public virtual bool Dispel(byte Player)
    {
        bool Result;
        string Reason = "";
        XYL Location;
        GlobalEnchantmentEventLog Log;
        if (CanDispel((sbyte)Player, ref Reason))
        {
            AddRef();
            Location = GetLocation();
            for (int I = 1, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (VisibleForPlayer((sbyte)I))
                {
                    Log = new GlobalEnchantmentEventLog(null);
                    Log.FPlayer = (sbyte)Player;
                    Log.FEnchantmentPlayer = FPlayerIndex;
                    Log.SetLocation(Location.X, Location.Y, Location.L);
                    var name = GetName();
                    Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(P4XDispelledRStr), [name]);
                    AoWHSMap.Current.FPlayers.GetPlayers(I).FEventLogbook.AddEvent(Log);
                    Log.Release();
                }
            }

            Result = Deactivate(Player);
            Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2626
    public virtual bool CanRemove(sbyte Player, ref string Reason)
    {
        bool Result;
        if (Player == FPlayerIndex)
        {
            Result = CanDeactivate((byte)FPlayerIndex, ref Reason);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2631
    public virtual bool Remove(byte Player)
    {
        bool Result;
        string Reason = "";
        XYL Location;
        GlobalEnchantmentEventLog Log;
        if (CanRemove((sbyte)Player, ref Reason))
        {
            AddRef();
            Location = GetLocation();
            for (int I = 1, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (VisibleForPlayer((sbyte)I))
                {
                    Log = new GlobalEnchantmentEventLog(null);
                    Log.FPlayer = FPlayerIndex;
                    Log.FEnchantmentPlayer = FPlayerIndex;
                    Log.SetLocation(Location.X, Location.Y, Location.L);
                    var name = GetName();
                    Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.P4XRemovedRStr), [name]);
                    AoWHSMap.Current.FPlayers.GetPlayers(I).FEventLogbook.AddEvent(Log);
                    Log.Release();
                }
            }

            Result = Deactivate((byte)FPlayerIndex);
            Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2507
    public virtual bool VisibleForPlayer(sbyte Player)
    {
        bool Result;
        XYL Location;
        if (!FEnchantmentMode.Contains(GlobalEnchantmentModeElement.TrueGlobal) && Player != FPlayerIndex)
        {
            Location = GetLocation();
            if (Location.X != -1)
            {
                Result = AoWHSMap.Current.VisibleForPlayer(Location.X, Location.Y, Location.L, Player);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:2492
    public virtual void CenterToLocation()
    {
        XYL Location;
        Location = GetLocation();
        if (Location.X != -1)
        {
            AoWHSMap.Current.CenterViewTo(Location.X, Location.Y, Location.L);
            AoWHSMap.Current.PlayCenterAnimation(Location.X, Location.Y, Location.L);
        }
    }

    // Spells.PAS:2530
    public void Setup(GlobalEnchantmentType Kind, string Name, int Upkeep)
    {
        FEnchantmentType = Kind;
        FName = Name;
        FEnchantmentManaUpkeep = Upkeep;
    }

    private static readonly string P4XDispelledRStr = "%s dispelled";

    // intf_P4_01.inc:194

    // intf_P4_01.inc:194

}
