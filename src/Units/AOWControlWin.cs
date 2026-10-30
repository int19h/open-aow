// Translated from AOWControlWin.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AOWControlWin
{
    // published controls. inverted army selection mask
    public partial class TControlWin : Aow.Rtl.TDataModule
    {
        public TControlWin(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow ControlWindow;
        public Aow.Units.AoWLabel.TAOWLabel CWTitle;
        public Aow.Units.AoWButton.TAOWButton CWMinIcon;
        public Aow.Units.AoWFrame.TAoWFrame Unit1Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit2Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit3Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit4Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit5Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit6Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit7Frame;
        public Aow.Units.AoWFrame.TAoWFrame Unit8Frame;
        public Aow.Units.AoWImage.TAOWImage CTitleIcon;
        public Aow.Units.AoWLabel.TAOWLabel MovesLbl;
        public Aow.Units.AoWLabel.TAOWLabel PartyStatusLbl;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWImage.TAOWImage U1MIcon;
        public Aow.Units.AoWImage.TAOWImage U2MIcon;
        public Aow.Units.AoWImage.TAOWImage U3MIcon;
        public Aow.Units.AoWImage.TAOWImage U4MIcon;
        public Aow.Units.AoWImage.TAOWImage U5MIcon;
        public Aow.Units.AoWImage.TAOWImage U6MIcon;
        public Aow.Units.AoWImage.TAOWImage U7MIcon;
        public Aow.Units.AoWImage.TAOWImage U8MIcon;
        public Aow.Units.AoWImage.TAOWImage MoraleMask;
        public Aow.Units.AoWImage.TAOWImage MoveIcon;
        public Aow.Units.AoWPanel.TAOWPanel PartyT1Pnl;
        public Aow.Units.AoWPanel.TAOWPanel PartyBPnl;
        public Aow.Units.AoWPanel.TAOWPanel PartyT2Pnl;
        public Aow.Units.AoWPanel.TAOWPanel PartyT3Pnl;
        public Aow.Units.AoWPanel.TAOWPanel PartyT4Pnl;
        public Aow.Units.AOWGauge.TAoWGauge Unit1Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit2Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit3Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit4Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit5Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit6Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit7Gge;
        public Aow.Units.AOWGauge.TAoWGauge Unit8Gge;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit1Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit2Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit3Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit5Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit6Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit7Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit4Chk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Unit8Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp1Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp2Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp3Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp4Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp5Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp6Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp7Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp8Fr;
        public Aow.Units.AoWFrame.TAoWFrame Emp1Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp2Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp3Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp4Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp5Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp6Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp7Chk;
        public Aow.Units.AoWFrame.TAoWFrame Emp8Chk;
        public Aow.Units.AoWPanel.TAOWPanel PartyIPnl;
        public Aow.Units.AoWLabel.TAOWLabel PartyILbl;
        public Aow.Units.AoWButton.TAOWButton Center;
        public Aow.Units.AoWButton.TAOWButton Help;
        public Aow.Units.AoWButton.TAOWButton PPrev;
        public Aow.Units.AoWButton.TAOWButton PMove;
        public Aow.Units.AoWButton.TAOWButton PNext;
        public Aow.Units.AoWPanel.TAOWPanel PControlPnl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox PGuard;
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame2;
        public Aow.Units.AoWPanel.TAOWPanel PartyB2Pbl;
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWLabel.TAOWLabel PartyActionLbl1;
        public Aow.Units.AoWLabel.TAOWLabel PartyActionLbl2;
        public byte FSelection;
        // AOWControlWin.pas:294
        public void DrawUnit(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index] != -1)
            {
                var call__1 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                var call__2 = call__1.GetPreviewImage();
                if (call__2 != null)
                {
                    SD.ClipRect = R;
                    // { Original tests the unsorted index when checking the background library. }
                    var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Index);
                    var call__4 = call__3.GetAlignment();
                    if (((int)call__4) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                    {
                        var call__5 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__6 = call__5.GetAlignment();
                        var call__7 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__6];
                        var call__8 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__9 = call__8.GetAlignment();
                        var call__10 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__9];
                        var call__11 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                        var call__12 = call__11.GetAlignment();
                        var call__13 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__12];
                        call__13.ShowClipped(R.Left + (((R.Right - R.Left) - call__10.Width) / 2), R.Top + (((R.Bottom - R.Top) - call__7.Height) / 2), SD);
                    }

                    var call__14 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    var call__15 = call__14.GetPreviewImage();
                    var call__16 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    var call__17 = call__16.GetPreviewImage();
                    var call__18 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Index]);
                    call__18.ShowEx(R.Left + (((R.Right - R.Left) - call__17.Width) / 2), R.Top + (((R.Bottom - R.Top) - call__15.Height) / 2), SD, Aow.Units.AoWWinMan.AoWWinMan.General.FUnitShowInfo);
                }
            }
        }

        // AOWControlWin.pas:312
        public void CWMinIconClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            ControlWindow.Visible = false;
        }

        // AOWControlWin.pas:318
        public void CloseClick(Aow.Rtl.TObject Sender)
        {
            if (Sender != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            }

            ControlWindow.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ConButton.Visible = false;
        }

        // AOWControlWin.pas:324
        public void ControlWindowResize()
        {
            this.ControlWindow.Alignment.RightOffset = (this.ControlWindow.WinManager.WinWidth - this.ControlWindow.WinLeft) - this.ControlWindow.WinWidth;
            this.ControlWindow.Alignment.TopOffset = this.ControlWindow.WinTop;
        }

        // AOWControlWin.pas:333
        public void Unit1FrameMouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            bool bool__20 = (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag] > -1);
            if (bool__20)
            {
                var call__21 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag]);
                bool__20 = (call__21 != null);
            }

            if (bool__20)
            {
                {
                    var call__22 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[Sender.Tag]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__22;
                    if (Button == Aow.Rtl.TMouseButton.mbLeft)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    }
                }
            }
        }

        // AOWControlWin.pas:348
        public void Unit1ChkChange(Aow.Rtl.TObject Sender)
        {
            CheckStart();
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.SetSelection(unchecked((byte)FSelection));
            CheckEnd();
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AOWControlWin.pas:354
        public void CheckStart()
        {
            FSelection = 0;
            if (!Unit1Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])));
            }

            if (!Unit2Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])));
            }

            if (!Unit3Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])));
            }

            if (!Unit4Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])));
            }

            if (!Unit5Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])));
            }

            if (!Unit6Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])));
            }

            if (!Unit7Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])));
            }

            if (!Unit8Chk.Checked && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7] != -1))
            {
                FSelection = unchecked((byte)(FSelection | (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])));
            }
        }

        // AOWControlWin.pas:375
        public void CheckEnd()
        {
            var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            FSelection = unchecked((byte)call__23);
            Unit1Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0])) == 0);
            Unit2Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1])) == 0);
            Unit3Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[2])) == 0);
            Unit4Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[3])) == 0);
            Unit5Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[4])) == 0);
            Unit6Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[5])) == 0);
            Unit7Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[6])) == 0);
            Unit8Chk.Checked = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7] != -1) && ((FSelection & (1 << Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[7])) == 0);
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

            var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__25 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetPartyMoves(unchecked((byte)call__24));
            var call__26 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__25);
            MoveIcon.CurrentImage = call__26;
            var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__28 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.MovePoints(unchecked((byte)call__27));
            MovesLbl.Text = Aow.Rtl.Rtl.IntToStr(call__28);
            var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
            var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetArmyHS();
            var call__31 = call__30.FArmy.GetPartyStatus(unchecked((byte)call__29));
            var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPartyStatusRStr[(int)call__31]);
            PartyStatusLbl.Text = call__32;
        }

        // AOWControlWin.pas:407
        public void Unit1ChkMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                CheckStart();
                Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.SetSelection(unchecked((byte)~FSelection));
                CheckEnd();
            }
        }

        // AOWControlWin.pas:415
        public void CenterClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var army = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList;
            Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(army.FX, army.FY, army.FL);

            var army2 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList;
            Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)army2.FX), unchecked((sbyte)army2.FY), unchecked((sbyte)army2.FL));
        }

        // AOWControlWin.pas:422
        public void Unit1FramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect SaveRect = default;
            Aow.Units.AoWComp.TAoWComponent Component = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null)
            {
                Component = Sender;
                SaveRect = SurfDesc.ClipRect;
                var call__35 = Aow.Rtl.Rtl.Rect((Component.AOWOwner.WinLeft + Component.WinLeft) + 4, (Component.AOWOwner.WinTop + Component.WinTop) + 3, ((Component.AOWOwner.WinLeft + Component.WinLeft) + Component.WinWidth) - 3, ((Component.AOWOwner.WinTop + Component.WinTop) + Component.WinHeight) - 7);
                DrawUnit(Component.Tag, call__35, ref SurfDesc);
                SurfDesc.ClipRect = SaveRect;
            }
        }

        // AOWControlWin.pas:437
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(503);
        }

        // AOWControlWin.pas:442
        public void PPrevClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.SelectPrevArmy();
        }

        // AOWControlWin.pas:447
        public void PNextClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.SelectNextArmy();
        }

        // AOWControlWin.pas:452
        public void PMoveClick()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.ExecuteMove();
        }

        // AOWControlWin.pas:457
        public void PGuardChange()
        {
            if (PGuard.Checked)
            {
                if (!Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode.Contains(Aow.Units.AoWE.TArmyUserModeElement.aumGuard))
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode + TSet<Aow.Units.AoWE.TArmyUserModeElement>.Of(Aow.Units.AoWE.TArmyUserModeElement.aumGuard);
                }
            }
            else
            {
                if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode.Contains(Aow.Units.AoWE.TArmyUserModeElement.aumGuard))
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.FUserMode - TSet<Aow.Units.AoWE.TArmyUserModeElement>.Of(Aow.Units.AoWE.TArmyUserModeElement.aumGuard);
                }
            }
        }
    }

    public static partial class AOWControlWin
    {
        public static Aow.Units.AOWControlWin.TControlWin ControlWin;
    }
}
