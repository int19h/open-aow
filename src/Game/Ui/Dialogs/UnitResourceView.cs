// Translated from UnitIDlg.pas.
using Aow.Engine.Buffers;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Ui.Dialogs;

public struct UnitResourceView
{
    public FixedArray_byte_40 Reserved00;
    public byte Alignment;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Damage;
    public sbyte Hits;
    public sbyte Moves;
    public sbyte Resistance;
    public FixedArray_byte_9 Reserved2F;
    public AbilityOwner AbilityOwner;
}
