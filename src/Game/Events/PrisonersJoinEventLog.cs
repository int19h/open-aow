// Translated from Dungeon.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// FX, FY, FL
public partial class PrisonersJoinEventLog : JoinEventLog
{
    public PrisonersJoinEventLog(ECustomNode owner) : base(owner)
    { }

    public sbyte FX;
    public sbyte FY;
    public sbyte FL;

    // Dungeon.pas:292
    public override bool Accept(ref string ErrorText)
    {
        bool Result;
        Dungeon Dungeon;
        StructureTokenEvent TE;
        Dungeon = (Dungeon)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
        if (Dungeon != null && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            TE = Dungeon.CreateTE();
            Dungeon.SetupTE(TE);
            TE.FParam = FPlayer;
            TE.FAction = 1;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Dungeon.pas:311
    public override bool Cancel()
    {
        Dungeon Dungeon;
        int Index;
        Dungeon = (Dungeon)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
        if (Dungeon != null)
        {
            for (int I = 0, last = Dungeon.FPrisoners.Count - 1; I <= last; I++)
            {
                var unitType = Dungeon.FPrisoners.GetUnit(I).GetUnitType();
                if (Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType))
                {
                    Index = Dungeon.FPrisoners.GetUnit(I).GetUnitGFXResourceIndex();
                    if (AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index) != null)
                    {
                        var random = Random(15);
                        var unitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index);
                        unitGFXResource.FSFXLibraries[3].PlayEx(0, 100, false, random, SamplePanning.Center, null);
                    }
                }
            }
        }

        return true;
    }

}
