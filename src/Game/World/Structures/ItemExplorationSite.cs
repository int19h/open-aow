// Translated from ItemExpS.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// ItemCount, Items
public partial class ItemExplorationSite : ExplorationSite
{
    public ItemExplorationSite(ECustomNode owner) : base(owner)
    {
        _items = new ExplorationSiteItemList(null) { Holder = CreationHolder() };
        _itemCount = 2;
    }

    // ItemExpS.pas:102
    public new const int ClassId = 132083;

    private byte _itemCount;
    // 0 user-defined, 4 random
    private ExplorationSiteItemList _items;

    // ItemExpS.pas:95
    public override void Dispose()
    {
        base.Dispose();
        _items?.Dispose();
        _items = null;
    }

    // ItemExpS.pas:107
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(_items, 8);
        stream.Bytes(Bytes.Of(ref _itemCount, 1), 1, 9);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Items", objectValue: _items);
        _itemCount = persister.RawByte("ItemCount", value: _itemCount);
    }

    // ItemExpS.pas:88

    // ItemExpS.pas:114
    public override void Activate()
    {
        base.Activate();
        _items.RegisterItems();
    }

    // ItemExpS.pas:120
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // ItemExpS.pas:181
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1)
        {
            Generate();
        }
    }

    // ItemExpS.pas:125
    public override bool GetExplored() => _items.Count == 0;

    // ItemExpS.pas:146
    public override void SetupCombat(Combat.Battle combat, ArmyHexSprite army)
    {
        base.SetupCombat(combat, army);
        combat.Items = _items;
    }

    // ItemExpS.pas:152
    public override void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
        ArmyHexSprite army;
        ItemExplorationSiteEventLog log;
        Item item = default;
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(combatResult))
        {
            army = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            log = new ItemExplorationSiteEventLog(null) { Holder = CreationHolder() };
            log._player = army.Army.Player;
            log.Title = AbilityRules.TranslateResourceString(GetEngine(), s_itemsFoundText);
            log.Location = new MapCoordinate(X: GetHexX(), Y: GetHexY(), MapLevel: GetHexLevel());
            for (int I = _items.Count - 1; I >= 0; I--)
            {
                item = _items.GetItem(I);
                log.ItemIds.Add(item.ItemId);
                var hexLevel2 = GetHexLevel();
                var hexY2 = GetHexY();
                var hexX2 = GetHexX();
                item.PlaceOnMap(StrategicMap.Current, hexX2, hexY2, hexLevel2);
            }

            // { Original parses the formatted text as CSV. }
            var itemsExplored = AbilityRules.TranslateResourceString(GetEngine(), s_itemsExploredFormat);
            var resourceEditName = _resource.ResourceEditName();
            var format = RtlUnit.Format(itemsExplored, [resourceEditName]);
            log.Messages.CommaText = format;
            log.Execute(false, ExecuteTrigger.Event, null);
            log.Release();
            Update();
        }
    }

    // ItemExpS.pas:130
    public virtual void Generate()
    {
        Item item;
        if (_itemCount != 0)
        {
            RandSeed = StrategicMap.Current.Random(16777215);
            if (_itemCount == 4)
            {
                _itemCount = (byte)(StrategicMap.Current.Random(3) + 1);
            }

            _items.Clear();
            for (int itemOrdinal = 1, last = _itemCount; itemOrdinal <= last; itemOrdinal++)
            {
                item = StrategicMap.Current.ItemControl.GenerateItem(s_itAll, 0, 65535);
                if (item != null)
                {
                    item.Owner = _items;
                }
            }
        }
    }

    private static readonly string s_itemsExploredFormat = "\"From the %s\",\"you recovered the following items:\"";
    private static readonly string s_itemsFoundText = "Items Found";
    private static readonly Set<ItemType> s_itAll = Set<ItemType>.FromMask(127uL);

    // ItemExpS.pas:32

    // ItemExpS.pas:32

}
