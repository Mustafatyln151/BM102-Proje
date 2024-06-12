using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp_bmprojeui1.Core;

namespace WpfApp_bmprojeui1
{
    /// <summary>
    /// Interaction logic for GirisEkrani.xaml
    /// </summary>

    public partial class GirisEkrani : Page
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Profilepicture { get; set; }

        public GirisEkrani()
        {

            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenMainWindow();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenMainWindow();
        }
        private void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Mevcut pencereyi kapatmak istiyorsanız, bunu yeni pencere açıldıktan sonra yapın
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}

