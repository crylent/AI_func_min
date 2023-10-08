using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public class Solution
{
    public float X1, X2;

    public Solution(float x1, float x2)
    {
        X1 = x1;
        X2 = x2;
    }

    public float Calculate(IMathExpression expression) => expression.Calculate(X1, X2);
}