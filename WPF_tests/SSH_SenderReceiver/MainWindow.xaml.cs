using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace SSH_SenderReciever
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selected_file = "";
        private void hide(UIElement elem)
        {
            elem.Visibility = Visibility.Hidden;
        }
        private void show(UIElement elem)
        {
            elem.Visibility = Visibility.Visible;
        }
        private bool file_selected = false;
        private void change_state()
        {
            if (!file_selected)
            {
                hide(OpenButton);
                hide(Rectangle);
                show(CancelButton);
                show(UploadButton);
            }
            else
            {
                show(OpenButton);
                show(Rectangle);
                hide(CancelButton);
                hide(UploadButton);
            }
            file_selected = !file_selected;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void send_file(string path)
        { 
            Process putty = new Process();
            putty.StartInfo.FileName = "cmd.exe";
            putty.StartInfo.CreateNoWindow = true;
            putty.StartInfo.ErrorDialog = true;
            string args = "pscp -P port -pw password " + path.Replace(@"\\", @"\") + " main@remote.vdi.mipt.ru:" + InputTextbox.Text;
            putty.StartInfo.Arguments = @"/C cd C:\Users\Nikolaos\D\Downloads\Putty & " + args.ToString() + " & pause";
            try
            {
                if (putty.Start())
                {
                    //StreamReader stream = putty.StandardOutput;
                    //string response = "";
                    //char[] data = new char[256];
                    //do
                    //{
                    //    int bytes = stream.Read(data, 0, data.Length);
                    //    response += data;
                    //}
                    //while (!stream.EndOfStream);
                    //MessageBox.Show(response);

                    //while (!putty.HasExited)
                    //{

                    //}
                    //MessageBox.Show("Putty is deactivated!");
                    //Console.WriteLine(putty.StandardOutput.ReadToEnd());
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message);
            }
        }

        private void receive_file(string path)
        {
            Process putty = new Process();
            putty.StartInfo.FileName = "cmd.exe";
            putty.StartInfo.CreateNoWindow = true;
            putty.StartInfo.ErrorDialog = true;
            string args = "pscp -P port -pw password main@remote.vdi.mipt.ru:" + InputTextbox.Text + " " + path;
            putty.StartInfo.Arguments = @"/C cd C:\Users\Nikolaos\D\Downloads\Putty & " + args.ToString() + " & pause";
            try
            {
                if (putty.Start())
                {
                    
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message);
            }
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            string filename = "";
            try
            {
                if (e.Data.GetFormats().Contains(DataFormats.FileDrop))
                {
                    filename = ((e.Data.GetData(DataFormats.FileDrop)) as string[])[0];
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message);
            }
            if (File.Exists(filename))
            {
                PathTextBlock.Text = filename;
                selected_file = filename;
                change_state();
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog choose_file = new OpenFileDialog();
            choose_file.Filter = "Any files (*.*)|*.*";
            choose_file.InitialDirectory = @"D:\Downloads\Putty\";
            if (choose_file.ShowDialog() == true)
            {
                PathTextBlock.Text = choose_file.FileName;
                selected_file = choose_file.FileName;
                change_state();
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog select_filename_dialog = new SaveFileDialog();
            select_filename_dialog.InitialDirectory = @"D:\Downloads\Putty\";
            select_filename_dialog.FileName = InputTextbox.Text.Remove(0, InputTextbox.Text.LastIndexOf('/') + 1);
            if (select_filename_dialog.ShowDialog() == true)
            {
                receive_file(select_filename_dialog.FileName);
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            send_file(selected_file);
            change_state();
            selected_file = "";
            PathTextBlock.Text = @"D:\Downloads";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            selected_file = "";
            change_state();
            PathTextBlock.Text = @"D:\Downloads";
        }
    }
}
