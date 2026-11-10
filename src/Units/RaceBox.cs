// Translated from RaceBox.pas.
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids
{

    public partial class TRaceBox : TComboBox
    {
        public TNotifyEvent<object> FOnGenerateItems;
        public sbyte FRace;
        public bool FAllowRandom;
        public bool FAllowNone;
        public bool FSelectableOnly;
        public TRaceList FRaces;

        // RaceBox.pas:98
        public override void Loaded()
        {
            base.Loaded();
            GenerateItems();
        }

        // RaceBox.pas:52

        public TRaceBox(TComponent owner) : base(owner)
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
        public void SetRaces(TRaceList Value)
        {
            FRaces = Value;
            if (!ComponentState.Contains(TComponentState.csDesigning) && !ComponentState.Contains(TComponentState.csLoading))
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
        public TNotifyEvent<object> OnGenerateItems { get => FOnGenerateItems; set => FOnGenerateItems = value; }
    }
}

namespace Aow.Game.Ui.Grids.RaceBox
{

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
}

namespace Aow.Game.Ui.Grids
{

    public static partial class RaceBoxUnit
    {
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
