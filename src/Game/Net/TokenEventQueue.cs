namespace Aow.Game.Net;

/// <summary>The queue a token event was added to (issue 343).</summary>
public enum TokenEventQueue
{
    /// <summary>A player's token control (<see cref="TokenControl.Add"/>).</summary>
    Player,

    /// <summary>The executor's system events, which run between the token controls' events.</summary>
    System,

    /// <summary>The executing batch itself (<see cref="TokenExecutor.InsertExecuteEvent"/>).</summary>
    Executor,
}
