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
using Microsoft.Win32;

namespace Sudoku
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sudoku_solver core;
        public TextBox[,] squares;
        public Key[] digit_keys, arrows;
        public Brush bruh;
        public Settings settings;
        //public bool auto_move_cursor, move_to_the_next_row, is_logging_on, mark_set_tiles;
        //public string logging_path;
        public string error_code = "", current_error = "";
        public void make_settings_file() 
        {
            try
            {
                StreamWriter stream = File.CreateText("settings.txt");
                stream.WriteLine("SETTINGS");
                stream.WriteLine("\"mark_set_tiles\"=True");
                stream.WriteLine("\"is_logging_on\"=False");
                stream.WriteLine(String.Format("\"logging_path\"=\"{0}\"", Environment.CurrentDirectory + "\\logging.txt"));
                stream.WriteLine("\"move_to_the_next_row\"=True");
                stream.WriteLine("\"auto_move_cursor\"=True");
                stream.WriteLine("\"step_by_step\"=False");
                stream.Close();
                settings.mark_set_tiles = true;
                settings.is_logging_on = false;
                settings.logging_path = Environment.CurrentDirectory + "\\logging.txt";
                settings.move_to_the_next_row = true;
                settings.auto_move_cursor = true;
                settings.step_by_step = false;
            }
            catch (Exception exept) 
            {
                MessageBox.Show("Error! " + exept.Message);
            }
        }
        public void read_settings_from_file() 
        {
            try
            {
                StreamReader stream = File.OpenText("settings.txt");
                string[] data = stream.ReadToEnd().Replace("\r", "").Split('\n');
                stream.Close();
                if (data.Length == 7) 
                {
                    settings.mark_set_tiles = Convert.ToBoolean(data[1].Split('=')[1]);
                    settings.is_logging_on = Convert.ToBoolean(data[2].Split('=')[1]);
                    settings.logging_path = data[3].Split('=')[1].Replace("\"", "");
                    settings.move_to_the_next_row = Convert.ToBoolean(data[4].Split('=')[1]);
                    settings.auto_move_cursor = Convert.ToBoolean(data[5].Split('=')[1]);
                    settings.step_by_step = Convert.ToBoolean(data[6].Split('=')[1]);
                }
            }
            catch (Exception exept) 
            {
                MessageBox.Show("Error! " + exept.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            settings = new Settings();
            if (!File.Exists("settings.txt"))
            {
                make_settings_file();
            }
            else 
            {
                read_settings_from_file();
            }
            bruh = textbox_00.BorderBrush;
            //KeyBinding fffuuuu1 = new KeyBinding(ApplicationCommands.NotACommand, new KeyGesture(Key.O, ModifierKeys.Control)); 
            //KeyBinding fffuuuu2 = new KeyBinding(ApplicationCommands.NotACommand, new KeyGesture(Key.S, ModifierKeys.Control));
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
                    }
                }
            }
            core = new Sudoku_solver(settings);
        }
        public void show_error_in_row(int n) 
        {
            for (int i = 0; i < 9; i++) 
            {
                squares[n, i].BorderBrush = new SolidColorBrush(Colors.Red);
            }
            current_error = "ROW_" + n.ToString();
            return;
        }
        public void show_error_in_column(int n)
        {
            for (int i = 0; i < 9; i++)
            {
                squares[i, n].BorderBrush = new SolidColorBrush(Colors.Red);
            }
            current_error = "COLUMN_" + n.ToString();
            return;
        }
        public void show_error_in_square(int n)
        {
            int a = n / 3, b = n % 3;
            for (int i = 3 * a; i < 3 * a + 3; i++) 
            {
                for (int j = 3 * b; j < 3 * b + 3; j++) 
                {
                    squares[i, j].BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            current_error = "SQUARE_" + n.ToString();
            return;
        }
        public void hide_error_in_row(int n) 
        {
            for (int i = 0; i < 9; i++)
            {
                squares[n, i].BorderBrush = bruh;
                current_error = "";
            }
            return;
        }
        public void hide_error_in_column(int n) 
        {
            for (int i = 0; i < 9; i++)
            {
                squares[i, n].BorderBrush = bruh;
                current_error = "";
            }
            return;
        }
        public void hide_error_in_square(int n)
        {
            int a = n / 3, b = n % 3;
            for (int i = 3 * a; i < 3 * a + 3; i++)
            {
                for (int j = 3 * b; j < 3 * b + 3; j++)
                {
                    squares[i, j].BorderBrush = bruh;
                    current_error = "";
                }
            }
            return;
        }
        public void hide_current_error() 
        {
            if (current_error.Equals(""))
                return;
            string[] words = current_error.Split('_');
            switch (words[0])
            {
                case "ROW":
                    {
                        hide_error_in_row(Convert.ToInt32(words[1]));
                        break;
                    }
                case "COLUMN":
                    {
                        hide_error_in_column(Convert.ToInt32(words[1]));
                        break;
                    }
                case "SQUARE":
                    {
                        hide_error_in_square(Convert.ToInt32(words[1]));
                        break;
                    }
            }
        }
        public void show_error_by_code(string code) 
        {
            string[] words = code.Split('_');
            switch (words[0])
            {
                case "ROW":
                    {
                        show_error_in_row(Convert.ToInt32(words[1]));
                        break;
                    }
                case "COLUMN":
                    {
                        show_error_in_column(Convert.ToInt32(words[1]));
                        break;
                    }
                case "SQUARE":
                    {
                        show_error_in_square(Convert.ToInt32(words[1]));
                        break;
                    }
            }
        }
        public void update_errors() 
        {
            /*
             * Когда может быть вызван update_errors?
             * Когда происходит проверка судоку при нажатии кнопки,
             * когда происходит нажатие клавиши Backspace и
             * когда происходит нажатие одной из клавиш цифр.
             * При этом есть три варианта:
             * 1) ошибки не было, но она появилась;
             * 2) ошибка была, но пропала, и теперь их нет;
             * 3) ошибка была, пропала, но есть ещё.
             * В момент нажатия кнопки гарантируется, что не важно,
             * есть ли на данный момент ошибка
             */
            /*
             * Код кнопки:
             * int index = 0;
               index = Convert.ToInt32(error_code.Split('_')[1]);
               error_code = error_code.Split('_')[0];
               switch (error_code)
               {
                   case "ROW": 
                       {
                           show_error_in_row(index);
                           break;
                       }
                   case "COLUMN":
                       {
                           show_error_in_column(index);
                           break;
                       }
                   case "SQUARE":
                       {
                           show_error_in_square(index);
                           break;
                       }
               }
             */
            //В общем, если нет ошибки, решаем, иначе показываем
            /*
             * 
             * Код при нажатии Backspace (есть ошибка), часть 1:
             * if (!current_error.Equals("") && !current_error.Equals(error_code))
               {
                   hide_current_error();
               }
             */
            //Ошибку нужно спрятать, если 1) она была и 2) теперь либо их нет, либо она другая
            /*
             * Код при нажатии Backspace (есть ошибка), часть 2:
             * string[] words = error_code.Split('_');
               switch (words[0])
               {
                   case "ROW":
                       {
                           show_error_in_row(Convert.ToInt32(words[1]));
                           break;
                       }
                   case "COLUMN":
                       {
                           show_error_in_column(Convert.ToInt32(words[1]));
                           break;
                       }
                   case "SQUARE":
                       {
                           show_error_in_square(Convert.ToInt32(words[1]));
                           break;                        
                       }
                }
             */
            //Backspace может только убрать ошибку, но могли остаться ещё. Необходимо показать новую.
            /*
             * Код при нажатии Backspace (нет ошибки):
             * if (!current_error.Equals(""))
               {
                   string[] words = current_error.Split('_');
                   switch (words[0])
                   {
                       case "ROW":
                           {
                               hide_error_in_row(i);
                               break;
                           }
                       case "COLUMN":
                           {
                               hide_error_in_column(j);
                               break;
                           }
                       case "SQUARE":
                           {
                               hide_error_in_square(3 * (i / 3) + j / 3);
                               break;
                           }
                   }
               }
             */
            //Теперь ошибки нет, значит, ошибку нужно спрятать
            /*
             * Код при нажатии на клавишу с цифрой:
             * if (core.is_contradictive(ref error_code))
               {
                   if (!current_error.Equals("") && !current_error.Equals(error_code)) 
                   {
                       hide_current_error();
                   }
                   string[] words = error_code.Split('_');
                   switch (words[0])
                   {
                       case "ROW":
                           {
                               if (i == Convert.ToInt32(words[1])) 
                               {
                                   show_error_in_row(i);
                               }
                               break;
                           }
                       case "COLUMN":
                           {
                               if (j == Convert.ToInt32(words[1]))
                               {
                                   show_error_in_column(j);
                               }
                               break;
                           }
                       case "SQUARE":
                           {
                               if (3 * (i / 3) + j / 3 == Convert.ToInt32(words[1]))
                               {
                                   show_error_in_square(3 * (i / 3) + j / 3);
                               }
                               break;
                           }
                   }
               }
               else 
               {
                   hide_current_error();
               }
             */
            //
            if (error_code.Equals(""))
            {
                hide_current_error();
            }
            else 
            {
                if (current_error.Equals(""))
                {
                    show_error_by_code(error_code);
                }
                else 
                {
                    if (!current_error.Equals(error_code))
                    {
                        hide_current_error();
                        show_error_by_code(error_code);
                    }
                    //иначе ничего не делать
                }
            }
            return;
        }
        public void accept_file(string filename) 
        {
            string[] table = File.ReadAllLines(filename);

            char[] C = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }; // или ' '
            if (table.Length == 9 && table.All(str => str.Length == 9))
            {
                if ((table.All(obj => obj.All(b => C.Contains(b)))))
                {
                    //MessageBox.Show("Now this is podracing!");
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (table[i][j] == '0')
                            {
                                squares[i, j].Text = "";
                                core[i, j] = 0;
                            }
                            else
                            {
                                squares[i, j].Text = table[i][j].ToString();
                                core[i, j] = Convert.ToInt32(table[i][j].ToString());
                            }
                            if (core.get_status(i, j)) 
                            {
                                squares[i, j].Background = new SolidColorBrush(Colors.White);
                            }
                            core.set_status(i, j, false);
                        }
                    }
                    hide_current_error();
                }
                current_error = "";
            }
        }
        public void save_file(string filename) 
        {
            FileStream file = File.Create(filename);
            string[] table = new string[9];
            byte[] buffer;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (squares[i, j].Text.Equals(""))
                    {
                        table[i] += "0";
                    }
                    else
                    {
                        table[i] += squares[i, j].Text;
                    }
                }
                table[i] += "\n";
            }
            for (int i = 0; i < 9; i++)
            {
                buffer = Encoding.Default.GetBytes(table[i]);
                file.Write(buffer, 0, buffer.Length);

            }
            file.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Наверное, этот код уже не нужен
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
            //error_code = "";
            //if (core.is_contradictive(ref error_code)) 
            //{
            //    //int index = 0;
            //    //index = Convert.ToInt32(error_code.Split('_')[1]);
            //    //error_code = error_code.Split('_')[0];
            //    //switch (error_code)
            //    //{
            //    //    case "ROW": 
            //    //        {
            //    //            show_error_in_row(index);
            //    //            break;
            //    //        }
            //    //    case "COLUMN":
            //    //        {
            //    //            show_error_in_column(index);
            //    //            break;
            //    //        }
            //    //    case "SQUARE":
            //    //        {
            //    //            show_error_in_square(index);
            //    //            break;
            //    //        }
            //    //}
            //    update_errors();
            //    return;
            //}
            error_code = core.is_contradictive2();
            if (!error_code.Equals(""))
            {
                update_errors();
                return;
            }
            core.solve_sudoku();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (settings.mark_set_tiles)
                    {
                        if (core.get_status(i, j))
                        {
                            squares[i, j].Background = new SolidColorBrush(Colors.LightGreen);
                        }
                    }
                    if (core[i, j] == 0)
                    {
                        squares[i, j].Text = ""; // или 0, или ?
                    }
                    else
                    {
                        squares[i, j].Text = core[i, j].ToString();
                    }
                }
            }
        }

        private void grid1_Drop(object sender, DragEventArgs e)
        {
            string filename = "";
            try
            {
                if (e.Data.GetFormats().Contains(DataFormats.FileDrop))
                {
                    filename = ((e.Data.GetData(DataFormats.FileDrop)) as string[])[0];
                }
                /*foreach (string str in e.Data.GetFormats(false)) 
                {
                    filename += str + "\n";
                }
                MessageBox.Show(filename);*/
            }
            catch (Exception exept) 
            {
                MessageBox.Show("Error: " + exept.Message);
            }
            if (File.Exists(filename))
            {
                accept_file(filename);
            }
            //MessageBox.Show(e.Data.GetDataPresent("FileDrop").ToString());
            return;
        }

        private void grid1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //e.Effects = DragDropEffects.None; нельзя здесь определять e.Effects
                if (File.Exists((e.Data.GetData(DataFormats.FileDrop) as string))) 
                {
                    //
                }
            }
            else 
            {
                //может, надо по размеру и расширению "*.sud" смотреть? 
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32((sender as TextBox).Name[8].ToString());
            int j = Convert.ToInt32((sender as TextBox).Name[9].ToString());
            (sender as TextBox).Background = new SolidColorBrush(Colors.Beige);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32((sender as TextBox).Name[8].ToString());
            int j = Convert.ToInt32((sender as TextBox).Name[9].ToString());
            if (core.get_status(i, j))
            {
                (sender as TextBox).Background = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                (sender as TextBox).Background = new SolidColorBrush(Colors.White);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) 
            {
                e.Handled = true;
                MenuItem_Click(this, e);
                return;
            }
            if (e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                e.Handled = true;
                MenuItem_Click_1(this, e);
                return;
            }
            int i = Convert.ToInt32((sender as TextBox).Name[8].ToString());
            int j = Convert.ToInt32((sender as TextBox).Name[9].ToString());
            if (arrows.Contains(e.Key))
            {
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
                if (settings.move_to_the_next_row && e.Key == Key.Right && j == 8 && i != 8) 
                {
                    Keyboard.Focus(squares[i + 1, 0]);
                }
            }
            else 
            {
                if (digit_keys.Contains(e.Key))
                {
                    squares[i, j].Text = e.Key.ToString().Remove(0, 1);
                    core[i, j] = Convert.ToInt32(squares[i, j].Text);
                    e.Handled = true;
                    if (settings.auto_move_cursor)
                    {
                        if (j != 8)
                        {
                            Keyboard.Focus(squares[i, j + 1]);
                        }
                        else
                        {
                            if (settings.move_to_the_next_row && i != 8)
                            {
                                Keyboard.Focus(squares[i + 1, 0]);
                            }
                        }
                    }
                    
                    //if (core.is_contradictive(ref error_code))
                    //{
                    //    if (!current_error.Equals("") && !current_error.Equals(error_code)) 
                    //    {
                    //        hide_current_error();
                    //    }
                    //    string[] words = error_code.Split('_');
                    //    switch (words[0])
                    //    {
                    //        case "ROW":
                    //            {
                    //                if (i == Convert.ToInt32(words[1])) 
                    //                {
                    //                    show_error_in_row(i);
                    //                }
                    //                break;
                    //            }
                    //        case "COLUMN":
                    //            {
                    //                if (j == Convert.ToInt32(words[1]))
                    //                {
                    //                    show_error_in_column(j);
                    //                }
                    //                break;
                    //            }
                    //        case "SQUARE":
                    //            {
                    //                if (3 * (i / 3) + j / 3 == Convert.ToInt32(words[1]))
                    //                {
                    //                    show_error_in_square(3 * (i / 3) + j / 3);
                    //                }
                    //                break;
                    //            }
                    //    }
                    //}
                    //else 
                    //{
                    //    hide_current_error();
                    //}
                    error_code = core.is_contradictive2();
                    update_errors();
                }
                else 
                {
                    if (e.Key == Key.Back) 
                    {
                        squares[i, j].Text = "";
                        core[i, j] = 0;
                        //if (core.is_contradictive(ref error_code))
                        //{
                            
                        //    if (!current_error.Equals("") && !current_error.Equals(error_code))
                        //    {
                        //        hide_current_error();
                        //    }
                        //    /*string[] words = error_code.Split('_');
                        //    switch (words[0])
                        //    {
                        //        case "ROW":
                        //            {
                        //                show_error_in_row(Convert.ToInt32(words[1]));
                        //                break;
                        //            }
                        //        case "COLUMN":
                        //            {
                        //                show_error_in_column(Convert.ToInt32(words[1]));
                        //                break;
                        //            }
                        //        case "SQUARE":
                        //            {
                        //                show_error_in_square(Convert.ToInt32(words[1]));
                        //                break;
                        //            }
                        //    }*/
                        //    show_error_by_code(error_code);
                        //}
                        //else
                        //{
                        //    hide_current_error();
                        //}
                        error_code = core.is_contradictive2();
                        update_errors();
                    }
                    e.Handled = true;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog choose_file_menu = new OpenFileDialog();
            choose_file_menu.Filter = "Sudoku puzzle files (*.sud)|*.sud";
            choose_file_menu.InitialDirectory = Environment.CurrentDirectory;
            if (choose_file_menu.ShowDialog() == true) 
            {
                accept_file(choose_file_menu.FileName);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog choose_file_menu = new SaveFileDialog();
            choose_file_menu.Filter = "Sudoku puzzle files (*.sud)|*.sud";
            //choose_file_menu.AddExtension = true;
            if (choose_file_menu.ShowDialog() == true) 
            {
                save_file(choose_file_menu.FileName);
            }
        }
    }
}
