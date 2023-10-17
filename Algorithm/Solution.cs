using System.Numerics;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public class Solution<T> where T: INumber<T>
{
    public T X1, X2;

    public Solution(T x1, T x2)
    {
        X1 = x1;
        X2 = x2;
    }

    public float Calculate(IMathExpression expression) => expression.Calculate(
        float.CreateChecked(X1), 
        float.CreateChecked(X2)
        );

    public Solution<TNew> Convert<TNew>() where TNew : INumber<TNew>
        => new(TNew.CreateChecked(X1), TNew.CreateChecked(X2));
}