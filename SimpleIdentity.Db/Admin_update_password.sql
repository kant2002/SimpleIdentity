CREATE PROCEDURE [dbo].[Admin_update_password]
          @user_id int,
		  @passw_text   varchar(max)
AS
-- =============================================
-- UT: exec Admin_update_password @user_id=1, @passw_text='Retro_Test1'
-- Description: Update current user password
-- =============================================
BEGIN
	UPDATE app_user SET passw=HASHBYTES('SHA2_512', @passw_text), last_mod_passw_date=GETDATE() 
	WHERE user_id=@user_id
END