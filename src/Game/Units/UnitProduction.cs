// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

// unit index, researched
public partial class UnitProduction : EObject
{
    public new const int ClassID = 131737;

    public UnitProduction(ECustomNode owner) : base(owner)
    { }

    public int FUnitIndex;
    public bool FResearched;

    // Structur.PAS:7
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitIndex, 20);
        Stream.rwBool(ref FResearched, 21);
    }

    // Engine.pas:524

}
