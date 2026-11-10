// Translated from SmokeCloud.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells
{

    // FCloud
    public partial class SmokeCloudEnchantment : Magic.GlobalEnchantment
    {
        // SmokeCloud.pas:99
        public new const int ClassID = 135748;

        public SmokeCloud FCloud;

        // SmokeCloud.pas:91
        public SmokeCloudEnchantment(ECustomNode owner) : base(owner)
        {
            FEnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask((Set<CloudEnchantmentOptionsElement>.FromMask(FEnchantmentMode.Mask) - Set<CloudEnchantmentOptionsElement>.Of(CloudEnchantmentOptionsElement.Bit0, CloudEnchantmentOptionsElement.Bit1)).Mask);
            FDispelMana = 25;
        }

        // SmokeCloud.pas:102
        public override string GetName() => AoWHSSet.Current.FSpellControl.GetSpell(74).FSpellName;

        // SmokeCloud.pas:156
        public override bool VisibleForPlayer(sbyte Player)
        {
            HNWalkData Walk = default;
            bool Result = FPlayerIndex == Player;
            ref var walk = ref Walk;
            var yhx = FCloud.Yhx;
            var xhx = FCloud.Xhx;
            HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (!Result && walk.Radius <= 3)
            {
                Result = AoWHSMap.Current.VisibleForPlayer(walk.X, walk.Y, FCloud.Lhx, Player);
                HNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // The unused fourth byte in the original register result has no defined value.
        public override XYL GetLocation() => new() { X = FCloud.GetXhx(), Y = FCloud.GetYhx(), L = FCloud.GetLhx() };

        public override bool Deactivate(byte player)
        {
            if ((FEnchantmentStatus.Image & 1) == 0) return false;
            bool result = base.Deactivate(player);
            if ((FCloud.FStatus.Image & 0x20) != 0) FCloud?.Dispose();
            return result;
        }

        public enum CloudEnchantmentOptionsElement : byte
        {
            Bit0 = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
        }
    }
}

namespace Aow.Game.World
{

    // FPlayer, FTurnsRemaining, FDrifting, FDirection, FUpdated, FVisibleForSeatedPlayer,
    // FEnchantment
    public partial class SmokeCloud : MultiHexMO
    {
        public SmokeCloud(ECustomNode owner) : base(owner)
        {
            FEnchantment = new SmokeCloudEnchantment(null);
            FEnchantment.FCloud = this;
            FVisibleForSeatedPlayer = false;
            FDrifting = false;
        }

        // SmokeCloud.pas:187
        public new const int ClassID = 135746;

        public sbyte FPlayer;
        public int FTurnsRemaining;
        public bool FDrifting;
        public byte FDirection;
        public bool FUpdated;
        public bool FVisibleForSeatedPlayer;
        public SmokeCloudEnchantment FEnchantment;

        // SmokeCloud.pas:181
        public override void Dispose()
        {
            base.Dispose();
            FEnchantment.Release();
        }

        // SmokeCloud.pas:206
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
            Stream.rwInteger(ref FTurnsRemaining, 16);
            Stream.rwBool(ref FDrifting, 17);
            Stream.rwByte(ref FDirection, 18);
            Stream.rwEObject(FEnchantment, 19);
        }

        // SmokeCloud.pas:172

        // SmokeCloud.pas:373
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayer && !FUpdated)
            {
                UpdateCloud();
                return;
            }

            if (Msg.MsgID == 537001987 && Msg.Payload.Int == FPlayer)
            {
                FUpdated = false;
            }
            else
            {
                if (Msg.MsgID == 537001993)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }

        // SmokeCloud.pas:190
        public override int GetShowPriority() => base.GetShowPriority() + 201326592;

        // SmokeCloud.pas:300
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            if (FVisibleForSeatedPlayer || AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // SmokeCloud.pas:266
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((MapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // SmokeCloud.pas:272
        public override void RemoveFromMap()
        {
            MapLevel Level;
            Level = (MapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // SmokeCloud.pas:245
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            if (Msg.MsgID != 16 || AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (FResource.TerrainList.Terrain[Msg.HN] >= -1)
            {
                if (Msg.MsgID == 4392 && FStatus.Contains(MapObjectStatusElement.Activated))
                {
                    Msg.Param[0] = 1;
                }
                else
                {
                    if (Msg.MsgID == 4365)
                    {
                        UpdateVisibleForSeatedPlayer();
                    }
                }
            }
        }

        // SmokeCloud.pas:281
        public override void Activate()
        {
            base.Activate();
            if (FDirection == 0)
            {
                FDirection = (byte)(AoWHSMap.Current.Random(6) + 1);
            }

            var smokeCloudEnchantment = FEnchantment;
            if (smokeCloudEnchantment.FPlayerIndex > 0 && !smokeCloudEnchantment.FEnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
            {
                smokeCloudEnchantment.Activate(smokeCloudEnchantment.FPlayerIndex);
            }

            var yhx = Yhx;
            UpdateConcealment(Xhx, yhx, Lhx);
            UpdateVisibleForSeatedPlayer();
        }

        // SmokeCloud.pas:292
        public override void Deactivate()
        {
            base.Deactivate();
            if (FEnchantment.FPlayerIndex > 0)
            {
                FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
            }

            var lhx = GetLhx();
            var yhx = GetYhx();
            UpdateConcealment(GetXhx(), yhx, lhx);
        }

        // SmokeCloud.pas:238
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (Info.HN > 18 && Info.HN < 37)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16777216;
            }
        }

        // SmokeCloud.pas:193
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result;
            int Xhx = default;
            int Yhx = default;
            int HN;
            if (FResource.Engine.HSMap.Status.Contains(HSMapStatusElement.Editting))
            {
                HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                HN = HexGeometry.dHXtoHN(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx);
                Result = HN < FResource.TerrainList.Count && FResource.TerrainList.Terrain[HN] >= -1;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // SmokeCloud.pas:216
        public void UpdateConcealment(sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            HNWalkData Walk = default;
            MapLevel Level;
            AoWMapField MapField;
            ArmyHS ArmyHS;
            Level = AoWHSMap.Current.MapContainer.MapLevels[Lhx];
            HNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (FResource.TerrainList.Count > Walk.HN)
            {
                if (FResource.TerrainList.Terrain[Walk.HN] >= -1 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                {
                    MapField = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                    ArmyHS = MapField.GetArmyHS();
                    if (ArmyHS != null)
                    {
                        ArmyHS.FArmy.Update();
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // SmokeCloud.pas:253
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible;
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            Visible = AoWHSMap.Current.Visible(xhx, yhx, lhx, 3) || FEnchantment.FPlayerIndex == AoWHSMap.Current.FSeatedPlayer;
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                AoWHSMap.Current.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // SmokeCloud.pas:306
        public bool ValidMoveDir(int Direction)
        {
            int Xhx = default;
            int Yhx = default;
            MapLevel Level;
            Level = (MapLevel)Owner.Owner;
            var yhx = GetYhx();
            HexGeometry.CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
            HexGeometry.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
            return Xhx >= 10 && Yhx >= 10 && Level.Width >= Xhx + 10 && Level.Height >= Yhx + 10;
        }

        // SmokeCloud.pas:317
        public void UpdateCloud()
        {
            int Xhx = default;
            int Yhx = default;
            ByteList Directions;
            bool Valid;
            int Direction;
            int Tries;
            int Lhx;
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
                    Directions = new ByteList();
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
                        Direction = Directions[AoWHSMap.Current.Random(Directions.Count)];
                        Valid = ValidMoveDir(Direction);
                        if (!Valid)
                        {
                            Directions.Remove((byte)Direction);
                        }
                    }

                    Directions?.Dispose();
                    if (!Valid)
                    {
                        Tries = 20;
                        while (!Valid && Tries != 0)
                        {
                            Direction = AoWHSMap.Current.Random(6) + 1;
                            Valid = ValidMoveDir(Direction);
                            Tries -= 1;
                        }
                    }

                    if (Valid)
                    {
                        FDirection = (byte)Direction;
                        Lhx = GetLhx();
                        var yhx = GetYhx();
                        HexGeometry.CenterHNtoHX(GetXhx(), yhx, Direction, ref Xhx, ref Yhx);
                        HexGeometry.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                        Disconnect();
                        UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                        FXhx = (sbyte)Xhx;
                        FYhx = (sbyte)Yhx;
                        Owner = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx);
                        Connect();
                        UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                    }
                }
            }
            else
            {
                Dispose();
            }
        }

        public new partial class Resource : MultiHexMOR
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // SmokeCloud.pas:385
            public new const int ClassID = 135747;

            // SmokeCloud.pas:391
            public override int ResourceUserClassID() => 135746;

            // SmokeCloud.pas:388
            public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.SmokeCloudRStr);

        }

        // SmokeCloud.pas:43

    }
}

namespace Aow.Game.Magic.Spells
{

    public struct CloudSpellFields
    {
        public FixedArray_byte_8 Reserved;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct CloudMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public byte SeatedPlayer;
    }
}
