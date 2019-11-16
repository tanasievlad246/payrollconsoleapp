using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;    

namespace Payroll_App
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Staff> myStaff = new List<Staff>();
            FileReader fr = new FileReader();
            int month = 0;
            int year = 0;

            while (year == 0)
            {
                Console.WriteLine("\n Enter the year: ");

                try
                {
                    year = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("You need to enter a valid year");
                }
            }

            while (month == 0)
            {
                Console.WriteLine("\n Enter the month: ");

                try
                {
                    month = Convert.ToInt32(Console.ReadLine());

                    if (month < 1 || month > 12)
                    {
                        Console.WriteLine("Your month needs to be a number between 1 and 12");
                        month = 0;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("You need to enter a valid month number between 1 and 12");
                }
            }

            myStaff = fr.ReadFile();

            for (int i = 0; i < myStaff.Count; i++)
            {
                try
                {
                    Console.WriteLine("Enter the hours worked for {0}", myStaff[i].NameOfStaff);
                    myStaff[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(myStaff[i].ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    i--;
                }
            }

            PaySlip ps = new PaySlip(month, year);
            ps.GeneratePaySlip(myStaff);
            ps.GenerateSummary(myStaff);
            Console.ReadLine();
        }
    }

    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }
        public int HoursWorked
        {
            get { return hWorked; }
            set
            {
                if (value > 0)
                {
                    hWorked = value;
                }
                else
                {
                    hWorked = 0;
                }
            }
        }

        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay.........");
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "Name = " + NameOfStaff + " Hours Worked = " + HoursWorked + " Hourly Rate = " + hourlyRate + " Total Pay = " + TotalPay;
        }
    }

        class Manager : Staff
        {
            private const float managerHourlyRate = 50;

            public int Allowance { get; private set; }

            public Manager(string name) : base(name, managerHourlyRate) { }

            public override void CalculatePay()
            {
                base.CalculatePay();
                Allowance = 1000;

                if (HoursWorked > 160)
                {
                    TotalPay += Allowance;
                }
            }

            public override string ToString()
            {
                return "Title: Manager Name = " + NameOfStaff + " Hours Worked = " + HoursWorked + " Hourly Rate = " + managerHourlyRate + " Total Pay = " + TotalPay;
            }
        }

        class Admin : Staff
        {
            private const float overtimeRate = 15.5F;
            private const float adminHourlyRate = 30;

            public float Overtime { get; private set; }

            public Admin(string name) : base(name, adminHourlyRate) { }

            public override void CalculatePay()
            {
                if (HoursWorked > 160)
                {
                    Overtime = overtimeRate * (HoursWorked - 160);

                }

                base.CalculatePay();
            }

            public override string ToString()
            {
                return "Title: Admin Name = " + NameOfStaff + " Hours Worked = " + HoursWorked + " Hourly Rate = " + adminHourlyRate + " Overtime Rate = " + overtimeRate + " Total Pay = " + TotalPay;
            }
        }

        class FileReader
        {
            public List<Staff> ReadFile()
            {
                List<Staff> myStaff = new List<Staff>();
                string[] result = new string[2];
                string path = "staff.txt";
                string[] separator = { ", " };

                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        while (sr.EndOfStream != true) 
                        {
                            string[] strList = sr.ReadLine().Split(separator, StringSplitOptions.None);
                            result[0] = strList[0];
                            result[1] = strList[1];
                            if (result[1] == "Manager")
                            {
                                myStaff.Add(new Manager(result[0]));
                            }
                            else
                            {
                                myStaff.Add(new Admin(result[0]));
                            }
                        }
                        sr.Close();
                    }
                }
                else
                {
                    Console.WriteLine("the file staff.txt does not exist, add a file called staff.txt \n to the project folder and add staff in it in the for 'name title' \n");
                }
                return myStaff;
            }
        }

        class PaySlip
        {
            private int month;
            private int year;

            enum MonthsOfYear { Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec}

            public PaySlip(int payMonth, int payYear)
            {
                month = payMonth;
                year = payYear;
            }

            public void GeneratePaySlip(List<Staff> myStaff)
            {
                string path;

                foreach (Staff f in myStaff)
                {
                    path = f.NameOfStaff + ".txt";
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine("1. PAYSLIP FOR {0} {1}", (MonthsOfYear)month, year);
                        sw.WriteLine("2. ===========================================");
                        sw.WriteLine("3. Name of Staff: {0}", f.NameOfStaff);
                        sw.WriteLine("4. Hours Wroked: {0}", f.HoursWorked);
                        sw.WriteLine("5.");
                        sw.WriteLine("6. Basic Pay: {0:C}", f.BasicPay);
                        sw.WriteLine("7. Allowance: {0:C}", ((Manager)f).Allowance);
                        sw.WriteLine("7. Allowance: {0:C}", ((Admin)f).Overtime);
                        sw.WriteLine("8. ===========================================");
                        sw.WriteLine("9. Total Pay: {0:C}", f.TotalPay);
                        sw.WriteLine("10. ===========================================");
                        sw.Close();
                    }
                }
            }

            public void GenerateSummary(List<Staff> myStaff)
            {
                var lazyEmployees =
                    from e in myStaff
                    where e.HoursWorked < 10
                    orderby e.NameOfStaff ascending
                    select new { e.NameOfStaff, e.HoursWorked };

                string path = "summary.txt";

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("1. Staff with less than 10 hours worked");
                    sw.WriteLine("2. ");
                    foreach (var emp in lazyEmployees)
                    {
                        sw.WriteLine("Name of Staff: {0} Hours Worked: {1}", emp.NameOfStaff, emp.NameOfStaff);
                    }
                    sw.Close();
                }
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }
    }

