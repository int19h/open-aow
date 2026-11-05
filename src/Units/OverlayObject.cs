// Translated from OverlayObject.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.ILTer;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.OverlayObject
{
    public partial class TOverlayObject : TFixedILTerrainMO
    {
        public TOverlayObject(TECustomNode owner) : base(owner)
        {}

        // OverlayObject.pas:36
        public new const int ClassID = 131228;

    }

    // FOverlayType
    public partial class TOverlayObjectResource : TFixedILTerrainMOR
    {
        public TOverlayObjectResource(TECustomNode owner) : base(owner)
        {}

        // OverlayObject.pas:42
        public new const int ClassID = 131229;

        public int FOverlayType;
        // OverlayObject.pas:48
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FOverlayType, 40);
        }

        // OverlayObject.pas:45
        public override int ResourceUserClassID() => 131228;

        // OverlayObject.pas:39
        public override string ResourceEditName() => TranslateRStr(OverlayObjectRStr);

    }

    public static partial class OverlayObject
    {
        // OverlayObject.pas:54
        public static void RegisterOverlayObject(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TOverlayObject>(), EClass.Of<TOverlayObjectResource>()]);
        }

        // OverlayObject.pas:59
        public static void UnRegisterOverlayObject(TEngine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<TOverlayObject>(), EClass.Of<TOverlayObjectResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
