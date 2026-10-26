// Generated from E2DirectSound.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.E2DirectSound
{
    public partial class IDirectSoundBuffer : Aow.Rtl.IUnknown
    {
        // E2DirectSound.pas:15
        public int GetCaps(object Caps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:16
        public int GetCurrentPosition(object Play, object Write)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:17
        public int GetFormat(object Format, int Size, object Written)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:18
        public int GetVolume(ref int Volume)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:19
        public int GetPan(ref int Pan)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:20
        public int GetFrequency(ref int Frequency)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:21
        public int GetStatus(ref int Status)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:22
        public int Initialize(Aow.Rtl.IUnknown Sound, in Aow.Units.E2DirectSound.TDSBufferDesc Desc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:23
        public int Lock(int Offset, int Size, ref object Data1, ref int Size1, ref object Data2, ref int Size2, int Flags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:25
        public int Play(int Reserved1, int Reserved2, int Flags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:26
        public int SetCurrentPosition(int Position)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:27
        public int SetFormat(in Aow.Rtl.TWaveFormatEx Format)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:28
        public int SetVolume(int Volume)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:29
        public int SetPan(int Pan)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:30
        public int SetFrequency(int Frequency)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:31
        public int Stop()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:32
        public int Unlock(object Data1, int Size1, object Data2, int Size2)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:33
        public int Restore()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public struct TDSBufferDesc
    {
        public int Size;
        public int Flags;
        public int BufferBytes;
        public int Reserved;
        public Ptr<Aow.Rtl.TWaveFormatEx> Format;
    }

    public partial class IDirectSound : Aow.Rtl.IUnknown
    {
        // E2DirectSound.pas:37
        public int CreateSoundBuffer(in Aow.Units.E2DirectSound.TDSBufferDesc Desc, Aow.Units.E2DirectSound.IDirectSoundBuffer Buffer, Aow.Rtl.IUnknown Outer)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:39
        public int GetCaps(object Caps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:40
        public int DuplicateSoundBuffer(Aow.Units.E2DirectSound.IDirectSoundBuffer Source, Aow.Units.E2DirectSound.IDirectSoundBuffer Dest)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:41
        public int SetCooperativeLevel(int Window, int Level)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:42
        public int Compact()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:43
        public int GetSpeakerConfig(ref int Config)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:44
        public int SetSpeakerConfig(int Config)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // E2DirectSound.pas:45
        public int Initialize(Ptr<Aow.Rtl.TGUID> Device)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public static partial class E2DirectSound
    {
    }
}
