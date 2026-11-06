// Translated from HolyG.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.ILTer;
using static Aow.Units.EventLog.EventLogUnit;
using Aow.Units.GlobalTargetSpells;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.HolyG;

public enum AnonymousEnum_hgSummoning : byte
{
    hgSummoning = 0
}

// FMode, FPlayer, FDuration, FOriginalOverlay
public partial class THolyGround : TILTerrainMO
{
    public THolyGround(TECustomNode owner) : base(owner)
    {}

    // HolyG.pas:92
    public new const int ClassID = 131987;

    public TSet<AnonymousEnum_hgSummoning> FMode;
    public sbyte FPlayer;
    public sbyte FDuration;
    public sbyte FOriginalOverlay;
    // HolyG.pas:201
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwShortintEx(ref FPlayer, -1, 40);
        Stream.rwShortintEx(ref FDuration, -1, 41);
        Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
    }

    // HolyG.pas:243
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Value);
        }
    }

    // HolyG.pas:249
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        if (!FMode.Contains(AnonymousEnum_hgSummoning.hgSummoning))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // HolyG.pas:187
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

    // HolyG.pas:193
    public override void RemoveFromMap()
    {
        TMapLevel Level;
        Level = (TMapLevel)Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(Level, FXhx, FYhx);
    }

    // HolyG.pas:219
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4418)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                return;
            }

            return;
        }

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

    // HolyG.pas:209
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

    // HolyG.pas:181
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        if (FResource.TerrainList.Terrain[Info.HN] >= 0)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
        }
    }

    // HolyG.pas:95
    public void TriggerArmyDamage(TArmyHS Army)
    {
        TArmyEventLog Log;
        int Damage;
        int Damaged;
        TAbstractUnit AUnit;
        Damaged = 0;
        RandSeed = AoWHSMap.Random(16777215);
        Log = new TArmyEventLog(null);
        Log.FArmyText = TranslateRStr(HolyGroundDamageRStr);
        var holyGroundDamage = TranslateRStr(HolyGroundDamageRStr);
        Log.FArmyMessages.Add(holyGroundDamage);
        Log.FArmyIcon = 2;
        Log.FPlayer = Army.FArmy.FPlayer;
        Log.FArmyView.Setup(Army.FArmy, 255);
        var army = Army.FArmy;
        for (int I = 0, last = army.Count - 1; I <= last; I++)
        {
            AUnit = army.GetUnit(I);
            bool condition = AUnit.GetAlignment() != TAlignment.alNone;
            if (condition)
            {
                var alignment = AUnit.GetAlignment();
                condition = alsPureEvil.Contains(alignment);
            }

            if (condition && !AUnit.GetAbilityEnabled(1))
            {
                Damage = AUnit.ExecuteDamageRole(3, 4, TSet<TDamageType>.Of(TDamageType.dtHoly));
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
                Log.FDamageTypes.Add((int)TDamageType.dtHoly);
            }
            else
            {
                Log.FDamageValues.Add(0);
                Log.FDamageTypes.Add((int)TDamageType.dtNone);
            }
        }

        Log.ValidateGameOver();
        if (Damaged != 0)
        {
            var yhx = GetYhx();
            var lhx = GetLhx();
            var xhx = GetXhx();
            DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
        }

        Log.Release();
        Army.FArmy.Validate();
    }

    // HolyG.pas:139
    public void ArmyPlaced(TArmyHS Army)
    {
        if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            TriggerArmyDamage(Army);
        }
    }

    // HolyG.pas:144
    public void RestoreTerrain()
    {
        TMapField Field;
        sbyte Overlay;
        Field = (TMapField)Owner;
        Overlay = FOriginalOverlay;
        Destroy();
        if (Overlay > 0)
        {
            AoWHSMap.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
        }
    }

    // HolyG.pas:154
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

    // HolyG.pas:166
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
            bool condition = !AUnit.GetImmunityTypes().Contains(TDamageType.dtHoly) && AUnit.GetAlignment() != TAlignment.alNone;
            if (condition)
            {
                var alignment = AUnit.GetAlignment();
                condition = alsPureEvil.Contains(alignment);
            }

            Result = condition && !AUnit.GetAbilityEnabled(1);
        }

        return Result;
    }

    // HolyG.pas:254
    public void ShowSummonAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TMapField Field;
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        Field = AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
        if (this == Field.FindOwnedChild(131987) && AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
        {
            bool condition = AoWHSMap.FSeatedPlayer == Anim.FTag;
            if (!condition)
            {
                var lhx2 = GetLhx();
                var yhx2 = GetYhx();
                var xhx2 = GetXhx();
                condition = AoWHSMap.WatchingTerrain(xhx2, yhx2, lhx2, 0);
            }

            if (condition)
            {
                AoWHSSet.FSpellControl.GetSpell(13).FCastSounds.Play(0);
            }
        }
    }

    // HolyG.pas:265
    public void NewFrameSummonAnimation(TAnimation Anim, int FrameCounter)
    {
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        Anim = (TAnimation)(object)AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
        if (this == ((TMapField)(object)Anim).FindOwnedChild(131987) && FrameCounter == 9)
        {
            FMode = FMode - TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
        }
    }

}

public partial class THolyGroundResource : TILTerrainMOR
{
    public THolyGroundResource(TECustomNode owner) : base(owner)
    {}

    // HolyG.pas:276
    public new const int ClassID = 131988;

    // HolyG.pas:279
    public override int ResourceUserClassID() => 131987;

    // HolyG.pas:273
    public override string ResourceEditName() => TranslateRStr(HolyGroundRStr);

}

public partial class THolyGroundSpellTE : TGlobalTargetSpellTE
{
    public THolyGroundSpellTE(TECustomNode owner) : base(owner)
    {}

    // HolyG.pas:282
    public new const int ClassID = 135942;

    // HolyG.pas:329
    public override bool Process(TAbstractUnit source, int value)
    {
        base.Process(source, value);
        CreateHolyGround(source);
        AoWHSMap.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(TAbstractUnit Caster)
    {
        TMapLevel Level = default;
        THNWalkData Walk = default;
        THolyGroundResource Resource = default;
        TAoWMapField Field = default;
        THolyGround Ground = default;
        TImageSequenceAnimation Anim = default;
        Level = AoWHSMap.MapContainer.GetMapLevel(FLocation.L);
        InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        Resource = (THolyGroundResource)AoWHSSet.MapObjectResourceList.FindChild(131988);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (TAoWMapField)Level.Field[Walk.X, Walk.Y];
                if (((THolyGroundSpell)AoWHSSet.FSpellControl.GetSpell(13)).ValidTargetMapF(Field))
                {
                    if (Field.GetArmyHS() == null)
                    {
                        Field.RestoreTerrain();
                        Ground = (THolyGround)Resource.CreateResourceUser();
                        Ground.FDuration = (sbyte)((AoWHSMap.Random(11) - 5) / 5 + 3);
                        Ground.FPlayer = Caster.FPlayer;
                        Ground.FOriginalOverlay = Field.OverlayType;
                        AoWHSMap.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                        if (AoWHSMap.PlaceHX(Ground, Walk.X, Walk.Y, FLocation.L))
                        {
                            Ground.FMode = TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
                            Anim = new TImageSequenceAnimation(null);
                            Anim.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(13).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Ground.ShowSummonAnimation;
                            Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                            Anim.FFrameCounter = -AoWHSMap.Random(15);
                            Anim.FTag = Caster.FPlayer;
                            AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                        }
                        else
                        {
                            Ground.Free();
                        }
                    }
                }
            }

            WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230


}

public partial class THolyGroundSpell : TGlobalTargetSpell
{
    public THolyGroundSpell(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(HolyGroundsRStr);
        FSpellID = 13;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msLife;
        FAboveGroundOnly = false;
    }

    // HolyG.pas:339


    // HolyG.pas:375
    public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // HolyG.pas:384
    public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
    {
        THolyGroundSpellTE Event;
        Event = new THolyGroundSpellTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // HolyG.pas:360
    public override bool ValidTargetSelectionMapF(TMapField Field)
    {
        TArmyHS Army;
        bool Result = base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((TAoWMapField)Field).TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
        Result = (Result && ((TAoWMapField)Field).OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((TAoWMapField)Field).OverlayType);
        Result = Result && ((TAoWMapField)Field).FindNoneTransparentHS(typeof(TStructure)) == null;
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

    // HolyG.pas:350
    public override bool ValidTargetMapF(TMapField Field)
    {
        bool Result = base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((TAoWMapField)Field).TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
        Result = (Result && ((TAoWMapField)Field).OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((TAoWMapField)Field).OverlayType);
        return Result && ((TAoWMapField)Field).FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;
    }

    // HolyG.pas:63

}

public static partial class HolyGUnit
{
    // HolyG.pas:395
    public static void RegisterHolyGround(TEngine Engine)
    {
        Engine.RegisterEClasses([EClass.Of<THolyGround>(), EClass.Of<THolyGroundResource>(), EClass.Of<THolyGroundSpellTE>()]);
        var holyGroundSpell = new THolyGroundSpell(null);
        ((TAoWEngine)Engine).FGameSet.FSpellControl.RegisterSpell(holyGroundSpell);
    }

    // HolyG.pas:401
    public static void UnRegisterHolyGround(TEngine Engine)
    {
        Engine.UnregisterEClasses([EClass.Of<THolyGround>(), EClass.Of<THolyGroundResource>(), EClass.Of<THolyGroundSpellTE>()]);
        ((TAoWEngine)Engine).FGameSet.FSpellControl.UnregisterSpellID(13);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
