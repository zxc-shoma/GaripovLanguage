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
            List<Client> currentClient = LanguageEntities.GetContext().Client.ToList();
            //ChangePage(0, 0);

            ComboBox.SelectedIndex = 0;
            FiltrBox.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;

            //TBAllRecords.Text = LanguageEntities.GetContext().Client.ToList().Count().ToString();

            //var currentClientService = LanguageEntities.GetContext().ClientService.ToList();
            //var clientIds = currentClient.Select(c => c.ID).ToList();
            //var x = currentClientService.Where(p => clientIds.Contains(p.ClientID)).ToList();

            Update();

            ClientListview.ItemsSource = LanguageEntities.GetContext().Client.ToList();


           
            
            ClientListview.Items.Refresh();
            


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
            var currentClientService = LanguageEntities.GetContext().ClientService.ToList();
            
            TBCount.Text = currentClient.Count().ToString();

            

            if (FiltrBox.SelectedIndex ==0)
            {
                currentClient = currentClient.ToList(); 
            }

            if (FiltrBox.SelectedIndex == 1)
            {
                currentClient = currentClient.Where(p => p.GenderCode == "ж").ToList();
            }
            else if (FiltrBox.SelectedIndex == 2)
            {
                currentClient = currentClient.Where(p => p.GenderCode == "м").ToList();
            }

            currentClient = currentClient.Where(p => p.LastName.ToLower().Contains(TBSearch.Text.ToLower()) || p.FirstName.ToLower().Contains(TBSearch.Text.ToLower()) || p.Patronymic.ToLower().Contains(TBSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBSearch.Text.ToLower()) || p.Phone.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Contains(TBSearch.Text.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower())).ToList();

            

          




            if (SortBox.SelectedIndex == 0)
            {
                currentClient = currentClient.ToList();
            }

            if (SortBox.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.LastName).ToList();
            }
            else if (SortBox.SelectedIndex == 2)
            {
                
                 currentClient = currentClient.OrderByDescending(p => p.last).ToList();

                
                
            }
            else if (SortBox.SelectedIndex == 3)
            {
                currentClient = currentClient.OrderByDescending(p => p.count).ToList();
            }

            ClientListview.ItemsSource = currentClient;
            TBCount.Text = currentClient.Count.ToString();
            TableList = currentClient;

            //ChangePage(0, 0);

            if (ComboBox.SelectedIndex == 0)
            {
                CountInPage = 10;

            }
            else if (ComboBox.SelectedIndex == 1)
            {
                CountInPage = 50;

            }
            else if (ComboBox.SelectedIndex == 2)
            {
                CountInPage = 200;

            }
            else if (ComboBox.SelectedIndex == 3)
            {
                CountInPage = 0;
            }
            ChangePage(0, 0);
           

            
        }







        private void ClientListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
            ClientListview.Items.Refresh();
        }

        private void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void FiltrBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            Update();
            
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Client));
            Update();

        }

        private void ClientListview_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LanguageEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ClientListview.ItemsSource = LanguageEntities.GetContext().Client.ToList();
                Update();
            }
        }
    }
}
