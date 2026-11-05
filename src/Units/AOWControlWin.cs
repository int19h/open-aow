// Translated from AOWControlWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AOWControlWin
{
    // published controls. inverted army selection mask
    public partial class TControlWin : TDataModule
    {
        public TControlWin(TComponent owner) : base(owner)
        { }

        public TAOWWindow ControlWindow;
        public TAOWLabel CWTitle;
        public TAOWButton CWMinIcon;
        public TAoWFrame Unit1Frame;
        public TAoWFrame Unit2Frame;
        public TAoWFrame Unit3Frame;
        public TAoWFrame Unit4Frame;
        public TAoWFrame Unit5Frame;
        public TAoWFrame Unit6Frame;
        public TAoWFrame Unit7Frame;
        public TAoWFrame Unit8Frame;
        public TAOWImage CTitleIcon;
        public TAOWLabel MovesLbl;
        public TAOWLabel PartyStatusLbl;
        public TAOWButton Close;
        public TAOWImage U1MIcon;
        public TAOWImage U2MIcon;
        public TAOWImage U3MIcon;
        public TAOWImage U4MIcon;
        public TAOWImage U5MIcon;
        public TAOWImage U6MIcon;
        public TAOWImage U7MIcon;
        public TAOWImage U8MIcon;
        public TAOWImage MoraleMask;
        public TAOWImage MoveIcon;
        public TAOWPanel PartyT1Pnl;
        public TAOWPanel PartyBPnl;
        public TAOWPanel PartyT2Pnl;
        public TAOWPanel PartyT3Pnl;
        public TAOWPanel PartyT4Pnl;
        public TAoWGauge Unit1Gge;
        public TAoWGauge Unit2Gge;
        public TAoWGauge Unit3Gge;
        public TAoWGauge Unit4Gge;
        public TAoWGauge Unit5Gge;
        public TAoWGauge Unit6Gge;
        public TAoWGauge Unit7Gge;
        public TAoWGauge Unit8Gge;
        public TAOWCheckBox Unit1Chk;
        public TAOWCheckBox Unit2Chk;
        public TAOWCheckBox Unit3Chk;
        public TAOWCheckBox Unit5Chk;
        public TAOWCheckBox Unit6Chk;
        public TAOWCheckBox Unit7Chk;
        public TAOWCheckBox Unit4Chk;
        public TAOWCheckBox Unit8Chk;
        public TAoWFrame Emp1Fr;
        public TAoWFrame Emp2Fr;
        public TAoWFrame Emp3Fr;
        public TAoWFrame Emp4Fr;
        public TAoWFrame Emp5Fr;
        public TAoWFrame Emp6Fr;
        public TAoWFrame Emp7Fr;
        public TAoWFrame Emp8Fr;
        public TAoWFrame Emp1Chk;
        public TAoWFrame Emp2Chk;
        public TAoWFrame Emp3Chk;
        public TAoWFrame Emp4Chk;
        public TAoWFrame Emp5Chk;
        public TAoWFrame Emp6Chk;
        public TAoWFrame Emp7Chk;
        public TAoWFrame Emp8Chk;
        public TAOWPanel PartyIPnl;
        public TAOWLabel PartyILbl;
        public TAOWButton Center;
        public TAOWButton Help;
        public TAOWButton PPrev;
        public TAOWButton PMove;
        public TAOWButton PNext;
        public TAOWPanel PControlPnl;
        public TAOWCheckBox PGuard;
        public TAoWFrame AoWFrame2;
        public TAOWPanel PartyB2Pbl;
        public TAoWFrame AoWFrame1;
        public TIvTranslator Translator;
        public TAOWLabel PartyActionLbl1;
        public TAOWLabel PartyActionLbl2;
        public byte FSelection;
        // AOWControlWin.pas:294
        public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc SD)
        {
            if (General.FSelectedUnits[Index] != -1)
            {
                if (General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetPreviewImage() != null)
                {
                    SD.ClipRect = R;
                    // { Original tests the unsorted index when checking the background library. }
                    if ((int)General.FUnitList.GetUnit(Index).GetAlignment() < IntGfxMod.UnitBGs.Count)
                    {
                        var alignment = General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetAlignment();
                        var unitBG = IntGfxMod.UnitBGs[(int)alignment];
                        var alignment2 = General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetAlignment();
                        var unitBGWidth = IntGfxMod.UnitBGs[(int)alignment2];
                        var alignment3 = General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetAlignment();
                        IntGfxMod.UnitBGs[(int)alignment3].ShowClipped(R.Left + (R.Right - R.Left - unitBGWidth.Width) / 2, R.Top + (R.Bottom - R.Top - unitBG.Height) / 2, SD);
                    }

                    var previewImage = General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetPreviewImage();
                    var previewImage2 = General.FUnitList.GetUnit(General.FSelectedUnits[Index]).GetPreviewImage();
                    General.FUnitList.GetUnit(General.FSelectedUnits[Index]).ShowEx(R.Left + (R.Right - R.Left - previewImage2.Width) / 2, R.Top + (R.Bottom - R.Top - previewImage.Height) / 2, SD, General.FUnitShowInfo);
                }
            }
        }

        // AOWControlWin.pas:312
        public void CWMinIconClick()
        {
            General.PlayButtonSound(0);
            ControlWindow.Visible = false;
        }

        // AOWControlWin.pas:318
        public void CloseClick(TObject Sender)
        {
            if (Sender != null)
            {
                General.PlayButtonSound(0);
            }

            ControlWindow.Visible = false;
            AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = false;
        }

        // AOWControlWin.pas:324
        public void ControlWindowResize()
        {
            this.ControlWindow.Alignment.RightOffset = this.ControlWindow.WinManager.WinWidth - this.ControlWindow.WinLeft - this.ControlWindow.WinWidth;
            this.ControlWindow.Alignment.TopOffset = this.ControlWindow.WinTop;
        }

        // AOWControlWin.pas:333
        public void Unit1FrameMouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (General.FUnitList != null && General.FSelectedUnits[Sender.Tag] > -1 && General.FUnitList.GetUnit(General.FSelectedUnits[Sender.Tag]) != null)
            {
                var unit = General.FUnitList.GetUnit(General.FSelectedUnits[Sender.Tag]);
                General.FSelectedUnit = unit;
                if (Button == TMouseButton.mbLeft)
                {
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                }
            }
        }

        // AOWControlWin.pas:348
        public void Unit1ChkChange(TObject Sender)
        {
            CheckStart();
            AoWHSMap.FSelectedArmy.SetSelection(FSelection);
            CheckEnd();
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }
        }

        // AOWControlWin.pas:354
        public void CheckStart()
        {
            FSelection = 0;
            if (!Unit1Chk.Checked && General.FSelectedUnits[0] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[0]));
            }

            if (!Unit2Chk.Checked && General.FSelectedUnits[1] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[1]));
            }

            if (!Unit3Chk.Checked && General.FSelectedUnits[2] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[2]));
            }

            if (!Unit4Chk.Checked && General.FSelectedUnits[3] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[3]));
            }

            if (!Unit5Chk.Checked && General.FSelectedUnits[4] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[4]));
            }

            if (!Unit6Chk.Checked && General.FSelectedUnits[5] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[5]));
            }

            if (!Unit7Chk.Checked && General.FSelectedUnits[6] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[6]));
            }

            if (!Unit8Chk.Checked && General.FSelectedUnits[7] != -1)
            {
                FSelection = (byte)(FSelection | (1 << General.FSelectedUnits[7]));
            }
        }

        // AOWControlWin.pas:375
        public void CheckEnd()
        {
            FSelection = AoWHSMap.FSelectedArmy.GetSelection();
            Unit1Chk.Checked = General.FSelectedUnits[0] != -1 && (FSelection & (1 << General.FSelectedUnits[0])) == 0;
            Unit2Chk.Checked = General.FSelectedUnits[1] != -1 && (FSelection & (1 << General.FSelectedUnits[1])) == 0;
            Unit3Chk.Checked = General.FSelectedUnits[2] != -1 && (FSelection & (1 << General.FSelectedUnits[2])) == 0;
            Unit4Chk.Checked = General.FSelectedUnits[3] != -1 && (FSelection & (1 << General.FSelectedUnits[3])) == 0;
            Unit5Chk.Checked = General.FSelectedUnits[4] != -1 && (FSelection & (1 << General.FSelectedUnits[4])) == 0;
            Unit6Chk.Checked = General.FSelectedUnits[5] != -1 && (FSelection & (1 << General.FSelectedUnits[5])) == 0;
            Unit7Chk.Checked = General.FSelectedUnits[6] != -1 && (FSelection & (1 << General.FSelectedUnits[6])) == 0;
            Unit8Chk.Checked = General.FSelectedUnits[7] != -1 && (FSelection & (1 << General.FSelectedUnits[7])) == 0;
            if (Unit1Chk.Checked)
            {
                Unit1Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit1Frame.ILIndexFrame = 12;
            }

            if (Unit2Chk.Checked)
            {
                Unit2Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit2Frame.ILIndexFrame = 12;
            }

            if (Unit3Chk.Checked)
            {
                Unit3Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit3Frame.ILIndexFrame = 12;
            }

            if (Unit4Chk.Checked)
            {
                Unit4Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit4Frame.ILIndexFrame = 12;
            }

            if (Unit5Chk.Checked)
            {
                Unit5Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit5Frame.ILIndexFrame = 12;
            }

            if (Unit6Chk.Checked)
            {
                Unit6Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit6Frame.ILIndexFrame = 12;
            }

            if (Unit7Chk.Checked)
            {
                Unit7Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit7Frame.ILIndexFrame = 12;
            }

            if (Unit8Chk.Checked)
            {
                Unit8Frame.ILIndexFrame = 26;
            }
            else
            {
                Unit8Frame.ILIndexFrame = 12;
            }

            var selection = AoWHSMap.FSelectedArmy.GetSelection();
            var partyMove = General.FUnitList.GetPartyMoves(selection);
            var mTtoInt = General.MTtoInt(partyMove);
            MoveIcon.CurrentImage = mTtoInt;
            var fSelectedArmySelection = AoWHSMap.FSelectedArmy.GetSelection();
            var movePoints = General.FUnitList.MovePoints(fSelectedArmySelection);
            MovesLbl.Text = IntToStr(movePoints);
            var selection2 = AoWHSMap.FSelectedArmy.GetSelection();
            var partyStatus = AoWHSMap.FSelectedArmy.GetArmyHS().FArmy.GetPartyStatus(selection2);
            var partyStatus2 = TranslateRStr(TPartyStatusRStr[(int)partyStatus]);
            PartyStatusLbl.Text = partyStatus2;
        }

        // AOWControlWin.pas:407
        public void Unit1ChkMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight)
            {
                CheckStart();
                AoWHSMap.FSelectedArmy.SetSelection((byte)~FSelection);
                CheckEnd();
            }
        }

        // AOWControlWin.pas:415
        public void CenterClick()
        {
            General.PlayButtonSound(0);
            var army = General.FUnitList;
            AoWHSMap.CenterViewTo(army.FX, army.FY, army.FL);

            var army2 = General.FUnitList;
            AoWHSMap.PlayCenterAnimation(army2.FX, army2.FY, army2.FL);
        }

        // AOWControlWin.pas:422
        public void Unit1FramePreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TRect SaveRect;
            TAoWComponent Component;
            if (General.FUnitList != null)
            {
                Component = Sender;
                SaveRect = SurfDesc.ClipRect;
                var rect = Rect(Component.AOWOwner.WinLeft + Component.WinLeft + 4, Component.AOWOwner.WinTop + Component.WinTop + 3, Component.AOWOwner.WinLeft + Component.WinLeft + Component.WinWidth - 3, Component.AOWOwner.WinTop + Component.WinTop + Component.WinHeight - 7);
                DrawUnit(Component.Tag, rect, ref SurfDesc);
                SurfDesc.ClipRect = SaveRect;
            }
        }

        // AOWControlWin.pas:437
        public void HelpClick()
        {
            ShowHelp(503);
        }

        // AOWControlWin.pas:442
        public void PPrevClick()
        {
            AoWHSMap.SelectPrevArmy();
        }

        // AOWControlWin.pas:447
        public void PNextClick()
        {
            AoWHSMap.SelectNextArmy();
        }

        // AOWControlWin.pas:452
        public void PMoveClick()
        {
            AoWHSMap.FSelectedArmy.ExecuteMove();
        }

        // AOWControlWin.pas:457
        public void PGuardChange()
        {
            if (PGuard.Checked)
            {
                if (!General.FUnitList.FUserMode.Contains(TArmyUserModeElement.aumGuard))
                {
                    General.FUnitList.FUserMode = General.FUnitList.FUserMode + TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
                }
            }
            else
            {
                if (General.FUnitList.FUserMode.Contains(TArmyUserModeElement.aumGuard))
                {
                    General.FUnitList.FUserMode = General.FUnitList.FUserMode - TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
                }
            }
        }
    }

    public static partial class AOWControlWin
    {
        public static TControlWin ControlWin;
    }
}
