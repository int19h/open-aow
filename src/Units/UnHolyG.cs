// Translated from UnHolyG.pas.
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.Magic.Spells.UnHolyG;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells.UnHolyG
{

    public enum AnonymousEnum_hgSummoning : byte
    {
        hgSummoning = 0
    }
}

namespace Aow.Game.World.Terrain
{

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class TUnHolyGround : TILTerrainMO
    {
        public TUnHolyGround(TECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:93
        public new const int ClassID = 131985;

        public TSet<AnonymousEnum_hgSummoning> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;

        // UnHolyG.pas:204
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // UnHolyG.pas:243
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
        }

        // UnHolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(AnonymousEnum_hgSummoning.hgSummoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // UnHolyG.pas:190
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

        // UnHolyG.pas:196
        public override void RemoveFromMap()
        {
            TMapLevel Level;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // UnHolyG.pas:222
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                    return;
                }
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                    {
                        if (CanDoDamage(Msg.Param[0].As<TArmyHS>().FArmy))
                        {
                            Msg.Result = 1;
                        }
                    }
                }
                else
                {
                    if (Msg.MsgID == 4402)
                    {
                        if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && FPlayer > 0 && FDuration > 0)
                        {
                            RestoreTerrain();
                        }
                    }
                }
            }
        }

        // UnHolyG.pas:212
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result;
            if (FResource.TerrainList.Terrain[Info.HN] >= 0)
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

        // UnHolyG.pas:184
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (FResource.TerrainList.Terrain[Info.HN] >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // UnHolyG.pas:96
        public void TriggerArmyDamage(TArmyHS Army)
        {
            TArmyEventLog Log;
            int Damage;
            int Damaged;
            TAbstractUnit AUnit;
            Damaged = 0;
            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
            Log = new TArmyEventLog(null);
            Log.FArmyText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TUnHolyGround.UnholyGroundDamageRStr);
            var unholyGroundDamage = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TUnHolyGround.UnholyGroundDamageRStr);
            Log.FArmyMessages.Add(unholyGroundDamage);
            Log.FArmyIcon = 2;
            var army = Army.FArmy;
            for (int I = 0, last = army.Count - 1; I <= last; I++)
            {
                AUnit = army.GetUnit(I);
                bool condition = AUnit.GetAlignment() != TAlignment.alNone;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(alignment);
                }

                if (condition && !AUnit.GetAbilityEnabled(1))
                {
                    Damage = AUnit.ExecuteDamageRole(4, 3, TSet<TDamageType>.Of(TDamageType.dtDeath));
                    if (Damage > AUnit.GetHitPoints())
                    {
                        Damage = AUnit.GetHitPoints();
                    }

                    if (Damage != 0)
                    {
                        AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - Damage));
                        Damaged += 1;
                    }

                    Log.FDamageValues.Add((byte)Damage);
                    Log.FDamageTypes.Add((int)TDamageType.dtDeath);
                }
                else
                {
                    Log.FDamageValues.Add(0);
                    Log.FDamageTypes.Add((int)TDamageType.dtDeath);
                }
            }

            Log.FPlayer = army.FPlayer;
            var armyHS = army.GetArmyHS();
            Log.FArmyView.Setup(armyHS.FArmy, 255);

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // UnHolyG.pas:142
        public void ArmyPlaced(TArmyHS Army)
        {
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // UnHolyG.pas:147
        public void RestoreTerrain()
        {
            TMapField Field;
            sbyte Overlay;
            Field = (TMapField)Owner;
            Overlay = FOriginalOverlay;
            Dispose();
            if (Overlay > 0)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
            }
        }

        // UnHolyG.pas:157
        public void NewTurn(sbyte Player)
        {
            TArmyHS Army;
            Army = (TArmyHS)Owner.FindOwnedChild(131607);
            if (Army != null && Player == Army.FArmy.FPlayer)
            {
                TriggerArmyDamage(Army);
            }

            if (Player == FPlayer && FDuration > 0)
            {
                FDuration = (sbyte)(FDuration - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // UnHolyG.pas:169
        public bool CanDoDamage(TUnitList Units)
        {
            int I;
            TAbstractUnit AUnit;
            bool Result = false;
            I = Units.Count;
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                bool condition = !AUnit.GetImmunityTypes().Contains(TDamageType.dtDeath) && AUnit.GetAlignment() != TAlignment.alNone;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(alignment);
                }

                Result = condition && !AUnit.GetAbilityEnabled(1);
            }

            return Result;
        }

        // UnHolyG.pas:254
        public void ShowSummonAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TMapField Field;
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            Field = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == Field.FindOwnedChild(131985) && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
            {
                bool condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Anim.FTag;
                if (!condition)
                {
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.WatchingTerrain(xhx2, yhx2, lhx2, 0);
                }

                if (condition)
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(14).FCastSounds.Play(0);
                }
            }
        }

        // UnHolyG.pas:265
        public void NewFrameSummonAnimation(TAnimation Anim, int FrameCounter)
        {
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            var field = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == field.FindOwnedChild(131985) && FrameCounter == 9)
            {
                FMode = FMode - TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
            }
        }

        private static readonly string UnholyGroundDamageRStr = "Unholy Ground Damage";

    }

    public partial class TUnHolyGroundResource : TILTerrainMOR
    {
        public TUnHolyGroundResource(TECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:278
        public new const int ClassID = 131986;

        // UnHolyG.pas:283
        public override int ResourceUserClassID() => 131985;

        // UnHolyG.pas:273
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TUnHolyGroundResource.UnholyGroundRStr);

        private static readonly string UnholyGroundRStr = "Unholy Ground";

    }
}

namespace Aow.Game.Commands
{

    public partial class TUnholyGroundSpellTE : TGlobalTargetSpellTE
    {
        public TUnholyGroundSpellTE(TECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:288
        public new const int ClassID = 135943;

        // UnHolyG.pas:340
        public override bool Process(TAbstractUnit source, int value)
        {
            base.Process(source, value);
            CreateUnholyGround(source);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        // UnHolyG.pas:293
        public void CreateUnholyGround(TAbstractUnit Source)
        {
            TMapLevel Level = default;
            THNWalkData[] Walk__cell = new THNWalkData[1];
            ref THNWalkData Walk = ref Walk__cell[0];
            TUnHolyGroundResource Resource = default;
            TAoWMapField MapF = default;
            TUnHolyGround Ground = default;
            TImageSequenceAnimation Anim = default;
            var walk = Walk;
            Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[FLocation.L];
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            Resource = (TUnHolyGroundResource)Aow.Game.Engine.TAoWHSSet.AoWHSSet.MapObjectResourceList.FindChild(131986);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    MapF = (TAoWMapField)Level.GetField(walk.X, walk.Y);
                    if (((TGlobalTargetSpell)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(14)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                    {
                        MapF.RestoreTerrain();
                        Ground = (TUnHolyGround)Resource.CreateResourceUser();
                        Ground.FDuration = (sbyte)(3 + (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(11) - 5) / 5);
                        Ground.FPlayer = Source.FPlayer;
                        Ground.FOriginalOverlay = MapF.OverlayType;
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Ground, walk.X, walk.Y, FLocation.L))
                        {
                            Ground.FMode = TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
                            Anim = new TImageSequenceAnimation(null);
                            Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(14).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Ground.ShowSummonAnimation;
                            Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                            Anim.FFrameCounter = -Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(15);
                            Anim.FTag = Source.FPlayer;
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Ground?.Dispose();
                        }
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TUnholyGroundSpell : TGlobalTargetSpell
    {
        public TUnholyGroundSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TUnholyGroundSpell.UnholyGroundsRStr);
            FSpellID = 14;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
            FAboveGroundOnly = false;
        }

        // UnHolyG.pas:349

        // UnHolyG.pas:383
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                TargetSelection.FHNs.Add(0);
            }
        }

        // UnHolyG.pas:392
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TUnholyGroundSpellTE Event;
            Event = new TUnholyGroundSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // UnHolyG.pas:368
        public override bool ValidTargetSelectionMapF(TMapField Field)
        {
            TArmyHS Army;
            bool Result = base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && Field.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            Result = Result && Field.FindNoneTransparentHS(typeof(TStructure)) == null;
            if (Result)
            {
                Army = ((TAoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // UnHolyG.pas:360
        public override bool ValidTargetMapF(TMapField Field)
        {
            bool Result = base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && Field.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            return Result && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;
        }

        private static readonly string UnholyGroundsRStr = "Evil Woods";

        // UnHolyG.pas:62

        // UnHolyG.pas:62

    }
}
