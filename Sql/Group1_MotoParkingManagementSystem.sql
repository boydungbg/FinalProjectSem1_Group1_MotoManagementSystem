drop database if exists Group1_MotoParkingManagementSystem;
create database if not exists Group1_MotoParkingManagementSystem char set 'utf8'; 
use Group1_MotoParkingManagementSystem;
create table if not exists Customer(
cus_id varchar(20) primary key,
cus_fullname nvarchar(50) not null,
cus_address nvarchar(70) not null,
license_plate varchar(20) not null
);
insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
values ('123456789','Đào Văn Đức','Thái Bình','75G1-2222'),
('123456709','Lê Chí Dũng','Bắc Giang','33G1-3333'),
('122332387','Nguyễn Văn A','Hà Nội','22E1-2222'),
('123445785','Dũng đẹp zai','Hà Nội','44S1-4422'),
('122446775','Bùi Việt Hoàng','Phú Thọ','88G1-8888'),
('123567436','Boydung','Bắc Giang','88A1-8888');
select * from Customer;
create table if not exists Card(
card_id int primary key auto_increment ,
card_type nvarchar(50) not null,
license_plate varchar(20) not null,
card_status tinyint not null default 0 ,
date_created datetime not null default current_timestamp
); 
ALTER TABLE Card AUTO_INCREMENT = 10000;
insert into Card(card_type,license_plate)
values('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate'),
('Thẻ ngày','No License Plate');
insert into Card(card_type,license_plate)
values('Thẻ tháng','75G1-2222'),
('Thẻ tháng','33G1-3333'),
('Thẻ tháng','22E1-2222'),
('Thẻ tháng','44S1-4422'),
('Thẻ tháng','88G1-8888'),
('Thẻ tháng','88A1-8888');
select * from Card;
create table if not exists Card_detail(
card_id int not null,
cus_id varchar(20)  not null,
constraint pk_carddetail primary key (card_id,cus_id),
constraint fk_CardDetail_Card foreign key (card_id) references Card(card_id),
constraint fk_CardDetail_Customer foreign key (cus_id) references Customer(cus_id),
start_day datetime not null,
end_day datetime not null
);
insert into Card_detail (card_id,cus_id,start_day,end_day)
values(10010,'123456789','2019-05-24','2019-06-24'),
(10011,'123456709','2019-05-24','2019-06-24'),
(10012,'122332387','2019-05-24','2019-06-24'),
(10013,'123445785','2019-05-24','2019-06-24'),
(10014,'122446775','2019-05-24','2019-06-24'),
(10015,'123567436','2019-04-24','2019-05-30');
select * from Card_detail;
create table if not exists Accounts(
acc_name varchar(50) primary key,
acc_pass varchar(30) not null,
acc_fullname nvarchar(50) not null,
acc_email varchar(50) not null,
acc_level tinyint not null,
acc_dateCreated datetime not null default current_timestamp
);
insert into Accounts(acc_name,acc_pass, acc_fullname, acc_email,acc_level)
values('manager_01','24122000','Lê Chí Dũng','boydungbg@gmail.com',0),
('security_01','24122000','Bùi Việt Hoàng','hoangmage@gmail.com',1);
select * from Accounts;
create table if not exists Card_Logs(
cl_id int primary key auto_increment,
card_id int  not null,
constraint fk_CardLogs_Card foreign key (card_id) references Card(card_id),
acc_name varchar(50)  not null,
constraint fk_CardLogs_Account foreign key (acc_name) references Accounts(acc_name),
cl_licensePlate varchar(20) not null,
cl_dateTimeStart datetime not null,
cl_dateTimeEnd datetime,
cl_sendTime varchar(50),
cl_intoMoney double
);
/*insert into Card_Logs(card_id,acc_name,cl_licensePlate,cl_dateTimeStart)
values('CD01','security_01','88-X8-9999',current_timestamp()),
('CM01','security_01','88-X8-1234',current_timestamp()),
('CM03','security_01','88-X8-5678',current_timestamp()),
('CD22','security_01','88-X8-9988',current_timestamp());*/
drop user if exists 'MPSUser'@'localhost';
create user if not exists 'MPSUser'@'localhost' identified by '123456';
    grant all on Card to 'MPSUser'@'localhost';
    grant all on Customer to 'MPSUser'@'localhost';
    grant all on Card_detail to 'MPSUser'@'localhost';
    grant all on Card_Logs to 'MPSUser'@'localhost';
	grant all on Accounts to 'MPSUser'@'localhost';
    grant lock tables on Group1_MotoParkingManagementSystem.* to 'MPSUser'@'localhost';
/*select c.card_id,c.card_type,c.license_plate,cus.cus_id,cus.cus_fullname,
cus.cus_address,cd.start_day,cd.end_day,max(cd.date_created),c.card_status from Card c
inner join Card_detail cd on c.card_id = cd.card_id
inner join Customer cus on  cd.cus_id = cus.cus_id
where c.card_id like 'CM%'
group by c.card_id;*/
/*select c.card_id,c.card_type,c.license_plate,cd.cus_id,cd.start_day,
            cd.end_day,cd.date_created,c.card_status from Card c
            inner join Card_detail cd on c.card_id = cd.card_id
            where c.card_id like 'CM%';*/
select *from Card_logs where cl_dateTimeStart LIKE'2019-05-26 00:00:00' AND'2019-06-26 00:00:00';
select * from Card_logs;


/*Update  Card_logs  SET cl_dateTimeEnd =current_timestamp()  , cl_sendTime ='asd',cl_intoMoney = 0
                        where card_id = 'CM01'  and  cl_licensePlate = '75-G1-2222' and cl_dateTimeStart = '2019-05-20 00:00:00';*/



