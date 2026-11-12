// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FOnChanged. UpdateCount. FOnHeroChanged. Heroes
public partial class HeroControl : EngineObject
{
    // Hero.PAS:2413
    public HeroControl(ECustomNode owner) : base(owner)
    {
        _onChangedStore = new();
        _onHeroChangedStore = new();
        _heroes = new HeroList(null);
    }

    private EventStore<HeroControl> _onChangedStore;

    public event NotifyEvent<HeroControl> FOnChanged
    {
        add => _onChangedStore.Add(value);
        remove => _onChangedStore.Remove(value);
    }

    private int _updateCount;

    private EventStore<Hero> _onHeroChangedStore;

    public event NotifyEvent<Hero> FOnHeroChanged
    {
        add => _onHeroChangedStore.Add(value);
        remove => _onHeroChangedStore.Remove(value);
    }

    private HeroList _heroes;

    // Hero.PAS:2421
    public override void Dispose()
    {
        BeginUpdate();
        for (int I = _heroes.GetCount() - 1; I >= 0; I--)
        {
            UnregisterHero((Hero)_heroes._children.List[I]);
        }

        _heroes?.Dispose();
        _onHeroChangedStore.Clear();
        _onChangedStore.Clear();
        base.Dispose();
    }

    // Hero.PAS:2435
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(_heroes, 20);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = _heroes.GetCount() - 1; I >= 0; I--)
            {
                _heroes.GetHero(I).Registered = true;
            }
        }
    }

    // Hero.PAS:2567
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ValidateLibraryHeroes();
            Activate();
        }
        else
        {
            if (message.MessageId == 536936471)
            {
                Deactivate();
            }
            else
            {
                for (int I = _heroes.GetCount() - 1; I >= 0; I--)
                {
                    if (((EngineObject)_heroes._children.List[I]).Owner == null)
                    {
                        ((EngineObject)_heroes._children.List[I]).MessageProc(ref message, sender);
                    }
                }
            }
        }
    }

    // Hero.PAS:2432
    public void Changed()
    {
        if (_updateCount == 0)
        {
            _onChangedStore.Raise(this);
        }
    }

    // Hero.PAS:2444
    public void BeginUpdate()
    {
        _updateCount += 1;
    }

    // Hero.PAS:2447
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            Changed();
        }
    }

    // Hero.PAS:2450
    public void UpdateLibraryHeroes()
    {
        BeginUpdate();
        for (int I = _heroes.GetCount() - 1; I >= 0; I--)
        {
            ((Hero)_heroes._children.List[I]).UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Hero.PAS:2458
    public Hero GetHero(int index) => (Hero)_heroes._children.List[index];

    // Hero.PAS:2461
    public int GetCount() => _heroes.GetCount();

    // Hero.PAS:2464
    public Hero FindId(int id) => _heroes.FindHero(id);

    // Hero.PAS:2467
    public int IndexOf(Hero hero) => _heroes._children.IndexOf(hero);

    // Hero.PAS:2470
    public int IndexOfLibrary(string libraryName, int libraryId)
    {
        Hero hero;
        int result = _heroes.GetCount();
        while (result != 0)
        {
            result -= 1;
            hero = (Hero)_heroes._children.List[result];
            if (hero.LibraryId == libraryId && CompareText(hero.GetLibraryName(), libraryName) == 0)
            {
                return result;
            }
        }

        return -1;
    }

    // Hero.PAS:2483
    public void ValidateLibraryHeroes()
    {
        Hero hero;
        for (int I = _heroes.GetCount() - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (hero != null && hero.LibraryId != -1 && AoWHSSet.Current.HeroLibraries.FindHero(hero.LibraryName, hero.LibraryId) == null)
            {
                if (hero.Owner == null)
                {
                    hero?.Dispose();
                }
                else
                {
                    hero.LibraryId = -1;
                }
            }
        }
    }

    // Hero.PAS:2495
    public void Activate()
    {
        Hero hero;
        for (int I = _heroes.GetCount() - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (!hero.UnitStatus.Contains(UnitStatusElement.Activated) && hero.Owner == null)
            {
                hero.Activate();
            }
        }
    }

    // Hero.PAS:2505
    public void Deactivate()
    {
        Hero hero;
        for (int I = _heroes.GetCount() - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (hero.UnitStatus.Contains(UnitStatusElement.Activated) && hero.Owner == null)
            {
                hero.Deactivate();
            }
        }
    }

    // Hero.PAS:2515
    public bool RegisterHero(Hero hero)
    {
        bool result;
        if (!hero.Registered)
        {
            hero.Registered = true;
            _heroes.AddChild(hero);
            Changed();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:2522
    public void UnregisterHero(Hero hero)
    {
        if (hero.Registered)
        {
            hero.Registered = false;
            _heroes.RemoveChild(hero);
            Changed();
        }
    }

    // Hero.PAS:2528
    public void HeroChanged(Hero hero)
    {
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            _onHeroChangedStore.Raise(hero);
        }
    }

    // Hero.PAS:2531
    public int GetAverageFreeHeroLevel()
    {
        int count;
        Hero hero;
        int result = 0;
        count = 0;
        for (int I = _heroes._children.Count - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (!hero.HeroStatus.Contains(HeroStatusElement.Died) && hero.Player <= 0)
            {
                result += hero.GetLevel();
                count += 1;
            }
        }

        if (count != 0)
        {
            result = result / count;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Hero.PAS:2545
    public int GetMinFreeHeroLevel()
    {
        Hero hero;
        int result = 30;
        for (int I = _heroes._children.Count - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (!hero.HeroStatus.Contains(HeroStatusElement.Died) && hero.Player <= 0 && result > hero.GetLevel())
            {
                result = hero.GetLevel();
            }
        }

        return result;
    }

    // Hero.PAS:2557
    public void ListDeadHeroes(HeroList list)
    {
        Hero hero;
        for (int I = _heroes._children.Count - 1; I >= 0; I--)
        {
            hero = (Hero)_heroes._children.List[I];
            if (hero.HeroStatus.Contains(HeroStatusElement.Died))
            {
                list.AddChild(hero);
            }
        }
    }

    // intf_P2_07.inc:267

}
