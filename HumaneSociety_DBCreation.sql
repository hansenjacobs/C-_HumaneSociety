USE [master]
GO

/****** Object:  Database [HumaneSociety]    Script Date: 3/14/2018 2:41:28 PM ******/
CREATE DATABASE [HumaneSociety]

/* ABOVE IS THE FIRST QUERY, EVERYTHING BELOW IS THE SECOND QUERY */

USE [HumaneSociety]
GO

CREATE TABLE Catagories (ID INTEGER IDENTITY (1,1) PRIMARY KEY, catagory VARCHAR(50));
CREATE TABLE Breeds (ID INTEGER IDENTITY (1,1) PRIMARY KEY, breed VARCHAR(50), catagory INTEGER FOREIGN KEY REFERENCES Catagories(ID), pattern VARCHAR(50));
CREATE TABLE DietPlans(ID INTEGER IDENTITY (1,1) PRIMARY KEY, food VARCHAR(50), amount INTEGER);
CREATE TABLE Rooms (ID INTEGER IDENTITY (1,1) PRIMARY KEY, name VARCHAR(50), building VARCHAR(50));
CREATE TABLE Shots (ID INTEGER IDENTITY (1,1) PRIMARY KEY, name VARCHAR(50));
CREATE TABLE Animals (ID INTEGER IDENTITY (1,1) PRIMARY KEY, name VARCHAR(50), breed INTEGER FOREIGN KEY REFERENCES Breeds(ID), weight INTEGER, age INTEGER, diet INTEGER FOREIGN KEY REFERENCES DietPlans(ID), location INTEGER FOREIGN KEY REFERENCES Rooms(ID));
CREATE TABLE AnimalShotJunctions (Animal_ID INTEGER FOREIGN KEY REFERENCES Animals(ID), Shot_ID INTEGER FOREIGN KEY REFERENCES Shots(ID), dateRecieved DATE, CONSTRAINT animalShotPrimaryKey PRIMARY KEY (Animal_ID, Shot_ID));
CREATE TABLE USStates (ID INTEGER IDENTITY (1,1) PRIMARY KEY, name VARCHAR(50), abbrev VARCHAR(10));
CREATE TABLE UserAddresses (ID INTEGER IDENTITY (1,1) PRIMARY KEY, addessLine1 VARCHAR(50), addressLine2 VARCHAR(50), zipcode INTEGER, USStates INTEGER FOREIGN KEY REFERENCES USStates(ID)); 
CREATE TABLE Clients (ID INTEGER IDENTITY (1,1) PRIMARY KEY, firstName VARCHAR(50), lastName VARCHAR(50), userName VARCHAR(50), pass VARCHAR(50), userAddress INTEGER FOREIGN KEY REFERENCES UserAddresses(ID), email VARCHAR(50));
CREATE TABLE ClientAnimalJunctions (client INTEGER FOREIGN KEY REFERENCES Clients(ID), animal INTEGER FOREIGN KEY REFERENCES Animals(ID) CONSTRAINT ClientAnimalKey PRIMARY KEY (client, animal)); 
CREATE TABLE Employees (ID INTEGER IDENTITY (1,1) PRIMARY KEY, firsttName VARCHAR(50), lastName VARCHAR(50), userName VARCHAR(50), pass VARCHAR(50), employeeNumber INTEGER, email VARCHAR(50));

INSERT INTO USStates Values('Alabama','AL');
INSERT INTO USStates Values('Alaska','AK');
INSERT INTO USStates Values('Arizona','AZ');
INSERT INTO USStates Values('Arkansas','AR');
INSERT INTO USStates Values('California','CA');
INSERT INTO USStates Values('Colorado','CO');
INSERT INTO USStates Values('Connecticut','CT');
INSERT INTO USStates Values('Delaware','DE');
INSERT INTO USStates Values('Florida','FL');
INSERT INTO USStates Values('Georgia','GA');
INSERT INTO USStates Values('Hawaii','HI');
INSERT INTO USStates Values('Idaho','ID');
INSERT INTO USStates Values('Illinois','IL');
INSERT INTO USStates Values('Indiana','IN');
INSERT INTO USStates Values('Iowa','IA');
INSERT INTO USStates Values('Kansa','KS');
INSERT INTO USStates Values('Kentucky','KY');
INSERT INTO USStates Values('Louisiana','LA');
INSERT INTO USStates Values('Maine','ME');
INSERT INTO USStates Values('Maryland','MD');
INSERT INTO USStates Values('Massachusetts','MA');
INSERT INTO USStates Values('Michigan','MI');
INSERT INTO USStates Values('Minnesota','MN');
INSERT INTO USStates Values('Mississippi','MS');
INSERT INTO USStates Values('Missouri','MO');
INSERT INTO USStates Values('Montana','MT');
INSERT INTO USStates Values('Nebraska','NE');
INSERT INTO USStates Values('Nevada','NV');
INSERT INTO USStates Values('New Hampshire','NH');
INSERT INTO USStates Values('New Jersey','NJ');
INSERT INTO USStates Values('New Mexico','NM');
INSERT INTO USStates Values('New York','NY');
INSERT INTO USStates Values('New Hampshire','NH');
INSERT INTO USStates Values('North Carolina','NC')
INSERT INTO USStates Values('North Dakota','ND');
INSERT INTO USStates Values('Ohio','OH');
INSERT INTO USStates Values('Oklahoma','OK');
INSERT INTO USStates Values('Oregon','OR');
INSERT INTO USStates Values('Pennsylvania','PA');
INSERT INTO USStates Values('Rhode Island','RI');
INSERT INTO USStates Values('South Carolina','SC');
INSERT INTO USStates Values('South Dakota','SD');
INSERT INTO USStates Values('Tennessee','TN');
INSERT INTO USStates Values('Texas','TX');
INSERT INTO USStates Values('Utah','UT');
INSERT INTO USStates Values('Vermont','VT');
INSERT INTO USStates Values('Virgina','VA');
INSERT INTO USStates Values('Washington','WA');
INSERT INTO USStates Values('West Virgina','WV');
INSERT INTO USStates Values('Wisconsin','WI');
INSERT INTO USStates Values('Wyoming','WY');

INSERT INTO Shots (name) VALUES ('booster');

ALTER TABLE Animals ADD demeanor VARCHAR(50);
ALTER TABLE Animals ADD kidFriendly BIT;
ALTER TABLE Animals ADD petFriendly BIT;
ALTER TABLE Animals ADD gender BIT;
ALTER TABLE Animals ADD adoptionStatus VARCHAR(50);
ALTER TABLE Animals ADD Employee_ID INTEGER FOREIGN KEY REFERENCES Employees(ID);
ALTER TABLE Clients ADD income INTEGER;
ALTER TABLE Clients ADD kids INTEGER; 
ALTER TABLE Clients ADD homeSize INTEGER;
ALTER TABLE ClientAnimalJunctions ADD approvalStatus VARCHAR(50);