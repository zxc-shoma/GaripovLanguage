using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client currentClient = new Client();

        
        public AddEditPage(Client SelectedClient)
        {
            InitializeComponent();
            if (SelectedClient != null )
            {
                this.currentClient = SelectedClient;
            }
            DataContext = currentClient;

            var cli = LanguageEntities.GetContext().Client.Where(p => p.ID == currentClient.ID).Select(p => p.GenderCode).FirstOrDefault();

            male.IsChecked = cli == "м";
            female.IsChecked = cli == "ж";

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(currentClient.LastName) || !currentClient.LastName.All(c => char.IsLetter(c) || c == ' ' || c == '-') || currentClient.LastName.Length > 50)
                errors.AppendLine("Фамилия может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            if (string.IsNullOrWhiteSpace(currentClient.FirstName) || !currentClient.FirstName.All(c => char.IsLetter(c) || c == ' ' || c == '-') || currentClient.FirstName.Length > 50)
                errors.AppendLine("Имя может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            if (string.IsNullOrWhiteSpace(currentClient.Patronymic) || !currentClient.Patronymic.All(c => char.IsLetter(c) || c == ' ' || c == '-') || currentClient.Patronymic.Length > 50)
                errors.AppendLine("Отчество может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");

            // Валидация Email
            if (string.IsNullOrWhiteSpace(currentClient.Email))
                errors.AppendLine("Email не может быть пустым.");
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(currentClient.Email);
                    if (addr.Address != currentClient.Email)
                        throw new Exception();
                }
                catch
                {
                    errors.AppendLine("Укажите правильный email агента.");
                }
            }

            // Валидация телефона
            if (string.IsNullOrWhiteSpace(currentClient.Phone))
                errors.AppendLine("Телефон не может быть пустым.");
            else
            {
                string phonePattern = @"^\+?\d[\d\-\(\)\s]+$";
                string cleanedPhone = currentClient.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                if (!Regex.IsMatch(currentClient.Phone, phonePattern) || !cleanedPhone.All(char.IsDigit))
                    errors.AppendLine("Телефон может содержать только цифры, плюс, минус, скобки и пробелы.");
            }

         
            

          
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentClient.GenderCode = male.IsChecked == true ? "м" : "ж";

       
            if (currentClient.ID == 0)
            {
                LanguageEntities.GetContext().Client.Add(currentClient);
            }
            try
            {
                LanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                
                FileInfo fileInfo = new FileInfo(myOpenFileDialog.FileName);

                
                if (fileInfo.Length < 2 * 1024 * 1024)
                {
                    currentClient.PhotoPath = myOpenFileDialog.FileName;
                    _photo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
                }
                else
                {
                  
                    MessageBox.Show("Размер файла превышает 2 мегабайта. Выберите другой файл.");
                }
            }

        }
    }
}
