// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Net;

using Aow.Rtl;

namespace Aow.Game.Session;

// status, email game
public partial class PbemPlayerSettings : EngineObject
{
    // Player.PAS:236
    public PbemPlayerSettings(ECustomNode owner) : base(owner)
    {
        Status = Set<PbemPlayerStatusElement>.Of();
        EmailGame = new EmailGame(null) { Holder = CreationHolder() };
    }

    public Set<PbemPlayerStatusElement> Status { get; set; }

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
    public void SetGame(EngineObject game)
    {
        EmailGame.Assign(game, GetEngine());
    }

    // intf_P1_03.inc:160

}
