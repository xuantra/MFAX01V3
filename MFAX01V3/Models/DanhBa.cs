using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFAX01V3.Models
{
    [Serializable]
    public class DanhBa:BaseViewModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string SoDienThoai { get; set; }
        public string DonVi { get; set; }
        public PhoneType LineType { get; set; }
        public DanhBa()
        {

        }
    }
}
