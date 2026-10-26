// Generated from EList, Engine, FMemStr, PlugInM by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using Aow.Rtl;
using System.Runtime.CompilerServices;

namespace Aow.Units
{
    [InlineArray(10)]
    public struct FixedArray_Aow_Rtl_TList_10
    {
        private Aow.Rtl.TList element0;
    }

    [InlineArray(256)]
    public struct FixedArray_Aow_Units_Engine_TIntegerHashList_256
    {
        private Aow.Units.Engine.TIntegerHashList element0;
    }

    [InlineArray(256)]
    public struct FixedArray_Aow_Units_Engine_TStringHashList_256
    {
        private Aow.Units.Engine.TStringHashList element0;
    }

    [InlineArray(3)]
    public struct FixedArray_IntOrRef_3
    {
        private IntOrRef element0;
    }

    [InlineArray(4)]
    public struct FixedArray_byte_4
    {
        private byte element0;
        public static Aow.Units.FixedArray_byte_4 From(ReadOnlySpan<byte> values)
        {
            Aow.Units.FixedArray_byte_4 result = default;
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = values[i];
            }

            return result;
        }
    }

    [InlineArray(4)]
    public struct FixedArray_int_4
    {
        private int element0;
    }
}
