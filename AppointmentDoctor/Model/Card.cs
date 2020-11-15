using AppointmentWirhDoctor.model;
using System;

namespace AppointmentDoctor.Model
{
    public class Card : Patient
    {
        public DateTime Time { get; set; }
        public string Reports { get; set; }
        public bool IsTakingMedication { get; set; }
        public bool HaveHereditaryDiseases { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}
