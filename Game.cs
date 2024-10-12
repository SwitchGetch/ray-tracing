using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

public static class MouseMove
{
    public static Vector2f Difference = new Vector2f();

    public static void CountDifference()
    {
        Difference = Game.MousePosition - Game.WindowCenter;
    }
}

public static class Game
{
    private static RenderWindow Window = new RenderWindow(new VideoMode(1200, 900), "");
    private static Sprite Canvas = new Sprite(new Texture(Window.Size.X, Window.Size.Y));
    private static Camera Camera = new Camera();

    private static Font Font = new Font("arial.ttf");
    private static Text Text = new Text("", Font, 20);

    public static Vector2f WindowSize { get => (Vector2f)Window.Size; }
    public static Vector2f WindowCenter { get => (Vector2f)Window.Size / 2; }
    public static Vector2f MousePosition { get => (Vector2f)Mouse.GetPosition(Window); }
    public static Color RandomColor { get => new Color((byte)new Random().Next(0, 256), (byte)new Random().Next(0, 256), (byte)new Random().Next(0, 256)); }

    private static bool FreeMouseCursor = false;

    private static Shader Shader = new Shader(null, null, "shader.frag");
    private static RenderStates renderStates = new RenderStates(Shader);

    private static Stopwatch Timer = new Stopwatch();

    private static float _TotalTime = 0;
    private static float _DeltaTime = 0;
    private static uint _FPS = 0;
    public static float TotalTime { get => _TotalTime; private set => _TotalTime = value; }
    public static float DeltaTime { get => _DeltaTime; private set => _DeltaTime = value; }
    public static uint FPS { get => _FPS; private set => _FPS = value; }

    private static void Start()
    {
        Window.Closed += (s, e) => Window.Close();

        Window.KeyPressed += (s, e) =>
        {
            switch(e.Code)
            {
                case Keyboard.Key.LAlt:

                    FreeMouseCursor = !FreeMouseCursor;

                    if (FreeMouseCursor)
                    {
						Window.SetMouseCursorVisible(true);
					}
                    else
                    {
						Window.SetMouseCursorVisible(false);
						Mouse.SetPosition((Vector2i)WindowCenter, Window);
					}

                    break;
            }
        };

        Window.Resized += (s, e) => Shader.SetUniform("u_resolution", WindowSize);

        Window.SetFramerateLimit(60);

        Window.SetMouseCursorVisible(false);

        Shader.SetUniform("u_resolution", WindowSize);
    }

    private static void Update()
    {
        CountDeltaTime();
        Window.DispatchEvents();

        if (!FreeMouseCursor)
        {
			MouseMove.CountDifference();
			Mouse.SetPosition((Vector2i)WindowCenter, Window);
		}

        Camera.Move();
        Camera.Rotate();

        Shader.SetUniform("u_cameraPosition", Camera.Position);
        Shader.SetUniform("u_cameraAngle", Camera.Angle);

        Text.DisplayedString = Camera.Stats + "\n\n FPS: " + FPS;

        Shader.SetUniform("u_time", TotalTime);
        FPS = (uint)(1 / DeltaTime);
    }

    private static void Render()
    {
        Window.Clear();

        Window.Draw(Canvas, renderStates);

        Window.Draw(Text);

        Window.Display();
    }

    public static void Run()
    {
        Start();

        while (Window.IsOpen)
        {
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