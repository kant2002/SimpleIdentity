CREATE PROCEDURE [dbo].[Admin_validate_user]
	@login_id   varchar(MAX),
	@passw_text varchar(MAX)
AS
-- =============================================
-- UT exec Admin_validate_user @login_id='user', @passw_text='password1'
--         Returns user_id or 0 if not found. Also returns role type
--         Role validation is performed within application
-- =============================================
BEGIN
	DECLARE @user_id int,
	        @role_type varchar(1),
			@accessed_data varchar(max)
            
	SELECT @user_id = user_id, @role_type=role_type
	FROM app_user
	WHERE active_ind = 1
	  AND login_id = @login_id
      AND passw = HASHBYTES('SHA2_512', @passw_text)
    
	SELECT ISNULL(@user_id, 0) AS user_id, @role_type AS role_type
END