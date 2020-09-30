﻿
  CREATE OR REPLACE FORCE VIEW "TBNETERP"."V_GET_TICHDIEM" ("MAGIAODICH", "MADONVI", "MAQUAYBAN", "NGUOITAO", "TTIENCOVAT", "MAKHACHHANG", "NGAYTAO") AS 
  SELECT MAGIAODICH, MADONVI , MAQUAYBAN,  CONCAT( concat(NGUOITAO,'-'),MANGUOITAO )  as NGUOITAO , 
TTIENCOVAT, MAKHACHHANG, 
-- CONCAT(concat(NGAYTAO,' '), concat( Thoigian,':00')) as NGAYTAO
TO_CHAR(NGAYTAO , 'yyyy-MM-dd HH:mm:ss') as NGAYTAO
-- NGAYTAO
FROM NVGDQUAY_ASYNCCLIENT 
where TO_CHAR( NGAYTAO,'MM-DD-YYYY') =TO_CHAR(SYSDATE, 'MM-DD-YYYY')   
and MAKHACHHANG is not null 
ORDER BY  THOIGIAN desc
 
 ;
 
