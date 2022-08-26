/*
 Pre-Deployment Script Template							
 ==================================================================================================================================
 Updates for WOW Development
 Please use the following format while updating this file

 *******************************************Instructions  Order of updates ********************************************************
--  Object narrative and Script Date, append devolopers name
--  WRITE the create objects Tables 
--  Initialize default values example 0,1,null or ' '(blank) or getdate().***
--  Write Insert into and Updates
--  Make sure the insert or update statement is for right customers.***
--  If new menu is added, make sure menu images are checked in.***
--  Write SPs 
--  Write Functions
--  DO NOT USE "USE AMICSBIZDB" and any other use statements
--  Write all scripts with if exists.
*/
go
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
if (select updatenum from appver) < 103
begin
	print 'Please run other updates and bring up to 103 before continuing.'+char(13)+'Update Cancelled'
	return
end

update appver set updatenum = 104, aspxnum= 104
 

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
