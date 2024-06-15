using SadConsole.Configuration;

Settings.WindowTitle = "Game of Life";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<game_of_life.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true);

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
