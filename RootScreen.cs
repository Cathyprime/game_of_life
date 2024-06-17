namespace game_of_life.Scenes;

using System;
using gock;
using SadConsole.Input;

public class RootScreen : ScreenObject
{
    private TimeSpan displayDelay = new TimeSpan(0, 0, 2);
    private TimeSpan timeSinceLastFrame;
    private ScreenSurface mainSurface;
    private Speed speed = new Speed();
    private TimeSpan fadeTimer;
    private ColoredGlyph glyph;
    private bool pause = false;
    private GameOfLife game;

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
        if (timeSinceLastFrame >= speed.GetCurrentSpeed())
        {
            timeSinceLastFrame = TimeSpan.Zero;
            mainSurface.Clear();
            game.Update(pause);
            game.Draw(mainSurface, glyph);
        }

        if (fadeTimer != TimeSpan.Zero)
        {
            DrawMessage(speed.ToString()!);

            fadeTimer -= delta;
            if (fadeTimer <= TimeSpan.Zero)
            {
                fadeTimer = TimeSpan.Zero;
            }
        }
    }

    private void DrawMessage(in string message)
    {
        var startPosWidth = (int)(GameSettings.GAME_WIDTH * 0.75 - message.Length / 2);

        mainSurface.DrawBox(new Rectangle(startPosWidth, 2, message.Length + 2, 3), ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin,
                                                        new ColoredGlyph(Color.Violet, Color.Black)));

        mainSurface.Print(startPosWidth + 1, 3, message);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Space))
            pause = !pause;
        else if (keyboard.IsKeyPressed(Keys.OemPlus))
        {
            speed++;
            fadeTimer = displayDelay;
        }
        else if (keyboard.IsKeyPressed(Keys.OemMinus))
        {
            speed--;
            fadeTimer = displayDelay;
        }

        return base.ProcessKeyboard(keyboard);
    }
}

internal class Speed
{
    private int speedLevel = 3;
    private TimeSpan delay = new TimeSpan(5000000L);

    public override string? ToString()
    {
        return (speedLevel) switch
        {
            1 => "Speed: x0.25",
            2 => "Speed: x0.50",
            3 => "Speed: x1",
            4 => "Speed: x2",
            5 => "Speed: x4",
            _ => null
        };
    }

    public static Speed operator ++(Speed s)
    {
        if (s.speedLevel >= 5)
            return s;

        s.speedLevel++;
        return s;
    }

    public static Speed operator --(Speed s)
    {
        if (s.speedLevel <= 1)
            return s;

        s.speedLevel--;
        return s;
    }

    public TimeSpan? GetCurrentSpeed()
    {
        return (speedLevel) switch
        {
            1 => delay * 2 * 2,
            2 => delay * 2,
            3 => delay,
            4 => delay / 2,
            5 => delay / 2 / 2,
            _ => null
        };
    }
}
