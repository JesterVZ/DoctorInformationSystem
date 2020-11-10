using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AppointmentWirhDoctor.model
{
    public class Doctor : Human
    {
        public string Specialization { get; set; }
        public int OnHolyday { get; set; }
        public int WorkExperience { get; set; }

    }
}
