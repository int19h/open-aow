// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic;

// (ID, references, status, mode, dispel mana, type, player, name, upkeep)
public partial class GlobalEnchantment : EngineObject
{

    // Spells.PAS:2471
    public GlobalEnchantment(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        EnchantmentMode = Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
    }

    public int _enchantmentId { get; set; }

    private int _refCount;

    public Set<GlobalEnchantmentStatusElement> EnchantmentStatus { get; private set; }
    public Set<GlobalEnchantmentModeElement> EnchantmentMode { get; protected set; }

    public int DispelMana { get; set; }
    public GlobalEnchantmentType EnchantmentType { get; set; }
    public sbyte PlayerIndex { get; set; }
    public string Name { get; set; } = "";
    public int EnchantmentManaUpkeep { get; set; }

    // Spells.PAS:2520
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _enchantmentId = persister.Int32("EnchantmentId", value: _enchantmentId);
        EnchantmentType = persister.Enum("EnchantmentType", value: EnchantmentType);
        PlayerIndex = PersistSupport.UnsignedByte(persister, name: "PlayerIndex", value: PlayerIndex);
        Name = persister.String("Name", value: Name);
        EnchantmentManaUpkeep = persister.Int32("EnchantmentManaUpkeep", value: EnchantmentManaUpkeep);
        DispelMana = persister.Int32("DispelMana", value: DispelMana);
    }

    // Spells.PAS:2477
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // Spells.PAS:2482
    public override int Release()
    {
        int count;
        _refCount -= 1;
        count = _refCount;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // Spells.PAS:2503
    public virtual string GetName() => Name;

    // Spells.PAS:2505
    public virtual int GetUpkeep() => EnchantmentManaUpkeep;

    // Spells.PAS:2490
    public virtual MapCoordinate GetLocation()
    {
        MapCoordinate result = default;
        result = MapCoordinate.None;
        return result;
    }

    // Spells.PAS:2488
    public virtual int GetAIUpkeepPriority() => 0;

    // Spells.PAS:2563
    public virtual bool CanDeactivate(byte player, ref string reason) => true;

    // Spells.PAS:2565
    public virtual bool Deactivate(byte player)
    {
        EnchantmentStatus = EnchantmentStatus - Set<GlobalEnchantmentStatusElement>.Of(GlobalEnchantmentStatusElement.Activated);
        if (EnchantmentMode.Contains(GlobalEnchantmentModeElement.Owned))
        {
            Owner = null;
        }
        else
        {
            StrategicMap.Current.GlobalMagicControl.Enchantments.RemoveChild(this);
        }

        StrategicMap.Current.GlobalMagicControl.Update();
        if (EnchantmentManaUpkeep != 0)
        {
            StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.Update();
        }

        Release();
        return true;
    }

    // Spells.PAS:2536
    public virtual bool CanActivate(sbyte player, ref string reason)
    {
        bool result;
        if (EnchantmentType != GlobalEnchantmentType.None && StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(EnchantmentType))
        {
            reason = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.IsAlreadyActiveFormat), [Name]);
            result = false;
        }
        else
        {
            reason = "";
            result = true;
        }

        return result;
    }

    // Spells.PAS:2550
    public virtual bool Activate(sbyte player)
    {
        PlayerIndex = player;
        EnchantmentStatus = EnchantmentStatus + Set<GlobalEnchantmentStatusElement>.Of(GlobalEnchantmentStatusElement.Activated);
        StrategicMap.Current.GlobalMagicControl.ValidateEnchantmentId(this);
        if (EnchantmentMode.Contains(GlobalEnchantmentModeElement.Owned))
        {
            Owner = StrategicMap.Current.GlobalMagicControl.Enchantments;
        }
        else
        {
            StrategicMap.Current.GlobalMagicControl.Enchantments.AddChild(this);
        }

        AddRef();
        StrategicMap.Current.GlobalMagicControl.Update();
        if (EnchantmentManaUpkeep != 0)
        {
            StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.Update();
        }

        return true;
    }

    // Spells.PAS:2575
    public virtual bool CanCast(sbyte player, ref string reason) => CanActivate(player, ref reason);

    // Spells.PAS:2577
    public virtual bool Cast(sbyte player)
    {
        bool result;
        string reason = "";
        GlobalEnchantmentEventLog log;
        if (CanCast(player, ref reason))
        {
            result = Activate(player);
            if (result)
            {
                log = new GlobalEnchantmentEventLog(null) { Holder = CreationHolder() };
                log.EnchantmentPlayer = player;
                log._player = player;
                log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [Name]);
                EventLog.DistributeEventLog(log, false);
                log.Release();
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2597
    public virtual bool CanDispel(sbyte player, ref string reason) => CanDeactivate((byte)player, ref reason);

    // Spells.PAS:2599
    public virtual bool Dispel(byte player)
    {
        bool result;
        string reason = "";
        MapCoordinate location;
        GlobalEnchantmentEventLog log;
        if (CanDispel((sbyte)player, ref reason))
        {
            AddRef();
            location = GetLocation();
            for (int I = 1, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
            {
                if (VisibleForPlayer((sbyte)I))
                {
                    log = new GlobalEnchantmentEventLog(null) { Holder = CreationHolder() };
                    log._player = (sbyte)player;
                    log.EnchantmentPlayer = PlayerIndex;
                    log.SetLocation(location.X, location.Y, location.MapLevel);
                    var name = GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_dispelledFormat), [name]);
                    StrategicMap.Current.Players.GetPlayers(I).EventLogbook.AddEvent(log);
                    log.Release();
                }
            }

            result = Deactivate(player);
            Release();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2626
    public virtual bool CanRemove(sbyte player, ref string reason)
    {
        bool result;
        if (player == PlayerIndex)
        {
            result = CanDeactivate((byte)PlayerIndex, ref reason);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2631
    public virtual bool Remove(byte player)
    {
        bool result;
        string reason = "";
        MapCoordinate location;
        GlobalEnchantmentEventLog log;
        if (CanRemove((sbyte)player, ref reason))
        {
            AddRef();
            location = GetLocation();
            for (int I = 1, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
            {
                if (VisibleForPlayer((sbyte)I))
                {
                    log = new GlobalEnchantmentEventLog(null) { Holder = CreationHolder() };
                    log._player = PlayerIndex;
                    log.EnchantmentPlayer = PlayerIndex;
                    log.SetLocation(location.X, location.Y, location.MapLevel);
                    var name = GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.RemovedFormat), [name]);
                    StrategicMap.Current.Players.GetPlayers(I).EventLogbook.AddEvent(log);
                    log.Release();
                }
            }

            result = Deactivate((byte)PlayerIndex);
            Release();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2507
    public virtual bool VisibleForPlayer(sbyte player)
    {
        bool result;
        MapCoordinate location;
        if (!EnchantmentMode.Contains(GlobalEnchantmentModeElement.TrueGlobal) && player != PlayerIndex)
        {
            location = GetLocation();
            if (location.X != -1)
            {
                result = StrategicMap.Current.VisibleForPlayer(location.X, location.Y, location.MapLevel, player);
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Spells.PAS:2492
    public virtual void CenterToLocation()
    {
        MapCoordinate location;
        location = GetLocation();
        if (location.X != -1)
        {
            StrategicMap.Current.CenterViewTo(location.X, location.Y, location.MapLevel);
            StrategicMap.Current.PlayCenterAnimation(location.X, location.Y, location.MapLevel);
        }
    }

    // Spells.PAS:2530
    public void Setup(GlobalEnchantmentType kind, string name, int upkeep)
    {
        EnchantmentType = kind;
        Name = name;
        EnchantmentManaUpkeep = upkeep;
    }

    private static readonly string s_dispelledFormat = "%s dispelled";

    // intf_P4_01.inc:194

    // intf_P4_01.inc:194

}
