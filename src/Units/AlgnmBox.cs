// Translated from AlgnmBox.pas.
using Aow.Game.Engine;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

public partial class AlignmentBox : ComboBox
{
    public Alignment FAlignment;
    public Set<Alignment> FAlignments;
    public bool FSetMode;

    // AlgnmBox.pas:133
    public override void Loaded()
    {
        base.Loaded();
        GenerateItems();
    }

    // AlgnmBox.pas:54

    public AlignmentBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AlgnmBox.pas:60
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AlgnmBox.pas:100
    public void SetAlignments(Set<Alignment> Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AlgnmBox.pas:110
    public void SetAlignment(Alignment Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AlgnmBox.pas:120
    protected internal override void Change()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public bool UseSets { get => FSetMode; set => FSetMode = value; }
}
