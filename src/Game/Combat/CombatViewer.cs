#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

namespace Aow.Game.Combat;

// Status. Combat. Delay. Speed. Position. Time. Logbook
public sealed class CombatViewer : IDisposable
{
    // Combat2.pas:546
    public CombatViewer(EngineObject creator)
    {
        Creator = creator;
        Combat = new Battle(null) { Holder = Creator.CreationHolder() };
        Time = -1;
        _speed = 5;
        Delay = 5;
    }

    // The node that created this viewer. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    public CombatViewerStatuses Status { get; private set; }
    public Battle Combat { get; }
    public byte Delay { get; private set; }

    private byte _speed;

    public int Position { get; private set; }
    public int Time { get; private set; }
    public CombatLogbook Logbook { get; private set; }

    // Combat2.pas:555
    public void Dispose()
    {
        if (Status.Contains(CombatViewerStatuses.Initialized))
        {
            Finalize();
        }

        Combat?.Dispose();
    }

    // Combat2.pas:562
    public void Initialize(CombatLogbook logbook)
    {
        if (Status.Contains(CombatViewerStatuses.Initialized))
        {
            Finalize();
        }

        Status = CombatViewerStatuses.Initialized;
        Logbook = logbook;
        _ = Logbook.AddRef();
        Position = 0;
        Time = -1;
        Combat.CombatData.Assign(logbook.InitialData, engine: Creator.GetEngine());
        Combat.Activate();
        Delay = _speed;
    }

    // Combat2.pas:575
    public void Finalize()
    {
        if (Status.Contains(CombatViewerStatuses.Initialized))
        {
            Combat.Deactivate();
            Combat.Clear();
            _ = Logbook.Release();
            Logbook = null;
            Status &= ~CombatViewerStatuses.Initialized;
        }
    }

    // Combat2.pas:587
    public int EndPosition => Logbook.Actions.Count + 1;

    // Combat2.pas:590
    public void SetSpeed(byte speed)
    {
        if (speed == 0)
        {
            speed = 1;
        }

        _speed = speed;
    }

    // Combat2.pas:593
    public void SetPosition(int position)
    {
        if (Status.Contains(CombatViewerStatuses.Initialized))
        {
            Time = -1;
            if (position > EndPosition)
            {
                position = EndPosition;
            }

            if (position > 0 && position <= Logbook.Actions.Count)
            {
                Combat.Deactivate();
                Combat.CombatData.Assign(Logbook.InitialData, engine: Creator.GetEngine());
                Combat.Activate();
                Position = 0;
                while (position > Position)
                {
                    Position += 1;
                    if (position != Position)
                    {
                        Logbook.Actions.GetCombatActions(Position - 1).Execute(Combat);
                    }
                }
            }
            else
            {
                if (position == 0)
                {
                    Position = 0;
                    Combat.Deactivate();
                    Combat.CombatData.Assign(Logbook.InitialData, engine: Creator.GetEngine());
                    Combat.Activate();
                }
                else
                {
                    Position = EndPosition;
                    Combat.Deactivate();
                    Combat.CombatData.Assign(Logbook.FinalData, engine: Creator.GetEngine());
                    Combat.Activate();
                }
            }
        }
        else
        {
            Position = position;
        }
    }

    // Combat2.pas:629
    public void Play() => Status = (Status | CombatViewerStatuses.Playing) & ~CombatViewerStatuses.Paused;

    // Combat2.pas:632
    public void Pause() => Status |= CombatViewerStatuses.Paused;

    // Combat2.pas:635
    public void Update()
    {
        if ((Status & (CombatViewerStatuses.Initialized | CombatViewerStatuses.Playing | CombatViewerStatuses.Paused)) == (CombatViewerStatuses.Initialized | CombatViewerStatuses.Playing) && EndPosition != Position)
        {
            bool playing = false;
            if (Position == 0)
            {
                Time += 1;
                if (Time >= Delay)
                {
                    Position = 1;
                    Time = -1;
                }
            }

            while (true)
            {
                bool condition = Position > 0;
                if (condition)
                {
                    int count = Logbook.Actions.Count;
                    condition = Position <= count;
                }

                if (!(condition && !playing))
                {
                    break;
                }

                Time += 1;
                playing = Logbook.Actions.GetCombatActions(Position - 1).Play(this);
                if (!playing)
                {
                    Position += 1;
                    Delay = _speed;
                    Time = -1;
                }
            }

            if (EndPosition == Position)
            {
                SetPosition(EndPosition);
            }
        }
    }

    // Combat2.pas:667
    public void ShowObject(CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (Logbook.Actions.Count >= Position)
        {
            Logbook.Actions.GetCombatActions(Position - 1).ShowObject(this, objectValue, frame, direction, X, Y, surface);
        }
        else
        {
            objectValue.Show(frame, direction, X, Y, surface);
        }
    }

    // Combat2.pas:674
    public void ShowFieldObject(CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        bool condition = Position > 0;
        if (condition)
        {
            int count = Logbook.Actions.Count;
            condition = Position <= count;
        }

        if (condition)
        {
            Logbook.Actions.GetCombatActions(Position - 1).ShowFieldObject(this, objectValue, frame, direction, X, Y, surface);
        }
        else
        {
            objectValue.Show(frame, direction, X, Y, surface);
        }
    }

    // Combat2.pas:681
    public void ShowFightingObjects(int frame, int X, int Y, in LockedSurface surface)
    {
        bool condition = Position > 0;
        if (condition)
        {
            int count = Logbook.Actions.Count;
            condition = Position <= count;
        }

        if (condition)
        {
            Logbook.Actions.GetCombatActions(Position - 1).ShowFightingObjects(this, frame, X, Y, surface);
        }
    }

    // Combat2.pas:688
    public void DefaultShowFightingObjects(CombatObject source, CombatObject target, int frame, int X, int Y, in LockedSurface surface)
    {
        if (target is not null)
        {
            if (target.Side == CombatSideIndex.Attacker)
            {
                ShowObject(target, frame, direction: 3, X, Y, surface);
            }
            else
            {
                ShowObject(target, frame, direction: 5, X + 25, Y, surface);
            }
        }

        if (source is not null)
        {
            if (source.Side == CombatSideIndex.Attacker)
            {
                ShowObject(source, frame, direction: 2, X, Y + 15, surface);
            }
            else
            {
                ShowObject(source, frame, direction: 6, X + 25, Y + 15, surface);
            }
        }
    }
}
