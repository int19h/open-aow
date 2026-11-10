// Translated from Crypt.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;
using static Aow.Game.Magic.Abilities.AoWEUnit;

namespace Aow.Game.World.Structures;

public partial class TCrypt : TItemExplorationSite
{
    public TCrypt(TECustomNode owner) : base(owner)
    { }

    // Crypt.pas:34
    public new const int ClassID = 131850;

}

public partial class TCryptResource : TItemExplorationSiteResource
{
    public TCryptResource(TECustomNode owner) : base(owner)
    { }

    // Crypt.pas:40
    public new const int ClassID = 131851;

    // Crypt.pas:43
    public override int ResourceUserClassID() => 131850;

    // Crypt.pas:37
    public override string ResourceEditName() => TranslateRStr(CryptRStr);

}

public static partial class CryptUnit
{
    // Crypt.pas:46
    public static void RegisterCrypt(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TCrypt>(), EClass.Of<TCryptResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
