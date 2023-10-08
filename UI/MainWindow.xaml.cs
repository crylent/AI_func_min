using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AI_func_min.Algorithm;
using AI_func_min.Expression;

namespace AI_func_min.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMathExpression _expression = new ExpressionA();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FindMinimum(object sender, RoutedEventArgs routedEventArgs)
        {
            var algorithm = new GeneticAlgorithm(_expression, new GeneticAlgorithm.Parameters
            {
                X1Min = ParseFloat(X1Min), X1Max = ParseFloat(X1Max),
                X2Min = ParseFloat(X2Min), X2Max = ParseFloat(X2Max),
                Population = ParseInt(Population), Generations = ParseInt(Generations),
                MutationStrength = ParseFloat(MutationStrength),
                MutationCurve = ParseFloat(MutationCurve)
            });
            Mouse.OverrideCursor = Cursors.Wait;
            var r = algorithm.Optimize();
            Mouse.OverrideCursor = Cursors.Arrow;
            Result.Text = $"x₁ = {r.X1}\nx₂ = {r.X2}\nmin = {r.Calculate(_expression)}";
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