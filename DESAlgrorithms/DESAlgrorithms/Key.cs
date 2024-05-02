using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlgrorithms
{
    public class Key
    {
        public int STT { get; set; }
       public string Khoa { get; set; }

        public Key(int STT, string key_Round)
        {
            this.STT = STT;
            this.Khoa = key_Round;
        }
    }
}
