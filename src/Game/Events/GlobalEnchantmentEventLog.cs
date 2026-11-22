#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (enchantment player)
public class GlobalEnchantmentEventLog(CustomNode owner) : LocationTextEventLog(owner)
{
    public new const int ClassId = 131_819;

    public sbyte EnchantmentPlayer { get; set; }

    // Spells.PAS:2660
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentPlayer = (sbyte)persister.UnsignedByte(name: "EnchantmentPlayer", value: EnchantmentPlayer);
    }

    // Spells.PAS:2665
    public override int LeftIcon
    {
        get
        {
            sbyte player = EnchantmentPlayer;
            return player > 0 && StrategicMap.Current.Players.GetCount() > player
                ? StrategicMap.Current.Players.GetPlayers(EnchantmentPlayer).PlayerRace
                : 12;
        }
    }
}
