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

namespace WPF_tasks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool gender_entered, age_entered, name_entered, prefs_entered;
        string gender_content, age_content, name_content, prefs_content;
        string basic_text = "Имя:  \nПол:  \nВозраст:  \nИнтересы:  ";
        string not_stated = "не указано";
        CheckBox[] preferences;

        public MainWindow()
        {
            InitializeComponent();
            preferences = new CheckBox[] {Sports_Checkbox, Cinema_Checkbox, Music_Checkbox, Cooking_Checkbox, Science_Checkbox};
        }
        public void set_prefs() 
        {
            string result = "";
            bool is_checked = false;
            for (int i = 0; i < 5; i++) 
            {
                if (preferences[i].IsChecked == true) 
                {
                    if (!is_checked) 
                    {
                        is_checked = true;
                    }
                    result = string.Concat(result, preferences[i].Content, "; ");
                }
            }
            if (!is_checked)
            {
                prefs_entered = false;
            }
            else 
            {
                prefs_entered = true;
            }
            prefs_content = result;
        }

        public void Print_user_data() //string.Remove(from, how many), string.IndexOf(Char, Int32), string.Insert(index, text)
        {
            string text = basic_text;
            int temp;
            temp = text.IndexOf('\n', 0);
            if (!name_entered)
            {
                
                text = text.Remove(temp - 1, 1).Insert(, not_stated);
            }
            else 
            {
                text = 
            }
            
        }

        private void Fem_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!gender_entered) 
            {
                gender_entered = true;
            }
            gender_content = (String) Fem_Radiobutton.Content;
        }

        private void Masc_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!gender_entered)
            {
                gender_entered = true;
            }
            gender_content = (String)Masc_Radiobutton.Content;
        }

        private void Before20_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before20_Radiobutton.Content;
        }

        private void Before35_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before35_Radiobutton.Content;
        }

        private void Before50_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before50_Radiobutton.Content;
        }

        private void After50_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)After50_Radiobutton.Content;
        }

        private void Sports_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
        }

        private void Cinema_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
        }

        private void Music_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
        }

        private void Cooking_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
        }

        private void Science_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
        }

        private void name_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (name_textbox.Text.Equals(""))
            {
                name_entered = false;
            }
            else 
            {
                if (!name_entered) 
                {
                    name_entered = true;
                }
                name_content = name_textbox.Text;
            }
        }

    }
}
