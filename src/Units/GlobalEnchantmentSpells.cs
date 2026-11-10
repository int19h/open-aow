// Translated from GlobalEnchantmentSpells.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands
{

    // FEnchantment
    public partial class GlobalEnchantmentSpellTE : SpellTE
    {
        public GlobalEnchantmentSpellTE(ECustomNode owner) : base(owner)
        { }

        // GlobalEnchantmentSpells.pas:389
        public new const int ClassID = 135954;

        public GlobalEnchantment FEnchantment;

        // GlobalEnchantmentSpells.pas:379
        public override void Dispose()
        {
            if (FEnchantment != null)
            {
                FEnchantment.Release();
                FEnchantment = null;
            }

            base.Dispose();
        }

        // GlobalEnchantmentSpells.pas:394
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FEnchantment, null, 25);
        }

        // GlobalEnchantmentSpells.pas:400
        public override bool Validate(Hero source)
        {
            string Reason = "";
            bool Result = base.Validate(source);
            return Result && FEnchantment.CanCast(source.FPlayer, ref Reason);
        }

        // GlobalEnchantmentSpells.pas:408
        public override bool Process(AbstractUnit source, int value) => base.Process(source, value);

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalEnchantment
{

    // FAIBasePriority
    public partial class GlobalEnchantmentSpell : Spell
    {
        public int FAIBasePriority;

        // GlobalEnchantmentSpells.pas:413
        public GlobalEnchantmentSpell(ECustomNode owner) : base(owner)
        {
            FAIBasePriority = -1;
        }

        // GlobalEnchantmentSpells.pas:429
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            if (ExecuteCount == 0)
            {
                ((GlobalEnchantmentSpellTE)Event).FEnchantment.Cast(Source.FPlayer);
            }

            return false;
        }

        // GlobalEnchantmentSpells.pas:494
        public override void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
        {
            AICastSpellAction Action;
            bool aiCanCastSpell = AICanCastSpell(Source);
            if (aiCanCastSpell)
            {
                var aiUpkeepPriority = AIUpkeepPriority();
                aiCanCastSpell = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
            }

            if (aiCanCastSpell)
            {
                Action = CreateCastSpellAction();
                SetupCastSpellAction(Source, Action);
                Action.FPriority = FAIBasePriority;
                Action.SetOwner(Actions);
            }
        }

        // GlobalEnchantmentSpells.pas:507
        public override bool AIExecuteCastSpellAction(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            if (AICanCastSpell(Source))
            {
                var aiUpkeepPriority = AIUpkeepPriority();
                AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
                var hero = (Hero)Source;
                if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
                {
                    Result = this.Activate(Source);
                }
                else
                {
                    if (hero.FCastingSpellID != FSpellID)
                    {
                        hero.ExecuteStartCasting(FSpellID);
                        Result = true;
                    }
                    else
                    {
                        Result = true;
                    }
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:435
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
        {
            Magic.GlobalEnchantment Enchantment;
            bool Result = base.CanActivate(Source, ref MessageText);
            if (Result)
            {
                if (!Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
                {
                    Enchantment = CreateEnchantment();
                    if (Enchantment != null)
                    {
                        SetupEnchantment(Source, Enchantment);
                        Result = Enchantment.CanCast(Source.FPlayer, ref MessageText);
                        Enchantment.Release();
                    }
                }
                else
                {
                    Result = false;
                    MessageText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr);
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:458
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            GlobalEnchantmentSpellTE Event;
            string Reason = "";
            if (CanActivate(Source, ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new GlobalEnchantmentSpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.FEnchantment = CreateEnchantment();
                SetupEnchantment(Source, Event.FEnchantment);
                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:476
        public virtual bool AICanCastSpell(AbstractUnit Source)
        {
            string Reason = "";
            bool Result = FAIBasePriority != -1 && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
            return Result && CanActivate(Source, ref Reason);
        }

        // GlobalEnchantmentSpells.pas:419
        public virtual Magic.GlobalEnchantment CreateEnchantment() => null;

        // GlobalEnchantmentSpells.pas:424
        public virtual void SetupEnchantment(AbstractUnit Source, Magic.GlobalEnchantment Enchantment)
        {
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
        }

        // GlobalEnchantmentSpells.pas:484
        public int AIUpkeepPriority()
        {
            Magic.GlobalEnchantment Enchantment;
            // { Original creates the base enchantment here. }
            Enchantment = new Magic.GlobalEnchantment(null);
            int Result = Enchantment.GetAIUpkeepPriority();
            Enchantment.Release();
            return Result;
        }
    }

    public partial class EnchantedRoadsEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:535
        public new const int ClassID = 135955;

        // GlobalEnchantmentSpells.pas:527
        public EnchantedRoadsEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnchantedRoadsRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:540
        public override int GetAIUpkeepPriority() => 900;

        // GlobalEnchantmentSpells.pas:563
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
            return true;
        }

        // GlobalEnchantmentSpells.pas:545
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result && Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EnchantedRoads))
            {
                var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:555
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
            return true;
        }

    }

    public partial class EnchantedRoads : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:571
        public EnchantedRoads(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnchantedRoadsRStr);
            FSpellID = 44;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Earth;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:613
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Animation Anim = default;
            TwinkleAnimation Twinkle = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Anim = new Animation(null);
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap.Current;
            Anim.Activate();
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(44).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:636
        public override Magic.GlobalEnchantment CreateEnchantment() => new EnchantedRoadsEnchantment(null);

        public void ShowAnimation(Animation anim, int xhpSurface, int yhpSurface, in SurfaceDesc surface)
        {
            var map = AoWHSMap.Current;
            var level = map.MapContainer.GetMapLevel(map.ViewL);
            int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
            for (int x = map.ViewXhx; x <= maxX; x++)
            {
                int xhp = 0, yhp = 0;
                Aow.Game.World.Map.HexGeometry.HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
                xhp = xhp - xhpSurface + 16;
                yhp = yhp - yhpSurface + 16;
                for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
                {
                    var field = (AoWMapField)level.GetField(x, y);
                    if (field.OverlayType == 4 && map.FSeatedPlayer != -1 && field.GetExplored(map.FSeatedPlayer))
                        FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - ((field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255)) & 15), surface);
                }
            }
            if (anim.FFrameCounter == 50) anim.Dispose();
        }
    }

    public partial class AirMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:649
        public new const int ClassID = 135966;

        // GlobalEnchantmentSpells.pas:641
        public AirMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.AirMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:654
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:679
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
            // { Original uses Earth. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Earth);
            return true;
        }

        // GlobalEnchantmentSpells.pas:659
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
                {
                    var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                    var name = GetName();
                    Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:670
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
            // { Original uses Earth. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Earth);
            return true;
        }

    }

    public partial class AirMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:688
        public AirMastery(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.AirMasteryRStr);
            FSpellID = 60;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            // { Original also assigns Earth to Air Mastery. }
            FMagicSphere = MagicSphere.Earth;
            FCastDuration = 40;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:727
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Animation Anim = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Air);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Anim = new Animation(null);
            Anim.FAnimationLevel = 28;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap.Current;
            Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            Anim.Activate();
            Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(60).FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:747
        public override Magic.GlobalEnchantment CreateEnchantment() => new AirMasteryEnchantment(null);

        // GlobalEnchantmentSpells.pas:701
        public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Count;
            int X;
            int Y;
            int Speed;
            int Image;
            if (AoWHSMap.Current.ViewL == 0)
            {
                RandSeed = AoWHSMap.Current.Frame - Anim.FFrameCounter;
                switch ((int)GameTables.AoWEngine.FAnimationDetail)
                {
                    case 0:
                        Count = 5;
                        break;
                    case 2:
                        Count = 20;
                        break;
                    default:
                        Count = 10;
                        break;
                }

                for (int I = 0; I <= Count - 1; I++)
                {
                    X = Random(Anim.FTag & 65535);
                    Y = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
                    Speed = Random(15) + 5;
                    Image = Random(3);
                    X += (Anim.FFrameCounter - 45) * Speed * 2;
                    Y -= (Anim.FFrameCounter - 45) * Speed;
                    FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
                }
            }

            if (Anim.FFrameCounter == 90)
            {
                Anim.Dispose();
            }
        }
    }

    public partial class WaterMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:760
        public new const int ClassID = 135956;

        // GlobalEnchantmentSpells.pas:752
        public WaterMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.WaterMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:765
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:791
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
            Aow.Game.World.Map.AoWHSMap.Current.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Fire);
            return true;
        }

        // GlobalEnchantmentSpells.pas:770
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
                {
                    var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                    var name = GetName();
                    Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:781
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
            Aow.Game.World.Map.AoWHSMap.Current.UpdateVisibility();
            // { Original uses Fire for Water Mastery. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Fire);
            return true;
        }

    }

    public partial class WaterMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:806
        public WaterMastery(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.WaterMasteryRStr);
            FSpellID = 45;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Water;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:819
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            // { Original ignores the event step. }
            base.ExecuteTE(Event, Source, 0);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Water);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            FCastSounds.Play(0);
            Aow.Game.World.Map.AoWHSMap.Current.AddEffect(TworldFXType.WaterMastery);
            CastingDone(Source, FBaseManaCost);
            return false;
        }

        // GlobalEnchantmentSpells.pas:801
        public override Magic.GlobalEnchantment CreateEnchantment() => new WaterMasteryEnchantment(null);
    }

    // FDaysActive
    public partial class HatredEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:841
        public new const int ClassID = 135957;

        public int FDaysActive;

        // GlobalEnchantmentSpells.pas:851
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("DaysActive", ref FDaysActive);
        }

        // GlobalEnchantmentSpells.pas:833
        public HatredEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HatredRStr);
            FEnchantmentType = GlobalEnchantmentType.Hatred;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:863
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
            {
                NewDay();
            }
        }

        // GlobalEnchantmentSpells.pas:846
        public override int GetAIUpkeepPriority() => 500;

        // GlobalEnchantmentSpells.pas:876
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            if (FDaysActive > 10)
            {
                AoWHSMap.Current.FRaces.FRelationOffset += 40;
            }
            else
            {
                AoWHSMap.Current.FRaces.FRelationOffset += FDaysActive * 4;
            }

            return true;
        }

        // GlobalEnchantmentSpells.pas:869
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            NewDay();
            return true;
        }

        // GlobalEnchantmentSpells.pas:857
        public void NewDay()
        {
            FDaysActive += 1;
            if (FDaysActive <= 10)
            {
                AoWHSMap.Current.FRaces.FRelationOffset -= 4;
            }
        }

    }

    public partial class Hatred : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:889
        public Hatred(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HatredRStr);
            FSpellID = 46;
            FSpellType = SpellType.Global;
            FBaseManaCost = 20;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Death;
        }

        // GlobalEnchantmentSpells.pas:900
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Aow.Game.World.Map.AoWHSMap.Current.AddEffect(TworldFXType.Hatred);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:884
        public override Magic.GlobalEnchantment CreateEnchantment() => new HatredEnchantment(null);
    }

    // FDaysActive
    public partial class HarmonyEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:921
        public new const int ClassID = 135958;

        public int FDaysActive;

        // GlobalEnchantmentSpells.pas:931
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDaysActive, 20);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("DaysActive", ref FDaysActive);
        }

        // GlobalEnchantmentSpells.pas:913
        public HarmonyEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HarmonyRStr);
            FEnchantmentType = GlobalEnchantmentType.Harmony;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:943
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
            {
                NewDay();
            }
        }

        // GlobalEnchantmentSpells.pas:926
        public override int GetAIUpkeepPriority() => 500;

        // GlobalEnchantmentSpells.pas:956
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            if (FDaysActive > 10)
            {
                AoWHSMap.Current.FRaces.FRelationOffset -= 40;
            }
            else
            {
                AoWHSMap.Current.FRaces.FRelationOffset -= FDaysActive * 4;
            }

            return true;
        }

        // GlobalEnchantmentSpells.pas:949
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            NewDay();
            return true;
        }

        // GlobalEnchantmentSpells.pas:937
        public void NewDay()
        {
            FDaysActive += 1;
            if (FDaysActive <= 10)
            {
                AoWHSMap.Current.FRaces.FRelationOffset += 4;
            }
        }

    }

    public partial class Harmony : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:969
        public Harmony(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HarmonyRStr);
            FSpellID = 47;
            FSpellType = SpellType.Global;
            FBaseManaCost = 15;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Life;
        }

        // GlobalEnchantmentSpells.pas:980
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Aow.Game.World.Map.AoWHSMap.Current.AddEffect(TworldFXType.Tranquility);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:964
        public override Magic.GlobalEnchantment CreateEnchantment() => new HarmonyEnchantment(null);
    }

    public partial class PowerLeakEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1001
        public new const int ClassID = 135959;

        // GlobalEnchantmentSpells.pas:993
        public PowerLeakEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.PowerLeakRStr);
            FEnchantmentType = GlobalEnchantmentType.PowerLeak;
            FDispelMana = 25;
        }

        // GlobalEnchantmentSpells.pas:1006
        public override int GetAIUpkeepPriority() => 750;

        // GlobalEnchantmentSpells.pas:1021
        public override bool Deactivate(byte Player)
        {
            base.Deactivate(Player);
            for (int I = AoWHSMap.Current.FPlayers.Count - 1; I >= 0; I--)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.Update();
            }

            return true;
        }

        // GlobalEnchantmentSpells.pas:1011
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            for (int I = AoWHSMap.Current.FPlayers.Count - 1; I >= 0; I--)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.Update();
            }

            return true;
        }

    }

    public partial class PowerLeak : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1036
        public PowerLeak(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.PowerLeakRStr);
            FSpellID = 48;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Cosmos;
            FCastDuration = 20;
            FAIBasePriority = 750;
        }

        // GlobalEnchantmentSpells.pas:1049
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TwinkleAnimation Anim;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Anim = new TwinkleAnimation(null);
            Anim.Setup(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(48).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Implode), 100, 64, 50, 30);
            Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1031
        public override Magic.GlobalEnchantment CreateEnchantment() => new PowerLeakEnchantment(null);
    }

    // FWidth, FHeight, FOriginalTerrain
    public partial class FloodControl : EObject
    {
        public int FWidth;
        public int FHeight;
        public sbyte[] FOriginalTerrain = [];

        // GlobalEnchantmentSpells.pas:1072
        public override void Dispose()
        {
            if (FOriginalTerrain.Length != 0)
            {
                Array.Resize(ref FOriginalTerrain, 0);
            }

            base.Dispose();
        }

        // GlobalEnchantmentSpells.pas:1078
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FWidth, 20);
            Stream.rwInteger(ref FHeight, 21);
            if (FWidth != 0)
            {
                if (Stream.StorageMode().Contains(StorageModeElement.Read))
                {
                    Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
                }

                Stream.rwData(MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, FWidth * FHeight * 2)), FWidth * FHeight * 2, 22);
            }
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("Width", ref FWidth);
            persister.Int("Height", ref FHeight);
            if (FWidth != 0)
            {
                int count = FWidth * FHeight * 2;
                if (persister.Loading) Array.Resize(ref FOriginalTerrain, count);
                persister.Bytes("OriginalTerrain", MemoryMarshal.AsBytes(FOriginalTerrain.AsSpan(0, count)));
            }
        }

        // GlobalEnchantmentSpells.pas:1066
        public FloodControl(ECustomNode owner) : base(owner)
        {
            FOriginalTerrain = [];
        }

        // GlobalEnchantmentSpells.pas:1090
        public void Setup()
        {
            FWidth = AoWHSMap.Current.MapContainer.Width;
            FHeight = AoWHSMap.Current.MapContainer.Height;
            Array.Resize(ref FOriginalTerrain, FWidth * FHeight * 2);
            Array.Clear(FOriginalTerrain);
        }

        // GlobalEnchantmentSpells.pas:1098
        public void Flood()
        {
            MapLevel MapLevel;
            HNWalkData Walk = default;
            MapField MapF;
            int Index;
            HexagonSprite RoadHS;
            sbyte TerrainType;
            Aow.Game.World.Map.AoWHSMap.Current.LockMapChangedEvent();
            MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(0);
            // { Original includes the last row when walking neighbouring hexagons. }
            for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if (MapLevel.GetField(X, Y).TerrainType == 0)
                    {
                        Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, 1);
                        while (Walk.Radius == 1)
                        {
                            MapF = MapLevel.GetField(Walk.X, Walk.Y);
                            if ((!Set<byte>.WithBounds(0, 255).With(0).With(6).With(15).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).With(2).With(7).Contains((byte)MapF.OverlayType)) || (MapF.TerrainType == 0 && MapF.OverlayType == 5))
                            {
                                Index = (FWidth * Walk.Y + Walk.X) * 2;
                                if (FOriginalTerrain[Index] == 0 && FOriginalTerrain[Index + 1] == 0 && MapF.FindNoneTransparentHS(typeof(Structure)) == null)
                                {
                                    FOriginalTerrain[Index] = MapF.TerrainType;
                                    FOriginalTerrain[Index + 1] = MapF.OverlayType;
                                }

                                if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)MapF.OverlayType))
                                {
                                    RoadHS = MapF.FindOwnedHS(typeof(AbstractRoad));
                                    if (RoadHS != null)
                                    {
                                        RoadHS.Dispose();
                                    }
                                }
                            }

                            Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                        }
                    }
                }
            }

            for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
            {
                Index = (FWidth * Y + 1) * 2;
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if (FOriginalTerrain[Index] != 0)
                    {
                        MapF = MapLevel.GetField(X, Y);
                        TerrainType = MapF.TerrainType;
                        if (TerrainType != 0)
                        {
                            MapF.TerrainType = 0;
                            MapF.TerrainChanged();
                        }
                    }

                    Index += 2;
                }
            }

            Aow.Game.World.Map.AoWHSMap.Current.UnlockMapChangedEvent();
            Aow.Game.World.Map.AoWHSMap.Current.InvalidateMap();
        }

        // GlobalEnchantmentSpells.pas:1160
        public void Restore()
        {
            EObjectList Resources;
            DelphiList Candidates;
            int Index;
            MapField MapF;
            MapLevel MapLevel;
            sbyte Overlay;
            MapObjectResource Resource;
            Aow.Game.World.Map.AoWHSMap.Current.LockMapChangedEvent();
            MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(0);
            for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
            {
                Index = (FWidth * Y + 1) * 2;
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if (FOriginalTerrain[Index] != 0)
                    {
                        MapF = MapLevel.GetField(X, Y);
                        if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)MapF.TerrainType))
                        {
                            MapF.TerrainType = FOriginalTerrain[Index];
                            MapF.TerrainChanged();
                        }
                    }

                    Index += 2;
                }
            }

            Resources = AoWHSSet.Current.MapObjectResourceList.FChildren;
            Candidates = new DelphiList();
            for (int Y = 1, last = MapLevel.Height - 1; Y <= last; Y++)
            {
                Index = (FWidth * Y + 1) * 2;
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    Overlay = FOriginalTerrain[Index + 1];
                    if (Overlay > 0)
                    {
                        MapF = MapLevel.GetField(X, Y);
                        if (Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)Overlay))
                        {
                            Aow.Game.Engine.AoWHSSet.Current.RoadControl.Place(AoWHSMap.Current.MapContainer, (sbyte)X, (sbyte)Y, 0, MapF.TerrainType);
                        }
                        else
                        {
                            Candidates.Clear();
                            for (int I = 0, lastI = Resources.Count - 1; I <= lastI; I++)
                            {
                                Resource = (MapObjectResource)Resources.List[I];
                                if (Resource.EClass.ClassID == 131431 && Resource.TerrainList.UsedTerrain() == 1 && Resource.TerrainList.Terrain[0] == MapF.TerrainType && Overlay == Resource.TerrainList.Overlay[0])
                                {
                                    Candidates.Add(Resource);
                                }
                            }

                            if (Candidates.Count != 0)
                            {
                                ((MapObjectResource)Candidates[Aow.Game.World.Map.AoWHSMap.Current.Random(Candidates.Count)]).PlaceResourceUser(AoWHSMap.Current, (sbyte)X, (sbyte)Y, 0);
                            }
                        }
                    }

                    Index += 2;
                }
            }

            Candidates?.Dispose();
            Aow.Game.World.Map.AoWHSMap.Current.UnlockMapChangedEvent();
            Aow.Game.World.Map.AoWHSMap.Current.InvalidateMap();
        }

    }

    // FRemoved, FRestoreDays, FDispelPlayer, FTurnsActive, FFloodControl
    public partial class FloodEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1240
        public new const int ClassID = 135960;

        public bool FRemoved;
        public sbyte FRestoreDays;
        public sbyte FDispelPlayer;
        // alignment
        public int FTurnsActive;
        public FloodControl FFloodControl;

        // GlobalEnchantmentSpells.pas:1234
        public override void Dispose()
        {
            FFloodControl?.Dispose();
            base.Dispose();
        }

        // GlobalEnchantmentSpells.pas:1250
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTurnsActive, 20);
            Stream.rwEObject(FFloodControl, 21);
            Stream.rwBool(ref FRemoved, 22);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDispelPlayer), 23);
            Stream.rwShortintEx(ref FRestoreDays, -1, 24);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("TurnsActive", ref FTurnsActive);
            persister.Object("FloodControl", FFloodControl);
            persister.Bool("Removed", ref FRemoved);
            PersistSupport.UnsignedByte(persister, "DispelPlayer", ref FDispelPlayer);
            persister.SByte("RestoreDays", ref FRestoreDays, -1);
        }

        // GlobalEnchantmentSpells.pas:1224
        public FloodEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FloodRStr);
            FEnchantmentType = GlobalEnchantmentType.Flood;
            FFloodControl = new FloodControl(null);
            FDispelMana = 100;
            FRestoreDays = -1;
        }

        // GlobalEnchantmentSpells.pas:1302
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if (Msg.Payload.Int == FPlayerIndex && !FRemoved)
                {
                    NewTurn();
                }

                if (Msg.Payload.Int == FDispelPlayer && FRemoved)
                {
                    FRestoreDays = (sbyte)(FRestoreDays - 1);
                    if (FRestoreDays <= 0)
                    {
                        FFloodControl.Restore();
                        base.Deactivate((byte)FDispelPlayer);
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1268
        public override string GetName()
        {
            string Result = base.GetName();
            if (FRemoved)
            {
                Result = Result + " (" + LowerCase(Aow.Game.Magic.Abilities.DurationText.FmtDaysLeftStr(FRestoreDays)) + ")";
            }
            else
            {
                if (DaysBeforeActive() != 0)
                {
                    var daysBeforeActive = DaysBeforeActive();
                    Result = RtlUnit.Format(XinYdaysRStr, [Result, daysBeforeActive]);
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1276
        public override int GetUpkeep()
        {
            int Result;
            if (!FRemoved)
            {
                Result = base.GetUpkeep();
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1245
        public override int GetAIUpkeepPriority() => 500;

        // GlobalEnchantmentSpells.pas:1328
        public override bool CanDeactivate(byte Player, ref string Reason)
        {
            bool Result;
            if (!FRemoved)
            {
                Result = base.CanDeactivate(Player, ref Reason);
            }
            else
            {
                Reason = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(XAlreadyRemovedRStr), [FName]);
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1338
        public override bool Deactivate(byte Player)
        {
            bool Result;
            if (!FRemoved)
            {
                FDispelPlayer = (sbyte)Player;
                FRemoved = true;
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
                {
                    FRestoreDays = 2;
                }
                else
                {
                    FRestoreDays = 1;
                }

                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.Update();
                Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.Update();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1320
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            FRemoved = false;
            FFloodControl.Setup();
            return true;
        }

        // GlobalEnchantmentSpells.pas:1260
        public int DaysBeforeActive()
        {
            int Result;
            if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && FTurnsActive < 2)
            {
                Result = 2 - FTurnsActive;
            }
            else
            {
                if (AoWHSMap.Current.FPlayMode == PlayMode.Turn && FTurnsActive == 0)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1282
        public void NewTurn()
        {
            if (!FRemoved)
            {
                FTurnsActive += 1;
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
                {
                    if (FTurnsActive > 1 && FTurnsActive < 4)
                    {
                        FFloodControl.Flood();
                    }
                    else
                    {
                        if (FTurnsActive == 4)
                        {
                            if (Aow.Game.World.Map.AoWHSMap.Current.Random(2) == 1)
                            {
                                FFloodControl.Flood();
                            }
                        }
                    }
                }
                else
                {
                    if (AoWHSMap.Current.FPlayMode == PlayMode.Turn)
                    {
                        if (FTurnsActive < 3)
                        {
                            FFloodControl.Flood();
                        }
                        else
                        {
                            if (FTurnsActive == 3)
                            {
                                if (Aow.Game.World.Map.AoWHSMap.Current.Random(2) == 1)
                                {
                                    FFloodControl.Flood();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static readonly string XAlreadyRemovedRStr = "%s already removed";
        private static readonly string XinYdaysRStr = "%s in %d day(s)";

    }

    public partial class Flood : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1356
        public Flood(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FloodRStr);
            FSpellID = 50;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Water;
        }

        // GlobalEnchantmentSpells.pas:1370
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TwinkleAnimation Twinkle;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(50).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1353
        public override Magic.GlobalEnchantment CreateEnchantment() => new FloodEnchantment(null);

        // GlobalEnchantmentSpells.pas:1367
        public void ShowAnimation(int Anim, int XhpSurf, int YhpSurf, int SurfaceDesc)
        {
        }
    }

    public partial class LifeMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1395
        public new const int ClassID = 135967;

        // GlobalEnchantmentSpells.pas:1387
        public LifeMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1398
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:1428
        public override bool Deactivate(byte Player)
        {
            EMsg Msg = default;
            base.Deactivate(Player);
            // { Original doubles Death. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Death);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));
            Aow.Game.Engine.Nodes.EMsg.InitMsg(ref Msg, 537002240, this);
            Aow.Game.World.Map.AoWHSMap.Current.SendMsg(ref Msg, this);
            return true;
        }

        // GlobalEnchantmentSpells.pas:1401
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
                {
                    var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                    var name = GetName();
                    Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1412
        public override bool Activate(sbyte Player)
        {
            Player APlayer;
            base.Activate(Player);
            // { Original doubles Death. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Death);
            APlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));

            for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
            {
                for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
                {
                    APlayer.FArmies.GetArmy(I).GetUnit(J).SetLifeMasteryBlessing();
                }
            }

            return true;
        }

    }

    public partial class LifeMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1441
        public LifeMastery(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
            FSpellID = 61;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Life Mastery. }
            FMagicSphere = MagicSphere.Cosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1457
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TwinkleAnimation Twinkle;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Life);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(61).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
            Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1454
        public override Magic.GlobalEnchantment CreateEnchantment() => new LifeMasteryEnchantment(null);
    }

    public partial class DeathMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1483
        public new const int ClassID = 135968;

        // GlobalEnchantmentSpells.pas:1475
        public DeathMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.DeathMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1568
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                if (Msg.Payload.Int != 0)
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FEnemyEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                    {
                        NewTurn((sbyte)Msg.Payload.Int);
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1486
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:1509
        public override bool Deactivate(byte Player)
        {
            EMsg Msg = default;
            base.Deactivate(Player);
            // { Original doubles Life. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Life);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
            Aow.Game.Engine.Nodes.EMsg.InitMsg(ref Msg, 537002241, this);
            Aow.Game.World.Map.AoWHSMap.Current.SendMsg(ref Msg, this);
            return true;
        }

        // GlobalEnchantmentSpells.pas:1489
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                {
                    var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                    var name = GetName();
                    Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1500
        public override bool Activate(sbyte Player)
        {
            base.Activate(Player);
            // { Original doubles Life. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Life);
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
            return true;
        }

        // GlobalEnchantmentSpells.pas:1522
        public void NewTurn(sbyte Player)
        {
            int Index;
            byte[] Flags;
            int Mask;
            MapLevel MapLevel;
            HNWalkData Walk = default;
            bool Border;
            Flags = [];
            Mask = 1 << (Player - 1);
            for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
            {
                MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L);
                Index = 0;
                Array.Resize(ref Flags, MapLevel.Width * MapLevel.Height);
                Array.Clear(Flags);
                for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
                {
                    for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                    {
                        if ((((AoWMapField)MapLevel.GetField(X, Y)).FFieldFlags & Mask) != 0)
                        {
                            Border = false;
                            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, 1);
                            while (Walk.Radius == 1 && !Border)
                            {
                                if (Walk.X >= 0 && Walk.X < MapLevel.Width && Walk.Y >= 0 && Walk.Y < MapLevel.Height)
                                {
                                    Border = (((AoWMapField)MapLevel.GetField(Walk.X, Walk.Y)).FFieldFlags & Mask) == 0;
                                }

                                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                            }

                            if (Border)
                            {
                                Flags[Index] = 1;
                            }
                        }

                        Index += 1;
                    }
                }

                Index = 0;
                for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
                {
                    for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                    {
                        if (Flags[Index] != 0)
                        {
                            ((AoWMapField)MapLevel.GetField(X, Y)).SetExplored(Player, false);
                        }

                        Index += 1;
                    }
                }
            }

            Aow.Game.World.Map.AoWHSMap.Current.UpdateVisibility();
            Array.Resize(ref Flags, 0);
        }

    }

    public partial class DeathMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1577
        public DeathMastery(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.DeathMasteryRStr);
            FSpellID = 62;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            // { Original assigns Cosmos to Death Mastery. }
            FMagicSphere = MagicSphere.Cosmos;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1618
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Death);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            var animation = new Animation(null);
            animation.FAnimationLevel = 28;
            animation.FOnShow = ShowAnimation;
            animation.FAnimationMap = AoWHSMap.Current;
            animation.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            animation.Activate();

            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1590
        public override Magic.GlobalEnchantment CreateEnchantment() => new DeathMasteryEnchantment(null);

        // GlobalEnchantmentSpells.pas:1593
        public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Count;
            int X;
            int Y;
            int Speed;
            int D;
            int Image;
            RandSeed = AoWHSMap.Current.Frame - Anim.FFrameCounter;
            switch (GameTables.AoWEngine.FAnimationDetail)
            {
                case 0:
                    Count = 5;
                    break;
                case 2:
                    Count = 20;
                    break;
                default:
                    Count = 10;
                    break;
            }

            for (int I = 0; I <= Count - 1; I++)
            {
                X = Random(Anim.FTag & 65535);
                Y = Random((int)(((uint)(int)(uint)Anim.FTag) >> 16));
                Speed = Random(15) + 5;
                Image = Random(3);
                // { Preserve the original image/displacement register lifetimes. }
                D = (Anim.FFrameCounter - 45) * Speed;
                X -= D * 2;
                Y += D;
                FCastImages.Show(Image, SurfaceDesc.ClipRect.Left + X, SurfaceDesc.ClipRect.Top + Y, 0, SurfaceDesc);
            }

            if (Anim.FFrameCounter == 90)
            {
                Anim.Dispose();
            }
        }
    }

    public partial class FireMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1647
        public new const int ClassID = 135969;

        // GlobalEnchantmentSpells.pas:1639
        public FireMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FireMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.FireMastery;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1724
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987)
            {
                if (Msg.Payload.Int != 0)
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
                    {
                        UpdateFire(2, !AoWHSMap.Current.FRequestCombatType);
                    }
                }
            }
        }

        // GlobalEnchantmentSpells.pas:1650
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:1680
        public override bool Deactivate(byte Player)
        {
            Player APlayer;
            base.Deactivate(Player);
            APlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

            for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
            {
                for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
                {
                    APlayer.FArmies.GetArmy(I).GetUnit(J).ResetFireMasteryProtection();
                }
            }

            // { Original doubles Water. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Water);
            return true;
        }

        // GlobalEnchantmentSpells.pas:1653
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result;
            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
            {
                var xIsAlreadyActive = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1663
        public override bool Activate(sbyte Player)
        {
            Player APlayer;
            base.Activate(Player);
            // { Original doubles Water. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Water);
            APlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

            for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
            {
                for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
                {
                    APlayer.FArmies.GetArmy(I).GetUnit(J).SetFireMasteryProtection();
                }
            }

            UpdateFire(15, true);
            return true;
        }

        // GlobalEnchantmentSpells.pas:1696
        public void UpdateFire(int Factor, bool Explosions)
        {
            int Chance;
            MapLevel MapLevel;
            Fire Fire;
            RandSeed = Aow.Game.World.Map.AoWHSMap.Current.Random(16777215);
            for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
            {
                MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L);
                for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
                {
                    for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                    {
                        Chance = Aow.Game.World.Map.AoWMapField.IgnitionChance((AoWMapField)MapLevel.GetField(X, Y));
                        if (Chance > 0)
                        {
                            if (Aow.Game.World.Map.AoWHSMap.Current.Random(10000) < Chance * Factor)
                            {
                                Fire = Aow.Game.Engine.AoWHSSet.Current.FFireControl.PlaceFire(X, Y, L, FireType.Large);
                                if (Fire != null)
                                {
                                    if (Explosions)
                                    {
                                        Fire.SetupExplosion(Random(30));
                                    }

                                    Fire.FTurnsLeft = 3;
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    public partial class FireMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1733
        public FireMastery(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FireMasteryRStr);
            FSpellID = 63;
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Fire;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1792
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            if (ExecuteCount == 0)
            {
                Source.GetLocation(ref X, ref Y, ref L);
                Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Fire);
                Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
                var animation = new Animation(null);
                animation.FAnimationLevel = 27;
                animation.FOnShow = ShowAnimation;
                animation.FAnimationMap = AoWHSMap.Current;
                animation.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
                animation.Activate();

                FCastSounds.Play(0);
                CastingDone(Source, Event.FEventMana);
            }

            if (ExecuteCount == 20)
            {
                base.ExecuteTE(Event, Source, 0);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1746
        public override Magic.GlobalEnchantment CreateEnchantment() => new FireMasteryEnchantment(null);

        public void ShowAnimation(Animation anim, int xhpSurface, int yhpSurface, in SurfaceDesc surface)
        {
            int chance = GameTables.AoWEngine.FAnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
            var map = AoWHSMap.Current;
            var level = map.MapContainer.GetMapLevel(map.ViewL);
            int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
            for (int x = map.ViewXhx; x <= maxX; x++)
            {
                int xhp = 0, yhp = 0;
                Aow.Game.World.Map.HexGeometry.HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
                xhp = xhp - xhpSurface + 16;
                yhp = yhp - yhpSurface + 16;
                for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
                {
                    var field = (AoWMapField)level.GetField(x, y);
                    if (field.TerrainType is 7 or 8 or 15 || map.FSeatedPlayer == -1 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) continue;
                    RandSeed = field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255) * 65536;
                    if (Random(100) >= chance) continue;
                    int delay = Random(40);
                    FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - delay, surface);
                    if (anim.FFrameCounter == delay && GameTables.AoWEngine.FAnimationDetail != 0)
                        Aow.Game.Audio.AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[x % 2]);
                }
            }
            if (anim.FFrameCounter == 90) anim.Dispose();
        }
    }

    public partial class EarthMasteryEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1828
        public new const int ClassID = 135970;

        // GlobalEnchantmentSpells.pas:1820
        public EarthMasteryEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EarthMasteryRStr);
            FEnchantmentType = GlobalEnchantmentType.None;
            FDispelMana = 175;
        }

        // GlobalEnchantmentSpells.pas:1831
        public override int GetAIUpkeepPriority() => 1250;

        // GlobalEnchantmentSpells.pas:1861
        public override bool Deactivate(byte Player)
        {
            Player APlayer;
            base.Deactivate(Player);
            APlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

            for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
            {
                for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
                {
                    APlayer.FArmies.GetArmy(I).GetUnit(J).ResetEarthMasteryStoneSkin();
                }
            }

            // { Original doubles Air. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Air);
            return true;
        }

        // GlobalEnchantmentSpells.pas:1834
        public override bool CanActivate(sbyte Player, ref string Reason)
        {
            bool Result = base.CanActivate(Player, ref Reason);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EarthMastery))
                {
                    Reason = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr), [FName]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalEnchantmentSpells.pas:1845
        public override bool Activate(sbyte Player)
        {
            Player APlayer;
            base.Activate(Player);
            // { Original doubles Air. }
            Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Air);
            APlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
            var playerMagicControl = APlayer.FMagicControl;
            playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

            for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
            {
                for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
                {
                    APlayer.FArmies.GetArmy(I).GetUnit(J).SetEarthMasteryStoneSkin();
                }
            }

            return true;
        }

    }

    public partial class EarthMastery : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1877
        public EarthMastery(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EarthMasteryRStr);
            FSpellID = 64;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Earth;
            FCastDuration = 40;
            FAIBasePriority = 1000;
        }

        // GlobalEnchantmentSpells.pas:1918
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Aow.Game.Magic.Spells.GlobalTarget.SpellTargets.NotifySpellSphere(Source.FPlayer, MagicSphere.Earth);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Aow.Game.World.Map.AoWHSMap.Current.AddEffect(TworldFXType.EarthMastery);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1932
        public override Magic.GlobalEnchantment CreateEnchantment() => new EarthMasteryEnchantment(null);

        // GlobalEnchantmentSpells.pas:1890
        public void ShowAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int MaxX;
            int MaxY;
            int Xhp = default;
            int Yhp = default;
            MapLevel MapLevel;
            AoWMapField MapF;
            MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(AoWHSMap.Current.ViewL);
            MaxX = AoWHSMap.Current.ViewXhx + AoWHSMap.Current.ViewWidthHX - 1;
            MaxY = AoWHSMap.Current.ViewYhx + AoWHSMap.Current.ViewHeightHX - 1;
            for (int X = AoWHSMap.Current.ViewXhx; X <= MaxX; X++)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(X, AoWHSMap.Current.ViewYhx, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf + 16;
                Yhp = Yhp - YhpSurf + 16;
                MapF = (AoWMapField)MapLevel.GetField(X, AoWHSMap.Current.ViewYhx);
                for (int Y = AoWHSMap.Current.ViewYhx; Y <= MaxY; Y++)
                {
                    // The original frame offset uses the field address.
                    if (MapF.OverlayType == 4)
                    {
                        FCastImages.Show(0, Xhp, Yhp, Anim.FFrameCounter - FrameFixtureInputs.FieldFrameOffset(MapF), SurfaceDesc);
                    }

                    // The original steps the field pointer by FHNOffsets[0, 4], one row down. The step
                    // after the last row is never read there, and the guard keeps it from reading here.
                    if (Y < MaxY)
                    {
                        MapF = (AoWMapField)MapLevel.GetField(X, Y + 1);
                    }

                    Yhp += 32;
                }
            }

            if (Anim.FFrameCounter == 50)
            {
                Anim.Dispose();
            }
        }
    }

    public partial class SpellWardEnchantment : Magic.GlobalEnchantment
    {
        // GlobalEnchantmentSpells.pas:1943
        public new const int ClassID = 135971;

        // GlobalEnchantmentSpells.pas:1935
        public SpellWardEnchantment(ECustomNode owner) : base(owner)
        {
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.SpellWardRStr);
            FEnchantmentType = GlobalEnchantmentType.SpellWard;
            FDispelMana = 50;
        }

        // GlobalEnchantmentSpells.pas:1946
        public override int GetAIUpkeepPriority() => 500;

    }

    public partial class SpellWard : GlobalEnchantmentSpell
    {
        // GlobalEnchantmentSpells.pas:1949
        public SpellWard(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.SpellWardRStr);
            FSpellID = 65;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Earth;
        }

        // GlobalEnchantmentSpells.pas:1960
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref L);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            Aow.Game.World.Map.AoWHSMap.Current.AddEffect(TworldFXType.SpellWard);
            FCastSounds.Play(0);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // GlobalEnchantmentSpells.pas:1973
        public override Magic.GlobalEnchantment CreateEnchantment() => new SpellWardEnchantment(null);
    }
}
