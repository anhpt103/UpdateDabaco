using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.SP.BANLE.Dto
{
    public class BILL_DTO
    {
        public int LOAIGD { get; set; }
        public DateTime NgayTao { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string MAGIAODICH { get; set; }
        public string INFOTHUNGAN { get; set; }
        public string MAKH { get; set; }
        public string THANHTIENCHU { get; set; }
        public decimal CONLAI { get; set; }
        public decimal TIENKHACHTRA { get; set; }
        public decimal DIEM { get; set; }
        public string QUAYHANG { get; set; }
    }
}
