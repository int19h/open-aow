#nullable disable
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// source unit ID. city target modes
public class CityTargetSpell : Spell
{
    private int _sourceId;

    public Set<CityTargetModeElement> CityTargetMode { get; protected set; }

    // CitySpells.pas:209
    public CityTargetSpell(CustomNode owner) : base(owner) => SpellType = SpellType.Global;

    // CitySpells.pas:377
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            _sourceId = source.UnitId;
            SetupTargetSelection(source, StrategicMap.Current.TargetSelection);
            StrategicMap.Current.TargetSelection.Activate();
            return true;
        }
        else
        {
            return false;
        }
    }

    // CitySpells.pas:288
    public virtual void OnTargetSelectionChange(TargetSelectionControl targetSelection)
    {
        Troop source = StrategicMap.Current.UnitControl.FindUnit(_sourceId);
        if (source is not null)
        {
            switch (targetSelection.Change)
            {
                case TargetSelectionChange.Deactivated:
                    targetSelection.Changed -= OnTargetSelectionChange;
                    targetSelection.Deactivate();
                    break;
                case TargetSelectionChange.Selected:
                    bool valid = default;
                    ValidateTarget(targetSelection, targetSelection.Target.X, targetSelection.Target.Y, targetSelection.Target.MapLevel, ref valid);
                    if (valid)
                    {
                        if (TargetSelected(source, targetSelection.Target.X, targetSelection.Target.Y, targetSelection.Target.MapLevel))
                        {
                            targetSelection.Changed -= OnTargetSelectionChange;
                            targetSelection.Deactivate();
                        }
                    }

                    break;
                case TargetSelectionChange.None:
                case TargetSelectionChange.Activated:
                default:
                    break;
            }
        }
        else
        {
            targetSelection.Changed -= OnTargetSelectionChange;
            targetSelection.Deactivate();
        }
    }

    // CitySpells.pas:215
    public virtual void PlayDefaultCityEffects(Troop source, City targetCity)
    {
        bool playSound = false;
        sbyte hexLevel = targetCity.GetHexLevel();
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(hexLevel);
        sbyte hexY = targetCity.GetHexY();
        sbyte hexX = targetCity.GetHexX();
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, 0);
        while (walk.HexNumber < targetCity.Resource.TerrainList.Count)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height && targetCity.Resource.TerrainList.Used[walk.HexNumber])
            {
                bool condition = source.Player == StrategicMap.Current.SeatedPlayer;
                if (!condition)
                {
                    sbyte targetCityHexLevel = targetCity.GetHexLevel();
                    condition = StrategicMap.Current.TerrainVisible((sbyte)walk.X, (sbyte)walk.Y, targetCityHexLevel, 0);
                }

                if (condition)
                {
                    // { Original passes the city layer before the radius. }
                    ImageSequenceAnimation a = new(null) { Holder = CreationHolder() };
                    a.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                    a.FrameCounter = Random(5);
                    a.AnimationLevel = 27;
                    sbyte targetCityHexLevel = targetCity.GetHexLevel();
                    _ = StrategicMap.Current.PlaceHex(a, walk.X, walk.Y, targetCityHexLevel);
                    bool test = source.Player == StrategicMap.Current.SeatedPlayer;
                    if (!test)
                    {
                        sbyte hexLevel2 = targetCity.GetHexLevel();
                        test = StrategicMap.Current.WatchingTerrain((sbyte)walk.X, (sbyte)walk.Y, hexLevel2, 0);
                    }

                    if (test)
                    {
                        playSound = true;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (playSound)
        {
            _ = GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
        }
    }

    // CitySpells.pas:323
    public virtual void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        targetSelection.Initialize(source);
        targetSelection.TargetMode += Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
        string format = Format(AbilityRules.TranslateResourceString(GetEngine(), s_selectTargetCityForOnMapFormat), [SpellName]);
        targetSelection.Text.Text = format;
        targetSelection.Title = SpellName;
        targetSelection.CancelText = AbilityRules.TranslateResourceString(GetEngine(), GameText.CancelText);
        targetSelection.SourceImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[20].GetPreviewImage();
        targetSelection.ValidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[21].GetPreviewImage();
        targetSelection.InvalidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[22].GetPreviewImage();
        MapCoordinate coordinate58 = source.GetLocation();
        sbyte x = coordinate58.X;
        sbyte y = coordinate58.Y;
        sbyte l = coordinate58.MapLevel;
        targetSelection.SourceLocation = new MapCoordinate(x, y, l);
        targetSelection.Range = 12;
        if (targetSelection.OnChangeStore.Count > 1)
        {
            // { Original rejects two existing listeners. }
            throw new InvalidOperationException("oohps");
        }

        targetSelection.Changed += OnTargetSelectionChange;
        targetSelection.SetValidateTargetHandler(ValidateTarget);
    }

    // CitySpells.pas:359
    public virtual bool TargetSelected(Troop source, int X, int Y, int L)
    {
        bool result;
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            CityTargetSpellTokenEvent eventValue = CreateTokenEvent();
            if (SetupTokenEvent(eventValue, source, X, Y, L))
            {
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                result = true;
            }
            else
            {
                result = false;
            }

            _ = eventValue.Release();
        }
        else
        {
            return false;
        }

        return result;
    }

    // CitySpells.pas:248
    public virtual bool ValidTargetCity(Troop source, City targetCity)
    {
        bool result = (CityTargetMode.Contains(CityTargetModeElement.Own) && targetCity.Player == source.Player) || (CityTargetMode.Contains(CityTargetModeElement.Enemy) && targetCity.Player != source.Player);
        result = result && (!targetCity.GetRazed() || CityTargetMode.Contains(CityTargetModeElement.Razed));
        if (result && CityTargetMode.Contains(CityTargetModeElement.SkipSource))
        {
            MapCoordinate coordinate59 = source.GetLocation();
            sbyte x = coordinate59.X;
            sbyte y = coordinate59.Y;
            sbyte l = coordinate59.MapLevel;
            City found = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<City>();
            return targetCity != found;
        }

        return result;
    }

    // CitySpells.pas:346
    public virtual CityTargetSpellTokenEvent CreateTokenEvent() => new(null) { Holder = CreationHolder() };

    // CityTargetSpell.CreateTokenEvent constructs CityTargetSpellTokenEvent.
    public sealed override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
        => ExecuteTokenEvent((CityTargetSpellTokenEvent)eventValue, source: source, executeCount: executeCount);

    protected virtual bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount) => false;

    // CitySpells.pas:351
    public virtual bool SetupTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int X, int Y, int L)
    {
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        eventValue.TargetPlayer = eventValue.GetSelectedCity().Player;
        return true;
    }

    // CitySpells.pas:262
    public void ValidateTarget(TargetSelectionControl targetSelection, sbyte X, sbyte Y, sbyte L, ref bool valid)
    {
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        Troop source = StrategicMap.Current.UnitControl.FindUnit(_sourceId);
        if (source is not null && X >= 0 && Y >= 0 && X < level.Width && Y < level.Height)
        {
            MapField field = level.GetField(X, Y);
            valid = !StrategicMap.Current.ExplorationEnabled || field.GetExplored(source.Player);
            if (valid)
            {
                City targetCity = field.FindNoneTransparentHexSprite<City>();
                valid = targetCity is not null && ValidTargetCity(source, targetCity);
            }
        }
        else
        {
            valid = false;
        }
    }

    private static readonly string s_selectTargetCityForOnMapFormat = "Select target city for %s on map.";
}
