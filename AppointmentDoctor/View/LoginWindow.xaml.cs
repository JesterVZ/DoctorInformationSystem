using AppointmentDoctor.Model;
using AppointmentWirhDoctor.model;
using AppointmentWithDoctor.model;
using AppointmentWithDoctor.SQL;
using System;
using System.Collections.Generic;
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

namespace AppointmentDoctor.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        SQLiteFunctions sQLite;
        public Doctor Doctor;
        public Patient Patient;
        public Admin Admin;
        public string Role;
        public LoginWindow()
        {
            InitializeComponent();
            sQLite = new SQLiteFunctions();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = sQLite.Login(LoginTextBox.Text, PasswordTextBox.Password);
            Role = sQLite.Role;
            if (result)
            {
                switch (sQLite.Role)
                {
                    case "Doctor":
                        Doctor = sQLite.ReturnDoctor(sQLite.CheckFIO);
                        break;
                    case "Patient":
                        Patient = sQLite.ReturnPatient(sQLite.CheckFIO);
                        break;
                }
                this.Close();
            }
        }
        private void RegisterVoidButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterPasswordBox.Password == RepeatRegisterPasswordBox.Password)
            {
                string role = "";
                int onHolyday = 0;
                string specialization = "";
                string gender = "";
                bool result = false;
                if(DoctorRadioButton.IsChecked == true)
                {
                    role = "Doctor";
                    ComboBoxItem typeItem = (ComboBoxItem)RegSpecializationComboBox.SelectedItem;
                    specialization = typeItem.Content.ToString();
                    onHolyday = RegOnHolydayComboBox.SelectedIndex;
                    result = sQLite.Registration(RegLoginTextBox.Text, RegFIOTextBox.Text, Convert.ToInt32(RegAgeTextBox.Text), " ", specialization, onHolyday, Convert.ToInt32(RegWorkExperienceTextBox.Text), RegisterPasswordBox.Password, role);

                }
                if (PatientRadioButton.IsChecked == true)
                {
                    role = "Patient";
                    ComboBoxItem typeItem = (ComboBoxItem)RegGenderComboBox.SelectedItem;
                    gender = typeItem.Content.ToString();
                    result = sQLite.Registration(RegLoginTextBox.Text, RegFIOTextBox.Text, Convert.ToInt32(RegAgeTextBox.Text), gender, " ", 0, 0, RegisterPasswordBox.Password, role);
                }

                Role = sQLite.Role;
                if (result)
                {
                    switch (role)
                    {
                        case "Doctor":
                            Doctor = new Doctor
                            {
                                FIO = RegFIOTextBox.Text,
                                Age = Convert.ToInt32(RegAgeTextBox.Text),
                                OnHolyday = onHolyday,
                                Specialization = specialization //Заглушка
                            };
                            break;
                        case "Patient":
                            Patient = new Patient
                            {
                                FIO = RegFIOTextBox.Text,
                                Age = Convert.ToInt32(RegAgeTextBox.Text),
                                Gender = gender
                            };
                            break;
                    }

                    this.Close();
                }
            } else
            {
                Notifications notifications = new Notifications();
                notifications.ErrorNotification("Пароли не совпадают");
            }

        }

        private void DoctorRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RegGenderStackPanel.Visibility = Visibility.Hidden;
            RegSpecializationStackPanel.Visibility = Visibility.Visible;
            RegOnHolydayStackPanel.Visibility = Visibility.Visible;
            RegWorkExperienceStackPanel.Visibility = Visibility.Visible;
        }

        private void PatientRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RegGenderStackPanel.Visibility = Visibility.Visible;
            RegSpecializationStackPanel.Visibility = Visibility.Hidden;
            RegOnHolydayStackPanel.Visibility = Visibility.Hidden;
            RegWorkExperienceStackPanel.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
