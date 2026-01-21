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
        private int _Id;
        public AddClientWindow(int Id)
        {
            InitializeComponent();
            OrgCb.ItemsSource = Db.Organisation.ToList();
            DateBirthDp.SelectedDate = DateTime.Now.Date;
            _Id = Id;

            if (_Id == -1)
            {
                ClientSp.DataContext = new Client();
            }
            else
            {
                ClientSp.DataContext = Db.Client.FirstOrDefault(el => el.Id == _Id);
            }
            
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(_Id == -1)
                {
                    var client = ClientSp.DataContext as Client;
                    client.IsDeleted = false;
                    client.Password = "password";
                    client.Login = "client";
                    client.DateBirth = DateBirthDp.SelectedDate.Value.Date;

                    Db.Client.Add(client);
                }

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
