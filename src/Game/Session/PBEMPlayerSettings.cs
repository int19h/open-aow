// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// status, email game
public partial class PBEMPlayerSettings : EObject
{
    // Player.PAS:236
    public PBEMPlayerSettings(ECustomNode owner) : base(owner)
    {
        FStatus = Set<PBEMPlayerStatusElement>.Of();
        FEmailGame = new EmailGame(null);
    }

    public Set<PBEMPlayerStatusElement> FStatus;
    public EmailGame FEmailGame;

    // Player.PAS:243
    public override void Dispose()
    {
        FEmailGame?.Dispose();
        base.Dispose();
    }

    // Player.PAS:249
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FStatus, 1, 20, Stream.rwData, 0);
        Stream.rwEObject(FEmailGame, 21);
    }

    // Player.PAS:256
    public void SetGame(EObject Game)
    {
        FEmailGame.Assign(Game, GameTables.AoWEngine);
    }

    // intf_P1_03.inc:160

}
