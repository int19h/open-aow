// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// LibraryName. NextLibraryId
public partial class HeroLibrary : HeroList
{
    public HeroLibrary(ECustomNode owner) : base(owner)
    { }

    public string LibraryName = "";
    private int _nextLibraryId;

    // Hero.PAS:2639
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref LibraryName, 20);
        stream.Int32(ref _nextLibraryId, 21);
    }

    // Hero.PAS:2592
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        var hero = (Hero)child;
        // { Original checks unit IDs rather than library IDs for a collision. }
        if (hero.LibraryId == 0 || FindHero(hero.LibraryId) != null)
        {
            hero.LibraryId = CreateUniqueLibraryId();
        }

        AppendChild(child);
        if (child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        return Count - 1;
    }

    // Hero.PAS:2580
    public int CreateUniqueLibraryId()
    {
        int I;
        do
        {
            I = 0;
            _nextLibraryId += 1;
            if (_nextLibraryId > 268435455)
            {
                _nextLibraryId = 0;
            }

            while (I != Count && this[I].LibraryId != _nextLibraryId)
            {
                I += 1;
            }
        }
        while (!(I == Count));
        return _nextLibraryId;
    }

    // Hero.PAS:2602
    public Hero CreateHero()
    {
        Hero hero;
        hero = new Hero(null);
        hero.SetHeroResourceIndex(0);
        hero.LibraryId = CreateUniqueLibraryId();
        hero.LibraryName = LibraryName;
        hero.SetOwner(this);
        hero.Release();
        return hero;
    }

    // Hero.PAS:2614
    public string GetName()
    {
        string result = LibraryName;
        if (IsEmpty(result))
        {
            result = AbilityRules.TranslateResourceString(GameText.SystemText);
        }

        return result;
    }

    // Hero.PAS:2620
    public Hero FindLibraryId(int id)
    {
        int I;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            if (id == this[I].LibraryId)
            {
                return this[I];
            }
        }

        return null;
    }

    // Hero.PAS:2633
    public void LoadFromFile(string fileName)
    {
        GameTables.GameEngine.ReadFromFile(fileName, this, null, GameProfile.HeroList);
    }

    // Hero.PAS:2636
    public void SaveToFile(string fileName)
    {
        GameTables.GameEngine.WriteToFile(fileName, this, null, GameProfile.HeroList);
    }

    // Engine.pas:631

}
