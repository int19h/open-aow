// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class StructureControl : EngineObject
{
    // Structur.PAS:3220
    // (structures, next ID, city count)
    public StructureControl(ECustomNode owner) : base(owner)
    {
        Structures = new ENode(null);
        NextId = 0;
    }

    public ENode Structures;
    public int NextId;
    public int CityCount;

    // Structur.PAS:3227
    public override void Dispose()
    {
        Structures?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3245
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref NextId, 20);
        stream.rwInteger(ref CityCount, 21);
    }

    // Structur.PAS:3233
    public Structure GetStructure(int index)
    {
        Structure result;
        if (index >= 0 && index < GetCount())
        {
            result = (Structure)Structures._children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:3240
    public int GetCount() => Structures.GetCount();

    // Structur.PAS:3252
    public int CreateUniqueId()
    {
        while (true)
        {
            NextId += 1;
            if (NextId > 268435455)
            {
                NextId = 0;
            }

            if (IndexOfId(NextId) == -1)
                break;
        }

        return NextId;
    }

    // Structur.PAS:3261
    public int IndexOfId(int id)
    {
        int result = GetCount();
        if (result != 0)
        {
            do
            {
                result -= 1;
                if (id == ((Structure)Structures._children.List[result]).StructureId)
                {
                    return result;
                }
            }
            while (!(result == 0));
        }

        return -1;
    }

    // Structur.PAS:3272
    public Structure FindId(int id)
    {
        Structure result;
        int index;
        index = IndexOfId(id);
        if (index != -1)
        {
            result = (Structure)Structures._children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:3281
    public void Register(Structure structure)
    {
        Assert(Structures._children.IndexOf(structure) == -1, "", "Structur.PAS", 3299);
        if (structure.StructureId == 0 || IndexOfId(structure.StructureId) != -1)
        {
            structure.StructureId = CreateUniqueId();
        }

        Structures.AddChild(structure);
        if (structure.EngineClass.ClassId == 131336)
        {
            CityCount += 1;
        }
    }

    // Structur.PAS:3306
    public void Unregister(Structure structure)
    {
        Structures._children.Remove(structure);
        if (structure.EngineClass.ClassId == 131336)
        {
            CityCount -= 1;
        }
    }

    // Structur.PAS:3312
    public void Activate()
    {
        CityCount = 0;
    }

    // intf_P1_09.inc:166

}
