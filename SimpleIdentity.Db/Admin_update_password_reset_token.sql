CREATE PROCEDURE [dbo].[Admin_update_password_reset_token]
    @user_id int,
    @token varchar(200) = null
AS
-- =============================================
-- UT: exec Admin_save_password_reset_token @user_id=1, @token='PHjzvvMuQkGIUP4he43C7Et16vs5'
-- Description: Update current user password reset token
-- =============================================
BEGIN
	UPDATE app_user SET passw_reset_token=@token WHERE user_id=@user_id
END