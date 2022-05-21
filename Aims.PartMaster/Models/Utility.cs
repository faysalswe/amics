using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.PartMaster.Models
{
     
    public class Utility
    {
        public string ReturnZeros(int number)
        {
            string zeros = string.Empty;
            if (number == 2)
            {
                zeros = "00";
            }
            else if (number == 3)
            {
                zeros = "000";
            }
            else if (number == 4)
            {
                zeros = "0000";
            }
            else if (number == 5)
            {
                zeros = "00000";
            }
            else if (number == 6)
            {
                zeros = "000000";
            }

            //siva dev

            return zeros;
        }

    }
}
