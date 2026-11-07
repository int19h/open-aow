// Translated from ItemExpS.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.ExploreS;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.ItemExpS;

public partial class TExplorationSiteItemList : TItemList
{
    public TExplorationSiteItemList(TECustomNode owner) : base(owner)
    { }

    // ItemExpS.pas:83
    public override void Destroy()
    {
        base.Destroy();
    }

    // intf_P2_04.inc:118

}

// FItemCount, FItems
public partial class TItemExplorationSite : TExplorationSite
{
    public TItemExplorationSite(TECustomNode owner) : base(owner)
    {
        FItems = new TExplorationSiteItemList(null);
        FItemCount = 2;
    }

    // ItemExpS.pas:102
    public new const int ClassID = 132083;

    public byte FItemCount;
    // 0 user-defined, 4 random
    public TExplorationSiteItemList FItems;

    // ItemExpS.pas:95
    public override void Destroy()
    {
        base.Destroy();
        FItems.Free();
        FItems = null;
    }

    // ItemExpS.pas:107
    public override void ReadWrite(TEStorageStream Stream)
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
        if (AoWHSMap.FDay == 1)
        {
            Generate();
        }
    }

    // ItemExpS.pas:125
    public override bool GetExplored() => FItems.Count == 0;

    // ItemExpS.pas:146
    public override void SetupCombat(TCombat Combat, TArmyHS Army)
    {
        base.SetupCombat(Combat, Army);
        Combat.FItems = FItems;
    }

    // ItemExpS.pas:152
    public override void ExecuteSearchDone(TCombatType CombatType, TCombatResult CombatResult)
    {
        TArmyHS Army;
        TItemExplorationSiteEventLog Log;
        TItem Item = default;
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(CombatResult))
        {
            Army = (TArmyHS)Owner.FindOwnedChild(131607);
            Log = new TItemExplorationSiteEventLog(null);
            Log.FPlayer = Army.FArmy.FPlayer;
            Log.FTitle = TranslateRStr(ItemsFoundRStr);
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
                Item.PlaceOnMap(AoWHSMap, xhx2, yhx2, lhx2);
            }

            // { Original parses the formatted text as CSV. }
            var itemsExplored = TranslateRStr(ItemsExploredRStr);
            var resourceEditName = FResource.ResourceEditName();
            var format = RtlUnit.Format(itemsExplored, [resourceEditName]);
            Log.FMessages.CommaText = format;
            Log.Execute(false, TExecuteTrigger.etEvent, null);
            Log.Release();
            Update();
        }
    }

    // ItemExpS.pas:130
    public virtual void Generate()
    {
        TItem Item;
        if (FItemCount != 0)
        {
            RandSeed = AoWHSMap.Random(16777215);
            if (FItemCount == 4)
            {
                FItemCount = (byte)(AoWHSMap.Random(3) + 1);
            }

            FItems.Clear();
            for (int itemOrdinal = 1, last = FItemCount; itemOrdinal <= last; itemOrdinal++)
            {
                Item = AoWHSMap.FItemControl.GenerateItem(itAll, 0, 65535);
                if (Item != null)
                {
                    Item.Owner = FItems;
                }
            }
        }
    }

    // ItemExpS.pas:32

}

// FItemIDs, FMessages, FTitle, FLocation
public partial class TItemExplorationSiteEventLog : TEventLog
{
    // ItemExpS.pas:187
    public TItemExplorationSiteEventLog(TECustomNode owner) : base(owner)
    {
        FItemIDs = new TIntegerList();
        FMessages = new TStringList();
    }

    // ItemExpS.pas:201
    public new const int ClassID = 132085;

    public TIntegerList FItemIDs;
    public TStringList FMessages;
    public string FTitle = "";
    public TXYL FLocation;

    // ItemExpS.pas:194
    public override void Destroy()
    {
        FItemIDs.Free();
        FMessages.Free();
        base.Destroy();
    }

    // ItemExpS.pas:204
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(20, FItemIDs.LoadFromStream, FItemIDs.SaveToStream);
        Stream.rwStrings(FMessages, 21);
        Stream.rwString(ref FTitle, 22);
    }

}

public partial class TItemExplorationSiteResource : TExplorationSiteResource
{
    public TItemExplorationSiteResource(TECustomNode owner) : base(owner)
    { }

    // ItemExpS.pas:212
    public new const int ClassID = 132084;

}

public static partial class ItemExpSUnit
{
    // ItemExpS.pas:215
    public static void RegisterItemExplorationSite(TEngine Engine)
    {
        Engine.RegisterEClasses([EClass.Of<TItemExplorationSiteEventLog>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
