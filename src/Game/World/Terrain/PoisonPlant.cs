// Translated from PoisonP.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// summoning status. caster player. remaining turns. original overlay
public partial class PoisonPlant : ImageLibraryTerrainMapObject
{
    public PoisonPlant(ECustomNode owner) : base(owner)
    {
        FCasterPlayer = -1;
        FOriginalOverlay = -1;
        FDaysRemaining = -1;
        FPlantStatus = Set<PoisonPlantStatusElement>.Of();
    }

    // PoisonP.pas:87
    public new const int ClassID = 131998;

    public Set<PoisonPlantStatusElement> FPlantStatus;
    public sbyte FCasterPlayer;
    public sbyte FDaysRemaining;
    public sbyte FOriginalOverlay;

    // PoisonP.pas:97
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwShortintEx(ref FCasterPlayer, -1, 40);
        Stream.rwShortintEx(ref FDaysRemaining, -1, 41);
        Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
    }

    // PoisonP.pas:78

    // PoisonP.pas:255
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
        }
    }

    // PoisonP.pas:261
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        if (!FPlantStatus.Contains(PoisonPlantStatusElement.Summoning))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // PoisonP.pas:240
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            UpdateMapFields((MapLevel)Owner.Owner, xhx, yhx);
        }

        return Result;
    }

    // PoisonP.pas:246
    public override void RemoveFromMap()
    {
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(Level, FXhx, FYhx);
    }

    // PoisonP.pas:218
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4418)
        {
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                ArmyPlaced(Msg.Param[0].As<ArmyHexSprite>());
            }
        }
        else
        {
            if (Msg.MsgID == 4384)
            {
                if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0 && !Msg.Param[0].As<ArmyHexSprite>().FArmy.ImmunityTypes(255).Contains(DamageType.Poison) && PoisonPlantActive())
                {
                    Msg.Result = 1;
                }
            }
            else
            {
                if (Msg.MsgID == 4402)
                {
                    // { Original excludes player zero from early restoration. }
                    if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0 && FCasterPlayer > 0 && FDaysRemaining > 0)
                    {
                        RestoreTerrain();
                    }
                }
            }
        }
    }

    // PoisonP.pas:209
    public override int CanMoveOver(in HexSpriteSenderInfo Info)
    {
        int Result;
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0 && PoisonPlantActive())
        {
            if (!CanDoDamage(((MoveControl)Info.Sender).FSettings.FUnits))
            {
                Result = 1;
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // PoisonP.pas:203
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
        }
    }

    // PoisonP.pas:92
    public bool PoisonPlantActive() => !Set<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)FTerrainType);

    // PoisonP.pas:105
    public bool CanDoDamage(UnitList Units)
    {
        int I;
        Troop U;
        bool Result = false;
        I = Units.Count;
        while (!Result && I != 0)
        {
            I -= 1;
            U = Units.GetUnit(I);
            Result = !U.GetImmunityTypes().Contains(DamageType.Poison) && !U.GetAbilityEnabled(1);
        }

        return Result;
    }

    // PoisonP.pas:120
    public void TriggerArmyDamage(ArmyHexSprite ArmyHS)
    {
        int Count;
        UnitList Units;
        ArmyEventLog Log;
        Troop U;
        int Damage;
        Count = 0;
        RandSeed = AoWHSMap.Current.Random(16777215);
        Log = new ArmyEventLog(null);
        Log.FArmyText = AbilityRules.TranslateRStr(PoisonPlantDamageRStr);
        Log.FPlayer = ArmyHS.FArmy.FPlayer;
        var poisonPlantDamage = AbilityRules.TranslateRStr(PoisonPlantDamageRStr);
        Log.FArmyMessages.Add(poisonPlantDamage);
        Log.FArmyIcon = 1;
        Log.FArmyView.Setup(ArmyHS.FArmy, 255);
        Units = ArmyHS.FArmy;
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            U = Units.GetUnit(I);
            U.AddRef();
            if (!U.GetImmunityTypes().Contains(DamageType.Poison) && !U.GetAbilityEnabled(1))
            {
                Damage = U.ExecuteDamageRole(4, 3, Set<DamageType>.Of(DamageType.Poison));
                if (Damage > U.GetHitPoints())
                {
                    Damage = U.GetHitPoints();
                }

                if (Damage != 0)
                {
                    U.SetHitPoints((sbyte)(U.GetHitPoints() - Damage));
                    if (U.GetHitPoints() != 0)
                    {
                        U.ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    }

                    Count += 1;
                }

                Log.FDamageValues.Add((byte)Damage);
                Log.FDamageTypes.Add((int)DamageType.Poison);
            }
            else
            {
                Log.FDamageValues.Add(0);
                Log.FDamageTypes.Add((int)DamageType.None);
            }

            U.Release();
        }

        Log.ValidateGameOver();
        if (Count != 0)
        {
            var yhx = GetYhx();
            var lhx = GetLhx();
            var xhx = GetXhx();
            EventLog.DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
        }

        ArmyHS.FArmy.Validate();
        Log.Release();
    }

    // PoisonP.pas:168
    public void ArmyPlaced(ArmyHexSprite ArmyHS)
    {
        if (PoisonPlantActive() && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            TriggerArmyDamage(ArmyHS);
        }
    }

    // PoisonP.pas:174
    public void RestoreTerrain()
    {
        MapField MapF;
        sbyte OriginalOverlay;
        MapF = (MapField)Owner;
        OriginalOverlay = FOriginalOverlay;
        Dispose();
        if (OriginalOverlay > 0)
        {
            AoWHSMap.Current.PlaceTerrain(MapF.Xhx, MapF.Yhx, MapF.Lhx, MapF.TerrainType, OriginalOverlay);
        }
    }

    // PoisonP.pas:186
    public void NewTurn(sbyte Player)
    {
        ArmyHexSprite ArmyHS;
        if (PoisonPlantActive())
        {
            ArmyHS = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (ArmyHS != null && Player == ArmyHS.FArmy.FPlayer)
            {
                TriggerArmyDamage(ArmyHS);
            }
        }

        if (Player == FCasterPlayer && FDaysRemaining > 0)
        {
            FDaysRemaining = (sbyte)(FDaysRemaining - 1);
            if (FDaysRemaining == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // PoisonP.pas:267
    public void ShowSummonAnimation(Animation Animation, int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        MapField MapF;
        var lhx = Animation.GetLhx();
        var yhx = Animation.GetYhx();
        var xhx = Animation.GetXhx();
        MapF = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
        if (this == MapF.FindOwnedChild(131998) && GameTables.AoWEngine.FAnimationDetail != 0 && Animation.FFrameCounter == 0)
        {
            bool condition = AoWHSMap.Current.FSeatedPlayer == Animation.FTag;
            if (!condition)
            {
                var lhx2 = GetLhx();
                var yhx2 = GetYhx();
                var xhx2 = GetXhx();
                condition = AoWHSMap.Current.WatchingTerrain(xhx2, yhx2, lhx2, 0);
            }

            if (condition)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(12).FCastSounds.Play(0);
            }
        }
    }

    // PoisonP.pas:279
    public void NewFrameSummonAnimation(Animation Animation, int FrameCounter)
    {
        var lhx = Animation.GetLhx();
        var yhx = Animation.GetYhx();
        var xhx = Animation.GetXhx();
        var field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
        if (this == field.FindOwnedChild(131998) && FrameCounter == 9)
        {
            FPlantStatus = FPlantStatus - Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        }
    }

    private static readonly string PoisonPlantDamageRStr = "Poison Plant Damage";

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // PoisonP.pas:293
        public new const int ClassID = 131999;

        // PoisonP.pas:298
        public override int ResourceUserClassID() => 131998;

        // PoisonP.pas:288
        public override string ResourceEditName() => AbilityRules.TranslateRStr(PoisonPlantRStr);

        private static readonly string PoisonPlantRStr = "Poison Plant";

    }

    // PoisonP.pas:28

    // PoisonP.pas:28

}
