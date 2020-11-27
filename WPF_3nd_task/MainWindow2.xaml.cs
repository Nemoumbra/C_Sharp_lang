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
        string not_stated = "не указано";
        CheckBox[] preferences;

        public MainWindow()
        {
            InitializeComponent();
            preferences = new CheckBox[] { Sports_Checkbox, Cinema_Checkbox, Music_Checkbox, Cooking_Checkbox, Science_Checkbox };
            gender_entered = false;
            age_entered = false;
            name_entered = false;
            prefs_entered = false;
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
                prefs_content = result;
            }
            
        }

        public void Print_user_data() 
        {
            string text = "Имя: ";
            if (!name_entered)
                text = string.Concat(text, not_stated, "\n");
            else
                text = string.Concat(text, name_content, "\n");
            text = string.Concat(text, "Пол: ");
            if (!gender_entered)
                text = string.Concat(text, not_stated, "\n");
            else
                text = string.Concat(text, gender_content, "\n");
            text = string.Concat(text, "Возраст: ");
            if (!age_entered)
                text = string.Concat(text, not_stated, "\n");
            else
                text = string.Concat(text, age_content, "\n");
            text = string.Concat(text, "Интересы: ");
            if (!prefs_entered)
                text = string.Concat(text, not_stated);
            else
                text = string.Concat(text, prefs_content);
            UserBioLabel.Content = text;
        }

        private void Fem_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!gender_entered)
            {
                gender_entered = true;
            }
            gender_content = (String)Fem_Radiobutton.Content;
            Print_user_data();
        }

        private void Masc_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!gender_entered)
            {
                gender_entered = true;
            }
            gender_content = (String)Masc_Radiobutton.Content;
            Print_user_data();
        }

        private void Before20_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before20_Radiobutton.Content;
            Print_user_data();
        }

        private void Before35_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before35_Radiobutton.Content;
            Print_user_data();
        }

        private void Before50_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)Before50_Radiobutton.Content;
            Print_user_data();
        }

        private void After50_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            if (!age_entered)
            {
                age_entered = true;
            }
            age_content = (String)After50_Radiobutton.Content;
            Print_user_data();
        }

        private void Sports_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Cinema_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Music_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Cooking_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Science_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
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
            Print_user_data();
        }

        private void Sports_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Cinema_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Music_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Cooking_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Science_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Science_Checkbox_Checked_1(object sender, RoutedEventArgs e)
        {
            set_prefs();
            Print_user_data();
        }

        private void Show_image_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            The_image.Visibility = Visibility.Hidden;
        }

        private void Show_image_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            The_image.Visibility = Visibility.Visible;
        }

    }
}
