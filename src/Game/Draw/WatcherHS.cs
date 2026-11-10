// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// state, unfog radius, enchantment, animation frame, unexplored HNs, visibility flag
public partial class WatcherHS : SingleHS
{
    // GlobalTargetSpells.pas:1220
    public new const int ClassID = 132989;

    public byte FWatcherState;
    public byte FUnfogRadius;
    public WatcherEnchantment FEnchantment;
    public int FAnimationFrame;
    public IntegerList FUnexplored;
    public bool FVisibleForSeatedPlayer;

    // GlobalTargetSpells.pas:1213
    public override void Dispose()
    {
        base.Dispose();
        FUnexplored?.Dispose();
        FEnchantment.Release();
    }

    // GlobalTargetSpells.pas:1344
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FEnchantment, 40);
    }

    // GlobalTargetSpells.pas:1203
    public WatcherHS(ECustomNode owner) : base(owner)
    {
        FEnchantment = new WatcherEnchantment(null);
        FEnchantment.FWatcher = this;
        FUnexplored = new IntegerList();
        FVisibleForSeatedPlayer = false;
        FAnimationFrame = -1;
    }

    // GlobalTargetSpells.pas:1422
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        int ID;
        base.MsgProc(ref Msg, Sender);
        ID = Msg.MsgID;
        if (ID == 537001992)
        {
            UpdateExploration();
            UpdateUnfog();
            return;
        }

        if (ID == 537001990)
        {
            UpdateUnfog();
            UpdateVisibleForSeatedPlayer();
        }
    }

    // GlobalTargetSpells.pas:1223
    public override byte GetLevel() => 27;

    // GlobalTargetSpells.pas:1255
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int DX = default;
        int DY = default;
        int Radius;
        int Angle;
        int PX = default;
        int PY = default;
        int Intensity;
        int Delay;
        bool condition = AoWHSMap.Current.ViewL == Lhx;
        if (condition)
        {
            var yhx = Yhx;
            var xhx = Xhx;
            condition = AoWHSMap.Current.Visible(xhx, yhx, Lhx, 0);
        }

        if (condition)
        {
            var yhx = Yhx;
            HexGeometry.HXtoHP(Xhx, yhx, ref X, ref Y);
            X = X - XhpSurf + 16;
            Y = Y - YhpSurf + 16;
            if (AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages != null)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.ShowLooped(1, X, Y, AoWHSMap.Current.Frame, SurfaceDesc);
            }

            if (FAnimationFrame != -1)
            {
                FAnimationFrame += 1;
                // The original reads the shared AoWEngine animation detail. The map engine is
                // not the global read by this routine.
                if (GameTables.AoWEngine.FAnimationDetail == 2 && AoWHSMap.Current.ViewL == Lhx)
                {
                    Intensity = 100 - FAnimationFrame;
                    var yhx2 = Yhx;
                    HexGeometry.HXtoHP(Xhx, yhx2, ref X, ref Y);
                    X = X - XhpSurf - 8;
                    Y = Y - YhpSurf;
                    for (int I = FUnexplored.Count - 1; I >= 0; I--)
                    {
                        HexGeometry.HNtoHP(FUnexplored[I], ref PX, ref PY);
                        PX += X;
                        PY += Y;
                        AoWHSSet.Current.FGeneralImages[5].ShowBlended(PX, PY, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Intensity]);
                    }
                }

                // { Original resets the shared random generator for deterministic particles. }
                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
                for (int I = 0; I <= 300; I++)
                {
                    Radius = Random(VisibilityRange() << 5);
                    Angle = Random(360);
                    Delay = Random(80);
                    if (FAnimationFrame - Delay >= 0)
                    {
                        AbilityRules.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                        AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, X + DX, Y + DY, FAnimationFrame - Delay, SurfaceDesc);
                    }
                }

                if (FAnimationFrame == 100)
                {
                    StopAnimation();
                }
            }
        }
    }

    // GlobalTargetSpells.pas:1388
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        ((MapField)Owner).UpdateMapField();
        return Result;
    }

    // GlobalTargetSpells.pas:1394
    public override void RemoveFromMap()
    {
        MapField MapF;
        MapF = (MapField)Owner;
        base.RemoveFromMap();
        MapF.UpdateMapField();
    }

    // GlobalTargetSpells.pas:1438
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        int OwnerPlayer;
        ref var mapFieldMsg = ref Msg;
        base.MapFieldMsgProc(ref Msg);
        if (mapFieldMsg.MsgID == 4363)
        {
            if (Activated())
            {
                OwnerPlayer = FEnchantment.FPlayerIndex;
                if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
                {
                    var visibilityRange = VisibilityRange();
                    if (mapFieldMsg.Param[0].Int <= visibilityRange)
                    {
                        mapFieldMsg.Result = 1;
                    }
                }
            }
        }

        if (mapFieldMsg.MsgID == 4362)
        {
            if (Activated())
            {
                OwnerPlayer = FEnchantment.FPlayerIndex;
                if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
                {
                    var visibilityRange = VisibilityRange();
                    if (mapFieldMsg.Param[0].Int <= visibilityRange)
                    {
                        mapFieldMsg.Result = 1;
                    }
                }
            }
        }

        if (mapFieldMsg.MsgID == 4365)
        {
            if (VisibilityRange() >= mapFieldMsg.Param[0].Int)
            {
                // { Param[0] is already available. }
                AoWHSMap.Current.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx, mapFieldMsg.Param[0].Int, mapFieldMsg.Param[0].Int);
            }

            UpdateVisibleForSeatedPlayer();
        }
    }

    // GlobalTargetSpells.pas:1350
    public override void Activate()
    {
        MapField MapF;
        base.Activate();
        FWatcherState = (byte)(Set<WatcherStateElement>.FromMask(FWatcherState) + Set<WatcherStateElement>.Of(WatcherStateElement.Active)).Image;
        FEnchantment.Activate(FEnchantment.FPlayerIndex);
        var mapField = (MapField)Owner;
        AoWHSMap.Current.NotifyPlayerChanged(mapField.Xhx, mapField.Yhx, mapField.Lhx);

        MapF = (MapField)Owner;
        var visibilityRange = VisibilityRange();
        var visibilityRange2 = VisibilityRange();
        AoWHSMap.Current.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, visibilityRange, visibilityRange2);
        UpdateExploration();
        UpdateUnfog();
    }

    // GlobalTargetSpells.pas:1366
    public override void Deactivate()
    {
        base.Deactivate();
        FWatcherState = (byte)(Set<WatcherStateElement>.FromMask(FWatcherState) - Set<WatcherStateElement>.Of(WatcherStateElement.Active)).Image;
        if (FEnchantment.FEnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
        {
            FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
        }

        AoWHSMap.Current.NotifyPlayerChanged(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx);
        UpdateUnfog();
    }

    // GlobalTargetSpells.pas:1403
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags | 4096 | 16777216;
        }
    }

    // GlobalTargetSpells.pas:1226
    public void StartAnimation(sbyte X, sbyte Y, sbyte Layer)
    {
        HNWalkData Walk = default;
        MapLevel Level;
        FAnimationFrame = 0;
        AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
        AoWHSSet.Current.FSpellControl.GetSpell(57).FCastSounds.Play(0);
        FUnexplored.Clear();
        if (AoWHSMap.Current.FExplorationEnabled)
        {
            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
            while (true)
            {
                var visibilityRange = VisibilityRange();
                if (!(Walk.Radius <= visibilityRange))
                    break;
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    if (!((AoWMapField)Level.Field[Walk.X, Walk.Y]).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                    {
                        FUnexplored.Add(Walk.HN);
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // GlobalTargetSpells.pas:1248
    public void StopAnimation()
    {
        AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
        FAnimationFrame = -1;
        FUnexplored.Clear();
    }

    // GlobalTargetSpells.pas:1305
    public void UpdateExploration()
    {
        int OwnerPlayer;
        if (Activated())
        {
            for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
            {
                OwnerPlayer = FEnchantment.FPlayerIndex;
                if (OwnerPlayer == I || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)I) == 2)
                {
                    var visibilityRange = VisibilityRange();
                    var yhx = Yhx;
                    var xhx = Xhx;
                    AoWHSMap.Current.ExploreArea(xhx, yhx, Lhx, (sbyte)I, visibilityRange);
                }
            }
        }
    }

    // GlobalTargetSpells.pas:1321
    public void UpdateUnfog()
    {
        int Radius;
        if ((byte)AoWHSMap.Current.FSeatedPlayer != 255 && (FEnchantment.FPlayerIndex == AoWHSMap.Current.FSeatedPlayer || (byte)AoWHSMap.Current.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)AoWHSMap.Current.FSeatedPlayer) == 2) && Activated())
        {
            Radius = VisibilityRange();
        }
        else
        {
            Radius = 0;
        }

        if (Radius != FUnfogRadius)
        {
            if (FUnfogRadius != 0)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                AoWHSMap.Current.FogArea(xhx, yhx, Lhx, FUnfogRadius);
            }

            FUnfogRadius = (byte)Radius;
            if (FUnfogRadius != 0)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                AoWHSMap.Current.UnFogArea(xhx, yhx, Lhx, FUnfogRadius);
            }
        }
    }

    // GlobalTargetSpells.pas:1376
    public int VisibilityRange()
    {
        int Result = 8;
        if (Lhx != 0 || FEnchantment.FPlayerIndex > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FMagicControl.FEnemyEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
        {
            Result += 1;
            Result = (int)(((uint)Result) >> 1);
        }

        return Result;
    }

    // GlobalTargetSpells.pas:1410
    public void UpdateVisibleForSeatedPlayer()
    {
        bool Visible;
        Visible = FEnchantment.VisibleForPlayer(AoWHSMap.Current.FSeatedPlayer);
        if (FVisibleForSeatedPlayer ^ Visible)
        {
            FVisibleForSeatedPlayer = Visible;
            AoWHSMap.Current.FGlobalMagicControl.TriggerChangedEvent();
        }
    }

    public enum WatcherStateElement : byte
    {
        Active = 0
    }

    // GlobalTargetSpells.pas:172

}
