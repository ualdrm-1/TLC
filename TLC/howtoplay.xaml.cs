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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TLC
{
    /// <summary>
    /// Логика взаимодействия для howtoplay.xaml
    /// </summary>
    public partial class howtoplay : UserControl
    {
        private SoundPlayer _spSelect;
        public howtoplay()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            _spSelect = new SoundPlayer(@"D:\studies\TLC\TLC\music\select.wav");
            _spSelect.Play();
        }
    }
}
