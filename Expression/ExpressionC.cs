namespace AI_func_min.Expression;

public class ExpressionC:IMathExpression
{
    public float Calculate(float x1, float x2) =>
       4 * float.Pow(x1 - 5, 2) + float.Pow(x2 - 6, 2);
}