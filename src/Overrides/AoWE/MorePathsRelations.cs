namespace Aow.Units.AoWE;

public static partial class AoWE
{
    private static void InitializeMorePathsRelations()
    {
        TDiplomaticRelationRStr[(int)TDiplomaticRelation.drAlliance] = AllianceRStr;
        TDiplomaticRelationRStr[(int)TDiplomaticRelation.drPeace] = P5PeaceRStr;
        TDiplomaticRelationRStr[(int)TDiplomaticRelation.drWar] = P5WarRStr;
        TDiplomaticRelationRStr[(int)TDiplomaticRelation.drUnknown] = UnknownRStr;
    }
}
