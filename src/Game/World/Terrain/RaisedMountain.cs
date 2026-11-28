#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Mountain.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Terrain;

// Mode, Player, Duration, OriginalOverlay
public class RaisedMountain(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // Mountain.pas:178
    public new const int ClassId = 131_434;

    public RaisedMountainModes Mode { get; set; }

    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int Duration { get; set; }
    public int OriginalOverlay { get; set; }

    // Mountain.pas:181
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.OptionalPlayer("Player", value: Player, fallback: -1, width: IdWidth.SByte);
        Duration = persister.SByte("Duration", value: Duration.TruncateToSByte(), fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay.TruncateToSByte(), fallback: -1);
    }

    // Mountain.pas:210
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn(PlayerId.FromInt32((sbyte)message.Payload.Int));
        }
    }

    // Mountain.pas:223
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!Mode.Contains(RaisedMountainModes.Raising))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // Mountain.pas:216
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4402)
        {
            if (base.Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:189
    public void RestoreTerrain()
    {
        MapField field = Owner;
        int overlay = OriginalOverlay;
        Dispose();
        if (overlay > 0)
        {
            GetEngine().StrategicMap.PlaceTerrain(field.HexX, field.HexY, L: field.HexLevel, terrainType: field.TerrainNumber, overlayType: overlay);
        }
    }

    // Mountain.pas:201
    public void NewTurn(PlayerId player)
    {
        if (player == Player && Duration > 0)
        {
            Duration = (Duration - 1).TruncateToSByte();
            if (Duration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:228
    public void ShowRaiseTerrainAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int hexLevel = animation.HexLevel;
        int hexY = animation.HexY;
        int hexX = animation.HexX;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapField field = strategicMap.MapContainer.GetField(hexX, hexY, hexLevel);
        // { Original tests the base mountain ID, although this object is raised terrain. }
        if (this == field.FindOwnedChild(MountainMapObject.ClassId) && animation.FrameCounter == 0)
        {
            bool condition = PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) == animation.Tag;
            if (!condition)
            {
                int hexLevel2 = HexLevel;
                int hexY2 = HexY;
                int hexX2 = HexX;
                condition = strategicMap.WatchingTerrain(hexX2, hexY2, L: hexLevel2, radius: 0);
            }

            if (condition)
            {
                _ = GetEngine().GamePack.SpellControl[19].CastSounds.Play(0);
            }
        }
    }

    // Mountain.pas:240
    public void NewFrameRaiseTerrainAnimation(Animation animation, int frameCounter)
    {
        int hexLevel = animation.HexLevel;
        int hexY = animation.HexY;
        int hexX = animation.HexX;
        MapField field = GetEngine().StrategicMap.MapContainer.GetField(hexX, hexY, hexLevel);
        if (field.FindOwnedChild(ClassId) == this && frameCounter == 9)
        {
            Mode &= ~RaisedMountainModes.Raising;
        }
    }

    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // Mountain.pas:249
        public new const int ClassId = 131_435;

        // Mountain.pas:255
        public override int ResourceUserClassId() => RaisedMountain.ClassId;

        // Mountain.pas:252
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_raisedMountainText);

        private static readonly string s_raisedMountainText = "Raised Mountain";

    }
}
