// Translated from Mountain.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.AoWTer;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.GlobalTargetSpells;
using Aow.Units.HSEngine;
using Aow.Units.ILTer;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.Mountain;

public enum AnonymousEnum_rmRaising : byte
{
    rmRaising = 0
}

public partial class TMountainMO : TAoWTerrain
{
    public TMountainMO(TECustomNode owner) : base(owner)
    { }

    // Mountain.pas:101
    public new const int ClassID = 131432;

    // Mountain.pas:104
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result = FResource.TerrainList.Overlay[Index];
        if (Result >= 0)
        {
            Result = 0;
        }

        return Result;
    }

    // Mountain.pas:110
    public override bool CanChangeTerrain(in THSSenderInfo Info) => true;

    // Mountain.pas:136
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int I;
        bool Valid;
        TTerrainList List;
        TMapField MapField;
        int X = default;
        int Y = default;
        THexagonSprite HS;
        I = 0;
        Valid = true;
        List = FResource.TerrainList;
        while (Valid && I < List.Count)
        {
            if (List.Terrain[I] >= 0 || List.Overlay[I] >= 0)
            {
                CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                if (X >= 0 && Y >= 0 && X < MapContainer.Width && Y < MapContainer.Height)
                {
                    MapField = MapContainer.GetField(X, Y, Lhx);
                    HS = MapField.GetMaxLevelNoneTransparentHS();
                    if (HS != null && HS.EClass.ClassID == 131432)
                    {
                        Valid = ValidMountainOverlap(MapField, List.Overlay[I]);
                    }
                    else
                    {
                        Valid = HS == null || HS.GetLevel() < 10;
                    }
                }
            }

            I += 1;
        }

        return Valid && MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(TMapField MapField, sbyte Overlay)
    {
        bool Valid;
        int I;
        TMapObject HS;
        sbyte OtherOverlay;
        Valid = true;
        I = 0;
        while (I != MapField.Count && Valid)
        {
            HS = (TMapObject)MapField.HSList[I];
            if (HS.EClass.ClassID == 131432)
            {
                var hNofHS = MapField.GetHNofHS(HS);
                OtherOverlay = HS.Resource.TerrainList.Overlay[hNofHS];
                Valid = !((OtherOverlay == 0 && Overlay == 0) || (OtherOverlay == 1 && Overlay == 1));
            }

            I += 1;
        }

        return Valid;
    }

}

public partial class TMountainMOR : TAoWTerrainResource
{
    public TMountainMOR(TECustomNode owner) : base(owner)
    { }

    // Mountain.pas:169
    public new const int ClassID = 131433;

    // Mountain.pas:175
    public override int ResourceUserClassID() => 131432;

    // Mountain.pas:172
    public override string ResourceEditName() => TranslateRStr(MountainRStr);

}

// FMode, FPlayer, FDuration, FOriginalOverlay
public partial class TRaisedMountain : TILTerrainMO
{
    public TRaisedMountain(TECustomNode owner) : base(owner)
    { }

    // Mountain.pas:178
    public new const int ClassID = 131434;

    public TSet<AnonymousEnum_rmRaising> FMode;
    public sbyte FPlayer;
    public sbyte FDuration;
    public sbyte FOriginalOverlay;

    // Mountain.pas:181
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwShortintEx(ref FPlayer, -1, 40);
        Stream.rwShortintEx(ref FDuration, -1, 41);
        Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
    }

    // Mountain.pas:210
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
        }
    }

    // Mountain.pas:223
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        if (!FMode.Contains(AnonymousEnum_rmRaising.rmRaising))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // Mountain.pas:216
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4402)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:189
    public void RestoreTerrain()
    {
        TMapField Field;
        sbyte Overlay;
        Field = (TMapField)Owner;
        Overlay = FOriginalOverlay;
        this.Destroy();
        if (Overlay > 0)
        {
            AoWHSMap.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
        }
    }

    // Mountain.pas:201
    public void NewTurn(sbyte Player)
    {
        if (Player == FPlayer && FDuration > 0)
        {
            FDuration = (sbyte)(FDuration - 1);
            if (FDuration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:228
    public void ShowRaiseTerrainAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TMapField Field;
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        Field = AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
        // { Original tests the base mountain ID, although this object is raised terrain. }
        if (this == Field.FindOwnedChild(131432) && Anim.FFrameCounter == 0)
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
                AoWHSSet.FSpellControl.GetSpell(19).FCastSounds.Play(0);
            }
        }
    }

    // Mountain.pas:240
    public void NewFrameRaiseTerrainAnimation(TAnimation Anim, int FrameCounter)
    {
        var lhx = Anim.GetLhx();
        var yhx = Anim.GetYhx();
        var xhx = Anim.GetXhx();
        var field = AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
        if (field.FindOwnedChild(131434) == this && FrameCounter == 9)
        {
            FMode = FMode - TSet<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.rmRaising);
        }
    }

}

public partial class TRaisedMountainResource : TILTerrainMOR
{
    public TRaisedMountainResource(TECustomNode owner) : base(owner)
    { }

    // Mountain.pas:249
    public new const int ClassID = 131435;

    // Mountain.pas:255
    public override int ResourceUserClassID() => 131434;

    // Mountain.pas:252
    public override string ResourceEditName() => TranslateRStr(RaisedMountainRStr);

}

public partial class TRaiseTerrainTE : TGlobalTargetSpellTE
{
    public TRaiseTerrainTE(TECustomNode owner) : base(owner)
    { }

    // Mountain.pas:258
    public new const int ClassID = 135946;

    // Mountain.pas:317
    public override bool Process(TAbstractUnit source, int value)
    {
        base.Process(source, value);
        RaiseTerrain(source);
        AoWHSMap.ValidateTerrainObservation(source.FPlayer, Unsafe.As<TXYL, TLocation>(ref FLocation).X, Unsafe.As<TXYL, TLocation>(ref FLocation).Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer, 1);
        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(TAbstractUnit Caster)
    {
        TList Resources = default;
        TMapLevel Level = default;
        THNWalkData Walk = default;
        TAoWMapField Field = default;
        TRaisedMountain Mountain = default;
        TMapObjectResource Resource = default;
        int I = default;
        bool Found = default;
        TImageSequenceAnimation Anim = default;
        Resources = AoWHSSet.MapObjectResourceList.FChildren;
        Level = AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TLocation>(ref FLocation).Layer];
        InitHNWalk(ref Walk, Unsafe.As<TXYL, TLocation>(ref FLocation).X, Unsafe.As<TXYL, TLocation>(ref FLocation).Y, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (TAoWMapField)Level.Field[Walk.X, Walk.Y];
                Resource = null;
                if (((TGlobalTargetSpell)AoWHSSet.FSpellControl.GetSpell(19)).ValidTargetMapF(Field))
                {
                    Found = false;
                    I = Resources.Count;
                    while (!Found && I != 0)
                    {
                        I -= 1;
                        Resource = (TMapObjectResource)Resources.List[I];
                        Found = Resource.EClass.ClassID == 131435 && Resource.TerrainList.UsedTerrain() == 1;
                    }

                    if (Found)
                    {
                        Field.RestoreTerrain();
                        Mountain = (TRaisedMountain)Resource.CreateResourceUser();
                        Mountain.FDuration = (sbyte)((AoWHSMap.Random(11) - 5) / 5 + 3);
                        Mountain.FPlayer = Caster.FPlayer;
                        Mountain.FOriginalOverlay = Field.OverlayType;
                        AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer, 0, -1);
                        if (AoWHSMap.PlaceHX(Mountain, Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer))
                        {
                            Mountain.FMode = Mountain.FMode + TSet<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.rmRaising);
                            Anim = new TImageSequenceAnimation(null);
                            Anim.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(19).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Mountain.ShowRaiseTerrainAnimation;
                            Anim.FOnNewFrame = Mountain.NewFrameRaiseTerrainAnimation;
                            Anim.FFrameCounter = -AoWHSMap.Random(15);
                            AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer);
                        }
                    }
                }
            }

            WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}

public partial class TRaiseTerrain : TGlobalTargetSpell
{
    public TRaiseTerrain(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(RaiseTerrainRStr);
        FSpellID = 19;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msEarth;
        FAboveGroundOnly = true;
    }

    // Mountain.pas:327

    // Mountain.pas:364
    public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // Mountain.pas:373
    public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
    {
        TRaiseTerrainTE Event;
        Event = new TRaiseTerrainTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // Mountain.pas:347
    public override bool ValidTargetSelectionMapF(TMapField Field)
    {
        bool Valid;
        TArmyHS Army;
        Valid = base.ValidTargetSelectionMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null;
        if (Valid)
        {
            Army = ((TAoWMapField)Field).GetArmyHS();
            if (Army != null)
            {
                Valid = !Army.VisibleForSeatedPlayer();
            }
        }

        return Valid;
    }

    // Mountain.pas:338
    public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;

    // Mountain.pas:77

}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct TLocation
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte Layer;
    [FieldOffset(3)]
    public sbyte Reserved;
}

public static partial class MountainUnit
{
    // Mountain.pas:384
    public static void RegisterMountain(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TMountainMO>(), EClass.Of<TMountainMOR>(), EClass.Of<TRaiseTerrainTE>(), EClass.Of<TRaisedMountain>(), EClass.Of<TRaisedMountainResource>()]);
        var raiseTerrain = new TRaiseTerrain(null);
        ((TAoWEngine)AEngine).FGameSet.FSpellControl.RegisterSpell(raiseTerrain);
    }

    // Mountain.pas:390
    public static void UnRegisterMountain(TEngine AEngine)
    {
        AEngine.UnregisterEClasses([EClass.Of<TMountainMO>(), EClass.Of<TMountainMOR>(), EClass.Of<TRaiseTerrainTE>(), EClass.Of<TRaisedMountain>(), EClass.Of<TRaisedMountainResource>()]);
        // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
        ((TAoWEngine)AEngine).FGameSet.FSpellControl.UnregisterSpellID(12);
    }

    public static readonly string MountainNameRStr = "Mountain";

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
