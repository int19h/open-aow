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
    internal static TAIGroup_Deactivate__TOriginalAssertText TAIGroup_Deactivate__OriginalAssertFile = new TAIGroup_Deactivate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 30,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 73, 71, 114, 111, 117, 112, 46, 112, 97, 115, 0])
    };

    internal static TAIGroup_Deactivate__TOriginalAssertText TAIGroup_Deactivate__OriginalAssertMessage = new TAIGroup_Deactivate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static TSpellControl_GetSpell__TOriginalAssertText TSpellControl_GetSpell__OriginalAssertFile = new TSpellControl_GetSpell__TOriginalAssertText
    {
        RefCount = -1,
        Len = 29,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 83, 112, 101, 108, 108, 115, 46, 80, 65, 83, 0])
    };

    internal static TSpellControl_GetSpell__TOriginalAssertText TSpellControl_GetSpell__OriginalAssertMessage = new TSpellControl_GetSpell__TOriginalAssertText
    {
        RefCount = -1,
        Len = 15,
        Text = FixedArray_byte_128.From([73, 110, 118, 97, 108, 105, 100, 32, 83, 112, 101, 108, 108, 73, 68, 0])
    };

    internal static TSet<TDamageType> TWallUnit_GetImmunityTypes__AllDamageTypes = TSet<TDamageType>.FromMask(1023UL);

    internal static TSet<TDamageType> TWallUnit_GetImmunityTypes__WallDamageTypes = TSet<TDamageType>.FromMask(256UL);

    internal static string TranslateRStr(string S)
    {
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine != null)
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

    internal static int DVtoDEV(int SourceDV, int TargetDV, TCombatObject Source, TCombatObject Target)
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

    internal static int StatisticsToMaxDamage(int Damage, TSet<TDamageType> DamageTypes, TSet<TDamageType> ImmunityTypes, TSet<TDamageType> ProtectionTypes)
    {
        int Result;
        DamageTypes = DamageTypes - ImmunityTypes;
        if (DamageTypes != TSet<TDamageType>.Of())
        {
            if (DamageTypes - ProtectionTypes == TSet<TDamageType>.Of())
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

    internal static int StatisticsToLimitedDV(int Attack, int Damage, TSet<TDamageType> DamageTypes, int Maximum, int Defense, int Resistance, TSet<TDamageType> ImmunityTypes, TSet<TDamageType> ProtectionTypes)
    {
        int Result;
        int Limit;
        int Limit100;
        Limit = Maximum;
        DamageTypes = DamageTypes - ImmunityTypes;
        if (DamageTypes != TSet<TDamageType>.Of())
        {
            Attack = Attack - Defense;
            if (DamageTypes - ProtectionTypes == TSet<TDamageType>.Of())
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

    internal static int RoadConstructionCost(sbyte Player, TMovepath Path, int MovePoints)
    {
        int I;
        int Points;
        TAoWMapField Field;
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
                Field = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetFieldXYL(item.XYL);
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

    internal static void ShowDamageIcon(int X, int Y, in TSurfaceDesc Surface, int Damage, TDamageType DamageType)
    {
        if (DamageType != TDamageType.dtNone)
        {
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FCombatImages[(int)DamageType + 10].Show(X + 15, Y + 10, Surface);
            if (TSet<TDamageType>.Of(TDamageType.dtCold, TDamageType.dtLightning, TDamageType.dtMagic, TDamageType.dtHoly).Contains(DamageType))
            {
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.Color = 0;
            }
            else
            {
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.Color = 16777215;
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

    internal static TMorale MoraleValueToMorale(int Value)
    {
        TMorale Result;
        if (Value <= 20)
        {
            Result = TMorale.mTerrible;
        }
        else
        {
            if (Value > 80)
            {
                Result = TMorale.mHigh;
            }
            else
            {
                switch (Value)
                {
                    case >= 21 and <= 40:
                        Result = TMorale.mPoor;
                        break;
                    case >= 41 and <= 60:
                        Result = TMorale.mOK;
                        break;
                    case >= 61 and <= 80:
                        Result = TMorale.mGood;
                        break;
                    default:
                        Result = TMorale.mOK;
                        // { Original leaks the exception without raising it. }
                        new ArgumentException("Invalid MoralValue");
                        break;
                }
            }
        }

        return Result;
    }

    internal static TStatus dAlignmentsToStatus(TSet<TAlignment> A, TSet<TAlignment> B)
    {
        TStatus Result;
        if (A == B)
        {
            Result = TStatus.sFriendly;
        }
        else
        {
            if ((A & B) == TSet<TAlignment>.Of())
            {
                Result = TStatus.sHostile;
            }
            else
            {
                Result = TStatus.sNeutral;
            }
        }

        return Result;
    }

    internal static int AlsToInt(TSet<TAlignment> Alignments)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    internal static TSet<TAlignment> IntToAls(int Value)
    {
        TSet<TAlignment> Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    internal static int PlaceRandomUnits(int X, int Y, int L, sbyte Player, TUnitIndexList Units, int Level, int Value, TSet<TUnitType> Types)
    {
        int Index;
        int Tries;
        int Difference;
        int BestIndex;
        int I;
        int Delta;
        TUnit Unit_;
        int Result = 0;
        Tries = 50;
        if (Units.Count != 0)
        {
            while (Result < Value && Tries != 0)
            {
                Tries -= 1;
                Index = Units[Random(Units.Count)];
                if (Index != -1 && Types.Contains(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitType) && Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).GetObtainValue() + Result > Value && Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FLevel <= Level)
                {
                    BestIndex = Index;
                    I = 20;
                    Difference = Math.Abs(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).GetObtainValue() - (Value - Result));
                    do
                    {
                        Index = Units[Random(Units.Count)];
                        if (Index != -1 && Types.Contains(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitType))
                        {
                            Delta = Math.Abs(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).GetObtainValue() - (Value - Result));
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

                if (Index != -1 && Types.Contains(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitType) && Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FLevel <= Level)
                {
                    Unit_ = new TUnit(null);
                    Unit_.SetUnitResource(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index));
                    if (Unit_.Place(X, Y, L, Player, TSet<TUnitPlaceModeElement>.Of()) != null)
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

    internal static TAlignment IRtoAlignment(sbyte Relation)
    {
        TAlignment Result;
        if (Relation < 0)
        {
            switch (Relation)
            {
                case -2:
                    Result = TAlignment.alGood;
                    break;
                case -4:
                    Result = TAlignment.alEvil;
                    break;
                case -3:
                    Result = TAlignment.alNeutral;
                    break;
                default:
                    Result = TAlignment.alNeutral;
                    break;
            }
        }
        else
        {
            Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(Relation).GetAlignment();
        }

        return Result;
    }

    internal static TDiplomaticRelation DiplomaticRelation(sbyte Player, sbyte IndependentRelation, sbyte OtherPlayer, sbyte OtherIndependentRelation)
    {
        TDiplomaticRelation Result;
        sbyte Race;
        sbyte OtherRace;
        TAlignment Alignment;
        TAlignment OtherAlignment;
        if (Player == 0 || OtherPlayer == 0)
        {
            if (Player != 0)
            {
                Race = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FPlayerRace;
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
                OtherRace = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(OtherPlayer).FPlayerRace;
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
                    Alignment = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(Race).GetAlignment();
                }
                else
                {
                    Alignment = Aow.Game.Magic.Abilities.AbilityRules.IRtoAlignment(IndependentRelation);
                }

                if (OtherRace != -1)
                {
                    OtherAlignment = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(OtherRace).GetAlignment();
                }
                else
                {
                    OtherAlignment = Aow.Game.Magic.Abilities.AbilityRules.IRtoAlignment(OtherIndependentRelation);
                }

                Result = Aow.Game.Magic.Abilities.AlignmentTables.dAlignmentToDiplomaticRelation(Alignment, OtherAlignment);
            }
            else
            {
                Result = Aow.Game.Magic.Abilities.AlignmentTables.StatusToDiplomaticRelation(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Race).GetRelation(OtherRace));
            }
        }
        else
        {
            if (Player != OtherPlayer)
            {
                Result = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation(Player, OtherPlayer);
            }
            else
            {
                Result = TDiplomaticRelation.drAlliance;
            }
        }

        return Result;
    }

    internal static bool ValidAttackRelation(sbyte Player, sbyte IndependentRelation, sbyte TargetPlayer, sbyte TargetRace)
    {
        bool Result;
        TDiplomaticRelation Relation;
        if (Player == 0)
        {
            if (TargetPlayer != 0)
            {
                Relation = Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Player, IndependentRelation, TargetPlayer, TargetRace);
                Result = TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drWar, TDiplomaticRelation.drUnknown).Contains(Relation);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).ValidAttackRelation(TargetPlayer, TargetRace);
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

    internal static TSet<TDamageType> InvertDamageTypes(TSet<TDamageType> Types)
    {
        TDamageType DamageType;
        TSet<TDamageType> Result = TSet<TDamageType>.Of();
        for (int damageTypeOrdinal = (int)(TDamageType)0; damageTypeOrdinal <= (int)(TDamageType)9; damageTypeOrdinal++)
        {
            DamageType = (TDamageType)damageTypeOrdinal;
            if (!Types.Contains(DamageType))
            {
                Result = Result + TSet<TDamageType>.Of(DamageType);
            }
        }

        return Result;
    }

    internal static int GetMovepathBattleFieldStrength(sbyte Player, sbyte IndependentRelation, TMovepath Movepath, int TargetRange, int Range, int Step)
    {
        int Result;
        TUnitUserList Units;
        int I;
        int J;
        int Distance;
        THNWalkData Walk = default;
        TBattleFieldInfo Info = default;
        TMapLevel Level;
        TAoWMapField Field;
        TArmyHS Army;
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

            Aow.Game.Combat.TBattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, IndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(), Aow.Game.Magic.Abilities.AbilityRules.dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
            Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
            Info.Requests = Info.Requests + TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents, TBattleFieldInfoRequestsElement.bfrEffectiveStrength);
            if (Step > 0)
            {
                Units = new TUnitUserList(null);
                var item = Movepath[Movepath.Count - 1];
                Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[item.L];
                var movepathItem = Movepath[Movepath.Count - 1];
                var item2 = Movepath[Movepath.Count - 1];
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, item2.X, movepathItem.Y, 0);
                while (Walk.Radius <= Step)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        Field = (TAoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if ((Field.FFieldFlags & 67108864) != 0)
                        {
                            Army = Field.GetArmyHS();
                            if (Army != null && Army.FArmy.FPlayer != Player)
                            {
                                Units.AddUnits(Army.FArmy);
                            }
                        }
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                Info.Units = Units;
            }

            while (I >= 0)
            {
                Info.Strength = 0;
                var item = Movepath[I];
                var movepathItem = Movepath[I];
                var item2 = Movepath[I];
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.RetrieveBattleFieldInfo(item2.X, movepathItem.Y, item.L, Range, ref Info);
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
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.RetrieveBattleFieldInfo(item5.X, item4.Y, item3.L, Range, ref Info);
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

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIExecuter.UseProcessTime(50);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static TEnhancementAbility CreateEnhancementAbility(int ID, string Name, TSet<TAbilitySelectionType> SelectionTypes)
    {
        TEnhancementAbility Ability;
        Ability = new TEnhancementAbility(null);
        Ability.FAbilityID = ID;
        Ability.FAbilityName = Name;
        Ability.FSelectionTypes = SelectionTypes;
        return Ability;
    }

    internal static FixedArray_int_3 CommandAbilityIDs = FixedArray_int_3.From([29, 28, 148]);

    internal static FixedArray_int_13 RaceColors = FixedArray_int_13.From([14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224]);

    internal static TSet<TDamageType> dtAll = TSet<TDamageType>.FromMask(1023UL);

    internal static FixedArray_string_2 ExplosionSFX = FixedArray_string_2.From(["SFX\\ATTACK\\STORM.WAV", "SFX\\ATTACK\\EXPLOD1.WAV"]);

    internal static TSet<TAbilitySelectionType> astAll = TSet<TAbilitySelectionType>.Of().WithRange(TAbilitySelectionType.astUnit, TAbilitySelectionType.astEditor);

    internal static TSet<Aow.Game.Magic.TMagicSphere> msAll = TSet<Aow.Game.Magic.TMagicSphere>.Of().WithRange(Aow.Game.Magic.TMagicSphere.msCosmos, Aow.Game.Magic.TMagicSphere.msNone);

    internal static int AIDID;

    internal static FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12 P2RaceTerrain = FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12.From([new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDesert), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttSnow, TAoWTerrainType.ttIce, TAoWTerrainType.ttUndergroundIce) }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttWater), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDesert, TAoWTerrainType.ttWasteland) }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttSnow, TAoWTerrainType.ttIce, TAoWTerrainType.ttUndergroundIce), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDesert) }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttGrass), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttWasteland) }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttGrass), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttWasteland) }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDirt), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDirt), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttDirt), Disliked = TSet<TAoWTerrainType>.Of() }, new TP2RaceTerrain { Preferred = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttWasteland), Disliked = TSet<TAoWTerrainType>.Of(TAoWTerrainType.ttGrass) }]);

    internal static FixedArray_string_5 P2AbilityTypeNames = FixedArray_string_5.From(["Enhancement", "Handicap", "Movement", "Combat", "Enchantment"]);

    internal static FixedArray_byte_9 ArmySelectionMasks = FixedArray_byte_9.From([0, 1, 3, 7, 15, 31, 63, 127, 255]);

    internal static int TMPDID;

    internal static EClass P4DurationDataClass = EClass.Of<TDurationAbilityData>();

    internal static EClass P4MultiLevelDataClass = EClass.Of<TMultiLevelAbilityData>();

    internal static EClass P4CastSpellActionClass = EClass.Of<TAICastSpellAction>();

    internal static EClass P4ImageAnimationClass = EClass.Of<TImageSequenceAnimation>();

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
        TEndTurnTE Event;
        Event = new TEndTurnTE(null);
        Event.FPlayer = Player;
        Event.FDay = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay;
        Event.FMode = TEndTurnMode.emEndTurn;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.TokenControls[Player].AddEvent(Event);
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

    internal static bool CanConstructRoad(sbyte Player, TAoWMapField Field)
    {
        bool Result = Player == Field.GetPlayer() || Field.GetPlayer() == -1;
        Result = Result && TSet<byte>.WithBounds(0, 255).WithRange(1, 5).Contains((byte)Field.TerrainType);
        Result = Result && !TSet<byte>.WithBounds(0, 255).With(0).With(2).With(4).With(5).Contains((byte)Field.OverlayType);
        // Original shared helper
        return Result && Field.FindNoneTransparentHS(typeof(TStructure)) == null;
    }

    public static void RetrieveStormDamageInfo(sbyte player, TStormType storm, sbyte x, sbyte y,
                sbyte level, sbyte innerRadius, sbyte outerRadius, ref TStormDamageInfo info)
    {
        var query = new StormDamageQuery
        {
            Info = new()
            {
                Player = player,
                FriendlyRelations = TSet<byte>.WithBounds(0, 7).With(0).With(2),
                EnemyRelations = TSet<byte>.WithBounds(0, 7).With(1).With(3)
            }
        };
        var mapLevel = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(level);
        var message = new TMapFieldMsg { MsgID = 0x1151 };
        message.Param[0] = (int)storm;
        message.Param[2] = query;
        THNWalkData walk = default;
        Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref walk, x, y, innerRadius);
        try
        {
            while (walk.Radius <= outerRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    message.Param[1] = walk.Radius;
                    mapLevel.GetField(walk.X, walk.Y).SendMapFieldMsg(ref message);
                }
                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }

    internal static TSet<TMoveType> IntToMoveTypes(int value) => TSet<TMoveType>.FromMask((byte)value);

    internal static TSet<TDamageType> AllDamageTypes = TSet<TDamageType>.FromMask(1023UL);

    internal static void TriggerStormDamage(TStormType Storm, sbyte X, sbyte Y, sbyte L, sbyte InnerRadius, sbyte OuterRadius, sbyte Player)
    {
        TMapFieldMsg Msg = default;
        THNWalkData Walk = default;
        TMapLevel Level;
        Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[L];
        Msg.MsgID = 4432;
        Msg.Param[0] = (int)Storm;
        Msg.Param[2] = Player;
        Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, InnerRadius);
        while (Walk.Radius <= OuterRadius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Msg.Param[1] = Walk.Radius;
                Level.Field[Walk.X, Walk.Y].SendMapFieldMsg(ref Msg);
            }

            Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
        }
    }

}
