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
        List<Card> Cards;
        public DiseaseHestory(List<Card> cards)
        {
            try
            {
                Cards = cards;
                InitializeComponent();
                OutputOnformation(cards[0]);
            }
            catch
            {

            }

        }

        private void OutputOnformation(Card card)
        {
            FioTextBlock.Text = card.FIO;
            AgeTextBlock.Text = card.Age.ToString();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            data.ItemsSource = Cards;
        }
    }
}
