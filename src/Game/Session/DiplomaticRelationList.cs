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
    public DiplomaticRelation GetDiplomaticRelation(int Index)
    {
        DiplomaticRelation Result;
        if (Index < Count)
        {
            Result = (DiplomaticRelation)List[Index];
        }
        else
        {
            Result = DiplomaticRelation.Unknown;
        }

        return Result;
    }

    // Player.PAS:23
    public void SetDiplomaticRelation(int Index, DiplomaticRelation Value)
    {
        this[Index] = (byte)(int)Value;
    }

    // intf_P1_03.inc:46

}
