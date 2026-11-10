// Translated from ItemExpS.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units
{

    public partial class ExplorationSiteItemList : ItemList
    {
        public ExplorationSiteItemList(ECustomNode owner) : base(owner)
        { }

        // intf_P2_04.inc:118

    }
}

namespace Aow.Game.World.Structures
{

    // FItemCount, FItems
    public partial class ItemExplorationSite : ExplorationSite
    {
        public ItemExplorationSite(ECustomNode owner) : base(owner)
        {
            FItems = new ExplorationSiteItemList(null);
            FItemCount = 2;
        }

        // ItemExpS.pas:102
        public new const int ClassID = 132083;

        public byte FItemCount;
        // 0 user-defined, 4 random
        public ExplorationSiteItemList FItems;

        // ItemExpS.pas:95
        public override void Dispose()
        {
            base.Dispose();
            FItems?.Dispose();
            FItems = null;
        }

        // ItemExpS.pas:107
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FItems, 8);
            Stream.rwData(Bytes.Of(ref FItemCount, 1), 1, 9);
        }

        // ItemExpS.pas:88

        // ItemExpS.pas:114
        public override void Activate()
        {
            base.Activate();
            FItems.RegisterItems();
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
            if (AoWHSMap.Current.FDay == 1)
            {
                Generate();
            }
        }

        // ItemExpS.pas:125
        public override bool GetExplored() => FItems.Count == 0;

        // ItemExpS.pas:146
        public override void SetupCombat(Combat.Battle Combat, ArmyHS Army)
        {
            base.SetupCombat(Combat, Army);
            Combat.FItems = FItems;
        }

        // ItemExpS.pas:152
        public override void ExecuteSearchDone(CombatType CombatType, CombatResult CombatResult)
        {
            ArmyHS Army;
            ItemExplorationSiteEventLog Log;
            Item Item = default;
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(CombatResult))
            {
                Army = (ArmyHS)Owner.FindOwnedChild(131607);
                Log = new ItemExplorationSiteEventLog(null);
                Log.FPlayer = Army.FArmy.FPlayer;
                Log.FTitle = AbilityRules.TranslateRStr(ItemsFoundRStr);
                var xhx = GetXhx();
                Log.FLocation.X = xhx;
                var yhx = GetYhx();
                Log.FLocation.Y = yhx;
                var lhx = GetLhx();
                Log.FLocation.L = lhx;
                for (int I = FItems.Count - 1; I >= 0; I--)
                {
                    Item = FItems.GetItem(I);
                    Log.FItemIDs.Add(Item.FItemID);
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    Item.PlaceOnMap(AoWHSMap.Current, xhx2, yhx2, lhx2);
                }

                // { Original parses the formatted text as CSV. }
                var itemsExplored = AbilityRules.TranslateRStr(ItemsExploredRStr);
                var resourceEditName = FResource.ResourceEditName();
                var format = RtlUnit.Format(itemsExplored, [resourceEditName]);
                Log.FMessages.CommaText = format;
                Log.Execute(false, ExecuteTrigger.Event, null);
                Log.Release();
                Update();
            }
        }

        // ItemExpS.pas:130
        public virtual void Generate()
        {
            Item Item;
            if (FItemCount != 0)
            {
                RandSeed = AoWHSMap.Current.Random(16777215);
                if (FItemCount == 4)
                {
                    FItemCount = (byte)(AoWHSMap.Current.Random(3) + 1);
                }

                FItems.Clear();
                for (int itemOrdinal = 1, last = FItemCount; itemOrdinal <= last; itemOrdinal++)
                {
                    Item = AoWHSMap.Current.FItemControl.GenerateItem(itAll, 0, 65535);
                    if (Item != null)
                    {
                        Item.Owner = FItems;
                    }
                }
            }
        }

        private static readonly string ItemsExploredRStr = "\"From the %s\",\"you recovered the following items:\"";
        private static readonly string ItemsFoundRStr = "Items Found";
        private static Set<ItemType> itAll = Set<ItemType>.FromMask(127uL);

        // ItemExpS.pas:32

        // ItemExpS.pas:32

    }
}

namespace Aow.Game.Events
{

    // FItemIDs, FMessages, FTitle, FLocation
    public partial class ItemExplorationSiteEventLog : EventLog
    {
        // ItemExpS.pas:187
        public ItemExplorationSiteEventLog(ECustomNode owner) : base(owner)
        {
            FItemIDs = new IntegerList();
            FMessages = new StringList();
        }

        // ItemExpS.pas:201
        public new const int ClassID = 132085;

        public IntegerList FItemIDs;
        public StringList FMessages;
        public string FTitle = "";
        public XYL FLocation;

        // ItemExpS.pas:194
        public override void Dispose()
        {
            FItemIDs?.Dispose();
            FMessages?.Dispose();
            base.Dispose();
        }

        // ItemExpS.pas:204
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(20, FItemIDs.LoadFromStream, FItemIDs.SaveToStream);
            Stream.rwStrings(FMessages, 21);
            Stream.rwString(ref FTitle, 22);
        }

    }
}

namespace Aow.Game.World.Structures
{

    public partial class ItemExplorationSiteResource : ExplorationSiteResource
    {
        public ItemExplorationSiteResource(ECustomNode owner) : base(owner)
        { }

        // ItemExpS.pas:212
        public new const int ClassID = 132084;

    }
}
