using AppointmentDoctor.Model;
using AppointmentWirhDoctor.model;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Логика взаимодействия для DiseaseHestory.xaml
    /// </summary>
    public partial class DiseaseHestory : Window
    {
        public DiseaseHestory()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card
            {
                FIO = "ываыа ываыва ываыва",
                Age = 16,
                Gender = "Мужчина",
                Diagnosis = "еблан",
                HaveHereditaryDiseases = false,
                Treatment = "вафаываыва",
                IsTakingMedication = false,
                Doctor = new Doctor
                {
                    FIO = "вава ываываыв ваываваы ваыв"
                }
            });
            cards.Add(new Card
            {
                FIO = "ываыа ываыва ываыва",
                Age = 16,
                Gender = "Мужчина",
                Diagnosis = "еблан",
                HaveHereditaryDiseases = false,
                Treatment = "вафаываыва",
                IsTakingMedication = false
            });
            cards.Add(new Card
            {
                FIO = "ываыа ываыва ываыва",
                Age = 16,
                Gender = "Мужчина",
                Diagnosis = "еблан",
                HaveHereditaryDiseases = false,
                Treatment = "вафаываыва",
                IsTakingMedication = false
            });
            data.ItemsSource = cards;
        }
    }
}
