// Translated from Storms.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// storm, X, Y, layer
public partial class StormSpellTokenEvent : SpellTokenEvent
{
    public StormSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:730
    public new const int ClassId = 135937;

    public Storm Storm;
    public sbyte X;
    public sbyte Y;
    public sbyte MapLevel;

    // Storms.pas:735
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref X), 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Y), 21);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 22);
    }

    // Storms.pas:743
    public override bool Process(Troop source, int value)
    {
        bool result;
        base.Process(source, value);
        if (value == 0)
        {
            Storm = ((StormSpell)AoWHSSet.Current.SpellControl.GetSpell(EventSpellId)).CreateStorm();
            Storm.InitializeStorm(source.Player, X, Y, MapLevel);
            AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        }

        if (!Storm.UpdateStorm())
        {
            Storm.FinalizeStorm();
            Storm?.Dispose();
            Storm = null;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // NetworkE.pas:230

}
