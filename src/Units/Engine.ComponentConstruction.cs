using Aow.Rtl;

namespace Aow.Units.Engine;

public partial class TEComponent
{
    public TEComponent(TComponent owner) : base(owner) { }

    private protected TEComponent(ConstructionContext context, TComponent owner) : base(context, owner) { }
}
