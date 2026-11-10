#nullable enable

namespace Aow.Game.Layout;

// A data module whose layout is generated. GameLayouts loads each root through the session.
public interface ILayoutModule
{
    void InitializeComponent(LayoutSession session);
}
