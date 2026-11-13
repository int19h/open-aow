// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class StructureControl : EngineObject
{
    // Structur.PAS:3220
    // (structures, next ID, city count)
    public StructureControl(ECustomNode owner) : base(owner)
    {
        _structures = new ENode(null) { Holder = CreationHolder() };
        _nextId = 0;
    }

    private readonly ENode _structures;
    private int _nextId;
    public int CityCount;

    // Structur.PAS:3227
    public override void Dispose()
    {
        _structures?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3245
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _nextId, 20);
        stream.Int32(ref CityCount, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _nextId = persister.Int32("NextId", value: _nextId);
        CityCount = persister.Int32("CityCount", value: CityCount);
    }

    // Structur.PAS:3233
    public Structure GetStructure(int index)
    {
        Structure result;
        if (index >= 0 && index < GetCount())
        {
            result = (Structure)_structures.GetChild(index);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:3240
    public int GetCount() => _structures.GetCount();

    // Structur.PAS:3252
    public int CreateUniqueId()
    {
        while (true)
        {
            _nextId += 1;
            if (_nextId > 268435455)
            {
                _nextId = 0;
            }

            if (IndexOfId(_nextId) == -1)
                break;
        }

        return _nextId;
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
                if (id == ((Structure)_structures.GetChild(result)).StructureId)
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
            result = (Structure)_structures.GetChild(index);
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
        Assert(_structures.IndexOfChild(structure) == -1, "", "Structur.PAS", 3299);
        if (structure.StructureId == 0 || IndexOfId(structure.StructureId) != -1)
        {
            structure.StructureId = CreateUniqueId();
        }

        _structures.AddChild(structure);
        if (structure.EngineClass.ClassId == 131336)
        {
            CityCount += 1;
        }
    }

    // Structur.PAS:3306
    public void Unregister(Structure structure)
    {
        _structures.DropChild(structure);
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
