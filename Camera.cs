using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Camera
{
    public Vector3f Position = new Vector3f();
    public Vector3f Direction = new Vector3f(0, 0, 1);
    public float MoveSpeed = 5;
    public float RotateSpeed = 0.5f;

    private Vector3f Forward = new Vector3f(0, 0, 1);
    private Vector3f Backward = new Vector3f(0, 0, -1);
    private Vector3f Right = new Vector3f(1, 0, 0);
    private Vector3f Left = new Vector3f(-1, 0, 0);
    private Vector3f Up = new Vector3f(0, 1, 0);
    private Vector3f Down = new Vector3f(0, -1, 0);

    private Vector2f _Angle = new Vector2f();
    public Vector2f Angle { get => _Angle; private set => _Angle = value; }

    public string Stats
    {
        get
        {
            return
                "\n Position: " + "\n X: " + Position.X + "\n Y: " + Position.Y + "\n Z: " + Position.Z +
                "\n\n Direction: " + "\n X: " + Direction.X + "\n Y: " + Direction.Y + "\n Z: " + Direction.Z +
                "\n\n Angle: " + "\n X: " + Angle.X + "\n Y: " + Angle.Y +
                "\n\n MoveSpeed: " + MoveSpeed + "\n\n RotateSpeed: " + RotateSpeed + "\n\n FPS: " + Game.FPS;
        }
    }

    public void Move()
    {
        Vector3f D = new Vector3f();

        if (Keyboard.IsKeyPressed(Keyboard.Key.W)) D += Forward;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S)) D += Backward;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) D += Left;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) D += Right;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) D += Up;
        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) D += Down;

        Position += Game.DeltaTime * MoveSpeed * Vector.Normalize(D);
    }

    public void Rotate()
    {
        if (MouseMove.Difference.X == 0 && MouseMove.Difference.Y == 0) return;

        float k = -Game.DeltaTime * RotateSpeed;

        if (MouseMove.Difference.X == 0) return;

        float ay = k * MouseMove.Difference.X;
        Direction = Vector.Rotate(Direction, ay, Axis.Y);
        _Angle.Y += ay;

        Forward = Vector.Rotate(Forward, ay, Axis.Y);
        Backward = -Forward;
        Right = Vector.Rotate(Right, ay, Axis.Y);
        Left = -Right;

        if (MouseMove.Difference.Y == 0) return;

        float ax = k * MouseMove.Difference.Y;
        Direction = Vector.Rotate(Direction, ax, Axis.X);
        _Angle.X += ax;
    }
}