CREATE PROCEDURE [dbo].[Admin_valdate_password_reset_token]
    @user_id int,
    @token   varchar(200)
AS
-- =============================================
-- UT: exec Admin_valdate_password_reset_token @user_id=1, @token='PHjzvvMuQkGIUP4he43C7Et16vs5'
-- Description: Validate password reset token for user
-- =============================================
BEGIN
DECLARE @stored_token varchar(200)
    SELECT @stored_token = passw_reset_token
    FROM app_user
    WHERE user_id = @user_id

    IF (@stored_token IS NULL)
    BEGIN
        SELECT CONVERT(BIT, 0)
        RETURN
    END

    IF (@stored_token = @token)
    BEGIN
        SELECT CONVERT(BIT, 1)
        RETURN
    END

    SELECT CONVERT(BIT, 0)
END
