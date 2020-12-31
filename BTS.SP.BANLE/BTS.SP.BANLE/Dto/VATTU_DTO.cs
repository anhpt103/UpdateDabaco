namespace BTS.SP.BANLE.Dto
{
    public class VATTU_DTO
    {
        public string MAVATTU { get; set; }
        public string TENVATTU { get; set; }
        public string DONVITINH { get; set; }
        public string MANHACUNGCAP { get; set; }
        public string BARCODE { get; set; }
        public decimal GIABANBUONVAT { get; set; }
        public decimal GIABANLEVAT { get; set; }
        public decimal GIAVON { get; set; }
        public decimal SOLUONG { get; set; }
        public string ITEMCODE { get; set; }
        public string MAVATRA { get; set; }
        public decimal TYLEVATRA { get; set; }
        public string MABO { get; set; }
        public decimal TIENKHUYENMAIRETURN { get; set; }
        public decimal TONGLEBOHANG { get; set; }
        public decimal TYLELAILE { get; set; }
        public bool LAMACAN { get; set; }
        public decimal TONCUOIKYSL { get; set; }
        public class ViewModelGrid
        {
            public string BARCODE { get; set; }
            public string MAVATTU { get; set; }
            public string TENVATTU { get; set; }
            public string DONVITINH { get; set; }
            public decimal GIABANLECOVAT { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal GIAVON { get; set; }
            public decimal TYLECK { get; set; }
            public decimal TYLELAILE { get; set; }
            public decimal TIENCK { get; set; }
            public decimal TYLEKM { get; set; }
            public decimal TIENKM { get; set; }
            public decimal TTIENCOVAT { get; set; }
            public string MAVATRA { get; set; }
            public decimal TYLEVATRA { get; set; }
            public decimal TIENKHUYENMAIRETURN { get; set; }
            public string ITEMCODE { get; set; }
            public string THANHTIENFULL { get; set; } //Thành tiền đầy đủ thông tin
            public string MAVATTAT { get; set; }  //Mã VAT viết tắt
        }

        public class OBJ_VAT
        {
            public string MAVATRA { get; set; }
            public decimal TYLEVATRA { get; set; }
            public decimal CHUACO_GTGT { get; set; }
            public decimal CO_GTGT { get; set; }
        }
    }
}
