using Aow.Rtl;

namespace Aow.Engine;

public sealed class EngineDebugNotification
{
    public EngineDebugInfo Info;

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct EngineDebugInfo
    {
        public int ID;
        private string text;

        public string Text { readonly get => text ?? ""; set => text = value; }
    }
}
