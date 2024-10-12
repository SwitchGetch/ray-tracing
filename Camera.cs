using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Camera
{
    private readonly float HalfPi = (float)Math.PI / 2;
    private readonly float Pi = (float)Math.PI;
    private readonly float TwoPi = (float)Math.PI * 2;

    public Vector3f Position = new Vector3f();
    public float MoveSpeed = 5;
    public float RotateSpeed = 0.25f;

    private Vector3f MoveDirection = new Vector3f(0, 0, 1);
	private Vector3f direction = new Vector3f(0, 0, 1);

    public Vector3f Direction
    {
        get => direction;
        set
        {
            direction = Vector.Normalize(value);

            float dot = direction.X * MoveDirection.X + direction.Z * MoveDirection.Z;
            angle.X = (float)Math.Acos(dot) * Math.Sign(direction.Y);
            angle.Y = (float)Math.Acos(MoveDirection.Z) * Math.Sign(-direction.X);
		}
    }

	private Vector2f angle = new Vector2f();

    public Vector2f Angle
    {
        get => angle;
        set
        {
            angle = value;

            if (angle.X > HalfPi) angle.X = HalfPi;
            else if (angle.X < -HalfPi) angle.X = -HalfPi;
            if (angle.Y > Pi) angle.Y -= TwoPi;
            else if (angle.Y < -Pi) angle.Y += TwoPi;

            direction = Vector.Rotate(Vector.Rotate(new Vector3f(0, 0, 1), angle.X, Axis.X), angle.Y, Axis.Y);
            MoveDirection = Vector.Rotate(new Vector3f(0, 0, 1), angle.Y, Axis.Y);
		}
    }

	public string Stats
    {
        get
        {
            return
                $"\n Position:\n X: {Position.X:F7}\n Y: {Position.Y:F7}\n Z: {Position.Z:F7}" +
                $"\n\n Direction:\n X: {Direction.X:F7}\n Y: {Direction.Y:F7}\n Z: {Direction.Z:F7}" +
                $"\n\n Move Direction:\n X: {MoveDirection.X:F7}\n Z: {MoveDirection.Z:F7}" +
                $"\n\n Rotate Angle:\n X: {Angle.X:F7}\n Y: {Angle.Y:F7}" +
                $"\n\n Move Speed: {MoveSpeed}\n\n Rotate Speed: {RotateSpeed}";
        }
    }

    private void AngleSetter() { }

    public void Move()
    {
        Vector3f D = new Vector3f();

        if (Keyboard.IsKeyPressed(Keyboard.Key.W)) D += MoveDirection;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S)) D -= MoveDirection;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) D += new Vector3f(-MoveDirection.Z, 0, MoveDirection.X);
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) D -= new Vector3f(-MoveDirection.Z, 0, MoveDirection.X);
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) D.Y++;
        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) D.Y--;

        Position += Game.DeltaTime * MoveSpeed * Vector.Normalize(D);
    }

    public void Rotate()
    {
        if (MouseMove.Difference.X == 0 && MouseMove.Difference.Y == 0) return;

        Angle += -Game.DeltaTime * RotateSpeed * new Vector2f(MouseMove.Difference.Y, MouseMove.Difference.X);


		/*float k = -Game.DeltaTime * RotateSpeed;

        if (MouseMove.Difference.X != 0)
        {
            float ay = k * MouseMove.Difference.X;
            Direction = Vector.Rotate(Direction, ay, Axis.Y);
            angle.Y += ay;
        }

        if (MouseMove.Difference.Y != 0)
        {
            float ax = k * MouseMove.Difference.Y;
            Direction = Vector.Rotate(Direction, ax, Axis.X);
            angle.X += ax;
        }*/
    }
}