using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDapper
{
    
    public class Telephones : List<Telephone>
    {
        public override string ToString()
        {
            return string.Join(",",this);
        }

        public static Telephones FromString(string value)
        {
            Telephones result = new Telephones();
            string[] telephones = value.ToString().Split(',');
            result.AddRange(telephones.Select(t => new Telephone(t)));
            return result;
        }

    }
}