// Translated from ItemExpS.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.ExploreS;
using static Aow.Units.ExploreS.ExploreS;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ItemExpS
{
    // { TExplorationSiteItemList (instance size 24, VMT 557C2298) }
    public partial class TExplorationSiteItemList : Aow.Units.AoWE.TItemList
    {
        public TExplorationSiteItemList(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // ItemExpS.pas:83
        public override void Destroy()
        {
            base.Destroy();
        }

        // intf_P2_04.inc:118

    }

    // { TItemExplorationSite (instance size 64, VMT 557C23BC): $38 FItemCount, $3C FItems. }
    public partial class TItemExplorationSite : Aow.Units.ExploreS.TExplorationSite
    {
        public TItemExplorationSite(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__1 = new Aow.Units.ItemExpS.TExplorationSiteItemList(null);
            FItems = call__1;
            FItemCount = 2;
        }

        // ItemExpS.pas:102
        public new const int ClassID = 132083;

        public byte FItemCount;
        // { $38; 0 user-defined, 4 random }
        public Aow.Units.FixedArray_byte_3 F_ItemExplorationSite_39;
        public Aow.Units.ItemExpS.TExplorationSiteItemList FItems;
        // ItemExpS.pas:95
        // { $3C }
        public override void Destroy()
        {
            base.Destroy();
            FItems.Free();
            FItems = null;
        }

        // ItemExpS.pas:107
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FItems, 8);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FItemCount, 1), 1, 9);
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1)
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
        public override void SetupCombat(Aow.Units.AoWE.TCombat A1, Aow.Units.AoWE.TArmyHS A2)
        {
            base.SetupCombat(A1, A2);
            A1.FItems = FItems;
        }

        // ItemExpS.pas:152
        public override void ExecuteSearchDone(Aow.Units.AoWE.TCombatType A1, Aow.Units.AoWE.TCombatResult A2)
        {
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.ItemExpS.TItemExplorationSiteEventLog Log = default;
            int I = default;
            Aow.Units.AoWE.TItem Item = default;
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(A2))
            {
                var call__3 = Owner.FindOwnedChild(131607);
                Army = (Aow.Units.AoWE.TArmyHS)call__3;
                Log = new Aow.Units.ItemExpS.TItemExplorationSiteEventLog(null);
                Log.FPlayer = unchecked((sbyte)Army.FArmy.FPlayer);
                var call__4 = Aow.Units.AoWE.AoWE.ItemsFoundRStr;
                var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(call__4);
                Log.FTitle = call__5;
                var call__6 = GetXhx();
                Log.FLocation.X = unchecked((sbyte)call__6);
                var call__7 = GetYhx();
                Log.FLocation.Y = unchecked((sbyte)call__7);
                var call__8 = GetLhx();
                Log.FLocation.L = unchecked((sbyte)call__8);
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
                        Item.PlaceOnMap(Aow.Units.AoWE.AoWE.AoWHSMap, unchecked((sbyte)call__13), unchecked((sbyte)call__12), unchecked((sbyte)call__11));
                    }
                }

                // { Original parses the formatted text as CSV. }
                var call__14 = Aow.Units.AoWE.AoWE.ItemsExploredRStr;
                var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(call__14);
                var call__16 = FResource.ResourceEditName();
                var call__17 = Aow.Rtl.Rtl.Format(call__15, [call__16]);
                Log.FMessages.CommaText = call__17;
                Log.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                Log.Release();
                Update();
            }
        }

        // ItemExpS.pas:130
        public virtual void Generate()
        {
            byte I = default;
            Aow.Units.AoWE.TItem Item = default;
            if (FItemCount != 0)
            {
                var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                Aow.Rtl.Rtl.RandSeed = call__18;
                if (FItemCount == 4)
                {
                    var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                    FItemCount = unchecked((byte)(call__19 + 1));
                }

                FItems.Clear();
                {
                    int first__23 = (int)1;
                    int end__20 = (int)FItemCount;
                    for (int ordinal__22 = first__23; ordinal__22 <= end__20; ordinal__22++)
                    {
                        I = (byte)ordinal__22;
                        var call__21 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.GenerateItem(Aow.Units.AoWE.AoWE.itAll, 0, 65535);
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


        public new partial class Meta : Aow.Units.ExploreS.TExplorationSite.Meta
        {
            // ItemExpS.pas:102
            public override int ClassID() => Aow.Units.ItemExpS.TItemExplorationSite.ClassID;

            // ItemExpS.pas:32
            public new Aow.Units.ItemExpS.TItemExplorationSite Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ItemExpS.TItemExplorationSite)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.ItemExpS.TItemExplorationSite => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { +204 }
    // { TItemExplorationSiteEventLog (instance size 40, VMT 557C264C):
//     $18 FItemIDs, $1C FMessages, $20 FTitle, $24 FLocation. }
    public partial class TItemExplorationSiteEventLog : Aow.Units.EventLog.TEventLog
    {
        // ItemExpS.pas:187
        public TItemExplorationSiteEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__24 = new Aow.Units.Engine.TIntegerList();
            FItemIDs = call__24;
            var call__25 = new Aow.Rtl.TStringList();
            FMessages = call__25;
        }

        // ItemExpS.pas:201
        public new const int ClassID = 132085;

        public Aow.Units.Engine.TIntegerList FItemIDs;
        // { $18 }
        public Aow.Rtl.TStringList FMessages;
        // { $1C }
        public Aow.Rtl.AnsiString FTitle;
        // { $20 }
        public Aow.Units.HSEngine.TXYL FLocation;
        // ItemExpS.pas:194
        // { $24 }
        public override void Destroy()
        {
            FItemIDs.Free();
            FMessages.Free();
            base.Destroy();
        }

        // ItemExpS.pas:204
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(20, FItemIDs.LoadFromStream, FItemIDs.SaveToStream);
            Stream.rwStrings(FMessages, 21);
            Stream.rwString(ref FTitle, 22);
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // ItemExpS.pas:201
            public override int ClassID() => Aow.Units.ItemExpS.TItemExplorationSiteEventLog.ClassID;

            // ItemExpS.pas:53
            public new Aow.Units.ItemExpS.TItemExplorationSiteEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ItemExpS.TItemExplorationSiteEventLog)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.ItemExpS.TItemExplorationSiteEventLog => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TItemExplorationSiteResource (instance size 104, VMT 557C276C) }
    public partial class TItemExplorationSiteResource : Aow.Units.ExploreS.TExplorationSiteResource
    {
        public TItemExplorationSiteResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // ItemExpS.pas:212
        public new const int ClassID = 132084;

        public new partial class Meta : Aow.Units.ExploreS.TExplorationSiteResource.Meta
        {
            // ItemExpS.pas:212
            public override int ClassID() => Aow.Units.ItemExpS.TItemExplorationSiteResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.ItemExpS.TItemExplorationSiteResource => (Meta)MetaRegistry.Of(typeof(T));
            // ExploreS.pas:61
            public new Aow.Units.ItemExpS.TItemExplorationSiteResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ItemExpS.TItemExplorationSiteResource)base.Create(AOwner);
        }
    }

    public static partial class ItemExpS
    {
        // ItemExpS.pas:215
        public static void RegisterItemExplorationSite(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.ItemExpS.TItemExplorationSiteEventLog>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
