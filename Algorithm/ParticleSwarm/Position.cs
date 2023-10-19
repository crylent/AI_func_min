using System.Numerics;

namespace AI_func_min.Algorithm.ParticleSwarm;

public class Position: Solution<float>,
    IAdditionOperators<Position, Position, Position>,
    ISubtractionOperators<Position, Position, Position>,
    IUnaryNegationOperators<Position, Position>,
    IMultiplyOperators<Position, float, Position>
{
    public Position(float x1, float x2) : base(x1, x2)
    {
    }
    
    public static Position operator +(Position left, Position right) => new(left.X1 + right.X1, left.X2 + right.X2);
    public static Position operator -(Position left, Position right) => new(left.X1 - right.X1, left.X2 - right.X2);
    public static Position operator -(Position value) => new(-value.X1, -value.X2);
    public static Position operator *(Position left, float right) => new(left.X1 * right, left.X2 * right);
}