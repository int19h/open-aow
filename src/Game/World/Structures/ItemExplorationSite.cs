// Translated from ItemExpS.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
        Items = new ExplorationSiteItemList(null);
        ItemCount = 2;
    }

    // ItemExpS.pas:102
    public new const int ClassId = 132083;

    public byte ItemCount;
    // 0 user-defined, 4 random
    public ExplorationSiteItemList Items;

    // ItemExpS.pas:95
    public override void Dispose()
    {
        base.Dispose();
        Items?.Dispose();
        Items = null;
    }

    // ItemExpS.pas:107
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Items, 8);
        stream.rwData(Bytes.Of(ref ItemCount, 1), 1, 9);
    }

    // ItemExpS.pas:88

    // ItemExpS.pas:114
    public override void Activate()
    {
        base.Activate();
        Items.RegisterItems();
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
        if (AoWHSMap.Current.Day == 1)
        {
            Generate();
        }
    }

    // ItemExpS.pas:125
    public override bool GetExplored() => Items.Count == 0;

    // ItemExpS.pas:146
    public override void SetupCombat(Combat.Battle combat, ArmyHexSprite army)
    {
        base.SetupCombat(combat, army);
        combat.Items = Items;
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
            log = new ItemExplorationSiteEventLog(null);
            log._player = army.Army.Player;
            log.Title = AbilityRules.TranslateResourceString(s_itemsFoundText);
            var hexX = GetHexX();
            log.Location.X = hexX;
            var hexY = GetHexY();
            log.Location.Y = hexY;
            var hexLevel = GetHexLevel();
            log.Location.MapLevel = hexLevel;
            for (int I = Items.Count - 1; I >= 0; I--)
            {
                item = Items.GetItem(I);
                log.ItemIds.Add(item.ItemId);
                var hexLevel2 = GetHexLevel();
                var hexY2 = GetHexY();
                var hexX2 = GetHexX();
                item.PlaceOnMap(AoWHSMap.Current, hexX2, hexY2, hexLevel2);
            }

            // { Original parses the formatted text as CSV. }
            var itemsExplored = AbilityRules.TranslateResourceString(s_itemsExploredFormat);
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
        if (ItemCount != 0)
        {
            RandSeed = AoWHSMap.Current.Random(16777215);
            if (ItemCount == 4)
            {
                ItemCount = (byte)(AoWHSMap.Current.Random(3) + 1);
            }

            Items.Clear();
            for (int itemOrdinal = 1, last = ItemCount; itemOrdinal <= last; itemOrdinal++)
            {
                item = AoWHSMap.Current.ItemControl.GenerateItem(s_itAll, 0, 65535);
                if (item != null)
                {
                    item.Owner = Items;
                }
            }
        }
    }

    private static readonly string s_itemsExploredFormat = "\"From the %s\",\"you recovered the following items:\"";
    private static readonly string s_itemsFoundText = "Items Found";
    private static Set<ItemType> s_itAll = Set<ItemType>.FromMask(127uL);

    // ItemExpS.pas:32

    // ItemExpS.pas:32

}
