// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// { Interface declaration run owned by P1. }
// FMode, FTurnsLeft, FAge, FShowPriority, FPlayer, FExplosionFrame
public partial class Fire : ImageLibraryTerrainMapObject
{

    public new const int ClassID = 132990;

    public Set<ModeElement> FMode;
    public sbyte FTurnsLeft;
    public byte FAge;
    public int FShowPriority;
    public sbyte FPlayer;
    public int FExplosionFrame;

    // Structur.PAS:3402
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTurnsLeft), 5);
        RwSet(ref FMode, 1, 6, Stream.rwData, 0);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 7);
        Stream.rwByte(ref FAge, 8);
    }

    // Structur.PAS:3386
    // mode, lifetime, age, priority, owner player, explosion frame
    public Fire(ECustomNode owner) : base(owner)
    {
        FTurnsLeft = 3;
        FAge = 0;
        FPlayer = 0;
        FShowPriority = 1744830464;
        FMode = Set<ModeElement>.Of(ModeElement.Expandable);
        FExplosionFrame = 0;
    }

    // Structur.PAS:3672
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayer)
        {
            NewTurn();
            return;
        }

        if (Msg.MsgID == 537001987 && Msg.Payload.Int == FPlayer)
        {
            NewTurnDone();
        }
        else
        {
            if (Msg.MsgID == 537002242 && (byte)Msg.Param[1].Int == 6)
            {
                WaterMasteryCast();
            }
        }
    }

    // Structur.PAS:3411
    public override sbyte GetTerrain(int Index)
    {
        sbyte Result = FResource.TerrainList.GetTerrain(Index);
        if (Result >= 0)
        {
            Result = -1;
        }

        return Result;
    }

    // Structur.PAS:3417
    public override int GetShowPriority() => FShowPriority;

    // Structur.PAS:3472
    public override void Connect()
    {
        CalcShowPriority((MapField)Owner);
        base.Connect();
    }

    // Structur.PAS:3517
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Frame;
        int X = default;
        int Y = default;
        if (FExplosionFrame != 0)
        {
            Frame = AoWHSMap.Current.Frame - FExplosionFrame;
            if (Frame >= 0 && Frame < FResource.ImageSequenceList.GetImageSequence(20).GetTotalSequenceFrames())
            {
                HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
                FResource.ImageSequenceList.Show(20, X - XhpSurf, Y - YhpSurf, Frame, SurfaceDesc);
            }
            else
            {
                if (Frame < -1000 || Frame >= 0)
                {
                    FExplosionFrame = 0;
                    base.Show(XhpSurf, YhpSurf, SurfaceDesc);
                }
            }
        }
        else
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // Structur.PAS:3478
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            ((MapField)Owner).UpdateMapField();
        }

        return Result;
    }

    // Structur.PAS:3484
    public override void RemoveFromMap()
    {
        MapField MapField;
        MapField = (MapField)Owner;
        if (MapField != null)
        {
            base.RemoveFromMap();
            MapField.UpdateMapField();
        }
    }

    // Structur.PAS:3589
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        if (Msg.MsgID != 16 || AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (Msg.MsgID == 4418 && FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
        {
            ArmyPlaced(Msg.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (Msg.MsgID == 4384 && FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
        {
            if (!Msg.Param[0].As<ArmyHexSprite>().FArmy.ImmunityTypes(255).Contains(DamageType.Fire))
            {
                Msg.Result = 1;
            }
        }
    }

    // Structur.PAS:3570
    public override int CanMoveOver(in HexSpriteSenderInfo Info)
    {
        int Result;
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (!CanDoDamage(((MoveControl)Info.Sender).FSettings.FUnits))
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

    // Structur.PAS:3564
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
        }
    }

    // Structur.PAS:3509
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FStatus.Contains(MapObjectStatusElement.Visible) && FExplosionFrame != 0 && AoWHSMap.Current.Frame == FExplosionFrame)
        {
            var xhx = GetXhx();
            AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[(xhx + AoWHSMap.Current.Frame) % 2]);
        }

        base.MakeVisible(VisibilityList);
    }

    // Structur.PAS:3497
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
        if (Result)
        {
            var classID = EClass.ClassID;
            Result = MapContainer.GetField(Xhx, Yhx, Lhx).FindOwnedChild(classID) == null;
        }

        return Result;
    }

    // Structur.PAS:3435
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        CalcShowPriority(MapContainer.GetField(Xhx, Yhx, Lhx));
        return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
    }

    // Structur.PAS:3441
    public override sbyte GetMostFrequentMapFieldTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx, sbyte Default)
    {
        TerrainList Terrains;
        ByteArray128 Counts;
        HexNumberWalkData Walk = default;
        sbyte TerrainType;
        sbyte Result = Default;
        Terrains = FResource.TerrainList;
        if (Result < 0)
        {
            Result = 0;
        }

        Counts = default;
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.HN < Terrains.Count)
        {
            if (FResource.TerrainList.GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                TerrainType = MapLevel.GetField(Walk.X, Walk.Y).TerrainType;
                if (TerrainType >= 0)
                {
                    Counts[TerrainType] = (byte)(Counts[TerrainType] + 1);
                    if (Counts[TerrainType] > Counts[Result])
                    {
                        Result = TerrainType;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (Default >= 0)
        {
            if (Counts[Result] == Counts[Default])
            {
                Result = Default;
            }
        }

        return Result;
    }

    // Structur.PAS:3422
    public void CalcShowPriority(MapField MapField)
    {
        int X = default;
        int Y = default;
        HexSprite HS;
        HexGeometry.HXtoHP(MapField.Xhx, MapField.Yhx, ref X, ref Y);
        HS = MapField.GetMaxLevelNoneTransparentHS();
        FShowPriority = 1744830464 + ((Y + 36) << 13) + (X + 16);
        if (HS != this && HS != null)
        {
            if (HS.GetShowPriority() >= FShowPriority)
            {
                FShowPriority = HS.GetShowPriority() + 1;
            }
        }
    }

    // Structur.PAS:3503
    public void SetupExplosion(int Delay)
    {
        if (FResource.ImageSequenceList.GetImageSequence(20) != null)
        {
            FExplosionFrame = Delay + AoWHSMap.Current.Frame;
        }
    }

    // Structur.PAS:3539
    public void TriggerDamage()
    {
        MapFieldMessage Msg = default;
        Msg.MsgID = 4357;
        Msg.Param[0] = this;
        Msg.Result = 0;
        ((MapField)Owner).SendMapFieldMsg(ref Msg);
    }

    // Structur.PAS:3549
    public bool CanDoDamage(UnitList Units)
    {
        int I;
        Troop AUnit;
        bool Result = false;
        I = Units.GetCount();
        while (!Result && I != 0)
        {
            I -= 1;
            AUnit = Units.GetUnit(I);
            Result = !AUnit.GetImmunityTypes().Contains(DamageType.Fire);
        }

        return Result;
    }

    // Structur.PAS:3584
    public void ArmyPlaced(ArmyHexSprite ArmyHS)
    {
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ArmyHS.FArmy.TriggerFireDamage();
        }
    }

    // Structur.PAS:3622
    public void NewTurn()
    {
        if (FAge < 99)
        {
            FAge = (byte)(FAge + 1);
        }
    }

    // Structur.PAS:3627
    public void NewTurnDone()
    {
        HexNumberWalkData Walk = default;
        MapLevel MapLevel;
        int Chance;
        Fire Fire;
        if (FAge > 0)
        {
            MapLevel = (MapLevel)Owner.Owner;
            var yhx = GetYhx();
            var xhx = GetXhx();
            HexNumberWalkData.InitHNWalk(ref Walk, xhx, yhx, 1);
            while (Walk.Radius == 1)
            {
                // { Original excludes the top row when spreading fire. }
                if (Walk.X >= 0 && Walk.X < MapLevel.Width && Walk.Y > 0 && Walk.Y < MapLevel.Height)
                {
                    Chance = AoWMapField.IgnitionChance((AoWMapField)MapLevel.GetField(Walk.X, Walk.Y));
                    if (Chance > AoWHSMap.Current.Random(1000))
                    {
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var lhx = GetLhx();
                        Fire = AoWHSSet.Current.FFireControl.PlaceFire(Walk.X, Walk.Y, lhx, FireType.Large);
                        if (Fire != null)
                        {
                            Fire.FPlayer = FPlayer;
                            Fire.FTurnsLeft = 3;
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }
        }

        if (FTurnsLeft != 0)
        {
            FTurnsLeft = (sbyte)(FTurnsLeft - 1);
            if (FTurnsLeft != 0)
            {
                TriggerDamage();
            }
            else
            {
                Dispose();
            }
        }
        else
        {
            TriggerDamage();
        }
    }

    // Structur.PAS:3667
    public void WaterMasteryCast()
    {
        if (FTurnsLeft != 0)
        {
            Dispose();
        }
    }

    public enum ModeElement : byte
    {
        Expandable = 0
    }

    // FFireType
    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 132991;

        public FireType FFireType;

        // Structur.PAS:3685
        // FFireType
        public override void Dispose()
        {
            if (FEngine != null && !AoWHSSet.Current.Status.Contains(HexSpriteSetStatusElement.Destroying))
            {
                AoWHSSet.Current.FFireControl.UnregisterFireResource(this);
            }

            base.Dispose();
        }

        // Structur.PAS:3707
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FFireType, 1), 1, 20);
        }

        // Structur.PAS:3713
        public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                AoWHSSet.Current.FFireControl.RegisterFireResource(this);
            }
        }

        // Structur.PAS:3702
        public override int ResourceUserClassID() => 132990;

        // Structur.PAS:3692
        public override string ResourceEditName() => AbilityRules.TranslateRStr(GameTables.FireRStr);

    }

    // intf_P1_11.inc:16

}
