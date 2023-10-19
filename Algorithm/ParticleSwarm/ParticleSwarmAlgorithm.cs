using System;
using System.Collections.Generic;
using System.Linq;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm.ParticleSwarm;

public class ParticleSwarmAlgorithm: OptimizationAlgorithm<float>
{
    
    public class Parameters: OptimizationParameters<float>
    {
        public float MaxVelocity = 100;

        public float PhiLocal = 3f;
        public float PhiGlobal = 3f;
    }

    private readonly Parameters _params;

    private readonly List<Particle> _particles;

    public ParticleSwarmAlgorithm(IMathExpression expression, Parameters @params) : base(expression)
    {
        _params = @params;
        _particles = new List<Particle>(_params.Population);
    }

    public override Solution<float> Optimize()
    {
        Particle.ResetBest();
        CreateParticles();
        for (var i = 0; i < _params.Generations; i++)
        {
            NextIteration();
        }
        return Particle.GlobalBest;
    }

    private void NextIteration()
    {
        foreach (var particle in _particles)
        {
            particle.NextIteration();
        }
    }

    private void CreateParticles()
    {
        for (var i = 0; i < _params.Population; i++)
        {
            _particles.Add(CreateParticle());
        }
    }

    private Particle CreateParticle() => new(Expression, _params, RandomPositionVector(), RandomVelocityVector());

    private Position RandomPositionVector() => new(RandomX1(), RandomX2());
    private Position RandomVelocityVector() => new(RandomVelocity(), RandomVelocity());
    
    private float RandomVelocity() => RandomInRange(0, _params.MaxVelocity);

    protected override float RandomX1() => RandomInRange(_params.X1Min, _params.X1Max);
    protected override float RandomX2() => RandomInRange(_params.X2Min, _params.X2Max);
    private static float RandomInRange(float min, float max) => Random.Shared.NextSingle() * (max - min) + min;
}