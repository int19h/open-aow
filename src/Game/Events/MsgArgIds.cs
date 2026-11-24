using Aow.Rtl;

namespace Aow.Game.Events;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromEventLog(EventLogId? log) => new(log.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public EventLogId? EventLog => EventLogId.FromOriginal(argument.Int);
    }
}
