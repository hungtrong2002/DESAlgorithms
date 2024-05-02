using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlgrorithms
{
    public class PlainText
    {
        public  int STT { get; set; }
        public string nua_Trai{ get; set; }
        public string nua_Phai{ get; set; }
        public PlainText(int STT, string nua_Trai, string nua_Phai)
        {
            this.STT = STT;
            this.nua_Trai = nua_Trai;
            this.nua_Phai = nua_Phai;
        }

    }
}
