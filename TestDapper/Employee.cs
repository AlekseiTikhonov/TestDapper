using System;
using System.Collections.Generic;
using System.Text;

namespace TestDapper
{
    public class Employee
    {
        public string first_name { get; set; }
        public string last_name { get; set; }

        public int Id { get; set; }

        public Telephones telephone { get; set; }
    }
}
