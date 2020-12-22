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

        bool active, is_writing, is_folder, is_settings;
        Color current_bg_color;
        System.Windows.Media.LinearGradientBrush default_bg;
        public MainWindow()
        {
            InitializeComponent();
            active = true;
            is_writing = false;
            is_folder = false;
            is_settings = false;
            current_bg_color = Colors.White;
            default_bg = (System.Windows.Media.LinearGradientBrush) button1.Background;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (active)
            {
                active = false;
                grid1.Background = new SolidColorBrush(Colors.Black);
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button4.Visibility = Visibility.Hidden;
                if (is_writing) 
                {
                    button5.Visibility = Visibility.Hidden;
                    button6.Visibility = Visibility.Hidden;
                    textBox1.Visibility = Visibility.Hidden;
                    radioButton1.Visibility = Visibility.Hidden;
                    radioButton2.Visibility = Visibility.Hidden;
                    is_writing = false;
                }
                if (is_folder) 
                {
                    tabControl1.Visibility = Visibility.Hidden;
                    is_folder = false;
                }
                if (is_settings) 
                {
                    is_settings = false;
                    rectangle1.Visibility = Visibility.Hidden;
                    desk_background_checkBox1.Visibility = Visibility.Hidden;
                    picture_radioButton3.Visibility = Visibility.Hidden;
                    pic1_radioButton4.Visibility = Visibility.Hidden;
                    pic2_radioButton5.Visibility = Visibility.Hidden;
                    color_radioButton6.Visibility = Visibility.Hidden;
                    grey_radioButton7.Visibility = Visibility.Hidden;
                    beige_radioButton8.Visibility = Visibility.Hidden;
                    settings_label.Visibility = Visibility.Hidden;
                    style_checkBox1.Visibility = Visibility.Hidden;
                    hype_radioButton3.Visibility = Visibility.Hidden;
                    retro_radioButton4.Visibility = Visibility.Hidden;
                    stump_radioButton5.Visibility = Visibility.Hidden;
                    button7.Visibility = Visibility.Hidden;
                    button8.Visibility = Visibility.Hidden;
                }
            }
            else 
            {
                active = true;
                grid1.Background = new SolidColorBrush(current_bg_color);
                button1.Visibility = Visibility.Visible;
                button2.Visibility = Visibility.Visible;
                button4.Visibility = Visibility.Visible;
            }
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!is_writing)
            {
                textBox1.Visibility = Visibility.Visible;
                button5.Visibility = Visibility.Visible;
                button6.Visibility = Visibility.Visible;
                radioButton1.Visibility = Visibility.Visible;
                radioButton2.Visibility = Visibility.Visible;
                is_writing = true;
            }
            else 
            {
                textBox1.Visibility = Visibility.Hidden;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
                is_writing = false;
            }
            if (is_settings) 
            {
                is_settings = false;
                rectangle1.Visibility = Visibility.Hidden;
                desk_background_checkBox1.Visibility = Visibility.Hidden;
                picture_radioButton3.Visibility = Visibility.Hidden;
                pic1_radioButton4.Visibility = Visibility.Hidden;
                pic2_radioButton5.Visibility = Visibility.Hidden;
                color_radioButton6.Visibility = Visibility.Hidden;
                grey_radioButton7.Visibility = Visibility.Hidden;
                beige_radioButton8.Visibility = Visibility.Hidden;
                settings_label.Visibility = Visibility.Hidden;
                style_checkBox1.Visibility = Visibility.Hidden;
                hype_radioButton3.Visibility = Visibility.Hidden;
                retro_radioButton4.Visibility = Visibility.Hidden;
                stump_radioButton5.Visibility = Visibility.Hidden;
                button7.Visibility = Visibility.Hidden;
                button8.Visibility = Visibility.Hidden;
            }
            if (is_folder) 
            {
                tabControl1.Visibility = Visibility.Hidden;
                is_folder = false;
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                TabItem new_tab = new TabItem();
                if (radioButton2.IsChecked == true)
                {
                    new_tab.Header = "Русский язык";
                }
                else 
                {
                    new_tab.Header = "Математика";
                }
                new_tab.Content = textBox1.Text;
                tabControl1.Items.Add(new_tab);

                textBox1.Text = "";
                is_writing = false;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (!textBox1.Text.Equals("")) 
            {
                textBox1.Text = "";
                is_writing = false;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
            }
        }
private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!is_folder)
            {
                if (is_writing)
                {
                    textBox1.Visibility = Visibility.Hidden;
                    button5.Visibility = Visibility.Hidden;
                    button6.Visibility = Visibility.Hidden;
                    radioButton1.Visibility = Visibility.Hidden;
                    radioButton2.Visibility = Visibility.Hidden;
                    is_writing = false;
                }
                if (is_settings)
                {
                    is_settings = false;
                    rectangle1.Visibility = Visibility.Hidden;
                    desk_background_checkBox1.Visibility = Visibility.Hidden;
                    picture_radioButton3.Visibility = Visibility.Hidden;
                    pic1_radioButton4.Visibility = Visibility.Hidden;
                    pic2_radioButton5.Visibility = Visibility.Hidden;
                    color_radioButton6.Visibility = Visibility.Hidden;
                    grey_radioButton7.Visibility = Visibility.Hidden;
                    beige_radioButton8.Visibility = Visibility.Hidden;
                    settings_label.Visibility = Visibility.Hidden;
                    style_checkBox1.Visibility = Visibility.Hidden;
                    hype_radioButton3.Visibility = Visibility.Hidden;
                    retro_radioButton4.Visibility = Visibility.Hidden;
                    stump_radioButton5.Visibility = Visibility.Hidden;
                    button7.Visibility = Visibility.Hidden;
                    button8.Visibility = Visibility.Hidden;
                }
                is_folder = true;
                tabControl1.Visibility = Visibility.Visible;
            }
            else 
            {
                tabControl1.Visibility = Visibility.Hidden;
                is_folder = false;
            }
        }

        private void desk_background_checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            picture_radioButton3.IsEnabled = true;
            color_radioButton6.IsEnabled = true;
        }

        private void desk_background_checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            picture_radioButton3.IsEnabled = false;
            color_radioButton6.IsEnabled = false;
            pic1_radioButton4.IsEnabled = false;
            pic2_radioButton5.IsEnabled = false;
            grey_radioButton7.IsEnabled = false;
            beige_radioButton8.IsEnabled = false;

            picture_radioButton3.IsChecked = false;
            color_radioButton6.IsChecked = false;
            pic1_radioButton4.IsChecked = false;
            pic2_radioButton5.IsChecked = false;
            grey_radioButton7.IsChecked = false;
            beige_radioButton8.IsChecked = false;
        }

        private void picture_radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            pic1_radioButton4.IsEnabled = true;
            pic2_radioButton5.IsEnabled = true;
            grey_radioButton7.IsChecked = false;
            beige_radioButton8.IsChecked = false;
        }

        private void picture_radioButton3_Unchecked(object sender, RoutedEventArgs e)
        {
            pic1_radioButton4.IsEnabled = false;
            pic2_radioButton5.IsEnabled = false;
        }

        private void color_radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            grey_radioButton7.IsEnabled = true;
            beige_radioButton8.IsEnabled = true;
            pic1_radioButton4.IsChecked = false;
            pic2_radioButton5.IsChecked = false;
        }

        private void color_radioButton6_Unchecked(object sender, RoutedEventArgs e)
        {
            grey_radioButton7.IsEnabled = false;
            beige_radioButton8.IsEnabled = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!is_settings)
            {
                is_settings = true;
                rectangle1.Visibility = Visibility.Visible;
                desk_background_checkBox1.Visibility = Visibility.Visible;
                picture_radioButton3.Visibility = Visibility.Visible;
                pic1_radioButton4.Visibility = Visibility.Visible;
                pic2_radioButton5.Visibility = Visibility.Visible;
                color_radioButton6.Visibility = Visibility.Visible;
                grey_radioButton7.Visibility = Visibility.Visible;
                beige_radioButton8.Visibility = Visibility.Visible;
                settings_label.Visibility = Visibility.Visible;
                style_checkBox1.Visibility = Visibility.Visible;
                hype_radioButton3.Visibility = Visibility.Visible;
                retro_radioButton4.Visibility = Visibility.Visible;
                stump_radioButton5.Visibility = Visibility.Visible;
                button7.Visibility = Visibility.Visible;
                button8.Visibility = Visibility.Visible;
            }
            else 
            {
                is_settings = false;
                rectangle1.Visibility = Visibility.Hidden;
                desk_background_checkBox1.Visibility = Visibility.Hidden;
                picture_radioButton3.Visibility = Visibility.Hidden;
                pic1_radioButton4.Visibility = Visibility.Hidden;
                pic2_radioButton5.Visibility = Visibility.Hidden;
                color_radioButton6.Visibility = Visibility.Hidden;
                grey_radioButton7.Visibility = Visibility.Hidden;
                beige_radioButton8.Visibility = Visibility.Hidden;
                settings_label.Visibility = Visibility.Hidden;
                style_checkBox1.Visibility = Visibility.Hidden;
                hype_radioButton3.Visibility = Visibility.Hidden;
                retro_radioButton4.Visibility = Visibility.Hidden;
                stump_radioButton5.Visibility = Visibility.Hidden;
                button7.Visibility = Visibility.Hidden;
                button8.Visibility = Visibility.Hidden;
            }
            if (is_writing)
            {
                textBox1.Visibility = Visibility.Hidden;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
                is_writing = false;
            }
            if (is_folder)
            {
                tabControl1.Visibility = Visibility.Hidden;
                is_folder = false;
            }
        }
private void button7_Click(object sender, RoutedEventArgs e)
        {
            bool must_close = !(desk_background_checkBox1.IsChecked == true && grey_radioButton7.IsChecked == false && beige_radioButton8.IsChecked == false) && !(style_checkBox1.IsChecked == true && retro_radioButton4.IsChecked == false && hype_radioButton3.IsChecked == false && stump_radioButton5.IsChecked == false);
            if (must_close)
            {
                if (desk_background_checkBox1.IsChecked == false)
                {
                    current_bg_color = Colors.White;
                    SolidColorBrush new_color = new SolidColorBrush(current_bg_color);
                    grid1.Background = new_color;
                }
                if (grey_radioButton7.IsChecked == true)
                {
                    SolidColorBrush new_color = new SolidColorBrush(Colors.LightGray);
                    grid1.Background = new_color;
                    current_bg_color = Colors.LightGray;
                }
                if (beige_radioButton8.IsChecked == true)
                {
                    SolidColorBrush new_color = new SolidColorBrush(Colors.Khaki);
                    grid1.Background = new_color;
                    current_bg_color = Colors.Khaki;
                }
                /*
                if (pic1_radioButton4.IsChecked == true) 
                {
                    Image new_background = new Image();
                    new_background.Source = "/WpfApplication1;component/Images/Vk_title_photo.jpg";
                }
                if (pic2_radioButton5.IsChecked == true) 
                {
                    Image new_background = new Image();
                    new_background.Source = "/WpfApplication1;component/Images/Zhozh.jpg";
                }
                */
                if (style_checkBox1.IsChecked == false) 
                {
                    button1.Background = default_bg;
                    button2.Background = default_bg;
                    button3.Background = default_bg;
                    button4.Background = default_bg;
                    button5.Background = default_bg;
                    button6.Background = default_bg;
                    button7.Background = default_bg;
                    button8.Background = default_bg;
                    SolidColorBrush fg_color = new SolidColorBrush(Colors.Black);
                    button1.Foreground = fg_color;
                    button2.Foreground = fg_color;
                    button3.Foreground = fg_color;
                    button4.Foreground = fg_color;
                    button5.Foreground = fg_color;
                    button6.Foreground = fg_color;
                    button7.Foreground = fg_color;
                    button8.Foreground = fg_color;
                    SolidColorBrush border_color = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                    button1.BorderBrush = border_color;
                    button2.BorderBrush = border_color;
                    button3.BorderBrush = border_color;
                    button4.BorderBrush = border_color;
                    button5.BorderBrush = border_color;
                    button6.BorderBrush = border_color;
                    button7.BorderBrush = border_color;
                    button8.BorderBrush = border_color;
                }
                if (hype_radioButton3.IsChecked == true) 
                {
                    SolidColorBrush bg_color = new SolidColorBrush(Colors.Black);
                    button1.Background = bg_color;
                    button2.Background = bg_color;
                    button3.Background = bg_color;
                    button4.Background = bg_color;
                    button5.Background = bg_color;
                    button6.Background = bg_color;
                    button7.Background = bg_color;
                    button8.Background = bg_color;
                    SolidColorBrush fg_color = new SolidColorBrush(Colors.Magenta);
                    button1.Foreground = fg_color;
                    button2.Foreground = fg_color;
                    button3.Foreground = fg_color;
                    button4.Foreground = fg_color;
                    button5.Foreground = fg_color;
                    button6.Foreground = fg_color;
                    button7.Foreground = fg_color;
                    button8.Foreground = fg_color;
                    SolidColorBrush border_color = new SolidColorBrush(Colors.LimeGreen);
                    button1.BorderBrush = border_color;
                    button2.BorderBrush = border_color;
                    button3.BorderBrush = border_color;
                    button4.BorderBrush = border_color;
                    button5.BorderBrush = border_color;
                    button6.BorderBrush = border_color;
                    button7.BorderBrush = border_color;
                    button8.BorderBrush = border_color;
                }
                if (retro_radioButton4.IsChecked == true) 
                {
                    SolidColorBrush bg_color = new SolidColorBrush(Color.FromRgb(0, 128, 255));
                    button1.Background = bg_color;
                    button2.Background = bg_color;
                    button3.Background = bg_color;
                    button4.Background = bg_color;
                    button5.Background = bg_color;
                    button6.Background = bg_color;
                    button7.Background = bg_color;
                    button8.Background = bg_color;
                    SolidColorBrush fg_color = new SolidColorBrush(Colors.DarkOrange);
                    button1.Foreground = fg_color;
                    button2.Foreground = fg_color;
                    button3.Foreground = fg_color;
                    button4.Foreground = fg_color;
                    button5.Foreground = fg_color;
                    button6.Foreground = fg_color;
                    button7.Foreground = fg_color;
                    button8.Foreground = fg_color;
                    SolidColorBrush border_color = new SolidColorBrush(Colors.DarkOrange);
                    button1.BorderBrush = border_color;
                    button2.BorderBrush = border_color;
                    button3.BorderBrush = border_color;
                    button4.BorderBrush = border_color;
                    button5.BorderBrush = border_color;
                    button6.BorderBrush = border_color;
                    button7.BorderBrush = border_color;
                    button8.BorderBrush = border_color;
                }

                is_settings = false;
                rectangle1.Visibility = Visibility.Hidden;
                desk_background_checkBox1.Visibility = Visibility.Hidden;
                picture_radioButton3.Visibility = Visibility.Hidden;
                pic1_radioButton4.Visibility = Visibility.Hidden;
                pic2_radioButton5.Visibility = Visibility.Hidden;
                color_radioButton6.Visibility = Visibility.Hidden;
                grey_radioButton7.Visibility = Visibility.Hidden;
                beige_radioButton8.Visibility = Visibility.Hidden;
                settings_label.Visibility = Visibility.Hidden;
                style_checkBox1.Visibility = Visibility.Hidden;
                hype_radioButton3.Visibility = Visibility.Hidden;
                retro_radioButton4.Visibility = Visibility.Hidden;
                stump_radioButton5.Visibility = Visibility.Hidden;
                button7.Visibility = Visibility.Hidden;
                button8.Visibility = Visibility.Hidden;
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            desk_background_checkBox1.IsChecked = false;
            style_checkBox1.IsChecked = false;
            /*is_settings = false;
            rectangle1.Visibility = Visibility.Hidden;
            desk_background_checkBox1.Visibility = Visibility.Hidden;
            picture_radioButton3.Visibility = Visibility.Hidden;
            pic1_radioButton4.Visibility = Visibility.Hidden;
            pic2_radioButton5.Visibility = Visibility.Hidden;
            color_radioButton6.Visibility = Visibility.Hidden;
            grey_radioButton7.Visibility = Visibility.Hidden;
            beige_radioButton8.Visibility = Visibility.Hidden;
            settings_label.Visibility = Visibility.Hidden;
            style_checkBox1.Visibility = Visibility.Hidden;
            hype_radioButton3.Visibility = Visibility.Hidden;
            retro_radioButton4.Visibility = Visibility.Hidden;
            stump_radioButton5.Visibility = Visibility.Hidden;
            button7.Visibility = Visibility.Hidden;
            button8.Visibility = Visibility.Hidden;*/
        }

        private void style_checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            retro_radioButton4.IsEnabled = true;
            hype_radioButton3.IsEnabled = true;
            stump_radioButton5.IsEnabled = true;
        }

        private void style_checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            retro_radioButton4.IsEnabled = false;
            hype_radioButton3.IsEnabled = false;
            stump_radioButton5.IsEnabled = false;

            retro_radioButton4.IsChecked = false;
            hype_radioButton3.IsChecked = false;
            stump_radioButton5.IsChecked = false;
        }

        private void hype_radioButton3_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void retro_radioButton4_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
