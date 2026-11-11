// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
// face library, city library, library name, name, race name, description, alignments,
// references, units, hero unit, special units, hero names
public partial class RaceResource : EngineResource
{
    public new const int ClassID = 131585;

    // PlayerC.PAS:1732
    public RaceResource(ECustomNode owner) : base(owner)
    {
        FUnitIndexes = new UnitIndexList();
        FSpecialUnitIndexes = new UnitIndexList();
        FHeroNames = new StringList();
    }

    public CustomImageLibrary FFaceLibrary;
    public CustomImageLibrary FImageLibrary;
    public string FImageLibraryName = "";
    public string FName = "";
    public string FRaceName = "";
    public string FDescription = "";
    public Set<Alignment> FAlignments;
    public int FReferences;
    public UnitIndexList FUnitIndexes;
    public int FHeroUnitIndex;
    public UnitIndexList FSpecialUnitIndexes;
    public StringList FHeroNames;

    // PlayerC.PAS:1740
    public override void Dispose()
    {
        ReleaseIL();
        FUnitIndexes?.Dispose();
        FSpecialUnitIndexes?.Dispose();
        FHeroNames?.Dispose();
        base.Dispose();
    }

    // PlayerC.PAS:1749
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // PlayerC.PAS:1861
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 5);
        Stream.rwString(ref FRaceName, 6);
        Stream.rwString(ref FDescription, 7);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FImageLibraryName, 8);
        }

        RwSet(ref FAlignments, 1, 9, Stream.rwData, 0);
        Stream.rwStreamProc(10, FUnitIndexes.LoadFromStream, FUnitIndexes.SaveToStream);
        Stream.rwStreamProc(11, FSpecialUnitIndexes.LoadFromStream, FSpecialUnitIndexes.SaveToStream);
        Stream.rwInteger(ref FHeroUnitIndex, 12);
        Stream.rwStrings(FHeroNames, 13);
    }

    // PlayerC.PAS:1798
    public override int AddRef()
    {
        if (FReferences == 0)
        {
            LinkToIL();
        }

        FReferences += 1;
        return FReferences;
    }

    // PlayerC.PAS:1805
    public override int Release()
    {
        FReferences -= 1;
        if (FReferences == 0)
        {
            ReleaseIL();
        }

        return FReferences;
    }

    // PlayerC.PAS:1855
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FUnitIndexes.VerifyUnits(true);
        }
    }

    // PlayerC.PAS:1818
    public override LibraryImage GetCellImage()
    {
        int I;
        AddRef();
        LibraryImage Result = null;
        if (FImageLibrary != null)
        {
            I = 0;
            while (I < FImageLibrary.Count && Result == null)
            {
                Result = FImageLibrary[I];
                I += 1;
            }
        }

        Release();
        return Result;
    }

    // PlayerC.PAS:1836
    public override void SetCellImage(LibraryImage Value)
    {
    }

    // PlayerC.PAS:1752
    public LibraryImage GetFace()
    {
        LibraryImage Result;
        CustomImageLibrary Images;
        AddRef();
        Images = FFaceLibrary;
        if (Images != null)
        {
            Result = Images.GetSafe(GetRaceIndex());
        }
        else
        {
            Result = null;
        }

        Release();
        return Result;
    }

    // PlayerC.PAS:1763
    public int GetRaceIndex() => EditIndex;

    // PlayerC.PAS:1766
    public void LinkToIL()
    {
        if (FImageLibrary == null)
        {
            if (!IsEmpty(FImageLibraryName))
            {
                FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
            }

            FFaceLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
        }
    }

    // PlayerC.PAS:1776
    public void ReleaseIL()
    {
        // { Original leaves the face library linked when the city library is nil. }
        if (FImageLibrary != null)
        {
            if (!IsEmpty(FImageLibraryName))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
            }

            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
            FFaceLibrary = null;
            FImageLibrary = null;
        }
    }

    // PlayerC.PAS:1788
    public void SetImageLibraryName(string Value)
    {
        if (Value != FImageLibraryName)
        {
            ReleaseIL();
            FImageLibraryName = Value;
            if (FReferences != 0)
            {
                LinkToIL();
            }
        }
    }

    // PlayerC.PAS:1839
    public LibraryImage GetCityImage(int Size, int Terrain, int Level)
    {
        LibraryImage Result;
        int Index;
        if (Terrain == 5)
        {
            Index = Size + 60 - 1;
        }
        else
        {
            Index = Terrain * 10 + 70 + (Size - 1);
        }

        if (FImageLibrary.ImageExist(Index))
        {
            Result = FImageLibrary[Index];
        }
        else
        {
            Result = null;
        }

        if (Result == null)
        {
            if (Level == 0)
            {
                Index = Size + 50 - 1;
            }
            else
            {
                Index = Size + 60 - 1;
            }

            if (FImageLibrary.ImageExist(Index))
            {
                Result = FImageLibrary[Index];
            }
        }

        return Result;
    }

    // PlayerC.PAS:1875
    public Alignment GetAlignment()
    {
        Alignment Result;
        if (FAlignments == AlignmentTables.alsPureGood)
        {
            return Alignment.PureGood;
        }

        if (FAlignments == AlignmentTables.alsGood)
        {
            Result = Alignment.Good;
        }
        else
        {
            if (FAlignments == AlignmentTables.alsNeutral)
            {
                Result = Alignment.Neutral;
            }
            else
            {
                if (FAlignments == AlignmentTables.alsEvil)
                {
                    Result = Alignment.Evil;
                }
                else
                {
                    if (FAlignments == AlignmentTables.alsPureEvil)
                    {
                        Result = Alignment.PureEvil;
                    }
                    else
                    {
                        if (FAlignments == alsPureNeutral)
                        {
                            Result = Alignment.PureNeutral;
                        }
                        else
                        {
                            Result = Alignment.None;
                        }
                    }
                }
            }
        }

        return Result;
    }

    // intf_P1_09.inc:24

    // PlayerC.PAS:1815
    public override string ResourceEditName() => "Race";

    private static Set<Alignment> alsPureNeutral = Set<Alignment>.FromMask(72uL);

}
