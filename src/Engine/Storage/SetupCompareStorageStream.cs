// Translated from Engine.pas.
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Storage;

// a read stream that announces a comparison to come (smSetupCompare)
public partial class SetupCompareStorageStream : EReadStorageStream
{
    public SetupCompareStorageStream(Stream AStream, GameEngine AEngine, bool APartial) : base(AStream, AEngine, APartial)
    { }

    // Engine.pas:10911
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.SetupCompare);
}
