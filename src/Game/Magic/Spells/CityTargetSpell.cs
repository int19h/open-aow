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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// source unit ID. city target modes
public partial class CityTargetSpell : Spell
{
    public int SourceId;
    public Set<CityTargetModeElement> CityTargetMode;

    // CitySpells.pas:209
    public CityTargetSpell(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Global;
    }

    // CitySpells.pas:377
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            SourceId = source.UnitId;
            SetupTargetSelection(source, AoWHSMap.Current.TargetSelection);
            AoWHSMap.Current.TargetSelection.Activate();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CitySpells.pas:288
    public virtual void OnTargetSelectionChange(TargetSelectionControl targetSelection)
    {
        Troop source = default;
        bool valid = default;
        source = AoWHSMap.Current.UnitControl.FindUnit(SourceId);
        if (source != null)
        {
            switch (targetSelection.Change)
            {
                case TargetSelectionChange.Deactivated:
                    targetSelection.FOnChange -= OnTargetSelectionChange;
                    targetSelection.Deactivate();
                    break;
                case TargetSelectionChange.Selected:
                    ValidateTarget(targetSelection, targetSelection.TargetX, targetSelection.TargetY, targetSelection.TargetL, ref valid);
                    if (valid)
                    {
                        if (TargetSelected(source, targetSelection.TargetX, targetSelection.TargetY, targetSelection.TargetL))
                        {
                            targetSelection.FOnChange -= OnTargetSelectionChange;
                            targetSelection.Deactivate();
                        }
                    }

                    break;
            }
        }
        else
        {
            targetSelection.FOnChange -= OnTargetSelectionChange;
            targetSelection.Deactivate();
        }
    }

    // CitySpells.pas:215
    public virtual void PlayDefaultCityEffects(Troop source, City targetCity)
    {
        MapLevel level;
        ImageSequenceAnimation A;
        HexNumberWalkData walk = default;
        bool playSound;
        playSound = false;
        var hexLevel = targetCity.GetHexLevel();
        level = AoWHSMap.Current.MapContainer.GetMapLevel(hexLevel);
        var hexY = targetCity.GetHexY();
        var hexX = targetCity.GetHexX();
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
        while (walk.HexNumber < targetCity.Resource.TerrainList.Count)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height && targetCity.Resource.TerrainList.Used[walk.HexNumber])
            {
                bool condition = source.Player == AoWHSMap.Current.SeatedPlayer;
                if (!condition)
                {
                    var targetCityHexLevel = targetCity.GetHexLevel();
                    condition = AoWHSMap.Current.TerrainVisible((sbyte)walk.X, (sbyte)walk.Y, targetCityHexLevel, 0);
                }

                if (condition)
                {
                    // { Original passes the city layer before the radius. }
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                    A.FrameCounter = Random(5);
                    A.AnimationLevel = 27;
                    var targetCityHexLevel = targetCity.GetHexLevel();
                    AoWHSMap.Current.PlaceHex(A, walk.X, walk.Y, targetCityHexLevel);
                    bool test = source.Player == AoWHSMap.Current.SeatedPlayer;
                    if (!test)
                    {
                        var hexLevel2 = targetCity.GetHexLevel();
                        test = AoWHSMap.Current.WatchingTerrain((sbyte)walk.X, (sbyte)walk.Y, hexLevel2, 0);
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
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
        }
    }

    // CitySpells.pas:323
    public virtual void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        targetSelection.Initialize(source);
        targetSelection.TargetMode = targetSelection.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
        var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_selectTargetCityForOnMapFormat), [SpellName]);
        targetSelection.Text.Text = format;
        targetSelection.Title = SpellName;
        targetSelection.CancelText = AbilityRules.TranslateResourceString(GameText.s_cancelText);
        targetSelection.SourceImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[20].GetPreviewImage();
        targetSelection.ValidTargetImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[21].GetPreviewImage();
        targetSelection.InvalidTargetImage = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[22].GetPreviewImage();
        source.GetLocation(ref X, ref Y, ref L);
        targetSelection.SourceX = X;
        targetSelection.SourceY = Y;
        targetSelection.SourceL = L;
        targetSelection.Range = 12;
        if (targetSelection._onChangeStore.Count > 1)
        {
            // { Original rejects two existing listeners. }
            throw new InvalidOperationException("oohps");
        }

        targetSelection.FOnChange += OnTargetSelectionChange;
        targetSelection.OnValidateTarget = ValidateTarget;
    }

    // CitySpells.pas:359
    public virtual bool TargetSelected(Troop source, int X, int Y, int L)
    {
        bool result;
        CityTargetSpellTokenEvent eventValue;
        if (!AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = CreateTokenEvent();
            if (SetupTokenEvent(eventValue, source, X, Y, L))
            {
                AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                result = true;
            }
            else
            {
                result = false;
            }

            eventValue.Release();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CitySpells.pas:248
    public virtual bool ValidTargetCity(Troop source, City targetCity)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City found;
        bool result = (CityTargetMode.Contains(CityTargetModeElement.Own) && targetCity.Player == source.Player) || (CityTargetMode.Contains(CityTargetModeElement.Enemy) && targetCity.Player != source.Player);
        result = result && (!targetCity.GetRazed() || CityTargetMode.Contains(CityTargetModeElement.Razed));
        if (result && CityTargetMode.Contains(CityTargetModeElement.SkipSource))
        {
            source.GetLocation(ref X, ref Y, ref L);
            found = (City)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(City));
            result = targetCity != found;
        }

        return result;
    }

    // CitySpells.pas:346
    public virtual CityTargetSpellTokenEvent CreateTokenEvent() => new CityTargetSpellTokenEvent(null);

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
        Troop source;
        MapLevel level;
        AoWMapField field;
        City targetCity;
        level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        source = AoWHSMap.Current.UnitControl.FindUnit(SourceId);
        if (source != null && X >= 0 && Y >= 0 && X < level.Width && Y < level.Height)
        {
            field = (AoWMapField)level.GetField(X, Y);
            valid = !AoWHSMap.Current.ExplorationEnabled || field.GetExplored(source.Player);
            if (valid)
            {
                targetCity = (City)field.FindNoneTransparentHexSprite(typeof(City));
                if (targetCity != null)
                {
                    valid = ValidTargetCity(source, targetCity);
                }
                else
                {
                    valid = false;
                }
            }
        }
        else
        {
            valid = false;
        }
    }

    private static readonly string s_selectTargetCityForOnMapFormat = "Select target city for %s on map.";
}
