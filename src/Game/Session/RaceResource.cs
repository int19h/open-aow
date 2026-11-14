// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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
        _heroNames = new StringList();
    }

    private CustomImageLibrary _faceLibrary;

    public CustomImageLibrary ImageLibrary { get; private set; }

    private string _imageLibraryName = "";
    private string _name = "";

    public string RaceName { get; private set; } = "";

    private string _description = "";

    public Set<Alignment> Alignments { get; private set; }

    private int _references;

    public UnitIndexList UnitIndexes { get; }

    private int _heroUnitIndex;

    public UnitIndexList SpecialUnitIndexes { get; }

    private readonly StringList _heroNames;

    // PlayerC.PAS:1740
    public override void Dispose()
    {
        ReleaseImageLibrary();
        UnitIndexes?.Dispose();
        SpecialUnitIndexes?.Dispose();
        _heroNames?.Dispose();
        base.Dispose();
    }

    // PlayerC.PAS:1861
    // A load and a save store the image library name. Only the translate stream sets the text-only
    // mode.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _name = persister.String("Name", value: _name);
        RaceName = persister.String("RaceName", value: RaceName);
        _description = persister.String("Description", value: _description);
        // The original ReadWrite leaves out the image library name under text only (the translate
        // stream).
        if (!PersistSupport.Translating(persister))
        {
            _imageLibraryName = persister.String("ImageLibraryName", value: _imageLibraryName);
        }

        Alignments = persister.Set("Alignments", value: Alignments);
        PersistSupport.UnitIndexList(persister, name: "UnitIndexes", list: UnitIndexes);
        PersistSupport.UnitIndexList(persister, name: "SpecialUnitIndexes", list: SpecialUnitIndexes);
        _heroUnitIndex = persister.Int32("HeroUnitIndex", value: _heroUnitIndex);
        PersistSupport.Lines(persister, name: "HeroNames", lines: _heroNames);
    }

    // PlayerC.PAS:1798
    public override int AddRef()
    {
        if (_references == 0)
        {
            LinkToImageLibrary();
        }

        _references += 1;
        return _references;
    }

    // PlayerC.PAS:1805
    public override int Release()
    {
        _references -= 1;
        if (_references == 0)
        {
            ReleaseImageLibrary();
        }

        return _references;
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
        images = _faceLibrary;
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
            if (!IsEmpty(_imageLibraryName))
            {
                ImageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            }

            _faceLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
        }
    }

    // PlayerC.PAS:1776
    public void ReleaseImageLibrary()
    {
        // { Original leaves the face library linked when the city library is nil. }
        if (ImageLibrary != null)
        {
            if (!IsEmpty(_imageLibraryName))
            {
                GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            }

            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "FACES\\R_FACES.ILB");
            _faceLibrary = null;
            ImageLibrary = null;
        }
    }

    // PlayerC.PAS:1788
    public void SetImageLibraryName(string value)
    {
        if (value != _imageLibraryName)
        {
            ReleaseImageLibrary();
            _imageLibraryName = value;
            if (_references != 0)
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
        if (Alignments == AlignmentTables.PureGoodAlignments)
        {
            return Alignment.PureGood;
        }

        if (Alignments == AlignmentTables.GoodAlignments)
        {
            result = Alignment.Good;
        }
        else
        {
            if (Alignments == AlignmentTables.NeutralAlignments)
            {
                result = Alignment.Neutral;
            }
            else
            {
                if (Alignments == AlignmentTables.EvilAlignments)
                {
                    result = Alignment.Evil;
                }
                else
                {
                    if (Alignments == AlignmentTables.PureEvilAlignments)
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

    private static readonly Set<Alignment> s_pureNeutralAlignments = Set<Alignment>.FromMask(72uL);

}
