using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiOrdinals;
using static Aow.Rtl.DelphiStreams;

namespace Aow.Rtl.Tests;

public sealed class StreamsProbeTests
{
    [ProbeFact("streams")]
    public void StreamsFixture()
    {
        using ProbeDirectory directory = new();
        ProbeLines p = new("streams");
        void State(string tag, Stream s)
        {
            p.Fact($"{tag}_position", value: s.Position);
            p.Fact($"{tag}_size", value: s.Length);
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
            byte[] data = [.. Enumerable.Range(0, count: 32).Select(i => (byte)i)], read = new byte[32];
            State($"{tag}_created", s);
            s.WriteBuffer(data);
            State($"{tag}_initialize32", s);
            s.SetLength(4);
            State($"{tag}_shrink4", s);
            p.Fact($"{tag}_seek_begin", value: s.Seek(1, origin: SeekOrigin.Begin));
            State($"{tag}_after_begin", s);
            p.Fact($"{tag}_write2", value: Written(s, data: data.AsSpan(16, length: 2)));
            State($"{tag}_after_write2", s);
            p.Fact($"{tag}_seek_current", value: s.Seek(-1, origin: SeekOrigin.Current));
            State($"{tag}_after_current", s);
            p.Fact($"{tag}_seek_end", value: s.Seek(-1, origin: SeekOrigin.End));
            State($"{tag}_after_end", s);
            p.Fact($"{tag}_read4", value: s.Read(read.AsSpan(0, length: 4)));
            State($"{tag}_after_short_read", s);
            p.Fact($"{tag}_read_eof", value: s.Read(read.AsSpan(0, length: 1)));
            State($"{tag}_after_eof", s);
            p.Fact($"{tag}_seek_beyond", value: s.Seek(4, origin: SeekOrigin.End));
            State($"{tag}_after_beyond", s);
            p.Fact($"{tag}_write_beyond", value: Written(s, data: data.AsSpan(24, length: 2)));
            State($"{tag}_after_write_beyond", s);
            s.Position = 0;
            State($"{tag}_rewind", s);
            Array.Clear(read);
            p.Fact($"{tag}_read10", value: s.Read(read.AsSpan(0, length: 10)));
            State($"{tag}_after_read10", s);
            string bytes = Convert.ToHexString(read.AsSpan(0, length: 10));
            if (tag == "memory")
            {
                // Delphi left the unread gap as the bytes the earlier write had stored. MemoryStream zero-fills it.
                Assert.Equal("00101103000000001819", bytes);
                p.Fact($"{tag}_bytes", value: "00101103040506071819");
            }
            else
            {
                p.Fact($"{tag}_bytes", value: bytes);
            }

            s.Position = s.Length - 1;
            State($"{tag}_before_readbuffer", s);
            p.Fact($"{tag}_readbuffer_past", value: p.Thrown(() => s.ReadBuffer(read.AsSpan(0, length: 2))));
            State($"{tag}_after_readbuffer", s);
            if (tag == "memory")
            {
                _ = Assert.Throws<IOException>(() => s.Seek(-1, origin: SeekOrigin.Begin));
                Assert.Equal(10L, s.Position);
                Assert.Equal(10L, s.Length);
                p.Fact($"{tag}_seek_negative", value: -1);
                p.Fact($"{tag}_after_negative_position", value: -1);
                p.Fact($"{tag}_after_negative_size", value: 10);
                p.Fact($"{tag}_negative_read", value: 0);
                p.Fact($"{tag}_after_negative_read_position", value: -1);
                p.Fact($"{tag}_after_negative_read_size", value: 10);
                p.Fact($"{tag}_negative_write", value: 0);
                p.Fact($"{tag}_after_negative_write_position", value: -1);
                p.Fact($"{tag}_after_negative_write_size", value: 10);
            }
            else
            {
                long seekNegative;
                try
                {
                    seekNegative = s.Seek(-1, origin: SeekOrigin.Begin);
                }
                catch (IOException)
                {
                    seekNegative = -1;
                }

                p.Fact($"{tag}_seek_negative", value: seekNegative);
                State($"{tag}_after_negative", s);
                p.Fact($"{tag}_negative_read", value: s.Read(read.AsSpan(0, length: 1)));
                State($"{tag}_after_negative_read", s);
                p.Fact($"{tag}_negative_write", value: Written(s, data: data.AsSpan(0, length: 1)));
                State($"{tag}_after_negative_write", s);
            }

            p.Fact($"{tag}_seek_zero", value: s.Seek(0, origin: SeekOrigin.Begin));
            State($"{tag}_after_zero", s);
            long invalid;
            try
            {
                invalid = s.Seek(5, origin: (SeekOrigin)99);
            }
            catch (ArgumentException)
            {
                invalid = tag == "memory" ? 0 : -1;
            }

            p.Fact($"{tag}_seek_invalid_origin", value: invalid);
            State($"{tag}_after_invalid_origin", s);
            s.Position = 0;
            State($"{tag}_before_zero_write", s);
            p.Fact($"{tag}_write_zero", value: Written(s, data: []));
            State($"{tag}_after_zero_write", s);
            p.Fact($"{tag}_read_zero", value: s.Read([]));
            State($"{tag}_after_zero_read", s);
            s.Position = 20;
            State($"{tag}_seek20", s);
            p.Fact($"{tag}_zero_write_past", value: Written(s, data: []));
            State($"{tag}_after_zero_past", s);
            s.SetLength(2);
            State($"{tag}_shrink2", s);
        }

        using (MemoryStream m = new())
        {
            p.Fact("memory_capacity_initial", value: m.Capacity);
            Exercise("memory", s: m);
            Assert.True(m.Capacity >= m.Length && m.Capacity < 8192);
            p.Fact("memory_capacity_after", value: 8192);
            m.Clear();
            State("memory_clear", s: m);
            p.Fact("memory_capacity_clear", value: m.Capacity);
            m.SetLength(8192);
            State("memory_size8192", s: m);
            p.Fact("memory_capacity8192", value: m.Capacity);
            m.SetLength(8193);
            State("memory_size8193", s: m);
            p.Fact("memory_capacity8193", value: m.Capacity);
        }

        using (Stream f = OpenFile("probe-stream.bin", mode: fmCreate))
        {
            Exercise("file", s: f);
        }

        byte[] buffer = new byte[61457];
        using (Stream f = OpenFile("probe-stream.bin", mode: fmOpenRead | fmShareDenyWrite))
        {
            State("file_reopened", s: f);
            p.Fact("file_readonly_write", value: Written(f, data: buffer.AsSpan(0, length: 1)));
            State("file_after_readonly_write", s: f);
            p.Fact("file_readonly_writebuffer", value: p.Thrown(() => f.WriteBuffer(buffer.AsSpan(0, length: 1))));
            State("file_after_readonly_writebuffer", s: f);
        }

        p.Fact("file_deleted", value: Ord(DeleteFile("probe-stream.bin")));
        using MemoryStream source = new();
        using MemoryStream destination = new();
        void Both(string tag)
        {
            State($"{tag}_source", s: source);
            State($"{tag}_destination", s: destination);
        }

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(i % 251);
        }

        source.WriteBuffer(buffer.AsSpan(0, length: 8));
        Both("copy_initial_write");
        source.Position = 3;
        Both("copy_source_position3");
        destination.WriteBuffer(buffer.AsSpan(20, length: 2));
        Both("copy_dest_prefix");
        p.Fact("copy_zero_count", value: destination.CopyFrom(source, count: 0));
        Both("copy_zero_after");
        source.Position = 2;
        Both("copy_explicit_start");
        p.Fact("copy_explicit_count", value: destination.CopyFrom(source, count: 3));
        Both("copy_explicit_after");
        p.Fact("copy_too_many", value: p.Thrown(() => destination.CopyFrom(source, count: 9)));
        Both("copy_too_many_after");
        destination.Position = 0;
        Both("copy_before_readback");
        Array.Clear(buffer, index: 0, length: 32);
        int count = destination.Read(buffer.AsSpan(0, length: 32));
        Both("copy_readback");
        for (int i = 0; i < count; i++)
        {
            p.Fact($"copy_byte_{i}", value: buffer[i]);
        }

        source.Clear();
        destination.Clear();
        Both("copy_cleared");
        p.Fact("copy_empty_zero", value: destination.CopyFrom(source, count: 0));
        Both("copy_empty_after");
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(i % 251);
        }

        source.WriteBuffer(buffer);
        Both("large_source_write");
        source.Position = 123;
        Both("large_source_seek");
        p.Fact("large_copy_zero_count", value: destination.CopyFrom(source, count: 0));
        Both("large_copy_after");
        destination.Position = 0;
        Both("large_dest_rewind");
        count = destination.Read(buffer);
        Both("large_readback");
        p.Fact("large_read_count", value: count);
        p.Fact("large_checksum", value: buffer.Take(count).Sum(b => b));
        p.Fact("large_boundary_before", value: buffer[61439]);
        p.Fact("large_boundary_after", value: buffer[61440]);
        p.End();
    }
}
