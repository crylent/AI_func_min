using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AI_func_min.Expression;

namespace AI_func_min.Algorithm.Genetic;

public abstract class GeneticAlgorithm<T>: OptimizationAlgorithm<T> where T: INumber<T>
{
    public class Parameters: OptimizationParameters<T>
    {
        public float MutationStrength = 0.5f;
        public float MutationCurve = 3f;
    }

    protected readonly Parameters Params;

    private readonly Solution<T>[] _entities;
    private Solution<T> _bestSolution = null!;
    protected float Min;

    protected GeneticAlgorithm(IMathExpression expression, Parameters @params): base(expression)
    {
        Params = @params;
        _entities = new Solution<T>[Params.Population];
    }

    public override Solution<float> Optimize()
    {
        CreateFirstGeneration();
        for (var g = 0; g < Params.Generations; g++)
        {
            for (var parent = 0; parent < _entities.Length; parent++)
            {
                var secondParent = SelectSecondParent(_entities[parent]);
                var mutation = float.Pow(
                    Params.MutationStrength * (Params.Generations - g) / Params.Generations,
                    Params.MutationCurve);
                var child = MakeChild(_entities[parent], _entities[secondParent], mutation);
                if (Calculate(child) < Calculate(_entities[parent])) ReplaceParent(parent, child);
            }
        }
        return _bestSolution.Convert<float>();
    }

    protected abstract Solution<T> MakeChild(Solution<T> parent, Solution<T> otherParent, float mutation);

    private void CreateFirstGeneration()
    {
        for (var e = 0; e < _entities.Length; e++)
        {
            _entities[e] = new Solution<T>(RandomX1(), RandomX2());
            UpdateBestSolution(_entities[e]);
        }
    }

    private void UpdateBestSolution(Solution<T> newSolution)
    {
        var result = Calculate(newSolution);
        if (Min < Calculate(newSolution)) return;
        Min = result;
        _bestSolution = newSolution;
    }

    private void ReplaceParent(int parent, Solution<T> child)
    {
        _entities[parent] = child;
        UpdateBestSolution(child);
    }

    private int SelectSecondParent(Solution<T> firstParent)
    {
        var chances = new List<float>(Params.Population);
        foreach (var other in _entities)
        {
            if (other == firstParent) chances.Add(0);
            else chances.Add(1f / float.CreateChecked(Calculate(other) - Min));
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
}