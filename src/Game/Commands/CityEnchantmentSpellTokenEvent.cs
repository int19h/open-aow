// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Commands;

public partial class CityEnchantmentSpellTokenEvent : CityTargetSpellTokenEvent
{
    public CityEnchantmentSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:551
    public new const int ClassId = 131821;

    // NetworkE.pas:230

}
