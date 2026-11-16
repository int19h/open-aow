// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (enchantment player)
public partial class GlobalEnchantmentEventLog : LocationTextEventLog
{
    public GlobalEnchantmentEventLog(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131819;

    public sbyte EnchantmentPlayer { get; set; }

    // Spells.PAS:2660
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentPlayer = PersistSupport.UnsignedByte(persister, name: "EnchantmentPlayer", value: EnchantmentPlayer);
    }

    // Spells.PAS:2665
    public override int GetLeftIcon()
    {
        int result;
        sbyte player;
        player = EnchantmentPlayer;
        result = player > 0 && StrategicMap.Current.Players.GetCount() > player
            ? StrategicMap.Current.Players.GetPlayers(EnchantmentPlayer).PlayerRace
            : 12;

        return result;
    }

}
