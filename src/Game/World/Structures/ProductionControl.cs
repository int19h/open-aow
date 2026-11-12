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
        Settings = new ProductionSettings(null);
        UnitProductions = new UnitProductionList(null);
        Queue = new ProductionQueue(null);
        Queue.ProductionControl = this;
        Status = ProductionControlStatus.Idle;
        Clear();
    }

    private NotifyEvent<object> _onChange;

    public int UpdateCount { get; private set; }
    public ProductionQueue Queue { get; }

    public bool LoopQueue;

    public UnitProductionList UnitProductions { get; }

    public ProductionControlStatus Status;

    public ProductionSettings Settings { get; }

    // Structur.PAS:202
    public override void Dispose()
    {
        Settings?.Dispose();
        UnitProductions?.Dispose();
        Queue?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:783
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Queue, 20);
        stream.rwEObject(UnitProductions, 21);
        stream.rwEObject(Settings, 22);
        stream.rwData(Bytes.Of(ref Status, 1), 1, 23);
        stream.rwBool(ref LoopQueue, 24);
    }

    // Structur.PAS:261
    public virtual int CancelProductionReturnGold()
    {
        int result;
        if (Status == ProductionControlStatus.Producing)
        {
            if (this.Settings.TurnsTotal != 0)
            {
                result = Round((double)(this.Settings.TurnsLeft * this.Settings.Gold) / this.Settings.TurnsTotal);
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Structur.PAS:270
    public virtual int CancelProductionReturnMana()
    {
        int result;
        if (Status == ProductionControlStatus.Producing)
        {
            if (this.Settings.TurnsTotal != 0)
            {
                result = Round((double)(this.Settings.TurnsLeft * this.Settings.Mana) / this.Settings.TurnsTotal);
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Structur.PAS:295
    public virtual void ExecuteCancelProduction()
    {
        sbyte player;
        if (Status == ProductionControlStatus.Producing)
        {
            Queue.GetItem(0).Order.Cancel(this);
            if (Settings.GroupId != 0)
            {
                UnregisterGroupProduction();
            }

            player = GetPlayer();
            if (player > -1)
            {
                var players = AoWHSMap.Current.Players.GetPlayers(player);
                var cancelProductionReturnGold = CancelProductionReturnGold();
                AoWHSMap.Current.Players.GetPlayers(player).SetGems(players.Gems + cancelProductionReturnGold);
                var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(player);
                var cancelProductionReturnMana = CancelProductionReturnMana();
                AoWHSMap.Current.Players.GetPlayers(player).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + cancelProductionReturnMana);
            }

            Settings.Clear();
            Status = ProductionControlStatus.Waiting;
            Update();
        }
    }

    // Structur.PAS:483
    public virtual bool ExecuteAIProduction(int index, int groupId)
    {
        ProductionItem item;
        ExecuteDeleteProductions();
        item = new ProductionItem(null);
        item.Order = new ProduceUnit(index, groupId);
        ExecuteAddProduction(item);
        item?.Dispose();
        return true;
    }

    // Structur.PAS:497
    public virtual bool ExecuteAIResearch(int index)
    {
        bool result;
        ProductionItem item;
        if (!UnitProductions.GetItem(index).Researched)
        {
            ExecuteDeleteProductions();
            item = new ProductionItem(null);
            item.Order = new ResearchSpell(index);
            ExecuteAddProduction(item);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Structur.PAS:392
    public virtual bool ExecuteProduction(ProductionItem item)
    {
        bool result;
        string reason = "";
        if (CanExecuteProduction(item, ref reason))
        {
            BeginUpdate();
            ExecuteCancelProduction();
            Status = ProductionControlStatus.Waiting;
            item.Order.Execute(this);
            EndUpdate();
            item.Order.Executed(this);
            result = true;
        }
        else
        {
            Status = ProductionControlStatus.Waiting;
            result = false;
        }

        return result;
    }

    // Structur.PAS:450
    public virtual void ExecuteAddProduction(ProductionItem item)
    {
        ProductionItem newItem;
        BeginUpdate();
        newItem = (ProductionItem)item.Copy(GameTables.GameEngine);
        newItem.SetOwner(Queue);
        if (Queue.Count == 1)
        {
            ExecuteProduction(newItem);
        }

        EndUpdate();
    }

    // Structur.PAS:461
    public virtual void ExecuteDeleteProduction(int index)
    {
        BeginUpdate();
        if (index == 0)
        {
            ExecuteCancelProduction();
            Queue.GetItem(index)?.Dispose();
            if (Queue.Count != 0)
            {
                ExecuteProduction(Queue.GetItem(0));
            }
            else
            {
                Status = ProductionControlStatus.Idle;
            }
        }
        else
        {
            Queue.GetItem(index)?.Dispose();
        }

        EndUpdate();
    }

    // Structur.PAS:475
    public virtual void ExecuteDeleteProductions()
    {
        BeginUpdate();
        ExecuteCancelProduction();
        Queue.Clear();
        EndUpdate();
    }

    // Structur.PAS:514
    public virtual ExecuteResult ExecuteProductionTokenEvent(ProductionTokenEvent eventValue)
    {
        ExecuteResult result;
        switch (eventValue.Command)
        {
            case 0:
                BeginUpdate();
                if (!eventValue.Queue)
                {
                    ExecuteDeleteProductions();
                }

                ExecuteAddProduction(eventValue.Item);
                result = ExecuteResult.Done;
                EndUpdate();
                break;
            case 1:
                ExecuteDeleteProduction(eventValue.Index);
                result = ExecuteResult.Done;
                break;
            case 2:
                ExecuteDeleteProductions();
                result = ExecuteResult.Done;
                break;
            case 3:
                if (Queue.Count != 0 && Status == ProductionControlStatus.Waiting)
                {
                    ExecuteProduction(Queue.GetItem(0));
                }

                result = ExecuteResult.Done;
                break;
            case 4:
                LoopQueue = eventValue.Index == 1;
                Update();
                result = ExecuteResult.Done;
                break;
            default:
                result = ExecuteResult.Done;
                break;
        }

        return result;
    }

    // Structur.PAS:551
    public virtual ProductionTokenEvent CreateProductionTokenEvent()
    {
        ProductionTokenEvent eventValue;
        eventValue = new ProductionTokenEvent(null);
        GetLocation(ref eventValue.X, ref eventValue.Y, ref eventValue.MapLevel);
        eventValue.Player = GetPlayer();
        return eventValue;
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
        I = Queue.Count;
        bool result = I >= GetQueueSizeMax();
        while (!result && I != 0)
        {
            I -= 1;
            result = LockQueueProduction(Queue.GetItem(I));
        }

        return result;
    }

    // Structur.PAS:837
    public virtual bool LoopableProduction(ProductionItem item) => item.Order.Loopable(this);

    // Structur.PAS:834
    public virtual bool LockQueueProduction(ProductionItem item) => item.Order.LocksQueue(this);

    // Structur.PAS:561
    public virtual bool UnitProduced() => false;

    // Structur.PAS:636
    public virtual void UnitResearched()
    {
        UnitProductions.GetItem(Settings.UnitProductionIndex).Researched = true;
    }

    // Structur.PAS:219
    public virtual void Update()
    {
        if (UpdateCount == 0)
        {
            if (_onChange != null)
            {
                _onChange(this);
            }
        }
    }

    // Structur.PAS:564
    public virtual Unit CreateUnitProduction(int index)
    {
        Unit result;
        Unit unit;
        if (UnitProductions.GetItem(index) != null)
        {
            unit = new Unit(null);
            var unitProduction = UnitProductions.GetItem(index);
            unit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
            result = unit;
        }
        else
        {
            result = null;
        }

        return result;
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
    public virtual void NewTurn(sbyte player)
    {
        if (Status == ProductionControlStatus.Waiting && player == GetPlayer())
        {
            if (Queue.Count != 0)
            {
                ExecuteProduction(Queue.GetItem(0));
            }
            else
            {
                Status = ProductionControlStatus.Idle;
            }
        }
        else
        {
            if (Status == ProductionControlStatus.Producing && player == Settings.Player)
            {
                if (Settings.TurnsLeft > 0)
                {
                    Settings.TurnsLeft = (sbyte)(Settings.TurnsLeft - 1);
                }

                if (Settings.TurnsLeft == 0)
                {
                    Queue.GetItem(0).Order.Complete(this, player);
                }
            }
        }

        // City.pas:496, PrdPlace.pas:231
        if (Status == ProductionControlStatus.Producing && player == Settings.Player && Settings.TurnsLeft == 0)
        {
            Queue.GetItem(0).Order.Completed(this);
        }
    }

    // Structur.PAS:347
    public virtual void PlayerChanged()
    {
        for (int I = Queue.Count - 1; I >= 1; I--)
        {
            Queue.GetItem(I)?.Dispose();
        }

        UnregisterGroupProduction();
        LoopQueue = false;
        if (Queue.Count != 0 && Status != ProductionControlStatus.Producing)
        {
            Queue.GetItem(0)?.Dispose();
            Status = ProductionControlStatus.Idle;
        }
    }

    // Structur.PAS:361
    public virtual bool CanExecuteProduction(ProductionItem item, ref string reason) => item.Order.CanExecute(this, ref reason);

    // Structur.PAS:710
    public virtual bool DeleteProduction(int index)
    {
        bool result;
        ProductionTokenEvent eventValue;
        bool condition = index >= 0 && index < Queue.Count;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 1;
            eventValue.Index = index;
            var player = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:726
    public virtual bool DeleteProductions()
    {
        bool result;
        ProductionTokenEvent eventValue;
        bool condition = Queue.Count != 0;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 2;
            var player = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:840
    public virtual string GetProductionName(ProductionItem item) => item.Order.Name(this);

    // Structur.PAS:865
    public virtual string GetProductionNameProgressive(ProductionItem item) => item.Order.NameProgressive(this);

    // Structur.PAS:577
    public virtual int UnitProductionGold(int index)
    {
        int unitIndex;
        unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex).ObtainValue;
    }

    // Structur.PAS:590
    public virtual int UnitProductionMana(int index) => 0;

    // Structur.PAS:585
    public virtual int UnitProductionGoldUpkeep(int index)
    {
        var unitProduction = UnitProductions.GetItem(index);
        return (AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level + 1) * 2;
    }

    // Structur.PAS:593
    public virtual int UnitProductionManaUpkeep(int index) => 0;

    // Structur.PAS:596
    public virtual int UnitProductionTime(int index)
    {
        int unitIndex;
        unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex).Level;
    }

    // Structur.PAS:604
    public virtual bool CanProduceUnit(int index, bool queue, ref string reason) => true;

    // Structur.PAS:607
    public virtual bool ProduceUnit(int index, int groupId, bool queue)
    {
        bool result;
        ProductionTokenEvent eventValue;
        string reason = "";
        bool condition = CanProduceUnit(index, queue, ref reason);
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = new ProductionItem(null);
            // { explicit item receiver }
            eventValue.Item.Order = new ProduceUnit(index, groupId);

            var player = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:631
    public virtual bool ProduceUnit(int index, bool queue) => ProduceUnit(index, 0, queue);

    // Structur.PAS:641
    public virtual int UnitResearchGold(int index)
    {
        int unitIndex;
        unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex).ObtainValue;
    }

    // Structur.PAS:649
    public virtual int UnitResearchMana(int index) => 0;

    // Structur.PAS:652
    public virtual int UnitResearchTime(int index)
    {
        int unitIndex;
        unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex).Level;
    }

    // Structur.PAS:660
    public virtual bool CanResearchUnit(int index, bool queue, ref string reason)
    {
        bool result;
        if (!queue || Queue.IndexOf(new ResearchSpell(index)) == -1)
        {
            result = true;
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(s_actionAlreadyQueuedText);
        }

        return result;
    }

    // Structur.PAS:670
    public virtual bool ResearchUnit(int index, bool queue)
    {
        bool result;
        ProductionTokenEvent eventValue;
        string reason = "";
        bool condition = CanResearchUnit(index, queue, ref reason);
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = new ProductionItem(null);
            // { explicit item receiver }
            eventValue.Item.Order = new ResearchSpell(index);

            var player = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            // { Original returns False after successfully queueing research. }
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Structur.PAS:210
    public void BeginUpdate()
    {
        UpdateCount += 1;
    }

    // Structur.PAS:213
    public void EndUpdate()
    {
        UpdateCount -= 1;
        if (UpdateCount == 0)
        {
            Update();
        }
    }

    // Structur.PAS:316
    public void RegisterGroupProduction()
    {
        AIGroup group;
        if (Status == ProductionControlStatus.Producing && Settings.GroupId != 0)
        {
            group = AoWHSMap.Current.AIGroupManager.FindId(Settings.GroupId);
            if (group != null)
            {
                group.Productions.Add(this, Settings.Unit);
            }
        }
    }

    // Structur.PAS:327
    public void UnregisterGroupProduction()
    {
        AIGroup group;
        if (Status == ProductionControlStatus.Producing && Settings.GroupId != 0)
        {
            group = AoWHSMap.Current.AIGroupManager.FindId(Settings.GroupId);
            if (group != null)
            {
                group.Productions.DeleteUnit(Settings.Unit);
            }

            Settings.GroupId = 0;
        }
    }

    // Structur.PAS:339
    public void GroupProductionProduced(Troop troop, int groupId)
    {
        AIGroup group;
        var player = GetPlayer();
        group = AoWHSMap.Current.Players.GetPlayers(player).AIGroups.FindId(groupId);
        if (group != null)
        {
            group.UnitProduced(this, troop);
        }
    }

    // Structur.PAS:693
    public bool Produce(ProductionItem item, bool queue)
    {
        bool result;
        ProductionTokenEvent eventValue;
        var player = GetPlayer();
        if (!AoWHSMap.Current.Players.GetPlayers(player).GetBusy())
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = (ProductionItem)item.Copy(GameTables.GameEngine);
            var player2 = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player2).AddEvent(eventValue);
            eventValue.Release();
            // { Original returns False after successfully queueing production. }
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Structur.PAS:741
    public bool RetryProductionExecution(ref string reason)
    {
        bool result;
        ProductionTokenEvent eventValue;
        bool condition = Queue.Count != 0 && Status == ProductionControlStatus.Waiting;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            if (CanExecuteProduction(Queue.GetItem(0), ref reason))
            {
                eventValue = CreateProductionTokenEvent();
                eventValue.Command = 3;
                var player = GetPlayer();
                AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                eventValue.Release();
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:760
    public bool SetLoopQueueTo(bool value)
    {
        bool result;
        ProductionTokenEvent eventValue;
        bool condition = value != LoopQueue;
        if (condition)
        {
            var player = GetPlayer();
            condition = !AoWHSMap.Current.Players.GetPlayers(player).GetBusy();
        }

        if (condition)
        {
            eventValue = CreateProductionTokenEvent();
            eventValue.Command = 4;
            if (value)
            {
                eventValue.Index = 1;
            }
            else
            {
                eventValue.Index = 0;
            }

            var player = GetPlayer();
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:793
    public void ProductionDone()
    {
        BeginUpdate();
        if (LoopQueue && LoopableProduction(Queue.GetItem(0)))
        {
            var count = Queue.Count;
            Queue.MoveChild(0, count - 1);
        }
        else
        {
            Queue.GetItem(0)?.Dispose();
        }

        if (Queue.Count != 0)
        {
            ExecuteProduction(Queue.GetItem(0));
        }
        else
        {
            Status = ProductionControlStatus.Idle;
        }

        EndUpdate();
    }

    // Structur.PAS:890
    public int FindOptimalResearchProduction(CombatPredictor predictor, int gold)
    {
        int result;
        Player Player;
        Unit unit;
        DoubleItemList productions;
        int priority;
        int minimum;
        bool include;
        var player = GetPlayer();
        Player = AoWHSMap.Current.Players.GetPlayers(player);
        unit = new Unit(null);
        productions = new DoubleItemList();
        for (int I = UnitProductions.Count - 1; I >= 0; I--)
        {
            if (!UnitProductions.GetItem(I).Researched)
            {
                include = UnitResearchGold(I) <= gold;
                if (predictor != null)
                {
                    var unitProduction = UnitProductions.GetItem(I);
                    unit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
                    priority = predictor.Usefullness(unit);
                    // { Original replaces the affordability check here. }
                    include = (priority & 8388608) != 0;
                }
                else
                {
                    priority = RosterTables.GetUnitIndexAIPriority(UnitProductions.GetItem(I).UnitIndex, Player.PlayerIndex, 0);
                }

                if (include)
                {
                    productions.AddAbility(I, priority);
                }
            }
        }

        if (productions.Count != 0)
        {
            productions.SortDescendingItemB();
            result = 0;
            minimum = Round((double)productions[0].Second * 0.8);
            while (result + 1 < productions.Count && productions[result + 1].Second >= minimum)
            {
                result += 1;
            }

            if (AoWHSMap.Current.Random(2) == 1)
            {
                result = 0;
            }
            else
            {
                result = AoWHSMap.Current.Random(result + 1);
            }

            result = productions[result].First;
        }
        else
        {
            result = -1;
        }

        productions?.Dispose();
        unit?.Dispose();
        return result;
    }

    private static readonly string s_actionAlreadyQueuedText = "Action already queued";

    // intf_P1_02.inc:120

    // intf_P1_02.inc:120

}

public enum ProductionControlStatus : byte
{
    Idle = 0, Waiting = 1, Producing = 2
}
