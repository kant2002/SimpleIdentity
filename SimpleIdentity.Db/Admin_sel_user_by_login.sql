CREATE PROCEDURE [dbo].[Admin_sel_user_by_login]
	@login_id varchar(50)
AS
-- =============================================
-- UT:  exec Admin_sel_user_by_login login_id=1
-- Description:	Select user information with specific login_id
-- =============================================
BEGIN
	SELECT
		user_id,
		login_id,
		first_name,
		last_name,
		email,
		role_type
	FROM app_user
	WHERE login_id	= @login_id
END