// Translated from AoWE.pas.
using Aow.Engine.Collections;

namespace Aow.Game.Session;

// inherited TByteList, default relation
public partial class DiplomaticRelationList : ByteList
{
    // Player.PAS:11
    public DiplomaticRelationList()
    {
        Default = (int)DiplomaticRelation.Unknown;
    }

    // Player.PAS:17
    public DiplomaticRelation GetDiplomaticRelation(int index)
    {
        DiplomaticRelation result;
        if (index < Count)
        {
            result = (DiplomaticRelation)List[index];
        }
        else
        {
            result = DiplomaticRelation.Unknown;
        }

        return result;
    }

    // Player.PAS:23
    public void SetDiplomaticRelation(int index, DiplomaticRelation value)
    {
        this[index] = (byte)(int)value;
    }

    // intf_P1_03.inc:46

}
