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
using System.Xml.Serialization;

namespace GaripovLanguage
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        int CountInPage = 10;
        List<Client> CurrentPageList = new List<Client>();
        List<Client> TableList;
        public ClientPage()
        {
            InitializeComponent();
            var currentClient = LanguageEntities.GetContext().Client.ToList();
            

            //var currentClientService = LanguageEntities.GetContext().ClientService.ToList();
            //var clientIds = currentClient.Select(c => c.ID).ToList();
            //var x = currentClientService.Where(p => clientIds.Contains(p.ClientID)).ToList();






            ClientListview.ItemsSource = currentClient; 
            
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountInPage != 0)
            {
                if (CountRecords % CountInPage > 0)
                {
                    CountPage = CountRecords / CountInPage + 1;
                }
                else
                {
                    CountPage = CountRecords / CountInPage;
                }

                Boolean Ifupdate = true;

                int min;

                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * CountInPage + CountInPage < CountRecords ? CurrentPage * CountInPage + CountInPage : CountRecords;
                        for (int i = CurrentPage * CountInPage; i < min; i++)
                        {
                            CurrentPageList.Add(TableList[i]);
                        }
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * CountInPage + CountInPage < CountRecords ? CurrentPage * CountInPage + CountInPage : CountRecords;
                                for (int i = CurrentPage * CountInPage; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * CountInPage + CountInPage < CountRecords ? CurrentPage * CountInPage + CountInPage : CountRecords;
                                for (int i = CurrentPage * CountInPage; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                    }
                }
                if (Ifupdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * CountInPage + CountInPage < CountRecords ? CurrentPage * CountInPage + CountInPage : CountRecords;
                    TBCount.Text = min.ToString();
                    TBAllRecords.Text = CountRecords.ToString();

                    ClientListview.ItemsSource = CurrentPageList;

                    ClientListview.Items.Refresh();
                }
            }
            else
            {
                PageListBox.Items.Clear();
                PageListBox.Items.Add(1);
            }
        }
        public void Update()
        {
            var currentClient = LanguageEntities.GetContext().Client.ToList();
            TBAllRecords.Text = LanguageEntities.GetContext().Client.ToList().Count().ToString();
            TBCount.Text = currentClient.Count().ToString();

            ClientListview.ItemsSource = currentClient;

            TableList = currentClient;
            if (strCount.SelectedIndex == 0)
            {
                CountInPage = 10;
            }
            else if (strCount.SelectedIndex == 1)
            {
                CountInPage = 50;
            }
            else if (strCount.SelectedIndex == 2)
            {
                CountInPage = 200;
            }
            else if (strCount.SelectedIndex == 3)
            {
                CountInPage = 0;
            }


            ChangePage(0, 0);
        }







        private void ClientListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void strCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);

        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;

            if (currentClient.count == 0)
            {
                if (MessageBox.Show("Вы точно хотите удалить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    LanguageEntities.GetContext().Client.Remove(currentClient);
                    LanguageEntities.GetContext().SaveChanges();
                    ClientListview.ItemsSource = LanguageEntities.GetContext().Client.ToList();
                    Update();
                }
            }
            else
            {
                MessageBox.Show("Невозможно выполнить удаление, так как клиент посещал школу!");
            }

        }
    }
}
