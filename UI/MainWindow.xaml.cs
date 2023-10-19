using System;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Input;
using AI_func_min.Algorithm;
using AI_func_min.Algorithm.Genetic;
using AI_func_min.Algorithm.ParticleSwarm;
using AI_func_min.Expression;

namespace AI_func_min.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IMathExpression _expression = new ExpressionA();

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void FindMinimum(object? sender, EventArgs eventArgs)
        {
            IOptimizationWrapper algorithm = Algorithms.SelectedIndex switch
            {
                0 => new FloatGeneticAlgorithm(_expression,
                    new FloatGeneticAlgorithm.Parameters
                    {
                        X1Min = ParseFloat(X1Min),
                        X1Max = ParseFloat(X1Max),
                        X2Min = ParseFloat(X2Min),
                        X2Max = ParseFloat(X2Max),
                        Population = ParseInt(Population),
                        Generations = ParseInt(Generations),
                        MutationStrength = ParseFloat(MutationStrength),
                        MutationCurve = ParseFloat(MutationCurve)
                    }),
                1 => new BinaryGeneticAlgorithm(_expression,
                    new BinaryGeneticAlgorithm.Parameters
                    {
                        X1Min = ParseInt(X1Min),
                        X1Max = ParseInt(X1Max),
                        X2Min = ParseInt(X2Min),
                        X2Max = ParseInt(X2Max),
                        Population = ParseInt(Population),
                        Generations = ParseInt(Generations),
                        MutationStrength = ParseFloat(MutationStrength),
                        MutationCurve = ParseFloat(MutationCurve)
                    }),
                2 => new ParticleSwarmAlgorithm(_expression, 
                    new ParticleSwarmAlgorithm.Parameters 
                    {
                        X1Min = ParseFloat(X1Min),
                        X1Max = ParseFloat(X1Max),
                        X2Min = ParseFloat(X2Min),
                        X2Max = ParseFloat(X2Max),
                        Population = ParseInt(Population),
                        Generations = ParseInt(Generations)
                    }),
                _ => null!
            };
            Mouse.OverrideCursor = Cursors.Wait;
            var result = algorithm.Optimize();
            Mouse.OverrideCursor = Cursors.Arrow;
            Result.Text = $"x₁ = {result.X1}\nx₂ = {result.X2}\nmin = {result.Calculate(_expression)}";
        }

        private static float ParseFloat(TextBox textBox) => float.Parse(textBox.Text);
        private static int ParseInt(TextBox textBox) => int.Parse(textBox.Text);

        private void OnFloatParameterChanged(object sender, TextCompositionEventArgs e)
        {
            var textBox = (sender as TextBox)!;
            e.Handled = !float.TryParse(textBox.Text + e.Text, out _);
        }

        private void OnIntParameterChanged(object sender, TextCompositionEventArgs e)
        {
            var textBox = (sender as TextBox)!;
            e.Handled = !int.TryParse(textBox.Text + e.Text, out _);
        }

        private void OnFunctionSelected(object? sender, EventArgs eventArgs)
        {
            _expression = Functions.SelectedIndex switch
            {
                0 => new ExpressionA(),
                1 => new ExpressionB(),
                2 => new ExpressionC(),
                _ => _expression
            };
        }
        

        private void CommandManager_OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}