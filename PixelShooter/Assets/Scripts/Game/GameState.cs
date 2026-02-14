namespace PixelShooter.Game
{
    /// <summary>
    /// Represents the different states of the game
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Waiting for player to select a shooter
        /// </summary>
        Selection,

        /// <summary>
        /// A shooter is currently active and executing
        /// </summary>
        ShooterActive,

        /// <summary>
        /// Level has been completed
        /// </summary>
        LevelComplete,

        /// <summary>
        /// Game is paused
        /// </summary>
        Paused
    }
}
