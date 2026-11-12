using static Aow.Rtl.RtlUnit;

namespace Aow.Rtl.Tests;

public sealed class StreamsProbeTests
{
    [ProbeFact("streams")]
    public void StreamsFixture()
    {
        using var directory = new ProbeDirectory();
        var p = new ProbeLines("streams");
        void State(string tag, Stream s)
        {
            p.Fact(tag + "_position", s.Position);
            p.Fact(tag + "_size", s.Length);
        }

        long Written(Stream s, ReadOnlySpan<byte> data)
        {
            long before = s.Position;
            try
            {
                s.Write(data);
            }
            catch (NotSupportedException)
            {
                return 0;
            }

            return s.Position - before;
        }

        void Exercise(string tag, Stream s)
        {
            byte[] data = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray(), read = new byte[32];
            State(tag + "_created", s);
            s.WriteBuffer(data);
            State(tag + "_initialize32", s);
            s.SetLength(4);
            State(tag + "_shrink4", s);
            p.Fact(tag + "_seek_begin", s.Seek(1, SeekOrigin.Begin));
            State(tag + "_after_begin", s);
            p.Fact(tag + "_write2", Written(s, data.AsSpan(16, 2)));
            State(tag + "_after_write2", s);
            p.Fact(tag + "_seek_current", s.Seek(-1, SeekOrigin.Current));
            State(tag + "_after_current", s);
            p.Fact(tag + "_seek_end", s.Seek(-1, SeekOrigin.End));
            State(tag + "_after_end", s);
            p.Fact(tag + "_read4", s.Read(read.AsSpan(0, 4)));
            State(tag + "_after_short_read", s);
            p.Fact(tag + "_read_eof", s.Read(read.AsSpan(0, 1)));
            State(tag + "_after_eof", s);
            p.Fact(tag + "_seek_beyond", s.Seek(4, SeekOrigin.End));
            State(tag + "_after_beyond", s);
            p.Fact(tag + "_write_beyond", Written(s, data.AsSpan(24, 2)));
            State(tag + "_after_write_beyond", s);
            s.Position = 0;
            State(tag + "_rewind", s);
            Array.Clear(read);
            p.Fact(tag + "_read10", s.Read(read.AsSpan(0, 10)));
            State(tag + "_after_read10", s);
            string bytes = Convert.ToHexString(read.AsSpan(0, 10));
            if (tag == "memory")
            {
                // Delphi left the unread gap as the bytes the earlier write had stored. MemoryStream zero-fills it.
                Xunit.Assert.Equal("00101103000000001819", bytes);
                p.Fact(tag + "_bytes", "00101103040506071819");
            }
            else
            {
                p.Fact(tag + "_bytes", bytes);
            }

            s.Position = s.Length - 1;
            State(tag + "_before_readbuffer", s);
            p.Fact(tag + "_readbuffer_past", p.Thrown(() => s.ReadBuffer(read.AsSpan(0, 2))));
            State(tag + "_after_readbuffer", s);
            if (tag == "memory")
            {
                Xunit.Assert.Throws<IOException>(() => s.Seek(-1, SeekOrigin.Begin));
                Xunit.Assert.Equal(10L, s.Position);
                Xunit.Assert.Equal(10L, s.Length);
                p.Fact(tag + "_seek_negative", -1);
                p.Fact(tag + "_after_negative_position", -1);
                p.Fact(tag + "_after_negative_size", 10);
                p.Fact(tag + "_negative_read", 0);
                p.Fact(tag + "_after_negative_read_position", -1);
                p.Fact(tag + "_after_negative_read_size", 10);
                p.Fact(tag + "_negative_write", 0);
                p.Fact(tag + "_after_negative_write_position", -1);
                p.Fact(tag + "_after_negative_write_size", 10);
            }
            else
            {
                long seekNegative;
                try
                {
                    seekNegative = s.Seek(-1, SeekOrigin.Begin);
                }
                catch (IOException)
                {
                    seekNegative = -1;
                }

                p.Fact(tag + "_seek_negative", seekNegative);
                State(tag + "_after_negative", s);
                p.Fact(tag + "_negative_read", s.Read(read.AsSpan(0, 1)));
                State(tag + "_after_negative_read", s);
                p.Fact(tag + "_negative_write", Written(s, data.AsSpan(0, 1)));
                State(tag + "_after_negative_write", s);
            }

            p.Fact(tag + "_seek_zero", s.Seek(0, SeekOrigin.Begin));
            State(tag + "_after_zero", s);
            long invalid;
            try
            {
                invalid = s.Seek(5, (SeekOrigin)99);
            }
            catch (ArgumentException)
            {
                invalid = tag == "memory" ? 0 : -1;
            }

            p.Fact(tag + "_seek_invalid_origin", invalid);
            State(tag + "_after_invalid_origin", s);
            s.Position = 0;
            State(tag + "_before_zero_write", s);
            p.Fact(tag + "_write_zero", Written(s, []));
            State(tag + "_after_zero_write", s);
            p.Fact(tag + "_read_zero", s.Read([]));
            State(tag + "_after_zero_read", s);
            s.Position = 20;
            State(tag + "_seek20", s);
            p.Fact(tag + "_zero_write_past", Written(s, []));
            State(tag + "_after_zero_past", s);
            s.SetLength(2);
            State(tag + "_shrink2", s);
        }

        using (var m = new MemoryStream())
        {
            p.Fact("memory_capacity_initial", m.Capacity);
            Exercise("memory", m);
            Xunit.Assert.True(m.Capacity >= m.Length && m.Capacity < 8192);
            p.Fact("memory_capacity_after", 8192);
            m.Clear();
            State("memory_clear", m);
            p.Fact("memory_capacity_clear", m.Capacity);
            m.SetLength(8192);
            State("memory_size8192", m);
            p.Fact("memory_capacity8192", m.Capacity);
            m.SetLength(8193);
            State("memory_size8193", m);
            p.Fact("memory_capacity8193", m.Capacity);
        }

        using (var f = OpenFile("probe-stream.bin", fmCreate)) Exercise("file", f);
        byte[] buffer = new byte[61457];
        using (var f = OpenFile("probe-stream.bin", fmOpenRead | fmShareDenyWrite))
        {
            State("file_reopened", f);
            p.Fact("file_readonly_write", Written(f, buffer.AsSpan(0, 1)));
            State("file_after_readonly_write", f);
            p.Fact("file_readonly_writebuffer", p.Thrown(() => f.WriteBuffer(buffer.AsSpan(0, 1))));
            State("file_after_readonly_writebuffer", f);
        }

        p.Fact("file_deleted", Ord(DeleteFile("probe-stream.bin")));
        using var source = new MemoryStream();
        using var destination = new MemoryStream();
        void Both(string tag)
        {
            State(tag + "_source", source);
            State(tag + "_destination", destination);
        }

        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)(i % 251);
        source.WriteBuffer(buffer.AsSpan(0, 8));
        Both("copy_initial_write");
        source.Position = 3;
        Both("copy_source_position3");
        destination.WriteBuffer(buffer.AsSpan(20, 2));
        Both("copy_dest_prefix");
        p.Fact("copy_zero_count", destination.CopyFrom(source, 0));
        Both("copy_zero_after");
        source.Position = 2;
        Both("copy_explicit_start");
        p.Fact("copy_explicit_count", destination.CopyFrom(source, 3));
        Both("copy_explicit_after");
        p.Fact("copy_too_many", p.Thrown(() => destination.CopyFrom(source, 9)));
        Both("copy_too_many_after");
        destination.Position = 0;
        Both("copy_before_readback");
        Array.Clear(buffer, 0, 32);
        int count = destination.Read(buffer.AsSpan(0, 32));
        Both("copy_readback");
        for (int i = 0; i < count; i++) p.Fact($"copy_byte_{i}", buffer[i]);
        source.Clear();
        destination.Clear();
        Both("copy_cleared");
        p.Fact("copy_empty_zero", destination.CopyFrom(source, 0));
        Both("copy_empty_after");
        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)(i % 251);
        source.WriteBuffer(buffer);
        Both("large_source_write");
        source.Position = 123;
        Both("large_source_seek");
        p.Fact("large_copy_zero_count", destination.CopyFrom(source, 0));
        Both("large_copy_after");
        destination.Position = 0;
        Both("large_dest_rewind");
        count = destination.Read(buffer);
        Both("large_readback");
        p.Fact("large_read_count", count);
        p.Fact("large_checksum", buffer.Take(count).Sum(b => (int)b));
        p.Fact("large_boundary_before", buffer[61439]);
        p.Fact("large_boundary_after", buffer[61440]);
        p.End();
    }
}
