// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

public partial class ConstructStructureTE : TokenEvent
{
    public ConstructStructureTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135962;

    public int FUnitID;
    public sbyte FPlayer;
    public int FControlID;

    // Structur.PAS:3098
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FControlID, 20);
        Stream.rwInteger(ref FUnitID, 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 22);
    }

    // Structur.PAS:3106
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        AbstractUnit AUnit;
        StructureConstructionControl Control;
        StructureResource Resource;
        Structure Structure;
        string Reason = "";
        string Text = "";
        int Gold = default;
        int Days = default;
        EChangeNotifyNode Owner;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel Level;
        HNWalkData Walk = default;
        ExecuteResult Result = ExecuteResult.DoneHalt;
        base.Execute(TE);
        AUnit = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        Control = (StructureConstructionControl)((ConstructAbility)AoWHSSet.Current.FAbilityControl.GetAbility(125)).FConstructionControls.FindID(FControlID);
        if (Control != null && AUnit != null && AUnit.FPlayer == FPlayer && Control.CanConstruct(AUnit, ref Reason))
        {
            Resource = Control.StructureResource(AUnit);
            if (Resource != null)
            {
                AUnit.GetLocation(ref X, ref Y, ref L);
                Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
                HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.HN < Resource.TerrainList.Count)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Level.Width > Walk.X && Level.Height > Walk.Y && Resource.TerrainList.GetUsed(Walk.HN))
                    {
                        AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, L, 0, -1);
                    }

                    HNWalkData.WalkToNextHN(ref Walk);
                }

                bool locked = AUnit.Owner is not HeroList;
                Owner = locked ? (EChangeNotifyNode)AUnit.Owner : null;
                if (locked)
                {
                    Owner.BeginUpdate();
                }

                Structure = (Structure)Resource.PlaceResourceUser(AoWHSMap.Current, X, Y, L);
                if (Structure != null)
                {
                    Control.GetInfo(AUnit, ref Text, ref Gold, ref Days);
                    Structure.ExecuteBuild(AUnit, Days);
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer);
                    AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).SetGems(players.FGems - Gold);
                    AUnit.Killed();
                    Result = ExecuteResult.Done;
                }

                if (locked)
                {
                    Owner.EndUpdate();
                }
            }
        }

        return Result;
    }

    // NetworkE.pas:230

}
