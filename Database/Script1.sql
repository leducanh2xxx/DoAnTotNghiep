Use VatTuNongNghiepDB
Go

-- trigger for categories

CREATE TRIGGER trg_insert_category ON dbo.Categories
FOR INSERT
AS 
  UPDATE Categories
	SET Created_At = GETDATE(), Updated_At = GETDATE()
	from Categories inner join inserted i
	on Categories.CategoryId = i.CategoryId
go
create trigger trg_category_update on Categories
for update 
as
	begin
	  update Categories
		set Updated_At = getDate()
		from Categories inner join deleted d
		on Categories.CategoryId = d.CategoryId;
	end


-- trigger for users
go
CREATE TRIGGER trg_insert_user ON Users
FOR INSERT
AS 
  UPDATE Users
	SET Created_At = GETDATE(), Updated_At = GETDATE()
	from Users inner join inserted i
	on Users.UserId = i.UserId
go
create trigger trg_user_update on Users
for update 
as
	begin
	  update Users
		set Updated_At = getDate()
		from Users inner join deleted d
		on Users.UserId = d.UserId;
	end

		-- trigger for products
go
CREATE TRIGGER trg_insert_products ON Products
FOR INSERT
AS 
  UPDATE Products
	SET Created_At = GETDATE(), Updated_At = GETDATE()
	from Products inner join inserted i
	on Products.ProductId = i.ProductId
go
create trigger trg_product_update on Products
for update 
as
	begin
	  update Products
		set Updated_At = getDate()
		from Products inner join deleted d
		on Products.ProductId = d.ProductId;
	end

				-- trigger for orders
go
CREATE TRIGGER trg_insert_order ON Orders
FOR INSERT
AS 
  UPDATE Orders
	SET Created_At = GETDATE(), Updated_At = GETDATE()
	from Orders inner join inserted i
	on Orders.OrderId = i.OrderId
go
create trigger trg_order_update on Orders
for update 
as
	begin
	  update Orders
		set Updated_At = GETDATE()
		from Orders inner join deleted d
		on Orders.OrderId = d.OrderId;
	end

					-- trigger for orders_detail
go
CREATE TRIGGER trg_insert_orderd ON OrderDetails
FOR INSERT
AS 
  UPDATE OrderDetails
	SET Created_At = GETDATE(), Updated_At = GETDATE()
	from OrderDetails inner join inserted i
	on OrderDetails.OrderId = i.OrderId and OrderDetails.ProductId = i.ProductId
go
create trigger trg_orderd_update on OrderDetails
for update 
as
	begin
	  update OrderDetails
		set Updated_At = GETDATE()
		from OrderDetails inner join deleted d
		on OrderDetails.OrderId = d.OrderId and OrderDetails.ProductId = d.ProductId;
	end
GO