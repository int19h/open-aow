#nullable disable
// Translated from ItemExpS.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Structures;

// ItemCount, Items
public class ItemExplorationSite : ExplorationSite
{
    public ItemExplorationSite(CustomNode owner) : base(owner)
    {
        _items = new ExplorationSiteItemList(null) { Holder = CreationHolder() };
        _itemCount = 2;
    }

    // ItemExpS.pas:102
    public new const int ClassId = 132_083;

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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Items", objectValue: _items);
        _itemCount = persister.RawByte("ItemCount", value: _itemCount);
    }

    // ItemExpS.pas:114
    public override void Activate()
    {
        base.Activate();
        _items.RegisterItems();
    }

    // ItemExpS.pas:120
    public override void Deactivate() => base.Deactivate();

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
    public override bool Explored => _items.Count == 0;

    // ItemExpS.pas:146
    public override void SetupCombat(Battle combat, ArmyHexSprite army)
    {
        base.SetupCombat(combat, army);
        combat.Items = _items;
    }

    // ItemExpS.pas:152
    public override void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(combatResult))
        {
            ArmyHexSprite army = Owner.GetArmyHexSprite();
            ItemExplorationSiteEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Player = army.Army.Player,
                Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_itemsFoundText),
                Location = new MapCoordinate(X: HexX, Y: HexY, MapLevel: HexLevel)
            };
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                Item item = _items.GetItem(i);
                _ = log.ItemIds.Add(item.ItemId);
                _ = item.PlaceOnMap(level: HexLevel, y: HexY, x: HexX, map: StrategicMap.Current);
            }

            // { Original parses the formatted text as CSV. }
            string itemsExplored = AbilityRules.TranslateResourceString(GetEngine(), S: s_itemsExploredFormat);
            string resourceEditName = Resource.ResourceEditName();
            string format = Format(itemsExplored, args: [resourceEditName]);
            log.Messages.CommaText = format;
            _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
            _ = log.Release();
            Update();
        }
    }

    // ItemExpS.pas:130
    public virtual void Generate()
    {
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
                Item item = StrategicMap.Current.ItemControl.GenerateItem(s_itAll, 0, 65535);
                item?.Owner = _items;
            }
        }
    }

    private static readonly string s_itemsExploredFormat = "\"From the %s\",\"you recovered the following items:\"";
    private static readonly string s_itemsFoundText = "Items Found";
    private static readonly Set<ItemType> s_itAll = Set<ItemType>.FromMask(127uL);
}
