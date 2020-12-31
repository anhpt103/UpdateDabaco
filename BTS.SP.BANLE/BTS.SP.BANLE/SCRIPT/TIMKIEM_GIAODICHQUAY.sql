﻿create or replace PROCEDURE "TIMKIEM_GIAODICHQUAY"
(
  P_KEYSEARCH IN VARCHAR2 ,
  P_TUNGAY IN OUT DATE,
  P_DENNGAY IN OUT DATE,
  P_DIEUKIENLOC IN OUT NUMBER,
  P_UNITCODE IN VARCHAR2,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
BEGIN
  IF P_TUNGAY IS NULL OR P_TUNGAY = '' THEN P_TUNGAY := SYSDATE;
  END IF;
  IF P_DENNGAY IS NULL OR P_DENNGAY = '' THEN P_DENNGAY := SYSDATE;
  END IF;
  IF P_DIEUKIENLOC IS NULL OR P_DIEUKIENLOC = '' THEN P_DIEUKIENLOC := '0';
  END IF;
  -- TÌM KIẾM THEO MÃ GIAO DỊCH
  IF P_DIEUKIENLOC = '0' THEN
   QUERY_SELECT := 'SELECT MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                    NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,MAVOUCHER,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,
                    TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE
                    FROM NVGDQUAY_ASYNCCLIENT 
                    WHERE MAGIAODICH LIKE ''%'||P_KEYSEARCH||'%'' AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') AND UNITCODE = '''||P_UNITCODE||''' ORDER BY THOIGIAN DESC ';
  -- TÌM KIẾM THEO SỐ TIỀN
  ELSIF P_DIEUKIENLOC = '1' THEN
   QUERY_SELECT := 'SELECT MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                    NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,MAVOUCHER,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,
                    TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE
                    FROM NVGDQUAY_ASYNCCLIENT 
                    WHERE TTIENCOVAT = TO_NUMBER('||P_KEYSEARCH||') AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') AND UNITCODE = '''||P_UNITCODE||''' ORDER BY THOIGIAN DESC ';
   -- TÌM KIẾM THEO THU NGÂN TẠO   
  ELSE
   QUERY_SELECT := 'SELECT MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                    NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,MAVOUCHER,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,
                    TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE
                    FROM NVGDQUAY_ASYNCCLIENT 
                    WHERE NGUOITAO LIKE N'''||P_KEYSEARCH||'%'' AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') AND TO_DATE(NGAYPHATSINH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') AND UNITCODE = '''||P_UNITCODE||''' ORDER BY THOIGIAN DESC ';
  END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_GIAODICHQUAY;
 
 