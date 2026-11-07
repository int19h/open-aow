// Translated from Pad.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ILTer;

namespace Aow.Units.Pad;

// FPadActive
public partial class TPad : TFixedILTerrainMO
{
    public TPad(TECustomNode owner) : base(owner)
    {
        FPadActive = true;
    }

    // Pad.pas:52
    public new const int ClassID = 131334;

    public bool FPadActive;

    // Pad.pas:46

    // Pad.pas:55
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // Pad.pas:58
    public override sbyte ForceTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
    {
        sbyte Result;
        if (MapLevel.Lhx != 0)
        {
            Result = 12;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Pad.pas:20

}

// FPadSize
public partial class TPadResource : TFixedILTerrainMOR
{
    public TPadResource(TECustomNode owner) : base(owner)
    {
        SetLevel(10);
    }

    // Pad.pas:76
    public new const int ClassID = 131335;

    public byte FPadSize;

    // Pad.pas:69
    public override void Destroy()
    {
        if (Engine != null && !Engine.HSSet.Status.Contains(THSSetStatusElement.ssDestroying))
        {
            ((TAoWHSSet)Engine.HSSet).FPadControl.UnregisterPadResource(this, FPadSize);
        }

        base.Destroy();
    }

    // Pad.pas:63

    // Pad.pas:101
    public override EditorClass Editor()
    {
        EditorClass Result = Engine.Editors.FindEditor(131335);
        if (Result == null)
        {
            Result = base.Editor();
        }

        return Result;
    }

    // Pad.pas:95
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ActivateMsg();
        }
    }

    // Pad.pas:82
    public override int ResourceUserClassID() => 131334;

    // Pad.pas:85
    public void ActivateMsg()
    {
        FPadSize = 0;
        for (int I = 0, last = TerrainList.Count - 1; I <= last; I++)
        {
            if (TerrainList.GetTerrain(I) >= -1)
            {
                FPadSize = (byte)(FPadSize + 1);
            }
        }

        ((TAoWHSSet)Engine.HSSet).FPadControl.RegisterPadResource(this, FPadSize);
    }

    // Pad.pas:32

    // Pad.pas:79
    public override string ResourceEditName() => "Pad";

}

public static partial class PadUnit
{
    // Pad.pas:107
    public static void RegisterPad(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TPad>(), EClass.Of<TPadResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
