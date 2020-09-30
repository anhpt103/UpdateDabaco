﻿create or replace PROCEDURE NHAPBANBUONTRALAICHITIET(P_TABLE_NAME IN VARCHAR2,P_LOAICHUNGTU IN VARCHAR2, 
P_GROUPBY IN VARCHAR2,P_WAREHOUSE IN VARCHAR2, P_MALOAI IN VARCHAR2,  P_MANHOM IN VARCHAR2, P_NGUOIDUNG IN VARCHAR2,
P_MAVATTU IN VARCHAR2,P_NHACUNGCAP IN VARCHAR2,P_UNITCODE  IN VARCHAR2, P_MATHUE  IN VARCHAR2,
P_TUNGAY IN DATE,P_DENNGAY IN DATE, cur  OUT SYS_REFCURSOR) AS

P_EXPRESSION VARCHAR(3000);
QUERY_STR VARCHAR(3000);
P_TABLE_GROUPBY VARCHAR(3000);
P_COLUMNS_GROUPBY VARCHAR(3000);
P_SELECT_COLUMNS_GROUPBY VARCHAR(3000);
P_SELECT VARCHAR(3000);
BEGIN
IF TRIM(P_MALOAI) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MALOAIVATTU IN ('||P_MALOAI||')';
END IF;
IF TRIM(P_MANHOM) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MANHOMVATTU IN ('||P_MANHOM||')';
END IF;
IF TRIM(P_MAVATTU) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND ct.MAHANG IN ('||P_MAVATTU||')';
END IF;    
IF TRIM(P_NHACUNGCAP) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MAKHACHHANG IN ('||P_NHACUNGCAP||')';
END IF;
IF TRIM(P_MATHUE) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND ct.VAT IN ('||P_MATHUE||')';
END IF;  

IF P_GROUPBY = 'MAKHO' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'c.MAKHO,c.TENKHO,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MAKHO, ''NULL'')  as MaCha, NVL(a.TENKHO, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NGAYCHUNGTU ';
P_TABLE_GROUPBY:= ' INNER JOIN DM_KHO c ON t.MAKHONHAP = c.MAKHO AND c.UNITCODE = '''||P_UNITCODE||'''';
P_SELECT:='t.NGAYCHUNGTU as NGAYCHUNGTU, c.MAKHO AS MAKHO,c.TENKHO as TENKHO,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'MADONVI' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MADONVI,dv.TENDONVI,b.MAVATTU,b.TENVATTU,t.NGAYCHUNGTU, ct.DONGIA, b.GiaBanLeVat ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MADONVI, ''NULL'')  as MaCha, NVL(a.TENDONVI, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NGAYCHUNGTU ';
P_TABLE_GROUPBY:= ' INNER JOIN AU_DONVI dv on b.MADONVI = dv.MADONVI AND dv.MADONVI = '''||P_UNITCODE||'''';
P_SELECT:='t.NGAYCHUNGTU as NGAYCHUNGTU, b.MADONVI AS MADONVI,dv.TENDONVI as TENDONVI,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'MANHOMVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MANHOMVATTU,kh.TENNHOMVT,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MANHOMVATTU, ''NULL'')  as MaCha, NVL(a.TENNHOMVT, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NGAYCHUNGTU ';
P_TABLE_GROUPBY:= ' INNER JOIN DM_NHOMVATTU kh on b.MANHOMVATTU = kh.MANHOMVTU AND kh.UNITCODE = '''||P_UNITCODE||'''';
P_SELECT:='t.NGAYCHUNGTU as NGAYCHUNGTU, b.MANHOMVATTU AS MANHOMVATTU,kh.TENNHOMVT as TENNHOMVT,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'MALOAIVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MALOAIVATTU,kh.TENLOAIVT,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MALOAIVATTU, ''NULL'')  as MaCha, NVL(a.TENLOAIVT, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NgayChungTu ';
P_TABLE_GROUPBY:= ' INNER JOIN DM_LOAIVATTU kh ON b.MALOAIVATTU = kh.MALOAIVATTU AND kh.UNITCODE = '''||P_UNITCODE||'''';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU,b.MALOAIVATTU AS MALOAIVATTU,kh.TENLOAIVT as TENLOAIVT,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'MANHACUNGCAP' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MAKHACHHANG,ncc.TENNCC,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MAKHACHHANG, ''NULL'')  as MaCha, NVL(a.TENNCC, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NgayChungTu ';
P_TABLE_GROUPBY:= ' INNER JOIN DM_NHACUNGCAP ncc on t.MAKHACHHANG = ncc.MANCC AND ncc.UNITCODE = '''||P_UNITCODE||'''';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU ,t.MAKHACHHANG AS MAKHACHHANG,ncc.TENNCC as TENNCC,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'NGUOIDUNG' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.I_CREATE_BY,ncc.TENNHANVIEN,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.USERNAME, ''NULL'')  as MaCha, NVL(a.TENNHANVIEN, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NgayChungTu ';
P_TABLE_GROUPBY:= ' INNER JOIN AU_NGUOIDUNG ncc on t.I_CREATE_BY = ncc.USERNAME AND ncc.UNITCODE = '''||P_UNITCODE||'''';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU ,t.I_CREATE_BY AS USERNAME,ncc.TENNHANVIEN as TENNHANVIEN,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'MALOAITHUE' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'ct.VAT,lt.LOAITHUE,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MALOAITHUE, ''NULL'')  as MaCha, NVL(a.LOAITHUE, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NgayChungTu ';
P_TABLE_GROUPBY:= ' ';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU ,ct.VAT AS MALOAITHUE,lt.LOAITHUE as LOAITHUE,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSIF P_GROUPBY = 'PHIEU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MACHUNGTU,t.MACHUNGTUPK,b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU ';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MACHUNGTU, ''NULL'')  as MaCha, NVL(a.MACHUNGTUPK, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU as NgayChungTu ';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU ,t.MACHUNGTU AS MACHUNGTU,t.MACHUNGTUPK as MACHUNGTUPK,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
ELSE --ELSIF P_GROUPBY = 'MAVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MAVATTU,b.TENVATTU,ct.DONGIA,b.GiaBanLeVat,t.NGAYCHUNGTU';
P_SELECT_COLUMNS_GROUPBY:= ' NVL(a.MAVATTU, ''NULL'')  as MaCha, NVL(a.TENVATTU, ''NULL'') as TenCha,a.MAVATTU as MaCon,a.TENVATTU as TenCon,a.NGAYCHUNGTU AS NgayChungTu ';
P_TABLE_GROUPBY:= ' AND b.UNITCODE = '''||P_UNITCODE||''' ';
P_SELECT:=' t.NGAYCHUNGTU as NGAYCHUNGTU,b.MAVATTU as MAVATTU,b.TENVATTU as TENVATTU';
END;
END IF;
QUERY_STR:= 'SELECT (a.SOLUONG) as SoLuong,(a.VON) as VON ,(a.DONGIANHAP) as DonGiaNhap,a.GiaBanLeVat as GiaBan, (a.TIENHANG) as TienHang, (a.TIENVAT) as TienVat, (a.TIENCHIETKHAU) as TienChietKhau, (a.TIENHANG - a.TIENCHIETKHAU + a.TIENVAT) AS TongTien ,'||P_SELECT_COLUMNS_GROUPBY||'
FROM (
SELECT '||P_SELECT||',SUM(ct.SOLUONG) as SOLUONG,
 SUM(ct.SOLUONG*ROUND(NVL(ct.DONGIA,0))) AS TIENHANG ,
 ROUND(SUM(ROUND(NVL(ct.DONGIA,0))*NVL(lt.TYGIA/100,0)*ct.SOLUONG)) AS TIENVAT,
  0 as TIENCHIETKHAU,
  ROUND(NVL(ct.DONGIA,0)) as DONGIANHAP,
  ROUND(NVL(b.GiaBanLeVat,0)) as GiaBanLeVat,
  ROUND(SUM(NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0)),2) AS VON
FROM VATTUCHUNGTUCHITIET ct 
INNER JOIN VATTUCHUNGTU t on t.MACHUNGTUPK = ct.MACHUNGTUPK 
INNER JOIN V_VATTU_GIABAN b on b.MAVATTU = ct.MAHANG
LEFT JOIN DM_LOAITHUE lt on lt.MALOAITHUE = ct.VAT
'||P_TABLE_GROUPBY||'
WHERE
    t.TRANGTHAI =10
    AND t.UNITCODE = '''||P_UNITCODE||'''
    AND t.NGAYCHUNGTU <=TO_DATE('''||P_DENNGAY||''',''DD/MM/YY'')
    AND t.NGAYCHUNGTU >=TO_DATE('''||P_TUNGAY||''',''DD/MM/YY'') AND t.UNITCODE = '''||P_UNITCODE||''' AND 
    t.LOAICHUNGTU = '''||P_LOAICHUNGTU||''' '||P_EXPRESSION||' GROUP BY '||P_COLUMNS_GROUPBY||' ORDER BY '||P_COLUMNS_GROUPBY||' ) a';
OPEN CUR FOR QUERY_STR;
DBMS_OUTPUT.put_line (QUERY_STR);
  EXCEPTION
   WHEN NO_DATA_FOUND
   THEN
      DBMS_OUTPUT.put_line ('<your message>' || SQLERRM);
   WHEN OTHERS
   THEN
         DBMS_OUTPUT.put_line (QUERY_STR  || SQLERRM);     
END NHAPBANBUONTRALAICHITIET;