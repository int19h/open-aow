// Translated from AoWProdScreen.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWProdScreen.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// view, place, production, queue, mode, preview, images
public partial class TProductionScreen : TDataModule
{
    public TProductionScreen(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWPanel UnitPnl;
    public TAOWListBox UAbilitiesLB;
    public TAOWImage UAttackIcon;
    public TAOWImage UDefenseIcon;
    public TAOWImage UMovesIcon;
    public TAOWImage UDamageIcon;
    public TAOWImage UResistanceIcon;
    public TAOWImage UHitsIcon;
    public TAOWLabel UAttack;
    public TAOWLabel UDefense;
    public TAOWLabel UMoves;
    public TAOWLabel UDamage;
    public TAOWLabel UResistance;
    public TAOWLabel UHits;
    public TAOWPanel UStatPnl;
    public TAOWPanel UStatP1;
    public TAOWPanel UStatP2;
    public TAOWLabel UAlignment;
    public TAOWLabel ULevel;
    public TAOWPanel UPicturePnl;
    public TAOWPanel UCostPnl;
    public TAOWLabel StructName;
    public TAOWLabel CostLbl;
    public TAOWLabel UCost;
    public TAOWLabel UnitLbl;
    public TAOWLabel TurnsLbl;
    public TAOWLabel UTurns;
    public TAOWCheckBox B4CB;
    public TAOWCheckBox B5CB;
    public TAOWCheckBox B6CB;
    public TAOWCheckBox B1CB;
    public TAOWCheckBox B2CB;
    public TAOWCheckBox B3CB;
    public TAOWVScrollBar T2SBar;
    public TAOWPanel AbilPnl;
    public TAOWPanel UpgradePnl;
    public TAOWLabel UpgradeLbl;
    public TAOWPanel UpgradeMemoFr;
    public TAOWMemo UpgradeMemo;
    public TAOWPanel RightPnl;
    public TAOWPanel QueuePnl;
    public TAOWImage Oval;
    public TAOWImage ShadowL;
    public TAOWImage ShadowR;
    public TAOWButton MainBtn3;
    public TAOWButton MainBtn1;
    public TAOWListBox QueueLB;
    public TAOWPanel ActivityPnl;
    public TAOWPanel InfoPnl2;
    public TAOWLabel ActiveAction;
    public TAOWLabel ActiveActionTurns;
    public TAOWImage SepBar;
    public TAOWPanel ActivityPnl2;
    public TAoWGauge ActiveActionTurnsBar;
    public TAOWButton MainBtn2;
    public TAOWPanel InfoPnl;
    public TAOWPanel IPPnl;
    public TAoWFrame IPPicFr;
    public TAOWMemo PlaceInfoMemo;
    public TAOWPanel IIPnl;
    public TAoWFrame IncomePicture;
    public TAOWMemo IncomeMemo;
    public TAOWMemo IncomeHdr;
    public TAOWPanel CurrentInfo;
    public TAoWFrame RPillar;
    public TAoWFrame LPillar;
    public TAOWPanel QueueInfo;
    public TAOWLabel QueueLbl;
    public TAOWCheckBox LoopQueueBox;
    public TAOWLabel RepeatLbl;
    public TAOWPanel ActiveActionPnl;
    public TAOWImage CurrentActionImage;
    public TAoWFrame AoWFrame1;
    public TAOWImage BlackBg2;
    public TAOWPanel UPictureFramePnl;
    public TAOWPanel UStatFr1;
    public TAOWPanel UStatFr2;
    public TAOWPanel UInfoFr;
    public TAOWPanel CostFr;
    public TAOWPanel TurnsFr;
    public TAOWPanel UpgPicPnl;
    public TAOWImage UpgradePic;
    public TAOWPanel UpgPicFr;
    public TAOWPanel UpgCostPnl;
    public TAOWLabel UpgCostLbl;
    public TAOWLabel UpgradeCost;
    public TAOWLabel UpgTurnsLbl;
    public TAOWLabel UpgradeTurns;
    public TAOWPanel UpgCostFr;
    public TAOWPanel UpgTurnsFr;
    public TAOWImage RelIcn;
    public TAOWVScrollBar QueueVSB;
    public TAOWVScrollBar IPVSB;
    public TAOWVScrollBar IIVSB;
    public TAOWMemo IncomeValuesMemo;
    public TaowUserDrawBox UFace;
    public TAOWImage InfoSep;
    public TAOWButton Sphere;
    public TaowUserDrawBox MapView;
    public TAOWButton PNext;
    public TAOWButton PPrev;
    public TIvTranslator Translator;
    public int FViewX;
    public int FViewY;
    public TProductionPlace FPlace;
    public int FProductionIndex;
    public int FQueueIndex;
    public byte FMode;
    public TAoWCombatMap FPreview;
    public TIntegerList FImages;

    // AoWProdScreen.pas:590
    public void FreePreview()
    {
        if (FPreview != null)
        {
            FPreview?.Dispose();
            FPreview = null;
        }
    }

    // AoWProdScreen.pas:599
    public void ReleaseImages()
    {
        for (int I = FImages.Count - 1; I >= 0; I--)
        {
            var image = FImages[I];
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(image).Release();
        }

        FImages.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int Index)
    {
        int Gfx;
        Gfx = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitGFXResourceIndex;
        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx).AddRef();
        FImages.Add(Gfx);
    }

    // AoWProdScreen.pas:617
    public void ResetChecks()
    {
        B1CB.Checked = false;
        B2CB.Checked = false;
        B3CB.Checked = false;
        B4CB.Checked = false;
        B5CB.Checked = false;
        B6CB.Checked = false;
    }

    // AoWProdScreen.pas:627
    public void Reset()
    {
        ReleaseImages();
        FMode = 0;
        InfoPnl.Visible = true;
        InfoPnl2.Visible = true;
        ActivityPnl.Visible = false;
        ActivityPnl2.Visible = false;
        UnitPnl.Visible = false;
        UpgradePnl.Visible = false;
        MainBtn1.Visible = true;
        var options = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.OptionsRStr);
        MainBtn1.Text = options;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        ShadowL.Visible = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0;
        ShadowR.Visible = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0;
        Oval.Visible = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0;
        FQueueIndex = -1;
        QueueLB.Selected = -1;
        var queue = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.QueueRStr);
        QueueLbl.Text = queue;
        var repeat = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RepeatRStr);
        RepeatLbl.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        FPlace = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWProdScreen.pas:659
    public void SetMode(byte Mode)
    {
        FMode = Mode;
        switch (FMode)
        {
            case 0:
                InfoPnl.Visible = true;
                InfoPnl2.Visible = true;
                ActivityPnl.Visible = false;
                ActivityPnl2.Visible = false;
                var change = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ChangeRStr);
                MainBtn1.Text = change;
                MainBtn2.Visible = false;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                var exit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ExitRStr);
                MainBtn3.Text = exit;
                QueueLB.Selected = -1;
                break;
            case 1:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var info = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.InfoRStr);
                MainBtn1.Text = info;
                var done = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DoneRStr);
                MainBtn3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var infoText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.InfoRStr);
                MainBtn1.Text = infoText;
                var doneText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DoneRStr);
                MainBtn3.Text = doneText;
                break;
        }
    }

    // AoWProdScreen.pas:698
    public void SelectDefaultAction()
    {
        ResetChecks();
        B1CB.Checked = true;
        SelectProduction(0);
    }

    // AoWProdScreen.pas:705
    public void ProductionEvent(TProductionPlaceEventLog Event)
    {
        ShowPlace(Event.GetProductionPlace());
        SetMode(1);
        SelectDefaultAction();
    }

    // AoWProdScreen.pas:712
    public void PlaceChanged(object Sender)
    {
        if (Dlg.Visible && Sender == FPlace)
        {
            if (FPlace.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                UpdateInfo();
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWProdScreen.pas:729
    public void SelectProduction(int Index)
    {
        TUnit U;
        if (FMode != 1)
        {
            SetMode(1);
        }

        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        FProductionIndex = Index;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        MainBtn1.Visible = false;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        switch (FProductionIndex)
        {
            case >= 0 and <= 3:
                U = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
                if (U != null)
                {
                    UnitPnl.Visible = true;
                    var attack = U.GetAttack();
                    UAttack.Text = IntToStr(attack);
                    var defense = U.GetDefense();
                    UDefense.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    UResistance.Text = IntToStr(resistance);
                    var damage = U.GetDamage();
                    UDamage.Text = IntToStr(damage);
                    var hits = U.GetHits();
                    UHits.Text = IntToStr(hits);
                    var moves = U.GetMoves();
                    UMoves.Text = IntToStr(moves);
                    var moveTypes = U.GetMoveTypes();
                    var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
                    UMovesIcon.CurrentImage = mTtoInt;
                    var alignment = U.GetAlignment();
                    var alignmentText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TAlignmentRStr[(int)alignment]);
                    var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AlignmentXRStr), [alignmentText]);
                    UAlignment.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LevelXRStr), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UpkeepXRStr), [(int)upkeep]);
                    ULevel.Text = aoWEFormat + " / " + format2;
                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                    {
                        MainBtn1.Visible = true;
                        var produce = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProduceRStr);
                        MainBtn1.Text = produce;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProduceXRStr), [name]);
                        UnitLbl.Text = format3;
                        var unitProductionGold = FPlace.FProductionControl.UnitProductionGold(FProductionIndex);
                        UCost.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = FPlace.FProductionControl.UnitProductionTime(FProductionIndex);
                        UTurns.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainBtn1.Visible = true;
                        var install = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.InstallRStr);
                        MainBtn1.Text = install;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.InstallXRStr), [name]);
                        UnitLbl.Text = format3;
                        var unitResearchGold = FPlace.FProductionControl.UnitResearchGold(FProductionIndex);
                        UCost.Text = IntToStr(unitResearchGold);
                        var unitResearchTime = FPlace.FProductionControl.UnitResearchTime(FProductionIndex);
                        UTurns.Text = IntToStr(unitResearchTime);
                    }

                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    U.Release();
                }
                else
                {
                    UnitPnl.Visible = false;
                }

                break;
            case 4:
                MainBtn1.Visible = true;
                var productionName = new Trade(0).Name(FPlace.FProductionControl);
                MainBtn1.Text = productionName;
                var fProductionControlQueueLocked = FPlace.FProductionControl.GetQueueLocked();
                MainBtn2.Visible = !fProductionControlQueueLocked;
                var queueText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.QueueRStr);
                MainBtn2.Text = queueText;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                if (FPlace.GetMagicSphere() == TMagicSphere.msNone)
                {
                    var produceMerchandiseText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TProductionScreen.ProduceMerchandiseTextRStr);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    var generateExtraPowerText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TProductionScreen.GenerateExtraPowerTextRStr);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePnl.Visible = true;
                if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
                {
                    UpgradePic.CurrentImage = 28;
                }
                else
                {
                    UpgradePic.CurrentImage = 27;
                }

                UpgradeLbl.Text = MainBtn1.Text;
                break;
            case 5:
                MainBtn1.Visible = true;
                var raze = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RazeRStr);
                MainBtn1.Text = raze + "...";
                PPrev.Visible = false;
                PNext.Visible = false;
                var fPlaceName = FPlace.GetName();
                var format4 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TProductionScreen.ProductionPlaceXRazeRStr), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RazeRStr);
                UpgradeLbl.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string Reason;
        IncomeHdr.Items.Clear();
        var name = FPlace.GetName();
        StructName.Text = name;
        var income = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.IncomeRStr);
        IncomeHdr.Items.Add(income);
        if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
        {
            var power = FPlace.GetPower();
            var xPower = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TProductionScreen.XPowerRStr);
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetIncome() != 0)
        {
            var fPlaceIncome = FPlace.GetIncome();
            var xGold = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XGoldRStr);
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
        {
            IncomePicture.ILIndexTexture = 64;
            IPPicFr.ILTexture = null;
            var sphereImage = Aow.Game.Ui.Dialogs.TProductionScreen.SphereImage(FPlace.GetMagicSphere());
            Sphere.SetILUIndex(sphereImage);
            Sphere.Visible = true;
        }
        else
        {
            if (FPlace is TShipyard)
            {
                IncomeHdr.Items.Clear();
                IncomePicture.ILIndexTexture = 68;
            }
            else
            {
                IncomePicture.ILIndexTexture = 65;
            }

            IPPicFr.ILTexture = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin;
            if (FPlace is TShipyard)
            {
                IPPicFr.ILIndexTexture = 67;
            }
            else
            {
                IPPicFr.ILIndexTexture = 66;
            }

            Sphere.Visible = false;
        }

        IncomeMemo.Items.Clear();
        IncomeValuesMemo.Items.Clear();
        FPlace.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueLB.Items.Clear();
        for (int I = 0, last = FPlace.FProductionControl.FQueue.GetCount() - 1; I <= last; I++)
        {
            var name2 = FPlace.FProductionControl.FQueue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = FPlace.FProductionControl.FQueue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EmptyQueueRStr);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = FPlace.FProductionControl.FQueue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = FPlace.FProductionControl.FQueue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            FQueueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = FPlace.FProductionControl.FLoopQueue;
        if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
        {
            var nameProgressive = FPlace.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (FPlace.FProductionControl.FSettings.FTurnsLeft != -1)
            {
                var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(FPlace.FProductionControl.FSettings.FTurnsLeft);
                ActiveActionTurns.Text = fmtTurnsLeftStr;
                ActiveActionTurns.Visible = true;
                ActiveActionTurns.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = FPlace.FProductionControl.FSettings.FTurnsTotal;
                this.ActiveActionTurnsBar.Position = FPlace.FProductionControl.FSettings.FTurnsTotal - FPlace.FProductionControl.FSettings.FTurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurns.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            var activityImage = FPlace.FProductionControl.FQueue.GetItem(0).Order.ActivityImage(FPlace);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsIdle)
            {
                var currentActivityText = FPlace.GetCurrentActivityText();
                ActiveAction.Text = currentActivityText;
                var defaultText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DefaultRStr);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
                if (CompareText(ActiveAction.Text, none) != 0)
                {
                    ActiveActionTurns.Visible = true;
                    var activityImage = new Trade(0).ActivityImage(FPlace);
                    CurrentActionImage.CurrentImage = activityImage;
                }
                else
                {
                    ActiveActionTurns.Visible = false;
                    CurrentActionImage.CurrentImage = 62;
                }

                ActiveActionTurnsBar.Visible = false;
            }
            else
            {
                if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
                {
                    Reason = "";
                    var nameProgressive = FPlace.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
                    ActiveAction.Text = nameProgressive;
                    var queue = FPlace.FProductionControl.FQueue.GetItem(0);
                    FPlace.FProductionControl.CanExecuteProduction(queue, ref Reason);
                    ActiveActionTurns.Text = Reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurnsBar.Visible = false;
                    var activityImage = FPlace.FProductionControl.FQueue.GetItem(0).Order.ActivityImage(FPlace);
                    CurrentActionImage.CurrentImage = activityImage;
                }
            }
        }

        switch (FMode)
        {
            case 1:
                if (FProductionIndex != -1)
                {
                    SelectProduction(FProductionIndex);
                }

                break;
            case 2:
                QueueLBChange();
                break;
        }

        Dlg.FDrawn = false;
    }

    // AoWProdScreen.pas:180
    public void ShowPlace(TProductionPlace Place)
    {
        TArmyHS Army;
        TRndTacHsm Generator;
        THexagonSprite Center;
        Reset();
        FPlace = Place;
        UpdateInfo();
        SetMode(0);
        var text = Place.GetDescription().Text;
        PlaceInfoMemo.Items.Text = text;
        for (int I = 0, last = FPlace.FProductionControl.FUnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(FPlace.FProductionControl.FUnitProductions.GetItem(I).FUnitIndex);
        }

        FreePreview();
        Generator = new TRndTacHsm();
        var lhx = FPlace.GetLhx();
        var yhx = FPlace.GetYhx();
        FPreview = Generator.RandomMap(null, FPlace.GetXhx(), yhx, lhx);
        Generator?.Dispose();
        Army = ((TAoWMapField)FPlace.Owner).GetArmyHS();
        if (Army != null)
        {
            for (int I = 0, last = Army.FArmy.GetCount() - 1; I <= last; I++)
            {
                var unit = Army.FArmy.GetUnit(I);
                Aow.Game.Combat.TAoWCombatMap.PlaceOverviewUnit(FPreview, unit, 0, I);
            }
        }

        var baseIncome = FPlace.GetBaseIncome();
        B5CB.CaptureInput = baseIncome != 0;
        Center = FPreview.FCenterViewLocation;
        if (Center != null)
        {
            var fCenterViewLocationYhx = FPreview.FCenterViewLocation.GetYhx();
            Aow.Game.World.Map.HexGeometry.HXtoHP(Center.GetXhx(), fCenterViewLocationYhx, ref FViewX, ref FViewY);
            FViewX = FViewX + 16 - MapView.WinWidth / 2;
            FViewY = FViewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            FViewX = 800;
            FViewY = 800;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWProdScreen.pas:221
    public void ProductionScreenCreate()
    {
        FImages = new TIntegerList();
    }

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy()
    {
        FImages?.Dispose();
    }

    // AoWProdScreen.pas:231
    public void ActivityChange(TAOWCheckBox Sender, int Slot)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        if (Sender.Checked)
        {
            ResetChecks();
            SelectProduction(Slot);
            Sender.Checked = true;
        }
    }

    // AoWProdScreen.pas:242
    public void ActivityDoubleClick(TAOWCheckBox Sender, int Slot)
    {
        ResetChecks();
        SelectProduction(Slot);
        Sender.Checked = true;
        MainBtn1Click();
    }

    // AoWProdScreen.pas:250
    public void ActivityPreDraw(TAOWCheckBox Sender, TSurfaceDesc SD, int Slot)
    {
        TP2UnitShowInfo Info = default;
        TRect R;
        TUnit U;
        R = Rect(ActivityPnl2.WinLeft + Sender.WinLeft, ActivityPnl2.WinTop + Sender.WinTop, ActivityPnl2.WinLeft + Sender.WinLeft + Sender.WinWidth, ActivityPnl2.WinTop + Sender.WinTop + Sender.WinHeight);
        switch (Slot)
        {
            case >= 0 and <= 3:
                U = FPlace.FProductionControl.CreateUnitProduction(Slot);
                if (U != null)
                {
                    Info.Direction = 3;
                    Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                    Info.Animation = 0;
                    Info.Frame = 0;
                    Info.Looped = false;
                    if (!FPlace.FProductionControl.FUnitProductions.GetItem(Slot).FResearched)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[5].Show(R.Left + 2, R.Top + 2, SD);
                        Info.Direction = 5;
                        if (U != null)
                        {
                            U.ShowEx(R.Left - 4, R.Top, SD, Info);
                        }

                        var lookupTable = Aow.Game.Draw.GfxState.AlphaBlendTable.GetLookupTables(45);
                        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[5].ShowBlended(R.Left + 2, R.Top + 2, SD, lookupTable);
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[4].Show(R.Left + 2, R.Top + 2, SD);
                        if (U != null)
                        {
                            U.ShowEx(R.Left - 4, R.Top, SD, Info);
                        }
                    }

                    U.Release();
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 4:
                if (FPlace.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(FPlace);
                    Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[activityImage].Show(R.Left + 2, R.Top + 2, SD);
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 5:
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.CityWin[46].Show(R.Left + 2, R.Top + 2, SD);
                break;
        }
    }

    // AoWProdScreen.pas:299
    public void MainBtn1Click()
    {
        string Reason = "";
        switch (FMode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                break;
            case 1:
                switch (FProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                        {
                            if (FPlace.CanProduce(FProductionIndex, false, ref Reason))
                            {
                                FPlace.Produce(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                            }
                        }
                        else
                        {
                            if (FPlace.CanResearch(FProductionIndex, false, ref Reason))
                            {
                                FPlace.Research(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                            }
                        }

                        break;
                    case 4:
                        if (FPlace.CanTrade(false, ref Reason))
                        {
                            FPlace.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case 5:
                        if (FPlace.CanRaze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, ref Reason))
                        {
                            FPlace.Raze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                }

                break;
            case 2:
                if (QueueLB.Selected != -1)
                {
                    FPlace.FProductionControl.DeleteProduction(QueueLB.Selected);
                }

                break;
        }
    }

    // AoWProdScreen.pas:347
    public void MainBtn2Click()
    {
        string Reason = "";
        switch (FMode)
        {
            case 1:
                switch (FProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                        {
                            if (FPlace.CanProduce(FProductionIndex, true, ref Reason))
                            {
                                FPlace.Produce(FProductionIndex, true);
                            }
                            else
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                            }
                        }
                        else
                        {
                            if (FPlace.CanResearch(FProductionIndex, true, ref Reason))
                            {
                                FPlace.Research(FProductionIndex, true);
                            }
                            else
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                            }
                        }

                        break;
                    case 4:
                        if (FPlace.CanTrade(true, ref Reason))
                        {
                            FPlace.Trade(true);
                        }
                        else
                        {
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                }

                break;
            case 2:
                if (FQueueIndex == 0)
                {
                    if (FPlace.FProductionControl.RetryProductionExecution(ref Reason))
                    {
                        SetMode(0);
                    }
                    else
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                    }
                }

                break;
        }
    }

    // AoWProdScreen.pas:383
    public void MainBtn3Click()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FMode == 0)
        {
            Dlg.Visible = false;
        }
        else
        {
            SetMode(0);
        }
    }

    // AoWProdScreen.pas:389
    public void UFaceDraw(TaowUserDrawBox Sender, TSurfaceDesc SD)
    {
        TUnit U;
        TRect R;
        U = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
        if (U != null)
        {
            R = Rect(UPictureFramePnl.WinLeft + Sender.WinLeft + 2, UPictureFramePnl.WinTop + Sender.WinTop + 2, UPictureFramePnl.WinLeft + Sender.WinLeft + Sender.WinWidth - 4, UPictureFramePnl.WinTop + Sender.WinTop + Sender.WinHeight - 2);
            U.ShowFace(R, SD);
            U.Release();
        }
    }

    // AoWProdScreen.pas:406
    public void QueueLBChange()
    {
        var count = FPlace.FProductionControl.FQueue.GetCount();
        if (QueueLB.Selected < count && FMode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                FQueueIndex = QueueLB.Selected;
                if (FPlace.FProductionControl.FQueue.GetItem(FQueueIndex).Order is ProduceUnit produce)
                {
                    SelectProduction(produce.Index);
                }
                else
                {
                    SelectProduction(4);
                }

                SetMode(2);
                QueueLB.Selected = FQueueIndex;
                MainBtn1.Visible = true;
                var delete = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DeleteRStr);
                MainBtn1.Text = delete;
                if (FQueueIndex == 0 && FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
                {
                    MainBtn2.Visible = true;
                    var execute = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ExecuteRStr);
                    MainBtn2.Text = execute;
                }
                else
                {
                    MainBtn2.Visible = false;
                }

                PPrev.Visible = false;
                PNext.Visible = false;
            }
            else
            {
                SetMode(1);
                SelectDefaultAction();
            }
        }
        else
        {
            if (FMode == 2)
            {
                QueueLB.Selected = FQueueIndex;
            }
            else
            {
                QueueLB.Selected = -1;
            }
        }
    }

    // AoWProdScreen.pas:439
    public void B4CBUpdate(TAOWCheckBox Sender, int Slot)
    {
        if (Slot <= 3)
        {
            var unitProduction = FPlace.FProductionControl.FUnitProductions.GetItem(Slot);
            Sender.CaptureInput = unitProduction != null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange()
    {
        FPlace.FProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
    }

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        TP2UnitShowInfo Info = default;
        TRect R;
        TUnit U;
        if (FPlace.FProductionControl.FQueue.GetCount() != 0)
        {
            if (FPlace.FProductionControl.FQueue.GetItem(0).Order.UnitProductionIndex != null)
            {
                R = Rect(ActiveActionPnl.WinLeft + Sender.WinLeft, ActiveActionPnl.WinTop + Sender.WinTop, ActiveActionPnl.WinLeft + Sender.WinLeft + Sender.WinWidth, ActiveActionPnl.WinTop + Sender.WinTop + Sender.WinHeight);
                var productionPlaceControl = FPlace.FProductionControl;
                if (productionPlaceControl.FUnitProductions.GetItem(productionPlaceControl.FSettings.FUnitProductionIndex) != null)
                {
                    U = FPlace.FProductionControl.CreateUnitProduction(FPlace.FProductionControl.FSettings.FUnitProductionIndex);
                    if (U != null)
                    {
                        Info.Direction = 3;
                        Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                        Info.Animation = 0;
                        Info.Frame = 0;
                        Info.Looped = false;
                        U.ShowEx(R.Left - 12, R.Top - 5, SD, Info);
                        U.Release();
                    }
                }
            }
        }
    }

    // AoWProdScreen.pas:483
    public void MapViewDraw(TaowUserDrawBox Sender, TSurfaceDesc SD)
    {
        TSurfaceDesc ViewSD = default;
        TRect R;
        int W;
        int H;
        int Width;
        if (FPreview != null)
        {
            R = Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
            Width = R.Right - R.Left;
            W = Width;
            H = R.Bottom - R.Top;
            var maxXhp = FPreview.GetMaxXhp();
            if (FViewX + W + 16 > maxXhp)
            {
                FViewX = FPreview.GetMaxXhp() - W - 16;
            }

            var maxYhp = FPreview.GetMaxYhp();
            if (FViewY + H + 16 > maxYhp)
            {
                FViewY = FPreview.GetMaxYhp() - H - 16;
            }

            if (FViewX < 16)
            {
                FViewX = 16;
            }

            if (FViewY < 16)
            {
                FViewY = 16;
            }

            Aow.Game.Draw.TSurfaceDesc.CreateSDfromSD(SD, R, ref ViewSD);
            FPreview.SetExplorationEnabled(false);
            FPreview.SetFogEnabled(false);
            FPreview.SetViewPort(FViewX, FViewY, 0, Width, R.Bottom - R.Top);
            FPreview.UpdateStaticScene();
            FPreview.UpdateDynamicScene();
            FPreview.DrawStaticScene(ref ViewSD);
            FPreview.DrawDynamicScene(ref ViewSD);
        }
    }

    // AoWProdScreen.pas:510
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        switch (Key)
        {
            case 37:
                FViewX -= 16;
                MapView.FDrawn = false;
                break;
            case 39:
                FViewX += 16;
                MapView.FDrawn = false;
                break;
            case 38:
                FViewY -= 16;
                MapView.FDrawn = false;
                break;
            case 40:
                FViewY += 16;
                MapView.FDrawn = false;
                break;
            case 27:
                Dlg.Visible = false;
                break;
        }
    }

    // AoWProdScreen.pas:521
    public void MapViewUpdate()
    {
        TAoWCombatMap Preview;
        Preview = FPreview;
        if (Preview != null)
        {
            Preview.NewFrame();
        }

        MapView.FDrawn = MapView.FDrawn && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame & 1) != 0;
    }

    // AoWProdScreen.pas:530
    public void DlgHide()
    {
        ClosePlace();
    }

    // AoWProdScreen.pas:535
    public void UAbilitiesLBMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility A;
        if (Button == TMouseButton.mbRight && UAbilitiesLB.Selected > -1)
        {
            var objectValue = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
            A = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                Aow.Game.Ui.Dialogs.TInfoBanner.SetInfoBanner(Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start;
        Index -= 1;
        if (Index < 0)
        {
            Index = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
        }

        while (Index != Start)
        {
            S = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
            {
                Index = Start + 1;
                ShowPlace((TProductionPlace)S);
            }

            Index += -1;
            if (Index < 0)
            {
                Index = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
            }
        }
    }

    // AoWProdScreen.pas:568
    public void PNextClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start + 1;
        if (Index >= Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetCount())
        {
            Index = 0;
        }

        while (Index != Start)
        {
            S = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
            {
                Index = Start - 1;
                ShowPlace((TProductionPlace)S);
            }

            Index += 1;
            if (Index >= Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStructures.GetCount())
            {
                Index = 0;
            }
        }
    }

    private static readonly string GenerateExtraPowerTextRStr = "Increases the power generated by the node by 25%.";
    private static readonly string ProduceMerchandiseTextRStr = "When producing merchandise the income is increased by 25%.";
    private static readonly string ProductionPlaceXRazeRStr = "Destroys the %s instantly by burning it to the ground.";
    private static readonly string XPowerRStr = "%s power";

    private static int SphereImage(global::Aow.Game.Magic.TMagicSphere Sphere)
    {
        return Sphere switch
        {
            global::Aow.Game.Magic.TMagicSphere.msLife => 27,
            global::Aow.Game.Magic.TMagicSphere.msFire => 42,
            global::Aow.Game.Magic.TMagicSphere.msEarth => 33,
            global::Aow.Game.Magic.TMagicSphere.msAir => 36,
            global::Aow.Game.Magic.TMagicSphere.msWater => 39,
            global::Aow.Game.Magic.TMagicSphere.msDeath => 30,
            _ => -1,
        };
    }
}
