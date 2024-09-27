  USE HRM
  Create table app_subject
  (
		subject_id int identity(1,1) primary key,
		subject_name varchar(50) unique,
		subject_description nvarchar(200),
		create_at datetime default getdate(), 
		create_by varchar(10)
  )
  GO
  create table app_menu
  (
		menu_id int identity(1,1) primary key,
		menu_name nvarchar(50),		
		menu_description nvarchar(200),
		menu_index int,
		subject_id int,		
		create_at datetime default getdate(),
		create_by varchar(10),
		foreign key(subject_id) references app_subject(subject_id)
  )
  GO

  create table app_menu_item
  (
		item_id int identity(1,1) primary key,		
		item_group nvarchar(50),
		item_name nvarchar(50),
		item_index int,
		item_controller nvarchar(200),
		item_icon nvarchar(200),
		item_header nvarchar(200),
		menu_id int,		
		create_at datetime default getdate(),
		create_by varchar(10),
		foreign key(menu_id) references app_menu(menu_id)
  )
  GO
  create table app_roles
  (
		role_id int identity(1,1) primary key,
		employee_code nvarchar(20),
		menu_id int,
		access bit default 0,		
		list_item_id varchar(200),
		create_at datetime default getdate(),
		create_by varchar(10),
		unique(employee_code,menu_id),
		foreign key (employee_code) references employees(employee_code),
		foreign key (menu_id) references app_menu(menu_id)
  )
  Go
