// Translated from Sign.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Structures
{

    // FText, FTextSeen
    public partial class Sign : Structure
    {
        public Sign(ECustomNode owner) : base(owner)
        {
            FText = new StringList();
            FTextSeen = new BitList();
        }

        // Sign.pas:99
        public new const int ClassID = 131330;

        public StringList FText;
        public BitList FTextSeen;

        // Sign.pas:92
        public override void Dispose()
        {
            FText?.Dispose();
            FTextSeen?.Dispose();
            base.Dispose();
        }

        // Sign.pas:105
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 7);
            if (Aow.Game.World.Map.AoWHSMap.Current != null && Aow.Game.World.Map.AoWHSMap.Current.FConnectionType != (PlayerConnectionType)1)
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
            if (!Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                FTextSeen[Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer] = true;
            }
        }

        // Sign.pas:113
        public bool GetTextSeen(sbyte Player) => FTextSeen[Player];

        // Sign.pas:116
        public void SetText(StringCollection Text)
        {
            FText.Assign(Text);
            Update();
        }

        // Sign.pas:125
        public bool CanChangeText()
        {
            ArmyHS ArmyHS;
            ArmyHS = ((AoWMapField)Owner).GetArmyHS();
            return ArmyHS != null && (byte)ArmyHS.FArmy.FPlayer == (byte)Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
        }

        // Sign.pas:134
        public bool ChangeText(int Player, StringCollection Text)
        {
            bool Result;
            UpdateSignTE Event;
            if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy() && CanChangeText())
            {
                Event = new UpdateSignTE(null);
                Event.FXhx = GetXhx();
                Event.FYhx = GetYhx();
                Event.FLhx = GetLhx();
                Event.FText.Assign(Text);
                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public new partial class Resource : StructureResource
        {
            public Resource(ECustomNode owner) : base(owner)
            {
                FConnectRoads = false;
                FTerrainBound = false;
            }

            // Sign.pas:159
            public new const int ClassID = 131331;

            // Sign.pas:152

            // Sign.pas:165
            public override int ResourceUserClassID() => 131330;

            // Sign.pas:168
            public override void Show(int X, int Y, in SurfaceDesc SurfaceDesc, Structure Structure)
            {
                ImageSequence Sequence;
                int Index;
                int Offset;
                int Image;
                bool Animate;
                if (FImageSequenceList != null)
                {
                    Index = Structure.ImageIndex;
                    Offset = ((MapField)Structure.Owner).Xhx + ((MapField)Structure.Owner).Yhx * 16;
                    Animate = Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer < 0 || !((Sign)Structure).GetTextSeen(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer);
                    if (Index < FImageSequenceList.Count)
                    {
                        Sequence = (ImageSequence)FImageSequenceList.List[Index];
                    }
                    else
                    {
                        Sequence = null;
                    }

                    while (Sequence != null)
                    {
                        if (Animate)
                        {
                            Image = Sequence.GetFrameIndex(Aow.Game.World.Map.AoWHSMap.Current.Frame, Offset);
                        }
                        else
                        {
                            Image = Sequence.GetUnloopedFrameIndex(0);
                        }

                        if (Image != -1)
                        {
                            if (Structure.GetSelected() && (Aow.Game.World.Map.AoWHSMap.Current.Frame & 6) == 6)
                            {
                                Sequence.ImageLibrary[Image].ShowTransparent(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                            }
                            else
                            {
                                Sequence.ImageLibrary[Image].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }
            }

            // Sign.pas:37

            // Sign.pas:162
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.Sign.Resource.SignRStr);

            private static readonly string SignRStr = "Sign";

        }

        // Sign.pas:24

    }
}

namespace Aow.Game.Commands
{

    // FText, FXhx, FYhx, FLhx
    public partial class UpdateSignTE : TokenEvent
    {
        public UpdateSignTE(ECustomNode owner) : base(owner)
        {
            FText = new StringList();
        }

        // Sign.pas:202
        public new const int ClassID = 131343;

        public StringList FText;
        public sbyte FXhx;
        public sbyte FYhx;
        public sbyte FLhx;

        // Sign.pas:199
        public override void Dispose()
        {
            FText?.Dispose();
            base.Dispose();
        }

        // Sign.pas:205
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 15);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 16);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 17);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 18);
        }

        // Sign.pas:196

        // Sign.pas:214
        public override ExecuteResult Execute(TokenExecuter TE)
        {
            ExecuteResult Result;
            Sign Sign;
            Sign = (global::Aow.Game.World.Structures.Sign)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, FLhx).FindOwnedChild(131330);
            if (Sign != null)
            {
                Sign.SetText(FText);
                Sign.FTextSeen.Clear();
                Sign.FTextSeen[Aow.Game.World.Map.AoWHSMap.Current.FCurrentPlayer] = true;
                Result = ExecuteResult.Done;
            }
            else
            {
                Result = ExecuteResult.DoneHalt;
            }

            return Result;
        }

        // Sign.pas:54

    }
}

namespace Aow.Game.World
{

    [StructLayout(LayoutKind.Explicit, Size = 315)]
    public struct SignMapFields
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
}
