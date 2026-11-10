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

// FDestinationX, FDestinationY, FDestinationL, FState (bit 0 razed)
public partial class Teleport : Structure
{
    public Teleport(ECustomNode owner) : base(owner)
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
    public Set<StateElement> FState;

    // Teleport.pas:122
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationX), 7);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationY), 8);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationL), 9);
        RwSet(ref FState, 1, 10, Stream.rwData, 0);
    }

    // Teleport.pas:97

    // Teleport.pas:157
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.HN == 0 && Msg.MsgID == 4416 && Msg.Param[1].As<MapField>().Xhx == FDestinationX && Msg.Param[1].As<MapField>().Yhx == FDestinationY && Msg.Param[1].As<MapField>().Lhx == FDestinationL)
        {
            bool notFogEnabled = !AoWHSMap.Current.FFogEnabled;
            if (!notFogEnabled)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                notFogEnabled = Aow.Game.World.Map.AoWHSMap.Current.VisibleForPlayer(xhx, yhx, lhx, AoWHSMap.Current.FSeatedPlayer);
            }

            if (notFogEnabled || Aow.Game.World.Map.AoWHSMap.Current.VisibleForPlayer(FDestinationX, FDestinationY, FDestinationL, AoWHSMap.Current.FSeatedPlayer))
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in HSSenderInfo Info)
    {
        if (Info.HN == 0 && !FState.Contains(StateElement.Razed))
        {
            var fieldValue = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
            ((MoveControl)Info.Sender).ExecuteExternalMove((AoWMapField)Info.MapField, (AoWMapField)fieldValue, 0);
        }
    }

    // Teleport.pas:175
    public override void Activate()
    {
        base.Activate();
        ((MapField)Owner).UpdateMapField();
    }

    // Teleport.pas:178
    public override void Deactivate()
    {
        base.Deactivate();
        FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
        ((MapField)Owner).UpdateMapField();
    }

    // Teleport.pas:131
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags | 32768;
        }
    }

    // Teleport.pas:115
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            FState = FState + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            FState = FState - Set<StateElement>.Of(StateElement.Razed);
        }

        this.FImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Teleport.pas:279
    public override bool ValidateMap(StringCollection Messages)
    {
        bool Result;
        if (FDestinationX == 0 || FDestinationY == 0)
        {
            Messages.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.WarningRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(InvalidLocationSpecifiedRStr));
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
        ImageSequenceAnimation Anim;
        Anim = new ImageSequenceAnimation(null);
        Anim.SetImageSequence(AoWHSSet.Current.FImageSequences.ImageSequences[13]);
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Anim, xhx, yhx, lhx);
        Anim = new ImageSequenceAnimation(null);
        Anim.SetImageSequence(AoWHSSet.Current.FImageSequences.ImageSequences[12]);
        Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Anim, FDestinationX, FDestinationY, FDestinationL);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(ArmyHS ArmyHS)
    {
        AoWMapField Field;
        ArmyHS OtherArmy;
        bool Result = false;
        if (FDestinationL >= 0 && FDestinationL < AoWHSMap.Current.MapContainer.LevelCount && FDestinationX >= 0 && FDestinationX < AoWHSMap.Current.MapContainer.Width && FDestinationY >= 0 && FDestinationY < AoWHSMap.Current.MapContainer.Height)
        {
            Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
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
        ArmyHS ArmyHS;
        ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
        // { Original only checks destination X here; X=0 is rejected. }
        return ArmyHS != null && Player == ArmyHS.FArmy.FPlayer && !FState.Contains(StateElement.Razed) && FDestinationX > 0;
    }

    // Teleport.pas:218
    public bool TeleportPlayer(sbyte Player)
    {
        bool Result;
        ArmyHS ArmyHS;
        Movepath Path;
        AoWMapField Field;
        TokenEvent Event;
        string MessageText;
        if (CanTeleport(Player))
        {
            ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
            if (CanTeleportArmy(ArmyHS))
            {
                ArmyHS.FArmy.ResetDestinationPos();
                Path = new Movepath();
                Path.AddXYL(FDestinationX, FDestinationY, FDestinationL);
                var lhx = GetLhx();
                var yhx = GetYhx();
                Path.AddXYL(GetXhx(), yhx, lhx);
                Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != MoveAction.Attack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (Field.GetPlayer() > -1 && Field.GetPlayer() != ArmyHS.FArmy.FPlayer)
                    {
                        if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                        {
                            // { Original passes action before mode. }
                            Event = global::Aow.Game.Draw.ArmyHS.CreateArmyMoveTE(ArmyHS, 255, Path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer).AddEvent(Event);
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
                        Result = global::Aow.Game.Draw.ArmyHS.MoveArmyEx(ArmyHS, 255, Path, Set<ArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (byte)AoWHSMap.Current.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer)
                    {
                        Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(FDestinationX, FDestinationY, FDestinationL);
                        Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    var player = Field.GetPlayer();
                    var playerPlayerRace = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(player);
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameTables.RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                    MessageText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.NoWarDeclaredtoXRStr), [raceNamesPlural]);
                    Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(MessageText);
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

    public enum StateElement : byte
    {
        Razed = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TeleportRStr);

        internal static readonly string TeleportRStr = "Teleport";

    }
}

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct TeleportFieldFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct TeleportMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct TeleportPlayerFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte Race;
}

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct TeleportImageFields
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
}
