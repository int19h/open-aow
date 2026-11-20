#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.World.Structures;

public class StructureControl : EngineObject
{
    // Structur.PAS:3220
    // (structures, next ID, city count)
    public StructureControl(CustomNode owner) : base(owner)
    {
        _structures = new OwningNode(null) { Holder = CreationHolder() };
        _nextId = 0;
    }

    private readonly OwningNode _structures;
    private int _nextId;

    public int CityCount { get; private set; }

    // Structur.PAS:3227
    public override void Dispose()
    {
        _structures?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3245
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _nextId = persister.Int32("NextId", value: _nextId);
        CityCount = persister.Int32("CityCount", value: CityCount);
    }

    // Structur.PAS:3233
    public Structure GetStructure(int index) => index >= 0 && index < GetCount() ? _structures.GetChild<Structure>(index) : null;

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
            {
                break;
            }
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
                if (id == _structures.GetChild<Structure>(result).StructureId)
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
        int index = IndexOfId(id);
        return index != -1 ? _structures.GetChild<Structure>(index) : null;
    }

    // Structur.PAS:3281
    public void Register(Structure structure)
    {
        Assert(_structures.IndexOfChild(structure) == -1, "", "Structur.PAS", 3299);
        if (structure.StructureId == 0 || IndexOfId(structure.StructureId) != -1)
        {
            structure.StructureId = CreateUniqueId();
        }

        _ = _structures.AddChild(structure);
        if (structure.EngineClass.ClassId == 131336)
        {
            CityCount += 1;
        }
    }

    // Structur.PAS:3306
    public void Unregister(Structure structure)
    {
        _ = _structures.DropChild(structure);
        if (structure.EngineClass.ClassId == 131336)
        {
            CityCount -= 1;
        }
    }

    // Structur.PAS:3312
    public void Activate() => CityCount = 0;
}
