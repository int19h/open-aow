// Translated from ManaCrystals.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.World.Terrain;

public partial class CrystalMagic : PlayerMagicControl
{
    public CrystalMagic(ECustomNode owner) : base(owner)
    { }

    public int Mana { get => FMana; set => SetMana(value); }
}
