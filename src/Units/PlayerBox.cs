// Translated from PlayerBox.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;

namespace Aow.Units.PlayerBox;

public partial class TPlayerComboBox : TComboBox
{
    public sbyte FPlayerIndex;
    public bool FAllowNone;
    public bool FAllowIndependent;
    public bool FAllowRandom;
    // PlayerBox.pas:87
    public override void Loaded()
    {
        base.Loaded();
        if (!ComponentState.Contains(TComponentState.csDesigning))
        {
            GenerateItems();
        }
    }

    // PlayerBox.pas:52
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // PlayerBox.pas:72
    public void SetPlayerIndex(sbyte Value)
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // PlayerBox.pas:80
    protected internal override void Change()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public bool AllowIndependents { get => FAllowIndependent; set => FAllowIndependent = value; }

    [Published]
    public bool AllowNoPlayer { get => FAllowNone; set => FAllowNone = value; }

    [Published]
    public bool AllowRandom { get => FAllowRandom; set => FAllowRandom = value; }
}

public struct TObjectFields
{
    public FixedArray_object_512 Fields;
}

public struct TNameView
{
    public FixedArray_byte_36 Reserved;
    private string name;
    public string Name { readonly get => name ?? ""; set => name = value; }
}

[StructLayout(LayoutKind.Explicit, Size = 21)]
public struct TRaceView
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public bool Selectable;
}

public static partial class PlayerBox
{
    // PlayerBox.pas:93
    public static void Register()
    {
        RegisterComponents("AoW", [typeof(TPlayerComboBox)]);
    }

    public static string PlayerRStr = "Player";

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
