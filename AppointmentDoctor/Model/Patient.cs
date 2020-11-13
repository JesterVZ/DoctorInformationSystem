
namespace AppointmentWirhDoctor.model
{
    public class Patient : Human
    {
        public string Gender { get; set; }
        public Doctor Doctor { get; set; }
    }
}
