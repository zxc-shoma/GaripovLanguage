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

namespace GaripovLanguage
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            var currentClient = LanguageEntities.GetContext().Client.ToList();
            //var currentClientService = LanguageEntities.GetContext().ClientService.ToList();
            //var clientIds = currentClient.Select(c => c.ID).ToList();
            //var x = currentClientService.Where(p => clientIds.Contains(p.ClientID)).ToList();






            ClientListview.ItemsSource = currentClient; 
            
        }

        private void ClientListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
