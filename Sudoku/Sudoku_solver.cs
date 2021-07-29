using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sudoku
{
    public class Square 
    {
        public int row_t, column_t, data;
        public HashSet<int> possible;
        public bool is_set;
        public Square(int row_t, int column_t) 
        {
            this.row_t = row_t;
            this.column_t = column_t;
            data = 0;
            is_set = false;
            //possible = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //Временно, вроде бы
        }
    }
    public class Sudoku_solver
    {
        private List<List<Square>> sudoku_matrix;
        private HashSet<int> basic_set;
        public bool is_done = false;
        public bool is_logging_on = false;
        public string logging_path = "";
        public Sudoku_solver() 
        {
            sudoku_matrix = new List<List<Square>>();
            for (int i = 0; i < 9; i++) 
            {
                sudoku_matrix.Add(new List<Square>());
                for (int j = 0; j < 9; j++) 
                {
                    sudoku_matrix[i].Add(new Square(i, j));
                }
            }
            basic_set = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
        /*public int get_square(int i, int j) 
        {
            return sudoku_matrix[i][j].data;
        }*/
        public int this[int i, int j] 
        {
            get 
            {
                return sudoku_matrix[i][j].data;
            }
            set 
            {
                if (value == 0)
                {
                    sudoku_matrix[i][j].data = 0;
                    sudoku_matrix[i][j].possible = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
                else 
                {
                    sudoku_matrix[i][j].data = value;
                    sudoku_matrix[i][j].possible = new HashSet<int>();
                }
            }
        }
        public bool get_status(int i, int j) 
        {
            return sudoku_matrix[i][j].is_set;
        }
        public HashSet<Square> get_zeros_row(int n) 
        {
            HashSet<Square> res = new HashSet<Square>();
            for (int i = 0; i < 9; i++) 
            {
                if (sudoku_matrix[n][i].data == 0) 
                {
                    res.Add(sudoku_matrix[n][i]);
                }
            }
            return res;
        }
        public HashSet<Square> get_zeros_column(int n)
        {
            HashSet<Square> res = new HashSet<Square>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == 0)
                {
                    res.Add(sudoku_matrix[i][n]);
                }
            }
            return res;
        }
        public HashSet<Square> get_zeros_square(int n)
        {
            HashSet<Square> res = new HashSet<Square>();
            int a = n / 3, b = n % 3;
            for (int i = 3*a; i < 3*a+3; i++)
            {
                for (int j = 3 * b; j < 3 * b + 3; j++) 
                {
                    if (sudoku_matrix[i][j].data == 0)
                    {
                        res.Add(sudoku_matrix[i][j]);
                    }
                }
            }
            return res;
        }
        public bool is_in_row(int digit, int n) 
        {
            for (int i = 0; i < 9; i++) 
            {
                if (sudoku_matrix[n][i].data == digit)
                    return true;
            }
            return false;
        }
        public bool is_in_column(int digit, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == digit)
                    return true;
            }
            return false;
        }
        public bool is_in_square(int digit, int n)
        {
            int a = n / 3, b = n % 3;
            for (int i = 3 * a; i < 3 * a + 3; i++)
            {
                for (int j = 3 * b; j < 3 * b + 3; j++)
                {
                    if (sudoku_matrix[i][j].data == digit)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public HashSet<int> present_in_row(int n)
        {
            HashSet<int> res = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[n][i].data != 0)
                {
                    res.Add(sudoku_matrix[n][i].data);
                }
            }
            return res;
        }
        public HashSet<int> present_in_column(int n)
        {
            HashSet<int> res = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data != 0)
                {
                    res.Add(sudoku_matrix[i][n].data);
                }
            }
            return res;
        }
        public HashSet<int> present_in_square(int n)
        {
            HashSet<int> res = new HashSet<int>();
            int a = n / 3, b = n % 3;
            for (int i = 3 * a; i < 3 * a + 3; i++)
            {
                for (int j = 3 * b; j < 3 * b + 3; j++)
                {
                    if (sudoku_matrix[i][j].data != 0)
                    {
                        res.Add(sudoku_matrix[i][j].data);
                    }
                }
            }
            return res;
        }
        public HashSet<int> absent_in_row(int n) 
        {
            HashSet<int> res = new HashSet<int>(basic_set);
            res.ExceptWith(present_in_row(n));
            return res;
        }
        public HashSet<int> absent_in_column(int n)
        {
            HashSet<int> res = new HashSet<int>(basic_set);
            res.ExceptWith(present_in_column(n));
            return res;
        }
        public HashSet<int> absent_in_square(int n)
        {
            HashSet<int> res = new HashSet<int>(basic_set);
            res.ExceptWith(present_in_square(n));
            return res;
        }
        public int get_square_id(Square sqr) 
        {
            return 3 * (sqr.row_t / 3) + sqr.column_t / 3;
        }
        
        public bool is_row_finished(int n) 
        {
            return present_in_row(n).Count == 9;
        }
        public bool is_column_finished(int n)
        {
            return present_in_column(n).Count == 9;
        }
        public bool is_square_finished(int n)
        {
            return present_in_square(n).Count == 9;
        }
        public bool confirm_changes() 
        {
            bool res = false;
            for (int i = 0; i < 9; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    if (sudoku_matrix[i][j].possible.Count == 1) 
                    {
                        sudoku_matrix[i][j].is_set = true;
                        sudoku_matrix[i][j].data = sudoku_matrix[i][j].possible.First();
                        if (is_logging_on)
                        {
                            if (File.Exists(logging_path))
                            {
                                StreamWriter stream = File.AppendText(logging_path);
                                stream.Write(String.Format("Setting digit {0} on ({1}; {2})\n", sudoku_matrix[i][j].data, i, j));
                                stream.Close();
                            }
                        }
                        sudoku_matrix[i][j].possible.Remove(sudoku_matrix[i][j].data);
                    }
                }
            }
            return res;
        }
        public bool do_row(int n) 
        {
            bool changed = false;
            int temp = 0; // Для хранения кол-ва элементов во мн-ве
            HashSet<int> absent = absent_in_row(n), present = present_in_row(n);
            HashSet<Square> zeros = get_zeros_row(n);
            for (int i = 0; i < 9; i++) 
            {
                if (sudoku_matrix[n][i].data == 0) 
                {
                    temp = sudoku_matrix[n][i].possible.Count;
                    sudoku_matrix[n][i].possible.ExceptWith(present);
                    if (temp != sudoku_matrix[n][i].possible.Count) 
                    {
                        changed = true;
                    }
                } 
            }
            foreach (int digit in absent) 
            {
                foreach (Square s in zeros) 
                {
                    
                    if (s.possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_column(digit, s.column_t) || is_in_square(digit, get_square_id(s))) /*Почему цифру нужно убрать*/) 
                    {
                        s.possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            int counter = 0;
            foreach (int digit in absent) 
            {
                counter = 0;
                foreach (Square s in zeros) 
                {
                    if (!s.possible.Contains(digit))
                    {
                        counter++;
                    }
                }
                if (counter == zeros.Count - 1)
                {
                    foreach (Square s in zeros) 
                    {
                        if (s.possible.Contains(digit)) 
                        {
                            s.possible = new HashSet<int>() { digit };
                            changed = true;
                        }
                    }
                }
            }
            return changed;
        }
        public bool do_column(int n)
        {
            bool changed = false;
            int temp = 0; // Для хранения кол-ва элементов во мн-ве
            HashSet<int> absent = absent_in_column(n), present = present_in_column(n);
            HashSet<Square> zeros = get_zeros_column(n);
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == 0)
                {
                    temp = sudoku_matrix[i][n].possible.Count;
                    sudoku_matrix[i][n].possible.ExceptWith(present);
                    if (temp != sudoku_matrix[i][n].possible.Count)
                    {
                        changed = true;
                    }
                }
            }
            foreach (int digit in absent)
            {
                foreach (Square s in zeros)
                {
                    if (s.possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_column(digit, s.column_t) || is_in_square(digit, get_square_id(s))) /*Почему цифру нужно убрать*/)
                    {
                        s.possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            int counter = 0;
            foreach (int digit in absent)
            {
                counter = 0;
                foreach (Square s in zeros)
                {
                    if (!s.possible.Contains(digit))
                    {
                        counter++;
                    }
                }
                if (counter == zeros.Count - 1)
                {
                    foreach (Square s in zeros)
                    {
                        if (s.possible.Contains(digit))
                        {
                            s.possible = new HashSet<int>() { digit };
                            changed = true;
                        }
                    }
                }
            }
            return changed;
        }
        public bool do_square(int n)
        {
            bool changed = false;
            int temp = 0; // Для хранения кол-ва элементов во мн-ве
            HashSet<int> absent = absent_in_square(n), present = present_in_square(n);
            HashSet<Square> zeros = get_zeros_square(n);
            int a = n / 3, b = n % 3;
            for (int i = 3*a; i < 3*a + 3; i++) 
            {
                for (int j = 3*b; j < 3*b+3; j++) 
                {
                    if (sudoku_matrix[i][j].data == 0) 
                    {
                        temp = sudoku_matrix[i][j].possible.Count;
                        sudoku_matrix[i][j].possible.ExceptWith(present);
                        if (temp != sudoku_matrix[i][j].possible.Count) 
                        {
                            changed = true;
                        }
                    }
                }
            }
            foreach (int digit in absent) 
            {
                foreach (Square s in zeros) 
                {
                    if (s.possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_row(digit, s.row_t) || is_in_column(digit, s.column_t)) /*Почему цифру нужно убрать*/) 
                    {
                        s.possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            int counter = 0;
            foreach (int digit in absent)
            {
                counter = 0;
                foreach (Square s in zeros)
                {
                    if (!s.possible.Contains(digit))
                    {
                        counter++;
                    }
                }
                if (counter == zeros.Count - 1)
                {
                    foreach (Square s in zeros)
                    {
                        if (s.possible.Contains(digit))
                        {
                            s.possible = new HashSet<int>() { digit };
                            changed = true;
                        }
                    }
                }
            }
            return changed;
        }
        public int count_digit_row(int n, int digit) 
        {
            int counter = 0;
            for (int j = 0; j < 9; j++) 
            {
                if (sudoku_matrix[n][j].data == digit) 
                {
                    counter++;
                }
            }
            return counter;
        }
        public int count_digit_column(int n, int digit)
        {
            int counter = 0;
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == digit)
                {
                    counter++;
                }
            }
            return counter;
        }
        public int count_digit_square(int n, int digit) 
        {
            int a = n / 3, b = n % 3;
            int counter = 0;
            for (int i = 3 * a; i < 3 * a + 3; i++) 
            {
                for (int j = 3 * b; j < 3 * b + 3; j++) 
                {
                    if (sudoku_matrix[i][j].data == digit) 
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
        public bool is_row_contradictive(int n) 
        {
            return !basic_set.All(digit => count_digit_row(n, digit) <= 1);
        }
        public bool is_column_contradictive(int n)
        {
            return !basic_set.All(digit => count_digit_column(n, digit) <= 1);
        }
        public bool is_square_contradictive(int n) 
        {
            return !basic_set.All(digit => count_digit_square(n, digit) <= 1);
        }
        public bool is_contradictive(ref string error)
        {
            for (int i = 0; i < 9; i++) 
            {
                if (is_row_contradictive(i)) 
                {
                    error = "ROW_" + i.ToString();
                    return true;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (is_column_contradictive(i))
                {
                    error = "COLUMN_" + i.ToString();
                    return true;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (is_square_contradictive(i))
                {
                    error = "SQUARE_" + i.ToString();
                    return true;
                }
            }
            return false;
        }
        public void solve_sudoku() 
        {
            bool flag1 = true, flag2 = true;
            while (flag1 || flag2) 
            {
                flag1 = false;
                flag2 = false;

                for (int i = 0; i < 9; i++) 
                {
                    if (!is_row_finished(i)) 
                    {
                        flag1 = do_row(i) || flag1;
                    }
                }
                flag2 = confirm_changes() || flag2;

                for (int i = 0; i < 9; i++)
                {
                    if (!is_column_finished(i))
                    {
                        flag1 = do_column(i) || flag1;
                    }
                }
                flag2 = confirm_changes() || flag2;
                for (int i = 0; i < 9; i++)
                {
                    if (!is_square_finished(i))
                    {
                        flag1 = do_square(i) || flag1;
                    }
                }
                flag2 = confirm_changes() || flag2;
            }
            is_done = true;
        }
    }   
}
