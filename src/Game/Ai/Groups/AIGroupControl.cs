// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

// cycles, group, status, location, army, army sprite, selection, targets, strengths, max
// strength, move table, immunity, predictor, move executer, recruitment, production locations,
// plugin, state
public partial class AIGroupControl : EObject
{

    // AIGroup.pas:973
    public AIGroupControl(ECustomNode owner) : base(owner)
    {
        FTargets = new AIGroupControlTargetList(null);
        FTargets.FControl = this;
        FRecruitmentGroups = new AIRecruitmentGroupList(null);
        FProductionLocations = new AIGroupProductionLocationList(null);
        FCombatPredictor = new CombatPredictor();
    }

    public int FProcessCycles;
    public AIGroup FGroup;
    public Set<StatusElement> FStatus;
    public XYL FLocation;
    public Army FArmy;
    public ArmyHS FArmyHS;
    public byte FSelection;
    public AIGroupControlTargetList FTargets;
    public FixedArray_int_10 FStrength;
    public int FStrengthMax;
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 FMovePointTable;
    public Set<DamageType> FImmunityTypes;
    public CombatPredictor FCombatPredictor;
    public AIMoveExecuter FMoveExecuter;
    public AIRecruitmentGroupList FRecruitmentGroups;
    public AIGroupProductionLocationList FProductionLocations;
    public AIGroupControlPlugin FPlugin;
    public int FState;

    // AIGroup.pas:983
    public override void Dispose()
    {
        DeactivatePlugin();
        FCombatPredictor?.Dispose();
        FTargets?.Dispose();
        FRecruitmentGroups?.Dispose();
        FProductionLocations?.Dispose();
        FGroup.Release();
        base.Dispose();
    }

    // AIGroup.pas:1084
    public virtual void MoveExecuterDone(object Sender)
    {
        if (FGroup.FTransporter != null && !FMoveExecuter.GetTransporting())
        {
            FGroup.DeleteUnit(FGroup.FTransporter);
        }

        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
        {
            FState = 0;
        }
        else
        {
            if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Failed)
            {
                if (!FGroup.FStatus.Contains(AIGroupStatusElement.Split))
                {
                    if (!Split())
                    {
                        Done();
                    }
                }
                else
                {
                    Done();
                }
            }
        }
    }

    // AIGroup.pas:1165
    public virtual void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        FTargets.Add(Target);
    }

    // AIGroup.pas:1155
    public virtual void RecruitmentGroupFoundEvent(AIRecruitmentGroup Group, ref bool SearchDone)
    {
        FRecruitmentGroups.Add(Group);
    }

    // AIGroup.pas:1160
    public virtual void ProductionLocationFoundEvent(AIGroupProductionLocation Location, ref bool SearchDone)
    {
        FProductionLocations.Add(Location);
    }

    // AIGroup.pas:1266
    public virtual bool Validate()
    {
        bool Result;
        if (!FGroup.FStatus.Contains(AIGroupStatusElement.Done))
        {
            FGroup.GetLocation(ref FLocation.X, ref FLocation.Y, ref FLocation.L);
            if (FLocation.X != -1)
            {
                FArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref FLocation))).GetArmyHS();
                RetrieveGroupArmy(FLocation.X, FLocation.Y, FLocation.L, ref FArmy, ref FSelection);
                UpdateGroupChanges();
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:1246
    public virtual void ProcessStates()
    {
        Done();
    }

    // AIGroup.pas:1251
    public virtual void ProcessStatesLooped()
    {
        Done();
    }

    // AIGroup.pas:1170
    public virtual bool Split()
    {
        int I;
        AIGroup Group;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        sbyte UnitX = default;
        sbyte UnitY = default;
        sbyte UnitL = default;
        bool Finished;
        bool Found;
        bool Result = false;
        Finished = false;
        while (!Finished)
        {
            I = FGroup.Count;
            Found = false;
            while (!Found && I != 0)
            {
                I -= 1;
                FGroup.GetUnit(I).GetLocation(ref X, ref Y, ref L);
                Found = X != FLocation.X || Y != FLocation.Y || L != FLocation.L;
            }

            if (Found)
            {
                var behavior = FGroup.GroupClass.Behavior;
                Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, behavior);
                Group.FStatus = Group.FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Split);
                I = FGroup.Count - 1;
                for (; I >= 0; I -= 1)
                {
                    FGroup.GetUnit(I).GetLocation(ref UnitX, ref UnitY, ref UnitL);
                    if (X == UnitX && Y == UnitY && L == UnitL)
                    {
                        Group.AddUnit(FGroup.GetUnit(I));
                    }
                }

                if (Group.Count != 0)
                {
                    FGroup.FStatus = FGroup.FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Split);
                    Result = true;
                }

                Group.Release();
            }
            else
            {
                Finished = true;
            }
        }

        return Result;
    }

    // AIGroup.pas:1214
    public virtual void Activate()
    {
        FState = 0;
        FMoveExecuter = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.FMoveExecuter;
        AoWHSMap.Current.FAIExecuter.UseProcessTime(100);
        FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Activated);
    }

    // AIGroup.pas:1222
    public virtual void Deactivate()
    {
        FMoveExecuter.FOnDone = null;
        FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Activated);
    }

    // AIGroup.pas:1101
    public virtual void ResetME()
    {
        FMoveExecuter.Reset();
        FMoveExecuter.FOnDone = MoveExecuterDone;
    }

    // AIGroup.pas:1107
    public virtual void SetupME()
    {
        AbstractUnit AUnit;
        ResetME();
        AUnit = FGroup.FLeader;
        if (AUnit != null)
        {
            FMoveExecuter.AddUnit(AUnit);
        }

        for (int I = 0, last = FGroup.Count - 1; I <= last; I++)
        {
            if (FGroup.GetUnit(I) != FGroup.FLeader)
            {
                var unit = FGroup.GetUnit(I);
                FMoveExecuter.AddUnit(unit);
            }
        }

        if (!FGroup.GetAdjacent())
        {
            FMoveExecuter.FMode = FMoveExecuter.FMode - Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent);
        }

        AUnit = FGroup.FTransporter;
        if (AUnit != null)
        {
            FMoveExecuter.AddTransporter(AUnit);
        }
    }

    // AIGroup.pas:1124
    public virtual void ExecuteME(sbyte X, sbyte Y, sbyte L)
    {
        AoWMapField Field;
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        if (Field.GetPlayer() != -1 && Field.GetPlayer() != FGroup.FPlayer)
        {
            FMoveExecuter.FMode = FMoveExecuter.FMode + Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent, AIMoveExecuterModeElement.Attack);
        }

        FMoveExecuter.ExecuteMove(X, Y, L);
    }

    // AIGroup.pas:994
    public void Done()
    {
        FGroup.FStatus = FGroup.FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        FState = -1;
    }

    // AIGroup.pas:1000
    public int GetStrengthMax(Set<DamageType> DamageTypes)
    {
        DamageType DamageType;
        int Result = 0;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            DamageType = (DamageType)damageTypeOrdinal;
            if (DamageTypes.Contains(DamageType) && Result < FStrength[(int)DamageType])
            {
                Result = FStrength[(int)DamageType];
            }
        }

        return Result;
    }

    // AIGroup.pas:1010
    public void UpdateGroupChanges()
    {
        DamageType DamageType;
        AbstractUnit AUnit;
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 MovePointTable = default;
        if (FGroup.FStatus.Contains(AIGroupStatusElement.Changed) || FProcessCycles == 1)
        {
            FImmunityTypes = AbilityRules.dtAll;
            FStrengthMax = 0;
            FStrength = default;
            if (FGroup.Count != 0)
            {
                FillBytes(Bytes.Of(ref FMovePointTable, 256), 1);
            }
            else
            {
                FillBytes(Bytes.Of(ref FMovePointTable, 256), -1);
            }

            for (int I = FGroup.Count - 1; I >= 0; I--)
            {
                AUnit = FGroup.GetUnit(I);
                FImmunityTypes = AUnit.GetImmunityTypes() & FImmunityTypes;
                AUnit.CreateMovePointTable(ref MovePointTable);
                AbilityRules.MaximizeMovePointTable(Bytes.Of(ref FMovePointTable), Bytes.Of(ref MovePointTable));
                for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                {
                    DamageType = (DamageType)damageTypeOrdinal;
                    var strength = AUnit.GetStrength(DamageType);
                    FStrength[(int)DamageType] += strength;
                }

                var strengthMax = AUnit.GetStrengthMax(AbilityRules.dtAll);
                FStrengthMax += strengthMax;
            }

            FGroup.FStatus = FGroup.FStatus - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
        }
    }

    // AIGroup.pas:1040
    public bool RetrieveGroupArmy(sbyte X, sbyte Y, sbyte L, ref Army Army, ref byte Selection)
    {
        bool Result;
        ArmyHS ArmyHS;
        ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
        if (ArmyHS != null)
        {
            Army = ArmyHS.FArmy;
            Selection = ArmyHS.FArmy.GetGroupSelection(FGroup.FGroupID);
            Result = true;
        }
        else
        {
            Selection = 0;
            Army = null;
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:1059
    public void CombineIdleGroups()
    {
        Player Player;
        AIGroup Group;
        XYL Location = default;
        Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer);
        for (int I = Player.FAIGroups.Count - 1; I >= 0; I--)
        {
            Group = Player.FAIGroups.GetGroups(I);
            if (Group != FGroup && Group.EClass.ClassID == FGroup.EClass.ClassID && Group.FStatus.Contains(AIGroupStatusElement.Done))
            {
                switch (Group.FTargetType)
                {
                    case AIGroupTargetStatus.Idle:
                        Unsafe.As<XYL, int>(ref Location) = Group.GetLoc();
                        if (Location.L == FLocation.L && HexGeometry.dHXtoRad(Location.X, Location.Y, FLocation.X, FLocation.Y) <= 1)
                        {
                            FGroup.AddUnits(Group);
                        }

                        break;
                }
            }
        }
    }

    // AIGroup.pas:1134
    public void ResetCP()
    {
        FCombatPredictor.DeleteOpponents();
        for (int I = FCombatPredictor.FSides[0].Count - 1; I >= 0; I--)
        {
            var units = FCombatPredictor.FSides[0].GetUnits(I);
            if (FGroup.IndexOf(units.FUnit) == -1)
            {
                var units2 = FCombatPredictor.FSides[0].GetUnits(I);
                FCombatPredictor.DeleteUnit(units2.FUnit);
            }
        }

        for (int I = FGroup.Count - 1; I >= 0; I--)
        {
            var unit = FGroup.GetUnit(I);
            if (FCombatPredictor.FSides[0].IndexOfUnit(unit) == -1)
            {
                var fGroupUnit = FGroup.GetUnit(I);
                FCombatPredictor.AddUnit(fGroupUnit);
            }
        }
    }

    // AIGroup.pas:1147
    public void SetupCP(AIGroupControlTarget Target)
    {
        ResetCP();
        if (((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref Target.FLocation))).GetMoveAction(FGroup.FPlayer) != MoveAction.Negotiate)
        {
            FCombatPredictor.InitAdjacentOpponents((byte)FGroup.FPlayer, (byte)FGroup.FIndependentRelation, Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L);
        }
    }

    // AIGroup.pas:1228
    public void ActivatePlugin(AIGroupControlPlugin Plugin)
    {
        DeactivatePlugin();
        FPlugin = Plugin;
        FPlugin.AddRef();
        FPlugin.Activate(this);
    }

    // AIGroup.pas:1236
    public void DeactivatePlugin()
    {
        if (this.FPlugin != null)
        {
            this.FPlugin.Release();
            this.FPlugin = null;
        }
    }

    // AIGroup.pas:1256
    public void MainProcessStates(bool Looped)
    {
        try
        {
            UpdateGroupChanges();
            if (Looped)
            {
                ProcessStatesLooped();
            }
            else
            {
                ProcessStates();
            }
        }
        catch
        {
            throw;
        }
    }

    // AIGroup.pas:1285
    public bool Process(bool Looped)
    {
        bool Result;
        bool Busy;
        FProcessCycles += 1;
        if (Validate())
        {
            try
            {
                Busy = false;
                while (!Busy && !FGroup.FStatus.Contains(AIGroupStatusElement.Done) && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() >= 0 && FMoveExecuter.FStatus != AIMoveExecuter.Status.Busy)
                {
                    if (FPlugin != null)
                    {
                        Busy = FPlugin.MainProcess();
                        if (!Busy)
                        {
                            DeactivatePlugin();
                            if (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() >= 0 && !FGroup.FStatus.Contains(AIGroupStatusElement.Done))
                            {
                                MainProcessStates(Looped);
                            }
                        }
                    }
                    else
                    {
                        MainProcessStates(Looped);
                    }
                }
            }
            catch
            {
                Done();
            }

            Result = !FGroup.FStatus.Contains(AIGroupStatusElement.Done);
        }
        else
        {
            Done();
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:1323
    public AbstractUnit FindAvailableTransporter()
    {
        AbstractUnit Result;
        int Distance;
        AbstractUnit AUnit;
        DoubleItemList<AbstractUnit> List;
        XYL Location = default;
        Player Player;
        List = new DoubleItemList<AbstractUnit>();
        Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer);
        for (int I = 0, last = Player.FArmies.Count - 1; I <= last; I++)
        {
            AUnit = Player.FArmies.GetArmy(I).Transporter(255);
            if (AUnit != null && (AUnit.FGroup == null || AUnit.FGroup.EClass.ClassID == 136295))
            {
                AUnit.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
                Distance = HexGeometry.dHXtoRad(Location.X, Location.Y, FLocation.X, FLocation.Y);
                if (FLocation.L == Location.L && Distance < 30)
                {
                    List.AddAB(AUnit, Distance);
                }
            }
        }

        if (List.Count != 0)
        {
            List.SortAscendingItemB();
            Result = List[0].A;
        }
        else
        {
            Result = null;
        }

        List?.Dispose();
        return Result;
    }

    public enum StatusElement : byte
    {
        Activated = 0, Loop = 1
    }

    // intf_P5_07.inc:259

}
