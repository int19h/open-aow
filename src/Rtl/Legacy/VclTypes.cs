namespace Aow.Rtl;

public enum TEditCharCase : byte { ecNormal, ecUpperCase, ecLowerCase }

public enum TMouseButton : byte { mbLeft, mbRight, mbMiddle }

public enum TGridDrawState : byte { gdSelected, gdFocused, gdFixed }

public enum TShiftState : byte { ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble }

public enum TPropertyAttribute : byte { paValueList, paSubProperties, paDialog, paMultiSelect, paAutoUpdate, paSortList, paReadOnly, paRevertable }

public delegate void TDrawCellEvent(TObject sender, int col, int row, TRect rect, TSet<TGridDrawState> state);
public delegate void TSelectCellEvent(TObject sender, int col, int row, ref bool select);
public delegate void TMouseEvent(TObject sender, TMouseButton button, TSet<TShiftState> shift, int x, int y);
public delegate void TMouseMoveEvent(TObject sender, TSet<TShiftState> shift, int x, int y);
public delegate void TKeyPressEvent(TObject sender, ref byte key);
public delegate void TKeyEvent(TObject sender, ref ushort key, TSet<TShiftState> shift);

public enum TBltMode : byte { bmNone, bmStretch, bmStretchToFit, bmCenter }

public delegate void TStreamProc(TStream stream);

public enum TConnectionType : byte { conNone, conUnknown, conTCP, conIPX, conModem, conSerial }

public enum TStreamSoundMode : byte { ssmStop, ssmPlay, ssmStream }

public enum TWindowState : byte { wsNormal, wsMinimized, wsMaximized }

public enum TScrollStyle : byte { ssNone, ssHorizontal, ssVertical, ssBoth }
