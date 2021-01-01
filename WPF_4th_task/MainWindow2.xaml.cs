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
        bool activated, settings, folders, writing;
        Button retro_example, hype_example, default_example;
        Color current_bg_color;
        //SolidColorBrush hype_brush, retro_brush;
        //System.Windows.Media.LinearGradientBrush default_button_style;
        public void hide(UIElement thing) 
        {
            thing.Visibility = Visibility.Hidden;
        }
        public void show(UIElement thing) 
        {
            thing.Visibility = Visibility.Visible;
        }
        public void close_settings() 
        {
            hide(settings_Label);
            hide(settings_Rectangle);
            hide(background_Checkbox);
            hide(button_style_Checkbox);
            hide(bg_color_Radiobutton);
            hide(bg_gray_Radiobutton);
            hide(bg_khaki_Radiobutton);
            hide(bg_photo_Radiobutton);
            hide(bg_pic1_Radiobutton);
            hide(bg_pic2_Radiobutton);
            hide(hype_Radiobutton);
            hide(retro_Radiobutton);
            hide(stump_Radiobutton);
            hide(save_settings_Button);
            hide(reset_settings_Button);
            settings = false;
        }
        public void open_settings() 
        {
            show(settings_Label);
            show(settings_Rectangle);
            show(background_Checkbox);
            show(button_style_Checkbox);
            show(bg_color_Radiobutton);
            show(bg_gray_Radiobutton);
            show(bg_khaki_Radiobutton);
            show(bg_photo_Radiobutton);
            show(bg_pic1_Radiobutton);
            show(bg_pic2_Radiobutton);
            show(hype_Radiobutton);
            show(retro_Radiobutton);
            show(stump_Radiobutton);
            show(save_settings_Button);
            show(reset_settings_Button);
            settings = true;
        }
        public void close_writing()
        {
            hide(input_field_Textbox);
            hide(save_work_Button);
            hide(send_work_Button);
            hide(maths_Radiobutton);
            hide(russian_Radiobutton);
            writing = false;
        }
        public void open_writing()
        {
            show(input_field_Textbox);
            show(save_work_Button);
            show(send_work_Button);
            show(maths_Radiobutton);
            show(russian_Radiobutton);
            writing = true;
        }
        public void close_folders()
        {
            hide(saved_works_Tabcontrol);
            folders = false;
        }
        public void open_folders()
        {
            show(saved_works_Tabcontrol);
            folders = true;
        }
        public void deactivate()
        {
            if (settings) 
            {
                close_settings();
            }
            if (writing) 
            {
                close_writing();
            }
            if (folders) 
            {
                close_folders();
            }
            hide(settings_Button);
            hide(folders_Button);
            hide(writing_Button);
            grid1.Background = new SolidColorBrush(Colors.Black);
            activated = false;
        }
        public void activate() 
        {
            show(settings_Button);
            show(folders_Button);
            show(writing_Button);
            grid1.Background = new SolidColorBrush(current_bg_color);
            activated = true;
        }
        public void add_new_folder(string work_name) 
        {
            if (!input_field_Textbox.Text.Equals("")) 
            {
                TabItem new_folder = new TabItem();
                new_folder.Header = work_name;
                new_folder.Content = input_field_Textbox.Text;
                saved_works_Tabcontrol.Items.Add(new_folder);
            }
        }
        public void set_button_style(Button template) 
        {
            settings_Button.Background = template.Background;
            folders_Button.Background = template.Background;
            writing_Button.Background = template.Background;
            on_off_Button.Background = template.Background;
            save_work_Button.Background = template.Background;
            send_work_Button.Background = template.Background;
            save_settings_Button.Background = template.Background;
            reset_settings_Button.Background = template.Background;

            settings_Button.BorderBrush = template.BorderBrush;
            folders_Button.BorderBrush = template.BorderBrush;
            writing_Button.BorderBrush = template.BorderBrush;
            on_off_Button.BorderBrush = template.BorderBrush;
            save_work_Button.BorderBrush = template.BorderBrush;
            send_work_Button.BorderBrush = template.BorderBrush;
            save_settings_Button.BorderBrush = template.BorderBrush;
            reset_settings_Button.BorderBrush = template.BorderBrush;

            settings_Button.Foreground = template.Foreground;
            folders_Button.Foreground = template.Foreground;
            writing_Button.Foreground = template.Foreground;
            on_off_Button.Foreground = template.Foreground;
            save_work_Button.Foreground = template.Foreground;
            send_work_Button.Foreground = template.Foreground;
            save_settings_Button.Foreground = template.Foreground;
            reset_settings_Button.Foreground = template.Foreground;
        }
        public void set_bg_color(Color cvet) 
        {
            grid1.Background = new SolidColorBrush(cvet);
            current_bg_color = cvet;
        }
        public MainWindow()
        {
            InitializeComponent();
            activated = true;
            settings = false;
            folders = false;
            writing = false;
            retro_example = new Button();
            retro_example.Background = new SolidColorBrush(Color.FromRgb(0, 128, 255));
            retro_example.BorderBrush = new SolidColorBrush(Colors.DarkOrange);
            retro_example.Foreground = new SolidColorBrush(Colors.DarkOrange);
            hype_example = new Button();
            hype_example.Background = new SolidColorBrush(Colors.Black);
            hype_example.BorderBrush = new SolidColorBrush(Colors.LimeGreen);
            hype_example.Foreground = new SolidColorBrush(Colors.Magenta);
            default_example = new Button();
            default_example.Background = settings_Button.Background;
            default_example.BorderBrush = new SolidColorBrush(Color.FromRgb(70, 70, 70));
            default_example.Foreground = new SolidColorBrush(Colors.Black);
            current_bg_color = Colors.White;
        }

        private void settings_Button_Click(object sender, RoutedEventArgs e)
        {
            if (settings)
            {
                close_settings();
            }
            else
            {
                if (writing)
                {
                    close_writing();
                }
                if (folders)
                {
                    close_folders();
                }
                open_settings();
            }
        }

        private void writing_Button_Click(object sender, RoutedEventArgs e)
        {
            if (writing)
            {
                close_writing();
            }
            else 
            {
                if (settings) 
                {
                    close_settings();                   
                }
                if (folders) 
                {
                    close_folders();                    
                }
                open_writing();               
            }
        }

        private void folders_Button_Click(object sender, RoutedEventArgs e)
        {
            if (folders)
            {
                close_folders();                
            }
            else
            {
                if (settings)
                {
                    close_settings();                    
                }
                if (writing)
                {
                    close_writing();                    
                }
                open_folders();                
            }
        }

        private void on_off_Button_Click(object sender, RoutedEventArgs e)
        {
            if (activated)
            {
                deactivate();                
            }
            else 
            {
                activate();                
            }
        }

        private void save_work_Button_Click(object sender, RoutedEventArgs e)
        {
            if (maths_Radiobutton.IsChecked == true)
            {
                add_new_folder("Математика");
            }
            else 
            {
                add_new_folder("Русский язык");
            }
            input_field_Textbox.Text = "";
            close_writing();            
        }

        private void send_work_Button_Click(object sender, RoutedEventArgs e)
        {
            input_field_Textbox.Text = "";
            close_writing();            
        }

        private void background_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            bg_color_Radiobutton.IsEnabled = true;
            bg_photo_Radiobutton.IsEnabled = true;
        }

        private void background_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            bg_color_Radiobutton.IsEnabled = false;
            bg_photo_Radiobutton.IsEnabled = false;

            bg_color_Radiobutton.IsChecked = false;
            bg_photo_Radiobutton.IsChecked = false;
        }

        private void bg_photo_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            bg_pic1_Radiobutton.IsEnabled = true;
            bg_pic2_Radiobutton.IsEnabled = true;

        }

        private void bg_photo_Radiobutton_Unchecked(object sender, RoutedEventArgs e)
        {
            bg_pic1_Radiobutton.IsEnabled = false;
            bg_pic2_Radiobutton.IsEnabled = false;
            bg_pic1_Radiobutton.IsChecked = false;
            bg_pic2_Radiobutton.IsChecked = false;
        }

        private void bg_color_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            bg_gray_Radiobutton.IsEnabled = true;
            bg_khaki_Radiobutton.IsEnabled = true;
        }

        private void bg_color_Radiobutton_Unchecked(object sender, RoutedEventArgs e)
        {
            bg_gray_Radiobutton.IsEnabled = false;
            bg_khaki_Radiobutton.IsEnabled = false;
            bg_gray_Radiobutton.IsChecked = false;
            bg_khaki_Radiobutton.IsChecked = false;
        }

        private void button_style_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            hype_Radiobutton.IsEnabled = true;
            retro_Radiobutton.IsEnabled = true;
            stump_Radiobutton.IsEnabled = true;
        }

        private void button_style_Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            hype_Radiobutton.IsEnabled = false;
            retro_Radiobutton.IsEnabled = false;
            stump_Radiobutton.IsEnabled = false;
            hype_Radiobutton.IsChecked = false;
            retro_Radiobutton.IsChecked = false;
            stump_Radiobutton.IsChecked = false;
        }

        private void reset_settings_Button_Click(object sender, RoutedEventArgs e)
        {
            background_Checkbox.IsChecked = false;
            button_style_Checkbox.IsChecked = false;
        }
        private void save_settings_Button_Click(object sender, RoutedEventArgs e)
        {
            bool has_to_close = !((background_Checkbox.IsChecked == true && bg_gray_Radiobutton.IsChecked == false && bg_khaki_Radiobutton.IsChecked == false && bg_pic1_Radiobutton.IsChecked == false && bg_pic2_Radiobutton.IsChecked == false) || (button_style_Checkbox.IsChecked == true && hype_Radiobutton.IsChecked == false && retro_Radiobutton.IsChecked == false && stump_Radiobutton.IsChecked == false));
            if (has_to_close)
            {
                if (background_Checkbox.IsChecked == false)
                {
                    set_bg_color(Colors.White);
                }
                if (bg_gray_Radiobutton.IsChecked == true)
                {
                    set_bg_color(Colors.LightGray);
                }
                if (bg_khaki_Radiobutton.IsChecked == true)
                {
                    set_bg_color(Colors.Khaki);
                }
                /*
                if (bg_pic1_Radiobutton.IsChecked == true) 
                {

                }
                if (bg_pic2_Radiobutton.IsChecked == true) 
                {

                }*/
                if (button_style_Checkbox.IsChecked == false)
                {
                    set_button_style(default_example);
                }
                if (hype_Radiobutton.IsChecked == true)
                {
                    set_button_style(hype_example);
                }
                if (retro_Radiobutton.IsChecked == true)
                {
                    set_button_style(retro_example);
                }

                close_settings();              
            }
            else 
            {
                MessageBox.Show("Некорректно введены данные!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
    }
}
