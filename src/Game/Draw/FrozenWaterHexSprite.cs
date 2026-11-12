// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw;

// signed days remaining, caster player
public partial class FrozenWaterHexSprite : SingleHexSprite
{
    public FrozenWaterHexSprite(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131436;

    public sbyte DaysRemaining;
    public sbyte CasterPlayer;

    // Structur.PAS:4487
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwShortint(ref DaysRemaining, 30);
        stream.rwShortint(ref CasterPlayer, 31);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.SByte("DaysRemaining", ref DaysRemaining);
        persister.SByte("CasterPlayer", ref CasterPlayer);
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
    public override byte GetLevel() => 28;

    // Structur.PAS:4503
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        if (info.HexNumber == 0 && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)info.TerrainType))
        {
            Dispose();
        }
    }

    // Structur.PAS:4524
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        var hexY = HexY;
        HexGeometry.HexToPixel(HexX, hexY, ref X, ref Y);
        X = X - pixelXSurface + 16;
        Y = Y - pixelYSurface + 16;
        var hexX = HexX;
        var hexY2 = HexY;
        var hexY3 = HexY;
        AoWHSSet.Current.ImageSequences.ShowLooped(70, X, Y, AoWHSMap.Current.Frame, hexX + hexY2 + hexY3 * 2, lockedSurface);
    }

    // Structur.PAS:4494
    public void MeltIce()
    {
        if (((MapField)Owner).TerrainType == 6)
        {
            var hexY = HexY;
            var hexX = HexX;
            AoWHSMap.Current.ChangeTerrain(this, hexX, hexY, HexLevel, 0, 0);
        }
        else
        {
            if (((MapField)Owner).TerrainType == 13)
            {
                var hexY = HexY;
                var hexX = HexX;
                AoWHSMap.Current.ChangeTerrain(this, hexX, hexY, HexLevel, 0, 10);
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
            DaysRemaining = (sbyte)(DaysRemaining - 1);
            if (DaysRemaining <= 0)
            {
                MeltIce();
            }
        }
    }

    // Engine.pas:524

}
