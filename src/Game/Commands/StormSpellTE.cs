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
public partial class StormSpellTE : SpellTE
{
    public StormSpellTE(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:730
    public new const int ClassID = 135937;

    public Storm FStorm;
    public sbyte FX;
    public sbyte FY;
    public sbyte FL;

    // Storms.pas:735
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 22);
    }

    // Storms.pas:743
    public override bool Process(AbstractUnit source, int value)
    {
        bool Result;
        base.Process(source, value);
        if (value == 0)
        {
            FStorm = ((StormSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).CreateStorm();
            FStorm.InitializeStorm(source.FPlayer, FX, FY, FL);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        }

        if (!FStorm.UpdateStorm())
        {
            FStorm.FinalizeStorm();
            FStorm?.Dispose();
            FStorm = null;
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // NetworkE.pas:230

}
