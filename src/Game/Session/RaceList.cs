// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class RaceList : EControlStyleNode, IChildList<Race>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Race, so the cast never fails.
    public Race this[int index] => (Race)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is Race;

    int IChildSink<Race>.AddChild(Race child) => AddChild(child);

    // Ability.PAS:367
    public RaceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    public int RelationOffset;

    // Ability.PAS:379
    public Race GetRace(int index)
    {
        Race result;
        if (index >= 0 && index < _children.Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Ability.PAS:385
    public int GetEnabledRaces()
    {
        int result = 0;
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null && this[I].Enabled)
            {
                result += 1;
            }
        }

        return result;
    }

    // Ability.PAS:394
    public sbyte GetRandomRace()
    {
        sbyte result;
        DelphiList races;
        races = new DelphiList();
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null && this[I].Enabled)
            {
                races.Add(_children.List[I]);
            }
        }

        if (races.Count != 0)
        {
            var random = Random(races.Count);
            result = ((Race)races.List[random]).RaceIndex;
        }
        else
        {
            result = -1;
        }

        races?.Dispose();
        return result;
    }

    // Ability.PAS:407
    public void ResetPlayerRelations()
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null)
            {
                this[I].ResetPlayerRelations();
            }
        }
    }

    // Ability.PAS:415
    public void ResetDefaultRelations()
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null)
            {
                this[I].ResetDefaultRaceRelations();
            }
        }
    }

    // Ability.PAS:423
    public void Update()
    {
        Race race;
        for (int I = 0, last = AoWHSSet.Current.RaceResources.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSSet.Current.RaceResources.GetRaceResource(I) != null)
            {
                if (GetRace(I) == null)
                {
                    race = new Race(null);
                    race.RaceIndex = (sbyte)I;
                    race.SetArrayOwner(this, I);
                    race.SetEnabled(true);
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
    public override void ReadWrite(EStorageStream stream) => stream.rwInteger(ref RelationOffset, 20);
}
