using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime;
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
using TLC.Properties;

namespace TLC
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        MainWindow mainwindow = Application.Current.MainWindow as MainWindow;
        private SoundPlayer _spSelect = new SoundPlayer(@"D:\studies\TLC\TLC\music\select.wav");
        public int SoundVol { get; set; }
        public MediaPlayer mp { get; set; }
        public bool isMusicPlay { get; set; }

        public Settings()
        {
            InitializeComponent();
            SoundVol = 5;
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;

            _spSelect.Play();
        }

        private void SliderMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mp.Volume = (double)SoundVol/10;
        }

        private void isMusic_Checked(object sender, RoutedEventArgs e)
        {
            
            mainwindow._mpBgr.Play();
            _spSelect.Play();
        }

        private void isSound_Checked(object sender, RoutedEventArgs e)
        {

            _spSelect.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            _spSelect.Play();
            SetActiveControl(howtoplay);
        }
        public void SetActiveControl(UserControl control)
        {
            howtoplay.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
        }
        private void isUncheckedMus(object sender, RoutedEventArgs e)
        {
            mainwindow._mpBgr.Pause();
            _spSelect.Play();
        }

        private void isSoundUnchecked(object sender, RoutedEventArgs e)
        {
        }
    }
}
