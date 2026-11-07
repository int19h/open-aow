using Aow.Rtl;

namespace Aow.Units.Engine;

public partial class TEObjectEditor
{
    // The popup hooks of the editor shell. No editor class overrides them yet: they join the
    // IEObjectEditorClass descriptor when the first one does.
    public static void UpdateMultiPopupMenu(TEObject obj, TEngine engine, ref TSet<TEditModeElement> handled, TPopupMenu menu, TEObjectList objects) { }

    public static void UpdatePopupMenu(TEObject obj, TEngine engine, ref TSet<TEditModeElement> handled, TPopupMenu menu) { }
}
