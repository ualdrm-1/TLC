using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TLC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MediaPlayer _mpBgr;
        public SoundPlayer _spSel;

        public MainWindow()
        {
            InitializeComponent();
            EXITBUTTON.Click += EXIT_BUTTON_Click;
            SETTINGS_BUTTON.Click += SETTINGS_BUTTON_Click;
            _mpBgr = new MediaPlayer();
            _mpBgr.Open(new Uri(@"D:\studies\TLC\TLC\music\fone_music.mp3"));
            _spSel = new SoundPlayer(@"D:\studies\TLC\TLC\music\select.wav");
            _mpBgr.Play();
            settings.mp = _mpBgr;
            BeforeSt.music = _mpBgr;
        }
        private void PLAY_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            _spSel.Play();
            SetActiveControl(BeforeSt);

        }
        private void SETTINGS_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            _spSel.Play();
            SetActiveControl(settings);

        }

        private void EXIT_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            _spSel.Load();
            _spSel.Play();
            this.Close();
        }

        public void SetActiveControl(UserControl control)
        {
            settings.Visibility=Visibility.Collapsed;

            control.Visibility=Visibility.Visible;
        }
    }
}