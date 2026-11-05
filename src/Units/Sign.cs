// Translated from Sign.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Sign;

// FText, FTextSeen
public partial class TSign : TStructure
{
    public TSign(TECustomNode owner) : base(owner)
    {
        FText = new TStringList();
        FTextSeen = new TBitList();
    }

    // Sign.pas:99
    public new const int ClassID = 131330;

    public TStringList FText;
    public TBitList FTextSeen;
    // Sign.pas:92
    public override void Destroy()
    {
        FText.Free();
        FTextSeen.Free();
        base.Destroy();
    }

    // Sign.pas:105
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FText, 7);
        if (AoWHSMap != null && AoWHSMap.FConnectionType != (TPlayerConnectionType)1)
        {
            Stream.rwStreamProc(8, FTextSeen.LoadFromStream, FTextSeen.SaveToStream);
        }
    }

    // Sign.pas:85


    // Sign.pas:102
    public override sbyte GetOverlay(int Index) => -1;

    // Sign.pas:119
    public override void Select()
    {
        base.Select();
        if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            FTextSeen[AoWHSMap.FSeatedPlayer] = true;
        }
    }

    // Sign.pas:113
    public bool GetTextSeen(sbyte Player) => FTextSeen[Player];

    // Sign.pas:116
    public void SetText(TStrings Text)
    {
        FText.Assign(Text);
        Update();
    }

    // Sign.pas:125
    public bool CanChangeText()
    {
        TArmyHS ArmyHS;
        ArmyHS = ((TAoWMapField)Owner).GetArmyHS();
        return ArmyHS != null && (byte)ArmyHS.FArmy.FPlayer == (byte)AoWHSMap.FSeatedPlayer;
    }

    // Sign.pas:134
    public bool ChangeText(int Player, TStrings Text)
    {
        bool Result;
        TUpdateSignTE Event;
        if (!AoWHSMap.FPlayers.GetPlayers(Player).GetBusy() && CanChangeText())
        {
            Event = new TUpdateSignTE(null);
            Event.FXhx = GetXhx();
            Event.FYhx = GetYhx();
            Event.FLhx = GetLhx();
            Event.FText.Assign(Text);
            AoWHSMap.FTokenManager.GetTokenControl(Player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sign.pas:24


}

public partial class TSignResource : TStructureResource
{
    public TSignResource(TECustomNode owner) : base(owner)
    {
        FConnectRoads = false;
        FTerrainBound = false;
    }

    // Sign.pas:159
    public new const int ClassID = 131331;

    // Sign.pas:152


    // Sign.pas:165
    public override int ResourceUserClassID() => 131330;

    // Sign.pas:168
    public override void Show(int X, int Y, in TSurfaceDesc SurfaceDesc, TStructure Structure)
    {
        TImageSequence Sequence;
        int Index;
        int Offset;
        int Image;
        bool Animate;
        if (FImageSequenceList != null)
        {
            Index = ((TSign)Structure).ImageIndex;
            Offset = ((TMapField)((TSign)Structure).Owner).Xhx + ((TMapField)((TSign)Structure).Owner).Yhx * 16;
            Animate = AoWHSMap.FSeatedPlayer < 0 || !((TSign)Structure).GetTextSeen(AoWHSMap.FSeatedPlayer);
            if (Index < FImageSequenceList.Count)
            {
                Sequence = (TImageSequence)FImageSequenceList.List[Index];
            }
            else
            {
                Sequence = null;
            }

            while (Sequence != null)
            {
                if (Animate)
                {
                    Image = Sequence.GetFrameIndex(AoWHSMap.Frame, Offset);
                }
                else
                {
                    Image = Sequence.GetUnloopedFrameIndex(0);
                }

                if (Image != -1)
                {
                    if (((TSign)Structure).GetSelected() && (AoWHSMap.Frame & 6) == 6)
                    {
                        Sequence.ImageLibrary[Image].ShowTransparent(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                    }
                    else
                    {
                        Sequence.ImageLibrary[Image].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                    }
                }

                Sequence = Sequence.Next;
            }
        }
    }

    // Sign.pas:37


    // Sign.pas:162
    public override string ResourceEditName() => TranslateRStr(SignRStr);

}

// FText, FXhx, FYhx, FLhx
public partial class TUpdateSignTE : TTokenEvent
{
    public TUpdateSignTE(TECustomNode owner) : base(owner)
    {
        FText = new TStringList();
    }

    // Sign.pas:202
    public new const int ClassID = 131343;

    public TStringList FText;
    public sbyte FXhx;
    public sbyte FYhx;
    public sbyte FLhx;
    // Sign.pas:199
    public override void Destroy()
    {
        FText.Free();
        base.Destroy();
    }

    // Sign.pas:205
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FText, 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 18);
    }

    // Sign.pas:196


    // Sign.pas:214
    public override TExecuteResult Execute(TTokenExecuter TE)
    {
        TExecuteResult Result;
        TSign Sign;
        Sign = (TSign)AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx).FindOwnedChild(131330);
        if (Sign != null)
        {
            Sign.SetText(FText);
            Sign.FTextSeen.Clear();
            Sign.FTextSeen[AoWHSMap.FCurrentPlayer] = true;
            Result = TExecuteResult.erDone;
        }
        else
        {
            Result = TExecuteResult.erDoneHalt;
        }

        return Result;
    }

    // Sign.pas:54


}

[StructLayout(LayoutKind.Explicit, Size = 315)]
public struct TSignMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_164 Reserved;
    [FieldOffset(164)]
    public sbyte CurrentPlayer;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
    [FieldOffset(166)]
    public FixedArray_byte_148 ReservedA6;
    [FieldOffset(314)]
    public byte StorageType;
}

public static partial class Sign
{
    // Sign.pas:229
    public static void RegisterSign(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TSign>(), EClass.Of<TSignResource>(), EClass.Of<TUpdateSignTE>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
