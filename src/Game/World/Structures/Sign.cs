// Translated from Sign.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

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
        if (AoWHSMap.Current != null && AoWHSMap.Current.FConnectionType != (PlayerConnectionType)1)
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
        if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            FTextSeen[AoWHSMap.Current.FSeatedPlayer] = true;
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
        return ArmyHS != null && (byte)ArmyHS.FArmy.FPlayer == (byte)AoWHSMap.Current.FSeatedPlayer;
    }

    // Sign.pas:134
    public bool ChangeText(int Player, StringCollection Text)
    {
        bool Result;
        UpdateSignTE Event;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy() && CanChangeText())
        {
            Event = new UpdateSignTE(null);
            Event.FXhx = GetXhx();
            Event.FYhx = GetYhx();
            Event.FLhx = GetLhx();
            Event.FText.Assign(Text);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
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
                Animate = AoWHSMap.Current.FSeatedPlayer < 0 || !((Sign)Structure).GetTextSeen(AoWHSMap.Current.FSeatedPlayer);
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
                        Image = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, Offset);
                    }
                    else
                    {
                        Image = Sequence.GetUnloopedFrameIndex(0);
                    }

                    if (Image != -1)
                    {
                        if (Structure.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
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
        public override string ResourceEditName() => AbilityRules.TranslateRStr(SignRStr);

        private static readonly string SignRStr = "Sign";

    }

    // Sign.pas:24

}
