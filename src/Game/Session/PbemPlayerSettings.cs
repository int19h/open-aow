// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// status, email game
public partial class PbemPlayerSettings : EngineObject
{
    // Player.PAS:236
    public PbemPlayerSettings(ECustomNode owner) : base(owner)
    {
        Status = Set<PbemPlayerStatusElement>.Of();
        EmailGame = new EmailGame(null);
    }

    public Set<PbemPlayerStatusElement> Status;

    public EmailGame EmailGame { get; }

    // Player.PAS:243
    public override void Dispose()
    {
        EmailGame?.Dispose();
        base.Dispose();
    }

    // Player.PAS:249
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        TransferSet(ref Status, 1, 20, stream.Bytes, 0);
        stream.Object(EmailGame, 21);
    }

    // Player.PAS:256
    public void SetGame(EngineObject game)
    {
        EmailGame.Assign(game, GameTables.GameEngine);
    }

    // intf_P1_03.inc:160

}
