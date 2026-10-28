// Translated from AoWArmyEventDlg.pas.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
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
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWArmyEventDlg
{
    // { TArmyEventDlg (instance size $8C, VMT 004365E4).
//     $78 completion callback; $80 army event; $84 frame; $88 visible unit indices. }
    public partial class TArmyEventDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow ArmyEventWin;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame BGoldSq;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel EventText;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage DamagePic;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton CloseBut;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0064 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage DecoL;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage DecoR;
        // { +0070 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0074 }
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        // { $78 }
        public Aow.Units.AoWE.TArmyEventLog FEvent;
        // { $80 }
        public int FFrame;
        // { $84 }
        public Aow.Units.Engine.TByteList FUnits;
        // AoWArmyEventDlg.pas:113
        // { $88 }
        public void DrawUnits(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Offset = default;
            int I = default;
            {
                var w__1 = FUnits;
                Offset = X - (w__1.Count * 16);
                {
                    I = 0;
                    int end__2 = w__1.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        DrawUnit(I, (I * 32) + Offset, Y, SD);
                    }
                }
            }
        }

        // AoWArmyEventDlg.pas:121
        public void DrawUnit(int Index, int X, int Y, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TArmyEventLog Event = default;
            int Frame = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Event = FEvent;
            var call__3 = FUnits[Index];
            var call__4 = Event.FDamageValues[call__3];
            if (call__4 != 0)
            {
                // { The original staggers animation by bits of the unit object's address. }
                var call__5 = FUnits[Index];
                var call__6 = Event.FArmyView.GetUnit(call__5);
                Frame = FFrame - (((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(call__6)) >> 3)) & 7);
                Info = default;
                Info.Frame = ((int)(((uint)Frame) >> 1)) - 1;
                Info.Direction = 3;
                Info.Animation = Aow.Units.AoWE.TUnitAnimation.uaHit;
                var call__7 = FUnits[Index];
                var call__8 = Event.FArmyView.GetUnit(call__7);
                call__8.ShowEx(X, Y, SD, Info);
                if (Frame == 5)
                {
                    var call__9 = FUnits[Index];
                    var call__10 = Event.FArmyView.GetUnit(call__9);
                    var call__11 = call__10.GetHitPoints();
                    if (call__11 != 0)
                    {
                        var call__12 = FUnits[Index];
                        var call__13 = Event.FArmyView.GetUnit(call__12);
                        var call__14 = call__13.GetUnitGFXResourceIndex();
                        var call__15 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__14);
                        call__15.FSFXLibraries[3].Play(0);
                    }
                    else
                    {
                        var call__16 = FUnits[Index];
                        var call__17 = Event.FArmyView.GetUnit(call__16);
                        var call__18 = call__17.GetUnitGFXResourceIndex();
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__18);
                        call__19.FSFXLibraries[4].Play(0);
                    }
                }

                if (Frame > 5)
                {
                    var call__20 = FUnits[Index];
                    var call__21 = Event.FDamageValues[call__20];
                    var call__22 = FUnits[Index];
                    var call__23 = Event.FDamageTypes[call__22];
                    Aow.Units.AoWE.AoWE.ShowDamageIcon(X, Y + 8, SD, call__21, (Aow.Units.AoWE.TDamageType)call__23);
                    var call__24 = FUnits[Index];
                    var call__25 = Event.FArmyView.GetUnit(call__24);
                    var call__26 = call__25.GetHitPoints();
                    var call__27 = FUnits[Index];
                    var call__28 = Event.FDamageValues[call__27];
                    var call__29 = FUnits[Index];
                    var call__30 = Event.FArmyView.GetUnit(call__29);
                    var call__31 = call__30.GetHits();
                    var call__32 = Aow.Units.AoWE.AoWE.HitPointsToHitPcnt(unchecked((sbyte)unchecked((byte)(call__26 - call__28))), unchecked((sbyte)call__31));
                    Aow.Units.AoWE.AoWE.ShowHitPcnt(X, Y, SD, call__32);
                }
                else
                {
                    var call__33 = FUnits[Index];
                    var call__34 = Event.FArmyView.GetUnit(call__33);
                    var call__35 = call__34.GetHits();
                    var call__36 = FUnits[Index];
                    var call__37 = Event.FArmyView.GetUnit(call__36);
                    var call__38 = call__37.GetHitPoints();
                    var call__39 = Aow.Units.AoWE.AoWE.HitPointsToHitPcnt(unchecked((sbyte)call__38), unchecked((sbyte)call__35));
                    Aow.Units.AoWE.AoWE.ShowHitPcnt(X, Y, SD, call__39);
                }
            }
            else
            {
                var call__40 = FUnits[Index];
                var call__41 = Event.FArmyView.GetUnit(call__40);
                call__41.Show(X, Y, SD);
            }
        }

        // AoWArmyEventDlg.pas:153
        public void ClearEvent()
        {
            Aow.Units.AoWE.TArmyEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Notify = default;
            int I = default;
            int Resource = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            {
                I = 0;
                int end__42 = FUnits.Count - 1;
                for (; I <= end__42; I += 1)
                {
                    var call__43 = FUnits[I];
                    var call__44 = FEvent.FArmyView.GetUnit(call__43);
                    var call__45 = call__44.GetUnitGFXResourceIndex();
                    Resource = call__45;
                    if (Resource != -1)
                    {
                        var call__46 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Resource);
                        call__46.Release();
                    }
                }
            }

            Notify = FOnClose;
            Event = FEvent;
            FOnClose = null;
            FEvent = null;
            if (Notify != null)
            {
                Notify(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWArmyEventDlg.pas:171
        public void ArmyEventDlgCreate()
        {
            var call__47 = Aow.Units.Engine.TByteList.Create();
            FUnits = call__47;
        }

        // AoWArmyEventDlg.pas:176
        // { draft }
        public void ArmyEventDlgDestroy()
        {
            FUnits.Free();
        }

        // AoWArmyEventDlg.pas:181
        // { draft }
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            ArmyEventWin.Visible = false;
        }

        // AoWArmyEventDlg.pas:187
        // { draft }
        public void UnitPnlDrawBack(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (FFrame < 1000)
            {
                FFrame += 1;
            }

            DrawUnits(ArmyEventWin.WinLeft + 203, ArmyEventWin.WinTop + 100, SD);
        }

        // AoWArmyEventDlg.pas:193
        public void UnitPnlUpdate()
        {
            UnitPnl.FDrawn = false;
            ArmyEventWin.FDrawn = false;
        }

        // AoWArmyEventDlg.pas:198
        // { draft }
        public void ArmyEventWinHide()
        {
            ClearEvent();
        }

        // AoWArmyEventDlg.pas:203
        // { draft }
        public void ArmyEventWinKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                ArmyEventWin.Visible = false;
            }
        }
    }

    public static partial class AoWArmyEventDlg
    {
        // AoWArmyEventDlg.pas:84
        public static void ShowArmyEvent(Aow.Units.AoWArmyEventDlg.TArmyEventDlg Dialog, Aow.Units.AoWE.TArmyEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.AoWE.TArmyEventLog Log = default;
            Aow.Units.AoWE.TArmyView Army = default;
            int I = default;
            int Image = default;
            {
                var w__48 = Dialog;
                Aow.Rtl.Rtl.Assert(w__48.FEvent == null, (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoW\\AoWArmyEventDlg.pas", 92);
                w__48.FOnClose = Done;
                w__48.FEvent = Event;
                w__48.FEvent.AddRef();
                Log = w__48.FEvent;
                w__48.DamagePic.CurrentImage = Log.FArmyIcon;
                var call__49 = Log.FArmyMessages.Text;
                w__48.EventText.Text = call__49;
                Army = Log.FArmyView;
                w__48.FUnits.Clear();
                {
                    I = 0;
                    int end__50 = Army.GetCount() - 1;
                    for (; I <= end__50; I += 1)
                    {
                        var call__51 = Army.GetUnitConcealedForPlayer(I, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        bool bool__52 = !call__51;
                        if (!bool__52)
                        {
                            var call__53 = Log.FDamageValues[I];
                            bool__52 = (call__53 != 0);
                        }

                        if (bool__52)
                        {
                            w__48.FUnits.Add(unchecked((byte)I));
                            var call__54 = Log.FArmyView.GetUnit(I);
                            var call__55 = call__54.GetUnitGFXResourceIndex();
                            Image = call__55;
                            if (Image != -1)
                            {
                                var call__56 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Image);
                                call__56.AddRef();
                            }
                        }
                    }
                }

                w__48.ArmyEventWin.Visible = true;
                w__48.FFrame = 0;
            }
        }
    }
}
