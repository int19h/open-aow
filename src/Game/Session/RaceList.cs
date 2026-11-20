#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Session;

public class RaceList : ControlStyleNode, IChildList<Race>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Race, so the cast never fails.
    public Race this[int index] => GetChild<Race>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Race;

    int IChildSink<Race>.AddChild(Race child) => AddChild(child);

    // Ability.PAS:367
    public RaceList(CustomNode owner) : base(owner) => SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));

    public int RelationOffset { get; set; }

    // Ability.PAS:379
    public Race GetRace(int index) => index >= 0 && index < Count ? this[index] : null;

    // Ability.PAS:385
    public int GetEnabledRaces()
    {
        int result = 0;
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null && this[i].Enabled)
            {
                result += 1;
            }
        }

        return result;
    }

    // Ability.PAS:394
    public sbyte GetRandomRace()
    {
        DelphiList<Race> races = new();
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null && this[i].Enabled)
            {
                _ = races.Add(this[i]);
            }
        }

        sbyte result;
        if (races.Count != 0)
        {
            int random = Random(races.Count);
            result = races.List[random].RaceIndex;
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
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null)
            {
                this[i].ResetPlayerRelations();
            }
        }
    }

    // Ability.PAS:415
    public void ResetDefaultRelations()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null)
            {
                this[i].ResetDefaultRaceRelations();
            }
        }
    }

    // Ability.PAS:423
    public void Update()
    {
        for (int i = 0, last = GamePack.Current.RaceResources.GetCount() - 1; i <= last; i++)
        {
            if (GamePack.Current.RaceResources.GetRaceResource(i) is not null)
            {
                if (GetRace(i) is null)
                {
                    Race race = new(null)
                    {
                        Holder = CreationHolder(),
                        RaceIndex = (sbyte)i
                    };
                    race.SetArrayOwner(this, index: i);
                    race.SetEnabled(true);
                }
            }
            else
            {
                GetRace(i)?.Dispose();
            }
        }
    }

    // intf_P1_03.inc:34
    // TEObject.ReadWrite is empty. The original ReadWrite bypasses TEControlStyleNode.ReadWrite, and
    // the tree method bypasses ControlStyleNode.PersistFields.
    public override void PersistFields(IPersister persister) => RelationOffset = persister.Int32("RelationOffset", value: RelationOffset);
}
