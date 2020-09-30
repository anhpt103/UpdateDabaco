﻿create or replace PROCEDURE BAOCAO_TONKHO_TONGHOP_THEOKHO(P_KY IN VARCHAR2,P_GROUPBY IN VARCHAR2, P_MAKHO IN VARCHAR2, P_MALOAI IN VARCHAR2,  P_MANHOM IN VARCHAR2,P_MAVATTU IN VARCHAR2, P_NHACUNGCAP IN VARCHAR2, P_UNITCODE IN VARCHAR2, CUR  OUT SYS_REFCURSOR) IS
QUERY_STR VARCHAR(32767);
P_SELECT_COLUMNS_IN_GROUPBY VARCHAR(32767);
P_SELECT_COLUMNS_OUT_GROUPBY VARCHAR(32767);
P_TABLE_GROUPBY VARCHAR(32767);
P_COLUMNS_GROUPBY VARCHAR(32767);
P_EXPRESSION  VARCHAR(32767):= '';
BEGIN
IF TRIM(P_MAKHO) IS NOT NULL THEN
P_EXPRESSION := P_EXPRESSION||' AND t.MAKHO IN ('||P_MAKHO||')';
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
--

IF P_GROUPBY = 'MAKHO' THEN
BEGIN
P_COLUMNS_GROUPBY:= 't.MAKHO';
P_SELECT_COLUMNS_OUT_GROUPBY:= ' c.MAKHO AS Ma, c.TENKHO AS Ten ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' t.MAKHO AS MA ';
P_TABLE_GROUPBY:= ' left join DMKHO c ON a.MA = c.MAKHO ';
END;
ELSIF P_GROUPBY = 'MANHOMVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MANHOMVATTU';
P_SELECT_COLUMNS_OUT_GROUPBY:= ' c.MANHOMVTU AS Ma, c.TENNHOMVT AS Ten ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MANHOMVATTU AS MA ';
P_TABLE_GROUPBY:= ' left join DMNHOMVATTU c ON a.MA = c.MANHOMVTU ';
END;
ELSIF P_GROUPBY = 'MALOAIVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MALOAIVATTU';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'c.MALOAIVATTU AS Ma, c.TENLOAIVT AS Ten ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MALOAIVATTU as MA ';
P_TABLE_GROUPBY:= ' left join DMLOAIVATTU c ON a.MA = c.MALOAIVATTU ';
END;
ELSIF P_GROUPBY = 'MAKHACHHANG' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MAKHACHHANG';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'c.MAKH AS Ma, c.TENKH AS Ten ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MAKHACHHANG AS MA ';
P_TABLE_GROUPBY:= ' left join DMKHACHHANG c ON a.MA = c.MAKH AND c.LOAIKHACHHANG = 1';
END;
ELSE --ELSIF P_GROUPBY = 'MAVATTU' THEN
BEGIN
P_COLUMNS_GROUPBY:= 'b.MAVATTU,b.TENVATTU';
P_SELECT_COLUMNS_OUT_GROUPBY:= 'a.MA AS Ma, a.TEN AS Ten ';
P_SELECT_COLUMNS_IN_GROUPBY:= ' b.MAVATTU AS MA, b.TENVATTU AS TEN ';
P_TABLE_GROUPBY:= ' ';
END;
END IF;

QUERY_STR := 'SELECT TO_CHAR(a.TONCUOIKYSL) as TONCUOIKYSL,TO_CHAR(a.TONCUOIKYGT) as TONCUOIKYGT,
'||P_SELECT_COLUMNS_OUT_GROUPBY||'
FROM (
SELECT  '||P_SELECT_COLUMNS_IN_GROUPBY||'
                          ,SUM(NVL(t.TONCUOIKYSL,0)) as TONCUOIKYSL
                          ,SUM(NVL(t.TONCUOIKYGT,0)) as TONCUOIKYGT
from '||P_KY||' t
LEFT JOIN DMVATTU b on b.MAVATTU = t.MAVATTU
WHERE
    t.UNITCODE ='''||P_UNITCODE||''' AND t.TONCUOIKYGT <> 0
	'||P_EXPRESSION||'
	GROUP BY  '||P_COLUMNS_GROUPBY||') a
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
END BAOCAO_TONKHO_TONGHOP_THEOKHO;