#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// The original's THeroLibrary derives from THeroList. The port's library holds its heroes as
// references in its node children and reads them through Heroes.
public partial class HeroLibrary : Engine.Nodes.ReferenceNode, IChildList<Hero>, IHeroHolder
{
    /// <summary>The heroes of this library, in their stored order.</summary>
    public HeroCollection Heroes => new(this);

    Hero IOwner<Hero>.this[int index] => Heroes[index];

    // A library takes heroes only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Hero;

    int IChildSink<Hero>.AddChild(Hero child) => AddChild(child);

    public HeroLibrary(CustomNode owner) : base(owner)
    { }

    public string LibraryName { get; set; } = "";

    private int _nextLibraryId;

    // Hero.PAS:2639
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        LibraryName = persister.String("LibraryName", value: LibraryName);
        _nextLibraryId = persister.Int32("NextLibraryId", value: _nextLibraryId);
    }

    // Hero.PAS:2592
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        // CheckChild refuses a non-hero. The cast names the child the library id is written on.
        Hero hero = (Hero)child;
        // { Original checks unit IDs rather than library IDs for a collision. }
        if (hero.LibraryId == 0 || Heroes.FindHero(hero.LibraryId) != null)
        {
            hero.LibraryId = CreateUniqueLibraryId();
        }

        AppendChild(child);
        return child.AddRef() == -1 ? throw new NotSupportedException("EObject doesn't support ReferenceCounting") : Heroes.Count - 1;
    }

    // Hero.PAS:2580
    public int CreateUniqueLibraryId()
    {
        int i;
        do
        {
            i = 0;
            _nextLibraryId += 1;
            if (_nextLibraryId > 268435455)
            {
                _nextLibraryId = 0;
            }

            while (i != Heroes.Count && Heroes[i].LibraryId != _nextLibraryId)
            {
                i += 1;
            }
        }
        while (!(i == Heroes.Count));
        return _nextLibraryId;
    }

    // Hero.PAS:2602
    public Hero CreateHero()
    {
        Hero hero;
        hero = new Hero(null) { Holder = CreationHolder() };
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
            result = AbilityRules.TranslateResourceString(GetEngine(), GameText.SystemText);
        }

        return result;
    }

    // Hero.PAS:2620
    public Hero FindLibraryId(int id)
    {
        int i;
        i = Heroes.Count;
        while (i != 0)
        {
            i -= 1;
            if (id == Heroes[i].LibraryId)
            {
                return Heroes[i];
            }
        }

        return null;
    }

    // Hero.PAS:2633
    public void LoadFromFile(string fileName) => GetEngine().ReadFromFile(fileName, this, null, GameProfile.HeroList);

    // Hero.PAS:2636
    public void SaveToFile(string fileName) => GetEngine().WriteToFile(fileName, this, null, GameProfile.HeroList);

    // Engine.pas:631

}
