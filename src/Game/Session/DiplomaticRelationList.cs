// Translated from AoWE.pas.
using Aow.Engine.Collections;

namespace Aow.Game.Session;

// inherited TByteList, default relation
public class DiplomaticRelationList : ByteList
{
    // Player.PAS:11
    public DiplomaticRelationList() => Default = (int)DiplomaticRelation.Unknown;

    // Player.PAS:17
    public DiplomaticRelation GetDiplomaticRelation(int index) => index < Count ? (DiplomaticRelation)List[index] : DiplomaticRelation.Unknown;

    // Player.PAS:23
    public void SetDiplomaticRelation(int index, DiplomaticRelation value) => this[index] = (byte)(int)value;
}
