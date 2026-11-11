
namespace Aow.Engine;

public sealed class EngineDebugNotification
{
    public DebugInfo Info;

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct DebugInfo
    {
        public int ID;
        private string text;

        public string Text { readonly get => text ?? ""; set => text = value; }
    }
}
