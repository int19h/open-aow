// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// guard location
public partial class GuardAreaAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:161
    public new const int ClassId = 136273;

    public MapCoordinate GuardLocation { get; internal set; }

    // The fourth byte of the packed location stored at id 50, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    // IndependentAG.pas:170
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = StoredCoordinateImage.From(GuardLocation, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("GuardLocation", value: location);
        (GuardLocation, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // IndependentAG.pas:153
    public GuardAreaAIGroup(CustomNode owner) : base(owner)
    {
        GuardLocation = MapCoordinate.None;
        ProcessPriority = 1;
        RecruitmentPriority = 2;
    }

    // IndependentAG.pas:167
    public override EngineClass GroupControlClass() => EngineClass.Of<GuardAreaAIGroupControl>();

    // IndependentAG.pas:164
    public static new GroupBehavior Behavior => GroupBehavior.GuardArea;

    public override void OnControlMoveFailed() => GuardLocation = MapCoordinate.None;

    public override void ProcessControlStates(AIGroupControl control)
    {
        MapField mapField;
        AIGroupTargetFinder finder;
        MoveControlSettings settings;
        switch (control.State)
        {
            case 0:
                if (GuardLocation.X == -1)
                {
                    GuardLocation = control.Location;
                }

                finder = new AIGroupTargetFinder(null) { Holder = control.CreationHolder() };
                finder.Setup(control);
                {
                    finder.Selector.SourceLocation = GuardLocation;
                    AITargetInfo targetInfo = finder.Selector.TargetInfo;
                    targetInfo.Mode = AITargetMode.GuardArea;
                    finder.Selector.TargetInfo = targetInfo;
                }

                sbyte moves = control.Army.Moves(control.Selection);
                finder.Selector.MovePointsMax = moves;
                control.ActivatePlugin(finder);
                finder.Release();
                control.State += 1;
                break;
            case 1:
                control.Targets.Validate();
                if (control.Targets.Count != 0)
                {
                    if (control.Army.Moves(control.Selection) != control.Army.MovePoints(control.Selection) || GuardLocation != control.Location)
                    {
                        settings = new MoveControlSettings(null) { Holder = control.CreationHolder() };
                        settings.Player = control.Army.Player;
                        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                        settings.SetSourcePosition(control.Army.Location);
                        settings.MovePointsMax = control.Army.MovePoints(control.Selection);
                        settings.ArmyHexSprite = control.ArmyHexSprite;
                        settings.Selection = control.Selection;
                        settings.MovePointTable = control.MovePointTable;
                        StrategicMap.Current.MoveControl.ProcessMove(settings);
                        for (int i = control.Targets.Count - 1; i >= 0; i--)
                        {
                            if (settings.GetMovePointsMapCoordinate(control.Targets.GetTargets(i).Location) == 16777215)
                            {
                                control.Targets.Delete(i);
                            }
                        }

                        settings.Release();
                    }

                    if (control.Targets.Count != 0)
                    {
                        control.Targets.SortOnPriority();
                        control.SetupME();
                        AIGroupControlTarget targetLocation = control.Targets.GetTargets(0);
                        AIGroupControlTarget target = control.Targets.GetTargets(0);
                        control.ExecuteME(control.Targets.GetTargets(0).Location.X, target.Location.Y, targetLocation.Location.MapLevel);
                        control.Targets.Delete(0);
                    }
                    else
                    {
                        control.State = 2;
                    }
                }
                else
                {
                    control.State = 2;
                }

                break;
            case 2:
                if (GuardLocation != control.Location)
                {
                    mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(GuardLocation);
                    if (mapField.GetPlayer() == -1 || mapField.GetPlayer() == Player)
                    {
                        control.SetupME();
                        control.ExecuteME(GuardLocation.X, GuardLocation.Y, GuardLocation.MapLevel);
                    }
                    else
                    {
                        control.Done();
                    }
                }
                else
                {
                    control.Done();
                }

                break;
        }
    }

}
