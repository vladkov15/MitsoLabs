using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Laba1
{
    public partial class MainWindow : Window
    {
        public static Dictionary<string, int[]> Dictionary => new Dictionary<string, int[]>
        {
            {"CD", new[] {1, 0, 0, 1, 0}},
            {"DVD", new[] {0, 1, 1, 0, 1}},
            {"Blu-ray", new[] {1, 0, 1, 0, 0}},
            {"Flash", new[] {0, 0, 1, 0, 1}}
        };
        
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private static int GetDistance(IEnumerable<int> input, string key)
        {
            return input.Select((value, index) => Math.Abs(value - Dictionary[key][index])).Sum();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var results = new Dictionary<string, List<double>>();
            
            var input = new[] {Sign1, Sign2, Sign3, Sign4, Sign5}
                .Select(x => x.IsChecked.GetValueOrDefault())
                .Select(Convert.ToInt32)
                .ToArray();
            
            foreach (var pair in Dictionary)
            {
                var calculations = Enumerable.Range(1, 7).Select(x => GetMarkByFunction(input, pair.Key, x)).ToList();
                calculations.Insert(0, GetDistance(input, pair.Key));
                results.Add(pair.Key, calculations);
            }

            var minDist = results
                .GroupBy(x => x.Value[0])
                .OrderBy(x => x.Key)
                .FirstOrDefault()?
                .Select(x => x.Key);
            
            ResultGrid.ItemsSource = results;
            
            if (minDist != null)
            {
                Results.Text = string.Join("\n", minDist);
            }
        }

        private static double GetMarkByFunction(IReadOnlyList<int> input, string key, int func)
        {
            double a = 0;
            double b = 0;
            double g = 0;
            double h = 0;
            
            for (var i = 0; i < input.Count; i++) 
            {
                a += Dictionary[key][i] * input[i];
                b += (1 - Dictionary[key][i]) * (1 - input[i]);
                g += input[i] * (1 - Dictionary[key][i]) ;
                h += (1 - input[i]) * Dictionary[key][i];
            }
            
            switch (func)
            {
                case 1:
                    return a / (a + b + g + h);
                case 2:
                    return a / (a + g + h);
                case 3:
                    return a / (2 * a + g + h);
                case 4:
                    return a / (a + 2 * (g + h));
                case 5:
                    return (a + b) / (a + b + g + h);
                case 6:
                    return a / (g + h);
                case 7:
                    return (a * b - g * h) / (a * b + g * h);
                default:
                    return 0;
            }
        }
    }
}
