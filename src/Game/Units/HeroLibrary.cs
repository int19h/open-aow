#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Units;

// The original's THeroLibrary derives from THeroList. The port's library holds its heroes as
// references in its node children and reads them through Heroes.
public sealed class HeroLibrary(CustomNode? owner) : Engine.Nodes.ReferenceNode(owner), IChildList<Hero>, IHeroHolder
{
    /// <summary>The heroes of this library, in their stored order.</summary>
    public HeroCollection Heroes => new(this);

    Hero IOwner<Hero>.this[int index] => Heroes[index];

    // A library takes heroes only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Hero;

    int IChildSink<Hero>.AddChild(Hero child) => AddChild(child);

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
    public override int AddChild(EngineObject? child)
    {
        CheckChild(child);
        // CheckChild refuses a non-hero. The cast names the child the library id is written on.
        Hero? hero = (Hero?)child;
        // { Original checks unit IDs rather than library IDs for a collision. }
        if (hero.LibraryId == 0 || Heroes.FindHero(hero.LibraryId) is not null)
        {
            hero.LibraryId = CreateUniqueLibraryId();
        }

        _ = AppendChild(child);
        return child.AddRef() == -1 ? throw new NotSupportedException("EObject doesn't support ReferenceCounting") : Heroes.Count - 1;
    }

    // Hero.PAS:2580
    public int CreateUniqueLibraryId()
    {
        do
        {
            _nextLibraryId += 1;
            if (_nextLibraryId > 268435455)
            {
                _nextLibraryId = 0;
            }
        }
        while (Heroes.FindIndex(_nextLibraryId, match: static (sought, hero) => hero.LibraryId == sought) >= 0);
        return _nextLibraryId;
    }

    // Hero.PAS:2602
    public Hero CreateHero()
    {
        Hero hero = new(null) { Holder = CreationHolder() };
        hero.SetHeroResourceIndex(0);
        hero.LibraryId = CreateUniqueLibraryId();
        hero.LibraryName = LibraryName;
        hero.SetOwner(this);
        _ = hero.Release();
        return hero;
    }

    // Hero.PAS:2614
    public string GetName()
    {
        string result = LibraryName;
        return IsEmpty(result) ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SystemText) : result;
    }

    // Hero.PAS:2620
    public Hero? FindLibraryId(int id) => Heroes.FindLast(item => id == item.LibraryId);

    // Hero.PAS:2633
    public void LoadFromFile(string fileName) => GetEngine().ReadFromFile(fileName, this, progress: null, profile: GameProfile.HeroList);

    // Hero.PAS:2636
    public void SaveToFile(string fileName) => GetEngine().WriteToFile(fileName, this, progress: null, profile: GameProfile.HeroList);

    Hero IReadOnlyList<Hero>.this[int index] => Heroes[index];
}
