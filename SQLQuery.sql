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

  --Thông tin sơ đồ tổ chức
  create table departments
  (
		department_id int primary key identity(1,1),
		department_name nvarchar(50) unique,
		department_location nvarchar(200),
		create_at datetime default getdate(),
		create_by varchar(10),
  )
  GO
  create table departments_section
  (
		section_id int identity(1,1) primary key,
		section_code int,
		section_name nvarchar(50),
		section_location nvarchar(200),
		department_id int,
		create_at datetime default getdate(),
		create_by varchar(10),
		foreign key (department_id) references departments(department_id)
  )
  GO
  Create table positions
  (
	position_id int identity(1,1) primary key,
	position_name nvarchar(50) unique not null,
	update_at datetime,
	update_by varchar(10)
  )
  GO
  create table employees
  (
		employee_id int identity(1,1),
		employee_code varchar(10) primary key,
		employee_status int,
		full_name nvarchar(100),
		hire_date date,
		maternity_leave_date date,
        resignation_date date,
		department_id int,
		section_id int,
        position_id int,      
		update_at datetime,
		update_by varchar(10),
		foreign key (department_id) references departments(department_id),
		foreign key (section_id) references departments_section(section_id),
		foreign key (position_id) references positions(position_id)
  )