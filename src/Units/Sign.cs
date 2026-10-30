// Translated from Sign.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Sign
{
    // { TSign (instance size 56, VMT 557A4098): +$30 FText, +$34 FTextSeen. }
    public partial class TSign : Aow.Units.AoWE.TStructure
    {
        public TSign(TECustomNode owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FText = call__1;
            var call__2 = new Aow.Units.Engine.TBitList();
            FTextSeen = call__2;
        }

        // Sign.pas:99
        public new const int ClassID = 131330;

        public Aow.Rtl.TStringList FText;
        public Aow.Units.Engine.TBitList FTextSeen;
        // Sign.pas:92
        public override void Destroy()
        {
            FText.Free();
            FTextSeen.Free();
            base.Destroy();
        }

        // Sign.pas:105
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 7);
            if ((Aow.Units.AoWE.AoWE.AoWHSMap != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType != ((Aow.Units.AoWE.TPlayerConnectionType)1)))
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
            if (!Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                FTextSeen[Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer] = true;
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
        public void SetText(Aow.Rtl.TStrings Text)
        {
            FText.Assign(Text);
            Update();
        }

        // Sign.pas:125
        public bool CanChangeText()
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__4 = ((Aow.Units.AoWE.TAoWMapField)Owner).GetArmyHS();
            ArmyHS = call__4;
            Result = (ArmyHS != null) && (unchecked((byte)ArmyHS.FArmy.FPlayer) == unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            return Result;
        }

        // Sign.pas:134
        public bool ChangeText(int Player, Aow.Rtl.TStrings Text)
        {
            bool Result = default;
            Aow.Units.Sign.TUpdateSignTE Event = default;
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__6 = call__5.GetBusy();
            bool bool__7 = !call__6;
            if (bool__7)
            {
                var call__8 = CanChangeText();
                bool__7 = call__8;
            }

            if (bool__7)
            {
                Event = new Aow.Units.Sign.TUpdateSignTE(null);
                var call__9 = GetXhx();
                Event.FXhx = unchecked((sbyte)call__9);
                var call__10 = GetYhx();
                Event.FYhx = unchecked((sbyte)call__10);
                var call__11 = GetLhx();
                Event.FLhx = unchecked((sbyte)call__11);
                Event.FText.Assign(Text);
                var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Player);
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

    // { TSignResource (instance size 100, VMT 557A42EC) }
    public partial class TSignResource : Aow.Units.AoWE.TStructureResource
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
        public override void Show(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc, Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Index = default;
            int Offset = default;
            int Image = default;
            bool Animate = default;
            if (FImageSequenceList != null)
            {
                Index = ((Aow.Units.Sign.TSign)Structure).ImageIndex;
                Offset = ((Aow.Units.HSEngine.TMapField)((Aow.Units.Sign.TSign)Structure).Owner).Xhx + (((Aow.Units.HSEngine.TMapField)((Aow.Units.Sign.TSign)Structure).Owner).Yhx * 16);
                bool bool__13 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer < 0);
                if (!bool__13)
                {
                    var call__14 = ((Aow.Units.Sign.TSign)Structure).GetTextSeen(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__13 = !call__14;
                }

                Animate = bool__13;
                if (Index < FImageSequenceList.Count)
                {
                    Sequence = (Aow.Units.ImageLib.TImageSequence)FImageSequenceList.List[Index];
                }
                else
                {
                    Sequence = null;
                }

                while (Sequence != null)
                {
                    if (Animate)
                    {
                        var call__15 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Offset);
                        Image = call__15;
                    }
                    else
                    {
                        Image = Sequence.GetUnloopedFrameIndex(0);
                    }

                    if (Image != -1)
                    {
                        var call__16 = ((Aow.Units.Sign.TSign)Structure).GetSelected();
                        if (call__16 && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 6) == 6))
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__19 = Aow.Units.AoWE.AoWE.SignRStr;
            var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(call__19);
            Result = call__20;
            return Result;
        }

    }

    // { TUpdateSignTE (instance size 24, VMT 557A43DC): +$10 FText, +$14 FXhx, +$15 FYhx, +$16 FLhx. }
    public partial class TUpdateSignTE : Aow.Units.NetworkE.TTokenEvent
    {
        public TUpdateSignTE(TECustomNode owner) : base(owner)
        {
            var call__21 = new Aow.Rtl.TStringList();
            FText = call__21;
        }

        // Sign.pas:202
        public new const int ClassID = 131343;

        public Aow.Rtl.TStringList FText;
        public sbyte FXhx;
        public sbyte FYhx;
        public sbyte FLhx;
        public Aow.Units.FixedArray_byte_1 F_UpdateSignTE_17;
        // Sign.pas:199
        public override void Destroy()
        {
            FText.Free();
            base.Destroy();
        }

        // Sign.pas:205
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 15);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 16);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 17);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 18);
        }

        // Sign.pas:196


        // Sign.pas:214
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Aow.Units.Sign.TSign Sign = default;
            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__23 = call__22.FindOwnedChild(131330);
            Sign = (Aow.Units.Sign.TSign)call__23;
            if (Sign != null)
            {
                Sign.SetText(FText);
                Sign.FTextSeen.Clear();
                Sign.FTextSeen[Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer] = true;
                Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            }
            else
            {
                Result = Aow.Units.NetworkE.TExecuteResult.erDoneHalt;
            }

            return Result;
        }

        // Sign.pas:54


    }

    [StructLayout(LayoutKind.Explicit, Size = 315)]
    public struct TSignMapFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_164 Reserved;
        [FieldOffset(164)]
        public sbyte CurrentPlayer;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
        [FieldOffset(166)]
        public Aow.Units.FixedArray_byte_148 ReservedA6;
        [FieldOffset(314)]
        public byte StorageType;
    }

    public static partial class Sign
    {
        // Sign.pas:229
        public static void RegisterSign(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Sign.TSign>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sign.TSignResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sign.TUpdateSignTE>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
