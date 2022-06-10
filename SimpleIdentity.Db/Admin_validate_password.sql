CREATE PROCEDURE [dbo].[Admin_validate_password]
    @user_id int,
	@passw_text varchar(60),
	@ErrorMessage varchar(max) output
AS
-- =============================================
-- UT: exec [dbo].[Admin_validate_password] @user_id=1, @passw_text='Retro_Test1', @ErrorMessage= NULL
-- Description: Validate that entered password is correct
-- =============================================
BEGIN
DECLARE @count int = 0

	IF Len(@passw_text) < 8
	BEGIN
		SET @ErrorMessage = 'Password must be 8 or more characters';
		SELECT -1
		Return
	END

	IF NOT EXISTS (SELECT 1 WHERE @passw_text like '%[0-9]%' and @passw_text like '%[A-Z]%' and @passw_text like '%[!@#$%^&*()-_+=.,;:''"~]%' )
	BEGIN
		SET @ErrorMessage = 'Password must contain upper, lower case characters, numbers and special characters';
		SELECT -2
		Return
	END

	--- Check if 3 or more characters continuos replicated character exists. For example 111 or aaa
	;WITH CTE AS 
		(
			SELECT STUFF(@passw_text,1,1,'') TXT, LEFT(@passw_text,1) Col1
			UNION ALL
			SELECT STUFF(TXT,1,1,'') TXT, LEFT(TXT,1) Col1 FROM CTE
			WHERE LEN(TXT) > 0
		)
		SELECT @count = @count + 1 FROM CTE
		WHERE @passw_text like '%' + REPLICATE(Col1, 3) + '%'

	IF @count > 0
	BEGIN
		SET @ErrorMessage = 'Password may not contain 3 or more repeated characters';
		SELECT -4
		Return
	END

	--- Check if 3 or more characters continuos increment for example 123 or abc
	SET @count = 0
	;WITH CTE AS 
		(
			SELECT STUFF(@passw_text,1,1,'') TXT, LEFT(@passw_text,1) Col1
			UNION ALL
			SELECT STUFF(TXT,1,1,'') TXT, LEFT(TXT,1) Col1 FROM CTE
			WHERE LEN(TXT) > 0
		)
		SELECT @count = @count + 1 FROM CTE
		WHERE @passw_text like '%' + Col1 + CHAR(ASCII(Col1) + 1) + CHAR(ASCII(Col1) + 2) + '%'

	IF @count > 0
	BEGIN
		SET @ErrorMessage = 'Password may not contain 3 or more incremental characters. For example 123 or abc';
		SELECT -5
		Return
	END

	SELECT 0
END
