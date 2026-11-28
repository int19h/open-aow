#nullable disable warnings
// Translated from Sign.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Text, TextSeen
public partial class Sign(CustomNode? owner) : Structure(owner)
{

    // Sign.pas:99
    public new const int ClassId = 131_330;

    public DelphiStringList Text { get; } = [];
    public BitList TextSeen { get; } = [];

    // Sign.pas:92
    public override void Dispose() => base.Dispose();

    // Sign.pas:105
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Lines(name: "Text", lines: Text);
        StrategicMap? strategicMap = GetEngine()?.StrategicMap;
        if (strategicMap is not null && strategicMap.ConnectionType != PlayerConnectionType.Network)
        {
            _ = persister.BitList(name: "TextSeen", list: TextSeen);
        }
        else
        {
            // The original ReadWrite leaves a stored TextSeen unread with no map or on a network map.
            persister.Skip("TextSeen");
        }
    }

    // Sign.pas:102
    public override FootprintCell<OverlayType> GetOverlay(int index) => FootprintCell<OverlayType>.Keep;

    // Sign.pas:119
    public override void Select()
    {
        base.Select();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (!strategicMap.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            TextSeen[PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer)] = true;
        }
    }

    // Sign.pas:113
    public bool GetTextSeen(PlayerId? player) => TextSeen[PlayerIdExtensions.ToOriginal(player)];

    // Sign.pas:116
    public void SetText(DelphiStringCollection text)
    {
        Text.Assign(text);
        Update();
    }

    // Sign.pas:125
    public bool CanChangeText()
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        return armyHexSprite is not null && (byte)PlayerIdExtensions.ToOriginal(armyHexSprite.Army.Player) == (byte)PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer);
    }

    // Sign.pas:134
    public bool ChangeText(PlayerId? player, DelphiStringCollection text)
    {
        if (!GetEngine().StrategicMap.Players.GetPlayers(player).GetBusy() && CanChangeText())
        {
            UpdateSignTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                Location = new MapCoordinate(HexX, HexY, HexLevel)
            };
            eventValue.Text.Assign(text);
            GetEngine().StrategicMap.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            ConnectRoads = false;
            TerrainBound = false;
        }

        // Sign.pas:159
        public new const int ClassId = 131_331;

        // Sign.pas:165
        public override int ResourceUserClassId() => Sign.ClassId;

        // Sign.pas:168
        public override void Show(int X, int Y, in LockedSurface lockedSurface, Structure structure)
        {
            if (ImageSequenceList is not null)
            {
                int index = structure.ImageIndex;
                int offset = structure.Owner.HexX + (structure.Owner.HexY * 16);
                // Show takes every structure. This resource draws a sign, and another kind fails here.
                StrategicMap strategicMap = GetEngine().StrategicMap;
                bool animate = PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) < 0 || !((Sign)structure).GetTextSeen(strategicMap.SeatedPlayer);
                ImageSequence? sequence = index < ImageSequenceList.Count ? ImageSequenceList.List[index] : null;

                while (sequence is not null)
                {
                    int image = animate ? sequence.GetFrameIndex(GetEngine().StrategicMap.Frame, offset) : sequence.GetUnloopedFrameIndex(0);

                    if (image != -1)
                    {
                        if (structure.GetSelected() && (GetEngine().StrategicMap.Frame & 0b0110) == 0b0110)
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

        // Sign.pas:162
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_signText);

        private static readonly string s_signText = "Sign";

    }
}
