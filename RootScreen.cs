namespace game_of_life.Scenes;

using System;
using gock;

class RootScreen : ScreenObject
{
    private ScreenSurface mainSurface;
    private GOL game;
    private ColoredGlyph glyph;
    private TimeSpan timeSinceLastFrame;
    private TimeSpan delay = new TimeSpan(5000000L);

    public RootScreen()
    {
        mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        game = new();
        glyph = new ColoredGlyph(foreground: Color.Green, background: Color.Transparent, glyph: '#');
        game.Draw(mainSurface, glyph);

        Children.Add(mainSurface);
    }

    public override void Update(TimeSpan delta)
    {
        timeSinceLastFrame += delta;
        if (timeSinceLastFrame >= delay) {
            timeSinceLastFrame = TimeSpan.Zero;
            mainSurface.Clear();
            game.Update();
            game.Draw(mainSurface, glyph);
        }
    }
}
