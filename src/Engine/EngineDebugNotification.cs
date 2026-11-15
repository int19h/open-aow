
namespace Aow.Engine;

public sealed class EngineDebugNotification
{
    public DebugInfo Info { get; set; }

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct DebugInfo
    {
        public DebugChannel Channel { get; set; }

        private string _text;

        public string Text { readonly get => _text ?? ""; set => _text = value; }
    }
}
