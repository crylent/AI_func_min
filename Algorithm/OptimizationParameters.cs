namespace AI_func_min.Algorithm;

public class OptimizationParameters<T>
{
    public T X1Min = default!;
    public T X1Max = default!;
    public T X2Min = default!;
    public T X2Max = default!;

    public int Population = 50;
    public int Generations = 2000;
}