using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public static class AbilityRules
{
    internal static AIGroup_Deactivate__TOriginalAssertText TAIGroup_Deactivate__OriginalAssertFile = new AIGroup_Deactivate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 30,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 73, 71, 114, 111, 117, 112, 46, 112, 97, 115, 0])
    };

    internal static AIGroup_Deactivate__TOriginalAssertText TAIGroup_Deactivate__OriginalAssertMessage = new AIGroup_Deactivate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static SpellControl_GetSpell__TOriginalAssertText TSpellControl_GetSpell__OriginalAssertFile = new SpellControl_GetSpell__TOriginalAssertText
    {
        RefCount = -1,
        Len = 29,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 83, 112, 101, 108, 108, 115, 46, 80, 65, 83, 0])
    };

    internal static SpellControl_GetSpell__TOriginalAssertText TSpellControl_GetSpell__OriginalAssertMessage = new SpellControl_GetSpell__TOriginalAssertText
    {
        RefCount = -1,
        Len = 15,
        Text = FixedArray_byte_128.From([73, 110, 118, 97, 108, 105, 100, 32, 83, 112, 101, 108, 108, 73, 68, 0])
    };

    internal static Set<DamageType> TWallUnit_GetImmunityTypes__AllDamageTypes = Set<DamageType>.FromMask(1023UL);

    internal static Set<DamageType> TWallUnit_GetImmunityTypes__WallDamageTypes = Set<DamageType>.FromMask(256UL);

    internal static string TranslateRStr(string S)
    {
        if (GameTables.AoWEngine != null)
        {
            return Aow.Game.Magic.Abilities.GameTables.AoWEngine.TranslateRStr(S);
        }

        return S;
    }

    internal static int DMmaxDCtoDV(int Damage, int AttackDefense, int Maximum)
    {
        int Result;
        if (Damage > 0)
        {
            Result = Aow.Game.Magic.Abilities.AbilityRules.DMDCtoDV(Damage, AttackDefense);
            // { Original updates only the unused Maximum parameter, leaving Result uncapped. }
            if (Damage > Maximum)
            {
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static int DMDCtoDV(int Damage, int AttackDefense)
    {
        int Result;
        int Threshold;
        int Correction;
        if (Damage > 0)
        {
            Threshold = 10 - AttackDefense * 2;
            if (Threshold > 18)
            {
                Result = Damage;
            }
            else
            {
                if (Threshold >= 0)
                {
                    Result = (int)(((uint)((Damage - 1) * (18 - Threshold) + 1)) >> 1) + (Damage + (18 - Threshold));
                    if (Threshold != 0)
                    {
                        Result += 1;
                    }
                }
                else
                {
                    Result = Damage + 1 + (18 - Threshold) + (int)(((uint)((Damage - 1) * (18 - Threshold) + 1)) >> 1);
                    Threshold = -Threshold;
                    Correction = (int)(((uint)(((Damage - 1) * Threshold * Threshold + (Threshold + 19)) / (Threshold + 19))) >> 1);
                    Threshold = Correction + Threshold + 1;
                    Result = Result - Threshold;
                }
            }

            Result = Result * 5;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static int ExecuteDamageRole(int Damage, int AttackDefense)
    {
        int Result;
        int Roll;
        if (Damage > 0)
        {
            Roll = Random(20);
            if (Roll <= 1)
            {
                Result = 0;
            }
            else
            {
                if (Roll >= 18)
                {
                    Result = Damage;
                }
                else
                {
                    AttackDefense = 10 - AttackDefense * 2;
                    if (Roll >= AttackDefense && AttackDefense < 18)
                    {
                        Result = ((Damage - 1) * (Roll - AttackDefense) + (int)(((uint)(18 - AttackDefense)) >> 1)) / (18 - AttackDefense) + 1;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static int DVtoDEV(int SourceDV, int TargetDV, CombatObject Source, CombatObject Target)
    {
        int Result;
        if (Target.GetTargetPriority() != 0)
        {
            var targetPriority = Target.GetTargetPriority();
            var hitPoints = Target.GetHitPoints();
            Result = Target.GetTargetPriority() + Round((double)targetPriority * ((double)SourceDV / (hitPoints * 100)));
            if (TargetDV != 0)
            {
                var sourceTargetPriority = Source.GetTargetPriority();
                var sourceHitPoints = Source.GetHitPoints();
                Result = Result - (Source.GetTargetPriority() + Round((double)sourceTargetPriority * ((double)TargetDV / (sourceHitPoints * 100))));
            }
        }
        else
        {
            Result = SourceDV + -99999 - Source.GetTargetPriority() * TargetDV;
        }

        return Result;
    }

    internal static int StatisticsToMaxDamage(int Damage, Set<DamageType> DamageTypes, Set<DamageType> ImmunityTypes, Set<DamageType> ProtectionTypes)
    {
        int Result;
        DamageTypes = DamageTypes - ImmunityTypes;
        if (DamageTypes != Set<DamageType>.Of())
        {
            if (DamageTypes - ProtectionTypes == Set<DamageType>.Of())
            {
                Result = (int)(((uint)(Damage + 1)) >> 1);
            }
            else
            {
                Result = Damage;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static int StatisticsToLimitedDV(int Attack, int Damage, Set<DamageType> DamageTypes, int Maximum, int Defense, int Resistance, Set<DamageType> ImmunityTypes, Set<DamageType> ProtectionTypes)
    {
        int Result;
        int Limit;
        int Limit100;
        Limit = Maximum;
        DamageTypes = DamageTypes - ImmunityTypes;
        if (DamageTypes != Set<DamageType>.Of())
        {
            Attack = Attack - Defense;
            if (DamageTypes - ProtectionTypes == Set<DamageType>.Of())
            {
                Damage = (int)(((uint)(Damage + 1)) >> 1);
            }

            Result = Aow.Game.Magic.Abilities.AbilityRules.DMmaxDCtoDV(Damage, Attack, Limit);
            Limit100 = Limit * 100;
            if (Result > Limit100)
            {
                Result = Limit100;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static void MaximizeMovePointTable(Span<byte> Table, ReadOnlySpan<byte> Other)
    {
        sbyte OldValue;
        sbyte Value;
        for (int I = 0; I <= 12; I++)
        {
            for (int J = 0; J <= 15; J++)
            {
                OldValue = MemoryMarshal.Cast<byte, FixedArray_Aow_Units_FixedArray_sbyte_16_16>(Table)[0][J][I];
                Value = MemoryMarshal.Cast<byte, FixedArray_Aow_Units_FixedArray_sbyte_16_16>(Other)[0][J][I];
                if (OldValue != -1 && (OldValue < Value || Value == -1))
                {
                    MemoryMarshal.Cast<byte, FixedArray_Aow_Units_FixedArray_sbyte_16_16>(Table)[0][J][I] = Value;
                }
            }
        }
    }

    internal static int RoadConstructionCost(sbyte Player, Movepath Path, int MovePoints)
    {
        int I;
        int Points;
        AoWMapField Field;
        int Result = 0;
        I = Path.Count;
        Points = 0;
        while (I != 0 && Points <= MovePoints)
        {
            I -= 1;
            Points += Path[I].Flag;
            if (Points <= MovePoints)
            {
                var item = Path[I];
                Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetFieldXYL(item.Packed);
                if (Aow.Game.Magic.Abilities.AbilityRules.CanConstructRoad(Player, Field))
                {
                    Result += 10;
                }
            }
        }

        return Result;
    }

    internal static string Format(string S, params ReadOnlySpan<FormatArg> Args)
    {
        string Result;
        try
        {
            Result = RtlUnit.Format(S, Args);
        }
        catch
        {
            Result = S;
        }

        return Result;
    }

    internal static void ShowDamageIcon(int X, int Y, in SurfaceDesc Surface, int Damage, DamageType DamageType)
    {
        if (DamageType != Combat.DamageType.None)
        {
            AoWHSSet.Current.FCombatImages[(int)DamageType + 10].Show(X + 15, Y + 10, Surface);
            if (Set<DamageType>.Of(Combat.DamageType.Cold, Combat.DamageType.Lightning, Combat.DamageType.Magic, Combat.DamageType.Holy).Contains(DamageType))
            {
                GameTables.AoWEngine.FCombatFont.Color = 0;
            }
            else
            {
                GameTables.AoWEngine.FCombatFont.Color = 16777215;
            }

            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 34, Y + 36, IntToStr(Damage), Surface);
        }
    }

    internal static int MoraleToMoraleValue(int Value)
    {
        int Result;
        switch (Value & 127)
        {
            case 0:
                Result = 10;
                break;
            case 1:
                Result = 30;
                break;
            case 2:
                Result = 50;
                break;
            case 3:
                Result = 70;
                break;
            case 4:
                Result = 90;
                break;
            default:
                Result = 50;
                // { Original leaks the exception without raising it. }
                new ArgumentException("Invalid TMoral Value");
                break;
        }

        return Result;
    }

    internal static AbstractUnit.Morale MoraleValueToMorale(int Value)
    {
        AbstractUnit.Morale Result;
        if (Value <= 20)
        {
            Result = AbstractUnit.Morale.Terrible;
        }
        else
        {
            if (Value > 80)
            {
                Result = AbstractUnit.Morale.High;
            }
            else
            {
                switch (Value)
                {
                    case >= 21 and <= 40:
                        Result = AbstractUnit.Morale.Poor;
                        break;
                    case >= 41 and <= 60:
                        Result = AbstractUnit.Morale.OK;
                        break;
                    case >= 61 and <= 80:
                        Result = AbstractUnit.Morale.Good;
                        break;
                    default:
                        Result = AbstractUnit.Morale.OK;
                        // { Original leaks the exception without raising it. }
                        new ArgumentException("Invalid MoralValue");
                        break;
                }
            }
        }

        return Result;
    }

    internal static Status dAlignmentsToStatus(Set<Alignment> A, Set<Alignment> B)
    {
        Status Result;
        if (A == B)
        {
            Result = Status.Friendly;
        }
        else
        {
            if ((A & B) == Set<Alignment>.Of())
            {
                Result = Status.Hostile;
            }
            else
            {
                Result = Status.Neutral;
            }
        }

        return Result;
    }

    internal static int AlsToInt(Set<Alignment> Alignments)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    internal static Set<Alignment> IntToAls(int Value)
    {
        Set<Alignment> Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    internal static int PlaceRandomUnits(int X, int Y, int L, sbyte Player, UnitIndexList Units, int Level, int Value, Set<UnitType> Types)
    {
        int Index;
        int Tries;
        int Difference;
        int BestIndex;
        int I;
        int Delta;
        Unit Unit_;
        int Result = 0;
        Tries = 50;
        if (Units.Count != 0)
        {
            while (Result < Value && Tries != 0)
            {
                Tries -= 1;
                Index = Units[Random(Units.Count)];
                if (Index != -1 && Types.Contains(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FUnitType) && Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).GetObtainValue() + Result > Value && Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FLevel <= Level)
                {
                    BestIndex = Index;
                    I = 20;
                    Difference = Math.Abs(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).GetObtainValue() - (Value - Result));
                    do
                    {
                        Index = Units[Random(Units.Count)];
                        if (Index != -1 && Types.Contains(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FUnitType))
                        {
                            Delta = Math.Abs(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).GetObtainValue() - (Value - Result));
                            if (Delta < Difference)
                            {
                                Difference = Delta;
                                BestIndex = Index;
                            }
                        }

                        I -= 1;
                    }
                    while (!(I == 0));
                    Index = BestIndex;
                }

                if (Index != -1 && Types.Contains(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FUnitType) && Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FLevel <= Level)
                {
                    Unit_ = new Unit(null);
                    Unit_.SetUnitResource(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                    if (Unit_.Place(X, Y, L, Player, Set<UnitPlaceModeElement>.Of()) != null)
                    {
                        Result += Unit_.GetObtainValue();
                        Tries = 50;
                    }

                    Unit_.Release();
                }
            }
        }

        return Result;
    }

    internal static Alignment IRtoAlignment(sbyte Relation)
    {
        Alignment Result;
        if (Relation < 0)
        {
            switch (Relation)
            {
                case -2:
                    Result = Alignment.Good;
                    break;
                case -4:
                    Result = Alignment.Evil;
                    break;
                case -3:
                    Result = Alignment.Neutral;
                    break;
                default:
                    Result = Alignment.Neutral;
                    break;
            }
        }
        else
        {
            Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(Relation).GetAlignment();
        }

        return Result;
    }

    internal static DiplomaticRelation DiplomaticRelation(sbyte Player, sbyte IndependentRelation, sbyte OtherPlayer, sbyte OtherIndependentRelation)
    {
        DiplomaticRelation Result;
        sbyte Race;
        sbyte OtherRace;
        Alignment Alignment;
        Alignment OtherAlignment;
        if (Player == 0 || OtherPlayer == 0)
        {
            if (Player != 0)
            {
                Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace;
            }
            else
            {
                if (IndependentRelation >= 0)
                {
                    Race = IndependentRelation;
                }
                else
                {
                    Race = -1;
                }
            }

            if (OtherPlayer != 0)
            {
                OtherRace = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(OtherPlayer).FPlayerRace;
            }
            else
            {
                if (OtherIndependentRelation >= 0)
                {
                    OtherRace = OtherIndependentRelation;
                }
                else
                {
                    OtherRace = -1;
                }
            }

            if (Race == -1 || OtherRace == -1)
            {
                if (Race != -1)
                {
                    Alignment = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(Race).GetAlignment();
                }
                else
                {
                    Alignment = Aow.Game.Magic.Abilities.AbilityRules.IRtoAlignment(IndependentRelation);
                }

                if (OtherRace != -1)
                {
                    OtherAlignment = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(OtherRace).GetAlignment();
                }
                else
                {
                    OtherAlignment = Aow.Game.Magic.Abilities.AbilityRules.IRtoAlignment(OtherIndependentRelation);
                }

                Result = Aow.Game.Magic.Abilities.AlignmentTables.dAlignmentToDiplomaticRelation(Alignment, OtherAlignment);
            }
            else
            {
                Result = Aow.Game.Magic.Abilities.AlignmentTables.StatusToDiplomaticRelation(Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).GetRelation(OtherRace));
            }
        }
        else
        {
            if (Player != OtherPlayer)
            {
                Result = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, OtherPlayer);
            }
            else
            {
                Result = Session.DiplomaticRelation.Alliance;
            }
        }

        return Result;
    }

    internal static bool ValidAttackRelation(sbyte Player, sbyte IndependentRelation, sbyte TargetPlayer, sbyte TargetRace)
    {
        bool Result;
        DiplomaticRelation Relation;
        if (Player == 0)
        {
            if (TargetPlayer != 0)
            {
                Relation = Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Player, IndependentRelation, TargetPlayer, TargetRace);
                Result = Set<DiplomaticRelation>.Of(Session.DiplomaticRelation.War, Session.DiplomaticRelation.Unknown).Contains(Relation);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).ValidAttackRelation(TargetPlayer, TargetRace);
        }

        return Result;
    }

    internal static void DegreesRadToCo(int Degrees, int Radius, ref int X, ref int Y)
    {
        float Radians;
        Radians = (float)((double)((double)Degrees * Math.PI) / 180);
        X = Round((double)Math.Cos(Radians) * Radius);
        Y = Round((double)Math.Sin(Radians) * Radius);
    }

    internal static string Encode(string Text, int Mode)
    {
        string Result = "";
        for (int I = 1; I <= Text.Length; I++)
        {
            Result = Result + Cp1252.Text((byte)(Cp1252.Byte(Text[I - 1]) ^ ((Text.Length + (I + 5)) & 255)));
        }

        return Result;
    }

    internal static Set<DamageType> InvertDamageTypes(Set<DamageType> Types)
    {
        DamageType DamageType;
        Set<DamageType> Result = Set<DamageType>.Of();
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            DamageType = (DamageType)damageTypeOrdinal;
            if (!Types.Contains(DamageType))
            {
                Result = Result + Set<DamageType>.Of(DamageType);
            }
        }

        return Result;
    }

    internal static int GetMovepathBattleFieldStrength(sbyte Player, sbyte IndependentRelation, Movepath Movepath, int TargetRange, int Range, int Step)
    {
        int Result;
        UnitUserList Units;
        int I;
        int J;
        int Distance;
        HNWalkData Walk = default;
        BattleFieldInfo Info = default;
        MapLevel Level;
        AoWMapField Field;
        ArmyHS Army;
        if (Movepath != null && Movepath.Count > 0)
        {
            Distance = Round((double)Range * 0.5);
            Result = 0;
            I = Movepath.Count - 1;
            Units = null;
            if (Movepath.Count - TargetRange < I)
            {
                I = Movepath.Count - TargetRange;
            }

            Aow.Game.Combat.BattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), dtAll - Set<DamageType>.Of(DamageType.Wall));
            Info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            Info.Requests = Info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents, BattleFieldInfoRequestsElement.EffectiveStrength);
            if (Step > 0)
            {
                Units = new UnitUserList(null);
                var item = Movepath[Movepath.Count - 1];
                Level = AoWHSMap.Current.MapContainer.MapLevels[item.L];
                var movepathItem = Movepath[Movepath.Count - 1];
                var item2 = Movepath[Movepath.Count - 1];
                Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, item2.X, movepathItem.Y, 0);
                while (Walk.Radius <= Step)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if ((Field.FFieldFlags & 67108864) != 0)
                        {
                            Army = Field.GetArmyHS();
                            if (Army != null && Army.FArmy.FPlayer != Player)
                            {
                                Units.AddUnits(Army.FArmy);
                            }
                        }
                    }

                    Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                }

                Info.Units = Units;
            }

            while (I >= 0)
            {
                Info.Strength = 0;
                var item = Movepath[I];
                var movepathItem = Movepath[I];
                var item2 = Movepath[I];
                Aow.Game.World.Map.AoWHSMap.Current.RetrieveBattleFieldInfo(item2.X, movepathItem.Y, item.L, Range, ref Info);
                if (Info.Strength > Result)
                {
                    Result = Info.Strength;
                }

                J = I;
                while (true)
                {
                    bool condition = J >= 0;
                    if (condition)
                    {
                        var item3 = Movepath[J];
                        var item4 = Movepath[I];
                        condition = item3.L == item4.L;
                    }

                    if (!(condition && J > I - Distance))
                        break;
                    J -= 1;
                }

                bool test = J == 0;
                if (!test)
                {
                    var item3 = Movepath[I];
                    var item4 = Movepath[J];
                    test = item3.L != item4.L;
                }

                if (test)
                {
                    if (J - 1 != I)
                    {
                        Info.Strength = 0;
                        var item3 = Movepath[J + 1];
                        var item4 = Movepath[J + 1];
                        var item5 = Movepath[J + 1];
                        Aow.Game.World.Map.AoWHSMap.Current.RetrieveBattleFieldInfo(item5.X, item4.Y, item3.L, Range, ref Info);
                        if (Info.Strength > Result)
                        {
                            Result = Info.Strength;
                        }
                    }
                }

                I = J;
            }

            if (Units != null)
            {
                Units?.Dispose();
            }

            Aow.Game.World.Map.AoWHSMap.Current.FAIExecuter.UseProcessTime(50);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static EnhancementAbility CreateEnhancementAbility(int ID, string Name, Set<AbilitySelectionType> SelectionTypes)
    {
        EnhancementAbility Ability;
        Ability = new EnhancementAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FSelectionTypes = SelectionTypes;
        return Ability;
    }

    internal static FixedArray_int_3 CommandAbilityIDs = FixedArray_int_3.From([29, 28, 148]);

    internal static FixedArray_int_13 RaceColors = FixedArray_int_13.From([14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224]);

    internal static Set<DamageType> dtAll = Set<DamageType>.FromMask(1023UL);

    internal static FixedArray_string_2 ExplosionSFX = FixedArray_string_2.From(["SFX\\ATTACK\\STORM.WAV", "SFX\\ATTACK\\EXPLOD1.WAV"]);

    internal static Set<AbilitySelectionType> astAll = Set<AbilitySelectionType>.Of().WithRange(AbilitySelectionType.Unit, AbilitySelectionType.Editor);

    internal static Set<MagicSphere> msAll = Set<MagicSphere>.Of().WithRange(MagicSphere.Cosmos, MagicSphere.None);

    internal static int AIDID;

    internal static FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12 P2RaceTerrain = FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12.From([new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Desert), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Snow, AoWTerrainType.Ice, AoWTerrainType.UndergroundIce) }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Water), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Desert, AoWTerrainType.Wasteland) }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Snow, AoWTerrainType.Ice, AoWTerrainType.UndergroundIce), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Desert) }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Grass), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Wasteland) }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Grass), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Wasteland) }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Dirt), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Dirt), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Dirt), Disliked = Set<AoWTerrainType>.Of() }, new P2RaceTerrain { Preferred = Set<AoWTerrainType>.Of(AoWTerrainType.Wasteland), Disliked = Set<AoWTerrainType>.Of(AoWTerrainType.Grass) }]);

    internal static FixedArray_string_5 P2AbilityTypeNames = FixedArray_string_5.From(["Enhancement", "Handicap", "Movement", "Combat", "Enchantment"]);

    internal static FixedArray_byte_9 ArmySelectionMasks = FixedArray_byte_9.From([0, 1, 3, 7, 15, 31, 63, 127, 255]);

    internal static int TMPDID;

    internal static EClass P4DurationDataClass = EClass.Of<DurationAbilityData>();

    internal static EClass P4MultiLevelDataClass = EClass.Of<MultiLevelAbilityData>();

    internal static EClass P4CastSpellActionClass = EClass.Of<AICastSpellAction>();

    internal static EClass P4ImageAnimationClass = EClass.Of<ImageSequenceAnimation>();

    internal static int P3LimitedDamage(int Attack, int Defense, int Damage)
    {
        if (Defense > 10)
        {
            Defense = 10;
        }

        return Attack * Defense * Damage / 2;
    }

    internal static bool EndPlayerTurn(sbyte Player)
    {
        EndTurnTE Event;
        Event = new EndTurnTE(null);
        Event.FPlayer = Player;
        Event.FDay = AoWHSMap.Current.FDay;
        Event.FMode = EndTurnTE.EndTurnMode.EndTurn;
        AoWHSMap.Current.FTokenManager.TokenControls[Player].AddEvent(Event);
        Event.Release();
        return true;
    }

    internal static int P4OffensiveStrength(int Attack, int Damage, int Repeats)
    {
        if (Damage > 10)
        {
            Damage = 10;
        }

        return Attack * Damage * Repeats / 2;
    }

    internal static bool CanConstructRoad(sbyte Player, AoWMapField Field)
    {
        bool Result = Player == Field.GetPlayer() || Field.GetPlayer() == -1;
        Result = Result && Set<byte>.WithBounds(0, 255).WithRange(1, 5).Contains((byte)Field.TerrainType);
        Result = Result && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(4).With(5).Contains((byte)Field.OverlayType);
        // Original shared helper
        return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null;
    }

    public static void RetrieveStormDamageInfo(sbyte player, StormType storm, sbyte x, sbyte y,
                sbyte level, sbyte innerRadius, sbyte outerRadius, ref StormDamageInfo info)
    {
        var query = new StormDamageQuery
        {
            Info = new()
            {
                Player = player,
                FriendlyRelations = Set<byte>.WithBounds(0, 7).With(0).With(2),
                EnemyRelations = Set<byte>.WithBounds(0, 7).With(1).With(3)
            }
        };
        var mapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(level);
        var message = new MapFieldMsg { MsgID = 0x1151 };
        message.Param[0] = (int)storm;
        message.Param[2] = query;
        HNWalkData walk = default;
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref walk, x, y, innerRadius);
        try
        {
            while (walk.Radius <= outerRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    message.Param[1] = walk.Radius;
                    mapLevel.GetField(walk.X, walk.Y).SendMapFieldMsg(ref message);
                }
                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }

    internal static Set<MoveType> IntToMoveTypes(int value) => Set<MoveType>.FromMask((byte)value);

    internal static Set<DamageType> AllDamageTypes = Set<DamageType>.FromMask(1023UL);

    internal static void TriggerStormDamage(StormType Storm, sbyte X, sbyte Y, sbyte L, sbyte InnerRadius, sbyte OuterRadius, sbyte Player)
    {
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        MapLevel Level;
        Level = AoWHSMap.Current.MapContainer.MapLevels[L];
        Msg.MsgID = 4432;
        Msg.Param[0] = (int)Storm;
        Msg.Param[2] = Player;
        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, InnerRadius);
        while (Walk.Radius <= OuterRadius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Msg.Param[1] = Walk.Radius;
                Level.Field[Walk.X, Walk.Y].SendMapFieldMsg(ref Msg);
            }

            Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
        }
    }

}
