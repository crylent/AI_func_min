using System;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public class BinaryGeneticAlgorithm: GeneticAlgorithm<int>
{
    
    public BinaryGeneticAlgorithm(IMathExpression expression, Parameters @params) : base(expression, @params)
    {
        Min = int.MaxValue;
    }

    protected override Solution<int> MakeChild(Solution<int> parent, Solution<int> otherParent, float mutation)
    {
        var child = DoCrossover(parent, otherParent);
        child.X1 = DoMutation(child.X1, int.Max(Params.X1Max, -Params.X1Min), mutation);
        child.X2 = DoMutation(child.X2, int.Max(Params.X2Max, -Params.X2Min), mutation);
        return child;
    }

    private static Solution<int> DoCrossover(Solution<int> a, Solution<int> b)
    {
        var pos = Random.Shared.Next(0, 2 * sizeof(int)); // random crossover position
        return pos switch
        {
            sizeof(int) => new Solution<int>(a.X1, b.X2),
            < sizeof(int) => new Solution<int>(BitCrossover(a.X1, b.X2, pos), b.X2),
            _ => new Solution<int>(a.X1, BitCrossover(a.X2, b.X2, pos - sizeof(int)))
        };
    }

    private static int DoMutation(int x, int max, float probability)
    {
        for (var i = 0; i < int.Log2(max); i++)
        {
            if (Random.Shared.Next() > probability) continue;
            var bit = (x & (1 << i)) >> i; // get i-th bit of x
            x = bit == 1 ? x | (1 << i) : x & ~(1 << i); // invert i-th bit
        }
        return x;
    }

    private static int BitCrossover(int a, int b, int pos)
    {
        var backPos = sizeof(int) - pos;
        return (a >> backPos << backPos) + (b << pos);
    }

    protected override int RandomX1() => Random.Shared.Next(Params.X1Min, Params.X1Max + 1);
    protected override int RandomX2() => Random.Shared.Next(Params.X2Min, Params.X2Max + 1);
}