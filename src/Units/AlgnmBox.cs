// Translated from AlgnmBox.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AlgnmBox;

public partial class TAlignmentBox : TComboBox
{
    public TAlignment FAlignment;
    public TSet<TAlignment> FAlignments;
    public bool FSetMode;

    // AlgnmBox.pas:133
    public override void Loaded()
    {
        base.Loaded();
        GenerateItems();
    }

    // AlgnmBox.pas:54

    public TAlignmentBox(TComponent owner) : base(owner)
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
    public void SetAlignments(TSet<TAlignment> Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AlgnmBox.pas:110
    public void SetAlignment(TAlignment Value)
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

public static partial class AlgnmBoxUnit
{
    public static string NoneRStr = "None";

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
