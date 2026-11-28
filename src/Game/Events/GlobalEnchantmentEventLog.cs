#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Session;

namespace Aow.Game.Events;

// (enchantment player)
public class GlobalEnchantmentEventLog(CustomNode owner, EngineObject holder) : LocationTextEventLog(owner, holder: holder)
{
    public new const int ClassId = 131_819;

    public PlayerId? EnchantmentPlayer { get; set; } = PlayerId.Independent;

    // Spells.PAS:2660
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentPlayer = persister.OptionalPlayer(name: "EnchantmentPlayer", value: EnchantmentPlayer, width: IdWidth.UnsignedByte, fallback: 0);
    }

    // Spells.PAS:2665
    public override int LeftIcon
    {
        get
        {
            sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(EnchantmentPlayer);
            return player > 0 && GetEngine().StrategicMap.Players.GetCount() > player
                ? GetEngine().StrategicMap.Players.GetPlayers(EnchantmentPlayer).PlayerRace
                : 12;
        }
    }
}
