using Aow.Rtl;

namespace Aow.Units.Engine;

public partial class TEObjectEditor
{
    public new partial class Meta
    {
        public virtual void UpdateMultiPopupMenu(TEObject obj, TEngine engine, ref TSet<TEditModeElement> handled, TPopupMenu menu, TEObjectList objects) { }
        public virtual void UpdatePopupMenu(TEObject obj, TEngine engine, ref TSet<TEditModeElement> handled, TPopupMenu menu) { }
    }
}
