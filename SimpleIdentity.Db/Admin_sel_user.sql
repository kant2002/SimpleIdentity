CREATE PROCEDURE [dbo].[Admin_sel_user]
	@user_id varchar(50)
AS
-- =============================================
-- UT:  exec Admin_sel_user user_id=1
-- Description:	Select user information with specific user_id
-- =============================================
BEGIN
	SELECT
		user_id,
		login_id,
		first_name,
		last_name,
		active_ind,
		email,
		role_type
	FROM app_user
	WHERE CONVERT(varchar, user_id)	= @user_id
END
