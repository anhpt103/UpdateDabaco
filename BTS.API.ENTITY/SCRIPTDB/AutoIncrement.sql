CREATE SEQUENCE AUTO_INCREMENT_SEQ_DonVi START WITH 1;

CREATE OR REPLACE TRIGGER increment_DonVi 
BEFORE INSERT ON DMDONVI 
FOR EACH ROW
BEGIN
  SELECT AUTO_INCREMENT_SEQ_DonVi.NEXTVAL
  INTO   :new.INDEX
  FROM   dual;
END;
/

CREATE SEQUENCE AUTO_INCREMENT_SEQ_Profiles START WITH 1;
CREATE OR REPLACE TRIGGER increment_frofiles
BEFORE INSERT ON PROFILES 
FOR EACH ROW
BEGIN
  SELECT AUTO_INCREMENT_SEQ_Profiles.NEXTVAL
  INTO   :new.INDEX
  FROM   dual;
END;
/

CREATE SEQUENCE AUTO_INCREMENT_SEQ_WareHouse START WITH 1;

CREATE OR REPLACE TRIGGER increment_WareHouse
BEFORE INSERT ON DMKHO
FOR EACH ROW
BEGIN
  SELECT AUTO_INCREMENT_SEQ_WareHouse.NEXTVAL
  INTO   :new.INDEX
  FROM   dual;
END;