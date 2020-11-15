using AppointmentWirhDoctor.model;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppointmentWithDoctor.model
{
    public class ListViewFunctions
    {
        public void FillingListView(ListView listView, List<Doctor> doctors)
        {
            for (int i = 0; i < doctors.Count; i++)
            {
                listView.Items.Add(doctors[i]);
            }
        }
        public void FillingListView(ListView listView, List<Patient> patients)
        {
            for (int i = 0; i < patients.Count; i++)
            {
                listView.Items.Add(patients[i]);
            }
        }
        public int GetHoverIndex(ListView listView)
        {
            var item = VisualTreeHelper.HitTest(listView, Mouse.GetPosition(listView)).VisualHit;
            int index = 0;
            while (item != null && !(item is ListViewItem))
            {
                item = VisualTreeHelper.GetParent(item);
            }
            if (item != null)
            {
                index = listView.Items.IndexOf(((ListBoxItem)item).DataContext);
            }
            return index;
        }
    }
}
