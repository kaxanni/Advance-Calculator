using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Variables;
using Parsing;
using Expression;

namespace Evaluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Variable _vars;
        public MainWindow()
        {
            InitializeComponent();

            _vars = new Variable();
        }
        //para mo gana ang button na Evaluate
        private void evaluate_click(object sender, RoutedEventArgs e)
        {
            if(exp_tb.Text == string.Empty)
            {
                return;
            }
            string[] data = Parser.parse(exp_tb.Text);

            if(data == null)
            {
                out_tb.Text += "Error: Invalid expression!\n";
                return;
            }

            if(data[0].ToLower() == "print")
            {
                for(int i = 1;i< data.Length; i++)
                {
                    try
                    {
                        double x = _vars.get(data[i]);

                        out_tb.Text += string.Format("{0} = {1}\n", data[i], x);
                    }
                    catch(Exception ex)
                    {
                        out_tb.Text += (ex.Message + "\n");
                    }
                }

                return;
            }
            else if(data[0].ToLower() == "status")
            {
                out_tb.Text += _vars.ToString();
                return;
            }

            if(data.Length != 2) {
                out_tb.Text += "Error: Invalid expression!\n";
                return;
            }

            out_tb.Text += exp_tb.Text + "\n";

            string[] _parsed = Parser.parseExpression(data[1]);

            for (int i = 0; i < _parsed.Length; i++)
            {
                if (!Parser.IsNonVariable(_parsed[i]) && !Double.TryParse(_parsed[i], out double _) && !Parser.unaryOperator(_parsed[i]))
                {
                    try
                    {
                        _parsed[i] = _vars.get(_parsed[i]).ToString(); 
                    }
                    catch(Exception ex)
                    {
                        out_tb.Text += (ex.Message + "\n");
                        return;
                    }
                }
            }

            InfixToPostfixConverter ipc = new InfixToPostfixConverter(_parsed);

            EvaluatePostfix evaluate = new EvaluatePostfix(ipc.convert());

            _vars.add(data[0], evaluate.evaluate());

            exp_tb.Clear();
        }
    }
}
