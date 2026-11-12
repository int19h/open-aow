
namespace Aow.Engine;

public sealed class EngineDebugNotification
{
    public DebugInfo Info;

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct DebugInfo
    {
        public int Id;
        private string _text;

        public string Text { readonly get => _text ?? ""; set => _text = value; }
    }
}
