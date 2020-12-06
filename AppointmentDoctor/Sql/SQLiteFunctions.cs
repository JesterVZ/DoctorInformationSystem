using AppointmentDoctor.Model;
using AppointmentWirhDoctor.model;
using AppointmentWithDoctor.model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppointmentWithDoctor.SQL
{
    public class SQLiteFunctions
    {
        private readonly List<Doctor> Doctors;
        private readonly List<Patient> Patients;
        private readonly Notifications notifications = new Notifications();
        readonly ListViewFunctions listViewFunctions;
        readonly ListView ListView;
        public string Role;
        public string CheckFIO;
        public SQLiteFunctions(ListView listView)
        {
            Doctors = new List<Doctor>();
            Patients = new List<Patient>();
            listViewFunctions = new ListViewFunctions();
            ListView = listView;
        }
        public SQLiteFunctions()
        {
            Doctors = new List<Doctor>();
            Patients = new List<Patient>();
            listViewFunctions = new ListViewFunctions();
        }
        public void ApplyChanges(Card patientCard)
        {
            using SQLiteConnection Connection = Connect();
            string commandText = "INSERT INTO [PatientCard] ([date], [FIO], [Age], [Reports], [IsTakingMedication], [HaveHereditaryDiseases], [Diagnosis], [Treatment]) VALUES(@date, @FIO, @Age, @Reports, @IsTakingMedication, @HaveHereditaryDiseases, @Diagnosis, @Treatment)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connection);
            Command.Parameters.AddWithValue("@date", patientCard.Time);
            Command.Parameters.AddWithValue("@FIO", patientCard.FIO);
            Command.Parameters.AddWithValue("@Age", patientCard.Age);
            Command.Parameters.AddWithValue("@Reports", patientCard.Reports);
            Command.Parameters.AddWithValue("@IsTakingMedication", patientCard.IsTakingMedication);
            Command.Parameters.AddWithValue("@HaveHereditaryDiseases", patientCard.HaveHereditaryDiseases);
            Command.Parameters.AddWithValue("@Diagnosis", patientCard.Diagnosis);
            Command.Parameters.AddWithValue("@Treatment", patientCard.Treatment);
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        public void SelectDoctors()
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            string commandText = "SELECT * FROM Doctors";
            using var Command = new SQLiteCommand(commandText, Connection);
            using SQLiteDataReader rdr = Command.ExecuteReader();
            while (rdr.Read())
            {
                Doctors.Add(new Doctor
                {
                    FIO = (string)rdr["FIO"],
                    Age = (int)(long)rdr["Age"],
                    OnHolyday = (int)(long)rdr["OnHolyday"],
                    Specialization = (string)rdr["Specialization"],
                    WorkExperience = (int)(long)rdr["WorkExperience"]
                });

            }
            Connection.Close();
            listViewFunctions.FillingListView(ListView, Doctors);
        }
        public void SelectPatients(string fio)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            string commandText = "SELECT * FROM Visit where doctor_fio = '"+fio+"'";
            using var Command = new SQLiteCommand(commandText, Connection);
            using SQLiteDataReader rdr = Command.ExecuteReader();
            while (rdr.Read())
            {
                Patients.Add(new Patient
                {
                    FIO = (string)rdr["patient_fio"]
                });
            }
            Connection.Close();
            listViewFunctions.FillingListView(ListView, Patients);
        }
        public void InsertPatient(Patient patient)
        {
            using SQLiteConnection Connection = Connect();
            string commandText = "INSERT INTO [Patients] ([FIO], [Age], [Gender]) VALUES(@FIO, @Age, @Gender)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connection);
            Command.Parameters.AddWithValue("@FIO", patient.FIO);
            Command.Parameters.AddWithValue("@Age", patient.Age);
            Command.Parameters.AddWithValue("@Gender", patient.Gender);
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
            //notifications.SuccessNotification(patient.FIO);
        }
        public void InsertVisit(Doctor doctor, Patient patient)
        {
            using SQLiteConnection Connection = Connect();
            string commandText = "INSERT INTO [Visit] ([doctor_fio], [patient_fio]) VALUES(@doctor_fio, @patient_fio)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connection);
            Command.Parameters.AddWithValue("@doctor_fio", doctor.FIO);
            Command.Parameters.AddWithValue("@patient_fio", patient.FIO);
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
            notifications.SuccessNotification(patient.FIO);
        }
        public void InsertDoctor(Doctor doctor)
        {
            using SQLiteConnection Connection = Connect();
            string commandText = "INSERT INTO [Doctors] ([FIO], [Age], [Specialization], [OnHolyday], [WorkExperience]) VALUES(@FIO, @Age, @Specialization, @OnHolyday, @WorkExperience)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connection);
            Command.Parameters.AddWithValue("@FIO", doctor.FIO);
            Command.Parameters.AddWithValue("@Age", doctor.Age);
            Command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            Command.Parameters.AddWithValue("@OnHolyday", doctor.OnHolyday);
            Command.Parameters.AddWithValue("@WorkExperience", doctor.WorkExperience);

            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        private void InsertUser(string login, string FIO, int age, string password, string role)
        {
            using SQLiteConnection Connection = Connect();
            string commandText = "INSERT INTO [Users] ([Login], [FIO], [Age], [Password], [Role]) VALUES(@Login, @FIO, @Age, @Password, @Role)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connection);
            Command.Parameters.AddWithValue("@Login", login);
            Command.Parameters.AddWithValue("@FIO", FIO);
            Command.Parameters.AddWithValue("@Age", age);
            Command.Parameters.AddWithValue("@Password", password);
            Command.Parameters.AddWithValue("@Role", role);

            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        public bool Registration(string login, string FIO, int age, string gender, string specialization, int onHolyday, int workExperience, string password, string role)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            SQLiteDataReader rdr = Reader(Connection, login, password, "Registration", "");
            int count = 0;
            while (rdr.Read())
            {
                count++;
            }
            if (count == 1)
            {
                notifications.ErrorNotification("Данный пользователь уже существует");
                return false;
            }
            else
            {
                InsertUser(login, FIO, age, password, role);
                Role = role;
                switch (role) {
                    case "Doctor":
                        InsertDoctor(new Doctor
                        {
                            FIO = FIO,
                            Age = age,
                            Specialization = specialization,
                            OnHolyday = onHolyday,
                            WorkExperience = workExperience
                        });
                        break;
                    case "Patient":
                        InsertPatient(new Patient
                        {
                            FIO = FIO,
                            Age = age,
                            Gender = gender
                        });
                        break;
                    case "Admin":
                        break;
                }

                return true;

            }
        }
        public bool Login(string login, string password)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            SQLiteDataReader rdr = Reader(Connection, login, password, "Login", "");
            int count = 0;
            while (rdr.Read())
            {
                Role = (string)rdr["Role"];
                CheckFIO = (string)rdr["FIO"];
                count++;
            }
            if(count > 0)
            {
                return true;
            }
            else
            {
                notifications.ErrorNotification("Неверный логин или пароль");
                return false;

            }
        }
        private SQLiteDataReader Reader(SQLiteConnection Connection, string login, string password, string status, string fio)
        {
            SQLiteCommand Command;
            string commandText;
            switch (status)
            {
                case "Login":
                    commandText = "SELECT * FROM Users where Login = @Login and Password = @Password;";
                    Command = new SQLiteCommand(commandText, Connection);
                    Command.Parameters.AddWithValue("@Login", login);
                    Command.Parameters.AddWithValue("@Password", password);
                    return Command.ExecuteReader();
                case "Registration":
                    commandText = "SELECT * FROM Users where Login = @Login;";
                    Command = new SQLiteCommand(commandText, Connection);
                    Command.Parameters.AddWithValue("@Login", login);
                    return Command.ExecuteReader();
                case "Doctor":
                    commandText = "SELECT * FROM Doctors where FIO = @FIO;";
                    Command = new SQLiteCommand(commandText, Connection);
                    Command.Parameters.AddWithValue("@FIO", fio);
                    return Command.ExecuteReader();
                case "Patient":
                    commandText = "SELECT * FROM Patients where FIO = @FIO;";
                    Command = new SQLiteCommand(commandText, Connection);
                    Command.Parameters.AddWithValue("@FIO", fio);
                    return Command.ExecuteReader();
                case "Card":
                    commandText = "SELECT * FROM PatientCard where FIO = @FIO;";
                    Command = new SQLiteCommand(commandText, Connection);
                    Command.Parameters.AddWithValue("@FIO", fio);
                    return Command.ExecuteReader();
            }
            return null;
        }
        public Doctor ReturnDoctor(string fio)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            SQLiteDataReader rdr = Reader(Connection, "", "", "Doctor", fio);
            while (rdr.Read())
            {
                return new Doctor
                {
                    FIO = (string)rdr["FIO"],
                    Age = (int)(long)rdr["Age"],
                    OnHolyday = (int)(long)rdr["OnHolyday"],
                    Specialization = (string)rdr["Specialization"],
                    WorkExperience = (int)(long)rdr["WorkExperience"]
                };
            }
            return null;

        }
        public Patient ReturnPatient(string fio)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            SQLiteDataReader rdr = Reader(Connection, "", "", "Patient", fio);
            while (rdr.Read())
            {
                return new Patient
                {
                    FIO = (string)rdr["FIO"],
                    Age = (int)(long)rdr["Age"],
                    Gender = (string)rdr["Gender"]
                };
            }
            return null;

        }
        public List<Card> ReturnCard(string fio)
        {
            using SQLiteConnection Connection = Connect();
            Connection.Open();
            SQLiteDataReader rdr = Reader(Connection, "", "", "Card", fio);
            List<Card> cards = new List<Card>();
            while (rdr.Read())
            {
                bool bool1, bool2;
                if((int)(long)rdr["IsTakingMedication"] == 1)
                {
                    bool1 = true;
                } else
                {
                    bool1 = false;
                }
                if((int)(long)rdr["HaveHereditaryDiseases"] == 1)
                {
                    bool2 = true;
                } else
                {
                    bool2 = false;
                }
                cards.Add(new Card
                {
                    FIO = (string)rdr["FIO"],
                    Age = (int)(long)rdr["Age"],
                    Reports = (string)rdr["Reports"],
                    IsTakingMedication = bool1,
                    HaveHereditaryDiseases = bool2,
                    Diagnosis = (string)rdr["Diagnosis"],
                    Treatment = (string)rdr["Treatment"],
                    Time = (DateTime)rdr["date"]
                });
            }
            return cards;
        }
        private SQLiteConnection Connect()
        {
            return new SQLiteConnection(@"Data Source=DataBase.db; Version=3;");
        }

    }
}
