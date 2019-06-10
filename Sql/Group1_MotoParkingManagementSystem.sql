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
values
('123456789','Đào Văn Đức','Thái Bình','75G1-2222'),
('123456709','Lê Chí Dũng','Bắc Giang','33G1-3333'),
('122332387','Nguyễn Văn A','Hà Nội','22E1-2222'),
('123445785','Dũng đẹp zai','Hà Nội','44S1-4422'),
('122446775','Bùi Việt Hoàng','Phú Thọ','88G1-8888'),
('123567436','Boydung','Bắc Giang','88A1-8888');
select * from Customer;
CREATE TABLE IF NOT EXISTS Card (
    card_id INT PRIMARY KEY AUTO_INCREMENT,
    cus_id VARCHAR(20),
    CONSTRAINT fk_Card_Customer FOREIGN KEY (cus_id)
        REFERENCES Customer (cus_id),
    license_plate VARCHAR(20) NOT NULL,
    start_day DATETIME,
    end_day DATETIME,
    card_type NVARCHAR(50) NOT NULL,
    card_status TINYINT NOT NULL DEFAULT 0,
    date_created DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
); 
ALTER TABLE Card AUTO_INCREMENT = 10001;
insert into Card(card_type,license_plate)
values('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có'),
('Thẻ ngày','Không có');
insert into Card(card_type,license_plate,cus_id,start_day,end_day)
values('Thẻ tháng','75G1-2222','123456789','2019-05-24','2019-06-24'),
('Thẻ tháng','33G1-3333','123456709','2019-05-24','2019-06-24'),
('Thẻ tháng','22E1-2222','122332387','2019-05-24','2019-06-24'),
('Thẻ tháng','44S1-4422','123445785','2019-05-24','2019-06-24'),
('Thẻ tháng','88G1-8888','122446775','2019-05-24','2019-06-09'),
('Thẻ tháng','88A1-8888','123567436','2019-04-24','2019-05-30');
select * from Card limit 0,10;
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
acc_name varchar(50)  not null,
constraint fk_CardLogs_Card foreign key (card_id) references Card(card_id),
constraint fk_CardLogs_Account foreign key (acc_name) references Accounts(acc_name),
cl_licensePlate varchar(20) not null,
cl_timeIn datetime not null,
cl_timeOut datetime,
cl_money double not null default 0,
cl_status tinyint not null default 0
);
select cl.cl_id,cl.card_id,cl.cl_licensePlate,cl.cl_timeIn,cl.cl_timeOut,cl.cl_status from Card_logs cl
            inner join Card c on cl.card_id = c.card_id
            where cl.cl_timeIn between '2019-6-6' and '2019-6-11' and card_type = "Thẻ ngày" or cl.cl_status = 1 limit 0,10;
select * from Card_logs;
select count(cl.cl_id) as cl_id from Card_logs cl inner join Card c on cl.card_id = c.card_id
             where cl.cl_timeIn between '2019-6-6' and '2019-6-11' and c.card_type ='Thẻ ngày' or cl.cl_status = 1;
drop procedure sp_card_logs_statistical;
delimiter //
create procedure sp_card_logs_statistical(IN pageNo int,IN fromTime nvarchar(50),IN outTime nvarchar(50),IN TypeCard nvarchar(50))
begin 
select cl.cl_id,cl.card_id,cl.cl_licensePlate,cl.cl_timeIn,cl.cl_timeOut,cl.cl_status,cl.cl_money
 from Card_logs cl inner join Card c on cl.card_id = c.card_id
 where cl.cl_timeIn between fromTime and outTime and c.card_type = TypeCard
 limit pageNo,10;
end // 
delimiter ;
drop user if exists 'MPSUser'@'localhost';
create user if not exists 'MPSUser'@'localhost' identified by '123456';
    grant all on Card to 'MPSUser'@'localhost';
    grant all on Customer to 'MPSUser'@'localhost';
    grant all on Card_detail to 'MPSUser'@'localhost';
    grant all on Card_Logs to 'MPSUser'@'localhost';
	grant all on Accounts to 'MPSUser'@'localhost';
    grant all on PROCEDURE  sp_card_logs_statistical to 'MPSUser'@'localhost';
    grant lock tables on Group1_MotoParkingManagementSystem.* to 'MPSUser'@'localhost';
    




