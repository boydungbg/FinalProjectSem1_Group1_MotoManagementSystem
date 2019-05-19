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
values ('123456789','Đào Văn Đức','Thái Bình','75-G1-2222'),
('123456709','Lê Chí Dũng','Bắc Giang','33-G1-3333'),
('122332387','Nguyễn Văn A','Hà Nội','22-E1-2222'),
('123445785','Dũng đẹp zai','Hà Nội','44-b1-4444'),
('122446775','Bùi Việt Hoàng','Phú Thọ','88-G1-8888'),
('123567436','Boydung','Bắc Giang','88-A1-8888');
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

insert into Card(card_id,card_type,license_plate)
values('CM01','Thẻ tháng','75-G1-2222'),
('CM02','Thẻ tháng','33-G1-3333'),
('CM03','Thẻ tháng','22-E1-2222'),
('CM04','Thẻ tháng','44-b1-4444'),
('CM05','Thẻ tháng','88-G1-8888'),
('CM06','Thẻ tháng','88-A1-8888');
select * from Card;
create table if not exists Card_detail(
card_id varchar(10)  not null,
constraint fk_CardDetail_Card foreign key (card_id) references Card(card_id),
cus_id varchar(10)  not null, 
constraint fk_CardDetail_Customer foreign key (cus_id) references Customer(cus_id),
start_day datetime not null,
end_day datetime not null,
date_created datetime not null default current_timestamp
);
insert into Card_detail(card_id,cus_id,start_day,end_day)
values('CM01','123456789','2019-05-17','2019-06-17'),
('CM02','123456709','2019-05-17','2019-06-17'), 
('CM03','122332387','2019-05-17','2019-06-17'),
('CM04','123445785','2019-05-17','2019-06-17'),
('CM05','122446775','2019-05-17','2019-06-17'),
('CM06','123567436','2019-05-17','2019-06-17');
select * from Card_detail;
create table if not exists Accounts(
acc_name varchar(50) primary key,
acc_pass varchar(30) not null,
acc_fullname nvarchar(50) not null,
acc_email varchar(50) not null,
acc_level bit not null,
acc_dateCreated datetime not null default current_timestamp
);
insert into Accounts(acc_name,acc_pass, acc_fullname, acc_email,acc_level)
values('manager_01','24122000','Lê Chí Dũng','boydungbg@gmail.com',0),
('security_01','24122000','Bùi Việt Hoàng','hoangmage@gmail.com',1);
select * from Accounts;
create table if not exists Card_Logs(
cl_id int primary key auto_increment,
card_id varchar(10)  not null,
constraint fk_CardLogs_Card foreign key (card_id) references Card(card_id),
acc_name varchar(10)  not null,
constraint fk_CardLogs_Account foreign key (acc_name) references Accounts(acc_name),
cl_licensePlate varchar(20) not null,
cl_dateTimeStart datetime not null,
cl_dateTimeEnd datetime,
cl_sendTime varchar(20),
cl_intoMoney double
);
drop user if exists 'MPSUser'@'localhost';
create user if not exists 'MPSUser'@'localhost' identified by '123456';
    grant all on Card to 'MPSUser'@'localhost';
    grant all on Customer to 'MPSUser'@'localhost';
    grant all on Card_detail to 'MPSUser'@'localhost';
    grant all on Card_Logs to 'MPSUser'@'localhost';
	grant all on Accounts to 'MPSUser'@'localhost';
    grant lock tables on Group1_MotoParkingManagementSystem.* to 'MPSUser'@'localhost';


