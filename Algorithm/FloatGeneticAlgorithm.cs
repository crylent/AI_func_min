using System;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public class FloatGeneticAlgorithm: GeneticAlgorithm<float>
{

    public FloatGeneticAlgorithm(IMathExpression expression, Parameters @params) : base(expression, @params)
    {
        Min = float.MaxValue;
    }

    protected override Solution<float> MakeChild(Solution<float> parent, Solution<float> otherParent, float mutation)
    {
        var child = DoCrossover(parent, otherParent);
        child.X1 = DoMutation(child.X1, RandomX1(), mutation);
        child.X2 = DoMutation(child.X2, RandomX2(), mutation);
        return child;
    }

    private static Solution<float> DoCrossover(Solution<float> a, Solution<float> b) => new(a.X1, b.X2);

    private static float DoMutation(float oldX, float newX, float mutation) => oldX * (1 - mutation) + newX * mutation;

    protected override float RandomX1() => RandomInRange(Params.X1Min, Params.X1Max);
    protected override float RandomX2() => RandomInRange(Params.X2Min, Params.X2Max);
    private static float RandomInRange(float min, float max) => Random.Shared.NextSingle() * (max - min) + min;
}