// Translated from Sign.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Sign
{
    // FText, FTextSeen
    public partial class TSign : TStructure
    {
        public TSign(TECustomNode owner) : base(owner)
        {
            var call__1 = new TStringList();
            FText = call__1;
            var call__2 = new TBitList();
            FTextSeen = call__2;
        }

        // Sign.pas:99
        public new const int ClassID = 131330;

        public TStringList FText;
        public TBitList FTextSeen;
        // Sign.pas:92
        public override void Destroy()
        {
            FText.Free();
            FTextSeen.Free();
            base.Destroy();
        }

        // Sign.pas:105
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 7);
            if (AoWHSMap != null && AoWHSMap.FConnectionType != (TPlayerConnectionType)1)
            {
                Stream.rwStreamProc(8, FTextSeen.LoadFromStream, FTextSeen.SaveToStream);
            }
        }

        // Sign.pas:85


        // Sign.pas:102
        public override sbyte GetOverlay(int Index) => -1;

        // Sign.pas:119
        public override void Select()
        {
            base.Select();
            if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                FTextSeen[AoWHSMap.FSeatedPlayer] = true;
            }
        }

        // Sign.pas:113
        public bool GetTextSeen(sbyte Player)
        {
            bool Result = default;
            var call__3 = FTextSeen[Player];
            Result = call__3;
            return Result;
        }

        // Sign.pas:116
        public void SetText(TStrings Text)
        {
            FText.Assign(Text);
            Update();
        }

        // Sign.pas:125
        public bool CanChangeText()
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            var call__4 = ((TAoWMapField)Owner).GetArmyHS();
            ArmyHS = call__4;
            Result = ArmyHS != null && (byte)ArmyHS.FArmy.FPlayer == (byte)AoWHSMap.FSeatedPlayer;
            return Result;
        }

        // Sign.pas:134
        public bool ChangeText(int Player, TStrings Text)
        {
            bool Result = default;
            TUpdateSignTE Event = default;
            var call__5 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__6 = call__5.GetBusy();
            bool bool__7 = !call__6;
            if (bool__7)
            {
                var call__8 = CanChangeText();
                bool__7 = call__8;
            }

            if (bool__7)
            {
                Event = new TUpdateSignTE(null);
                var call__9 = GetXhx();
                Event.FXhx = call__9;
                var call__10 = GetYhx();
                Event.FYhx = call__10;
                var call__11 = GetLhx();
                Event.FLhx = call__11;
                Event.FText.Assign(Text);
                var call__12 = AoWHSMap.FTokenManager.GetTokenControl(Player);
                call__12.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sign.pas:24


    }

    public partial class TSignResource : TStructureResource
    {
        public TSignResource(TECustomNode owner) : base(owner)
        {
            FConnectRoads = false;
            FTerrainBound = false;
        }

        // Sign.pas:159
        public new const int ClassID = 131331;

        // Sign.pas:152


        // Sign.pas:165
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131330;
            return Result;
        }

        // Sign.pas:168
        public override void Show(int X, int Y, in TSurfaceDesc SurfaceDesc, TStructure Structure)
        {
            TImageSequence Sequence = default;
            int Index = default;
            int Offset = default;
            int Image = default;
            bool Animate = default;
            if (FImageSequenceList != null)
            {
                Index = ((TSign)Structure).ImageIndex;
                Offset = ((TMapField)((TSign)Structure).Owner).Xhx + ((TMapField)((TSign)Structure).Owner).Yhx * 16;
                bool bool__13 = AoWHSMap.FSeatedPlayer < 0;
                if (!bool__13)
                {
                    var call__14 = ((TSign)Structure).GetTextSeen(AoWHSMap.FSeatedPlayer);
                    bool__13 = !call__14;
                }

                Animate = bool__13;
                if (Index < FImageSequenceList.Count)
                {
                    Sequence = (TImageSequence)FImageSequenceList.List[Index];
                }
                else
                {
                    Sequence = null;
                }

                while (Sequence != null)
                {
                    if (Animate)
                    {
                        var call__15 = Sequence.GetFrameIndex(AoWHSMap.Frame, Offset);
                        Image = call__15;
                    }
                    else
                    {
                        Image = Sequence.GetUnloopedFrameIndex(0);
                    }

                    if (Image != -1)
                    {
                        var call__16 = ((TSign)Structure).GetSelected();
                        if (call__16 && (AoWHSMap.Frame & 6) == 6)
                        {
                            var call__17 = Sequence.ImageLibrary[Image];
                            call__17.ShowTransparent(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                        }
                        else
                        {
                            var call__18 = Sequence.ImageLibrary[Image];
                            call__18.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }
        }

        // Sign.pas:37


        // Sign.pas:162
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__19 = SignRStr;
            var call__20 = TranslateRStr(call__19);
            Result = call__20;
            return Result;
        }

    }

    // FText, FXhx, FYhx, FLhx
    public partial class TUpdateSignTE : TTokenEvent
    {
        public TUpdateSignTE(TECustomNode owner) : base(owner)
        {
            var call__21 = new TStringList();
            FText = call__21;
        }

        // Sign.pas:202
        public new const int ClassID = 131343;

        public TStringList FText;
        public sbyte FXhx;
        public sbyte FYhx;
        public sbyte FLhx;
        // Sign.pas:199
        public override void Destroy()
        {
            FText.Free();
            base.Destroy();
        }

        // Sign.pas:205
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 15);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 16);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 17);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 18);
        }

        // Sign.pas:196


        // Sign.pas:214
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            TSign Sign = default;
            var call__22 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__23 = call__22.FindOwnedChild(131330);
            Sign = (TSign)call__23;
            if (Sign != null)
            {
                Sign.SetText(FText);
                Sign.FTextSeen.Clear();
                Sign.FTextSeen[AoWHSMap.FCurrentPlayer] = true;
                Result = TExecuteResult.erDone;
            }
            else
            {
                Result = TExecuteResult.erDoneHalt;
            }

            return Result;
        }

        // Sign.pas:54


    }

    [StructLayout(LayoutKind.Explicit, Size = 315)]
    public struct TSignMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_164 Reserved;
        [FieldOffset(164)]
        public sbyte CurrentPlayer;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
        [FieldOffset(166)]
        public FixedArray_byte_148 ReservedA6;
        [FieldOffset(314)]
        public byte StorageType;
    }

    public static partial class Sign
    {
        // Sign.pas:229
        public static void RegisterSign(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSign>(), EClass.Of<TSignResource>(), EClass.Of<TUpdateSignTE>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
