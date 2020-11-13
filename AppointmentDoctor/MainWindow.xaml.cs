using AppointmentDoctor.View;
using AppointmentDoctor.ViewModel;
using AppointmentWirhDoctor.model;
using AppointmentWithDoctor.SQL;
using Microsoft.Win32;
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

namespace AppointmentDoctor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginWindow loginWindow;
        private PatientCard patientCard;
        private DiseaseHestory diseaseHestory;
        private readonly SQLiteFunctions sQLite;
        private OpenFileDialog fileDialog;
        private bool? dialogOk;
        public MainWindow()
        {
            loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            InitializeComponent();
            switch (loginWindow.Role)
            {
                case "Doctor":
                    DoctorTabControl.Visibility = Visibility.Visible;
                    PatientTabControl.Visibility = Visibility.Hidden;
                    sQLite = new SQLiteFunctions(PatientListView);
                    OutputInformationAboutDoctor(loginWindow.Doctor, true);
                    sQLite.SelectPatients(loginWindow.Doctor.FIO);
                    break;  
                case "Patient":
                    DoctorTabControl.Visibility = Visibility.Hidden;
                    PatientTabControl.Visibility = Visibility.Visible;
                    sQLite = new SQLiteFunctions(DoctorsListView);
                    OutputInformationAboutPatient(loginWindow.Patient.FIO, loginWindow.Patient.Age, loginWindow.Patient.Gender);
                    sQLite.SelectDoctors();
                    break;
                case "Admin":
                    //Admin = sQLite.ReturnAdmin(sQLite.CheckFIO);
                    break;
            }

        }
        private bool SearchFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchTextBox.Text))
            {
                return true;
            }
            else
            {
                return ((item as Doctor).Specialization.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Doctor).FIO.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        private void OutputInformationAboutPatient(string FIO, int Age, string Gender)
        {
            GenderTextBox.Text = Gender;
            FIOTextBox.Text = FIO;
            AgeTextBox.Text = Age.ToString();


        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DoctorsListView.Items);
            view.Filter = SearchFilter;
            CollectionViewSource.GetDefaultView(DoctorsListView.Items).Refresh();
        }
        private void DoctorsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputInformationAboutDoctor((Doctor)DoctorsListView.SelectedItem, false);
        }
        private void OutputInformationAboutDoctor(Doctor doctor, bool isDoctor)
        {
            try
            {
                if (isDoctor)
                {
                    DoctorFIOTextBlock.DataContext = doctor;
                    DoctorAgeTextBlock.DataContext = doctor;
                    DoctorSpecializationTextBLock.DataContext = doctor;
                    DoctorWorkExperienceTextBLock.DataContext = doctor;
                    switch (doctor.OnHolyday)
                    {
                        case 0:
                            DoctorIsHolydayTextBLock.Text = "Да";
                            break;
                        case 1:
                            DoctorIsHolydayTextBLock.Text = "Нет";
                            break;
                    }
                }
                else
                {
                    FIOTextBlock.DataContext = doctor;
                    AgeTextBlock.DataContext = doctor;
                    SpecializationTextBLock.DataContext = doctor;
                    WorkExperienceTextBLock.DataContext = doctor;
                    switch (doctor.OnHolyday)
                    {
                        case 0:
                            IsHolydayTextBLock.Text = "Да";
                            break;
                        case 1:
                            IsHolydayTextBLock.Text = "Нет";
                            break;
                    }
                }

            }
            catch
            {

            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string PatientGender = GenderTextBox.Text;

            try
            {
                Patient patient = new Patient
                {
                    FIO = FIOTextBox.Text,
                    Age = Convert.ToInt32(AgeTextBox.Text),
                    Gender = PatientGender,
                    Doctor = (Doctor)DoctorsListView.SelectedItem

                };
                Doctor doctor = new Doctor { 
                    FIO = FIOTextBlock.Text
                };
                sQLite.InsertVisit(doctor, patient);
            }
            catch
            {

            }

        }

        /*private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadImageAsync();
        }
        private async void LoadImageAsync()
        {
            await Task.Run(() => LoadImage());
            if (dialogOk == true)
            {
                BitmapImage avatar = new BitmapImage();
                avatar.BeginInit();
                avatar.UriSource = new Uri(fileDialog.FileName);
                avatar.EndInit();
                AvatarImage.Source = avatar;
            }
        }
        private void LoadImage()
        {
            fileDialog = new OpenFileDialog
            {
                Multiselect = false
            };
            dialogOk = fileDialog.ShowDialog();

        }*/

        private void PatientListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient fio = (Patient)PatientListView.SelectedItem;
            patientCard = new PatientCard(sQLite.ReturnPatient(fio.FIO));
            patientCard.Show();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            diseaseHestory = new DiseaseHestory();
            diseaseHestory.Show();
        }
    }
}
