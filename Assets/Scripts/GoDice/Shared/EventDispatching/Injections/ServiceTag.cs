namespace GoDice.Shared.EventDispatching.Injections
{
    public static class ServiceTag
    {
        public const string TurnScoreTrackers = "TurnScoreTrackers";
        public const string TotalScoreTrackers = "TotalScoreTrackers";
        public const string FailuresTrackers = "FailuresTrackers";
        public const string TurnsTracker = "TurnsTracker";
        public const string RoundsTracker = "RoundsTracker";

        public const string PlayersToSkipTurn = "PlayersToSkipTurn";

        public const string UndoController = "UndoController";
        public const string PlayBtn = "PlayBtn";

        // ReSharper disable once InconsistentNaming
        public const string DiceSelectionDialogInstaller = "SelectionDialogInstaller";

        public const string InputActivator = "InputActivator";
    }
}