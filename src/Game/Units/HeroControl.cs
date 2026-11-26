#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Units;

// Changed. UpdateCount. HeroChanged. Heroes
public sealed class HeroControl : EngineObject
{
    // Hero.PAS:2413
    public HeroControl(CustomNode? owner) : base(owner) => _heroes = new HeroList(null) { Holder = CreationHolder() };

    public event NotifyEvent<HeroControl>? Changed;

    private int _updateCount;

    public event NotifyEvent<Hero>? HeroChanged;

    private readonly HeroList _heroes;

    // Hero.PAS:2421
    public override void Dispose()
    {
        BeginUpdate();
        for (int i = _heroes.GetCount() - 1; i >= 0; i--)
        {
            UnregisterHero(_heroes[i]);
        }

        _heroes?.Dispose();
        HeroChanged = null;
        Changed = null;
        base.Dispose();
    }

    // Hero.PAS:2435
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Heroes", objectValue: _heroes);
        if (persister.Loading)
        {
            for (int i = _heroes.GetCount() - 1; i >= 0; i--)
            {
                _heroes.GetHero(i).Registered = true;
            }
        }
    }

    // Hero.PAS:2567
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
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
                for (int i = _heroes.GetCount() - 1; i >= 0; i--)
                {
                    if (_heroes[i].Owner is null)
                    {
                        _heroes[i].MessageProc(ref message, sender);
                    }
                }
            }
        }
    }

    // Hero.PAS:2432
    public void NotifyChanged()
    {
        if (_updateCount == 0)
        {
            NotifyEvent<HeroControl>? handlers = Changed;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Changed));
            handlers?.Invoke(this);
        }
    }

    // Hero.PAS:2444
    public void BeginUpdate() => _updateCount += 1;

    // Hero.PAS:2447
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            NotifyChanged();
        }
    }

    // Hero.PAS:2450
    public void UpdateLibraryHeroes()
    {
        BeginUpdate();
        for (int i = _heroes.GetCount() - 1; i >= 0; i--)
        {
            _heroes[i].UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Hero.PAS:2458
    public Hero this[int index] => _heroes[index];

    // Hero.PAS:2461
    public int GetCount() => _heroes.GetCount();

    // Hero.PAS:2464
    public Hero FindId(int id) => _heroes.FindHero(id);

    // Hero.PAS:2467
    public int IndexOf(Hero hero) => _heroes.IndexOfChild(hero);

    // Hero.PAS:2470
    public int IndexOfLibrary(string libraryName, int libraryId)
    {
        int result = _heroes.GetCount();
        while (result != 0)
        {
            result -= 1;
            Hero hero = _heroes[result];
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
        for (int i = _heroes.GetCount() - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (hero is not null && hero.LibraryId != -1 && GetEngine().GamePack.HeroLibraries.FindHero(hero.LibraryName, hero.LibraryId) is null)
            {
                if (hero.Owner is null)
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
        for (int i = _heroes.GetCount() - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (!hero.UnitStatus.Contains(UnitStatuses.Activated) && hero.Owner is null)
            {
                hero.Activate();
            }
        }
    }

    // Hero.PAS:2505
    public void Deactivate()
    {
        for (int i = _heroes.GetCount() - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (hero.UnitStatus.Contains(UnitStatuses.Activated) && hero.Owner is null)
            {
                hero.Deactivate();
            }
        }
    }

    // Hero.PAS:2515
    public bool RegisterHero(Hero hero)
    {
        if (!hero.Registered)
        {
            hero.Registered = true;
            _ = _heroes.AddChild(hero);
            NotifyChanged();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:2522
    public void UnregisterHero(Hero hero)
    {
        if (hero.Registered)
        {
            hero.Registered = false;
            _ = _heroes.RemoveChild(hero);
            NotifyChanged();
        }
    }

    // Hero.PAS:2528
    public void NotifyHeroChanged(Hero hero)
    {
        if (!GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            NotifyEvent<Hero>? handlers = HeroChanged;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(HeroChanged));
            handlers?.Invoke(hero);
        }
    }

    // Hero.PAS:2531
    public int GetAverageFreeHeroLevel()
    {
        int result = 0;
        int count = 0;
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (!hero.HeroStatus.Contains(HeroStatuses.Died) && PlayerIdExtensions.ToOriginal(hero.Player) <= 0)
            {
                result += hero.GetLevel();
                count += 1;
            }
        }

        return count != 0 ? result / count : 0;
    }

    // Hero.PAS:2545
    public int GetMinFreeHeroLevel()
    {
        int result = 30;
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (!hero.HeroStatus.Contains(HeroStatuses.Died) && PlayerIdExtensions.ToOriginal(hero.Player) <= 0 && result > hero.GetLevel())
            {
                result = hero.GetLevel();
            }
        }

        return result;
    }

    // Hero.PAS:2557
    public void ListDeadHeroes(HeroList list)
    {
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Hero hero = _heroes[i];
            if (hero.HeroStatus.Contains(HeroStatuses.Died))
            {
                _ = list.AddChild(hero);
            }
        }
    }
}
