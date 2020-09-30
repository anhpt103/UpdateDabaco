CREATE OR REPLACE PROCEDURE BAOCAO_BLETRALAI_TONGHOP (P_GROUPBY IN VARCHAR2,P_MAKHO IN VARCHAR2, P_MALOAI IN VARCHAR2,  P_MANHOM IN VARCHAR2,P_MAVATTU IN VARCHAR2, P_MAKHACHHANG IN VARCHAR2, P_UNITCODE IN VARCHAR2, P_TUNGAY IN DATE, P_DENNGAY IN DATE, CUR  OUT SYS_REFCURSOR) AS
QUERY_STR VARCHAR(32767);
P_SELECT_COLUMNS_IN_GROUPBY VARCHAR(32767);
P_SELECT_COLUMNS_OUT_GROUPBY VARCHAR(32767);
P_TABLE_GROUPBY VARCHAR(32767);
P_JOIN_DKIEN VARCHAR(32767);
P_COLUMNS_GROUPBY VARCHAR(32767);
P_EXPRESSION  VARCHAR(32767):= '';
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
IF TRIM(P_MAKHACHHANG) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND b.MAKHACHHANG IN ('||P_MAKHACHHANG||')';
END IF;  

IF P_GROUPBY = 'MADONVI' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= ' c.MADONVI AS Ma, c.TENDONVI AS Ten,a.MA AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' t.MADONVI AS MA ';
P_JOIN_DKIEN := 'a.MA = j.MA';
P_TABLE_GROUPBY:= ' left join AU_DONVI c ON a.MA = c.MADONVI ';
END;
ELSIF P_GROUPBY = 'MAKHO' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'ct.MAKHOHANG, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= ' c.MAKHO AS Ma, c.TENKHO AS Ten,a.MADONVI AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' ct.MAKHOHANG AS MA,t.MADONVI ';
P_JOIN_DKIEN := 'a.MADONVI = j.MADONVI AND a.MA = j.MA';
P_TABLE_GROUPBY:= ' left join DM_KHO c ON a.MA = c.MAKHO AND SUBSTR(a.MADONVI,0,3)= SUBSTR(c.UNITCODE,0,3)';
END;
ELSIF P_GROUPBY = 'MANHOMVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MANHOMVATTU, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= ' c.MANHOMVTU AS Ma, c.TENNHOMVT AS Ten,a.MADONVI AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MANHOMVATTU AS MA, t.MADONVI ';
P_JOIN_DKIEN := 'a.MADONVI = j.MADONVI AND a.MA = j.MA';
P_TABLE_GROUPBY:= ' left join DM_NHOMVATTU c ON a.MA = c.MANHOMVTU  AND SUBSTR(a.MADONVI,0,3)= SUBSTR(c.UNITCODE,0,3)';
END;
ELSIF P_GROUPBY = 'MALOAIVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MALOAIVATTU, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'c.MALOAIVATTU AS Ma, c.TENLOAIVT AS Ten,a.MADONVI AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MALOAIVATTU as MA, t.MADONVI ';
P_JOIN_DKIEN := 'a.MADONVI = j.MADONVI AND a.MA = j.MA';
P_TABLE_GROUPBY:= ' left join DM_LOAIVATTU c ON a.MA = c.MALOAIVATTU AND SUBSTR(a.MADONVI,0,3)= SUBSTR(c.UNITCODE,0,3) ';
END;
ELSIF P_GROUPBY = 'MAKHACHHANG' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MAKHACHHANG, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'c.MAKH AS Ma, c.TENKH AS Ten,a.MADONVI AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' t.MAKHACHHANG AS MA, t.MADONVI ';
P_JOIN_DKIEN := 'a.MADONVI = j.MADONVI AND a.MA = j.MA';
P_TABLE_GROUPBY:= '  left join DM_KHACHHANG c ON a.MA = c.MAKH AND SUBSTR(a.MADONVI,0,3)= SUBSTR(c.UNITCODE,0,3)';
END;
ELSIF P_GROUPBY = 'MAGIAODICH' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MAGIAODICH, t.NGAYPHATSINH, t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'TO_CHAR(a.MA) AS Ma, TO_CHAR(a.TEN) AS Ten,TO_CHAR(a.MADONVI) AS MADONVI ';
P_SELECT_COLUMNS_IN_GROUPBY:= 't.MAGIAODICH AS MA, t.NGAYPHATSINH AS TEN, t.MADONVI AS MADONVI ';
P_TABLE_GROUPBY:= ' ';
P_JOIN_DKIEN := 'a.MA = j.MA AND a.MADONVI = j.MADONVI AND a.TEN = j.TEN';
END;
ELSE --ELSIF P_GROUPBY = 'MAVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MAVATTU,b.TENVATTU,t.MADONVI';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.MA AS Ma, a.TEN AS Ten,a.MADONVI AS MADONVI ';
P_JOIN_DKIEN := 'a.MADONVI = j.MADONVI AND a.MA = j.MA AND a.TEN = j.TEN';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MAVATTU AS MA, b.TENVATTU AS TEN, t.MADONVI ';
P_TABLE_GROUPBY:= ' ';
END;
END IF;

QUERY_STR := 'select a.SOLUONGBAN as SOLUONGBAN,a.VONCHUAVAT as VONCHUAVAT,a.VON as VON,a.TIENTHUE AS TIENTHUE,
                     a.DOANHTHU AS DOANHTHU,a.TONGBAN AS TONGBAN, a.TIENCHIETKHAU AS TIENCHIETKHAU, a.TIENKHUYENMAI AS TIENKHUYENMAI,a.LAIBANLE AS LAIBANLE,
                     j.TIENTHE AS TIENTHE, j.TIENCOD AS TIENCOD, j.TIENMAT AS TIENMAT, a.TIENVOUCHER AS TIENVOUCHER,
                     '||P_SELECT_COLUMNS_OUT_GROUPBY||'
from 
(SELECT '||P_SELECT_COLUMNS_IN_GROUPBY||'
                          ,SUM(NVL(ct.SOLUONG,0)) as SOLUONGBAN
                          ,SUM(NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0)) AS VONCHUAVAT
                          ,SUM(NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0) * (1 + NVL(b.TYLEVATVAO,0)/100)) AS VON
                          ,SUM(NVL(ct.TIENCHIETKHAU, 0)) AS TIENCHIETKHAU
                          ,SUM(NVL(ct.TIENKHUYENMAI, 0)) AS TIENKHUYENMAI
                          ,SUM(NVL(ct.TIENVOUCHER, 0)) AS TIENVOUCHER
                          ,SUM((NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENVOUCHER, 0)) / (1+ NVL(ct.VATBAN,0) / 100) ) as DOANHTHU 
                          ,SUM( NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENVOUCHER, 0)) as TONGBAN
                          ,SUM((NVL(ct.VATBAN,0) / 100) * ((NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0)- NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENVOUCHER, 0)) / (1+ NVL(ct.VATBAN,0) / 100)) ) as TIENTHUE
                          ,SUM(NVL(ct.SOLUONG,0) * NVL(ct.GIABANLECOVAT,0) - NVL(ct.TIENKHUYENMAI, 0) - NVL(ct.TIENCHIETKHAU, 0) - NVL(ct.TIENVOUCHER, 0) - NVL(ct.GIAVON,0) * NVL(ct.SOLUONG,0) * (1 + NVL(b.TYLEVATVAO,0)/100)) AS LAIBANLE 
from NVHANGGDQUAY_ASYNCCLIENT ct 
inner join NVGDQUAY_ASYNCCLIENT t on t.MAGIAODICHQUAYPK = ct.MAGDQUAYPK 
left join V_VATTU_GIABAN b on b.MAVATTU = ct.MAVATTU AND SUBSTR(b.MADONVI,0,3)= SUBSTR(t.MADONVI,0,3)
WHERE
    t.LOAIGIAODICH = 2
    AND t.MADONVI LIKE '''||P_UNITCODE||'%''
    AND t.NGAYPHATSINH <=TO_DATE('''||P_DENNGAY||''',''DD/MM/YY'')
    AND t.NGAYPHATSINH >=TO_DATE('''||P_TUNGAY||''',''DD/MM/YY'') 
	'||P_EXPRESSION||'
	group by '||P_COLUMNS_GROUPBY||' ) a
--SUM CHILDREN
INNER JOIN  (SELECT '||P_SELECT_COLUMNS_IN_GROUPBY||'
                          ,SUM(NVL(t.TIENTHE,0)) AS TIENTHE
                          ,SUM(NVL(t.TIENCOD,0)) AS TIENCOD
                          ,SUM(NVL(t.TIENMAT,0)) AS TIENMAT
from NVHANGGDQUAY_ASYNCCLIENT ct 
inner join NVGDQUAY_ASYNCCLIENT t on t.MAGIAODICHQUAYPK = ct.MAGDQUAYPK 
left join V_VATTU_GIABAN b on b.MAVATTU = ct.MAVATTU AND SUBSTR(b.UNITCODE,0,3)= SUBSTR(t.MADONVI,0,3)
WHERE
    t.LOAIGIAODICH = 2
    AND t.MADONVI LIKE '''||P_UNITCODE||'%''
    AND t.NGAYPHATSINH <=TO_DATE('''||P_DENNGAY||''',''DD/MM/YY'')
    AND t.NGAYPHATSINH >=TO_DATE('''||P_TUNGAY||''',''DD/MM/YY'') 
	'||P_EXPRESSION||'
	group by '||P_COLUMNS_GROUPBY||' ) j on '||P_JOIN_DKIEN||'
 '||P_TABLE_GROUPBY;
BEGIN
DBMS_OUTPUT.put_line (QUERY_STR);  
OPEN CUR FOR QUERY_STR;
EXCEPTION
            WHEN NO_DATA_FOUND THEN
             NULL;
               WHEN OTHERS THEN
      NULL;
END;

END BAOCAO_BLETRALAI_TONGHOP;