using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
//using System.Threading;

namespace Sudoku
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextBox[,] squares;
        public Key[] digit_keys, arrows;
        public MainWindow()
        {
            InitializeComponent();
            digit_keys = new Key[9] {Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9};
            arrows = new Key[4] { Key.Left, Key.Up, Key.Right, Key.Down };
            squares = new TextBox[9, 9];
            int i = 0, j = 0;
            foreach (UIElement elem in grid2.Children) 
            {
                if (elem is Grid) 
                {
                    foreach (UIElement textbox in (elem as Grid).Children) 
                    {
                        i = Convert.ToInt32((textbox as TextBox).Name[8].ToString()); // majick nomber 1
                        j = Convert.ToInt32((textbox as TextBox).Name[9].ToString()); // majick nomner 1 + 1
                        squares[i, j] = (textbox as TextBox);
                        squares[i, j].PreviewKeyDown += TextBox_PreviewKeyDown;
                        squares[i, j].GotFocus += TextBox_GotFocus;
                        squares[i, j].LostFocus += TextBox_LostFocus;
                        squares[i, j].CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        /*try
                        {
                            squares[i, j] = (textbox as TextBox);
                        }
                        catch (Exception exept) 
                        {
                            MessageBox.Show("Error:" + exept.Message);
                        }*/
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sudoku_solver core = new Sudoku_solver();
            for (int i = 0; i < 9; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    if (!squares[i, j].Text.Equals(""))
                    {
                        core[i, j] = Convert.ToInt32(squares[i, j].Text);
                    }
                    else 
                    {
                        core[i, j] = 0;
                    }
                }
            }
            if (core.is_contradictive()) 
            {
                MessageBox.Show("Conradictive!");
            }
            return;
        }

        private void grid1_Drop(object sender, DragEventArgs e)
        {
            string filename = ((e.Data.GetData("FileDrop")) as string[])[0];
            string[] table = File.ReadAllLines(filename);
            /*string ans = "";
            foreach (string str in table) 
            {
                ans += str + "\n";
            }
            MessageBox.Show(ans);*/
            char[] C = {'1', '2', '3', '4', '5', '6', '7', '8', '9', ' '};
            if (table.Length == 9 && table.All(str => str.Length == 9)) 
            {
                if ((table.All(obj => obj.All(b => C.Contains(b))))) 
                {
                    MessageBox.Show("Now this is podracing!");
                    //заполнить судоку этими данными
                }
            }
            return;
        }

        private void grid1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("FileDrop"))
            {
                e.Effects = DragDropEffects.None;
            }
            else 
            {
                //может, надо по размеру и расширению "*.sud" смотреть? 
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Background = new SolidColorBrush(Colors.Beige);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Background = new SolidColorBrush(Colors.White);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int i = 0, j = 0;
            if (arrows.Contains(e.Key))
            {
                i = Convert.ToInt32((sender as TextBox).Name[8].ToString());
                j = Convert.ToInt32((sender as TextBox).Name[9].ToString());
                /*if (e.Key == Key.Down && i != 8) 
                {
                    Keyboard.Focus(squares[i + 1, j]);
                }
                if (e.Key == Key.Up && i != 0) 
                {
                    Keyboard.Focus(squares[i - 1, j]);
                }
                if (e.Key == Key.Left && j != 0) 
                {
                    Keyboard.Focus(squares[i, j-1]);
                }
                if (e.Key == Key.Right && j != 8) 
                {
                    Keyboard.Focus(squares[i, j+1]);
                }*/
                if (e.Key == Key.Down && i != 8)
                {
                    Keyboard.Focus(squares[i + 1, j]);
                }
                if (e.Key == Key.Up && i != 0)
                {
                    Keyboard.Focus(squares[i - 1, j]);
                }
                if (e.Key == Key.Left && j != 0)
                {
                    Keyboard.Focus(squares[i, j - 1]);
                }
                if (e.Key == Key.Right && j != 8)
                {
                    Keyboard.Focus(squares[i, j + 1]);
                }
                if (e.Key == Key.Right && j == 8 && i != 8) 
                {
                    Keyboard.Focus(squares[i + 1, 0]);
                }
            }
            else 
            {
                if (digit_keys.Contains(e.Key))
                {
                    i = Convert.ToInt32((sender as TextBox).Name[8].ToString());
                    j = Convert.ToInt32((sender as TextBox).Name[9].ToString());
                    squares[i, j].Text = e.Key.ToString().Remove(0, 1);
                    //(sender as TextBox).Text = e.Key.ToString().Remove(0, 1); 
                    e.Handled = true;
                    if (j != 8)
                    {
                        Keyboard.Focus(squares[i, j + 1]);
                    }
                    else 
                    {
                        if (i != 8) 
                        {
                            Keyboard.Focus(squares[i + 1, 0]);
                        }
                    }
                }
                else 
                {
                    e.Handled = true;
                }
            }
        }
    }
}

