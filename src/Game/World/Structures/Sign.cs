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

// Text, TextSeen
public partial class Sign : Structure
{
    public Sign(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
        TextSeen = new BitList();
    }

    // Sign.pas:99
    public new const int ClassId = 131330;

    public StringList Text { get; }
    public BitList TextSeen { get; }

    // Sign.pas:92
    public override void Dispose()
    {
        Text?.Dispose();
        TextSeen?.Dispose();
        base.Dispose();
    }

    // Sign.pas:105
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStrings(Text, 7);
        if (AoWHSMap.Current != null && AoWHSMap.Current.ConnectionType != (PlayerConnectionType)1)
        {
            stream.rwStreamProc(8, TextSeen.LoadFromStream, TextSeen.SaveToStream);
        }
    }

    // Sign.pas:85

    // Sign.pas:102
    public override sbyte GetOverlay(int index) => -1;

    // Sign.pas:119
    public override void Select()
    {
        base.Select();
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            TextSeen[AoWHSMap.Current.SeatedPlayer] = true;
        }
    }

    // Sign.pas:113
    public bool GetTextSeen(sbyte player) => TextSeen[player];

    // Sign.pas:116
    public void SetText(StringCollection text)
    {
        Text.Assign(text);
        Update();
    }

    // Sign.pas:125
    public bool CanChangeText()
    {
        ArmyHexSprite armyHexSprite;
        armyHexSprite = ((AoWMapField)Owner).GetArmyHexSprite();
        return armyHexSprite != null && (byte)armyHexSprite.Army.Player == (byte)AoWHSMap.Current.SeatedPlayer;
    }

    // Sign.pas:134
    public bool ChangeText(int player, StringCollection text)
    {
        bool result;
        UpdateSignTokenEvent eventValue;
        if (!AoWHSMap.Current.Players.GetPlayers(player).GetBusy() && CanChangeText())
        {
            eventValue = new UpdateSignTokenEvent(null);
            eventValue.HexX = GetHexX();
            eventValue.HexY = GetHexY();
            eventValue.HexLevel = GetHexLevel();
            eventValue.Text.Assign(text);
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            ConnectRoads = false;
            TerrainBound = false;
        }

        // Sign.pas:159
        public new const int ClassId = 131331;

        // Sign.pas:152

        // Sign.pas:165
        public override int ResourceUserClassId() => 131330;

        // Sign.pas:168
        public override void Show(int X, int Y, in LockedSurface lockedSurface, Structure structure)
        {
            ImageSequence sequence;
            int index;
            int offset;
            int image;
            bool animate;
            if (ImageSequenceList != null)
            {
                index = structure.ImageIndex;
                offset = ((MapField)structure.Owner).HexX + ((MapField)structure.Owner).HexY * 16;
                animate = AoWHSMap.Current.SeatedPlayer < 0 || !((Sign)structure).GetTextSeen(AoWHSMap.Current.SeatedPlayer);
                if (index < ImageSequenceList.Count)
                {
                    sequence = (ImageSequence)ImageSequenceList.List[index];
                }
                else
                {
                    sequence = null;
                }

                while (sequence != null)
                {
                    if (animate)
                    {
                        image = sequence.GetFrameIndex(AoWHSMap.Current.Frame, offset);
                    }
                    else
                    {
                        image = sequence.GetUnloopedFrameIndex(0);
                    }

                    if (image != -1)
                    {
                        if (structure.GetSelected() && (AoWHSMap.Current.Frame & 6) == 6)
                        {
                            sequence.ImageLibrary[image].ShowTransparent(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                        }
                        else
                        {
                            sequence.ImageLibrary[image].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                        }
                    }

                    sequence = sequence.Next;
                }
            }
        }

        // Sign.pas:37

        // Sign.pas:162
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_signText);

        private static readonly string s_signText = "Sign";

    }

    // Sign.pas:24

}
