using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

public static class Game
{
    private static RenderWindow Window = new RenderWindow(new VideoMode(1000, 1000), "");

    private static Sprite Canvas = new Sprite(new Texture(Window.Size.X, Window.Size.Y));

    public static Vector2f WindowSize { get => (Vector2f)Window.Size; }
    public static Color RandomColor { get => new Color((byte)new Random().Next(0, 256), (byte)new Random().Next(0, 256), (byte)new Random().Next(0, 256)); }

    private static Shader shader = new Shader(null, null, "shader.frag");
    private static RenderStates renderStates = new RenderStates(shader);

    private static Stopwatch Timer = new Stopwatch();
    private static float TotalTime = 0;
    private static float DeltaTime = 0;

    private static void Start()
    {
        Window.Closed += (s, e) => Window.Close();

        Window.Resized += (s, e) => shader.SetUniform("u_resolution", WindowSize);

        shader.SetUniform("u_resolution", WindowSize);
    }

    private static void Update()
    {
        Window.DispatchEvents();

        shader.SetUniform("u_totalTime", TotalTime);

        Window.SetTitle("FPS: " + 1 / DeltaTime);
    }

    private static void Render()
    {
        Window.Clear();

        Window.Draw(Canvas, renderStates);

        Window.Display();
    }

    public static void Run()
    {
        Start();

        while (Window.IsOpen)
        {
            CountDeltaTime();

            Update();

            Render();
        }
    }

    private static void CountDeltaTime()
    {
        DeltaTime = (float)Timer.Elapsed.TotalSeconds;
        TotalTime += DeltaTime;
        Timer.Restart();
    }
}