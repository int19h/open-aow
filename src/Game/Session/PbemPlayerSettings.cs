#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Net;

namespace Aow.Game.Session;

// status, email game
public sealed class PbemPlayerSettings : EngineObject
{
    // Player.PAS:236
    public PbemPlayerSettings(CustomNode owner) : base(owner)
    {
        Status = PbemPlayerStatuses.None;
        EmailGame = new EmailGame(null) { Holder = CreationHolder() };
    }

    public PbemPlayerStatuses Status { get; set; }

    public EmailGame EmailGame { get; }

    // Player.PAS:243
    public override void Dispose()
    {
        EmailGame?.Dispose();
        base.Dispose();
    }

    // Player.PAS:249
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Status = persister.Set("Status", value: Status);
        persister.Object("EmailGame", objectValue: EmailGame);
    }

    // Player.PAS:256
    public void SetGame(EngineObject game) => EmailGame.Assign(game, engine: GetEngine());
}
