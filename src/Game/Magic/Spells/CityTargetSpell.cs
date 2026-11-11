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
    public int FSourceID;
    public Set<CityTargetModeElement> FCityTargetMode;

    // CitySpells.pas:209
    public CityTargetSpell(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Global;
    }

    // CitySpells.pas:377
    public override bool Activate(Troop Source)
    {
        bool Result;
        string Reason = "";
        if (CanActivate(Source, ref Reason))
        {
            FSourceID = Source.FUnitID;
            SetupTargetSelection(Source, AoWHSMap.Current.FTargetSelection);
            AoWHSMap.Current.FTargetSelection.Activate();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CitySpells.pas:288
    public virtual void OnTargetSelectionChange(TargetSelectionControl TargetSelection)
    {
        Troop Source = default;
        bool Valid = default;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
        if (Source != null)
        {
            switch (TargetSelection.FChange)
            {
                case TargetSelectionChange.Deactivated:
                    TargetSelection.FOnChange -= OnTargetSelectionChange;
                    TargetSelection.Deactivate();
                    break;
                case TargetSelectionChange.Selected:
                    ValidateTarget(TargetSelection, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL, ref Valid);
                    if (Valid)
                    {
                        if (TargetSelected(Source, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL))
                        {
                            TargetSelection.FOnChange -= OnTargetSelectionChange;
                            TargetSelection.Deactivate();
                        }
                    }

                    break;
            }
        }
        else
        {
            TargetSelection.FOnChange -= OnTargetSelectionChange;
            TargetSelection.Deactivate();
        }
    }

    // CitySpells.pas:215
    public virtual void PlayDefaultCityEffects(Troop Source, City TargetCity)
    {
        MapLevel Level;
        ImageSequenceAnimation A;
        HexNumberWalkData Walk = default;
        bool PlaySound;
        PlaySound = false;
        var lhx = TargetCity.GetLhx();
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(lhx);
        var yhx = TargetCity.GetYhx();
        var xhx = TargetCity.GetXhx();
        HexNumberWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
        while (Walk.HN < TargetCity.Resource.TerrainList.Count)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height && TargetCity.Resource.TerrainList.Used[Walk.HN])
            {
                bool condition = Source.FPlayer == AoWHSMap.Current.FSeatedPlayer;
                if (!condition)
                {
                    var targetCityLhx = TargetCity.GetLhx();
                    condition = AoWHSMap.Current.TerrainVisible((sbyte)Walk.X, (sbyte)Walk.Y, targetCityLhx, 0);
                }

                if (condition)
                {
                    // { Original passes the city layer before the radius. }
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                    A.FFrameCounter = Random(5);
                    A.FAnimationLevel = 27;
                    var targetCityLhx = TargetCity.GetLhx();
                    AoWHSMap.Current.PlaceHX(A, Walk.X, Walk.Y, targetCityLhx);
                    bool test = Source.FPlayer == AoWHSMap.Current.FSeatedPlayer;
                    if (!test)
                    {
                        var lhx2 = TargetCity.GetLhx();
                        test = AoWHSMap.Current.WatchingTerrain((sbyte)Walk.X, (sbyte)Walk.Y, lhx2, 0);
                    }

                    if (test)
                    {
                        PlaySound = true;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (PlaySound)
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }
    }

    // CitySpells.pas:323
    public virtual void SetupTargetSelection(Troop Source, TargetSelectionControl TargetSelection)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TargetSelection.Initialize(Source);
        TargetSelection.FTargetMode = TargetSelection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
        var format = RtlUnit.Format(AbilityRules.TranslateRStr(SelectTargetCityForXOnMapRStr), [FSpellName]);
        TargetSelection.FText.Text = format;
        TargetSelection.FTitle = FSpellName;
        TargetSelection.FCancelText = AbilityRules.TranslateRStr(GameText.CancelRStr);
        TargetSelection.FSourceImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
        TargetSelection.FValidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
        TargetSelection.FInvalidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
        Source.GetLocation(ref X, ref Y, ref L);
        TargetSelection.FSourceX = X;
        TargetSelection.FSourceY = Y;
        TargetSelection.FSourceL = L;
        TargetSelection.FRange = 12;
        if (TargetSelection.FOnChangeStore.Count > 1)
        {
            // { Original rejects two existing listeners. }
            throw new InvalidOperationException("oohps");
        }

        TargetSelection.FOnChange += OnTargetSelectionChange;
        TargetSelection.FOnValidateTarget = ValidateTarget;
    }

    // CitySpells.pas:359
    public virtual bool TargetSelected(Troop Source, int X, int Y, int L)
    {
        bool Result;
        CityTargetSpellTokenEvent Event;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = CreateTE();
            if (SetupTE(Event, Source, X, Y, L))
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Result = true;
            }
            else
            {
                Result = false;
            }

            Event.Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CitySpells.pas:248
    public virtual bool ValidTargetCity(Troop Source, City TargetCity)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City Found;
        bool Result = (FCityTargetMode.Contains(CityTargetModeElement.Own) && TargetCity.FPlayer == Source.FPlayer) || (FCityTargetMode.Contains(CityTargetModeElement.Enemy) && TargetCity.FPlayer != Source.FPlayer);
        Result = Result && (!TargetCity.GetRazed() || FCityTargetMode.Contains(CityTargetModeElement.Razed));
        if (Result && FCityTargetMode.Contains(CityTargetModeElement.SkipSource))
        {
            Source.GetLocation(ref X, ref Y, ref L);
            Found = (City)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(City));
            Result = TargetCity != Found;
        }

        return Result;
    }

    // CitySpells.pas:346
    public virtual CityTargetSpellTokenEvent CreateTE() => new CityTargetSpellTokenEvent(null);

    // CitySpells.pas:351
    public virtual bool SetupTE(CityTargetSpellTokenEvent Event, Troop Source, int X, int Y, int L)
    {
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        Event.FTargetPlayer = Event.GetSelectedCity().FPlayer;
        return true;
    }

    // CitySpells.pas:262
    public void ValidateTarget(TargetSelectionControl TargetSelection, sbyte X, sbyte Y, sbyte L, ref bool Valid)
    {
        Troop Source;
        MapLevel Level;
        AoWMapField Field;
        City TargetCity;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
        if (Source != null && X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
        {
            Field = (AoWMapField)Level.GetField(X, Y);
            Valid = !AoWHSMap.Current.FExplorationEnabled || Field.GetExplored(Source.FPlayer);
            if (Valid)
            {
                TargetCity = (City)Field.FindNoneTransparentHS(typeof(City));
                if (TargetCity != null)
                {
                    Valid = ValidTargetCity(Source, TargetCity);
                }
                else
                {
                    Valid = false;
                }
            }
        }
        else
        {
            Valid = false;
        }
    }

    private static readonly string SelectTargetCityForXOnMapRStr = "Select target city for %s on map.";
}
