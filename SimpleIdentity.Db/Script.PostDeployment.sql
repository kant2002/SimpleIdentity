/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
set identity_insert App_user on
MERGE [dbo].App_user AS Target
USING (VALUES
    (1, 'admin', 'a'),
    (2, 'user1', 'u')) AS Source(user_id, login_id, role_type) ON (Target.user_id = Source.user_id)
WHEN NOT MATCHED BY TARGET
    THEN
        INSERT      (user_id, login_id, role_type, first_name, last_name, email, passw)
        VALUES      (user_id, login_id, role_type, 'John', 'Doe', concat(login_id, '@test.com'), HASHBYTES('SHA2_512', 'password'));

set identity_insert App_user off