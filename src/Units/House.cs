// Translated from House.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.CombatStructure;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.City;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.House
{
    public partial class THouse : TCombatStructure
    {
        public THouse(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 2229522;

        public int FRace;
        public int FFlagID;
        // House.pas:45
        public override void Activate()
        {
            TPlayerStructure Structure = default;
            base.Activate();
            if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                var call__1 = AoWHSMap.MapContainer.GetMapLevel(CombatLLoc);
                var call__2 = call__1.GetField(CombatXLoc, CombatYLoc);
                var call__3 = call__2.FindNoneTransparentHS(typeof(TPlayerStructure));
                Structure = (TPlayerStructure)call__3;
                if (Structure != null)
                {
                    if (Structure is TCity)
                    {
                        FRace = ((TCity)Structure).FRace;
                    }
                    else
                    {
                        FRace = 0;
                    }

                    if (Structure.FPlayer == -1)
                    {
                        FFlagID = -1;
                    }
                    else
                    {
                        var call__4 = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__4.FPlayerRace;
                    }

                    if (FFlagID == -1)
                    {
                        FFlagID = 32;
                    }
                }
            }
        }

    }

    public partial class THouseResource : TCombatStructureResource
    {
        public THouseResource(TECustomNode owner) : base(owner)
        {}

        // House.pas:145
        public new const int ClassID = 2229523;

        // House.pas:151
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229522;
            return Result;
        }

        // House.pas:148
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat House";
            return Result;
        }

    }

    public static partial class House
    {
        // House.pas:154
        public static void RegisterHouse(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<THouseResource>(), EClass.Of<THouse>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
