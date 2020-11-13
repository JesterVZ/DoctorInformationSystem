using AppointmentWirhDoctor.model;

namespace AppointmentDoctor.Model
{
    public class Card : Patient
    {
        public bool IsTakingMedication { get; set; }
        public bool HaveHereditaryDiseases { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}
