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

    private static readonly int[] Counts = [0, 1, 2, 7, 8, 9];

    private static readonly int[] Values = [0, -1, 0x12345678, int.MinValue, 1, int.MaxValue];

    private static readonly int[] Indices = [-1, 0, 1, 7, 8, 31, 32, 63, 64, 129];

    private static readonly Family[] Families =
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

    private static readonly string[] Lines = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Probes", "asm-stubs.txt"));

    // The recording names the Delphi class. A type missing from this table fails the test.
    private static readonly Dictionary<Type, string> DelphiNames = new()
    {
        [typeof(ArgumentOutOfRangeException)] = "EListError",
        [typeof(AccessViolationException)] = "EAccessViolation",
    };

    private static string DelphiName(Exception error) =>
        DelphiNames.TryGetValue(error.GetType(), out string? name)
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
        for (int i = 0; i < Lines.Length; i++)
        {
            if (i == Lines.Length - 1)
            {
                Assert.True(Lines[i] == "end 28222", "asm-stubs.txt line " + (i + 1) + " expected <end 28222> actual <" + Lines[i] + ">");
                continue;
            }

            int family = FamilyOf(Lines[i]);
            Assert.True(family >= 0, "asm-stubs.txt line " + (i + 1) + " has no routine family <" + Lines[i] + ">");
            facts++;
        }

        Assert.Equal(28222, facts);
    }

    private static void Check(int family, List<string> actual)
    {
        string name = Families[family].Name;
        int index = 0;
        for (int line = 0; line < Lines.Length; line++)
        {
            if (FamilyOf(Lines[line]) != family)
            {
                continue;
            }

            if (index >= actual.Count)
            {
                Assert.Fail(name + " asm-stubs.txt line " + (line + 1) + " produced no line for <" + Lines[line] + ">");
            }

            if (actual[index] != Lines[line])
            {
                Assert.Fail(name + " asm-stubs.txt line " + (line + 1) + " expected <" + Lines[line] + "> actual <" + actual[index] + ">");
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
        for (int i = 0; i < Families.Length; i++)
        {
            foreach (string prefix in Families[i].Prefixes)
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
        for (int i = 0; i < Counts.Length; i++)
        {
            int count = Counts[i];
            for (int j = 0; j < Values.Length; j++)
            {
                int value = Values[j];
                string tag = "_count" + Dec(count) + "_value" + Dec(j);
                FillA5(ints);
                IntegerList.FillInteger(ints.AsSpan(1), count, value);
                FillA5(words);
                WordList.FillWord(words.AsSpan(1), count, (ushort)value);
                var large = new DoubleItemList.DoubleItem { A = value, B = ~value };
                FillA5(doubles);
                FillLarge(doubles, 1, count, large);
                quadValue = new QuadItemList.QuadInteger { A = value, B = ~value, C = value ^ 0x55AA55AA, D = -17 };
                FillA5(quads);
                QuadItemList.FillQuadInteger(quads.AsSpan(1), count, quadValue);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "Engine.FillInteger" + tag + "_slot" + Dec(k), ints[k]);
                    AddHex(lines, "Engine.FillWord" + tag + "_slot" + Dec(k), words[k]);
                    AddHex(lines, "Engine.FillLargeInteger" + tag + "_slot" + Dec(k) + "_A", doubles[k].A);
                    AddHex(lines, "Engine.FillLargeInteger" + tag + "_slot" + Dec(k) + "_B", doubles[k].B);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Dec(k) + "_A", quads[k].A);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Dec(k) + "_B", quads[k].B);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Dec(k) + "_C", quads[k].C);
                    AddHex(lines, "Engine.FillQuadInteger" + tag + "_slot" + Dec(k) + "_D", quads[k].D);
                }
            }
        }

        doubles[1].A = 0x12345678;
        doubles[1].B = -17;
        FillLarge(doubles, 0, 4, doubles[1]);
        quads[1] = quadValue;
        QuadItemList.FillQuadInteger(quads, 4, quads[1]);
        for (int k = 0; k < 4; k++)
        {
            AddHex(lines, "Engine.FillLargeInteger_overlap_A_" + Dec(k), doubles[k].A);
            AddHex(lines, "Engine.FillLargeInteger_overlap_B_" + Dec(k), doubles[k].B);
            AddHex(lines, "Engine.FillQuadInteger_overlap_A_" + Dec(k), quads[k].A);
            AddHex(lines, "Engine.FillQuadInteger_overlap_D_" + Dec(k), quads[k].D);
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
            for (int i = 0; i < Indices.Length; i++)
            {
                AddNum(lines, "Engine.TBitList.Get_default" + Dec(j) + "_index" + Dec(Indices[i]), bits[Indices[i]] ? 1 : 0);
            }

            for (int i = 1; i < Indices.Length; i++)
            {
                bits[Indices[i]] = (i & 1) != 0;
                AddNum(lines, "Engine.TBitList.Put_count_" + Dec(j) + "_" + Dec(i), bits.Count);
                for (int k = 0; k <= bits.Count; k++)
                {
                    AddNum(lines, "Engine.TBitList.Get_after_" + Dec(j) + "_" + Dec(i) + "_" + Dec(k), bits[k] ? 1 : 0);
                }
            }
        }

        bits.Count = 32;
        var guard = new byte[64];
        bits.FList = guard.AsMemory(32);
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
            AddNum(lines, "Engine.TBitList.Put_negative_count", bits.Count);
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
            bits.FList = Memory<byte>.Empty;
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
            string tag = Dec(count);
            AddNum(lines, "Engine.TBitList.Load_count_" + tag, list.Count);
            AddNum(lines, "Engine.TBitList.Load_capacity_" + tag, list.Capacity);
            AddNum(lines, "Engine.TBitList.Load_position_" + tag, (int)stream.Position);
            stream.Clear();
            list.SaveToStream(stream);
            AddNum(lines, "Engine.TBitList.Save_size_" + tag, (int)stream.Length);
            stream.Position = 1;
            int saved = 0;
            stream.Read(ref saved, 4);
            AddNum(lines, "Engine.TBitList.Save_count_" + tag, saved);
        }

        return lines;
    }

    private static List<string> ExploredLines()
    {
        var lines = new List<string>();
        var field = new AoWMapField(null!);
        for (int j = 0; j < Values.Length; j++)
        {
            field.FFieldFlags = Values[j];
            for (int i = -2; i <= 34; i++)
            {
                string label = Dec(j) + "_" + Dec(i);
                AddNum(lines, "AoWE.TAoWMapField.GetExplored_" + label, field.GetExplored(i) ? 1 : 0);
                field.SetExplored(i, false);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_clear_" + label, field.FFieldFlags);
                field.SetExplored(i, true);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_set_" + label, field.FFieldFlags);
            }
        }

        for (int i = 0; i < Values.Length; i++)
        {
            for (int j = 0; j < Values.Length; j++)
            {
                field.FFieldFlags = 0;
                field.SetExploredRaw(Values[i], Values[j]);
                string tag = Dec(i) + "_" + Dec(j);
                AddHex(lines, "AoWE.TAoWMapField.SetExplored_raw_" + tag, field.FFieldFlags);
                AddNum(lines, "AoWE.TAoWMapField.GetExplored_extreme_" + tag, field.GetExplored(Values[i]) ? 1 : 0);
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
            mine.FState = (byte)i;
            teleport.FState = Set<Teleport.StateElement>.FromMask((ulong)i);
            AddNum(lines, "Mine.TMine.GetRazed_" + Dec(i), mine.GetRazed() ? 1 : 0);
            AddNum(lines, "Teleport.TTeleport.GetRazed_" + Dec(i), teleport.GetRazed() ? 1 : 0);
        }

        return lines;
    }

    private static List<string> AbilityLines()
    {
        var lines = new List<string>();
        var abilities = new CustomAbilityList(null!);
        var unit = new AbstractUnit(null!);
        abilities.SetAbCount(0);
        for (int i = 1; i < Indices.Length; i++)
        {
            abilities.SetAbSet(Indices[i], (byte)i);
            AddNum(lines, "AoWE.TCustomAbilityList.SetAbSet_count_" + Dec(i), abilities.FAbilityCount);
            for (int k = -1; k <= abilities.FAbilityCount; k++)
            {
                AddNum(lines, "AoWE.TCustomAbilityList.GetAbSet_" + Dec(i) + "_" + Dec(k), abilities.GetAbSet(k) ? 1 : 0);
            }

            abilities.SetAbSet(Indices[i], 0);
            AddNum(lines, "AoWE.TCustomAbilityList.SetAbSet_clear_" + Dec(i), abilities.GetAbSet(Indices[i]) ? 1 : 0);
        }

        for (int j = 0; j <= 1; j++)
        {
            for (int i = 0; i <= abilities.FAbilityCount - 1; i++)
            {
                abilities.SetAbSet(i, (byte)((i + j) & 1));
            }

            unit.FAbilityCount = abilities.FAbilityCount;
            unit.FAbilityBits = abilities.FAbilityBits;
            for (int i = -1; i <= unit.FAbilityCount + 1; i++)
            {
                AddNum(lines, "AoWE.TAbstractUnit.GetAbilitySet_" + Dec(j) + "_" + Dec(i), unit.GetAbilitySet(i) ? 1 : 0);
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
        for (int i = 0; i < Counts.Length; i++)
        {
            for (int j = 0; j < Values.Length; j++)
            {
                string tag = Dec(i) + "_" + Dec(j);
                FillA5(ints);
                MoveControlSettings.FillInt(MemoryMarshal.AsBytes(ints.AsSpan())[4..], Counts[i], Values[j]);
                FillA5(words);
                PixelFormats.FillWord(MemoryMarshal.AsBytes(words.AsSpan())[2..], Counts[i], Values[j]);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "GFXE.FillInt_" + tag + "_" + Dec(k), ints[k]);
                    AddHex(lines, "GFXE.FillWord_" + tag + "_" + Dec(k), words[k]);
                }

                if (Counts[i] > 0)
                {
                    FillA5(quads);
                    PixelFormats.FillTrippleByte(MemoryMarshal.AsBytes(quads.AsSpan()), Counts[i], Values[j]);
                    for (int k = 0; k < 3; k++)
                    {
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Dec(k) + "_A", quads[k].A);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Dec(k) + "_B", quads[k].B);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Dec(k) + "_C", quads[k].C);
                        AddHex(lines, "GFXE.FillTrippleByte_" + tag + "_" + Dec(k) + "_D", quads[k].D);
                    }
                }
            }
        }

        try
        {
            PixelFormats.FillTrippleByte(new byte[12], 0, 0x12345678);
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
                string tag = Dec(i) + "_" + Dec(j);
                AddNum(lines, "HSEngine.HXtoV_" + tag, HexGeometry.HXtoV(i, j));
                AddNum(lines, "HSEngine.HXtoH_" + tag, HexGeometry.HXtoH(i, j));
                int x = 0;
                int y = 0;
                HexGeometry.HVtoHX(i, j, ref x, ref y);
                AddNum(lines, "HSEngine.HVtoHX_X_" + tag, x);
                AddNum(lines, "HSEngine.HVtoHX_Y_" + tag, y);
                HexGeometry.HPtoHX(i, j, ref x, ref y);
                AddNum(lines, "HSEngine.HPtoHX_X_" + tag, x);
                AddNum(lines, "HSEngine.HPtoHX_Y_" + tag, y);
                small.AsSpan().Fill(0xA5);
                ref sbyte sx = ref Unsafe.As<byte, sbyte>(ref small[1]);
                ref sbyte sy = ref Unsafe.As<byte, sbyte>(ref small[7]);
                HexGeometry.HPtoHXs(i, j, ref sx, ref sy);
                for (int k = 0; k < 12; k++)
                {
                    AddHex(lines, "HSEngine.HPtoHXs_" + tag + "_" + Dec(k), small[k]);
                }
            }
        }

        for (int i = 0; i < Values.Length; i++)
        {
            AddNum(lines, "HSEngine.HXtoV_extreme_" + Dec(i), HexGeometry.HXtoV(Values[i], -1));
            AddNum(lines, "HSEngine.HXtoH_extreme_" + Dec(i), HexGeometry.HXtoH(Values[i], 0));
            int x = 0;
            int y = 0;
            HexGeometry.HPtoHX(Values[i], Values[i], ref x, ref y);
            AddNum(lines, "HSEngine.HPtoHX_extreme_X_" + Dec(i), x);
            AddNum(lines, "HSEngine.HPtoHX_extreme_Y_" + Dec(i), y);
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
                var walk = new HNWalkData();
                walk.CenterX = a;
                walk.CenterY = b;
                for (int i = 0; i < 80; i++)
                {
                    HNWalkData.WalkToNextHN(ref walk);
                    string tag = Dec(a) + "_" + Dec(b) + "_" + Dec(i);
                    AddNum(lines, "HSEngine.WalkToNextHN_X_" + tag, walk.X);
                    AddNum(lines, "HSEngine.WalkToNextHN_Y_" + tag, walk.Y);
                    AddNum(lines, "HSEngine.WalkToNextHN_side_" + tag, walk.Side);
                    AddNum(lines, "HSEngine.WalkToNextHN_step_" + tag, walk.Step);
                    AddNum(lines, "HSEngine.WalkToNextHN_radius_" + tag, walk.Radius);
                    AddNum(lines, "HSEngine.WalkToNextHN_HN_" + tag, walk.HN);
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

    private static void AddNum(List<string> lines, string label, int value) => lines.Add(label + " " + Dec(value));

    private static void AddHex(List<string> lines, string label, int value) => lines.Add(label + " " + unchecked((uint)value).ToString("X8"));

    private static void AddText(List<string> lines, string label, string value) => lines.Add(label + " " + value);

    private static string Dec(int value) => value.ToString(CultureInfo.InvariantCulture);
}
