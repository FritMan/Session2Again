using Session2Try2.Data;
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
using System.Windows.Shapes;
using static Session2Try2.Classes.Helper;

namespace Session2Try2.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
            OrgCb.ItemsSource = Db.Organisation.ToList();
            DateBirthDp.SelectedDate = DateTime.Now.Date;
            ClientSp.DataContext = new Client();
            
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = ClientSp.DataContext as Client;
                client.IsDeleted = false;
                client.Password = "password";
                client.Login = "client";
                client.DateBirth = DateBirthDp.SelectedDate.Value.Date;

                Db.Client.Add(client);
                Db.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
