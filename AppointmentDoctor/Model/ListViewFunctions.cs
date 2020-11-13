using AppointmentWirhDoctor.model;
using System.Collections.Generic;
using System.Windows.Controls;

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
    }
}
