// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class AoWCampaignScenarioList : ENode
{
    public AoWCampaignScenarioList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2214
    public AoWCampaignScenario GetScenario(int Index)
    {
        AoWCampaignScenario Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (AoWCampaignScenario)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Diplomacy.pas:2221
    public int CreateUniqueID()
    {
        int Result = 1;
        while (IndexOfID(Result) != -1)
        {
            Result += 1;
        }

        return Result;
    }

    // Diplomacy.pas:2227
    public int IndexOfID(int ID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AoWCampaignScenario)FChildren.List[Result]).FID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Diplomacy.pas:2238
    public AoWCampaignScenario FindID(int ID)
    {
        AoWCampaignScenario Result;
        int Index;
        Index = IndexOfID(ID);
        if (Index != -1)
        {
            Result = (AoWCampaignScenario)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:631

}
