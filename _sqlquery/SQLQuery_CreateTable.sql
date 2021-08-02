create database TaxationForest


go
use TaxationForest

create table ActivityLog( --Журнал активности
ID_Log int identity,
Log_Date date,
Log_Time time,
Log_Source nvarchar(25),
Log_Action nvarchar(25),
Log_Message nvarchar(25),
Log_Description nvarchar(25),

ID_Log_User int,

constraint PK_ActivityLog primary key(ID_Log),
constraint FK_ActivityLog foreign key(ID_Log_User) references Users(ID_User)
)







create table Country( -- Cтрана
ID_Country int identity,
Country_Name int,

constraint PK_Country primary key (ID_Country),
)
create table Area( --Район
ID_Area int identity,
Area_Name int,

constraint PK_Area primary key (ID_Area)
)
create table Company( --Предприятие
ID_Company int identity,
Company_Name int,

constraint PK_Company primary key (ID_Company)
)
create table Leshoz ( --Лесхоз
ID_Leshoz int identity,
Leshoz_Name int,

constraint PK_Leshoz primary key (ID_Leshoz)
)
create table Forestry (--Лесничество
ID_Forestry int identity,
Forestry_Name int,

constraint PK_Forestry primary key (ID_Forestry)
)

create table ForestryObjects( -- Объект Лесничества
ID_ForestryObjects int identity,

ID_Country int, --FK
ID_Area int, --FK
ID_Company int, --FK
ID_Leshoz int, --FK
ID_Forestry int, --FK

constraint PK_ForestryObjects primary key (ID_ForestryObjects),
constraint FK_CountryForestryObjects  foreign key(ID_Country) references Country(ID_Country),
constraint FK_AreaForestryObjects foreign key(ID_Area) references Area(ID_Area),
constraint FK_CompanyForestryObjects foreign key(ID_Company) references Company(ID_Company),
constraint FK_LeshozForestryObjects foreign key(ID_Leshoz) references Leshoz(ID_Leshoz),
constraint FK_ForestryForestryObjects foreign key(ID_Forestry) references Forestry(ID_Forestry),
)




create table Users( -- Пользователь
ID_User int identity,
U_UserName nvarchar(25) unique,
U_Password nvarchar(25),

constraint PK_Users primary key (ID_User)
)


create table Quarters ( --Кварталы
ID_Quarters int identity,
Num_Quarters int,

constraint PK_Quarters primary key (ID_Quarters)

)

create table Videls ( --Выделы
ID_Videl int identity,

--PK
V_Num_Videl int,

V_Videl_Area float,
V_Type_Land nvarchar,
V_Num_Podvidels int,
V_Erosion int,

V_Procent int,

V_TLC  nvarchar(25),
V_YearFelling date,
V_NumberStumps  nvarchar(25),
V_D_stumps  nvarchar(25),
ID_User int,
ID_Quarters int,
constraint PK_Videls primary key (ID_Videl),
constraint FK_VidelsUsers foreign key(ID_User) references Users(ID_User),
constraint FK_VidelsQuarters foreign key(ID_Quarters) references Quarters(ID_Quarters)
)





--FK
/*
ID_ORL int, 
ID_Slope int,
ID_Bnum int,

ID_First int,
ID_Second int,
ID_Third int,
ID_Target_species int,

ID_Breed int,
ID_Bonitet int,
ID_ForestType int,
*/







