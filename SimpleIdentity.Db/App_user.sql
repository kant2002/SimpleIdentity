CREATE TABLE [dbo].[App_user]
(
  user_id						int IDENTITY (1,1)	NOT NULL,
  login_id						varchar (128)		NOT NULL,
  first_name					varchar (60)		NULL,
  last_name						varchar (60)		NOT NULL,
  role_type						varchar (1)			NOT NULL,
  email							varchar (100)		NULL,
  title							varchar (100)		NULL,
  active_ind					int					NOT NULL default (1),
  phone_nbr						varchar (20)		NULL,
  passw							varbinary (100)		NOT NULL,
  last_mod_passw_date			date				NOT NULL default(getdate()),
  passw_reset_token				varchar (200)		NULL,
  force_password_reset_ind		bit					NOT NULL default(0))
