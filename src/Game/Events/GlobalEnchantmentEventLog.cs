// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (enchantment player)
public partial class GlobalEnchantmentEventLog : LocationTextEventLog
{
    public GlobalEnchantmentEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131819;

    public sbyte EnchantmentPlayer;

    // Spells.PAS:2660
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref EnchantmentPlayer), 30);
    }

    // Spells.PAS:2665
    public override int GetLeftIcon()
    {
        int result;
        sbyte player;
        player = EnchantmentPlayer;
        if (player > 0 && StrategicMap.Current.Players.GetCount() > player)
        {
            result = StrategicMap.Current.Players.GetPlayers(EnchantmentPlayer).PlayerRace;
        }
        else
        {
            result = 12;
        }

        return result;
    }

}
