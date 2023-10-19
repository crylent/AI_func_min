using System;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm.ParticleSwarm;

public class Particle
{
    private readonly IMathExpression _expression;
    private readonly ParticleSwarmAlgorithm.Parameters _params;
    private readonly float _velocityFactor;
    
    private Position _position;
    private Position _velocity;

    private Position _bestPosition;
    private float _min;

    public static Position GlobalBest { get; private set; } = null!;
    private static float _globalMin;

    public Particle(IMathExpression expression, ParticleSwarmAlgorithm.Parameters @params, Position position, Position velocity)
    {
        _expression = expression;
        _params = @params;
        var phi = _params.PhiLocal + _params.PhiGlobal;
        _velocityFactor = 1f / float.Abs(2 - phi - float.Sqrt(float.Pow(phi, 2) - 4 * phi));
        _position = position;
        _velocity = velocity;
        _bestPosition = position;
        _min = _expression.Calculate(position.X1, position.X2);

        if (GlobalBest != null! && _min >= _globalMin) return;
        GlobalBest = position;
        _globalMin = _min;
    }

    public void NextIteration()
    {
        var posDiffLocal = _bestPosition - _position;
        var posDiffGlobal = GlobalBest - _position;
        _velocity += posDiffLocal * _params.PhiLocal * Random.Shared.NextSingle()
                     + posDiffGlobal * _params.PhiGlobal * Random.Shared.NextSingle();
        _velocity *= _velocityFactor;

        _position += _velocity;
        var value = _position.Calculate(_expression);
        if (value >= _min) return;
        _bestPosition = _position;
        _min = value;
        if (value >= _globalMin) return;
        GlobalBest = _position;
        _globalMin = value;
    }

    public static void ResetBest()
    {
        GlobalBest = null!;
    }
}