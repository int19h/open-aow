// Translated from AoWConstructDlg.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWConstructDlg
{
    // activation request
    public partial class TConstructDlg : Aow.Rtl.TDataModule
    {
        public TConstructDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        public Aow.Units.AoWButton.TAOWButton ConstructBtn;
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWPanel.TAOWPanel ConstructPnl;
        public Aow.Units.AoWListBox.TAOWListBox ConstructionList;
        public Aow.Units.AoWButton.TAOWButton SortConstructionsBtn;
        public Aow.Units.AoWButton.TAOWButton SortConstructionCostBtn;
        public Aow.Units.AoWListBox.TAOWListBox ConstructionCostList;
        public Aow.Units.AoWListBox.TAOWListBox ConstructionTurnsList;
        public Aow.Units.AoWButton.TAOWButton SortConstructionTurnsBtn;
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel1;
        public Aow.Units.AoWVScroll.TAOWVScrollBar ConSB;
        public Aow.Units.AoWImage.TAOWImage CeltM;
        public Aow.Units.AoWButton.TAOWButton AOWButton2;
        public Aow.Units.AoWVScroll.TAOWVScrollBar T4VSB;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWE.TActivateAbilityInfo FRequest;
        // AoWConstructDlg.pas:117
        public void Execute(Aow.Units.AoWE.TActivateAbilityInfo Request)
        {
            if (FRequest != null)
            {
                FRequest.Release();
            }

            FRequest = Request;
            FRequest.AddRef();
            UpdateConstructions();
            Dlg.Visible = true;
        }

        // AoWConstructDlg.pas:108
        public void Finish()
        {
            if (FRequest != null)
            {
                FRequest.Release();
                FRequest = null;
            }
        }

        // AoWConstructDlg.pas:142
        public void UpdateConstructions()
        {
            int Days = default;
            int Gold = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Rtl.AnsiString Name = default;
            int I = default;
            Aow.Units.AoWE.TConstructAbility Ability = default;
            Aow.Units.AoWE.TConstructionControl Control = default;
            Dlg.FDrawn = false;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FRequest.FUnitID);
            U = call__1;
            ConstructionList.Items.Clear();
            ConstructionCostList.Items.Clear();
            ConstructionTurnsList.Items.Clear();
            var call__2 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(125);
            Ability = (Aow.Units.AoWE.TConstructAbility)call__2;
            {
                I = 0;
                int end__3 = Ability.FConstructionControls.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = Ability.FConstructionControls.GetConstructionControl(I);
                    Control = call__4;
                    Control.GetInfo(U, ref Name, ref Gold, ref Days);
                    ConstructionList.Items.AddObject(Name, Control);
                    if (Gold != 0)
                    {
                        ConstructionCostList.Items.Add(Aow.Rtl.Rtl.IntToStr(Gold));
                    }
                    else
                    {
                        ConstructionCostList.Items.Add(AnsiString.FromBytes(45));
                    }

                    if (Days != 0)
                    {
                        ConstructionTurnsList.Items.Add(Aow.Rtl.Rtl.IntToStr(Days));
                    }
                    else
                    {
                        ConstructionTurnsList.Items.Add(AnsiString.FromBytes(45));
                    }
                }
            }
        }

        // AoWConstructDlg.pas:126
        public void SelectConstruction(int Index)
        {
            bool bool__5 = (Index >= 0);
            if (bool__5)
            {
                var call__6 = ConstructionList.Items.Count;
                bool__5 = (Index < call__6);
            }

            if (bool__5)
            {
                ConstructionList.Selected = Index;
                ConstructionCostList.Selected = Index;
                ConstructionTurnsList.Selected = Index;
            }
            else
            {
                ConstructionList.Selected = -1;
                ConstructionCostList.Selected = -1;
                ConstructionTurnsList.Selected = -1;
            }
        }

        // AoWConstructDlg.pas:62
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWConstructDlg.pas:68
        public void ConstructionListChange()
        {
            SelectConstruction(ConstructionList.Selected);
        }

        // AoWConstructDlg.pas:71
        public void ConstructionCostListChange()
        {
            SelectConstruction(ConstructionCostList.Selected);
        }

        // AoWConstructDlg.pas:74
        public void ConstructionTurnsListChange()
        {
            SelectConstruction(ConstructionTurnsList.Selected);
        }

        // AoWConstructDlg.pas:77
        public void ConstructBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TConstructionControl Control = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            bool bool__7 = (ConstructionList.Selected >= 0);
            if (bool__7)
            {
                var call__8 = ConstructionList.Items.Count;
                bool__7 = (ConstructionList.Selected < call__8);
            }

            if (bool__7)
            {
                var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FRequest.FUnitID);
                U = call__9;
                var call__10 = ConstructionList.Items.Objects[ConstructionList.Selected];
                Control = (Aow.Units.AoWE.TConstructionControl)call__10;
                if (Control.CanConstruct(U, ref Reason))
                {
                    if (Control.Construct(U))
                    {
                        Dlg.Visible = false;
                    }
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                }
            }
        }

        // AoWConstructDlg.pas:96
        public void SortConstructionsBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWConstructDlg.pas:99
        public void SortConstructionCostBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWConstructDlg.pas:102
        public void SortConstructionTurnsBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWConstructDlg.pas:105
        public void DlgHide()
        {
            Finish();
        }

        // AoWConstructDlg.pas:169
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWConstructDlg
    {
    }
}
