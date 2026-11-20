#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.World.Structures;

// change event, update count, queue, loop queue, unit productions, status, settings
public partial class ProductionControl : EngineObject
{

    // Structur.PAS:191
    public ProductionControl(CustomNode owner) : base(owner)
    {
        Settings = new ProductionSettings(null) { Holder = CreationHolder() };
        UnitProductions = new UnitProductionList(null) { Holder = CreationHolder() };
        Queue = new ProductionQueue(null)
        {
            Holder = CreationHolder(),
            ProductionControl = this
        };
        Status = ProductionControlStatus.Idle;
        Clear();
    }

    private event NotifyEvent<object> OnChange;

    public int UpdateCount { get; private set; }
    public ProductionQueue Queue { get; }

    public bool LoopQueue { get; private set; }

    public UnitProductionList UnitProductions { get; }

    public ProductionControlStatus Status { get; set; }

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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Queue", objectValue: Queue);
        persister.Object("UnitProductions", objectValue: UnitProductions);
        persister.Object("Settings", objectValue: Settings);
        Status = persister.Enum("Status", value: Status);
        LoopQueue = persister.Boolean("LoopQueue", value: LoopQueue);
    }

    // Structur.PAS:261
    public virtual int CancelProductionReturnGold()
    {
        return Status == ProductionControlStatus.Producing
            ? Settings.TurnsTotal != 0 ? Round((double)(Settings.TurnsLeft * Settings.Gold) / Settings.TurnsTotal) : 0
            : 0;
    }

    // Structur.PAS:270
    public virtual int CancelProductionReturnMana()
    {
        return Status == ProductionControlStatus.Producing
            ? Settings.TurnsTotal != 0 ? Round((double)(Settings.TurnsLeft * Settings.Mana) / Settings.TurnsTotal) : 0
            : 0;
    }

    // Structur.PAS:295
    public virtual void ExecuteCancelProduction()
    {
        if (Status == ProductionControlStatus.Producing)
        {
            Queue.GetItem(0).Order.Cancel(this);
            if (Settings.GroupId != 0)
            {
                UnregisterGroupProduction();
            }

            sbyte player = GetPlayer();
            if (player > -1)
            {
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                int cancelProductionReturnGold = CancelProductionReturnGold();
                StrategicMap.Current.Players.GetPlayers(player).SetGems(players.Gems + cancelProductionReturnGold);
                Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
                int cancelProductionReturnMana = CancelProductionReturnMana();
                StrategicMap.Current.Players.GetPlayers(player).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + cancelProductionReturnMana);
            }

            Settings.Clear();
            Status = ProductionControlStatus.Waiting;
            Update();
        }
    }

    // Structur.PAS:483
    public virtual bool ExecuteAIProduction(int index, int groupId)
    {
        ExecuteDeleteProductions();
        ProductionItem item = new(null)
        {
            Holder = CreationHolder(),
            Order = new ProduceUnit(index, groupId)
        };
        ExecuteAddProduction(item);
        item?.Dispose();
        return true;
    }

    // Structur.PAS:497
    public virtual bool ExecuteAIResearch(int index)
    {
        if (!UnitProductions.GetItem(index).Researched)
        {
            ExecuteDeleteProductions();
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = new ResearchSpell(index)
            };
            ExecuteAddProduction(item);
            item?.Dispose();
            return true;
        }
        else
        {
            return true;
        }
    }

    // Structur.PAS:392
    public virtual bool ExecuteProduction(ProductionItem item)
    {
        string reason = "";
        if (CanExecuteProduction(item, ref reason))
        {
            BeginUpdate();
            ExecuteCancelProduction();
            Status = ProductionControlStatus.Waiting;
            item.Order.Execute(this);
            EndUpdate();
            item.Order.Executed(this);
            return true;
        }
        else
        {
            Status = ProductionControlStatus.Waiting;
            return false;
        }
    }

    // Structur.PAS:450
    public virtual void ExecuteAddProduction(ProductionItem item)
    {
        BeginUpdate();
        ProductionItem newItem = item.Copy<ProductionItem>(GetEngine());
        newItem.SetOwner(Queue);
        if (Queue.Count == 1)
        {
            _ = ExecuteProduction(newItem);
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
                _ = ExecuteProduction(Queue.GetItem(0));
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
        switch (eventValue.Command)
        {
            case 0:
                BeginUpdate();
                if (!eventValue.Queue)
                {
                    ExecuteDeleteProductions();
                }

                ExecuteAddProduction(eventValue.Item);
                EndUpdate();
                break;
            case 1:
                ExecuteDeleteProduction(eventValue.Index);
                return ExecuteResult.Done;
            case 2:
                ExecuteDeleteProductions();
                return ExecuteResult.Done;
            case 3:
                if (Queue.Count != 0 && Status == ProductionControlStatus.Waiting)
                {
                    _ = ExecuteProduction(Queue.GetItem(0));
                }

                return ExecuteResult.Done;
            case 4:
                LoopQueue = eventValue.Index == 1;
                Update();
                return ExecuteResult.Done;
            default:
                return ExecuteResult.Done;
        }

        return ExecuteResult.Done;
    }

    // Structur.PAS:551
    public virtual ProductionTokenEvent CreateProductionTokenEvent()
    {
        ProductionTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        _ = eventValue.Location.X;
        _ = eventValue.Location.Y;
        _ = eventValue.Location.MapLevel;
        MapCoordinate coordinate35 = Location;
        sbyte x = coordinate35.X;
        sbyte y = coordinate35.Y;
        sbyte mapLevel = coordinate35.MapLevel;
        eventValue.Location = new MapCoordinate(x, y, mapLevel);
        eventValue.Player = GetPlayer();
        return eventValue;
    }

    // Structur.PAS:776
    public virtual MapCoordinate Location => MapCoordinate.None;

    // intf_P1_02.inc:133
    public virtual sbyte GetPlayer() => throw AbstractErrorException.Create("Abstract Error");

    // Structur.PAS:279
    public virtual int QueueSizeMax => 5;

    // Structur.PAS:282
    public virtual bool GetQueueLocked()
    {
        int i = Queue.Count;
        bool result = i >= QueueSizeMax;
        while (!result && i != 0)
        {
            i -= 1;
            result = LockQueueProduction(Queue.GetItem(i));
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
    public virtual void UnitResearched() => UnitProductions.GetItem(Settings.UnitProductionIndex).Researched = true;

    // Structur.PAS:219
    public virtual void Update()
    {
        if (UpdateCount == 0)
        {
            OnChange?.Invoke(this);
        }
    }

    // Structur.PAS:564
    public virtual Unit CreateUnitProduction(int index)
    {
        if (UnitProductions.GetItem(index) is not null)
        {
            Unit unit = new(null) { Holder = CreationHolder() };
            UnitProduction unitProduction = UnitProductions.GetItem(index);
            unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
            return unit;
        }
        else
        {
            return null;
        }
    }

    // Structur.PAS:225
    public virtual void Activate() => RegisterGroupProduction();

    // Structur.PAS:228
    public virtual void Deactivate() => UnregisterGroupProduction();

    // Structur.PAS:804
    public virtual void NewTurn(sbyte player)
    {
        if (Status == ProductionControlStatus.Waiting && player == GetPlayer())
        {
            if (Queue.Count != 0)
            {
                _ = ExecuteProduction(Queue.GetItem(0));
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
        for (int i = Queue.Count - 1; i >= 1; i--)
        {
            Queue.GetItem(i)?.Dispose();
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
        bool condition = index >= 0 && index < Queue.Count;
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 1;
            eventValue.Index = index;
            sbyte player = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:726
    public virtual bool DeleteProductions()
    {
        bool condition = Queue.Count != 0;
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 2;
            sbyte player = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:840
    public virtual string GetProductionName(ProductionItem item) => item.Order.Name(this);

    // Structur.PAS:865
    public virtual string GetProductionNameProgressive(ProductionItem item) => item.Order.NameProgressive(this);

    // Structur.PAS:577
    public virtual int UnitProductionGold(int index)
    {
        int unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return GamePack.Current.UnitResources.GetUnitResource(unitIndex).ObtainValue;
    }

    // Structur.PAS:590
    public virtual int UnitProductionMana(int index) => 0;

    // Structur.PAS:585
    public virtual int UnitProductionGoldUpkeep(int index)
    {
        UnitProduction unitProduction = UnitProductions.GetItem(index);
        return (GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level + 1) * 2;
    }

    // Structur.PAS:593
    public virtual int UnitProductionManaUpkeep(int index) => 0;

    // Structur.PAS:596
    public virtual int UnitProductionTime(int index)
    {
        int unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return GamePack.Current.UnitResources.GetUnitResource(unitIndex).Level;
    }

    // Structur.PAS:604
    public virtual bool CanProduceUnit(int index, bool queue, string reason) => true;

    // Structur.PAS:607
    public virtual bool ProduceUnit(int index, int groupId, bool queue)
    {
        string reason = "";
        bool condition = CanProduceUnit(index, queue, reason);
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = new ProductionItem(null)
            {
                Holder = CreationHolder(),             // { explicit item receiver }
                Order = new ProduceUnit(index, groupId)
            };

            sbyte player = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:631
    public virtual bool ProduceUnit(int index, bool queue) => ProduceUnit(index, 0, queue);

    // Structur.PAS:641
    public virtual int UnitResearchGold(int index)
    {
        int unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return GamePack.Current.UnitResources.GetUnitResource(unitIndex).ObtainValue;
    }

    // Structur.PAS:649
    public virtual int UnitResearchMana(int index) => 0;

    // Structur.PAS:652
    public virtual int UnitResearchTime(int index)
    {
        int unitIndex = UnitProductions.GetItem(index).UnitIndex;
        return GamePack.Current.UnitResources.GetUnitResource(unitIndex).Level;
    }

    // Structur.PAS:660
    public virtual bool CanResearchUnit(int index, bool queue, ref string reason)
    {
        if (!queue || Queue.IndexOf(new ResearchSpell(index)) == -1)
        {
            return true;
        }
        else
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), s_actionAlreadyQueuedText);
        }

        return false;
    }

    // Structur.PAS:670
    public virtual bool ResearchUnit(int index, bool queue)
    {
        string reason = "";
        bool condition = CanResearchUnit(index, queue, ref reason);
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = new ProductionItem(null)
            {
                Holder = CreationHolder(),             // { explicit item receiver }
                Order = new ResearchSpell(index)
            };

            sbyte player = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            // { Original returns False after successfully queueing research. }
            return false;
        }
        else
        {
            return true;
        }
    }

    // Structur.PAS:210
    public void BeginUpdate() => UpdateCount += 1;

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
        if (Status == ProductionControlStatus.Producing && Settings.GroupId != 0)
        {
            AIGroup group = StrategicMap.Current.AIGroupManager.Groups.FindId(Settings.GroupId);
            group?.Productions.Add(this, Settings.Unit);
        }
    }

    // Structur.PAS:327
    public void UnregisterGroupProduction()
    {
        if (Status == ProductionControlStatus.Producing && Settings.GroupId != 0)
        {
            AIGroup group = StrategicMap.Current.AIGroupManager.Groups.FindId(Settings.GroupId);
            group?.Productions.DeleteUnit(Settings.Unit);

            Settings.GroupId = 0;
        }
    }

    // Structur.PAS:339
    public void GroupProductionProduced(Troop troop, int groupId)
    {
        sbyte player = GetPlayer();
        AIGroup group = StrategicMap.Current.Players.GetPlayers(player).AIGroups.FindId(groupId);
        group?.UnitProduced(this, troop);
    }

    // Structur.PAS:693
    public bool Produce(ProductionItem item, bool queue)
    {
        sbyte player = GetPlayer();
        if (!StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 0;
            eventValue.Queue = queue;
            eventValue.Item = item.Copy<ProductionItem>(GetEngine());
            sbyte player2 = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player2).AddEvent(eventValue);
            _ = eventValue.Release();
            // { Original returns False after successfully queueing production. }
            return false;
        }
        else
        {
            return true;
        }
    }

    // Structur.PAS:741
    public bool RetryProductionExecution(ref string reason)
    {
        bool condition = Queue.Count != 0 && Status == ProductionControlStatus.Waiting;
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            if (CanExecuteProduction(Queue.GetItem(0), ref reason))
            {
                ProductionTokenEvent eventValue = CreateProductionTokenEvent();
                eventValue.Command = 3;
                sbyte player = GetPlayer();
                StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
                _ = eventValue.Release();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:760
    public bool SetLoopQueueTo(bool value)
    {
        bool condition = value != LoopQueue;
        if (condition)
        {
            sbyte player = GetPlayer();
            condition = !StrategicMap.Current.Players.GetPlayers(player).GetBusy();
        }
        if (condition)
        {
            ProductionTokenEvent eventValue = CreateProductionTokenEvent();
            eventValue.Command = 4;
            eventValue.Index = value ? 1 : 0;

            sbyte player = GetPlayer();
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:793
    public void ProductionDone()
    {
        BeginUpdate();
        if (LoopQueue && LoopableProduction(Queue.GetItem(0)))
        {
            int count = Queue.Count;
            Queue.MoveChild(0, count - 1);
        }
        else
        {
            Queue.GetItem(0)?.Dispose();
        }

        if (Queue.Count != 0)
        {
            _ = ExecuteProduction(Queue.GetItem(0));
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
        sbyte player = GetPlayer();
        Player playerPlayer = StrategicMap.Current.Players.GetPlayers(player);
        Unit unit = new(null) { Holder = CreationHolder() };
        DoubleItemList productions = new();
        for (int i = UnitProductions.Count - 1; i >= 0; i--)
        {
            if (!UnitProductions.GetItem(i).Researched)
            {
                bool include = UnitResearchGold(i) <= gold;
                int priority;
                if (predictor is not null)
                {
                    UnitProduction unitProduction = UnitProductions.GetItem(i);
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
                    priority = predictor.Usefullness(unit);
                    // { Original replaces the affordability check here. }
                    include = (priority & 8388608) != 0;
                }
                else
                {
                    priority = RosterTables.GetUnitIndexAIPriority(UnitProductions.GetItem(i).UnitIndex, playerPlayer.PlayerIndex, 0, engine: GetEngine());
                }

                if (include)
                {
                    _ = productions.AddAbility(i, priority);
                }
            }
        }

        int result;
        if (productions.Count != 0)
        {
            productions.SortDescendingItemB();
            result = 0;
            int minimum = Round(productions[0].Second * 0.8);
            while (result + 1 < productions.Count && productions[result + 1].Second >= minimum)
            {
                result += 1;
            }

            result = StrategicMap.Current.Random(2) == 1 ? 0 : StrategicMap.Current.Random(result + 1);

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
}

public enum ProductionControlStatus : byte
{
    Idle = 0, Waiting = 1, Producing = 2
}
