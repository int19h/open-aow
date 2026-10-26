// Generated from MainAG.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.AGCPlugins;
using static Aow.Units.AGCPlugins.AGCPlugins;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MainAG
{
    // { TPassiveAG (instance size 80, VMT 557D9ED8) }
    public partial class TPassiveAG : Aow.Units.AoWE.TAIGroup
    {
        // MainAG.pas:122
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 0;
            FRecruitmentPriority = 1;
        }

        // MainAG.pas:138
        public override int GetLoc()
        {
            int Result = default;
            Aow.Units.HSEngine.TXYL Location = default;
            if (FTargetMode == unchecked((byte)Aow.Units.AoWE.TGroupDefendMode.gdmCustom))
            {
                Location = FTargetXYL;
            }
            else
            {
                Location.XYL = base.GetLoc();
            }

            Result = Location.XYL;
            return Result;
        }

        // MainAG.pas:147
        public override bool GetRecruitableUnits(int Phase, Aow.Units.AoWE.TUnitList Units, Aow.Units.AoWE.TUnitList Recruitable)
        {
            bool Result = default;
            Aow.Units.HSEngine.TXYL Location = default;
            Aow.Units.AoWE.TGroupDefenseSelector Selector = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            var call__1 = GetLoc();
            Location.XYL = call__1;
            Result = false;
            Selector = Aow.Units.AoWE.TGroupDefenseSelector.Create();
            Selector.Execute(this, unchecked((sbyte)Location.X), unchecked((sbyte)Location.Y), unchecked((sbyte)Location.L), Units);
            var call__2 = Selector.FDefenders.Count;
            if (call__2 != 0)
            {
                {
                    I = Units.Count - 1;
                    byte end__3 = 0;
                    for (; I >= end__3; I -= 1)
                    {
                        AUnit = Units.GetUnit(I);
                        var call__4 = Selector.FDefenders.IndexOf(AUnit);
                        if (call__4 == -1)
                        {
                            Recruitable.AddUnit(AUnit);
                            Result = true;
                        }
                    }
                }
            }
            else
            {
                Recruitable.AddUnits(Units);
                Result = true;
            }

            Selector.Free();
            return Result;
        }

        // MainAG.pas:26
        public static new Aow.Units.MainAG.TPassiveAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.MainAG.TPassiveAG o = new Aow.Units.MainAG.TPassiveAG();
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
            // MainAG.pas:129
            public override int ClassID()
            {
                int Result = default;
                Result = 136280;
                return Result;
            }

            // MainAG.pas:135
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.MainAG.TPassiveAGC>();
                return Result;
            }

            // MainAG.pas:132
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbPassive;
                return Result;
            }

            // MainAG.pas:26
            public new Aow.Units.MainAG.TPassiveAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainAG.TPassiveAG o = (Aow.Units.MainAG.TPassiveAG)NewInstance();
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
                where T : Aow.Units.MainAG.TPassiveAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPassiveAGC (instance size 368, VMT 557DA028) }
    public partial class TPassiveAGC : Aow.Units.AoWE.TAIGroupControl
    {
        // MainAG.pas:199
        public override void ProcessStates()
        {
            Aow.Units.HSEngine.TXYL Location = default;
            Aow.Units.AoWE.TGroupDefenseSelector Selector = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            switch (FState)
            {
                case 0:
                    CombinePassiveGroups();
                    var call__5 = FGroup.GetLoc();
                    Location.XYL = call__5;
                    Selector = Aow.Units.AoWE.TGroupDefenseSelector.Create();
                    Selector.Execute(FGroup, unchecked((sbyte)Location.X), unchecked((sbyte)Location.Y), unchecked((sbyte)Location.L), null);
                    var call__6 = Selector.FDefenders.Count;
                    var call__7 = FGroup.Count;
                    if ((call__6 != call__7) && (FGroup.FPlayer != 0))
                    {
                        var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                        var call__9 = call__8.FAIControl.GetRandomGroupBehavior();
                        var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(unchecked((sbyte)FGroup.FPlayer), unchecked((sbyte)FGroup.FIndependentRelation), call__9);
                        Group = call__10;
                        {
                            I = FGroup.Count - 1;
                            byte end__11 = 0;
                            for (; I >= end__11; I -= 1)
                            {
                                var call__12 = FGroup.GetUnit(I);
                                AUnit = call__12;
                                var call__13 = Selector.FDefenders.IndexOf(AUnit);
                                if (call__13 == -1)
                                {
                                    Group.AddUnit(AUnit);
                                }
                            }
                        }

                        Group.Release();
                    }

                    if (Selector.FDefenseLocation.XYL != 16777215)
                    {
                        FGroup.FTargetXYL = Selector.FDefenseLocation;
                        FGroup.FTargetMode = unchecked((byte)unchecked((byte)Aow.Units.AoWE.TGroupDefendMode.gdmCustom));
                        SetupME();
                        FMoveExecuter.FMode = FMoveExecuter.FMode - TSet<Aow.Units.AoWE.TAIMoveExecuterModeElement>.Of(Aow.Units.AoWE.TAIMoveExecuterModeElement.memAdjacent);
                        ExecuteME(unchecked((sbyte)Selector.FDefenseLocation.X), unchecked((sbyte)Selector.FDefenseLocation.Y), unchecked((sbyte)Selector.FDefenseLocation.L));
                        Done();
                    }
                    else
                    {
                        Done();
                    }

                    Selector.Free();
                    break;
            }
        }

        // MainAG.pas:176
        public void CombinePassiveGroups()
        {
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Units.HSEngine.TXYL Location = default;
            Aow.Units.HSEngine.TXYL OtherLocation = default;
            int I = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
            Player = call__14;
            var call__15 = FGroup.GetLoc();
            Location.XYL = call__15;
            {
                I = Player.FAIGroups.Count - 1;
                byte end__16 = 0;
                for (; I >= end__16; I -= 1)
                {
                    var call__17 = Player.FAIGroups.GetGroups(I);
                    Group = call__17;
                    bool bool__18 = (FGroup.FGroupID != Group.FGroupID);
                    if (bool__18)
                    {
                        var call__19 = Group.ClassType.ClassID();
                        bool__18 = (call__19 == 136280);
                    }

                    if (bool__18 && (FGroup.FIndependentRelation == Group.FIndependentRelation))
                    {
                        var call__20 = Group.GetLoc();
                        OtherLocation.XYL = call__20;
                        bool bool__21 = (Location.L == OtherLocation.L);
                        if (bool__21)
                        {
                            var call__22 = Aow.Units.HSEngine.HSEngine.dHXtoRad(Location.X, Location.Y, OtherLocation.X, OtherLocation.Y);
                            bool__21 = (call__22 <= 1);
                        }

                        if (bool__21)
                        {
                            FGroup.AddUnits(Group);
                        }
                    }
                }
            }
        }
    }

    // { TFortifyAG (instance size 80, VMT 557DA118) }
    public partial class TFortifyAG : Aow.Units.AoWE.TAIGroup
    {
        // MainAG.pas:240
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 15;
            FRecruitmentPriority = 0;
        }

        // MainAG.pas:44
        public static new Aow.Units.MainAG.TFortifyAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.MainAG.TFortifyAG o = new Aow.Units.MainAG.TFortifyAG();
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
            // MainAG.pas:247
            public override int ClassID()
            {
                int Result = default;
                Result = 136281;
                return Result;
            }

            // MainAG.pas:253
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.MainAG.TFortifyAGC>();
                return Result;
            }

            // MainAG.pas:250
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbFortify;
                return Result;
            }

            // MainAG.pas:44
            public new Aow.Units.MainAG.TFortifyAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainAG.TFortifyAG o = (Aow.Units.MainAG.TFortifyAG)NewInstance();
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
                where T : Aow.Units.MainAG.TFortifyAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFortifyAGC (instance size 384, VMT 557DA268): $170 city location,
//     $174 enemy strength, $178 required strength, $17C target selector. }
    public partial class TFortifyAGC : Aow.Units.AoWE.TAIGroupControl
    {
        public Aow.Units.HSEngine.TXYL FFortifyLocation;
        // { $170 }
        public int FEnemyStrength;
        // { $174 }
        public int FRequiredStrength;
        // { $178 }
        public Aow.Units.AoWE.TAIGroupTargetSelector FTargetSelector;
        // MainAG.pas:262
        // { $17C }
        public override void Destroy()
        {
            FTargetSelector.Release();
            base.Destroy();
        }

        // MainAG.pas:256
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__23 = Aow.Units.AoWE.TAIGroupTargetSelector.Create(null);
            FTargetSelector = call__23;
        }

        // MainAG.pas:278
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            switch (((Aow.Units.AoWE.TAIGroupTargetFinder)Sender).FSelector.FTargetInfo.Mode)
            {
                case Aow.Units.AoWE.TAITargetMode.tmFortify:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case Aow.Units.AoWE.TAITargetMode.tmAttackArmies:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case Aow.Units.AoWE.TAITargetMode.tmAttack:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
            }
        }

        // MainAG.pas:407
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AoWE.TAIRecruitmentGroupFinder Recruitment = default;
            Aow.Units.AoWE.TAIProductionLocationFinder ProductionFinder = default;
            Aow.Units.AoWE.TAIGroupRecruitControl Recruit = default;
            Aow.Units.AoWE.TAIGroupProductionControl Produce = default;
            Aow.Units.AoWE.TAIGroupDefenseControl Defense = default;
            Aow.Units.AoWE.TAIGroupControlTarget CurrentTarget = default;
            Aow.Units.AoWE.TAIGroupControlTarget NewTarget = default;
            int I = default;
            int Excess = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.City.TCity ACity = default;
            switch (FState)
            {
                case 0:
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = Aow.Units.AoWE.TAITargetMode.tmFortify;
                    Finder.FMode = Finder.FMode + TSet<Aow.Units.AoWE.TAIGroupTargetFinderModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                    Finder.FRange = 12;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    I = 0;
                    while (true)
                    {
                        var call__24 = FTargets.Count;
                        bool bool__25 = (I < call__24);
                        if (bool__25)
                        {
                            var call__26 = FTargets.GetTargets(I);
                            bool__25 = call__26.FStatus.Contains(Aow.Units.AoWE.TAIGroupControlTargetStatusElement.gtsDone);
                        }

                        if (!bool__25)
                            break;
                        {
                            I += 1;
                        }
                    }

                    var call__27 = FTargets.Count;
                    if (I < call__27)
                    {
                        var call__28 = FTargets.GetTargets(I);
                        var call__29 = GetSurroundingEnemyStrength(call__28.FLocation, 70);
                        var call__30 = FTargets.GetTargets(I);
                        call__30.FDataA = call__29;
                        var call__31 = FTargets.GetTargets(I);
                        var call__32 = FTargets.GetTargets(I);
                        call__32.FStatus = call__31.FStatus + TSet<Aow.Units.AoWE.TAIGroupControlTargetStatusElement>.Of(Aow.Units.AoWE.TAIGroupControlTargetStatusElement.gtsDone);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    FTargets.Validate();
                    NewTarget = null;
                    var call__33 = FTargets.IndexOfXYL(FLocation.XYL);
                    I = call__33;
                    if (I == -1)
                    {
                        CurrentTarget = null;
                    }
                    else
                    {
                        var call__34 = FTargets.GetTargets(I);
                        CurrentTarget = call__34;
                    }

                {
                    I = 0;
                    int end__35 = FTargets.Count - 1;
                    for (; I <= end__35; I += 1)
                    {
                        var call__36 = FTargets.GetTargets(I);
                        var call__37 = FTargets.GetTargets(I);
                        var call__38 = FTargets.GetTargets(I);
                        call__38.FDataB = (call__36.FData.Priority * 10) + (call__37.FMovePoints * -2);
                        var call__39 = FTargets.GetTargets(I);
                        Excess = call__39.FDataA - FStrengthMax;
                        if (Excess > 0)
                        {
                            var call__40 = FTargets.GetTargets(I);
                            call__40.FDataB -= (int)(((uint)Excess) >> 4);
                        }

                        var call__41 = FTargets.GetTargets(I);
                        Excess = call__41.FPathStrength;
                        if (Excess > 0)
                        {
                            var call__42 = FTargets.GetTargets(I);
                            call__42.FDataB -= (int)(((uint)Excess) >> 3);
                        }

                        var call__43 = FTargets.GetTargets(I);
                        if (call__43.FLocation.XYL != FLocation.XYL)
                        {
                            var call__44 = FTargets.GetTargets(I);
                            var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(call__44.FLocation.XYL);
                            var call__46 = ((Aow.Units.AoWE.TAoWMapField)call__45).GetArmyHS();
                            Army = call__46;
                            if (Army != null)
                            {
                                var call__47 = Army.FArmy.Count;
                                var call__48 = FTargets.GetTargets(I);
                                call__48.FDataB -= call__47 << 2;
                            }
                        }
                    }
                }

                    FTargets.SortOnPriority();
                    var call__49 = FTargets.Count;
                    bool bool__50 = (call__49 != 0);
                    if (bool__50)
                    {
                        var call__51 = FTargets.GetTargets(0);
                        bool__50 = (call__51 != CurrentTarget);
                    }

                    if (bool__50)
                    {
                        FTargets.SortOnDataB();
                        bool bool__52 = (CurrentTarget == null);
                        if (!bool__52)
                        {
                            var call__53 = FTargets.GetTargets(0);
                            bool__52 = (CurrentTarget.FData.Priority < call__53.FData.Priority);
                        }

                        if (bool__52)
                        {
                            var call__54 = FTargets.GetTargets(0);
                            NewTarget = call__54;
                        }
                        else
                        {
                            NewTarget = CurrentTarget;
                        }
                    }

                    if ((NewTarget != null) && (NewTarget != CurrentTarget))
                    {
                        FFortifyLocation = NewTarget.FLocation;
                    }
                    else
                    {
                        if (CurrentTarget != null)
                        {
                            FFortifyLocation = CurrentTarget.FLocation;
                        }
                        else
                        {
                            FFortifyLocation.XYL = 16777215;
                        }
                    }

                    FState += 1;
                    break;
                case 3:
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        var call__55 = GetSurroundingEnemyStrength(FLocation, 70);
                        FEnemyStrength = call__55;
                    }
                    else
                    {
                        var call__56 = GetSurroundingEnemyStrength(FFortifyLocation, 70);
                        FEnemyStrength = call__56;
                    }

                    UpdateRequiredStrength();
                    FState += 1;
                    break;
                case 4:
                    Recruitment = Aow.Units.AoWE.TAIRecruitmentGroupFinder.Create(null);
                    Recruitment.Setup(this);
                    ActivatePlugin(Recruitment);
                    Recruitment.Release();
                    FState += 1;
                    break;
                case 5:
                    FProductionLocations.Clear();
                    ProductionFinder = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                    ProductionFinder.Setup(this);
                    ProductionFinder.FSelector.FMovePointsMax = 24;
                    ActivatePlugin(ProductionFinder);
                    ProductionFinder.Release();
                    FState += 1;
                    break;
                case 6:
                    var call__57 = FRecruitmentGroups.Count;
                    if ((call__57 != 0) && (FRequiredStrength > FStrengthMax))
                    {
                        Recruit = Aow.Units.AoWE.TAIGroupRecruitControl.Create(null);
                        Recruit.Setup(this);
                        var call__58 = FGroup.FProductions.GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                        Recruit.FRecruitStrength = FRequiredStrength - (call__58 + FStrengthMax);
                        Recruit.FMode = Aow.Units.AoWE.TAIGroupRecruitControlMode.rcmAutoRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 7:
                    FRecruitmentGroups.Validate(this);
                    bool bool__59 = (FStrengthMax < FRequiredStrength);
                    if (bool__59)
                    {
                        var call__60 = FRecruitmentGroups.Count;
                        bool__59 = (call__60 != 0);
                    }

                    if (bool__59)
                    {
                        Recruit = Aow.Units.AoWE.TAIGroupRecruitControl.Create(null);
                        Recruit.Setup(this);
                        var call__61 = FGroup.FProductions.GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                        Recruit.FRecruitStrength = FRequiredStrength - (call__61 + FStrengthMax);
                        Recruit.FMode = Aow.Units.AoWE.TAIGroupRecruitControlMode.rcmForceRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                    }

                    FState = 10;
                    break;
                case 10:
                    bool bool__62 = (FStrengthMax >= FEnemyStrength);
                    if (bool__62)
                    {
                        var call__63 = FGroup.GetAdjacent();
                        bool__62 = call__63;
                    }

                    if (bool__62)
                    {
                        FTargets.Clear();
                        Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                        Finder.Setup(this);
                        Finder.FSelector.FTargetInfo.Mode = Aow.Units.AoWE.TAITargetMode.tmAttackArmies;
                        Finder.FSelector.FMovePointsMax = 55;
                        if (FFortifyLocation.XYL == 16777215)
                        {
                            Finder.FSelector.FSourceLocation = FLocation;
                        }
                        else
                        {
                            Finder.FSelector.FSourceLocation = FFortifyLocation;
                        }

                        Finder.FMode = Finder.FMode + TSet<Aow.Units.AoWE.TAIGroupTargetFinderModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                        Finder.FRange = 12;
                        ActivatePlugin(Finder);
                        Finder.Release();
                        FState += 1;
                    }
                    else
                    {
                        if (FStrengthMax < FEnemyStrength)
                        {
                            FState = 18;
                        }
                        else
                        {
                            FState = 17;
                        }
                    }

                    break;
                case 11:
                    var call__64 = FTargets.Count;
                    if (call__64 >= 1)
                    {
                        var call__65 = FTargets.Count;
                        bool bool__66 = (call__65 == 1);
                        if (!bool__66)
                        {
                            var call__67 = FortifyCity();
                            bool__66 = (call__67 == null);
                        }

                        bool bool__68 = bool__66;
                        if (!bool__68)
                        {
                            var call__69 = CanBreachWall();
                            bool__68 = call__69;
                        }

                        if (bool__68)
                        {
                            FTargetSelector.Setup(this);
                            FTargetSelector.FPathStrengthFactor = (float)1.5;
                            FTargetSelector.FCurrentPathStrengthFactor = (float)1.3;
                            FTargetSelector.FSuperiority = (float)1.6;
                            FTargetSelector.FNearSuperiority = (float)1.4;
                            FTargetSelector.FMode = FTargetSelector.FMode - TSet<Aow.Units.AoWE.TAIGroupTargetSelectorModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetSelectorModeElement.tsmMultiTargets);
                            ActivatePlugin(FTargetSelector);
                            FState += 1;
                        }
                        else
                        {
                            FState = 18;
                        }
                    }
                    else
                    {
                        FState = 13;
                    }

                    break;
                case 12:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 10;
                    }
                    else
                    {
                        if (FFortifyLocation.XYL != 16777215)
                        {
                            SetupME();
                            ExecuteME(unchecked((sbyte)FFortifyLocation.X), unchecked((sbyte)FFortifyLocation.Y), unchecked((sbyte)FFortifyLocation.L));
                        }

                        FState = 20;
                    }

                    break;
                case 13:
                    var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL);
                    var call__71 = call__70.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                    ACity = (Aow.Units.City.TCity)call__71;
                    if ((ACity != null) && (ACity.GetActiveAction() == Aow.Units.City.TCityAction.caMigrate))
                    {
                        Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                        Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmCustom;
                        Defense.FCustomState = 19;
                        ActivatePlugin(Defense);
                        Defense.Release();
                    }

                    FState += 1;
                    break;
                case 14:
                    FTargets.Clear();
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = Aow.Units.AoWE.TAITargetMode.tmAttack;
                    Finder.FSelector.FMovePointsMax = 55;
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        Finder.FSelector.FSourceLocation = FLocation;
                    }
                    else
                    {
                        Finder.FSelector.FSourceLocation = FFortifyLocation;
                    }

                    Finder.FMode = Finder.FMode + TSet<Aow.Units.AoWE.TAIGroupTargetFinderModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                    Finder.FRange = 12;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 15:
                    var call__72 = FTargets.Count;
                    if (call__72 >= 1)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FPathStrengthFactor = (float)1.5;
                        FTargetSelector.FCurrentPathStrengthFactor = (float)1.3;
                        FTargetSelector.FSuperiority = (float)1.6;
                        FTargetSelector.FNearSuperiority = (float)1.4;
                        FTargetSelector.FMode = FTargetSelector.FMode - TSet<Aow.Units.AoWE.TAIGroupTargetSelectorModeElement>.Of(Aow.Units.AoWE.TAIGroupTargetSelectorModeElement.tsmMultiTargets);
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState = 18;
                    }

                    break;
                case 16:
                    if (FTargetSelector.FTargetStatus == Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 13;
                    }
                    else
                    {
                        if ((FFortifyLocation.XYL != 16777215) && !TSet<Aow.Units.AoWE.TAIGroupTargetSelectorStatus>.Of(Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssSettingupTarget, Aow.Units.AoWE.TAIGroupTargetSelectorStatus.tssExecutingTarget).Contains(FTargetSelector.FTargetStatus))
                        {
                            SetupME();
                            ExecuteME(unchecked((sbyte)FFortifyLocation.X), unchecked((sbyte)FFortifyLocation.Y), unchecked((sbyte)FFortifyLocation.L));
                        }

                        FState = 20;
                    }

                    break;
                case 17:
                    if (FStrengthMax >= FEnemyStrength)
                    {
                        var call__73 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL);
                        var call__74 = call__73.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                        ACity = (Aow.Units.City.TCity)call__74;
                        if ((ACity != null) && (ACity.GetActiveAction() == Aow.Units.City.TCityAction.caMigrate))
                        {
                            Defense = Aow.Units.AoWE.TAIGroupDefenseControl.Create(null);
                            Defense.FDefendMode = Aow.Units.AoWE.TGroupDefendMode.gdmCustom;
                            Defense.FCustomState = 19;
                            ActivatePlugin(Defense);
                            Defense.Release();
                        }
                    }

                    FState += 1;
                    break;
                case 18:
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        SetupME();
                        ExecuteME(unchecked((sbyte)FLocation.X), unchecked((sbyte)FLocation.Y), unchecked((sbyte)FLocation.L));
                    }
                    else
                    {
                        SetupME();
                        ExecuteME(unchecked((sbyte)FFortifyLocation.X), unchecked((sbyte)FFortifyLocation.Y), unchecked((sbyte)FFortifyLocation.L));
                    }

                    FState = 20;
                    break;
                case 19:
                    SetupME();
                    ExecuteME(unchecked((sbyte)FLocation.X), unchecked((sbyte)FLocation.Y), unchecked((sbyte)FLocation.L));
                    FState = 20;
                    break;
                case 20:
                    if (FortifyCity() != null)
                    {
                        FState = 21;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 21:
                    ACity = FortifyCity();
                    if ((ACity != null) && (ACity.FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsIdle))
                    {
                        if (ACity.FWallType == 0)
                        {
                            var call__75 = ACity.FortifyCost();
                            var call__76 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                            var call__77 = call__76.FAIBudget.RequestExpense(call__75, 0);
                            if (call__77)
                            {
                                if (ACity.Fortify(false))
                                {
                                    var call__78 = ACity.FortifyCost();
                                    var call__79 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                    call__79.FAIBudget.ExpenseMade(call__78, 0);
                                }
                            }
                        }
                        else
                        {
                            if ((ACity.FWallType == 1) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay > 29))
                            {
                                var call__80 = ACity.FortifyCost();
                                var call__81 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                var call__82 = call__81.FAIBudget.RequestExpense(call__80, 0);
                                if (call__82)
                                {
                                    if (ACity.Fortify(false))
                                    {
                                        var call__83 = ACity.FortifyCost();
                                        var call__84 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                        call__84.FAIBudget.ExpenseMade(call__83, 0);
                                    }
                                }
                            }
                        }
                    }

                    FState += 1;
                    break;
                case 22:
                    var call__85 = FGroup.FProductions.GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                    if ((call__85 + FStrengthMax) < FRequiredStrength)
                    {
                        FProductionLocations.Clear();
                        ProductionFinder = Aow.Units.AoWE.TAIProductionLocationFinder.Create(null);
                        ProductionFinder.Setup(this);
                        ProductionFinder.FSelector.FMovePointsMax = 24;
                        ActivatePlugin(ProductionFinder);
                        ProductionFinder.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 23:
                    var call__86 = FProductionLocations.Count;
                    if (call__86 > 0)
                    {
                        Produce = Aow.Units.AoWE.TAIGroupProductionControl.Create(null);
                        Produce.Setup(this);
                        var call__87 = FGroup.FProductions.GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                        Produce.FProduceStrength = FRequiredStrength - (call__87 + FStrengthMax);
                        Produce.FMode = Aow.Units.AoWE.TAIGroupProductionControlMode.pcmAutoProduce;
                        ActivatePlugin(Produce);
                        Produce.Release();
                    }

                    FState += 1;
                    break;
                case 24:
                    switch (FGroup.FTargetType)
                    {
                        case Aow.Units.AoWE.TAIGroupTargetStatus.gtsNone:
                        case Aow.Units.AoWE.TAIGroupTargetStatus.gtsIdle:
                            MakeUselessUnitsPassive();
                            break;
                    }

                    FState += 1;
                    break;
                case 25:
                    Done();
                    break;
            }
        }

        // MainAG.pas:275
        public override bool Split()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // MainAG.pas:268
        public Aow.Units.City.TCity FortifyCity()
        {
            Aow.Units.City.TCity Result = default;
            if (FFortifyLocation.XYL != 16777215)
            {
                var call__88 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(FFortifyLocation.XYL);
                var call__89 = call__88.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                Result = (Aow.Units.City.TCity)call__89;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // MainAG.pas:287
        public int GetSurroundingEnemyStrength(Aow.Units.HSEngine.TXYL Location, int Radius)
        {
            int Result = default;
            Aow.Units.AoWE.TMoveControlSettings Settings = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Aow.Units.AoWE.TBattleFieldInfo[] Info__cell = new Aow.Units.AoWE.TBattleFieldInfo[1];
            ref Aow.Units.AoWE.TBattleFieldInfo Info = ref Info__cell[0];
            int I = default;
            Settings = Aow.Units.AoWE.TMoveControlSettings.Create(null);
            Settings.FPlayer = unchecked((sbyte)FGroup.FPlayer);
            Settings.FMode = Settings.FMode + TSet<Aow.Units.AoWE.TMoveModeElement>.Of(Aow.Units.AoWE.TMoveModeElement.mmLogProcessedMapFields, Aow.Units.AoWE.TMoveModeElement.mmAI);
            Settings.SetSourcePos(Location.X, Location.Y, Location.L);
            Settings.FMovePointsMax = Radius;
            Settings.FArmyHS = null;
            Settings.FSelection = 0;
            var call__90 = Aow.Units.AoWE.AoWE.MoveTypesToInt(TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtFlying));
            Settings.FMovePointTable = Aow.Units.AoWE.AoWE.MovePointTables[call__90];
            Aow.Units.AoWE.AoWE.AoWHSMap.FMoveControl.ProcessMove(Settings);
            Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Info, unchecked((sbyte)FGroup.FPlayer), unchecked((sbyte)FGroup.FIndependentRelation), TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(Aow.Units.AoWE.TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents), Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
            Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
            Msg.MsgID = 4360;
            Msg.Param[0] = Ptr<Aow.Units.AoWE.TBattleFieldInfo>.To(Info__cell, 0).Box();
            {
                I = Settings.FProcessedMapFields.Count - 1;
                byte end__91 = 0;
                for (; I >= end__91; I -= 1)
                {
                    {
                        var w__92 = (Aow.Units.AoWE.TAoWMapField)Settings.FProcessedMapFields[I];
                        if ((w__92.FFieldFlags & 67108864) != 0)
                        {
                            w__92.SendMapFieldMsg(ref Msg);
                        }
                    }
                }
            }

            Result = Info.Strength;
            Settings.Release();
            return Result;
        }

        // MainAG.pas:315
        public void UpdateRequiredStrength()
        {
            int Strength = default;
            Aow.Units.AoWE.TPlayerStatistics Statistics = default;
            FRequiredStrength = FEnemyStrength;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay > 5)
            {
                Strength = (Aow.Units.AoWE.AoWE.AoWHSMap.FDay / 5) * 5000;
                if (Strength > 50000)
                {
                    Strength = 50000;
                }

                if (Strength > FEnemyStrength)
                {
                    FRequiredStrength = Strength;
                }
            }

            if ((FRequiredStrength > 0) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay < 10))
            {
                FRequiredStrength = Aow.Rtl.Rtl.Round((FRequiredStrength * Aow.Units.AoWE.AoWE.AoWHSMap.FDay) * 0.1);
            }

            var call__93 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
            Statistics = call__93.FStatistics;
            var call__94 = Statistics.FTurnInfo.Count;
            if (call__94 > 0)
            {
                var call__95 = Statistics.FTurnInfo.Count;
                var call__96 = Statistics.FTurnInfo.GetTurnInfo(call__95 - 1);
                Strength = (int)(((uint)call__96.FArmyStrength) >> 1);
                if (Strength < FRequiredStrength)
                {
                    FRequiredStrength = Strength;
                }

                var call__97 = Statistics.FTurnInfo.Count;
                var call__98 = Statistics.FTurnInfo.GetTurnInfo(call__97 - 1);
                Strength = Aow.Rtl.Rtl.Round(call__98.FArmyStrength * 0.1);
                if (Strength > 50000)
                {
                    Strength = 50000;
                }

                if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay < 10)
                {
                    Strength = Aow.Rtl.Rtl.Round((Strength * Aow.Units.AoWE.AoWE.AoWHSMap.FDay) * 0.1);
                }

                if (Strength > FRequiredStrength)
                {
                    FRequiredStrength = Strength;
                }
            }
        }

        // MainAG.pas:352
        public void MakeUselessUnitsPassive()
        {
            int Excess = default;
            int I = default;
            Aow.Units.AoWE.TUnitUserList Units = default;
            Aow.Units.AoWE.TUnitUserList Passive = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            Excess = FStrengthMax - FRequiredStrength;
            if (Excess > 0)
            {
                Units = Aow.Units.AoWE.TUnitUserList.Create(null);
                Passive = Aow.Units.AoWE.TUnitUserList.Create(null);
                Units.AddUnits(FGroup);
                Units.FChildren.Sort(Aow.Units.MainAG.MainAG.CompareFortifyUnits);
                var call__99 = Units.Count;
                I = call__99;
                while ((I != 0) && (Excess > 0))
                {
                    I -= 1;
                    if (!(Units.GetUnit(I) is Aow.Units.AoWE.TLeader))
                    {
                        var call__100 = Units.GetUnit(I);
                        var call__101 = call__100.GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                        Excess -= call__101;
                        if (Excess >= 0)
                        {
                            var call__102 = Units.GetUnit(I);
                            Passive.AddUnit(call__102);
                            var call__103 = Units.GetUnit(I);
                            Units.DeleteUnit(call__103);
                        }
                    }
                }

                var call__104 = Passive.Count;
                if (call__104 != 0)
                {
                    var call__105 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(unchecked((sbyte)FGroup.FPlayer), unchecked((sbyte)FGroup.FIndependentRelation), Aow.Units.AoWE.TGroupBehavior.gbPassive);
                    Group = call__105;
                    Group.AddUnits(Passive);
                    Group.Release();
                }

                Passive.Free();
                Units.Free();
            }
        }

        // MainAG.pas:391
        public bool CanBreachWall()
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            var call__106 = FGroup.Count;
            I = call__106;
            var call__107 = FGroup.GetStrength(Aow.Units.AoWE.TDamageType.dtWall);
            Result = call__107 != 0;
            while (!Result && (I != 0))
            {
                I -= 1;
                var call__108 = FGroup.GetUnit(I);
                AUnit = call__108;
                var call__109 = AUnit.GetAbilityEnabled(52);
                bool bool__110 = call__109;
                if (bool__110)
                {
                    var call__111 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(52);
                    var call__112 = ((Aow.Units.AoWE.TSpellCastingAbility)call__111).CanCastBreachWallSpell(AUnit);
                    bool__110 = call__112;
                }

                Result = bool__110;
            }

            return Result;
        }

        // MainAG.pas:59
        public static new Aow.Units.MainAG.TFortifyAGC Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.MainAG.TFortifyAGC o = new Aow.Units.MainAG.TFortifyAGC();
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

    // { TSuicidalAG (instance size 80, VMT 557DA358) }
    public partial class TSuicidalAG : Aow.Units.AoWE.TAIGroup
    {
        // MainAG.pas:722
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // MainAG.pas:73
        public static new Aow.Units.MainAG.TSuicidalAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.MainAG.TSuicidalAG o = new Aow.Units.MainAG.TSuicidalAG();
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
            // MainAG.pas:729
            public override int ClassID()
            {
                int Result = default;
                Result = 136276;
                return Result;
            }

            // MainAG.pas:735
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.MainAG.TSuicidalAGC>();
                return Result;
            }

            // MainAG.pas:732
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbSuicidal;
                return Result;
            }

            // MainAG.pas:73
            public new Aow.Units.MainAG.TSuicidalAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainAG.TSuicidalAG o = (Aow.Units.MainAG.TSuicidalAG)NewInstance();
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
                where T : Aow.Units.MainAG.TSuicidalAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSuicidalAGC (instance size 368, VMT 557DA4A8) }
    public partial class TSuicidalAGC : Aow.Units.AoWE.TAIGroupControl
    {
        // MainAG.pas:750
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:738
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__113 = FCombatPredictor.Execute();
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crDefeat, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__113))
            {
                var call__114 = FCombatPredictor.GetEfficiency();
                if (call__114 > 0)
                {
                    var call__115 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__115 + ((Target.FData.Priority * 100) - (Target.FMovePoints * 5));
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // MainAG.pas:759
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            switch (FState)
            {
                case 0:
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    Finder.FSelector.FMovePointsMax = 100;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Validate();
                    var call__116 = FTargets.Count;
                    if (call__116 != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var call__117 = FTargets.GetTargets(0);
                        var call__118 = FTargets.GetTargets(0);
                        var call__119 = FTargets.GetTargets(0);
                        ExecuteME(unchecked((sbyte)call__119.FLocation.X), unchecked((sbyte)call__118.FLocation.Y), unchecked((sbyte)call__117.FLocation.L));
                        FTargets.Delete(0);
                    }
                    else
                    {
                        Done();
                    }

                    break;
            }
        }
    }

    // { TTransportAG (instance size 80, VMT 557DA5A0) }
    public partial class TTransportAG : Aow.Units.AoWE.TAIGroup
    {
        // MainAG.pas:796
        public override void Destroy()
        {
            base.Destroy();
        }

        // MainAG.pas:789
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FProcessPriority = 2;
            FRecruitmentPriority = 20;
        }

        // MainAG.pas:91
        public static new Aow.Units.MainAG.TTransportAG Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.MainAG.TTransportAG o = new Aow.Units.MainAG.TTransportAG();
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
            // MainAG.pas:801
            public override int ClassID()
            {
                int Result = default;
                Result = 136295;
                return Result;
            }

            // MainAG.pas:807
            public override Aow.Units.AoWE.TAIGroupControl.Meta GroupControlClass()
            {
                Aow.Units.AoWE.TAIGroupControl.Meta Result = default;
                Result = Aow.Units.AoWE.TAIGroupControl.Meta.Of<Aow.Units.MainAG.TTransportAGC>();
                return Result;
            }

            // MainAG.pas:804
            public override Aow.Units.AoWE.TGroupBehavior Behavior()
            {
                Aow.Units.AoWE.TGroupBehavior Result = default;
                Result = Aow.Units.AoWE.TGroupBehavior.gbTransport;
                return Result;
            }

            // MainAG.pas:91
            public new Aow.Units.MainAG.TTransportAG Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainAG.TTransportAG o = (Aow.Units.MainAG.TTransportAG)NewInstance();
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
                where T : Aow.Units.MainAG.TTransportAG => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTransportAGC (instance size 368, VMT 557DA6F8) }
    public partial class TTransportAGC : Aow.Units.AoWE.TAIGroupControl
    {
        // MainAG.pas:822
        public override void MoveExecuterDone(Aow.Rtl.TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == Aow.Units.AoWE.TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:810
        public override void TargetFoundEvent(Aow.Units.AoWE.TAIGroupControlPlugin Sender, Aow.Units.AoWE.TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__120 = FCombatPredictor.Execute();
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crDefeat, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(call__120))
            {
                var call__121 = FCombatPredictor.GetEfficiency();
                if (call__121 > 75)
                {
                    var call__122 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__122 + ((Target.FData.Priority * 100) - (Target.FMovePoints * 5));
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // MainAG.pas:849
        public override void ProcessStates()
        {
            Aow.Units.AoWE.TAIGroupTargetFinder Finder = default;
            Aow.Units.AGCPlugins.TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    Finder = Aow.Units.AoWE.TAIGroupTargetFinder.Create(null);
                    Finder.Setup(this);
                    Finder.FSelector.FMovePointsMax = 100;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Validate();
                    var call__123 = FTargets.Count;
                    if (call__123 != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var call__124 = FTargets.GetTargets(0);
                        var call__125 = FTargets.GetTargets(0);
                        var call__126 = FTargets.GetTargets(0);
                        ExecuteME(unchecked((sbyte)call__126.FLocation.X), unchecked((sbyte)call__125.FLocation.Y), unchecked((sbyte)call__124.FLocation.L));
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    Wander = Aow.Units.AGCPlugins.TAIGroupWanderControl.Create(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 125;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
                    break;
                case 3:
                    Done();
                    break;
            }
        }

        // MainAG.pas:831
        public override void SetupME()
        {
            Aow.Units.AoWE.TAbstractUnit Transporter = default;
            Aow.Units.AoWE.TArmy Army = default;
            int I = default;
            base.SetupME();
            Transporter = FGroup.FLeader;
            if ((Transporter != null) && (Transporter.Owner is Aow.Units.AoWE.TArmy))
            {
                Army = (Aow.Units.AoWE.TArmy)Transporter.Owner;
                {
                    I = 0;
                    int end__127 = Army.Count - 1;
                    for (; I <= end__127; I += 1)
                    {
                        var call__128 = Army.GetUnit(I);
                        bool bool__129 = (Transporter != call__128);
                        if (bool__129)
                        {
                            var call__130 = Army.GetUnit(I);
                            var call__131 = FMoveExecuter.FUnits.IndexOfUnit(call__130);
                            bool__129 = (call__131 == -1);
                        }

                        if (bool__129)
                        {
                            var call__132 = Army.GetUnit(I);
                            FMoveExecuter.AddUnit(call__132);
                        }
                    }
                }
            }
        }
    }

    // { TRegistrator (instance size 4): no fields; map-load registration callback. }
    public partial class TRegistrator : Aow.Rtl.TObject
    {
        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // MainAG.pas:115
            public void MapLoaded(Aow.Rtl.TObject Sender)
            {
                if (Sender == Aow.Units.AoWE.AoWE.AoWHSMap)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.MainAG.TPassiveAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.MainAG.TSuicidalAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.MainAG.TFortifyAG>(), Aow.Units.AoWE.TAIGroup.Meta.Of<Aow.Units.MainAG.TTransportAG>()]);
                }
            }

            public static new Meta Of<T>()
                where T : Aow.Units.MainAG.TRegistrator => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class MainAG
    {
        // MainAG.pas:341
        public static int CompareFortifyUnits(object Item1, object Item2)
        {
            int Result = default;
            int Strength1 = default;
            int Strength2 = default;
            Strength1 = ((Aow.Units.AoWE.TAbstractUnit)Item1).GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
            Strength2 = ((Aow.Units.AoWE.TAbstractUnit)Item2).GetStrengthMax(Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
            if ((Aow.Rtl.TObject)Item1 is Aow.Units.AoWE.TLeader)
            {
                // { Original uses Strength2. }
                Strength1 = Strength2 + 16777215;
            }

            if ((Aow.Rtl.TObject)Item2 is Aow.Units.AoWE.TLeader)
            {
                Strength2 += 16777215;
            }

            Result = Strength2 - Strength1;
            return Result;
        }

        public static void Initialize()
        {
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainAG.TSuicidalAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainAG.TFortifyAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainAG.TPassiveAG>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainAG.TTransportAG>()]);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Add(Aow.Units.MainAG.TRegistrator.Meta.Of<Aow.Units.MainAG.TRegistrator>().MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Remove(Aow.Units.MainAG.TRegistrator.Meta.Of<Aow.Units.MainAG.TRegistrator>().MapLoaded);
            }
        }
    }
}
