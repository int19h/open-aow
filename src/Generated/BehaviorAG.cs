// Generated from BehaviorAG.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.IndependentAG;
using static Aow.Units.IndependentAG.IndependentAG;
using Aow.Units.MainAG;
using static Aow.Units.MainAG.MainAG;
using Aow.Units.AGCPlugins;
using static Aow.Units.AGCPlugins.AGCPlugins;

namespace Aow.Units.BehaviorAG
{
    // { TAbstractBehaviorAG (instance size 80, VMT 557DC2CC) }
    public partial class TAbstractBehaviorAG : Aow.Units.AoWE.TAIGroup
    {
        // BehaviorAG.pas:137
        public override bool CooperateWithGroup(Aow.Units.AoWE.TAIGroup Group, Aow.Units.AoWE.TAIGroupControlTarget Target)
        {
            bool Result = default;
            // { conflict: function TAbstractBehaviorAG.CooperateWithGroup(Group: TAIGroup): Boolean; }
            Result = base.CooperateWithGroup(Group, Target);
            Result = Result || (Group.ClassType.Behavior() == Aow.Units.AoWE.TGroupBehavior.gbFortify);
            return Result;
        }
    }

    // { TAbstractBehaviorAGC (instance size 372, VMT 557DC430): $170 FTargetSelector. }
    public partial class TAbstractBehaviorAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public Aow.Units.AoWE.TAIGroupTargetSelector FTargetSelector;
        // BehaviorAG.pas:150
        // { $170 }
        public override void Destroy()
        {
            FTargetSelector.Release();
            base.Destroy();
        }

        // BehaviorAG.pas:144
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.AoWE.TAIGroupTargetSelector.Create(null);
            FTargetSelector = call__1;
        }

        // BehaviorAG.pas:39
        public static new Aow.Units.BehaviorAG.TAbstractBehaviorAGC Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TAbstractBehaviorAGC o = new Aow.Units.BehaviorAG.TAbstractBehaviorAGC();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TSkirmishAG (instance size 80, VMT 557DC53C) }
    public partial class TSkirmishAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:722
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 12;
            FRecruitmentPriority = 12;
        }

        // BehaviorAG.pas:45
        public static new Aow.Units.BehaviorAG.TSkirmishAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TSkirmishAG o = new Aow.Units.BehaviorAG.TSkirmishAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:729
            public override int ClassID()
            {
                int Result = default;
                Result = 136294;
                return Result;
            }

            // BehaviorAG.pas:735
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TSkirmishAGC>();
                return Result;
            }

            // BehaviorAG.pas:732
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbSkirmish;
                return Result;
            }

            // BehaviorAG.pas:45
            public new Aow.Units.BehaviorAG.TSkirmishAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TSkirmishAG o = (Aow.Units.BehaviorAG.TSkirmishAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TSkirmishAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSkirmishAGC (instance size 372, VMT 557DC690) }
    public partial class TSkirmishAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:738
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            switch (FState)
            {
                case 0:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__2 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__2);
                    }
                    else
                    {
                        Finder.FSelector.SetTransporter(FGroup.FTransporter);
                    }

                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    var call__3 = FTargets.Count;
                    if (call__3 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FSuperiority = 3;
                        FTargetSelector.FNearSuperiority = 2;
                        FTargetSelector.FMode = FTargetSelector.FMode - TSet<Aow.Units.AoWE.TAIGroupTargetSelectorModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetSelectorModeElement.tsmCooperate, Aow.Units.AoWE.TAIGroupTargetSelectorModeElement.tsmRecruit, Aow.Units.AoWE.TAIGroupTargetSelectorModeElement.tsmProduce);
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 2:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 5;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 5:
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__4.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    var call__6 = call__5.FAIControl.GetRandomGroupBehavior();
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(unchecked((sbyte)FGroup.FPlayer), 0, call__6);
                    Group = call__7;
                    Group.AddUnits(FGroup);
                    Group.Release();
                    Done();
                    break;
            }
        }
    }

    // { TNormalAG (instance size 80, VMT 557DC78C) }
    public partial class TNormalAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:156
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:60
        public static new Aow.Units.BehaviorAG.TNormalAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TNormalAG o = new Aow.Units.BehaviorAG.TNormalAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:163
            public override int ClassID()
            {
                int Result = default;
                Result = 136292;
                return Result;
            }

            // BehaviorAG.pas:169
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TNormalAGC>();
                return Result;
            }

            // BehaviorAG.pas:166
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbNormal;
                return Result;
            }

            // BehaviorAG.pas:60
            public new Aow.Units.BehaviorAG.TNormalAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TNormalAG o = (Aow.Units.BehaviorAG.TNormalAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TNormalAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TNormalAGC (instance size 372, VMT 557DC8E0) }
    public partial class TNormalAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:172
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder Production = default;
            Aow.Units.AGCPlugins.TAIGroupRazeControl Raze = default;
            Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector Defend = default;
            switch (FState)
            {
                case 0:
                    Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    var call__8 = FGroup.Count;
                    if (call__8 > 0)
                    {
                        FTargets.Clear();
                        Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                        Finder.Setup(this);
                        if (FGroup.FTransporter == null)
                        {
                            var call__9 = FindAvailableTransporter();
                            Finder.FSelector.SetTransporter(call__9);
                        }
                        else
                        {
                            Finder.FSelector.SetTransporter(FGroup.FTransporter);
                        }

                        ActivatePlugin(Finder);
                        Finder.Release();
                        FState += 1;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 2:
                    var call__10 = FTargets.Count;
                    if (call__10 != 0)
                    {
                        Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                        Recruitment.Setup(this);
                        ActivatePlugin(Recruitment);
                        Recruitment.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 3:
                    var call__11 = FTargets.Count;
                    if (call__11 != 0)
                    {
                        FProductionLocations.Clear();
                        Production = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        Production.Setup(this);
                        ActivatePlugin(Production);
                        Production.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 4:
                    var call__12 = FTargets.Count;
                    if (call__12 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FSuperiority = (float)1.5;
                        FTargetSelector.FPathStrengthFactor = (float)0.9;
                        FTargetSelector.FCurrentPathStrengthFactor = (float)0.7;
                        FTargetSelector.FNearSuperiority = (float)1.3;
                        FTargetSelector.FMultiTargetFactor = 2;
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 5:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = Aow.Units.AGCPlugins.TAIGroupRazeControl.Create(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 6:
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__13.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Defend = Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector.Create(null);
                    Defend.Setup(this);
                    ActivatePlugin(Defend);
                    Defend.Release();
                    FState = 11;
                    break;
                case 11:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    CombineIdleGroups();
                    Done();
                    break;
            }
        }
    }

    // { TDefenderAG (instance size 80, VMT 557DC9D4) }
    public partial class TDefenderAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:270
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:75
        public static new Aow.Units.BehaviorAG.TDefenderAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TDefenderAG o = new Aow.Units.BehaviorAG.TDefenderAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:277
            public override int ClassID()
            {
                int Result = default;
                Result = 136278;
                return Result;
            }

            // BehaviorAG.pas:283
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TDefenderAGC>();
                return Result;
            }

            // BehaviorAG.pas:280
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbDefensive;
                return Result;
            }

            // BehaviorAG.pas:75
            public new Aow.Units.BehaviorAG.TDefenderAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TDefenderAG o = (Aow.Units.BehaviorAG.TDefenderAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TDefenderAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TDefenderAGC (instance size 372, VMT 557DCB28) }
    public partial class TDefenderAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:286
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder Production = default;
            Aow.Units.AGCPlugins.TAIGroupRazeControl Raze = default;
            Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector Defend = default;
            switch (FState)
            {
                case 0:
                    Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__14 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__14);
                    }
                    else
                    {
                        Finder.FSelector.SetTransporter(FGroup.FTransporter);
                    }

                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 2:
                    var call__15 = FTargets.Count;
                    if (call__15 != 0)
                    {
                        Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                        Recruitment.Setup(this);
                        ActivatePlugin(Recruitment);
                        Recruitment.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 3:
                    var call__16 = FTargets.Count;
                    if (call__16 != 0)
                    {
                        FProductionLocations.Clear();
                        Production = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        Production.Setup(this);
                        ActivatePlugin(Production);
                        Production.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 4:
                    var call__17 = FTargets.Count;
                    if (call__17 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FBattleFieldFactor = (float)1.7;
                        FTargetSelector.FCurrentBattleFieldFactor = (float)1.2;
                        FTargetSelector.FSuperiority = 2;
                        FTargetSelector.FNearSuperiority = (float)1.5;
                        FTargetSelector.FMinimumStrength = 10000;
                        FTargetSelector.FMultiTargetFactor = 2;
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 5:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = Aow.Units.AGCPlugins.TAIGroupRazeControl.Create(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 6:
                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__18.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Defend = Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector.Create(null);
                    Defend.Setup(this);
                    ActivatePlugin(Defend);
                    Defend.Release();
                    FState = 11;
                    break;
                case 11:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    CombineIdleGroups();
                    Done();
                    break;
            }
        }
    }

    // { TAggressorAG (instance size 80, VMT 557DCC24) }
    public partial class TAggressorAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:383
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:90
        public static new Aow.Units.BehaviorAG.TAggressorAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TAggressorAG o = new Aow.Units.BehaviorAG.TAggressorAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:390
            public override int ClassID()
            {
                int Result = default;
                Result = 136279;
                return Result;
            }

            // BehaviorAG.pas:396
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TAggressorAGC>();
                return Result;
            }

            // BehaviorAG.pas:393
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbAggressive;
                return Result;
            }

            // BehaviorAG.pas:90
            public new Aow.Units.BehaviorAG.TAggressorAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TAggressorAG o = (Aow.Units.BehaviorAG.TAggressorAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TAggressorAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TAggressorAGC (instance size 372, VMT 557DCD80) }
    public partial class TAggressorAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:399
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder Production = default;
            Aow.Units.AGCPlugins.TAIGroupRazeControl Raze = default;
            Aow.Units.AGCPlugins.TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__19 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__19);
                    }
                    else
                    {
                        Finder.FSelector.SetTransporter(FGroup.FTransporter);
                    }

                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 2:
                    var call__20 = FTargets.Count;
                    if (call__20 != 0)
                    {
                        Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                        Recruitment.Setup(this);
                        ActivatePlugin(Recruitment);
                        Recruitment.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 3:
                    var call__21 = FTargets.Count;
                    if (call__21 != 0)
                    {
                        FProductionLocations.Clear();
                        Production = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        Production.Setup(this);
                        ActivatePlugin(Production);
                        Production.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 4:
                    var call__22 = FTargets.Count;
                    if (call__22 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FPathStrengthFactor = (float)0.75;
                        FTargetSelector.FCurrentPathStrengthFactor = (float)0.55;
                        FTargetSelector.FSuperiority = (float)1.4;
                        FTargetSelector.FNearSuperiority = (float)1.2;
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 5:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = Aow.Units.AGCPlugins.TAIGroupRazeControl.Create(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 6:
                    var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__23.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = Aow.Units.AGCPlugins.TAIGroupWanderControl.Create(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
                    break;
                case 11:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    CombineIdleGroups();
                    Done();
                    break;
            }
        }
    }

    // { TExpanderAG (instance size 80, VMT 557DCE7C) }
    public partial class TExpanderAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:495
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:105
        public static new Aow.Units.BehaviorAG.TExpanderAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TExpanderAG o = new Aow.Units.BehaviorAG.TExpanderAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:502
            public override int ClassID()
            {
                int Result = default;
                Result = 136290;
                return Result;
            }

            // BehaviorAG.pas:508
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TExpanderAGC>();
                return Result;
            }

            // BehaviorAG.pas:505
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbExpand;
                return Result;
            }

            // BehaviorAG.pas:105
            public new Aow.Units.BehaviorAG.TExpanderAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TExpanderAG o = (Aow.Units.BehaviorAG.TExpanderAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TExpanderAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TExpanderAGC (instance size 372, VMT 557DCFD0) }
    public partial class TExpanderAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:511
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder Production = default;
            Aow.Units.AGCPlugins.TAIGroupRazeControl Raze = default;
            Aow.Units.AGCPlugins.TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__24 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__24);
                    }
                    else
                    {
                        Finder.FSelector.SetTransporter(FGroup.FTransporter);
                    }

                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 2:
                    var call__25 = FTargets.Count;
                    if (call__25 != 0)
                    {
                        Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                        Recruitment.Setup(this);
                        ActivatePlugin(Recruitment);
                        Recruitment.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 3:
                    var call__26 = FTargets.Count;
                    if (call__26 != 0)
                    {
                        FProductionLocations.Clear();
                        Production = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        Production.Setup(this);
                        ActivatePlugin(Production);
                        Production.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 4:
                    var call__27 = FTargets.Count;
                    if (call__27 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FMoveDayFactor = (float)0.05;
                        FTargetSelector.FPrioritySettings.FSetupBonus = 25;
                        FTargetSelector.FPrioritySettings.FExecuteBonus = 25;
                        FTargetSelector.FPrioritySettings.FMoveDayPenalty = -10;
                        FTargetSelector.FPrioritySettings.FImmediateBonus = 10;
                        FTargetSelector.FPrioritySettings.FTransportPenalty = -25;
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 5:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = Aow.Units.AGCPlugins.TAIGroupRazeControl.Create(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 6:
                    var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__28.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = Aow.Units.AGCPlugins.TAIGroupWanderControl.Create(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
                    break;
                case 11:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    CombineIdleGroups();
                    Done();
                    break;
            }
        }
    }

    // { TScorcherAG (instance size 80, VMT 557DD0CC) }
    public partial class TScorcherAG : Aow.Units.BehaviorAG.TAbstractBehaviorAG
    {
        // BehaviorAG.pas:609
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:120
        public static new Aow.Units.BehaviorAG.TScorcherAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BehaviorAG.TScorcherAG o = new Aow.Units.BehaviorAG.TScorcherAG();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TAIGroup.Meta
        {
            // BehaviorAG.pas:616
            public override int ClassID()
            {
                int Result = default;
                Result = 136291;
                return Result;
            }

            // BehaviorAG.pas:622
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.BehaviorAG.TScorcherAGC>();
                return Result;
            }

            // BehaviorAG.pas:619
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbScorch;
                return Result;
            }

            // BehaviorAG.pas:120
            public new Aow.Units.BehaviorAG.TScorcherAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.BehaviorAG.TScorcherAG o = (Aow.Units.BehaviorAG.TScorcherAG)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TScorcherAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TScorcherAGC (instance size 372, VMT 557DD220) }
    public partial class TScorcherAGC : Aow.Units.BehaviorAG.TAbstractBehaviorAGC
    {
        // BehaviorAG.pas:625
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder Production = default;
            Aow.Units.AGCPlugins.TAIGroupRazeControl Raze = default;
            Aow.Units.AGCPlugins.TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                    Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__29 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__29);
                    }
                    else
                    {
                        Finder.FSelector.SetTransporter(FGroup.FTransporter);
                    }

                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 2:
                    var call__30 = FTargets.Count;
                    if (call__30 != 0)
                    {
                        Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                        Recruitment.Setup(this);
                        ActivatePlugin(Recruitment);
                        Recruitment.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 3:
                    var call__31 = FTargets.Count;
                    if (call__31 != 0)
                    {
                        FProductionLocations.Clear();
                        Production = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        Production.Setup(this);
                        ActivatePlugin(Production);
                        Production.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 4:
                    var call__32 = FTargets.Count;
                    if (call__32 != 0)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FSuperiority = (float)1.75;
                        FTargetSelector.FNearSuperiority = (float)1.3;
                        FTargetSelector.FMultiTargetFactor = 200;
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 5:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = Aow.Units.AGCPlugins.TAIGroupRazeControl.Create(null);
                        Raze.Setup(this);
                        Raze.FMode = Raze.FMode + TSet<Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement>.Of(Aow.Units.AGCPlugins.TAIGroupRazeControlModeElement.rcmOnMigrate);
                        Raze.FCityStrengthFactor = (float)1.5;
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssNoTargetsFound)
                        {
                            FState = 10;
                        }
                        else
                        {
                            Done();
                        }
                    }

                    break;
                case 6:
                    var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__33.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = Aow.Units.AGCPlugins.TAIGroupWanderControl.Create(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
                    break;
                case 11:
                    if (FGroup.FTransporter != null)
                    {
                        FGroup.DeleteUnit(FGroup.FTransporter);
                    }

                    CombineIdleGroups();
                    Done();
                    break;
            }
        }
    }

    public partial class TRegistrator : Aow.Rtl.TObject
    {
        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // BehaviorAG.pas:136
            public void MapLoaded(Aow.Rtl.TObject Sender)
            {
                if (Sender == Aow.Units.AoWE.AoWE.AoWHSMap)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TDefenderAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TAggressorAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TExpanderAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TScorcherAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TNormalAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.BehaviorAG.TSkirmishAG>()]);
                }
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BehaviorAG.TRegistrator => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class BehaviorAG
    {
        public static void Initialize()
        {
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TDefenderAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TAggressorAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TExpanderAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TScorcherAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TNormalAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BehaviorAG.TSkirmishAG>()]);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Add(Aow.Units.BehaviorAG.TRegistrator.Meta.Of<Aow.Units.BehaviorAG.TRegistrator>().MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Remove(Aow.Units.BehaviorAG.TRegistrator.Meta.Of<Aow.Units.BehaviorAG.TRegistrator>().MapLoaded);
            }
        }
    }
}
