using System;

namespace WebUsersStat_MVC.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int NationalNumber { get; set; }

        public double AvgSalary { get; set; }

        public double LargestSalary { get; set; }

        public string UserSalaryStatus { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveStatus { get; set; }

    }
}
