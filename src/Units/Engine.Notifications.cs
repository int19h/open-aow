using Aow.Rtl;

namespace Aow.Game.Engine;

[LegacyObject]
public sealed class EngineDebugNotification
{
    public EngineDebugInfo Info;

    public EngineDebugNotification()
    {
        CaptureAddress.Assign(this);
    }

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct EngineDebugInfo
    {
        public int ID;
        private string text;

        public string Text { readonly get => text ?? ""; set => text = value; }
    }
}
