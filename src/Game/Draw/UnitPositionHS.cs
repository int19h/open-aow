// Translated from AoWTC.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw;

// FPosition. FIndex. FImageIndex. FImages. FDirection
public partial class UnitPositionHS : SingleHS
{
    public UnitPositionHS(ECustomNode owner) : base(owner)
    {
        FPosition = 0;
        FIndex = 0;
        FDirection = 1;
    }

    // AoWTC.pas:5016
    public new const int ClassID = 2228483;

    public sbyte FPosition;
    public sbyte FIndex;
    public int FImageIndex;
    public CustomImageLibrary FImages;
    public byte FDirection;

    // AoWTC.pas:5060
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPosition), 25);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FIndex), 26);
        Stream.rwByte(ref FDirection, 27);
    }

    // AoWTC.pas:5008

    // AoWTC.pas:5021
    public override byte GetLevel() => 27;

    // AoWTC.pas:5074
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        FImages[FImageIndex].ShowClipped(X - XhpSurf - 8, Y - YhpSurf - 8, SurfaceDesc);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        XYL P = default;
        base.Activate();
        if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting) && AoWCombatMap.Current.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex))
        {
            var yhx = GetYhx();
            var packXYL = HexGeometry.PackXYL(GetXhx(), yhx, 0);
            AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
            FImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            P.X = GetXhx();
            P.Y = GetYhx();
            P.L = 0;
            P.Flag = (sbyte)FDirection;
            AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
            Dispose();
        }
    }

    // AoWTC.pas:5049
    public override void Deactivate()
    {
        if (Activated() && ((MapLevel)Owner.Owner).Map.Status.Contains(HSMapStatusElement.Editting))
        {
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCUNITS.ILB");
            var unitPosition = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(FPosition, FIndex);
            var xhx = GetXhx();
            bool condition = unitPosition.X == xhx;
            if (condition)
            {
                var fUnitPositionsUnitPosition = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var yhx = GetYhx();
                condition = fUnitPositionsUnitPosition.Y == yhx;
            }

            if (condition)
            {
                var packXYL = HexGeometry.PackXYL(-1, -1, -1);
                AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
            }
        }

        base.Deactivate();
    }

    // AoWTC.pas:5068
    public override bool CanPlace(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => Map.GetField(Xhx, Yhx, Lhx).FindOwnedChild(2228483) == null && AoWCombatMap.Current.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);

    // AoWTC.pas:5026
    public void UpdateImageIndex()
    {
        FImageIndex = FPosition * 8 + FIndex;
    }

    // AoWTC.pas:223

}
