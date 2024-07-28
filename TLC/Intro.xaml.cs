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
using System.Windows.Shapes;

namespace TLC
{
    /// <summary>
    /// Логика взаимодействия для Intro.xaml
    /// </summary>
    public partial class Intro : Window
    {
        MainWindow mainwindow = Application.Current.MainWindow as MainWindow;
        public Intro()
        {
            InitializeComponent();
            intro_game.Play();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            intro_game.Stop();
            intro_game.Volume = 0;
            Game gamewindow = new Game(mainwindow.BeforeSt.dif, mainwindow.BeforeSt.character);
            gamewindow.ShowDialog();
        }
    }

}
