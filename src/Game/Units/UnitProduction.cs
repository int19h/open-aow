// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

// unit index, researched
public partial class UnitProduction : EngineObject
{
    public new const int ClassId = 131737;

    public UnitProduction(ECustomNode owner) : base(owner)
    { }

    public int UnitIndex;
    public bool Researched;

    // Structur.PAS:7
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref UnitIndex, 20);
        stream.rwBool(ref Researched, 21);
    }

    // Engine.pas:524

}
