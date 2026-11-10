
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

// Production id 10: trade for a turn, in a city or a production place. TurnsTotal is 1 for a
// queued trade and -1 for the default one (City.pas:1475, PrdPlace.pas:511). Both controls
// trade alike, a power node names it after the power it channels.
public sealed partial record Trade(int TurnsTotal) : ProductionOrder
{
    public const int Kind = 10;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, TurnsTotal, 0);

    public override CityAction CityAction => CityAction.Trade;

    public override string Name(ProductionControl Control) => Control.MagicSphere() != MagicSphere.None
        ? AbilityRules.TranslateRStr(GenerateExtraPowerRStr)
        : AbilityRules.TranslateRStr(GameText.ProduceMerchandiseRStr);

    public override string NameProgressive(ProductionControl Control) => Control.MagicSphere() != MagicSphere.None
        ? AbilityRules.TranslateRStr(GameText.GeneratingExtraPowerRStr)
        : AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);

    public override bool CanExecute(ProductionControl Control, ref string Reason) => true;

    public override void Executed(ProductionControl Control)
    {
        Control.FSettings.FTurnsTotal = (sbyte)TurnsTotal;
        Control.FSettings.FTurnsLeft = 1;
        Control.FSettings.FPlayer = Control.GetPlayer();
        Control.FStatus = ProductionControl.Status.Producing;
    }

    public override void Completed(ProductionControl Control)
    {
        Control.ProductionDone();
    }

    public override int ActivityImage(ProductionPlace Place) => Place.GetMagicSphere() != MagicSphere.None ? 48 : 47;

    internal static readonly string GenerateExtraPowerRStr = "Generate Extra Power";
}
