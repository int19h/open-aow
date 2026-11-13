
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

// Production id 10: trade for a turn, in a city or a production place. TurnsTotal is 1 for a
// queued trade and -1 for the default one (City.pas:1475, PrdPlace.pas:511). Both controls
// trade alike, a power node names it after the power it channels.
public sealed partial record Trade(int TurnsTotal) : ProductionOrder
{
    public const int Kind = 10;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, TurnsTotal, 0);

    public override CityAction CityAction => CityAction.Trade;

    public override string Name(ProductionControl control) => control.MagicSphere() != MagicSphere.None
        ? AbilityRules.TranslateResourceString(GenerateExtraPowerText)
        : AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText);

    public override string NameProgressive(ProductionControl control) => control.MagicSphere() != MagicSphere.None
        ? AbilityRules.TranslateResourceString(GameText.GeneratingExtraPowerText)
        : AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText);

    public override bool CanExecute(ProductionControl control, ref string reason) => true;

    public override void Executed(ProductionControl control)
    {
        control.Settings.TurnsTotal = (sbyte)TurnsTotal;
        control.Settings.TurnsLeft = 1;
        control.Settings.Player = control.GetPlayer();
        control.Status = ProductionControlStatus.Producing;
    }

    public override void Completed(ProductionControl control)
    {
        control.ProductionDone();
    }

    public override int ActivityImage(ProductionPlace place) => place.GetMagicSphere() != MagicSphere.None ? 48 : 47;

    internal static string GenerateExtraPowerText { get; } = "Generate Extra Power";
}
