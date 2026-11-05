// Translated from Teleport.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Teleport;

public enum TTeleportStateElement : byte
{
    tsRazed = 0,
    ts1 = 1,
    ts2 = 2,
    ts3 = 3,
    ts4 = 4,
    ts5 = 5,
    ts6 = 6,
    ts7 = 7
}

public enum TTeleportMoveFlagsElement : byte
{
    tm0 = 0,
    tm1 = 1,
    tm2 = 2,
    tm3 = 3,
    tm4 = 4,
    tm5 = 5,
    tm6 = 6,
    tm7 = 7
}

// FDestinationX, FDestinationY, FDestinationL, FState (bit 0 razed)
public partial class TTeleport : TStructure
{
    public TTeleport(TECustomNode owner) : base(owner)
    {
        FDestinationX = 0;
        FDestinationY = 0;
        FDestinationL = 0;
    }

    // Teleport.pas:105
    public new const int ClassID = 131332;

    public sbyte FDestinationX;
    public sbyte FDestinationY;
    public sbyte FDestinationL;
    public TSet<TTeleportStateElement> FState;
    // Teleport.pas:122
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationX), 7);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationY), 8);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationL), 9);
        RwSet(ref FState, 1, 10, Stream.rwData, 0);
    }

    // Teleport.pas:97


    // Teleport.pas:157
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.HN == 0 && Msg.MsgID == 4416 && Msg.Param[1].As<TMapField>().Xhx == FDestinationX && Msg.Param[1].As<TMapField>().Yhx == FDestinationY && Msg.Param[1].As<TMapField>().Lhx == FDestinationL)
        {
            bool notFogEnabled = !AoWHSMap.FFogEnabled;
            if (!notFogEnabled)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                notFogEnabled = AoWHSMap.VisibleForPlayer(xhx, yhx, lhx, AoWHSMap.FSeatedPlayer);
            }

            if (notFogEnabled || AoWHSMap.VisibleForPlayer(FDestinationX, FDestinationY, FDestinationL, AoWHSMap.FSeatedPlayer))
            {
                PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in THSSenderInfo Info)
    {
        if (Info.HN == 0 && !FState.Contains(TTeleportStateElement.tsRazed))
        {
            var fieldValue = AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
            ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)fieldValue, 0);
        }
    }

    // Teleport.pas:175
    public override void Activate()
    {
        base.Activate();
        ((TMapField)Owner).UpdateMapField();
    }

    // Teleport.pas:178
    public override void Deactivate()
    {
        base.Deactivate();
        FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moActivated);
        ((TMapField)Owner).UpdateMapField();
    }

    // Teleport.pas:131
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((TAoWMapField)(object)Info.MapField).FFieldFlags = ((TAoWMapField)(object)Info.MapField).FFieldFlags | 32768;
        }
    }

    // Teleport.pas:115
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            FState = FState + TSet<TTeleportStateElement>.Of(TTeleportStateElement.tsRazed);
        }
        else
        {
            FState = FState - TSet<TTeleportStateElement>.Of(TTeleportStateElement.tsRazed);
        }

        this.FImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Teleport.pas:279
    public override bool ValidateMap(TStrings Messages)
    {
        bool Result;
        if (FDestinationX == 0 || FDestinationY == 0)
        {
            Messages.Add(TranslateRStr(WarningRStr) + ": " + TranslateRStr(InvalidLocationSpecifiedRStr));
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Teleport.pas:145
    public void ShowTeleportAnimations()
    {
        TImageSequenceAnimation Anim;
        Anim = new TImageSequenceAnimation(null);
        Anim.SetImageSequence(AoWHSSet.FImageSequences.ImageSequences[13]);
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
        Anim = new TImageSequenceAnimation(null);
        Anim.SetImageSequence(AoWHSSet.FImageSequences.ImageSequences[12]);
        AoWHSMap.PlaceHX(Anim, FDestinationX, FDestinationY, FDestinationL);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(TArmyHS ArmyHS)
    {
        TAoWMapField Field;
        TArmyHS OtherArmy;
        bool Result = false;
        if (FDestinationL >= 0 && FDestinationL < AoWHSMap.MapContainer.LevelCount && FDestinationX >= 0 && FDestinationX < AoWHSMap.MapContainer.Width && FDestinationY >= 0 && FDestinationY < AoWHSMap.MapContainer.Height)
        {
            Field = (TAoWMapField)AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
            if (ArmyHS.FArmy.ValidTerrain(Field.TerrainType, Field.OverlayType))
            {
                OtherArmy = Field.GetArmyHS();
                // { Original rejects a same-player army when CanAddUnits succeeds. }
                if (OtherArmy != null && (byte)ArmyHS.FArmy.FPlayer == (byte)OtherArmy.FArmy.FPlayer && OtherArmy.FArmy.CanAddUnits(ArmyHS.FArmy) != 0)
                {
                    return Result;
                }

                Result = true;
            }
        }

        return Result;
    }

    // Teleport.pas:208
    public bool CanTeleport(sbyte Player)
    {
        TArmyHS ArmyHS;
        ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
        // { Original only checks destination X here; X=0 is rejected. }
        return ArmyHS != null && Player == ArmyHS.FArmy.FPlayer && !FState.Contains(TTeleportStateElement.tsRazed) && FDestinationX > 0;
    }

    // Teleport.pas:218
    public bool Teleport(sbyte Player)
    {
        bool Result;
        TArmyHS ArmyHS;
        TMovepath Path;
        TAoWMapField Field;
        TTokenEvent Event;
        string MessageText;
        if (CanTeleport(Player))
        {
            ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
            if (CanTeleportArmy(ArmyHS))
            {
                ArmyHS.FArmy.ResetDestinationPos();
                Path = new TMovepath();
                Path.AddXYL(FDestinationX, FDestinationY, FDestinationL);
                var lhx = GetLhx();
                var yhx = GetYhx();
                Path.AddXYL(GetXhx(), yhx, lhx);
                Field = (TAoWMapField)AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != TMoveAction.maAttack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = AoWHSMap.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drWar, TDiplomaticRelation.drUnknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (Field.GetPlayer() > -1 && Field.GetPlayer() != ArmyHS.FArmy.FPlayer)
                    {
                        if (!AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                        {
                            // { Original passes action before mode. }
                            Event = CreateArmyMoveTE(ArmyHS, 255, Path, TMoveAction.maAttack, TSet<TArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer).AddEvent(Event);
                                Event.Release();
                                Result = true;
                            }
                            else
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                        }
                    }
                    else
                    {
                        Result = MoveArmyEx(ArmyHS, 255, Path, TSet<TArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (byte)AoWHSMap.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer)
                    {
                        AoWHSMap.CenterViewTo(FDestinationX, FDestinationY, FDestinationL);
                        AoWHSMap.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    var player = Field.GetPlayer();
                    var playerPlayerRace = AoWHSMap.FPlayers.GetPlayers(player);
                    var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                    MessageText = Rtl.Rtl.Format(TranslateRStr(NoWarDeclaredtoXRStr), [raceNamesPlural]);
                    AoWHSMap.ShowMessage(MessageText);
                    Result = false;
                }

                // { Original frees the path only on normal completion. }
                Path.Free();
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Teleport.pas:26
    public override bool GetRazed() => (FState.Image & 1) != 0;
}

public partial class TTeleportResource : TStructureResource
{
    public TTeleportResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRazeable = true;
        FRebuildGold = 25;
        FRebuildDays = 2;
    }

    // Teleport.pas:299
    public new const int ClassID = 131333;

    // Teleport.pas:290


    // Teleport.pas:305
    public override int ResourceUserClassID() => 131332;

    // Teleport.pas:45


    // Teleport.pas:302
    public override string ResourceEditName() => TranslateRStr(TeleportRStr);

}

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct TTeleportFieldFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct TTeleportMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct TTeleportPlayerFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte Race;
}

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct TTeleportImageFields
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
}

public static partial class Teleport
{
    // Teleport.pas:308
    public static void RegisterTeleport(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TTeleport>(), EClass.Of<TTeleportResource>()]);
    }

    // Original AoWE resource initialization table
    public static string[] RaceNames = ["Humans", "Azracs", "Lizards", "Frostlings", "Elves", "Halflings", "Dwarves", "Highmen", "Dark Elves", "Orcs", "Goblins", "Undead(plural)", "Independents"];
    public static readonly string TeleportNoWarRStr = "Not at war with the %s";
    // { original RT_STRING $FCD6 }
    public static readonly string TeleportWarningRStr = "Warning";

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
