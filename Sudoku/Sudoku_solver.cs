using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    class Square 
    {
        public int x, y, data;
        public SortedSet<int> possible;
        public Square(int x, int y) 
        {
            this.x = x;
            this.y = y;
            data = 0;
            possible = new SortedSet<int>();
        }
    }
    class Sudoku_solver
    {
        private List<List<Square>> sudoku_matrix;
        private SortedSet<int> basic_set;
        public Sudoku_solver() 
        {
            sudoku_matrix = new List<List<Square>>(9);
            for (int i = 0; i < 9; i++) 
            {
                sudoku_matrix[i] = new List<Square>(9);
                for (int j = 0; j < 9; j++) 
                {
                    sudoku_matrix[i][j] = new Square(i, j);
                }
            }
            basic_set = new SortedSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
        public SortedSet<Square> get_zeros_row(int n) 
        {
            SortedSet<Square> res = new SortedSet<Square>();
            for (int i = 0; i < 9; i++) 
            {
                if (sudoku_matrix[n][i].data == 0) 
                {
                    res.Add(sudoku_matrix[n][i]);
                }
            }
            return res;
        }
        public SortedSet<Square> get_zeros_column(int n)
        {
            SortedSet<Square> res = new SortedSet<Square>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == 0)
                {
                    res.Add(sudoku_matrix[i][n]);
                }
            }
            return res;
        }
        public SortedSet<Square> get_zeros_square(int n)
        {
            SortedSet<Square> res = new SortedSet<Square>();
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

        public SortedSet<int> present_in_row(int n)
        {
            SortedSet<int> res = new SortedSet<int>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[n][i].data != 0)
                {
                    res.Add(sudoku_matrix[n][i].data);
                }
            }
            return res;
        }
        public SortedSet<int> present_in_column(int n)
        {
            SortedSet<int> res = new SortedSet<int>();
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data != 0)
                {
                    res.Add(sudoku_matrix[i][n].data);
                }
            }
            return res;
        }
        public SortedSet<int> present_in_square(int n)
        {
            SortedSet<int> res = new SortedSet<int>();
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
        public SortedSet<int> absent_in_row(int n) 
        {
            SortedSet<int> res = basic_set;
            res.ExceptWith(present_in_row(n));
            return res;
        }
        public SortedSet<int> absent_in_column(int n)
        {
            SortedSet<int> res = basic_set;
            res.ExceptWith(present_in_column(n));
            return res;
        }
        public SortedSet<int> absent_in_square(int n)
        {
            SortedSet<int> res = basic_set;
            res.ExceptWith(present_in_square(n));
            return res;
        }
        public int get_square_id(Square sqr) 
        {
            return 3 * (sqr.y / 3) + sqr.x / 3;
        }
        
        public bool is_row_finished(int n) 
        {
            return present_in_row(n) == basic_set;
        }
        public bool is_column_finished(int n)
        {
            return present_in_column(n) == basic_set;
        }
        public bool is_square_finished(int n)
        {
            return present_in_square(n) == basic_set;
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
                        sudoku_matrix[i][j].data = sudoku_matrix[i][j].possible.First();
                        sudoku_matrix[i][j].possible.Remove(sudoku_matrix[i][j].data);
                    }
                }
            }
            return res;
        }
        public bool do_row(int n) 
        {
            /*
            def do_row(L, n):
            changed = False
            absent = absent_row(field, n)
            zero_squares = get_zeroes_row(field, n)
            exist = get_row(L, n)
            for i in range(9):  # В ряду нет повторений
                L[n][i].possible.difference_update(exist)
            for i in absent:  # Перебираем отсутствующие цифры в n-ом ряду
                for j in zero_squares:  # Ищем в колоннах с нулями в n-ом ряду
                    if is_in_column(field, i, j.x) or is_in_square(field, i, get_square_id(j)):
                        if i in field[n][j.x].possible:  # Отмечаем, что в клетке [n][j.x] не может быть цифры i
                            field[n][j.x].possible.discard(i)
                            changed = True
            return changed*/
            bool changed = false;
            SortedSet<int> absent = absent_in_row(n), present = present_in_row(n);
            SortedSet<Square> zeros = get_zeros_row(n);
            for (int i = 0; i < 9; i++) 
            {
                if (sudoku_matrix[n][i].data == 0) 
                {
                    sudoku_matrix[n][i].possible.ExceptWith(present);
                    changed = true;
                } 
            }
            foreach (int digit in absent) 
            {
                foreach (Square s in zeros) 
                {
                    if (sudoku_matrix[n][s.x].possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_column(digit, s.x) || is_in_square(digit, get_square_id(s))) /*Почему цифру нужно убрать*/) 
                    {
                        sudoku_matrix[n][s.x].possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            return changed;
        }
        public bool do_column(int n)
        {
            /*def do_column(L, n):
                changed = False
                absent = absent_column(field, n)
                zero_squares = get_zeroes_column(field, n)
                exist = get_column(L, n)
                for i in range(9):  # В столбце нет повторений
                    L[i][n].possible.difference_update(exist)
                for i in absent:  # Перебираем отсутствующие цифры в n-ой колонне
                    for j in zero_squares:  # Ищем в строках с нулями в n-ой колонне
                        if is_in_row(field, i, j.y) or is_in_square(field, i, get_square_id(j)):
                            if i in field[j.y][n].possible:  # Отмечаем, что в клетке [j.y][n] не может быть цифры i
                                field[j.y][n].possible.discard(i)
                                changed = True
                return changed
            */
            bool changed = false;
            SortedSet<int> absent = absent_in_column(n), present = present_in_column(n);
            SortedSet<Square> zeros = get_zeros_column(n);
            for (int i = 0; i < 9; i++)
            {
                if (sudoku_matrix[i][n].data == 0)
                {
                    sudoku_matrix[i][n].possible.ExceptWith(present);
                    changed = true;
                }
            }
            foreach (int digit in absent)
            {
                foreach (Square s in zeros)
                {
                    if (sudoku_matrix[s.y][n].possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_column(digit, s.x) || is_in_square(digit, get_square_id(s))) /*Почему цифру нужно убрать*/)
                    {
                        sudoku_matrix[s.y][n].possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            return changed;
        }
        public bool do_square(int n)
        {
            /*def do_square(L, n):
                changed = False
                absent = absent_square(L, n)
                zero_squares = get_zeroes_square(L, n)
                exist = get_square(L, n)
                a = n // 3
                b = n % 3  # В квадрате нет повторений
                for i in range(3 * a, 3 * a + 3):
                    for j in range(3 * b, 3 * b + 3):
                        L[i][j].possible.difference_update(exist)
                for i in absent:  # Перебираем отсутствующие цифры в n-ом квадрате
                    for j in zero_squares:  # Ищем в строках и столбцах
                        if is_in_row(field, i, j.y) or is_in_column(field, i, j.x):
                            if i in field[j.y][j.x].possible:
                                field[j.y][j.x].possible.discard(i)
                                changed = True
                return changed
            */
            bool changed = false;
            SortedSet<int> absent = absent_in_square(n), present = present_in_square(n);
            SortedSet<Square> zeros = get_zeros_square(n);
            int a = n / 3, b = n % 3;
            for (int i = 3*a; i < 3*a + 3; i++) 
            {
                for (int j = 3*b; j < 3*b+3; j++) 
                {
                    if (sudoku_matrix[i][j].data == 0) 
                    {
                        sudoku_matrix[i][j].possible.ExceptWith(present);
                        changed = true;
                    }
                }
            }
            foreach (int digit in absent) 
            {
                foreach (Square s in zeros) 
                {
                    if (sudoku_matrix[s.y][s.x].possible.Contains(digit) /*Почему цифру можно убрать*/ && (is_in_row(digit, s.y) || is_in_column(digit, s.x)) /*Почему цифру нужно убрать*/) 
                    {
                        sudoku_matrix[s.y][s.x].possible.Remove(digit);
                        changed = true;
                    }
                }
            }
            return changed;
        }
    }
}
