// Translated from ItemExpS.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ExploreS;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ItemExpS
{
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
            var call__1 = new TExplorationSiteItemList(null);
            FItems = call__1;
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
        public override bool GetExplored()
        {
            bool Result = default;
            var call__2 = FItems.Count;
            Result = call__2 == 0;
            return Result;
        }

        // ItemExpS.pas:146
        public override void SetupCombat(TCombat A1, TArmyHS A2)
        {
            base.SetupCombat(A1, A2);
            A1.FItems = FItems;
        }

        // ItemExpS.pas:152
        public override void ExecuteSearchDone(TCombatType A1, TCombatResult A2)
        {
            TArmyHS Army = default;
            TItemExplorationSiteEventLog Log = default;
            int I = default;
            TItem Item = default;
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(A2))
            {
                var call__3 = Owner.FindOwnedChild(131607);
                Army = (TArmyHS)call__3;
                Log = new TItemExplorationSiteEventLog(null);
                Log.FPlayer = Army.FArmy.FPlayer;
                var call__4 = ItemsFoundRStr;
                var call__5 = TranslateRStr(call__4);
                Log.FTitle = call__5;
                var call__6 = GetXhx();
                Log.FLocation.X = call__6;
                var call__7 = GetYhx();
                Log.FLocation.Y = call__7;
                var call__8 = GetLhx();
                Log.FLocation.L = call__8;
                {
                    I = FItems.Count - 1;
                    byte end__9 = 0;
                    for (; I >= end__9; I -= 1)
                    {
                        var call__10 = FItems.GetItem(I);
                        Item = call__10;
                        Log.FItemIDs.Add(Item.FItemID);
                        var call__11 = GetLhx();
                        var call__12 = GetYhx();
                        var call__13 = GetXhx();
                        Item.PlaceOnMap(AoWHSMap, call__13, call__12, call__11);
                    }
                }

                // { Original parses the formatted text as CSV. }
                var call__14 = ItemsExploredRStr;
                var call__15 = TranslateRStr(call__14);
                var call__16 = FResource.ResourceEditName();
                var call__17 = Rtl.Rtl.Format(call__15, [call__16]);
                Log.FMessages.CommaText = call__17;
                Log.Execute(false, TExecuteTrigger.etEvent, null);
                Log.Release();
                Update();
            }
        }

        // ItemExpS.pas:130
        public virtual void Generate()
        {
            byte I = default;
            TItem Item = default;
            if (FItemCount != 0)
            {
                var call__18 = AoWHSMap.Random(16777215);
                RandSeed = call__18;
                if (FItemCount == 4)
                {
                    var call__19 = AoWHSMap.Random(3);
                    FItemCount = (byte)(call__19 + 1);
                }

                FItems.Clear();
                {
                    int first__23 = 1;
                    int end__20 = FItemCount;
                    for (int ordinal__22 = first__23; ordinal__22 <= end__20; ordinal__22++)
                    {
                        I = (byte)ordinal__22;
                        var call__21 = AoWHSMap.FItemControl.GenerateItem(itAll, 0, 65535);
                        Item = call__21;
                        if (Item != null)
                        {
                            Item.Owner = FItems;
                        }
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
            var call__24 = new TIntegerList();
            FItemIDs = call__24;
            var call__25 = new TStringList();
            FMessages = call__25;
        }

        // ItemExpS.pas:201
        public new const int ClassID = 132085;

        public TIntegerList FItemIDs;
        public TStringList FMessages;
        public AnsiString FTitle;
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
        {}

        // ItemExpS.pas:212
        public new const int ClassID = 132084;

    }

    public static partial class ItemExpS
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
}
