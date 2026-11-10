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

    public new const int ClassID = 131819;

    public sbyte FEnchantmentPlayer;

    // Spells.PAS:2660
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FEnchantmentPlayer), 30);
    }

    // Spells.PAS:2665
    public override int GetLeftIcon()
    {
        int Result;
        sbyte Player;
        Player = FEnchantmentPlayer;
        if (Player > 0 && AoWHSMap.Current.FPlayers.GetCount() > Player)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FEnchantmentPlayer).FPlayerRace;
        }
        else
        {
            Result = 12;
        }

        return Result;
    }

}
