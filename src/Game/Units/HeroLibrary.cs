// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FLibraryName. FNextLibraryID
public partial class HeroLibrary : HeroList
{
    public HeroLibrary(ECustomNode owner) : base(owner)
    { }

    public string FLibraryName = "";
    public int FNextLibraryID;

    // Hero.PAS:2639
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FLibraryName, 20);
        Stream.rwInteger(ref FNextLibraryID, 21);
    }

    // Hero.PAS:2592
    public override int AddChild(EObject Child)
    {
        var hero = (Hero)Child;
        // { Original checks unit IDs rather than library IDs for a collision. }
        if (hero.FLibraryID == 0 || FindHero(hero.FLibraryID) != null)
        {
            hero.FLibraryID = CreateUniqueLibraryID();
        }

        FChildren.Add(Child);
        if (Child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        return FChildren.Count - 1;
    }

    // Hero.PAS:2580
    public int CreateUniqueLibraryID()
    {
        int I;
        do
        {
            I = 0;
            FNextLibraryID += 1;
            if (FNextLibraryID > 268435455)
            {
                FNextLibraryID = 0;
            }

            while (I != FChildren.Count && ((Hero)FChildren.List[I]).FLibraryID != FNextLibraryID)
            {
                I += 1;
            }
        }
        while (!(I == FChildren.Count));
        return FNextLibraryID;
    }

    // Hero.PAS:2602
    public Hero CreateHero()
    {
        Hero Hero;
        Hero = new Hero(null);
        Hero.SetHeroResourceIndex(0);
        Hero.FLibraryID = CreateUniqueLibraryID();
        Hero.FLibraryName = FLibraryName;
        Hero.SetOwner(this);
        Hero.Release();
        return Hero;
    }

    // Hero.PAS:2614
    public string GetName()
    {
        string Result = FLibraryName;
        if (IsEmpty(Result))
        {
            Result = AbilityRules.TranslateRStr(GameText.SystemRStr);
        }

        return Result;
    }

    // Hero.PAS:2620
    public Hero FindLibraryID(int ID)
    {
        int I;
        I = FChildren.Count;
        while (I != 0)
        {
            I -= 1;
            if (ID == ((Hero)FChildren.List[I]).FLibraryID)
            {
                return (Hero)FChildren.List[I];
            }
        }

        return null;
    }

    // Hero.PAS:2633
    public void LoadFromFile(string FileName)
    {
        GameTables.AoWEngine.ReadFromFile(FileName, this, null, GameProfile.HeroList);
    }

    // Hero.PAS:2636
    public void SaveToFile(string FileName)
    {
        GameTables.AoWEngine.WriteToFile(FileName, this, null, GameProfile.HeroList);
    }

    // Engine.pas:631

}
