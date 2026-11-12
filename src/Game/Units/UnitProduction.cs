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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref UnitIndex, 20);
        stream.Boolean(ref Researched, 21);
    }

    // Engine.pas:524

}
