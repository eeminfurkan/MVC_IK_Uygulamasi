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

using System;

namespace MVC_IK_Uygulamasi.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWebViewAsync();
        }

        async void InitializeWebViewAsync()
        {
            try
            {
                // WebView2 çekirdeğinin başlatılmasını garantiliyoruz.
                await webView.EnsureCoreWebView2Async(null);

                // Web projemizin `http` adresini buraya yazıyoruz.
                // Bu adresi, web projenin Properties/launchSettings.json dosyasından bulabilirsin.
                webView.Source = new Uri("http://localhost:5245"); // DİKKAT: Port numarası sende farklı olabilir!
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2 başlatılamadı. Hata: {ex.Message}", "Kritik Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}