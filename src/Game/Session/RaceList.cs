// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class RaceList : EControlStyleNode
{
    // Ability.PAS:367
    public RaceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    public int FRelationOffset;

    // Ability.PAS:379
    public Race GetRace(int Index)
    {
        Race Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (Race)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Ability.PAS:385
    public int GetEnabledRaces()
    {
        int Result = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null && ((Race)FChildren.List[I]).FEnabled)
            {
                Result += 1;
            }
        }

        return Result;
    }

    // Ability.PAS:394
    public sbyte GetRandomRace()
    {
        sbyte Result;
        DelphiList Races;
        Races = new DelphiList();
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null && ((Race)FChildren.List[I]).FEnabled)
            {
                Races.Add(FChildren.List[I]);
            }
        }

        if (Races.Count != 0)
        {
            var random = Random(Races.Count);
            Result = ((Race)Races.List[random]).FRace;
        }
        else
        {
            Result = -1;
        }

        Races?.Dispose();
        return Result;
    }

    // Ability.PAS:407
    public void ResetPlayerRelations()
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null)
            {
                ((Race)FChildren.List[I]).ResetPlayerRelations();
            }
        }
    }

    // Ability.PAS:415
    public void ResetDefaultRelations()
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null)
            {
                ((Race)FChildren.List[I]).ResetDefaultRaceRelations();
            }
        }
    }

    // Ability.PAS:423
    public void Update()
    {
        Race Race;
        for (int I = 0, last = AoWHSSet.Current.FRaceResources.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSSet.Current.FRaceResources.GetRaceResource(I) != null)
            {
                if (GetRace(I) == null)
                {
                    Race = new Race(null);
                    Race.FRace = (sbyte)I;
                    Race.SetArrayOwner(this, I);
                    Race.SetEnabled(true);
                }
            }
            else
            {
                GetRace(I)?.Dispose();
            }
        }
    }

    // intf_P1_03.inc:34
    // TEObject.ReadWrite is empty. The original bypasses TEControlStyleNode.ReadWrite.
    public override void ReadWrite(EStorageStream stream) => stream.rwInteger(ref FRelationOffset, 20);
}
