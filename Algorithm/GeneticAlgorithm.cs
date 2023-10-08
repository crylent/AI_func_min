using System;
using System.Collections.Generic;
using System.Linq;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm;

public class GeneticAlgorithm
{
    private readonly IMathExpression _expression;

    public class Parameters
    {
        public float X1Min = -5;
        public float X1Max = 5;
        public float X2Min = -5;
        public float X2Max = 5;

        public int Population = 50;
        public int Generations = 2000;

        public float MutationStrength = 0.5f;
        public float MutationCurve = 3f;
    }

    private readonly Parameters _params;

    public GeneticAlgorithm(IMathExpression expression, Parameters @params)
    {
        _expression = expression;
        _params = @params;
        _entities = new Solution[_params.Population];
    }

    private readonly Solution[] _entities;
    private float _min = float.MaxValue;
    private Solution _bestSolution;

    public Solution Optimize()
    {
        CreateFirstGeneration();
        for (var g = 0; g < _params.Generations; g++)
        {
            foreach (var parent in _entities)
            {
                var secondParent = SelectSecondParent(parent);
                var mutation = float.Pow(
                    _params.MutationStrength * (_params.Generations - g) / _params.Generations,
                    _params.MutationCurve);
                var child = MakeChild(parent, _entities[secondParent], mutation);
                if (Calculate(child) < Calculate(parent)) ReplaceParent(secondParent, child);
            }
        }
        return _bestSolution;
    }

    private void CreateFirstGeneration()
    {
        for (var e = 0; e < _entities.Length; e++)
        {
            _entities[e] = new Solution(RandomX1(), RandomX1());
            UpdateBestSolution(_entities[e]);
        }
    }

    private void ReplaceParent(int parent, Solution child)
    {
        _entities[parent] = child;
        UpdateBestSolution(child);
    }

    private int SelectSecondParent(Solution firstParent)
    {
        var chances = new List<float>(_params.Population);
        foreach (var other in _entities)
        {
            if (other == firstParent) chances.Add(0);
            else chances.Add(1f / (Calculate(other) - _min));
        }
        var sum = chances.Sum();
        var rand = Random.Shared.NextSingle() * sum;
        float x;
        int i;
        for (i = 0, x = 0f; i < chances.Count && x < rand; i++)
        {
            x += chances[i];
        }

        if (i == chances.Count) return i - 1;
        return i;
    }

    private Solution MakeChild(Solution parent, Solution otherParent, float mutation)
    {
        var child = DoCrossover(parent, otherParent);
        child.X1 = DoMutation(child.X1, RandomX1(), mutation);
        child.X2 = DoMutation(child.X2, RandomX2(), mutation);
        return child;
    }

    private static Solution DoCrossover(Solution a, Solution b) => new(a.X1, b.X2);

    private float DoMutation(float oldX, float newX, float mutation) => oldX * (1 - mutation) + newX * mutation;

    private void UpdateBestSolution(Solution newSolution)
    {
        var result = Calculate(newSolution);
        if (_min < Calculate(newSolution)) return;
        _min = result;
        _bestSolution = newSolution;
    }

    private float RandomX1() => RandomInRange(_params.X1Min, _params.X1Max);
    private float RandomX2() => RandomInRange(_params.X2Min, _params.X2Max);
    private static float RandomInRange(float min, float max) => Random.Shared.NextSingle() * (max - min) + min;

    private float Calculate(Solution solution) => solution.Calculate(_expression);
}