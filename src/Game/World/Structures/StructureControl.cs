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
        FStructures = new ENode(null);
        FNextID = 0;
    }

    public ENode FStructures;
    public int FNextID;
    public int FCityCount;

    // Structur.PAS:3227
    public override void Dispose()
    {
        FStructures?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3245
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FNextID, 20);
        Stream.rwInteger(ref FCityCount, 21);
    }

    // Structur.PAS:3233
    public Structure GetStructure(int Index)
    {
        Structure Result;
        if (Index >= 0 && Index < GetCount())
        {
            Result = (Structure)FStructures.FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:3240
    public int GetCount() => FStructures.GetCount();

    // Structur.PAS:3252
    public int CreateUniqueID()
    {
        while (true)
        {
            FNextID += 1;
            if (FNextID > 268435455)
            {
                FNextID = 0;
            }

            if (IndexOfID(FNextID) == -1)
                break;
        }

        return FNextID;
    }

    // Structur.PAS:3261
    public int IndexOfID(int ID)
    {
        int Result = GetCount();
        if (Result != 0)
        {
            do
            {
                Result -= 1;
                if (ID == ((Structure)FStructures.FChildren.List[Result]).FStructureID)
                {
                    return Result;
                }
            }
            while (!(Result == 0));
        }

        return -1;
    }

    // Structur.PAS:3272
    public Structure FindID(int ID)
    {
        Structure Result;
        int Index;
        Index = IndexOfID(ID);
        if (Index != -1)
        {
            Result = (Structure)FStructures.FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:3281
    public void Register(Structure Structure)
    {
        Assert(FStructures.FChildren.IndexOf(Structure) == -1, "", "Structur.PAS", 3299);
        if (Structure.FStructureID == 0 || IndexOfID(Structure.FStructureID) != -1)
        {
            Structure.FStructureID = CreateUniqueID();
        }

        FStructures.AddChild(Structure);
        if (Structure.EClass.ClassID == 131336)
        {
            FCityCount += 1;
        }
    }

    // Structur.PAS:3306
    public void Unregister(Structure Structure)
    {
        FStructures.FChildren.Remove(Structure);
        if (Structure.EClass.ClassID == 131336)
        {
            FCityCount -= 1;
        }
    }

    // Structur.PAS:3312
    public void Activate()
    {
        FCityCount = 0;
    }

    // intf_P1_09.inc:166

}
