// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// player
public partial class Storm : Animation
{
    public Storm(ECustomNode owner) : base(owner)
    { }

    public sbyte FPlayer;

    // Storms.pas:34
    public virtual void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Storms.pas:236
    public virtual void UpdateTargetSelection(TargetSelectionControl Selection)
    {
        Selection.FOnShow = ShowTargetSelection;
        Selection.FTargetMode = Selection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.OwnerDraw);
    }

    // Storms.pas:209
    public virtual void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
    {
        FPlayer = Player;
        FAnimationLevel = 28;
        AoWHSMap.Current.LockExecuteEventLog();
    }

    // Storms.pas:216
    public virtual bool UpdateStorm() => false;

    // Storms.pas:221
    public virtual void FinalizeStorm()
    {
        AoWHSMap.Current.UnlockExecuteEventLog();
    }

    // Storms.pas:226
    public void ShowTargetSelection(TargetSelectionControl Selection, int HN, int Xs, int Ys, in LockedSurface SurfaceDesc)
    {
        int Intensity;
        HN = HexGeometry.HNtoRad(HN);
        Intensity = Math.Abs((AoWHSMap.Current.Frame + HN) % 9 - 4) * 20 + 20;
        var lookupTable = GraphicsState.IntensityBlendTable.GetLookupTables(Intensity);
        Selection.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, lookupTable);
    }

    // Storms.pas:242
    public bool GetBestTarget(sbyte Player, int X, int Y, int L, int Range, ref MapCoordinate Target, ref StormDamageInfo Info)
    {
        StormDamageInfo Candidate;
        HexNumberWalkData Walk = default;
        MapLevel Level;
        ArmyHexSprite ArmyHS;
        Level = AoWHSMap.Current.MapContainer.MapLevels[L];
        Target.Packed = 16777215;
        bool Result = false;
        Info = default;
        Candidate = default;
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Range)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                ArmyHS = ((AoWMapField)Level.Field[Walk.X, Walk.Y]).GetArmyHS();
                if (ArmyHS != null && ArmyHS.FArmy.FPlayer != 0 && AbilityRules.ValidAttackRelation(Player, 0, ArmyHS.FArmy.FPlayer, 0))
                {
                    GetTargetDamageInfo(Player, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L, ref Candidate);
                    if (Info.EnemyDamage - Info.FriendlyDamage < Candidate.EnemyDamage - Candidate.FriendlyDamage)
                    {
                        Info = Candidate;
                        Target = HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                        Result = true;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // Storms.pas:39
    public virtual string Name()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

}
