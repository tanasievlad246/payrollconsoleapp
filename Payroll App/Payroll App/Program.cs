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
            return "Name = " + NameOfStaff + " Hours Worked = " + hWorked + " Hourly Rate = " + hourlyRate + " Total Pay = " + TotalPay;
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
                return "Title: Manager Name = " + NameOfStaff + " Hours Worked = " + hWorked + " Hourly Rate = " + hourlyRate + " Total Pay = " + TotalPay;
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
                return "Title: Admin Name = " + NameOfStaff + " Hours Worked = " + hWorked + " Hourly Rate = " + hourlyRate + " Overtime Rate = " + overtimeRate + " Total Pay = " + TotalPay;
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
                        string[] strList = sr.ReadLine().Split(separator, 2, StringSplitOptions.None);
                        result[0] = strList[0];
                        result[1] = strList[1];
                    }
                }
            }
        }

        class PaySlip
        {

        }
    }
}
