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

namespace Client_prime
{
    /// <summary>
    /// Логика взаимодействия для ChatMessage.xaml
    /// </summary>
    public partial class ChatMessage : UserControl
    {
        public ChatMessage()
        {
            InitializeComponent();
        }
        public string Username
        {
            get { return textBlock1.Text; }
            set { textBlock1.Text = value; }
        }
        public string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public void Load_Image() 
        {
            return;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBlock1.Text);
        }

    }
}

