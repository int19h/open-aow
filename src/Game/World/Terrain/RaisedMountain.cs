// Translated from Mountain.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Terrain;

// Mode, Player, Duration, OriginalOverlay
public partial class RaisedMountain(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // Mountain.pas:178
    public new const int ClassId = 131434;

    public Set<RaisedMountainMode> Mode { get; set; }

    public sbyte Player { get; set; }
    public sbyte Duration { get; set; }
    public sbyte OriginalOverlay { get; set; }

    // Mountain.pas:181
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.SByte("Player", value: Player, fallback: -1);
        Duration = persister.SByte("Duration", value: Duration, fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay, fallback: -1);
    }

    // Mountain.pas:210
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
    }

    // Mountain.pas:223
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!Mode.Contains(RaisedMountainMode.Raising))
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
            if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:189
    public void RestoreTerrain()
    {
        MapField field = Owner;
        sbyte overlay = OriginalOverlay;
        Dispose();
        if (overlay > 0)
        {
            StrategicMap.Current.PlaceTerrain(field.HexX, field.HexY, field.HexLevel, field.TerrainType, overlay);
        }
    }

    // Mountain.pas:201
    public void NewTurn(sbyte player)
    {
        if (player == Player && Duration > 0)
        {
            Duration = (sbyte)(Duration - 1);
            if (Duration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // Mountain.pas:228
    public void ShowRaiseTerrainAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        sbyte hexLevel = animation.GetHexLevel();
        sbyte hexY = animation.GetHexY();
        sbyte hexX = animation.GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        // { Original tests the base mountain ID, although this object is raised terrain. }
        if (this == field.FindOwnedChild(131432) && animation.FrameCounter == 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == animation.Tag;
            if (!condition)
            {
                sbyte hexLevel2 = GetHexLevel();
                sbyte hexY2 = GetHexY();
                sbyte hexX2 = GetHexX();
                condition = StrategicMap.Current.WatchingTerrain(hexX2, hexY2, hexLevel2, 0);
            }

            if (condition)
            {
                _ = GamePack.Current.SpellControl.GetSpell(19).CastSounds.Play(0);
            }
        }
    }

    // Mountain.pas:240
    public void NewFrameRaiseTerrainAnimation(Animation animation, int frameCounter)
    {
        sbyte hexLevel = animation.GetHexLevel();
        sbyte hexY = animation.GetHexY();
        sbyte hexX = animation.GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (field.FindOwnedChild(131434) == this && frameCounter == 9)
        {
            Mode -= Set<RaisedMountainMode>.Of(RaisedMountainMode.Raising);
        }
    }

    public new partial class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // Mountain.pas:249
        public new const int ClassId = 131435;

        // Mountain.pas:255
        public override int ResourceUserClassId() => 131434;

        // Mountain.pas:252
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_raisedMountainText);

        private static readonly string s_raisedMountainText = "Raised Mountain";

    }

}
