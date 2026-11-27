#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Magic;

// (ID, references, status, mode, dispel mana, type, player, name, upkeep)
public class GlobalEnchantment(CustomNode? owner) : EngineObject(owner)
{
    public int EnchantmentId { get; set; }

    private int _refCount = 1;

    public GlobalEnchantmentStatuses EnchantmentStatus { get; private set; }
    public GlobalEnchantmentModes EnchantmentMode { get; protected init; } = GlobalEnchantmentModes.TrueGlobal | GlobalEnchantmentModes.Owned;

    public int DispelMana { get; set; }
    public GlobalEnchantmentType EnchantmentType { get; set; }
    public PlayerId? PlayerIndex { get; set; } = PlayerId.Independent;
    public string Name { get; set; } = "";
    public int EnchantmentManaUpkeep { get; set; }

    // Spells.PAS:2520
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentId = persister.Int32("EnchantmentId", value: EnchantmentId);
        EnchantmentType = persister.Enum("EnchantmentType", value: EnchantmentType);
        PlayerIndex = persister.OptionalPlayer(name: "PlayerIndex", value: PlayerIndex, width: IdWidth.UnsignedByte, fallback: 0);
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
        _refCount -= 1;
        int count = _refCount;
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
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Spells.PAS:2488
    public virtual int GetAIUpkeepPriority() => 0;

    // Spells.PAS:2563
    public virtual bool CanDeactivate(PlayerId? player, ref string reason) => true;

    // Spells.PAS:2565
    public virtual bool Deactivate(PlayerId? player)
    {
        EnchantmentStatus &= ~GlobalEnchantmentStatuses.Activated;
        if (EnchantmentMode.Contains(GlobalEnchantmentModes.Owned))
        {
            Owner = null;
        }
        else
        {
            _ = GetEngine().StrategicMap.GlobalMagicControl.Enchantments.RemoveChild(this);
        }

        GetEngine().StrategicMap.GlobalMagicControl.Update();
        if (EnchantmentManaUpkeep != 0)
        {
            GetEngine().StrategicMap.Players.GetPlayers(PlayerIndex).MagicControl.Update();
        }

        _ = Release();
        return true;
    }

    // Spells.PAS:2536
    public virtual bool CanActivate(PlayerId? player, ref string reason)
    {
        if (EnchantmentType != GlobalEnchantmentType.None && GetEngine().StrategicMap.GlobalMagicControl.ActiveEnchantments.Contains(EnchantmentType.ToFlag()))
        {
            reason = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat), arguments: [Name]);
            return false;
        }
        else
        {
            reason = "";
            return true;
        }
    }

    // Spells.PAS:2550
    public virtual bool Activate(PlayerId? player)
    {
        PlayerIndex = player;
        EnchantmentStatus |= GlobalEnchantmentStatuses.Activated;
        _ = GetEngine().StrategicMap.GlobalMagicControl.ValidateEnchantmentId(this);
        if (EnchantmentMode.Contains(GlobalEnchantmentModes.Owned))
        {
            Owner = GetEngine().StrategicMap.GlobalMagicControl.Enchantments;
        }
        else
        {
            _ = GetEngine().StrategicMap.GlobalMagicControl.Enchantments.AddChild(this);
        }

        _ = AddRef();
        GetEngine().StrategicMap.GlobalMagicControl.Update();
        if (EnchantmentManaUpkeep != 0)
        {
            GetEngine().StrategicMap.Players.GetPlayers(PlayerIndex).MagicControl.Update();
        }

        return true;
    }

    // Spells.PAS:2575
    public virtual bool CanCast(PlayerId? player, ref string reason) => CanActivate(player, ref reason);

    // Spells.PAS:2577
    public virtual bool Cast(PlayerId? player)
    {
        bool result;
        string reason = "";
        if (CanCast(player, ref reason))
        {
            result = Activate(player);
            if (result)
            {
                GlobalEnchantmentEventLog log = new(null, holder: CreationHolder())
                {
                    EnchantmentPlayer = player,
                    Player = player,
                    Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), arguments: [Name])
                };
                EventLog.DistributeEventLog(log, execute: false);
                _ = log.Release();
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Spells.PAS:2597
    public virtual bool CanDispel(PlayerId? player, ref string reason) => CanDeactivate(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player)), ref reason);

    // Spells.PAS:2599
    public virtual bool Dispel(PlayerId? player)
    {
        bool result;
        string reason = "";
        if (CanDispel(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player)), ref reason))
        {
            _ = AddRef();
            MapCoordinate location = GetLocation();
            for (int i = 1, last = GetEngine().StrategicMap.Players.GetCount() - 1; i <= last; i++)
            {
                if (VisibleForPlayer(PlayerId.FromOriginal((sbyte)i)))
                {
                    GlobalEnchantmentEventLog log = new(null, holder: CreationHolder())
                    {
                        Player = PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player)),
                        EnchantmentPlayer = PlayerIndex
                    };
                    log.SetLocation(location);
                    string name = GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_dispelledFormat), arguments: [name]);
                    GetEngine().StrategicMap.Players.GetPlayers(i).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }
            }

            result = Deactivate(player);
            _ = Release();
        }
        else
        {
            return false;
        }

        return result;
    }

    // Spells.PAS:2626
    public virtual bool CanRemove(PlayerId? player, ref string reason) => player == PlayerIndex && CanDeactivate(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(PlayerIndex)), ref reason);

    // Spells.PAS:2631
    public virtual bool Remove(PlayerId? player)
    {
        bool result;
        string reason = "";
        if (CanRemove(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player)), ref reason))
        {
            _ = AddRef();
            MapCoordinate location = GetLocation();
            for (int i = 1, last = GetEngine().StrategicMap.Players.GetCount() - 1; i <= last; i++)
            {
                if (VisibleForPlayer(PlayerId.FromOriginal((sbyte)i)))
                {
                    GlobalEnchantmentEventLog log = new(null, holder: CreationHolder())
                    {
                        Player = PlayerIndex,
                        EnchantmentPlayer = PlayerIndex
                    };
                    log.SetLocation(location);
                    string name = GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.RemovedFormat), arguments: [name]);
                    GetEngine().StrategicMap.Players.GetPlayers(i).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }
            }

            result = Deactivate(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(PlayerIndex)));
            _ = Release();
        }
        else
        {
            return false;
        }

        return result;
    }

    // Spells.PAS:2507
    public virtual bool VisibleForPlayer(PlayerId? player)
    {
        if (!EnchantmentMode.Contains(GlobalEnchantmentModes.TrueGlobal) && player != PlayerIndex)
        {
            MapCoordinate location = GetLocation();
            return location.X != -1 && GetEngine().StrategicMap.VisibleForPlayer(location.X, location.Y, location.MapLevel, player);
        }
        else
        {
            return true;
        }
    }

    // Spells.PAS:2492
    public virtual void CenterToLocation()
    {
        MapCoordinate location = GetLocation();
        if (location.X != -1)
        {
            GetEngine().StrategicMap.CenterViewTo(location.X, location.Y, location.MapLevel);
            GetEngine().StrategicMap.PlayCenterAnimation(location.X, location.Y, L: location.MapLevel);
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
}
