namespace AI_func_min.Expression;

public class ExpressionB:IMathExpression
{
    public float Calculate(float x1, float x2) =>
         float.Pow(x2 - float.Pow(x1, 2), 2) + 100 * float.Pow(1 - x1, 2);
}