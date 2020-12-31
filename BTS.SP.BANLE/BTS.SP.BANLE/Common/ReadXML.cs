using BTS.SP.BANLE.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BTS.SP.BANLE.Common
{
    public class ReadXML
    {
        
        public static List<NVGDQUAY_ASYNCCLIENT_DTO> TIMKIEM_GIAODICHQUAY_FROM_XML( DateTime TuNgay, DateTime DenNgay)
        {
            DateTime date = DateTime.Now;
            List<NVGDQUAY_ASYNCCLIENT_DTO> listReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            try
            {//LẤY DANH SÁCH CÁC FILE CHỨA GIAO DỊCH TÌM KIẾM
                foreach (DateTime? day in EachDay(TuNgay, DenNgay))
                {
                    if (day.HasValue)
                    {
                        date = (DateTime)day;
                        string pathXmlGiaoDichQuay = Config.path + "NVGDQUAY" + day.Value.ToString("ddMMyyyy") + ".xml";
                        if (File.Exists(pathXmlGiaoDichQuay))
                        {
                            XmlDataDocument xmldoc = new XmlDataDocument();
                            XmlNodeList xmlnode;
                            FileStream fs = new FileStream(pathXmlGiaoDichQuay, FileMode.Open, FileAccess.Read);
                            xmldoc.Load(fs);
                            xmlnode = xmldoc.GetElementsByTagName("DATAROW");
                            for (int i = 0; i <= xmlnode.Count - 1; i++)
                            {
                                NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
                                GDQUAY_DTO.MAGIAODICH = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                                GDQUAY_DTO.MAGIAODICHQUAYPK = xmlnode[i].ChildNodes.Item(1).InnerText.Trim();
                                GDQUAY_DTO.MADONVI = xmlnode[i].ChildNodes.Item(2).InnerText.Trim();
                                GDQUAY_DTO.LOAIGIAODICH = int.Parse(xmlnode[i].ChildNodes.Item(3).InnerText.Trim());
                                DateTime dateNow = DateTime.ParseExact(xmlnode[i].ChildNodes.Item(4).InnerText.Trim(), "dd/MM/yyyy", null);
                                GDQUAY_DTO.NGAYTAO = dateNow;
                                GDQUAY_DTO.MANGUOITAO = xmlnode[i].ChildNodes.Item(5).InnerText.Trim();
                                GDQUAY_DTO.NGUOITAO = xmlnode[i].ChildNodes.Item(6).InnerText.Trim();
                                GDQUAY_DTO.MAQUAYBAN = xmlnode[i].ChildNodes.Item(7).InnerText.Trim();
                                DateTime dateNgayPhatSinh = DateTime.ParseExact(xmlnode[i].ChildNodes.Item(8).InnerText.Trim(), "dd/MM/yyyy", null);
                                GDQUAY_DTO.NGAYPHATSINH = dateNgayPhatSinh;
                                GDQUAY_DTO.HINHTHUCTHANHTOAN = xmlnode[i].ChildNodes.Item(9).InnerText.Trim();
                                if (xmlnode[i].ChildNodes.Item(10) != null) GDQUAY_DTO.MAVOUCHER = xmlnode[i].ChildNodes.Item(10).InnerText.Trim();
                                else GDQUAY_DTO.MAVOUCHER = ";";
                                decimal TIENKHACHDUA = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(11).InnerText.Trim(), out TIENKHACHDUA);
                                GDQUAY_DTO.TIENKHACHDUA = TIENKHACHDUA;
                                decimal TIENVOUCHER = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(12).InnerText.Trim(), out TIENVOUCHER);
                                GDQUAY_DTO.TIENVOUCHER = TIENVOUCHER;
                                decimal TIENTHEVIP = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(13).InnerText.Trim(), out TIENTHEVIP);
                                GDQUAY_DTO.TIENTHEVIP = TIENTHEVIP;
                                decimal TIENTRALAI = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(14).InnerText.Trim(), out TIENTRALAI);
                                GDQUAY_DTO.TIENTRALAI = TIENTRALAI;
                                decimal TIENTHE = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(15).InnerText.Trim(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal TIENCOD = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(16).InnerText.Trim(), out TIENCOD);
                                GDQUAY_DTO.TIENCOD = TIENCOD;
                                decimal TIENMAT = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(17).InnerText.Trim(), out TIENMAT);
                                GDQUAY_DTO.TIENMAT = TIENMAT;
                                decimal TTIENCOVAT = 0;
                                decimal.TryParse(xmlnode[i].ChildNodes.Item(18).InnerText.Trim(), out TTIENCOVAT);
                                GDQUAY_DTO.TTIENCOVAT = TTIENCOVAT;
                                GDQUAY_DTO.THOIGIAN = xmlnode[i].ChildNodes.Item(19).InnerText.Trim();
                                GDQUAY_DTO.MAKHACHHANG = xmlnode[i].ChildNodes.Item(20).InnerText.Trim();
                                GDQUAY_DTO.I_CREATE_BY = xmlnode[i].ChildNodes.Item(21).InnerText.Trim();
                                DateTime I_CREATE_DATE = DateTime.ParseExact(xmlnode[i].ChildNodes.Item(22).InnerText.Trim(), "dd/MM/yyyy", null);
                                GDQUAY_DTO.I_CREATE_DATE = I_CREATE_DATE;
                                DateTime I_UPDATE_DATE = DateTime.ParseExact(xmlnode[i].ChildNodes.Item(23).InnerText.Trim(), "dd/MM/yyyy", null);
                                GDQUAY_DTO.I_UPDATE_DATE = I_UPDATE_DATE;
                                GDQUAY_DTO.I_UPDATE_BY = xmlnode[i].ChildNodes.Item(24).InnerText.Trim();
                                GDQUAY_DTO.I_STATE = xmlnode[i].ChildNodes.Item(25).InnerText.Trim();
                                GDQUAY_DTO.UNITCODE = xmlnode[i].ChildNodes.Item(26).InnerText.Trim();
                                for (int iNode = 27; iNode < xmlnode[i].ChildNodes.Count; iNode++)
                                {
                                    if (xmlnode[i].ChildNodes[iNode] != null)
                                    {
                                        NVHANGGDQUAY_ASYNCCLIENT details = new NVHANGGDQUAY_ASYNCCLIENT();
                                        details.ID = xmlnode[i].ChildNodes[iNode].ChildNodes[0].InnerText.ToString();
                                        details.MAGDQUAYPK = xmlnode[i].ChildNodes[iNode].ChildNodes[1].InnerText.ToString();
                                        details.MAKHOHANG = xmlnode[i].ChildNodes[iNode].ChildNodes[2].InnerText.ToString();
                                        details.MADONVI = xmlnode[i].ChildNodes[iNode].ChildNodes[3].InnerText.ToString();
                                        details.MAVATTU = xmlnode[i].ChildNodes[iNode].ChildNodes[4].InnerText.ToString();
                                        details.BARCODE = xmlnode[i].ChildNodes[iNode].ChildNodes[5].InnerText.ToString();
                                        details.DONVITINH = "";
                                        details.TENDAYDU = xmlnode[i].ChildNodes[iNode].ChildNodes[6].InnerText.ToString();
                                        details.NGUOITAO = xmlnode[i].ChildNodes[iNode].ChildNodes[7].InnerText.ToString();
                                        details.MABOPK = xmlnode[i].ChildNodes[iNode].ChildNodes[8] != null ? xmlnode[i].ChildNodes[iNode].ChildNodes[8].InnerText : "BH";
                                        details.NGAYTAO = DateTime.Now;
                                        details.NGAYPHATSINH = DateTime.Now;
                                        decimal SOLUONG = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[11].InnerText.ToString(), out SOLUONG);
                                        details.SOLUONG = SOLUONG;
                                        decimal TTIENCOVAT_DETAILS = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[12].InnerText.ToString(), out TTIENCOVAT_DETAILS);
                                        details.TTIENCOVAT = TTIENCOVAT_DETAILS;
                                        decimal VATBAN = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[13].InnerText.ToString(), out VATBAN);
                                        details.VATBAN = VATBAN;
                                        decimal GIABANLECOVAT = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[14].InnerText.ToString(), out GIABANLECOVAT);
                                        details.GIABANLECOVAT = GIABANLECOVAT;
                                        details.MAKHACHHANG = xmlnode[i].ChildNodes[iNode].ChildNodes[15].InnerText.ToString();
                                        details.MAKEHANG = xmlnode[i].ChildNodes[iNode].ChildNodes[16].InnerText.ToString();
                                        details.MACHUONGTRINHKM = xmlnode[i].ChildNodes[iNode].ChildNodes[17].InnerText.ToString();
                                        details.LOAIKHUYENMAI = xmlnode[i].ChildNodes[iNode].ChildNodes[18].InnerText.ToString();
                                        decimal TIENCHIETKHAU = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[19].InnerText.ToString(), out TIENCHIETKHAU);
                                        details.TIENCHIETKHAU = TIENCHIETKHAU;
                                        decimal TYLECHIETKHAU = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[20].InnerText.ToString(), out TYLECHIETKHAU);
                                        details.TYLECHIETKHAU = TYLECHIETKHAU;
                                        decimal TYLEKHUYENMAI = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[21].InnerText.ToString(), out TYLEKHUYENMAI);
                                        details.TYLEKHUYENMAI = TYLEKHUYENMAI;
                                        decimal TIENKHUYENMAI = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[22].InnerText.ToString(), out TIENKHUYENMAI);
                                        details.TIENKHUYENMAI = TIENKHUYENMAI;
                                        decimal TYLEVOUCHER = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[23].InnerText.ToString(), out TYLEVOUCHER);
                                        details.TYLEVOUCHER = TYLEVOUCHER;
                                        decimal TIENVOUCHER_DETAILS = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[24].InnerText.ToString(), out TIENVOUCHER_DETAILS);
                                        details.TIENVOUCHER = TIENVOUCHER_DETAILS;
                                        decimal TYLELAILE = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[25].InnerText.ToString(), out TYLELAILE);
                                        details.TYLELAILE = TYLELAILE;
                                        decimal GIAVON = 0;
                                        decimal.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[26].InnerText.ToString(), out GIAVON);
                                        details.GIAVON = GIAVON;
                                        int ISBANAM = 0;
                                        int.TryParse(xmlnode[i].ChildNodes[iNode].ChildNodes[27].InnerText.ToString(), out ISBANAM);
                                        details.ISBANAM = ISBANAM;
                                        details.MAVAT = "";
                                        GDQUAY_DTO.LST_DETAILS.Add(details);
                                    }
                                }
                                listReturn.Add(GDQUAY_DTO);
                            }
                        }
                    }
                }
            }
            catch(Exception ex){
                Console.Write(date);
            }
            return listReturn;
        }

        private static IEnumerable<DateTime?> EachDay(DateTime tuNgay, DateTime denNgay)
        {
            // Remove time info from start date (we only care about day). 
            DateTime currentDay = new DateTime(tuNgay.Year, tuNgay.Month, tuNgay.Day);
            while (currentDay <= denNgay)
            {
                yield return currentDay;
                currentDay = currentDay.AddDays(1);
            }
        }
    }
}
