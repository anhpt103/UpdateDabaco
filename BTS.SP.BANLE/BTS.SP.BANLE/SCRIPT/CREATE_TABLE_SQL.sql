﻿USE TBNETERP;
GO
ALTER TABLE DM_KHACHHANG
ADD
HANGKHACHHANG NVARCHAR(50),
HANGKHACHHANGCU NVARCHAR(50)
/
USE [TBNETERP]
GO
--END UPDATE
CREATE TABLE DM_HANGKHACHHANG 
   (
    ID VARCHAR(50) , 
	MAHANGKH VARCHAR(50) , 
	TENHANGKH NVARCHAR(200), 
	SOTIEN decimal(18,2), 
	TYLEGIAMGIASN decimal(18,2), 
	TYLEGIAMGIA decimal(18,2), 
	TRANGTHAI INT, 
	I_CREATE_DATE DATE, 
	I_CREATE_BY VARCHAR(50), 
	I_UPDATE_DATE DATE, 
	I_UPDATE_BY VARCHAR(50), 
	I_STATE VARCHAR(50), 
	UNITCODE VARCHAR(50), 
	QUYDOITIEN_THANH_DIEM decimal(18,2), 
	QUYDOIDIEM_THANH_TIEN decimal(18,2),
	HANG_KHOIDAU decimal(18,2), 
	PRIMARY KEY (ID)
) ;
 
CREATE TABLE AU_DONVI(
	ID VARCHAR(50),
    MADONVI VARCHAR(50),
    MADONVICHA VARCHAR(50),
    TENDONVI NVARCHAR(50),
    SODIENTHOAI VARCHAR(50),
    DIACHI VARCHAR(50),
	TRANGTHAI INT,
	MACUAHANG VARCHAR(50),
	TENCUAHANG VARCHAR(50),
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE AU_NGUOIDUNG(
	ID VARCHAR(50),
    USERNAME VARCHAR(50),
    PASSWORD VARCHAR(50),
    MANHANVIEN VARCHAR(50),
    TENNHANVIEN NVARCHAR(200),
    SODIENTHOAI VARCHAR(20),
	SOCHUNGMINHTHU VARCHAR(20),
	GIOITINH INT,
	TRANGTHAI INT,
	LEVEL INT,
	UNITCODE VARCHAR(50),
	PARENT_UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE AU_THAMSOHETHONG(
	ID VARCHAR(50),
    MA_THAMSO VARCHAR(50),
    TEN_THAMSO NVARCHAR(500),
    GIATRI_THAMSO INT,
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE DM_BOHANG(
	ID VARCHAR(50),
    MABOHANG VARCHAR(50),
    TENBOHANG NVARCHAR(300),
    GHICHU NVARCHAR(500),
	TRANGTHAI INT,
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE DM_BOHANGCHITIET(
	ID VARCHAR(50),
    MABOHANG VARCHAR(50),
    MAHANG VARCHAR(50),
    TENHANG NVARCHAR(500),
	SOLUONG DECIMAL,
	TYLECKLE DECIMAL,
	TYLECKBUON DECIMAL,
	TONGLE DECIMAL,
	DONGIA DECIMAL,
	TONGBUON DECIMAL,
	GHICHU NVARCHAR(500),
	TRANGTHAI INT,
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE DM_KHACHHANG(
	ID VARCHAR(50),
    MAKH VARCHAR(50),
    TENKH NVARCHAR(500),
	DIACHI NVARCHAR(300),
	DIENTHOAI VARCHAR(20),
	CMTND VARCHAR(20),
	EMAIL VARCHAR(100),
	SODIEM DECIMAL,
	TONGTIEN DECIMAL,
	NGAYCAPTHE DATE,
	NGAYHETHAN DATE,
	NGAYSINH DATE,
	UNITCODE VARCHAR(50),
	HANGKHACHHANG NVARCHAR(50),
	HANGKHACHHANGCU NVARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE DM_VATTU(
	ID VARCHAR(50),
    MAVATTU VARCHAR(50),
    TENVATTU NVARCHAR(500),
	MANHACUNGCAP NVARCHAR(50),
	DONVITINH VARCHAR(20),
	BARCODE VARCHAR(2000),
	GIABANBUONVAT VARCHAR(100),
	GIABANLEVAT DECIMAL,
	GIAVON DECIMAL,
	TONCUOIKYSL DECIMAL,
	TYLEVATRA DECIMAL,
	TYLELAILE DECIMAL,
	ITEMCODE VARCHAR(50),
	MAVATRA VARCHAR(20),
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE KHUYENMAI(
	ID VARCHAR(50),
    MACHUONGTRINH VARCHAR(50),
    TUNGAY DATE,
	DENNGAY DATE,
	TUGIO VARCHAR(50),
	DENGIO VARCHAR(50),
	MAVATTU VARCHAR(50),
	SOLUONG DECIMAL,
	TYLE DECIMAL,
	GIATRI DECIMAL,
	PRIMARY KEY (ID)
);

CREATE TABLE NVGDQUAY_ASYNCCLIENT(
	ID VARCHAR(50),
    MAGIAODICH VARCHAR(50),
    MAGIAODICHQUAYPK VARCHAR(50),
	MADONVI VARCHAR(50),
	NGAYTAO DATE,
	NGAYPHATSINH DATE,
	MANGUOITAO VARCHAR(50),
	NGUOITAO NVARCHAR(50),
	MAQUAYBAN VARCHAR(50),
	LOAIGIAODICH INT,
	HINHTHUCTHANHTOAN VARCHAR(50),
	TIENKHACHDUA DECIMAL,
	TIENVOUCHER DECIMAL,
	TIENTHEVIP DECIMAL,
	TIENTRALAI DECIMAL,
	TIENTHE DECIMAL,
	TIENCOD DECIMAL,
	TIENMAT DECIMAL,
	TTIENCOVAT DECIMAL,
	THOIGIAN VARCHAR(50),
	MAKHACHHANG VARCHAR(50),
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);

CREATE TABLE NVHANGGDQUAY_ASYNCCLIENT(
	ID VARCHAR(50),
    MAGDQUAYPK VARCHAR(50),
    MAKHOHANG VARCHAR(50),
	MADONVI VARCHAR(50),
	MAVATTU VARCHAR(50),
	MANGUOITAO VARCHAR(50),
	NGUOITAO NVARCHAR(50),
	MABOPK VARCHAR(50),
	NGAYTAO DATE,
	NGAYPHATSINH DATE,
	SOLUONG DECIMAL,
	TTIENCOVAT DECIMAL,
	GIABANLECOVAT DECIMAL,
	TYLECHIETKHAU DECIMAL,
	TIENCHIETKHAU DECIMAL,
	TYLEKHUYENMAI DECIMAL,
	TIENKHUYENMAI DECIMAL,
	TYLEVOUCHER DECIMAL,
	TIENVOUCHER DECIMAL,
	TYLELAILE DECIMAL,
	GIAVON DECIMAL,
	MAVAT VARCHAR(50),
	VATBAN DECIMAL,
	MACHUONGTRINHKM VARCHAR(50),
	UNITCODE VARCHAR(50),
	PRIMARY KEY (ID)
);