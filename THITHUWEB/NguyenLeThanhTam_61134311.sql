CREATE DATABASE Thi61CNTT_61134311
GO
USE Thi61CNTT_61134311
GO
CREATE TABLE LOAITAISAN
(
	MaLTS nvarchar(10) PRIMARY KEY,
	TenLTS nvarchar(50) NOT NULL
)
GO
CREATE TABLE TAISAN
(
	MaTS nvarchar(10) PRIMARY KEY,
	TenTS nvarchar(10) NOT NULL,
	DVT nvarchar(20) NOT NULL,
	XuatSu bit,
	DonGia int,
	AnhMH nvarchar(50),
	MaLTS nvarchar(10) NOT NULL FOREIGN KEY REFERENCES LOAITAISAN(MaLTS)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	GhiChu nvarchar(100) NOT NULL
)
GO


INSERT INTO TAISAN VALUES('TS0001',N'VÀNG',N'LƯỢNG',1,4000000,N'employee.png',N'LTS2',N'VÀNG THU MUA TRONG DỊCH')
INSERT INTO TAISAN VALUES('TS0002',N'ĐÔ LA',N'USD',1,4000000,N'employee.png',N'LTS3',N'NGOẠI TỆ')
INSERT INTO TAISAN VALUES('TS0003',N'ĐẤT',N'KM2',1,4000000,N'employee.png',N'LTS1',N'ĐẤT ĐAI QUI HOẠCH')
INSERT INTO TAISAN VALUES('TS0004',N'XE',N'CHIẾC',1,1000000,N'employee.png',N'LTS2',N'XE MÁY')


INSERT INTO LOAITAISAN VALUES('LTS1',N'BẤT ĐỘNG SẢN')
INSERT INTO LOAITAISAN VALUES('LTS2',N'ĐỘNG SẢN')
INSERT INTO LOAITAISAN VALUES('LTS3',N'NGOẠI TỆ')




CREATE PROCEDURE TaiSan_TimKiem
	@TenTS nvarchar(40)=NULL,
	@giaMin varchar(30)=NULL,
	@giaMax varchar(30)=NULL
AS
BEGIN
DECLARE @SqlStr NVARCHAR(4000),
		@ParamList nvarchar(2000)
SELECT @SqlStr = '
       SELECT * 
       FROM TAISAN
       WHERE  (1=1)
       '
IF @TenTS IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
              AND (TenTS LIKE ''%'+@TenTS+'%'')
              '
IF @giaMin IS NOT NULL and @giaMax IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
             AND (DonGia Between Convert(int,'''+@giaMin+''') AND Convert(int, '''+@giaMax+'''))
             '
      
	EXEC SP_EXECUTESQL @SqlStr
END


exec TaiSan_TimKiem N'điện thoại', NULL

