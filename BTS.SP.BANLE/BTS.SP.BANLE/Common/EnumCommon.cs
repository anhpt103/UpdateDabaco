using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.SP.BANLE.Common
{
    public class EnumCommon{
        public enum MethodGetPrice
        {
            GIABANLECOVAT = 1,
            GIABANBUONCOVAT = 3,
            GIAVON = 3,
        }

        public enum CountFieldGridView
        {
            STT = 0,
            MAVATTU = 1,
            TENVATTU = 2,
            DONVITINH = 3,
            DONGIA = 4,
            SOLUONG = 5,
            GIAVON = 6,
            TYLECK = 7,
            TIENCK = 8,
            THANHTIEN = 9
        }
        public enum MethodPay
        {
            TIENMAT = 0,
            TIENTHE = 1,
            PHIEUMUAHANG = 2
        }

        public enum LoaiGiaoDich
        {
            BANLE = 1,
            TRALAI = 2,
        }

        public enum MethodSelect
        {
            SUDUNG = 1,
            KHONGSUDUNG = 0,
        }

        public class DisableButton
        {
            public bool F1 { get; set; }
            public bool F2 { get; set; }
            public bool F3 { get; set; }
            public bool F4 { get; set; }
            public bool F5 { get; set; }
        }
    }
}
