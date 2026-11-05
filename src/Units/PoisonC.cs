// Translated from PoisonC.pas.
using Aow.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PoisonC
{
    // FPlayer, FTurnsRemaining, FDrifting, FDirection, FUpdated
    public partial class TPoisonCloud : TMultiHexMO
    {
        public TPoisonCloud(TECustomNode owner) : base(owner)
        {
            FDrifting = false;
        }

        // PoisonC.pas:64
        public new const int ClassID = 135743;

        public sbyte FPlayer;
        public int FTurnsRemaining;
        public bool FDrifting;
        public byte FDirection;
        public bool FUpdated;
        // PoisonC.pas:149
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
            Stream.rwInteger(ref FTurnsRemaining, 16);
            Stream.rwBool(ref FDrifting, 17);
            Stream.rwByte(ref FDirection, 18);
        }

        // PoisonC.pas:58


        // PoisonC.pas:275
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Value == FPlayer && !FUpdated)
            {
                UpdateCloud();
                return;
            }

            if (Msg.MsgID == 537001987 && Msg.Value == FPlayer)
            {
                FUpdated = false;
            }
        }

        // PoisonC.pas:67
        public override int GetShowPriority() => base.GetShowPriority() + 201326592;

        // PoisonC.pas:89
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                UpdateMapFields((TMapLevel)Owner.Owner, xhx, yhx);
            }

            return Result;
        }

        // PoisonC.pas:111
        public override void RemoveFromMap()
        {
            TMapLevel Level;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // PoisonC.pas:182
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            if (Msg.MsgID != 16 || AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (FResource.TerrainList.Terrain[Msg.HN] >= -1)
            {
                if (Msg.MsgID == 4418)
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                }
                else
                {
                    if (Msg.MsgID == 4384)
                    {
                        if (!Msg.Param[0].As<TArmyHS>().FArmy.ImmunityTypes(255).Contains(TDamageType.dtPoison))
                        {
                            Msg.Result = 1;
                        }
                    }
                    else
                    {
                        if (Msg.MsgID == 4432)
                        {
                            if ((byte)Msg.Param[0].Value == 1 && Msg.Param[1].Value == 0 && Msg.HN == 0)
                            {
                                Destroy();
                            }
                        }
                    }
                }
            }
        }

        // PoisonC.pas:119
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result;
            if (FResource.TerrainList.Terrain[Info.HN] >= -1)
            {
                if (!CanDoDamage(((TMoveControl)Info.Sender).FSettings.FUnits))
                {
                    Result = 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // PoisonC.pas:198
        public override void Activate()
        {
            base.Activate();
            if (FDirection == 0)
            {
                FDirection = (byte)(AoWHSMap.Random(6) + 1);
            }
        }

        // PoisonC.pas:83
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (FResource.TerrainList.Terrain[Info.HN] >= -1)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // PoisonC.pas:70
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result;
            int Xhx = default;
            int Yhx = default;
            int HN;
            if (FResource.Engine.HSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                HN = dHXtoHN(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx);
                Result = HN < FResource.TerrainList.Count && FResource.TerrainList.Terrain[HN] >= -1;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // PoisonC.pas:95
        public new void UpdateMapFields(TMapLevel Level, int Xhx, int Yhx)
        {
            THNWalkData Walk = default;
            int Count;
            Count = FResource.TerrainList.Count;
            InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                if (GetTerrain(Walk.HN) >= -1 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Level.GetField(Walk.X, Walk.Y).UpdateMapField();
                }

                WalkToNextHN(ref Walk);
            }
        }

        // PoisonC.pas:129
        public bool CanDoDamage(TUnitList Units)
        {
            int I;
            TAbstractUnit AUnit;
            bool Result = false;
            I = Units.GetCount();
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                Result = !AUnit.GetImmunityTypes().Contains(TDamageType.dtPoison) && !AUnit.GetAbilityEnabled(1);
            }

            return Result;
        }

        // PoisonC.pas:142
        public void ArmyPlaced(TArmyHS ArmyHS)
        {
            if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) && !((TAoWMapField)ArmyHS.Owner).GetMagicShieldActive())
            {
                ArmyHS.FArmy.IncommingStorm(TStormType.stPestilence, 0, ArmyHS.FArmy.FPlayer);
            }
        }

        // PoisonC.pas:158
        public void TriggerDamage()
        {
            TMapFieldMsg Msg = default;
            THNWalkData Walk = default;
            TMapLevel Level;
            Level = (TMapLevel)Owner.Owner;
            var yhx = GetYhx();
            var xhx = GetXhx();
            InitHNWalk(ref Walk, xhx, yhx, 0);
            Msg.MsgID = 4432;
            Msg.Param[0] = (int)TStormType.stPestilence;
            Msg.Param[2] = FPlayer;
            ref var walk = ref Walk;
            while (FResource.TerrainList.Count > walk.HN)
            {
                if (FResource.TerrainList.GetTerrain(walk.HN) >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && Level.Height > walk.Y)
                {
                    Msg.Param[1] = walk.Radius;
                    Level.GetField(walk.X, walk.Y).SendMapFieldMsg(ref Msg);
                }

                WalkToNextHN(ref Walk);
            }
        }

        // PoisonC.pas:204
        public bool ValidMoveDir(int Direction)
        {
            int Xhx = default;
            int Yhx = default;
            TMapLevel Level;
            Level = (TMapLevel)Owner.Owner;
            var yhx = GetYhx();
            CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
            CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
            return Xhx >= 10 && Yhx >= 10 && Xhx + 10 <= Level.Width && Yhx + 10 <= Level.Height;
        }

        // PoisonC.pas:216
        public void UpdateCloud()
        {
            int Xhx = default;
            int Yhx = default;
            TByteList Directions;
            bool Valid;
            int Direction;
            int Tries;
            int Lhx;
            int OldXhx;
            int OldYhx;
            if (FTurnsRemaining != 1)
            {
                FUpdated = true;
                if (FTurnsRemaining != 0)
                {
                    FTurnsRemaining -= 1;
                }

                if (FDrifting)
                {
                    Direction = 0;
                    Directions = new TByteList();
                    Valid = false;
                    Directions.Add(FDirection);
                    if (FDirection != 1)
                    {
                        Directions.Add((byte)(FDirection - 1));
                    }
                    else
                    {
                        Directions.Add(6);
                    }

                    if (FDirection != 6)
                    {
                        Directions.Add((byte)(FDirection + 1));
                    }
                    else
                    {
                        Directions.Add(1);
                    }

                    while (Directions.Count != 0 && !Valid)
                    {
                        Direction = Directions[AoWHSMap.Random(Directions.Count)];
                        Valid = ValidMoveDir(Direction);
                        if (!Valid)
                        {
                            Directions.Remove((byte)Direction);
                        }
                    }

                    Directions.Free();
                    if (!Valid)
                    {
                        Tries = 20;
                        while (!Valid && Tries != 0)
                        {
                            Direction = AoWHSMap.Random(6) + 1;
                            Valid = ValidMoveDir(Direction);
                            Tries -= 1;
                        }
                    }

                    if (Valid)
                    {
                        OldXhx = GetXhx();
                        OldYhx = GetYhx();
                        FDirection = (byte)Direction;
                        Lhx = GetLhx();
                        var yhx = GetYhx();
                        CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
                        CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                        Disconnect();
                        UpdateMapFields(AoWHSMap.MapContainer.GetMapLevel(Lhx), OldXhx, OldYhx);
                        FXhx = (sbyte)Xhx;
                        FYhx = (sbyte)Yhx;
                        Owner = AoWHSMap.MapContainer.GetField(Xhx, Yhx, Lhx);
                        Connect();
                        UpdateMapFields(AoWHSMap.MapContainer.GetMapLevel(Lhx), Xhx, Yhx);
                    }
                }

                TriggerDamage();
            }
            else
            {
                Destroy();
            }
        }

        // PoisonC.pas:27


    }

    public partial class TPoisonCloudResource : TMultiHexMOR
    {
        public TPoisonCloudResource(TECustomNode owner) : base(owner)
        {}

        // PoisonC.pas:286
        public new const int ClassID = 135744;

        // PoisonC.pas:292
        public override int ResourceUserClassID() => 135743;

        // PoisonC.pas:289
        public override string ResourceEditName() => TranslateRStr(PoisonCloudRStr);

    }

    public static partial class PoisonC
    {
        // PoisonC.pas:295
        public static void RegisterPoisonCloud(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TPoisonCloud>(), EClass.Of<TPoisonCloudResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
