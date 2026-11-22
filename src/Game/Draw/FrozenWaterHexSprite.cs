#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw;

// signed days remaining, caster player
public class FrozenWaterHexSprite(CustomNode owner) : SingleHexSprite(owner)
{
    public new const int ClassId = 131_436;

    public int DaysRemaining { get; set; }
    public sbyte CasterPlayer { get; set; }

    // Structur.PAS:4487
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DaysRemaining = persister.SByte("DaysRemaining", value: DaysRemaining.TruncateToSByte());
        CasterPlayer = persister.SByte("CasterPlayer", value: CasterPlayer);
    }

    // Structur.PAS:4518
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
    }

    // Structur.PAS:4482
    public override int Level => 28;

    // Structur.PAS:4503
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        if (info.HexNumber == 0 && info.TerrainType is not ((sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundIce))
        {
            Dispose();
        }
    }

    // Structur.PAS:4524
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        sbyte hexY = HexY;
        HexGeometry.HexToPixel(HexX, hexY, out int x, out int y);
        x = x - pixelXSurface + 16;
        y = y - pixelYSurface + 16;
        sbyte hexX = HexX;
        sbyte hexY2 = HexY;
        sbyte hexY3 = HexY;
        GamePack.Current.ImageSequences.ShowLooped(70, x, y, StrategicMap.Current.Frame, offset: hexX + hexY2 + (hexY3 * 2), lockedSurface);
    }

    // Structur.PAS:4494
    public void MeltIce()
    {
        if (Owner.TerrainType == 6)
        {
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            _ = StrategicMap.Current.ChangeTerrain(this, hexX, hexY, L: HexLevel, radius: 0, terrainType: 0);
        }
        else
        {
            if (Owner.TerrainType == 13)
            {
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = StrategicMap.Current.ChangeTerrain(this, hexX, hexY, L: HexLevel, radius: 0, terrainType: 10);
            }
            else
            {
                Dispose();
            }
        }
    }

    // Structur.PAS:4509
    public void NewTurn(sbyte player)
    {
        if (player == CasterPlayer)
        {
            DaysRemaining = (DaysRemaining - 1).TruncateToSByte();
            if (DaysRemaining <= 0)
            {
                MeltIce();
            }
        }
    }
}
