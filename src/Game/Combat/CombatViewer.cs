// Translated from AoWE.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

// Status. Combat. Delay. Speed. Position. Time. Logbook
public partial class CombatViewer : IDisposable
{
    // Combat2.pas:546
    public CombatViewer()
    {
        Combat = new Battle(null);
        Time = -1;
        _speed = 5;
        Delay = 5;
    }

    public Set<CombatViewerStatusElement> Status { get; private set; }
    public Battle Combat { get; }
    public byte Delay { get; private set; }

    private byte _speed;

    public int Position { get; private set; }
    public int Time { get; private set; }
    public CombatLogbook Logbook { get; private set; }

    // Combat2.pas:555
    public virtual void Dispose()
    {
        if (Status.Contains(CombatViewerStatusElement.Initialized))
        {
            Finalize();
        }

        Combat?.Dispose();
    }

    // Combat2.pas:562
    public void Initialize(CombatLogbook logbook)
    {
        if (Status.Contains(CombatViewerStatusElement.Initialized))
        {
            Finalize();
        }

        Status = Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized);
        Logbook = logbook;
        Logbook.AddRef();
        Position = 0;
        Time = -1;
        Combat.CombatData.Assign(logbook.InitialData, GameTables.GameEngine);
        Combat.Activate();
        Delay = _speed;
    }

    // Combat2.pas:575
    public void Finalize()
    {
        if (Status.Contains(CombatViewerStatusElement.Initialized))
        {
            Combat.Deactivate();
            Combat.Clear();
            Logbook.Release();
            Logbook = null;
            Status = Status - Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized);
        }
    }

    // Combat2.pas:587
    public int GetEndPosition() => Logbook.Actions.Count + 1;

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
        if (Status.Contains(CombatViewerStatusElement.Initialized))
        {
            Time = -1;
            if (position > GetEndPosition())
            {
                position = GetEndPosition();
            }

            if (position > 0 && position <= Logbook.Actions.Count)
            {
                Combat.Deactivate();
                Combat.CombatData.Assign(Logbook.InitialData, GameTables.GameEngine);
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
                    Combat.CombatData.Assign(Logbook.InitialData, GameTables.GameEngine);
                    Combat.Activate();
                }
                else
                {
                    Position = GetEndPosition();
                    Combat.Deactivate();
                    Combat.CombatData.Assign(Logbook.FinalData, GameTables.GameEngine);
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
    public void Play()
    {
        Status = Status + Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Playing) - Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Paused);
    }

    // Combat2.pas:632
    public void Pause()
    {
        Status = Status + Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Paused);
    }

    // Combat2.pas:635
    public void Update()
    {
        bool playing;
        if ((Status & Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized, CombatViewerStatusElement.Playing, CombatViewerStatusElement.Paused)) == Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized, CombatViewerStatusElement.Playing) && GetEndPosition() != Position)
        {
            playing = false;
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
                    var count = Logbook.Actions.Count;
                    condition = Position <= count;
                }

                if (!(condition && !playing))
                    break;
                Time += 1;
                playing = Logbook.Actions.GetCombatActions(Position - 1).Play(this);
                if (!playing)
                {
                    Position += 1;
                    Delay = _speed;
                    Time = -1;
                }
            }

            if (GetEndPosition() == Position)
            {
                SetPosition(GetEndPosition());
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
            var count = Logbook.Actions.Count;
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
            var count = Logbook.Actions.Count;
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
        if (target != null)
        {
            if (target.GetSide() == CombatSideIndex.Attacker)
            {
                ShowObject(target, frame, 3, X, Y, surface);
            }
            else
            {
                ShowObject(target, frame, 5, X + 25, Y, surface);
            }
        }

        if (source != null)
        {
            if (source.GetSide() == CombatSideIndex.Attacker)
            {
                ShowObject(source, frame, 2, X, Y + 15, surface);
            }
            else
            {
                ShowObject(source, frame, 6, X + 25, Y + 15, surface);
            }
        }
    }

    // intf_P3_02.inc:19

}
