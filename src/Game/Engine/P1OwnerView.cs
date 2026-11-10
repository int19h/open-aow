// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Engine;

public partial class P1OwnerView : EObject
{
    public P1OwnerView(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:24
    public virtual bool HasAbility(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:25
    public virtual void SetAbility(int Index, byte Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:26
    public virtual void Slot54()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:27
    public virtual void Slot58()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:28
    public virtual void Slot5C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:29
    public virtual string OwnerName()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:30
    public virtual void Slot64()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:31
    public virtual void Slot68()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:32
    public virtual void Slot6C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:33
    public virtual void Slot70()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:34
    public virtual void Slot74()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:35
    public virtual void Slot78()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:36
    public virtual void Slot7C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:37
    public virtual void Slot80()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:38
    public virtual void Slot84()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:39
    public virtual void Slot88()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:40
    public virtual Set<AbilitySelectionType> SelectionTypes()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
