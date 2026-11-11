// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FOnChanged. FUpdateCount. FOnHeroChanged. FHeroes
public partial class HeroControl : EngineObject
{
    // Hero.PAS:2413
    public HeroControl(ECustomNode owner) : base(owner)
    {
        FOnChangedStore = new();
        FOnHeroChangedStore = new();
        FHeroes = new HeroList(null);
    }

    internal EventStore<HeroControl> FOnChangedStore;

    public event NotifyEvent<HeroControl> FOnChanged
    {
        add => FOnChangedStore.Add(value);
        remove => FOnChangedStore.Remove(value);
    }

    public int FUpdateCount;

    internal EventStore<Hero> FOnHeroChangedStore;

    public event NotifyEvent<Hero> FOnHeroChanged
    {
        add => FOnHeroChangedStore.Add(value);
        remove => FOnHeroChangedStore.Remove(value);
    }

    public HeroList FHeroes;

    // Hero.PAS:2421
    public override void Dispose()
    {
        BeginUpdate();
        for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
        {
            UnRegisterHero((Hero)FHeroes.FChildren.List[I]);
        }

        FHeroes?.Dispose();
        FOnHeroChangedStore.Clear();
        FOnChangedStore.Clear();
        base.Dispose();
    }

    // Hero.PAS:2435
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FHeroes, 20);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.SetupCompare, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
            {
                FHeroes.GetHero(I).FRegistered = true;
            }
        }
    }

    // Hero.PAS:2567
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ValidateLibraryHeroes();
            Activate();
        }
        else
        {
            if (Msg.MsgID == 536936471)
            {
                Deactivate();
            }
            else
            {
                for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
                {
                    if (((EngineObject)FHeroes.FChildren.List[I]).Owner == null)
                    {
                        ((EngineObject)FHeroes.FChildren.List[I]).MsgProc(ref Msg, Sender);
                    }
                }
            }
        }
    }

    // Hero.PAS:2432
    public void Changed()
    {
        if (FUpdateCount == 0)
        {
            FOnChangedStore.Raise(this);
        }
    }

    // Hero.PAS:2444
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Hero.PAS:2447
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Hero.PAS:2450
    public void UpdateLibraryHeroes()
    {
        BeginUpdate();
        for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
        {
            ((Hero)FHeroes.FChildren.List[I]).UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Hero.PAS:2458
    public Hero GetHero(int Index) => (Hero)FHeroes.FChildren.List[Index];

    // Hero.PAS:2461
    public int GetCount() => FHeroes.GetCount();

    // Hero.PAS:2464
    public Hero FindID(int ID) => FHeroes.FindHero(ID);

    // Hero.PAS:2467
    public int IndexOf(Hero Hero) => FHeroes.FChildren.IndexOf(Hero);

    // Hero.PAS:2470
    public int IndexOfLibraryEx(string LibraryName, int LibraryID)
    {
        Hero Hero;
        int Result = FHeroes.GetCount();
        while (Result != 0)
        {
            Result -= 1;
            Hero = (Hero)FHeroes.FChildren.List[Result];
            if (Hero.FLibraryID == LibraryID && CompareText(Hero.GetLibraryName(), LibraryName) == 0)
            {
                return Result;
            }
        }

        return -1;
    }

    // Hero.PAS:2483
    public void ValidateLibraryHeroes()
    {
        Hero Hero;
        for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (Hero != null && Hero.FLibraryID != -1 && AoWHSSet.Current.FHeroLibraries.FindHero(Hero.FLibraryName, Hero.FLibraryID) == null)
            {
                if (Hero.Owner == null)
                {
                    Hero?.Dispose();
                }
                else
                {
                    Hero.FLibraryID = -1;
                }
            }
        }
    }

    // Hero.PAS:2495
    public void Activate()
    {
        Hero Hero;
        for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (!Hero.FUnitStatus.Contains(UnitStatusElement.Activated) && Hero.Owner == null)
            {
                Hero.Activate();
            }
        }
    }

    // Hero.PAS:2505
    public void Deactivate()
    {
        Hero Hero;
        for (int I = FHeroes.GetCount() - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (Hero.FUnitStatus.Contains(UnitStatusElement.Activated) && Hero.Owner == null)
            {
                Hero.Deactivate();
            }
        }
    }

    // Hero.PAS:2515
    public bool RegisterHero(Hero Hero)
    {
        bool Result;
        if (!Hero.FRegistered)
        {
            Hero.FRegistered = true;
            FHeroes.AddChild(Hero);
            Changed();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:2522
    public void UnRegisterHero(Hero Hero)
    {
        if (Hero.FRegistered)
        {
            Hero.FRegistered = false;
            FHeroes.RemoveChild(Hero);
            Changed();
        }
    }

    // Hero.PAS:2528
    public void HeroChanged(Hero Hero)
    {
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            FOnHeroChangedStore.Raise(Hero);
        }
    }

    // Hero.PAS:2531
    public int GetAverageFreeHeroLevel()
    {
        int Count;
        Hero Hero;
        int Result = 0;
        Count = 0;
        for (int I = FHeroes.FChildren.Count - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (!Hero.FHeroStatus.Contains(HeroStatusElement.Died) && Hero.FPlayer <= 0)
            {
                Result += Hero.GetLevel();
                Count += 1;
            }
        }

        if (Count != 0)
        {
            Result = Result / Count;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Hero.PAS:2545
    public int GetMinFreeHeroLevel()
    {
        Hero Hero;
        int Result = 30;
        for (int I = FHeroes.FChildren.Count - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (!Hero.FHeroStatus.Contains(HeroStatusElement.Died) && Hero.FPlayer <= 0 && Result > Hero.GetLevel())
            {
                Result = Hero.GetLevel();
            }
        }

        return Result;
    }

    // Hero.PAS:2557
    public void ListDeadHeroes(HeroList List)
    {
        Hero Hero;
        for (int I = FHeroes.FChildren.Count - 1; I >= 0; I--)
        {
            Hero = (Hero)FHeroes.FChildren.List[I];
            if (Hero.FHeroStatus.Contains(HeroStatusElement.Died))
            {
                List.AddChild(Hero);
            }
        }
    }

    // intf_P2_07.inc:267

}
