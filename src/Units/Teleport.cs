// Translated from Teleport.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

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
            bool notFogEnabled = !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFogEnabled;
            if (!notFogEnabled)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                notFogEnabled = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(xhx, yhx, lhx, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
            }

            if (notFogEnabled || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(FDestinationX, FDestinationY, FDestinationL, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in THSSenderInfo Info)
    {
        if (Info.HN == 0 && !FState.Contains(TTeleportStateElement.tsRazed))
        {
            var fieldValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
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
        FStatus = FStatus - TSet<Aow.Game.World.TMapObjectStatusElement>.Of(Aow.Game.World.TMapObjectStatusElement.moActivated);
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
            Messages.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WarningRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TTeleport.InvalidLocationSpecifiedRStr));
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
        Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FImageSequences.ImageSequences[13]);
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
        Anim = new TImageSequenceAnimation(null);
        Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FImageSequences.ImageSequences[12]);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, FDestinationX, FDestinationY, FDestinationL);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(TArmyHS ArmyHS)
    {
        TAoWMapField Field;
        TArmyHS OtherArmy;
        bool Result = false;
        if (FDestinationL >= 0 && FDestinationL < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.LevelCount && FDestinationX >= 0 && FDestinationX < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.Width && FDestinationY >= 0 && FDestinationY < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.Height)
        {
            Field = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
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
                Field = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != TMoveAction.maAttack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drWar, TDiplomaticRelation.drUnknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (Field.GetPlayer() > -1 && Field.GetPlayer() != ArmyHS.FArmy.FPlayer)
                    {
                        if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                        {
                            // { Original passes action before mode. }
                            Event = Aow.Game.Draw.TArmyHS.CreateArmyMoveTE(ArmyHS, 255, Path, TMoveAction.maAttack, TSet<TArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer).AddEvent(Event);
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
                        Result = Aow.Game.Draw.TArmyHS.MoveArmyEx(ArmyHS, 255, Path, TSet<TArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer)
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(FDestinationX, FDestinationY, FDestinationL);
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    var player = Field.GetPlayer();
                    var playerPlayerRace = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player);
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                    MessageText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.NoWarDeclaredtoXRStr), [raceNamesPlural]);
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(MessageText);
                    Result = false;
                }

                // { Original frees the path only on normal completion. }
                Path?.Dispose();
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

    private static readonly string InvalidLocationSpecifiedRStr = "invalid location specified";
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
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TTeleportResource.TeleportRStr);

    internal static readonly string TeleportRStr = "Teleport";

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
