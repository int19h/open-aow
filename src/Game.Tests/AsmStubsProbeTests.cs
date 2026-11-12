using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Xunit.Sdk;

namespace Aow.Game.Tests;

// Recomputes the native asm-stubs recording by calling the product routines.
// Each buffer starts filled with the byte A5, and the fills begin at element 1.
// HPtoHXs stores four bytes at each shortint argument, which aliases Small[1] and Small[7].
// The zero-count triple fill is aimed 12 bytes before an unmapped page, so the span stops at 12 bytes.
// The negative bit write views a 64-byte block from offset 32, and index -1 stores into byte 31.
public sealed class AsmStubsProbeTests
{
    private readonly record struct Family(string Name, string[] Prefixes);

    private static readonly int[] s_counts = [0, 1, 2, 7, 8, 9];

    private static readonly int[] s_values = [0, -1, 0x12345678, int.MinValue, 1, int.MaxValue];

    private static readonly int[] s_indices = [-1, 0, 1, 7, 8, 31, 32, 63, 64, 129];

    private static readonly Family[] s_families =
    [
        new("engine fills", ["Engine.FillInteger", "Engine.FillWord", "Engine.FillLargeInteger", "Engine.FillQuadInteger"]),
        new("bit get and put", ["Engine.TBitList.Get", "Engine.TBitList.Put"]),
        new("bit load and save", ["Engine.TBitList.Load", "Engine.TBitList.Save"]),
        new("explored flags", ["AoWE.TAoWMapField."]),
        new("razed state", ["Mine.TMine.GetRazed", "Teleport.TTeleport.GetRazed"]),
        new("ability bits", ["AoWE.TCustomAbilityList.", "AoWE.TAbstractUnit.GetAbilitySet"]),
        new("graphics fills", ["GFXE."]),
        new("hex coordinates", ["HSEngine.HXtoV", "HSEngine.HXtoH", "HSEngine.HVtoHX", "HSEngine.HPtoHXs", "HSEngine.HPtoHX"]),
        new("hex walk", ["HSEngine.WalkToNextHN"]),
    ];

    private static readonly string[] s_lines = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Probes", "asm-stubs.txt"));

    // The recording names the Delphi class. A type missing from this table fails the test.
    private static readonly Dictionary<Type, string> s_delphiNames = new()
    {
        [typeof(ArgumentOutOfRangeException)] = "EListError",
        [typeof(AccessViolationException)] = "EAccessViolation",
    };

    private static string DelphiName(Exception error) =>
        s_delphiNames.TryGetValue(error.GetType(), out string? name)
            ? name
            : throw new XunitException("asm-stubs has no Delphi name for " + error.GetType().Name);

    [Fact]
    public void EngineFills() => Check(0, EngineFillLines());

    [Fact]
    public void BitListGetAndPut() => Check(1, BitListAccessLines());

    [Fact]
    public void BitListLoadAndSave() => Check(2, BitListStreamLines());

    [Fact]
    public void MapExplored() => Check(3, ExploredLines());

    [Fact]
    public void Razed() => Check(4, RazedLines());

    [Fact]
    public void AbilityBits() => Check(5, AbilityLines());

    [Fact]
    public void GraphicsFills() => Check(6, GraphicsLines());

    [Fact]
    public void HexCoordinates() => Check(7, HexLines());

    [Fact]
    public void HexWalk() => Check(8, WalkLines());

    [Fact]
    public void RecordingClaimsEveryFact()
    {
        int facts = 0;
        for (int i = 0; i < s_lines.Length; i++)
        {
            if (i == s_lines.Length - 1)
            {
                Assert.True(s_lines[i] == "end 28222", "asm-stubs.txt line " + (i + 1) + " expected <end 28222> actual <" + s_lines[i] + ">");
                continue;
            }

            int family = FamilyOf(s_lines[i]);
            Assert.True(family >= 0, "asm-stubs.txt line " + (i + 1) + " has no routine family <" + s_lines[i] + ">");
            facts++;
        }

        Assert.Equal(28222, facts);
    }

    private static void Check(int family, List<string> actual)
    {
        string name = s_families[family].Name;
        int index = 0;
        for (int line = 0; line < s_lines.Length; line++)
        {
            if (FamilyOf(s_lines[line]) != family)
            {
                continue;
            }

            if (index >= actual.Count)
            {
                Assert.Fail(name + " asm-stubs.txt line " + (line + 1) + " produced no line for <" + s_lines[line] + ">");
            }

            if (actual[index] != s_lines[line])
            {
                Assert.Fail(name + " asm-stubs.txt line " + (line + 1) + " expected <" + s_lines[line] + "> actual <" + actual[index] + ">");
            }

            index++;
        }

        if (index != actual.Count)
        {
            Assert.Fail(name + " extra line <" + actual[index] + ">");
        }
    }

    private static int FamilyOf(string line)
    {
        int space = line.IndexOf(' ');
        string label = space < 0 ? line : line[..space];
        int best = -1;
        int length = -1;
        for (int i = 0; i < s_families.Length; i++)
        {
            foreach (string prefix in s_families[i].Prefixes)
            {
                if (label.StartsWith(prefix, StringComparison.Ordinal) && prefix.Length > length)
                {
                    best = i;
                    length = prefix.Length;
                }
            }
        }

        return best;
    }

    private static List<string> EngineFillLines()
    {
        var lines = new List<string>();
        var ints = new int[12];
        var words = new ushort[12];
        var doubles = new DoubleItemList.DoubleItem[12];
        var quads = new QuadItemList.QuadInteger[12];
        QuadItemList.QuadInteger quadValue = default;
        for (int i = 0; i < s_counts.Length; i++)
        {
            int count = s_counts[i];
            for (int j = 0; j < s_values.Length; j++)
            {
                int value = s_values[j];
                string tag = "_count" + Decrement(count) + "_value" + Decrement(j);
                FillA5(ints);
                Int32List.FillInteger(ints.AsSpan(1), count, value);
                FillA5(words);
                UInt16List.FillWord(words.AsSpan(1), count, (ushort)value);
                var large = new DoubleItemList.DoubleItem { First = value, Second = ~value };
                FillA5(doubles);
                FillLarge(doubles, 1, count, large);
                quadValue = new QuadItemList.QuadInteger { First = value, Second = ~value, Third = value ^ 0x55AA55AA, Fourth = -17 };
                FillA5(quads);
                QuadItemList.FillQuadInteger(quads.AsSpan(1), count, quadValue);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "Engine.FillInteger" + tag + "_slot" + Decrement(k), ints[k]);
                    AddHex(lines, "Engine.FillWord" + tag + "_slot" + Decrement(k), words[k]);
                    AddHex(lines, "Engine.FillLargeInteger" + tag + "_slot" + Decrement(k) + "_A", doubles[k].First);
                    AddHex(lines, "Engine.FillLargeInteger" + tag + "_slot" + Decrement(k) + "_B", doubles[k].Second);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Decrement(k) + "_A", quads[k].First);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Decrement(k) + "_B", quads[k].Second);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Decrement(k) + "_C", quads[k].Third);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Decrement(k) + "_D", quads[k].Fourth);
                }
            }
        }

        doubles[1].First = 0x12345678;
        doubles[1].Second = -17;
        FillLarge(doubles, 0, 4, doubles[1]);
        quads[1] = quadValue;
        QuadItemList.FillQuadInteger(quads, 4, quads[1]);
        for (int k = 0; k < 4; k++)
        {
            AddHex(lines, "Engine.FillLargeInteger_overlap_A_" + Decrement(k), doubles[k].First);
            AddHex(lines, "Engine.FillLargeInteger_overlap_B_" + Decrement(k), doubles[k].Second);
            AddHex(lines, "Engine.FillQuadInteger_overlap_A_" + Decrement(k), quads[k].First);
            AddHex(lines, "Engine.FillQuadInteger_overlap_D_" + Decrement(k), quads[k].Fourth);
        }

        return lines;
    }

    private static List<string> BitListAccessLines()
    {
        var lines = new List<string>();
        var bits = new BitList();
        for (int j = 0; j <= 1; j++)
        {
            bits.Clear();
            bits.Default = j != 0;
            for (int i = 0; i < s_indices.Length; i++)
            {
                AddNumber(lines, "Engine.TBitList.Get_default" + Decrement(j) + "_index" + Decrement(s_indices[i]), bits[s_indices[i]] ? 1 : 0);
            }

            for (int i = 1; i < s_indices.Length; i++)
            {
                bits[s_indices[i]] = (i & 1) != 0;
                AddNumber(lines, "Engine.TBitList.Put_count_" + Decrement(j) + "_" + Decrement(i), bits.Count);
                for (int k = 0; k <= bits.Count; k++)
                {
                    AddNumber(lines, "Engine.TBitList.Get_after_" + Decrement(j) + "_" + Decrement(i) + "_" + Decrement(k), bits[k] ? 1 : 0);
                }
            }
        }

        bits.Count = 32;
        var guard = new byte[64];
        bits.List = guard.AsMemory(32);
        try
        {
            try
            {
                bits[-1] = true;
                AddText(lines, "Engine.TBitList.Put_negative", "returned");
            }
            catch (Exception e)
            {
                AddText(lines, "Engine.TBitList.Put_negative", DelphiName(e));
            }

            AddHex(lines, "Engine.TBitList.Put_negative_guard", guard[31]);
            AddNumber(lines, "Engine.TBitList.Put_negative_count", bits.Count);
            bits[-1] = false;
            AddHex(lines, "Engine.TBitList.Put_negative_clear_guard", guard[31]);
            try
            {
                bits[-2] = true;
                AddText(lines, "Engine.TBitList.Put_negative_two", "returned");
            }
            catch (Exception e)
            {
                AddText(lines, "Engine.TBitList.Put_negative_two", DelphiName(e));
            }
        }
        finally
        {
            bits.List = Memory<byte>.Empty;
        }

        return lines;
    }

    private static List<string> BitListStreamLines()
    {
        var lines = new List<string>();
        int[] sizes = [1, 3, 13, 31, 32, 33];
        foreach (int count in sizes)
        {
            var stream = new MemoryStream();
            var list = new BitList();
            bool defaultValue = false;
            var data = new byte[64];
            data.AsSpan().Fill(0xA5);
            stream.Write(in defaultValue, 1);
            stream.Write(in count, 4);
            stream.Write(data);
            stream.Position = 0;
            list.LoadFromStream(stream);
            string tag = Decrement(count);
            AddNumber(lines, "Engine.TBitList.Load_count_" + tag, list.Count);
            AddNumber(lines, "Engine.TBitList.Load_capacity_" + tag, list.Capacity);
            AddNumber(lines, "Engine.TBitList.Load_position_" + tag, (int)stream.Position);
            stream.Clear();
            list.SaveToStream(stream);
            AddNumber(lines, "Engine.TBitList.Save_size_" + tag, (int)stream.Length);
            stream.Position = 1;
            int saved = 0;
            stream.Read(ref saved, 4);
            AddNumber(lines, "Engine.TBitList.Save_count_" + tag, saved);
        }

        return lines;
    }

    private static List<string> ExploredLines()
    {
        var lines = new List<string>();
        var field = new AoWMapField(null!);
        for (int j = 0; j < s_values.Length; j++)
        {
            field.FieldFlags = s_values[j];
            for (int i = -2; i <= 34; i++)
            {
                string label = Decrement(j) + "_" + Decrement(i);
                AddNumber(lines, "AoWE.TAoWMapField.GetExplored_" + label, field.GetExplored(i) ? 1 : 0);
                field.SetExplored(i, false);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_clear_" + label, field.FieldFlags);
                field.SetExplored(i, true);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_set_" + label, field.FieldFlags);
            }
        }

        for (int i = 0; i < s_values.Length; i++)
        {
            for (int j = 0; j < s_values.Length; j++)
            {
                field.FieldFlags = 0;
                field.SetExploredRaw(s_values[i], s_values[j]);
                string tag = Decrement(i) + "_" + Decrement(j);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_raw_" + tag, field.FieldFlags);
                AddNumber(lines, "AoWE.TAoWMapField.GetExplored_extreme_" + tag, field.GetExplored(s_values[i]) ? 1 : 0);
            }
        }

        return lines;
    }

    private static List<string> RazedLines()
    {
        var lines = new List<string>();
        var mine = new Mine(null!);
        var teleport = new Teleport(null!);
        for (int i = 0; i <= 255; i++)
        {
            mine.State = (byte)i;
            teleport.State = Set<Teleport.StateElement>.FromMask((ulong)i);
            AddNumber(lines, "Mine.TMine.GetRazed_" + Decrement(i), mine.GetRazed() ? 1 : 0);
            AddNumber(lines, "Teleport.TTeleport.GetRazed_" + Decrement(i), teleport.GetRazed() ? 1 : 0);
        }

        return lines;
    }

    private static List<string> AbilityLines()
    {
        var lines = new List<string>();
        var abilities = new CustomAbilityList(null!);
        var troop = new Troop(null!);
        abilities.SetAbilityCount(0);
        for (int i = 1; i < s_indices.Length; i++)
        {
            abilities.SetAbilitySet(s_indices[i], (byte)i);
            AddNumber(lines, "AoWE.TCustomAbilityList.SetAbSet_count_" + Decrement(i), abilities.AbilityCount);
            for (int k = -1; k <= abilities.AbilityCount; k++)
            {
                AddNumber(lines, "AoWE.TCustomAbilityList.GetAbSet_" + Decrement(i) + "_" + Decrement(k), abilities.GetAbilitySetAt(k) ? 1 : 0);
            }

            abilities.SetAbilitySet(s_indices[i], 0);
            AddNumber(lines, "AoWE.TCustomAbilityList.SetAbSet_clear_" + Decrement(i), abilities.GetAbilitySetAt(s_indices[i]) ? 1 : 0);
        }

        for (int j = 0; j <= 1; j++)
        {
            for (int i = 0; i <= abilities.AbilityCount - 1; i++)
            {
                abilities.SetAbilitySet(i, (byte)((i + j) & 1));
            }

            troop.AbilityCount = abilities.AbilityCount;
            troop.AbilityBits = abilities.AbilityBits;
            for (int i = -1; i <= troop.AbilityCount + 1; i++)
            {
                AddNumber(lines, "AoWE.TAbstractUnit.GetAbilitySet_" + Decrement(j) + "_" + Decrement(i), troop.GetAbilitySet(i) ? 1 : 0);
            }
        }

        return lines;
    }

    private static List<string> GraphicsLines()
    {
        var lines = new List<string>();
        var ints = new int[12];
        var words = new ushort[12];
        var quads = new QuadItemList.QuadInteger[12];
        for (int i = 0; i < s_counts.Length; i++)
        {
            for (int j = 0; j < s_values.Length; j++)
            {
                string tag = Decrement(i) + "_" + Decrement(j);
                FillA5(ints);
                MoveControlSettings.FillInteger(MemoryMarshal.AsBytes(ints.AsSpan())[4..], s_counts[i], s_values[j]);
                FillA5(words);
                PixelFormats.FillWord(MemoryMarshal.AsBytes(words.AsSpan())[2..], s_counts[i], s_values[j]);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "GFXE.FillInt_" + tag + "_" + Decrement(k), ints[k]);
                    AddHex(lines, "GFXE.FillWord_" + tag + "_" + Decrement(k), words[k]);
                }

                if (s_counts[i] > 0)
                {
                    FillA5(quads);
                    PixelFormats.FillTripleByte(MemoryMarshal.AsBytes(quads.AsSpan()), s_counts[i], s_values[j]);
                    for (int k = 0; k < 3; k++)
                    {
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Decrement(k) + "_A", quads[k].First);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Decrement(k) + "_B", quads[k].Second);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Decrement(k) + "_C", quads[k].Third);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Decrement(k) + "_D", quads[k].Fourth);
                    }
                }
            }
        }

        try
        {
            PixelFormats.FillTripleByte(new byte[12], 0, 0x12345678);
        }
        catch (Exception e)
        {
            AddText(lines, "GFXE.FillTrippleByte_zero", DelphiName(e));
        }

        return lines;
    }

    private static List<string> HexLines()
    {
        var lines = new List<string>();
        var small = new byte[12];
        for (int i = -65; i <= 65; i++)
        {
            for (int j = -3; j <= 3; j++)
            {
                string tag = Decrement(i) + "_" + Decrement(j);
                AddNumber(lines, "HSEngine.HXtoV_" + tag, HexGeometry.HexToV(i, j));
                AddNumber(lines, "HSEngine.HXtoH_" + tag, HexGeometry.HexToH(i, j));
                int x = 0;
                int y = 0;
                HexGeometry.HVtoHex(i, j, ref x, ref y);
                AddNumber(lines, "HSEngine.HVtoHX_X_" + tag, x);
                AddNumber(lines, "HSEngine.HVtoHX_Y_" + tag, y);
                HexGeometry.PixelToHex(i, j, ref x, ref y);
                AddNumber(lines, "HSEngine.HPtoHX_X_" + tag, x);
                AddNumber(lines, "HSEngine.HPtoHX_Y_" + tag, y);
                small.AsSpan().Fill(0xA5);
                ref sbyte sx = ref Unsafe.As<byte, sbyte>(ref small[1]);
                ref sbyte sy = ref Unsafe.As<byte, sbyte>(ref small[7]);
                HexGeometry.PixelToHexes(i, j, ref sx, ref sy);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "HSEngine.HPtoHXs_" + tag + "_" + Decrement(k), small[k]);
                }
            }
        }

        for (int i = 0; i < s_values.Length; i++)
        {
            AddNumber(lines, "HSEngine.HXtoV_extreme_" + Decrement(i), HexGeometry.HexToV(s_values[i], -1));
            AddNumber(lines, "HSEngine.HXtoH_extreme_" + Decrement(i), HexGeometry.HexToH(s_values[i], 0));
            int x = 0;
            int y = 0;
            HexGeometry.PixelToHex(s_values[i], s_values[i], ref x, ref y);
            AddNumber(lines, "HSEngine.HPtoHX_extreme_X_" + Decrement(i), x);
            AddNumber(lines, "HSEngine.HPtoHX_extreme_Y_" + Decrement(i), y);
        }

        return lines;
    }

    private static List<string> WalkLines()
    {
        var lines = new List<string>();
        for (int a = -1; a <= 1; a++)
        {
            for (int b = -1; b <= 1; b++)
            {
                var walk = new HexNumberWalkData();
                walk.CenterX = a;
                walk.CenterY = b;
                for (int i = 0; i < 80; i++)
                {
                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                    string tag = Decrement(a) + "_" + Decrement(b) + "_" + Decrement(i);
                    AddNumber(lines, "HSEngine.WalkToNextHN_X_" + tag, walk.X);
                    AddNumber(lines, "HSEngine.WalkToNextHN_Y_" + tag, walk.Y);
                    AddNumber(lines, "HSEngine.WalkToNextHN_side_" + tag, walk.Side);
                    AddNumber(lines, "HSEngine.WalkToNextHN_step_" + tag, walk.Step);
                    AddNumber(lines, "HSEngine.WalkToNextHN_radius_" + tag, walk.Radius);
                    AddNumber(lines, "HSEngine.WalkToNextHN_HN_" + tag, walk.HexNumber);
                }
            }
        }

        return lines;
    }

    private static void FillLarge(DoubleItemList.DoubleItem[] items, int start, int count, DoubleItemList.DoubleItem value)
    {
        LargeInteger bits = Unsafe.As<DoubleItemList.DoubleItem, LargeInteger>(ref value);
        EngineServices.FillLargeInteger(MemoryMarshal.Cast<DoubleItemList.DoubleItem, LargeInteger>(items.AsSpan(start)), count, bits);
    }

    private static void FillA5<T>(Span<T> items) where T : unmanaged => MemoryMarshal.AsBytes(items).Fill(0xA5);

    private static void AddNumber(List<string> lines, string label, int value) => lines.Add(label + " " + Decrement(value));

    private static void AddHex(List<string> lines, string label, int value) => lines.Add(label + " " + unchecked((uint)value).ToString("X8"));

    private static void AddText(List<string> lines, string label, string value) => lines.Add(label + " " + value);

    private static string Decrement(int value) => value.ToString(CultureInfo.InvariantCulture);
}
