using static Aow.Rtl.RtlUnit;

namespace Aow.Rtl.Tests;

public sealed class StreamsProbeTests
{
    [ProbeFact("streams")]
    public void StreamsFixture()
    {
        using var directory = new ProbeDirectory();
        var p = new ProbeLines("streams");
        void State(string tag, TStream s) { p.Fact(tag + "_position", s.Position); p.Fact(tag + "_size", s.Size); }
        void Exercise(string tag, TStream s)
        {
            byte[] data = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray(), read = new byte[32];
            State(tag + "_created", s); s.WriteBuffer(data); State(tag + "_initialize32", s); s.Size = 4; State(tag + "_shrink4", s);
            p.Fact(tag + "_seek_begin", s.Seek(1, 0)); State(tag + "_after_begin", s); p.Fact(tag + "_write2", s.Write(data.AsSpan(16, 2))); State(tag + "_after_write2", s);
            p.Fact(tag + "_seek_current", s.Seek(-1, 1)); State(tag + "_after_current", s); p.Fact(tag + "_seek_end", s.Seek(-1, 2)); State(tag + "_after_end", s);
            p.Fact(tag + "_read4", s.Read(read.AsSpan(0, 4))); State(tag + "_after_short_read", s); p.Fact(tag + "_read_eof", s.Read(read.AsSpan(0, 1))); State(tag + "_after_eof", s);
            p.Fact(tag + "_seek_beyond", s.Seek(4, 2)); State(tag + "_after_beyond", s); p.Fact(tag + "_write_beyond", s.Write(data.AsSpan(24, 2))); State(tag + "_after_write_beyond", s);
            s.Position = 0; State(tag + "_rewind", s); Array.Clear(read); p.Fact(tag + "_read10", s.Read(read.AsSpan(0, 10))); State(tag + "_after_read10", s); p.Fact(tag + "_bytes", Convert.ToHexString(read.AsSpan(0, 10)));
            s.Position = s.Size - 1; State(tag + "_before_readbuffer", s); p.Fact(tag + "_readbuffer_past", p.Thrown(() => s.ReadBuffer(read.AsSpan(0, 2)))); State(tag + "_after_readbuffer", s);
            p.Fact(tag + "_seek_negative", s.Seek(-1, 0)); State(tag + "_after_negative", s); p.Fact(tag + "_negative_read", s.Read(read.AsSpan(0, 1))); State(tag + "_after_negative_read", s); p.Fact(tag + "_negative_write", s.Write(data.AsSpan(0, 1))); State(tag + "_after_negative_write", s);
            p.Fact(tag + "_seek_zero", s.Seek(0, 0)); State(tag + "_after_zero", s); p.Fact(tag + "_seek_invalid_origin", s.Seek(5, 99)); State(tag + "_after_invalid_origin", s);
            s.Position = 0; State(tag + "_before_zero_write", s); p.Fact(tag + "_write_zero", s.Write([])); State(tag + "_after_zero_write", s); p.Fact(tag + "_read_zero", s.Read([])); State(tag + "_after_zero_read", s);
            s.Position = 20; State(tag + "_seek20", s); p.Fact(tag + "_zero_write_past", s.Write([])); State(tag + "_after_zero_past", s); s.Size = 2; State(tag + "_shrink2", s);
        }
        using (var m = new TMemoryStream())
        {
            p.Fact("memory_capacity_initial", m.Capacity); Exercise("memory", m); p.Fact("memory_capacity_after", m.Capacity); m.Clear(); State("memory_clear", m); p.Fact("memory_capacity_clear", m.Capacity);
            m.Size = 8192; State("memory_size8192", m); p.Fact("memory_capacity8192", m.Capacity); m.Size = 8193; State("memory_size8193", m); p.Fact("memory_capacity8193", m.Capacity);
        }
        using (var f = new TFileStream("probe-stream.bin", fmCreate)) Exercise("file", f);
        byte[] buffer = new byte[61457];
        using (var f = new TFileStream("probe-stream.bin", fmOpenRead | fmShareDenyWrite))
        {
            State("file_reopened", f); p.Fact("file_readonly_write", f.Write(buffer.AsSpan(0, 1))); State("file_after_readonly_write", f);
            p.Fact("file_readonly_writebuffer", p.Thrown(() => f.WriteBuffer(buffer.AsSpan(0, 1)))); State("file_after_readonly_writebuffer", f);
        }
        p.Fact("file_deleted", Ord(DeleteFile("probe-stream.bin")));
        using var src = new TMemoryStream(); using var dst = new TMemoryStream();
        void Both(string tag) { State(tag + "_source", src); State(tag + "_destination", dst); }
        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)(i % 251);
        src.WriteBuffer(buffer.AsSpan(0, 8)); Both("copy_initial_write"); src.Position = 3; Both("copy_source_position3"); dst.WriteBuffer(buffer.AsSpan(20, 2)); Both("copy_dest_prefix");
        p.Fact("copy_zero_count", dst.CopyFrom(src, 0)); Both("copy_zero_after"); src.Position = 2; Both("copy_explicit_start"); p.Fact("copy_explicit_count", dst.CopyFrom(src, 3)); Both("copy_explicit_after");
        p.Fact("copy_too_many", p.Thrown(() => dst.CopyFrom(src, 9))); Both("copy_too_many_after"); dst.Position = 0; Both("copy_before_readback");
        Array.Clear(buffer, 0, 32); int count = dst.Read(buffer.AsSpan(0, 32)); Both("copy_readback"); for (int i = 0; i < count; i++) p.Fact($"copy_byte_{i}", buffer[i]);
        src.Clear(); dst.Clear(); Both("copy_cleared"); p.Fact("copy_empty_zero", dst.CopyFrom(src, 0)); Both("copy_empty_after");
        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)(i % 251);
        src.WriteBuffer(buffer); Both("large_source_write"); src.Position = 123; Both("large_source_seek"); p.Fact("large_copy_zero_count", dst.CopyFrom(src, 0)); Both("large_copy_after");
        dst.Position = 0; Both("large_dest_rewind"); count = dst.Read(buffer); Both("large_readback"); p.Fact("large_read_count", count); p.Fact("large_checksum", buffer.Take(count).Sum(b => (int)b)); p.Fact("large_boundary_before", buffer[61439]); p.Fact("large_boundary_after", buffer[61440]); p.End();
    }
}
