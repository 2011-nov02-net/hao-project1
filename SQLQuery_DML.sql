
-- insert products
insert into product(productid,name,category,price) values('p101','Dying Light','Game',1.0);
insert into product(productid,name,category,price) values('p102','Dying Light 2','Game',2.0);
insert into product(productid,name,category,price) values('p103','Lootbox one','Lootbox',3.0);
insert into product(productid,name,category,price) values('p104','Lootbox two','Lootbox',4.0);

--insert login credential
insert into credential(email,password) values ('JSmith@gmail.com', '52640JSmith');
insert into credential(email,password) values ('ASavage@gmail.com', '52640ASavage');
insert into credential(email,password) values ('KKong@gmail.com', '52640KKong');
insert into credential(email,password) values ('TCook@gmail.com', '52640TCook');

--insert admin login credential
insert into admincredential values('haoyang439@gmail.com','52640HYang');

-- insert customer
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus1','John','Smith','JSmith@gmail.com','6021231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus2','Adam','Savage','ASavage@gmail.com','4801231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus3','King','Kong','KKong@gmail.com','9291231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus4','Tim','Cook','TCook@gmail.com','7771231234');

-- insert store
insert into store(storeloc,storephone,zipcode) values('Techland Los Angeles 1', '2131111111','90012');
insert into store(storeloc,storephone,zipcode) values('Techland Portland 2', '5032222222','97207');
insert into store(storeloc,storephone,zipcode) values('Techland New York 3', '5183333333','10001');
insert into store(storeloc,storephone,zipcode) values('Techland Miami 4', '3054444444','33101');


-- insert order
-- recalculate total for total cost
-- total
insert into orderr(orderid,storeloc,customerid,totalcost) values('o001','Techland Los Angeles 1','cus1',1);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o002','Techland Portland 2','cus2',4);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o003','Techland New York 3','cus3',9);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o004','Techland Miami 4','cus4',16);

-- insert bridges store - product
insert into Inventory(storeloc,productid,quantity) values('Techland Los Angeles 1', 'p101',1000);
insert into Inventory(storeloc,productid,quantity) values('Techland Los Angeles 1', 'p102',1000);
insert into Inventory(storeloc,productid,quantity) values('Techland Los Angeles 1', 'p103',1000);
insert into Inventory(storeloc,productid,quantity) values('Techland Los Angeles 1', 'p104',1000);

-- insert brideges product - order
-- processid is auto generated
-- quantity
insert into orderproduct(orderid,productid,quantity) values('o001','p101',1);
insert into orderproduct(orderid,productid,quantity) values('o002','p102',2);
insert into orderproduct(orderid,productid,quantity) values('o003','p103',3);
insert into orderproduct(orderid,productid,quantity) values('o004','p104',4);

update Inventory set quantity = 999 where productid = 'p101';
update Inventory set quantity = 998 where productid = 'p102';
update Inventory set quantity = 997 where productid = 'p103';
update Inventory set quantity = 996 where productid = 'p104';

-- insert bridges store - customer
insert into storecustomer(storeloc,customerid) values('Techland Los Angeles 1','cus1');
insert into storecustomer(storeloc,customerid) values('Techland Portland 2','cus2');
insert into storecustomer(storeloc,customerid) values('Techland New York 3','cus3');
insert into storecustomer(storeloc,customerid) values('Techland Miami 4','cus4');

--
select* from store;
select* from customer;
select* from credential;
select* from admincredential;
select* from storecustomer;
select* from orderr;
select* from orderproduct;
SELECT* from inventory;
select* from product;

delete from credential where email = 'NGuy@gmail.com';
