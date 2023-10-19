using System.Numerics;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public abstract class OptimizationAlgorithm<T>: IOptimizationWrapper where T: INumber<T>
{
    protected readonly IMathExpression Expression;

    protected OptimizationAlgorithm(IMathExpression expression)
    {
        Expression = expression;
    }
    
    public abstract Solution<float> Optimize();

    protected abstract T RandomX1();
    protected abstract T RandomX2();

    protected float Calculate(Solution<T> solution) => solution.Calculate(Expression);
}