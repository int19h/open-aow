// Translated from RaceBox.pas.
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

public partial class RaceBox : ComboBox
{
    public NotifyEvent<object> FOnGenerateItems;
    public sbyte FRace;
    public bool FAllowRandom;
    public bool FAllowNone;
    public bool FSelectableOnly;
    public RaceList FRaces;

    // RaceBox.pas:98
    public override void Loaded()
    {
        base.Loaded();
        GenerateItems();
    }

    // RaceBox.pas:52

    public RaceBox(global::Aow.Rtl.Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // RaceBox.pas:58
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // RaceBox.pas:77
    public void SetRace(sbyte Value)
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // RaceBox.pas:85
    protected internal override void Change()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // RaceBox.pas:92
    public void SetRaces(RaceList Value)
    {
        FRaces = Value;
        if (!ComponentState.Contains(global::Aow.Rtl.ComponentState.Designing) && !ComponentState.Contains(global::Aow.Rtl.ComponentState.Loading))
        {
            GenerateItems();
        }
    }

    [Published]
    public bool OnlyEnabledRaces { get => FSelectableOnly; set => FSelectableOnly = value; }

    [Published]
    public bool AllowRandomRace { get => FAllowRandom; set => FAllowRandom = value; }

    [Published]
    public bool AllowNoRace { get => FAllowNone; set => FAllowNone = value; }

    [Published]
    public NotifyEvent<object> OnGenerateItems { get => FOnGenerateItems; set => FOnGenerateItems = value; }

    public struct ObjectFields
    {
        public FixedArray_object_512 Fields;
    }

    public struct NameView
    {
        public FixedArray_byte_36 Reserved;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }
    }

    [StructLayout(LayoutKind.Explicit, Size = 21)]
    public struct RaceView
    {
        [FieldOffset(0)]
        public FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public bool Selectable;
    }
}
