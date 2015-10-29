using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPF_calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Regex reg = new Regex(@"\d");
       
        bool insideFigure=false;
        bool alreadyHaveOperation = false;
        bool havePoint = false;
        bool sumEqual = false;
        
        string figure;
        char oper;

        float fig1;
        float fig2;
        float sum;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (Grid.GetRow(b) == 1)
            {
                SpecialComandsButtons((string)b.Content);
                return;
            }

            if (reg.IsMatch((string)b.Content))
            {
                FigureButton((string)b.Content);
                return;
            }
            else
            {
                OperationButton((string)b.Content);
                return;
            }
        }

        #region Commands
        public void SpecialComandsButtons(string s) {
            
        }

        public void FigureButton(string s) {
            if (sumEqual) {
                sumEqual = false;
                TboxOperations.Clear();
            }
            
            if (!insideFigure)
            {
                figure = null;
                insideFigure = true;
            }

            TboxOperations.AppendText(s);
            figure += s;
        }

        public void OperationButton(string s)
        {
            if (s == ",")
            {  
                if (!havePoint)
                {
                    if (figure == null)
                    {
                        figure += "0" + s;
                        TboxOperations.AppendText(figure);
                    }
                    else
                    {
                        figure += s;
                        TboxOperations.AppendText(s);
                    }
                    havePoint = true;
                }
                return;
            }

                havePoint=false;// не точка - новая цифра
                //в любом случае принимает знак
           
            if (s!= "=" &&!alreadyHaveOperation){//оператора еще не было
            oper = Convert.ToChar(s);

            if (!sumEqual)
            fig1 = FigureConvert();

            ZeroingBool();
            alreadyHaveOperation=true;
            TboxOperations.AppendText("\n"+s+"\n");
            
            
            return;
            }

            fig2 = FigureConvert();
            if (alreadyHaveOperation)//оператор уже было
                SumFound(s);
        }

        #endregion

        public float FigureConvert() {
            float temp = Convert.ToSingle(figure);
            figure = null;
            return temp;
        }

        public void ZeroingBool() {
            insideFigure = false;
            havePoint = false;
            alreadyHaveOperation = false;
            sumEqual = false;
        }
        public void SumFound(string s)
        {
            ZeroingBool();

            switch (oper) { 
                case '+':
                    sum = fig1 + fig2;
                 
                    break;
                case '-':
                    sum = fig1 - fig2;
                    break;
                case '*':
                    sum = fig1 * fig2;
                    break;
                case '/':
                    if (fig2 == 0)
                    {
                        TboxOperations.Clear();
                        MessageBox.Show("на ноль делить нельзя!");
                        return;
                    }
                    else
                        sum = fig1 / fig2;
                    break;
            }

            TboxOperations.Clear();
            TboxOperations.AppendText(sum.ToString());

            sumEqual = true;
            fig1 = sum;

            if (s != "=") {
                OperationButton(s);
            }
        }

    }
}
