using System.Windows;

namespace AppointmentWithDoctor.model
{
    public class Notifications
    {
        public void SuccessNotification(string FIO)
        {
            MessageBox.Show("Уважаемый " + FIO + ", Ваша заявка внесена в базу данных", "Спасибо!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ErrorNotification(string error)
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
