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
    /// Логика взаимодействия для PatientCard.xaml
    /// </summary>
    public partial class PatientCard : Window
    {
        public List<TagTemplate> TagList = new List<TagTemplate>();
        private readonly ColorGenerator ColorGenerator = new ColorGenerator();
        public PatientCard(Patient patient)
        {
            InitializeComponent();
            OutputInformationAboutPatient(patient);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TagTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTag(TagTextBox.Text, ColorGenerator.GenerateColor());
            }
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            AddTag(TagTextBox.Text, ColorGenerator.GenerateColor());
        }
        private void AddTag(string value, string color)
        {
            TagList.Add(new TagTemplate()
            {
                TagValue = value,
                Color = color
            });
            TagsListView.ItemsSource = TagList;
            TagTextBox.Text = "";
            CollectionViewSource.GetDefaultView(TagsListView.ItemsSource).Refresh();
        }
        private void OutputInformationAboutPatient(Patient patient)
        {
            FIOTextBlock.DataContext = patient;
            AgeTextBlock.DataContext = patient;
            GenderTextBlock.DataContext = patient;
        }
    }
}
