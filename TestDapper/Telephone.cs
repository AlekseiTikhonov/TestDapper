using System;
using System.Collections.Generic;
using System.Text;


namespace TestDapper
{
    public class Telephone
    {
        public string TelephoneStr { get; set; }

        public Telephone()
        {
        }

        public Telephone(string name)
        {
            TelephoneStr = name;
        }

        public override string ToString()
        {
            return TelephoneStr;
        }

    }
}
