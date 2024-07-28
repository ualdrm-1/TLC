using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using TLC.Properties;

namespace TLC
{
    public class PlayerScore
    {
        public string Name { get; set;}
        public int ScorePlayer { get; set; }
    }
    public partial class BeforeStart : UserControl
    {
        private SoundPlayer _spSelect = new SoundPlayer(@"D:\studies\TLC\TLC\music\select.wav");
        public int dif;
        public MediaPlayer music { get;set; }
        PlayerScore player = new PlayerScore();
        List<PlayerScore> playersScores = new List<PlayerScore>();
        public BeforeStart()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            _spSelect.Play();
            
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            Female.IsChecked = false;
        }
        string text;
        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            Male.IsChecked = false;
        }
        private void PlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(PlayerName.Document.ContentStart, PlayerName.Document.ContentEnd);
            text = textRange.Text.Trim();

            if (text.Length > 18)
            {
                int gap = 0;
                while (PlayerName.CaretPosition.DeleteTextInRun(-1) == 0)
                {
                    PlayerName.CaretPosition = PlayerName.CaretPosition.GetPositionAtOffset(--gap);
                }
            }
            player.Name = text;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _spSelect.Play();
            if ((Female.IsChecked == true || Male.IsChecked==true) && (Easy.IsChecked ==true || Normal.IsChecked == true || Hard.IsChecked==true) && (text.Length!=0)) {
                Game gamewindow = new Game(dif);
                Intro intro = new Intro();
                music.Stop();
                intro.ShowDialog();
                //gamewindow.ShowDialog();
                player.ScorePlayer = gamewindow.TotalScore; 
            }
            else
            {
                Start.Focusable = false;
            }
        }
        public void SetActiveControl(UserControl control)
        {

            control.Visibility = Visibility.Visible;
        }

        private void Easy_Checked(object sender, RoutedEventArgs e)
        {
            _spSelect.Play();
            Normal.IsChecked = false; Hard.IsChecked = false; dif = 1;
        }

        private void Normal_Checked(object sender, RoutedEventArgs e)
        {
            _spSelect.Play();
            Easy.IsChecked = false ; Hard.IsChecked = false ; dif = 2;
        }

        private void Hard_Checked(object sender, RoutedEventArgs e)
        {
            _spSelect.Play();
            Easy.IsChecked = false ; Normal.IsChecked = false ; dif = 3;
        }
    }
}
