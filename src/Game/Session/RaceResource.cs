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
    public new const int ClassId = 131585;

    // PlayerC.PAS:1732
    public RaceResource(ECustomNode owner) : base(owner)
    {
        UnitIndexes = new UnitIndexList();
        SpecialUnitIndexes = new UnitIndexList();
        HeroNames = new StringList();
    }

    public CustomImageLibrary FaceLibrary;
    public CustomImageLibrary ImageLibrary;
    public string ImageLibraryName = "";
    public string Name = "";
    public string RaceName = "";
    public string Description = "";
    public Set<Alignment> Alignments;
    public int References;
    public UnitIndexList UnitIndexes;
    public int HeroUnitIndex;
    public UnitIndexList SpecialUnitIndexes;
    public StringList HeroNames;

    // PlayerC.PAS:1740
    public override void Dispose()
    {
        ReleaseImageLibrary();
        UnitIndexes?.Dispose();
        SpecialUnitIndexes?.Dispose();
        HeroNames?.Dispose();
        base.Dispose();
    }

    // PlayerC.PAS:1749
    public override GameEngine GetEngine() => GameTables.GameEngine;

    // PlayerC.PAS:1861
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 5);
        stream.rwString(ref RaceName, 6);
        stream.rwString(ref Description, 7);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref ImageLibraryName, 8);
        }

        RwSet(ref Alignments, 1, 9, stream.rwData, 0);
        stream.rwStreamProc(10, UnitIndexes.LoadFromStream, UnitIndexes.SaveToStream);
        stream.rwStreamProc(11, SpecialUnitIndexes.LoadFromStream, SpecialUnitIndexes.SaveToStream);
        stream.rwInteger(ref HeroUnitIndex, 12);
        stream.rwStrings(HeroNames, 13);
    }

    // PlayerC.PAS:1798
    public override int AddRef()
    {
        if (References == 0)
        {
            LinkToImageLibrary();
        }

        References += 1;
        return References;
    }

    // PlayerC.PAS:1805
    public override int Release()
    {
        References -= 1;
        if (References == 0)
        {
            ReleaseImageLibrary();
        }

        return References;
    }

    // PlayerC.PAS:1855
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            UnitIndexes.VerifyUnits(true);
        }
    }

    // PlayerC.PAS:1818
    public override LibraryImage GetCellImage()
    {
        int I;
        AddRef();
        LibraryImage result = null;
        if (ImageLibrary != null)
        {
            I = 0;
            while (I < ImageLibrary.Count && result == null)
            {
                result = ImageLibrary[I];
                I += 1;
            }
        }

        Release();
        return result;
    }

    // PlayerC.PAS:1836
    public override void SetCellImage(LibraryImage value)
    {
    }

    // PlayerC.PAS:1752
    public LibraryImage GetFace()
    {
        LibraryImage result;
        CustomImageLibrary images;
        AddRef();
        images = FaceLibrary;
        if (images != null)
        {
            result = images.GetSafe(GetRaceIndex());
        }
        else
        {
            result = null;
        }

        Release();
        return result;
    }

    // PlayerC.PAS:1763
    public int GetRaceIndex() => EditIndex;

    // PlayerC.PAS:1766
    public void LinkToImageLibrary()
    {
        if (ImageLibrary == null)
        {
            if (!IsEmpty(ImageLibraryName))
            {
                ImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            }

            FaceLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
        }
    }

    // PlayerC.PAS:1776
    public void ReleaseImageLibrary()
    {
        // { Original leaves the face library linked when the city library is nil. }
        if (ImageLibrary != null)
        {
            if (!IsEmpty(ImageLibraryName))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            }

            AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
            FaceLibrary = null;
            ImageLibrary = null;
        }
    }

    // PlayerC.PAS:1788
    public void SetImageLibraryName(string value)
    {
        if (value != ImageLibraryName)
        {
            ReleaseImageLibrary();
            ImageLibraryName = value;
            if (References != 0)
            {
                LinkToImageLibrary();
            }
        }
    }

    // PlayerC.PAS:1839
    public LibraryImage GetCityImage(int size, int terrain, int level)
    {
        LibraryImage result;
        int index;
        if (terrain == 5)
        {
            index = size + 60 - 1;
        }
        else
        {
            index = terrain * 10 + 70 + (size - 1);
        }

        if (ImageLibrary.ImageExist(index))
        {
            result = ImageLibrary[index];
        }
        else
        {
            result = null;
        }

        if (result == null)
        {
            if (level == 0)
            {
                index = size + 50 - 1;
            }
            else
            {
                index = size + 60 - 1;
            }

            if (ImageLibrary.ImageExist(index))
            {
                result = ImageLibrary[index];
            }
        }

        return result;
    }

    // PlayerC.PAS:1875
    public Alignment GetAlignment()
    {
        Alignment result;
        if (Alignments == AlignmentTables.s_pureGoodAlignments)
        {
            return Alignment.PureGood;
        }

        if (Alignments == AlignmentTables.s_goodAlignments)
        {
            result = Alignment.Good;
        }
        else
        {
            if (Alignments == AlignmentTables.s_neutralAlignments)
            {
                result = Alignment.Neutral;
            }
            else
            {
                if (Alignments == AlignmentTables.s_evilAlignments)
                {
                    result = Alignment.Evil;
                }
                else
                {
                    if (Alignments == AlignmentTables.s_pureEvilAlignments)
                    {
                        result = Alignment.PureEvil;
                    }
                    else
                    {
                        if (Alignments == s_pureNeutralAlignments)
                        {
                            result = Alignment.PureNeutral;
                        }
                        else
                        {
                            result = Alignment.None;
                        }
                    }
                }
            }
        }

        return result;
    }

    // intf_P1_09.inc:24

    // PlayerC.PAS:1815
    public override string ResourceEditName() => "Race";

    private static Set<Alignment> s_pureNeutralAlignments = Set<Alignment>.FromMask(72uL);

}
