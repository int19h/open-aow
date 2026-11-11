// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// change event, update count, queue, loop queue, unit productions, status, settings
public partial class ProductionControl : EngineObject
{

    // Structur.PAS:191
    public ProductionControl(ECustomNode owner) : base(owner)
    {
        FSettings = new ProductionSettings(null);
        FUnitProductions = new UnitProductionList(null);
        FQueue = new ProductionQueue(null);
        FQueue.FProductionControl = this;
        FStatus = Status.Idle;
        Clear();
    }

    public NotifyEvent<object> FOnChange;
    public int FUpdateCount;
    public ProductionQueue FQueue;
    public bool FLoopQueue;
    public UnitProductionList FUnitProductions;
    public Status FStatus;
    public ProductionSettings FSettings;

    // Structur.PAS:202
    public override void Dispose()
    {
        FSettings?.Dispose();
        FUnitProductions?.Dispose();
        FQueue?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:783
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FQueue, 20);
        Stream.rwEObject(FUnitProductions, 21);
        Stream.rwEObject(FSettings, 22);
        Stream.rwData(Bytes.Of(ref FStatus, 1), 1, 23);
        Stream.rwBool(ref FLoopQueue, 24);
    }

    // Structur.PAS:261
    public virtual int CancelProductionReturnGold()
    {
        int Result;
        if (FStatus == Status.Producing)
        {
            if (this.FSettings.FTurnsTotal != 0)
            {
                Result = Round((double)(this.FSettings.FTurnsLeft * this.FSettings.FGold) / this.FSettings.FTurnsTotal);
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Structur.PAS:270
    public virtual int CancelProductionReturnMana()
    {
        int Result;
        if (FStatus == Status.Producing)
        {
            if (this.FSettings.FTurnsTotal != 0)
            {
                Result = Round((double)(this.FSettings.FTurnsLeft * this.FSettings.FMana) / this.FSettings.FTurnsTotal);
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Structur.PAS:295
    public virtual void ExecuteCancelProduction()
    {
        sbyte Player;
        if (FStatus == Status.Producing)
        {
            FQueue.GetItem(0).Order.Cancel(this);
            if (FSettings.FGroupID != 0)
            {
                UnregisterGroupProduction();
            }

            Player = GetPlayer();
            if (Player > -1)
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                var cancelProductionReturnGold = CancelProductionReturnGold();
                AoWHSMap.Current.FPlayers.GetPlayers(Player).SetGems(players.FGems + cancelProductionReturnGold);
                var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                var cancelProductionReturnMana = CancelProductionReturnMana();
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana + cancelProductionReturnMana);
            }

            FSettings.Clear();
            FStatus = Status.Waiting;
            Update();
        }
    }

    // Structur.PAS:483
    public virtual bool ExecuteAIProduction(int Index, int GroupID)
    {
        ProductionItem Item;
        ExecuteDeleteProductions();
        Item = new ProductionItem(null);
        Item.Order = new ProduceUnit(Index, GroupID);
        ExecuteAddProduction(Item);
        Item?.Dispose();
        return true;
    }

    // Structur.PAS:497
    public virtual bool ExecuteAIResearch(int Index)
    {
        bool Result;
        ProductionItem Item;
        if (!FUnitProductions.GetItem(Index).FResearched)
        {
            ExecuteDeleteProductions();
            Item = new ProductionItem(null);
            Item.Order = new ResearchSpell(Index);
            ExecuteAddProduction(Item);
            Item?.Dispose();
            Result = true;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Structur.PAS:392
    public virtual bool ExecuteProduction(ProductionItem Item)
    {
        bool Result;
        string Reason = "";
        if (CanExecuteProduction(Item, ref Reason))
        {
            BeginUpdate();
            ExecuteCancelProduction();
            FStatus = Status.Waiting;
            Item.Order.Execute(this);
            EndUpdate();
            Item.Order.Executed(this);
            Result = true;
        }
        else
        {
            FStatus = Status.Waiting;
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:450
    public virtual void ExecuteAddProduction(ProductionItem Item)
    {
        ProductionItem NewItem;
        BeginUpdate();
        NewItem = (ProductionItem)Item.Copy(GameTables.AoWEngine);
        NewItem.SetOwner(FQueue);
        if (FQueue.Count == 1)
        {
            ExecuteProduction(NewItem);
        }

        EndUpdate();
    }

    // Structur.PAS:461
    public virtual void ExecuteDeleteProduction(int Index)
    {
        BeginUpdate();
        if (Index == 0)
        {
            ExecuteCancelProduction();
            FQueue.GetItem(Index)?.Dispose();
            if (FQueue.Count != 0)
            {
                ExecuteProduction(FQueue.GetItem(0));
            }
            else
            {
                FStatus = Status.Idle;
            }
        }
        else
        {
            FQueue.GetItem(Index)?.Dispose();
        }

        EndUpdate();
    }

    // Structur.PAS:475
    public virtual void ExecuteDeleteProductions()
    {
        BeginUpdate();
        ExecuteCancelProduction();
        FQueue.Clear();
        EndUpdate();
    }

    // Structur.PAS:514
    public virtual ExecuteResult ExecuteProductionTE(ProductionTokenEvent Event)
    {
        ExecuteResult Result;
        switch (Event.FCommand)
        {
            case 0:
                BeginUpdate();
                if (!Event.FQueue)
                {
                    ExecuteDeleteProductions();
                }

                ExecuteAddProduction(Event.FItem);
                Result = ExecuteResult.Done;
                EndUpdate();
                break;
            case 1:
                ExecuteDeleteProduction(Event.FIndex);
                Result = ExecuteResult.Done;
                break;
            case 2:
                ExecuteDeleteProductions();
                Result = ExecuteResult.Done;
                break;
            case 3:
                if (FQueue.Count != 0 && FStatus == Status.Waiting)
                {
                    ExecuteProduction(FQueue.GetItem(0));
                }

                Result = ExecuteResult.Done;
                break;
            case 4:
                FLoopQueue = Event.FIndex == 1;
                Update();
                Result = ExecuteResult.Done;
                break;
            default:
                Result = ExecuteResult.Done;
                break;
        }

        return Result;
    }

    // Structur.PAS:551
    public virtual ProductionTokenEvent CreateProductionTE()
    {
        ProductionTokenEvent Event;
        Event = new ProductionTokenEvent(null);
        GetLocation(ref Event.FX, ref Event.FY, ref Event.FL);
        Event.FPlayer = GetPlayer();
        return Event;
    }

    // Structur.PAS:776
    public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // intf_P1_02.inc:133
    public virtual sbyte GetPlayer()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Structur.PAS:279
    public virtual int GetQueueSizeMax() => 5;

    // Structur.PAS:282
    public virtual bool GetQueueLocked()
    {
        int I;
        I = FQueue.Count;
        bool Result = I >= GetQueueSizeMax();
        while (!Result && I != 0)
        {
            I -= 1;
            Result = LockQueueProduction(FQueue.GetItem(I));
        }

        return Result;
    }

    // Structur.PAS:837
    public virtual bool LoopableProduction(ProductionItem Item) => Item.Order.Loopable(this);

    // Structur.PAS:834
    public virtual bool LockQueueProduction(ProductionItem Item) => Item.Order.LocksQueue(this);

    // Structur.PAS:561
    public virtual bool UnitProduced() => false;

    // Structur.PAS:636
    public virtual void UnitResearched()
    {
        FUnitProductions.GetItem(FSettings.FUnitProductionIndex).FResearched = true;
    }

    // Structur.PAS:219
    public virtual void Update()
    {
        if (FUpdateCount == 0)
        {
            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }
    }

    // Structur.PAS:564
    public virtual Unit CreateUnitProduction(int Index)
    {
        Unit Result;
        Unit AUnit;
        if (FUnitProductions.GetItem(Index) != null)
        {
            AUnit = new Unit(null);
            var unitProduction = FUnitProductions.GetItem(Index);
            AUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex));
            Result = AUnit;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:225
    public virtual void Activate()
    {
        RegisterGroupProduction();
    }

    // Structur.PAS:228
    public virtual void Deactivate()
    {
        UnregisterGroupProduction();
    }

    // Structur.PAS:804
    public virtual void NewTurn(sbyte Player)
    {
        if (FStatus == Status.Waiting && Player == GetPlayer())
        {
            if (FQueue.Count != 0)
            {
                ExecuteProduction(FQueue.GetItem(0));
            }
            else
            {
                FStatus = Status.Idle;
            }
        }
        else
        {
            if (FStatus == Status.Producing && Player == FSettings.FPlayer)
            {
                if (FSettings.FTurnsLeft > 0)
                {
                    FSettings.FTurnsLeft = (sbyte)(FSettings.FTurnsLeft - 1);
                }

                if (FSettings.FTurnsLeft == 0)
                {
                    FQueue.GetItem(0).Order.Complete(this, Player);
                }
            }
        }

        // City.pas:496, PrdPlace.pas:231
        if (FStatus == Status.Producing && Player == FSettings.FPlayer && FSettings.FTurnsLeft == 0)
        {
            FQueue.GetItem(0).Order.Completed(this);
        }
    }

    // Structur.PAS:347
    public virtual void PlayerChanged()
    {
        for (int I = FQueue.Count - 1; I >= 1; I--)
        {
            FQueue.GetItem(I)?.Dispose();
        }

        UnregisterGroupProduction();
        FLoopQueue = false;
        if (FQueue.Count != 0 && FStatus != Status.Producing)
        {
            FQueue.GetItem(0)?.Dispose();
            FStatus = Status.Idle;
        }
    }

    // Structur.PAS:361
    public virtual bool CanExecuteProduction(ProductionItem Item, ref string Reason) => Item.Order.CanExecute(this, ref Reason);

    // Structur.PAS:710
    public virtual bool DeleteProduction(int Index)
    {
        bool Result;
        ProductionTokenEvent Event;
        bool condition = Index >= 0 && Index < FQueue.Count;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            Event = CreateProductionTE();
            Event.FCommand = 1;
            Event.FIndex = Index;
            var player = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:726
    public virtual bool DeleteProductions()
    {
        bool Result;
        ProductionTokenEvent Event;
        bool condition = FQueue.Count != 0;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            Event = CreateProductionTE();
            Event.FCommand = 2;
            var player = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:840
    public virtual string GetProductionName(ProductionItem Item) => Item.Order.Name(this);

    // Structur.PAS:865
    public virtual string GetProductionNameProgressive(ProductionItem Item) => Item.Order.NameProgressive(this);

    // Structur.PAS:577
    public virtual int UnitProductionGold(int Index)
    {
        int UnitIndex;
        UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
        return AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FObtainValue;
    }

    // Structur.PAS:590
    public virtual int UnitProductionMana(int Index) => 0;

    // Structur.PAS:585
    public virtual int UnitProductionGoldUpkeep(int Index)
    {
        var unitProduction = FUnitProductions.GetItem(Index);
        return (AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).FLevel + 1) * 2;
    }

    // Structur.PAS:593
    public virtual int UnitProductionManaUpkeep(int Index) => 0;

    // Structur.PAS:596
    public virtual int UnitProductionTime(int Index)
    {
        int UnitIndex;
        UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
        return AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel;
    }

    // Structur.PAS:604
    public virtual bool CanProduceUnit(int Index, bool Queue, ref string Reason) => true;

    // Structur.PAS:607
    public virtual bool ProduceUnitEx(int Index, int GroupID, bool Queue)
    {
        bool Result;
        ProductionTokenEvent Event;
        string Reason = "";
        bool condition = CanProduceUnit(Index, Queue, ref Reason);
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            Event = CreateProductionTE();
            Event.FCommand = 0;
            Event.FQueue = Queue;
            Event.FItem = new ProductionItem(null);
            // { explicit item receiver }
            Event.FItem.Order = new ProduceUnit(Index, GroupID);

            var player = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:631
    public virtual bool ProduceUnit(int Index, bool Queue) => ProduceUnitEx(Index, 0, Queue);

    // Structur.PAS:641
    public virtual int UnitResearchGold(int Index)
    {
        int UnitIndex;
        UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
        return AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FObtainValue;
    }

    // Structur.PAS:649
    public virtual int UnitResearchMana(int Index) => 0;

    // Structur.PAS:652
    public virtual int UnitResearchTime(int Index)
    {
        int UnitIndex;
        UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
        return AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel;
    }

    // Structur.PAS:660
    public virtual bool CanResearchUnit(int Index, bool Queue, ref string Reason)
    {
        bool Result;
        if (!Queue || FQueue.IndexOf(new ResearchSpell(Index)) == -1)
        {
            Result = true;
        }
        else
        {
            Result = false;
            Reason = AbilityRules.TranslateRStr(ActionAlreadyQueuedRStr);
        }

        return Result;
    }

    // Structur.PAS:670
    public virtual bool ResearchUnit(int Index, bool Queue)
    {
        bool Result;
        ProductionTokenEvent Event;
        string Reason = "";
        bool condition = CanResearchUnit(Index, Queue, ref Reason);
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            Event = CreateProductionTE();
            Event.FCommand = 0;
            Event.FQueue = Queue;
            Event.FItem = new ProductionItem(null);
            // { explicit item receiver }
            Event.FItem.Order = new ResearchSpell(Index);

            var player = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
            Event.Release();
            // { Original returns False after successfully queueing research. }
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Structur.PAS:210
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Structur.PAS:213
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Update();
        }
    }

    // Structur.PAS:316
    public void RegisterGroupProduction()
    {
        AIGroup Group;
        if (FStatus == Status.Producing && FSettings.FGroupID != 0)
        {
            Group = AoWHSMap.Current.FAIGroupManager.FindID(FSettings.FGroupID);
            if (Group != null)
            {
                Group.FProductions.Add(this, FSettings.FUnit);
            }
        }
    }

    // Structur.PAS:327
    public void UnregisterGroupProduction()
    {
        AIGroup Group;
        if (FStatus == Status.Producing && FSettings.FGroupID != 0)
        {
            Group = AoWHSMap.Current.FAIGroupManager.FindID(FSettings.FGroupID);
            if (Group != null)
            {
                Group.FProductions.DeleteUnit(FSettings.FUnit);
            }

            FSettings.FGroupID = 0;
        }
    }

    // Structur.PAS:339
    public void GroupProductionProduced(Troop AUnit, int GroupID)
    {
        AIGroup Group;
        var player = GetPlayer();
        Group = AoWHSMap.Current.FPlayers.GetPlayers(player).FAIGroups.FindID(GroupID);
        if (Group != null)
        {
            Group.UnitProduced(this, AUnit);
        }
    }

    // Structur.PAS:693
    public bool Produce(ProductionItem Item, bool Queue)
    {
        bool Result;
        ProductionTokenEvent Event;
        var player = GetPlayer();
        if (!AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy())
        {
            Event = CreateProductionTE();
            Event.FCommand = 0;
            Event.FQueue = Queue;
            Event.FItem = (ProductionItem)Item.Copy(GameTables.AoWEngine);
            var player2 = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player2).AddEvent(Event);
            Event.Release();
            // { Original returns False after successfully queueing production. }
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Structur.PAS:741
    public bool RetryProductionExecution(ref string Reason)
    {
        bool Result;
        ProductionTokenEvent Event;
        bool condition = FQueue.Count != 0 && FStatus == Status.Waiting;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            if (CanExecuteProduction(FQueue.GetItem(0), ref Reason))
            {
                Event = CreateProductionTE();
                Event.FCommand = 3;
                var player = GetPlayer();
                AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:760
    public bool SetLoopQueueTo(bool Value)
    {
        bool Result;
        ProductionTokenEvent Event;
        bool condition = Value != FLoopQueue;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.FPlayers.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            Event = CreateProductionTE();
            Event.FCommand = 4;
            if (Value)
            {
                Event.FIndex = 1;
            }
            else
            {
                Event.FIndex = 0;
            }

            var player = GetPlayer();
            AoWHSMap.Current.FTokenManager.GetTokenControl(player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:793
    public void ProductionDone()
    {
        BeginUpdate();
        if (FLoopQueue && LoopableProduction(FQueue.GetItem(0)))
        {
            var count = FQueue.Count;
            FQueue.FChildren.Move(0, count - 1);
        }
        else
        {
            FQueue.GetItem(0)?.Dispose();
        }

        if (FQueue.Count != 0)
        {
            ExecuteProduction(FQueue.GetItem(0));
        }
        else
        {
            FStatus = Status.Idle;
        }

        EndUpdate();
    }

    // Structur.PAS:890
    public int FindOptimalResearchProduction(CombatPredictor Predictor, int Gold)
    {
        int Result;
        Player Player;
        Unit AUnit;
        DoubleItemList Productions;
        int Priority;
        int Minimum;
        bool Include;
        var player = GetPlayer();
        Player = AoWHSMap.Current.FPlayers.GetPlayers(player);
        AUnit = new Unit(null);
        Productions = new DoubleItemList();
        for (int I = FUnitProductions.Count - 1; I >= 0; I--)
        {
            if (!FUnitProductions.GetItem(I).FResearched)
            {
                Include = UnitResearchGold(I) <= Gold;
                if (Predictor != null)
                {
                    var unitProduction = FUnitProductions.GetItem(I);
                    AUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex));
                    Priority = Predictor.Usefullness(AUnit);
                    // { Original replaces the affordability check here. }
                    Include = (Priority & 8388608) != 0;
                }
                else
                {
                    Priority = RosterTables.GetUnitIndexAIPriority(FUnitProductions.GetItem(I).FUnitIndex, Player.FPlayerIndex, 0);
                }

                if (Include)
                {
                    Productions.AddAB(I, Priority);
                }
            }
        }

        if (Productions.Count != 0)
        {
            Productions.SortDescendingItemB();
            Result = 0;
            Minimum = Round((double)Productions[0].B * 0.8);
            while (Result + 1 < Productions.Count && Productions[Result + 1].B >= Minimum)
            {
                Result += 1;
            }

            if (AoWHSMap.Current.Random(2) == 1)
            {
                Result = 0;
            }
            else
            {
                Result = AoWHSMap.Current.Random(Result + 1);
            }

            Result = Productions[Result].A;
        }
        else
        {
            Result = -1;
        }

        Productions?.Dispose();
        AUnit?.Dispose();
        return Result;
    }

    private static readonly string ActionAlreadyQueuedRStr = "Action already queued";

    public enum Status : byte
    {
        Idle = 0, Waiting = 1, Producing = 2
    }

    // intf_P1_02.inc:120

    // intf_P1_02.inc:120

}
