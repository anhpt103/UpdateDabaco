﻿create or replace PROCEDURE BANLE_AND_BANBUON_CHITIET(P_GROUPBY IN VARCHAR2, P_MAKHO IN VARCHAR2, P_MALOAI IN VARCHAR2,  
P_MANHOM IN VARCHAR2,P_MAVATTU IN VARCHAR2,P_NHACUNGCAP IN VARCHAR2, P_XUATXU IN VARCHAR2, 
P_UNITCODE IN VARCHAR2, P_TUNGAY IN DATE,P_DENNGAY IN DATE, cur  OUT SYS_REFCURSOR) IS

  P_EXPRESSION VARCHAR(2000);
  QUERY_STR VARCHAR(5000);
  P_SELECT_COLUMNS_IN_GROUPBY VARCHAR(2000);
P_SELECT_COLUMNS_OUT_GROUPBY VARCHAR(2000);
P_TABLE_GROUPBY VARCHAR(2000);
P_COLUMNS_GROUPBY VARCHAR(2000);
  BEGIN
IF TRIM(P_MAKHO) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND ct.MAKHOHANG IN ('||P_MAKHO||')';
END IF;
IF TRIM(P_MALOAI) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MALOAIVATTU IN ('||P_MALOAI||')';
END IF;
IF TRIM(P_MANHOM) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MANHOMVATTU IN ('||P_MANHOM||')';
END IF;
IF TRIM(P_MAVATTU) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MAVATTU IN ('||P_MAVATTU||')';
END IF;    
IF TRIM(P_NHACUNGCAP) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MAKHACHHANG IN ('||P_NHACUNGCAP||')';
END IF;  
IF TRIM(P_XUATXU) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MAXUATXU IN ('||P_XUATXU||')';
END IF;

IF P_GROUPBY = 'MADONVI' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE, a.MA AS MaCon, a.TEN AS TenCon, (a.NGAYPHATSINH) AS NgayGiaoDich , c.MADONVI AS MaCha, c.TENDONVI AS TenCha, (a.GROUPCODE) AS MADONVI';
P_SELECT_COLUMNS_IN_GROUPBY:= 'b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN, t.NGAYPHATSINH AS NGAYPHATSINH , t.MADONVI AS GROUPCODE ';
P_TABLE_GROUPBY:= ' left join AU_DONVI c ON a.GROUPCODE = c.MADONVI ';
END;
ELSIF P_GROUPBY = 'MAKHO' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, t.NGAYPHATSINH, ct.MAKHOHANG, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE, a.MA AS MaCon, a.TEN AS TenCon, (a.NGAYPHATSINH) AS NgayGiaoDich , c.MAKHO AS MaCha, c.TENKHO AS TenCha, (a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= 'b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN, t.NGAYPHATSINH AS NGAYPHATSINH , ct.MAKHOHANG  AS GROUPCODE, t.MADONVI';
P_TABLE_GROUPBY:= ' left join DM_KHO c ON a.GROUPCODE = c.MAKHO AND a.MADONVI= c.UNITCODE ';
END;
ELSIF P_GROUPBY = 'MANHOMVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, b.MANHOMVATTU,t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE, a.MA AS MaCon, a.TEN AS TenCon,c.MANHOMVTU AS MaCha, c.TENNHOMVT AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich, (a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= 'b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN , b.MANHOMVATTU AS GROUPCODE, t.NGAYPHATSINH AS NGAYPHATSINH, t.MADONVI';
P_TABLE_GROUPBY:= ' left join DM_NHOMVATTU c ON a.GROUPCODE = c.MANHOMVTU AND a.MADONVI= c.UNITCODE ';
END;
ELSIF P_GROUPBY = 'MALOAIVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, b.MALOAIVATTU,t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE,a.MA AS MaCon, a.TEN AS TenCon,c.MALOAIVATTU AS MaCha, c.TENLOAIVT AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich, (a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN, b.MALOAIVATTU AS GROUPCODE,t.NGAYPHATSINH AS NGAYPHATSINH, t.MADONVI';
P_TABLE_GROUPBY:= ' left join DM_LOAIVATTU c ON a.GROUPCODE = c.MALOAIVATTU AND a.MADONVI= c.UNITCODE ';
END;
ELSIF P_GROUPBY = 'MANHACUNGCAP' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU,  b.MAKHACHHANG,t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE, a.MA AS MaCon, a.TEN AS TenCon, c.MANCC AS MaCha, c.TENNCC AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich, (a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN, b.MAKHACHHANG AS GROUPCODE,t.NGAYPHATSINH AS NGAYPHATSINH, t.MADONVI';
P_TABLE_GROUPBY:= ' left join DM_NHACUNGCAP c ON a.GROUPCODE = c.MANCC AND a.MADONVI= c.UNITCODE ';
END;
ELSIF P_GROUPBY = 'MAGIAODICH' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE,b.MAVATTU, b.TENVATTU,t.MAGIAODICHQUAYPK,t.NGAYPHATSINH,t.LOAIGIAODICH,t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE,a.MA AS MaCon, a.TEN AS TenCon,  a.MAGIAODICHQUAYPK AS MaCha, a.LOAIGIAODICH AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich,(a.MADONVI) AS MADONVI';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MAVATTU AS MA, b.TENVATTU AS TEN,b.BARCODE AS BARCODE, t.MAGIAODICHQUAYPK AS MAGIAODICHQUAYPK,t.LOAIGIAODICH AS LOAIGIAODICH ,t.NGAYPHATSINH, t.MADONVI';
P_TABLE_GROUPBY:= ' ';
END;
ELSIF P_GROUPBY = 'MAXUATXU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, b.MAXUATXU,t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE,a.MA AS MaCon, a.TEN AS TenCon,c.MAXUATXU AS MaCha, c.TENXUATXU AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich, (a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.BARCODE AS BARCODE, b.MAVATTU AS MA, b.TENVATTU AS TEN, b.MAXUATXU AS GROUPCODE,t.NGAYPHATSINH AS NGAYPHATSINH, t.MADONVI';
P_TABLE_GROUPBY:= ' left join DMXUATXU c ON a.GROUPCODE = c.MAXUATXU AND a.MADONVI= c.UNITCODE ';
END;
ELSE --ELSIF P_GROUPBY = 'MAVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.BARCODE, b.MAVATTU, b.TENVATTU, t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.BARCODE AS BARCODE, a.MA AS MaCon, a.TEN AS TenCon,  a.MA AS MaCha, a.TEN AS TenCha,(a.NGAYPHATSINH) AS NgayGiaoDich, (a.MADONVI) AS MADONVI';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.BARCODE AS BARCODE,b.MAVATTU AS MA, b.TENVATTU AS TEN ,t.NGAYPHATSINH AS NGAYPHATSINH,t.MADONVI AS MADONVI';
P_TABLE_GROUPBY:= ' ';
END;
END IF;

QUERY_STR := 'select (a.SOLUONGBAN) as SOLUONGBAN,(ROUND(a.VONCHUAVAT,2)) as VONCHUAVAT,(ROUND(a.VON,2)) as VON,(ROUND(a.TIENTHUE,2)) AS TIENTHUE,
                     (ROUND(a.DOANHTHU,2)) AS DOANHTHU,(ROUND(a.TONGBAN,2)) AS TONGBAN, (ROUND(a.TIENKHUYENMAI,2)) AS TIENKHUYENMAI, (ROUND(a.TIENCHIETKHAU,2)) AS TIENCHIETKHAU,(ROUND(a.LAIBANLE,2)) AS LAIBANLE,
                     (ROUND(a.TIENVOUCHER,2)) AS TIENVOUCHER,
                     '||P_SELECT_COLUMNS_OUT_GROUPBY||'
from (
SELECT  '||P_SELECT_COLUMNS_IN_GROUPBY||'
                          ,SUM(NVL(ct.SOLUONG,0)) as SOLUONGBAN
                          ,SUM(NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0)) AS VONCHUAVAT
                          ,SUM(NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0) * (1 + NVL(b.TYLEVATVAO,0)/100)) AS VON
                          ,SUM(NVL(ct.TIENKHUYENMAI, 0)) AS TIENKHUYENMAI
                          ,SUM(NVL(ct.TIENCHIETKHAU, 0)) AS TIENCHIETKHAU
                          ,SUM(NVL(ct.TIENVOUCHER, 0)) AS TIENVOUCHER
                          ,SUM((NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENVOUCHER, 0)) / (1+ NVL(ct.VATBAN,0) / 100) ) as DOANHTHU 
                          ,SUM( NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENVOUCHER, 0)) as TONGBAN
                          ,SUM((NVL(ct.VATBAN,0) / 100) * ((NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENVOUCHER, 0)) / (1+ NVL(ct.VATBAN,0) / 100)) ) as TIENTHUE
                          ,SUM(NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENVOUCHER, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0) * (1 + NVL(b.TYLEVATVAO,0)/100)) AS LAIBANLE 
from NVHANGGDQUAY_ASYNCCLIENT ct 
inner join NVGDQUAY_ASYNCCLIENT t on t.MAGIAODICHQUAYPK = ct.MAGDQUAYPK 
left join V_VATTU_GIABAN b on b.MAVATTU = ct.MAVATTU AND b.UNITCODE= t.MADONVI
WHERE
    t.LOAIGIAODICH = 1 OR t.LOAIGIAODICH = 3
    AND t.MADONVI = '''||P_UNITCODE||'''
    AND t.NGAYPHATSINH <=TO_DATE('''||P_DENNGAY||''',''DD/MM/YY'')
    AND t.NGAYPHATSINH >=TO_DATE('''||P_TUNGAY||''',''DD/MM/YY'') 
      '||P_EXPRESSION||' group by '||P_COLUMNS_GROUPBY||'
     ORDER BY t.NGAYPHATSINH DESC) a	  
	  '||P_TABLE_GROUPBY || ' ORDER BY a.NGAYPHATSINH,a.MA';
 DBMS_OUTPUT.put_line (QUERY_STR );    
  OPEN cur FOR QUERY_STR;
  EXCEPTION
   WHEN NO_DATA_FOUND
   THEN
      DBMS_OUTPUT.put_line ('<your message>' || SQLERRM);
   WHEN OTHERS
   THEN
         DBMS_OUTPUT.put_line (QUERY_STR  || SQLERRM);     

  END BANLE_AND_BANBUON_CHITIET;