using System.Collections.Generic;

public struct Coordinate
{
    public int X;
    public int Y;

    public Coordinate(int _x, int _y)
    {
        X = _x;
        Y = _y;
    }

    public override bool Equals(object obj)
    {
        return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return X * 1000 + Y;
    }

    public static bool operator ==(Coordinate A, Coordinate B)
    {
        return A.X == B.X && A.Y == B.Y;
    }

    public static bool operator !=(Coordinate A, Coordinate B)
    {
        return !(A.X == B.X && A.Y == B.Y);
    }

    public static Coordinate Get(int _X, int _Y)
    {
        return CoordiantePool.GetCoordinate(_X, _Y);
    }
}

public class CoordinateList : List<Coordinate>
{
    public bool Contains(Coordinate coordinate)
    {
        foreach (var item in this)
        {
            if (item.Equals(coordinate))
                return true;
        }
        return false;
    }
}

public class CoordinateDictionary<T> : Dictionary<Coordinate,T>
{
    public bool ContainsKey(Coordinate coordinate)
    {
        foreach (var item in this)
        {
            if (item.Key.Equals(coordinate))
                return true;
        }
        return false;
    }
}

public static class CoordiantePool
{
    static Dictionary<int, Coordinate> keyValues = new Dictionary<int, Coordinate>();
    public static Coordinate GetCoordinate(int X, int Y)
    {
        if (!keyValues.ContainsKey(X * 1000 + Y))
            keyValues.Add(X * 1000 + Y, new Coordinate(X, Y));
        return keyValues[X * 1000 + Y];
    }
}
