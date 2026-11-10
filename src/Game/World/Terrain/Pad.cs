// Translated from Pad.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

// FPadActive
public partial class Pad : FixedILTerrainMO
{
    public Pad(ECustomNode owner) : base(owner)
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
    public override sbyte ForceTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx)
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

    // FPadSize
    public new partial class Resource : FixedILTerrainMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(10);
        }

        // Pad.pas:76
        public new const int ClassID = 131335;

        public byte FPadSize;

        // Pad.pas:69
        public override void Dispose()
        {
            if (Engine != null && !Engine.HSSet.Status.Contains(HSSetStatusElement.Destroying))
            {
                ((AoWHSSet)Engine.HSSet).FPadControl.UnregisterPadResource(this, FPadSize);
            }

            base.Dispose();
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
        public override void MsgProc(ref EMsg Msg, EObject Sender)
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

            ((AoWHSSet)Engine.HSSet).FPadControl.RegisterPadResource(this, FPadSize);
        }

        // Pad.pas:32

        // Pad.pas:79
        public override string ResourceEditName() => "Pad";

    }

    // Pad.pas:20

}
