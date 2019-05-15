drop database if exists Group1_MotoParkingManagementSystem;
create database if not exists Group1_MotoParkingManagementSystem char set 'utf8'; 

use Group1_MotoParkingManagementSystem;


create table if not exists Customer(
cus_id varchar(10) primary key,
cus_fullname nvarchar(50) not null,
cus_address nvarchar(50) not null,
license_plate varchar(20) not null
);
insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
values ('KH01','Đào Văn Đức','Thái Bình','75-G1-2222'),
('KH02','Lê Chí Dũng','Bắc Giang','33-G1-3333'),
('KH03','Nguyễn Văn A','Hà Nội','22-E1-2222'),
('KH04','Dũng đẹp zai','Hà Nội','44-b1-4444'),
('KH05','Bùi Việt Hoàng','Phú Thọ','88-G1-8888'),
('KH06','Boydung','Bắc Giang','88-A1-8888');
select * from Customer;
create table if not exists Card(
card_id varchar(10) primary key,
card_type nvarchar(50) not null,
license_plate varchar(20),
card_status bit not null default 0
); 
insert into Card(card_id,card_type)
values('CD01','Thẻ ngày'),
('CD02','Thẻ ngày'),
('CD03','Thẻ ngày'),
('CD04','Thẻ ngày'),
('CD05','Thẻ ngày'),
('CD06','Thẻ ngày'),
('CD07','Thẻ ngày'),
('CD08','Thẻ ngày'),
('CD09','Thẻ ngày'),
('CD10','Thẻ ngày'),
('CD11','Thẻ ngày'),
('CD12','Thẻ ngày'),
('CD13','Thẻ ngày'),
('CD14','Thẻ ngày'),
('CD15','Thẻ ngày'),
('CD16','Thẻ ngày'),
('CD17','Thẻ ngày'),
('CD18','Thẻ ngày'),
('CD19','Thẻ ngày'),
('CD20','Thẻ ngày'),
('CD21','Thẻ ngày'),
('CD22','Thẻ ngày'),
('CD23','Thẻ ngày'),
('CD24','Thẻ ngày'),
('CD25','Thẻ ngày');

insert into Card(card_id,card_type)
values('CM01','Thẻ tháng'),
('CM02','Thẻ tháng'),
('CM03','Thẻ tháng'),
('CM04','Thẻ tháng'),
('CM05','Thẻ tháng'),
('CM06','Thẻ tháng');
select * from Card;
create table if not exists Card_details(
card_id varchar(10)  not null,
constraint fk_CardDetails_Card foreign key (card_id) references Card(card_id),
cus_id varchar(10)  not null, 
constraint fk_CardDetails_Customer foreign key (cus_id) references Customer(cus_id),
card_time varchar(50) not null,
date_created datetime not null
);
insert into Card_details(card_id,cus_id,card_time,date_created)
values('CM01','KH01','15/5/2019 - 15/8/2019',current_timestamp()),
('CM02','KH02','15/5/2019 - 15/8/2019',current_timestamp()),
('CM03','KH03','15/5/2019 - 15/8/2019',current_timestamp()),
('CM04','KH04','15/5/2019 - 15/8/2019',current_timestamp()),
('CM05','KH05','15/5/2019 - 15/8/2019',current_timestamp()),
('CM06','KH06','15/5/2019 - 15/8/2019',current_timestamp());
select * from Card_details;
create table if not exists Accounts(
acc_id varchar(10) primary key,
acc_name varchar(50) not null,
acc_pass varchar(30) not null,
acc_fullname nvarchar(50) not null,
acc_email varchar(50) not null,
acc_level bit not null,
acc_dateCreated datetime not null default current_timestamp
);
insert into Accounts(acc_id,acc_name,acc_pass, acc_fullname, acc_email,acc_level)
values('M01','manager_01','24122000','Lê Chí Dũng','boydungbg@gmail.com',0),
('S01','security_01','24122000','Bùi Việt Hoàng','hoangmage@gmail.com',1);
select * from Accounts;
create table if not exists Card_Logs(
cl_id int primary key auto_increment,
card_id varchar(10)  not null,
constraint fk_CardLogs_Card foreign key (card_id) references Card(card_id),
acc_id varchar(10)  not null,
constraint fk_CardLogs_Account foreign key (acc_id) references Accounts(acc_id),
cl_licensePlate varchar(20) not null,
cl_dateTimeStart datetime not null,
cl_dateTimeEnd datetime,
cl_sendTime varchar(20),
cl_intoMoney double
);
drop user if exists 'CTSUser'@'localhost';
create user if not exists 'CTSUser'@'localhost' identified by '123456';
    grant all on Card to 'CTSUser'@'localhost';
    grant all on Customer to 'CTSUser'@'localhost';
    grant all on Card_details to 'CTSUser'@'localhost';
    grant all on Card_Logs to 'CTSUser'@'localhost';
    grant all  on Group1_MotoParkingManagementSystem.Accounts to 'CTSUser'@'localhost';
    grant lock tables on Group1_MotoParkingManagementSystem.* to 'CTSUser'@'localhost';
    -- grant all ON *.* TO 'CTSUser'@'%';


