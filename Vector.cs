using SFML.System;

public static class Vector
{
    public static float Length(Vector3f V) => (float)Math.Sqrt(V.X * V.X + V.Y * V.Y + V.Z * V.Z);

    public static Vector3f Normalize(Vector3f V) { float l = Length(V); return l == 0 ? V : V / l; }

    public static float ScalarProduct(Vector3f V1, Vector3f V2) => V1.X * V2.X + V1.Y * V2.Y + V1.Z * V2.Z;

    public static Vector3f Rotate(Vector3f P, float Angle, Axis Axis, Vector3f Relative = new Vector3f())
    {
        float Sin = (float)Math.Sin(Angle);
        float Cos = (float)Math.Cos(Angle);

        P -= Relative;

        switch (Axis)
        {
            case Axis.X: P = new Vector3f(P.X, P.Y * Cos - P.Z * Sin, P.Y * Sin + P.Z * Cos); break;
            case Axis.Y: P = new Vector3f(P.X * Cos + P.Z * Sin, P.Y, P.Z * Cos - P.X * Sin); break;
            case Axis.Z: P = new Vector3f(P.X * Cos - P.Y * Sin, P.X * Sin + P.Y * Cos, P.Z); break;
        }

        return P + Relative;
    }
}

public enum Axis { X, Y, Z }