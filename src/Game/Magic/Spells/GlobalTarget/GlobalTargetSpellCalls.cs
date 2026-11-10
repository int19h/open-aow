// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells.GlobalTarget;

public partial class GlobalTargetSpellCalls : Spell
{
    public GlobalTargetSpellCalls(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:303
    public virtual void ShowSelection(TargetSelectionControl Target, int HN, int X, int Y, in SurfaceDesc SD)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:305
    public virtual void SelectionChanged()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:306
    public virtual void SetupSelection(int Caster, int Target)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:307
    public virtual bool Selected(int Caster, int X, int Y, int Layer)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:308
    public virtual bool ValidSelection(int Target)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:309
    public virtual bool ValidSelectionMapF(int MapF)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GlobalTargetSpells.pas:310
    public virtual bool ValidMapF(int MapF)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
