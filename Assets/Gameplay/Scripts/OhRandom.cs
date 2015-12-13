using System;

public class OhRandom
{
    public static System.Random random;

    public static void Initialise()
    {
        if (random != null) return;
        random = new Random();
    }

    public static int Range(int a, int b)
    {
        Initialise();
        return random.Next() % (b - a) + a;
    }

    public static float Range(float a, float b)
    {
        Initialise();
        return (float)(random.NextDouble() * (b - a) + a);
    }
}
