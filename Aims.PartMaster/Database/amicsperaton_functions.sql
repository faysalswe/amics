
/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_encrypt]    Script Date: 13-07-2022 05:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_encrypt] 
(@pwd varchar(50)) 

RETURNS varchar(50)   

AS   
BEGIN   

 declare @encrypt varchar(50),@i smallint,@letter varchar (1)   

 set @pwd = @pwd + 'A' 

 set @encrypt=''   

 set @i=1   

 while @i < LEN(@pwd) + 1   

  begin   

   set @Letter = SUBSTRING(@pwd, @i, 1)   

   set @encrypt = @encrypt +CHAR((ASCII(@Letter)+1)*2)       

   set @i=@i+1   

  end   

 RETURN @encrypt   

END
GO

--------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_bomcost]    Script Date: 13-07-2022 05:02:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  FUNCTION [dbo].[amics_fn_api_bomcost] (@itemid uniqueidentifier)

RETURNS decimal(18,8)

AS

BEGIN

DECLARE @bomcost decimal (18,8)

set @bomcost=(select sum(x.bomcost)  from (SELECT        dbo.items_bom.quantity * list_items_1.cost / dbo.list_uoms.conversion AS bomcost

FROM            dbo.list_items AS list_items_1 LEFT OUTER JOIN

                         dbo.list_uoms ON list_items_1.uomid = dbo.list_uoms.id RIGHT OUTER JOIN

                         dbo.items_bom ON list_items_1.id = dbo.items_bom.itemsid_child

WHERE        (dbo.items_bom.itemsid_parent = @itemid )) as x )

 

return isnull(@bomcost,0)

END
GO
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_er]    Script Date: 13-07-2022 05:03:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_er]

(@invbasicid uniqueidentifier =null, @invserialid uniqueidentifier = null)

      

RETURNS varchar(50)

AS

BEGIN

       declare @solinesid uniqueidentifier

       if @invserialid is not null  -- serial item

              set @solinesid =isnull((select transferid from inv_serial where id=@invserialid),

                     (select so_linesid from inv_serial where id=@invserialid) )

       if @invbasicid is not null

              set @solinesid=(select so_linesid from inv_basic where id=@invbasicid)

 

       return (select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@solinesid)

 

END
GO

--select dbo.amics_fn_er(null,'7F241DFA-955B-47D9-B0FD-EDA1582D4077')

--------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_warehouseid]    Script Date: 13-07-2022 05:04:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_warehouseid]

 (@warehouse varchar(50),@location varchar(50))

       RETURNS uniqueidentifier

 AS

 BEGIN

       RETURN (select list_locations.id from list_locations inner join

              list_warehouses on list_locations.warehousesid=list_warehouses.id where

              list_locations.location=@location and list_warehouses.warehouse=@warehouse and

              list_locations.flag_delete<>1)

 END
 GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_inv_status]    Script Date: 13-07-2022 05:06:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[amics_fn_api_inv_status] (@citemsid uniqueidentifier,@cUserid uniqueidentifier) 
 RETURNS @invstatus table (pn varchar(50) not null,descr varchar(150) null,allocated decimal(18,8),avail decimal(18,8),notavail decimal(18,8),total decimal(18,8)) 
 AS 
 BEGIN 
 	declare @basicid uniqueidentifier, @serialid uniqueidentifier, @lotid uniqueidentifier, @invtypeid uniqueidentifier 
 	set @serialid=(select id from list_invtypes where invtype='serial') 
 	set @invtypeid=(select invtypeid from list_items where id=@citemsid) 
	if @citemsid='00000000-0000-0000-0000-000000000000' begin insert into @invstatus(pn,descr,allocated,avail,notavail,total) values   ('','',0,0,0,0)   return end 
 	insert into @invstatus(pn,descr) select itemnumber,description from list_items where id=@citemsid 
 	update @invstatus set allocated= (select isnull(SUM(ISNULL(dbo.inv_allocate.quantity, 0)),0) from inv_allocate where itemsid=@citemsid) 
 	if  @invtypeid = @serialid 
 		begin 
 		update @invstatus set total = (select isnull(sum(isnull(dbo.inv_serial.quantity,0)),0) from inv_serial 
 			inner join list_locations on inv_serial.locationsid=list_locations.id where itemsid=@citemsid  and 
 			inv_serial.quantity>0 and list_locations.warehousesid in (select warehouses_id from sec_users_warehouses where users_id=@cUserid)) 
 		update @invstatus set notavail = (select isnull(sum(isnull(dbo.inv_serial.quantity,0)),0) from inv_serial 
 			inner join list_locations on inv_serial.locationsid=list_locations.id where inv_serial.itemsid=@citemsid and 
 			inv_serial.quantity>0 and list_locations.invalid=1 and 
 			list_locations.warehousesid in (select warehouses_id from sec_users_warehouses where users_id=@cUserid)) 
 		end 
 	else 		 
 		begin 
 		--update @invstatus set total =(select isnull(sum(isnull(dbo.inv_basic.quantity,0)),0) from inv_basic 
 		--	inner join list_locations on inv_basic.locationsid=list_locations.id where itemsid=@citemsid  and 
 		--	inv_basic.quantity>0 and list_locations.warehousesid in (select warehouses_id from sec_users_warehouses where users_id=@cUserid)) 
		-- Change by Tony to get the correct invbasic totals   and do a left join.
		update @invstatus set total =(SELECT ISNULL(SUM(ISNULL(dbo.inv_basic.quantity, 0)), 0) 
				FROM dbo.inv_basic LEFT OUTER JOIN dbo.list_locations ON dbo.inv_basic.locationsid = dbo.list_locations.id
				WHERE dbo.inv_basic.itemsid = @citemsid and  inv_basic.quantity>0 AND dbo.list_locations.warehousesid IN
                             (SELECT warehouses_id
                               FROM  dbo.sec_users_warehouses
                               WHERE (users_id = @cUserid)))
 		update @invstatus set notavail =(select isnull(sum(isnull(dbo.inv_basic.quantity,0)),0) from inv_basic 
 			inner join list_locations on inv_basic.locationsid=list_locations.id where inv_basic.itemsid=@citemsid and 
 			inv_basic.quantity>0 and list_locations.invalid=1 and 
 			list_locations.warehousesid in (select warehouses_id from sec_users_warehouses where users_id=@cUserid)) 
 		end 
 	update @invstatus set avail=total-notavail-allocated 
 	RETURN 
 END
 GO
------------------------------------------------------------------------------------------------------------------------------------- 

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_view_location_summary]    Script Date: 13-07-2022 05:09:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[amics_fn_api_view_location_summary] (@citemsid uniqueidentifier,@cusersid uniqueidentifier)

RETURNS @viewloc_summary table (warehouse varchar(50),location varchar (50),somain varchar (50),name varchar(50),quantity smallint)

 

AS

BEGIN

       DECLARE @invtypeid uniqueidentifier

       set @invtypeid= (select invtypeid from list_items where id=@citemsid)

       if @invtypeid=(select id from list_invtypes where invtype='SERIAL')

       begin

       insert into  @viewloc_summary (warehouse,location,somain,name,quantity)

       (SELECT     dbo.list_warehouses.warehouse, dbo.list_locations.location, dbo.so_main.somain, dbo.list_projects.name, SUM(dbo.inv_serial.quantity)

                      AS quantity

FROM         dbo.list_projects INNER JOIN

                      dbo.so_main ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN

                      dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN

                      dbo.inv_serial ON dbo.so_lines.id = dbo.inv_serial.so_linesid LEFT OUTER JOIN

                      dbo.sec_users_warehouses LEFT OUTER JOIN

                      dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN

                      dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN

                      dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid ON dbo.inv_serial.locationsid = dbo.list_locations.id LEFT OUTER JOIN

                      dbo.list_items ON dbo.inv_serial.itemsid = dbo.list_items.id

GROUP BY dbo.list_locations.location, dbo.list_items.itemnumber, dbo.inv_serial.quantity, dbo.list_warehouses.warehouse, dbo.sec_users.id, dbo.list_items.id,

                      dbo.list_warehouses.warehouse, dbo.so_main.somain, dbo.list_projects.name,dbo.inv_serial.transferid

       HAVING      (dbo.list_items.id = @cItemsid) AND (dbo.inv_serial.quantity > 0) and (dbo.sec_users.id = @cusersid) and inv_serial.transferid is null)

 

       insert into  @viewloc_summary (warehouse,location,somain,name,quantity)

       (SELECT     dbo.list_warehouses.warehouse, dbo.list_locations.location, dbo.so_main.somain, dbo.list_projects.name, SUM(dbo.inv_serial.quantity)

                      AS quantity

FROM         dbo.list_projects INNER JOIN

                      dbo.so_main ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN

                      dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN

                      dbo.inv_serial ON dbo.so_lines.id = dbo.inv_serial.transferid LEFT OUTER JOIN

                      dbo.sec_users_warehouses LEFT OUTER JOIN

                      dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN

                      dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN

                      dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid ON dbo.inv_serial.locationsid = dbo.list_locations.id LEFT OUTER JOIN

                      dbo.list_items ON dbo.inv_serial.itemsid = dbo.list_items.id

GROUP BY dbo.list_locations.location, dbo.list_items.itemnumber, dbo.inv_serial.quantity, dbo.list_warehouses.warehouse, dbo.sec_users.id, dbo.list_items.id,

                      dbo.list_warehouses.warehouse, dbo.so_main.somain, dbo.list_projects.name,dbo.inv_serial.transferid

       HAVING      (dbo.list_items.id = @cItemsid) AND (dbo.inv_serial.quantity > 0) and (dbo.sec_users.id = @cusersid) and inv_serial.transferid is not null)

 

 

       end

if @invtypeid=(select id from list_invtypes where invtype='BASIC')

       insert into  @viewloc_summary (warehouse,location,somain,name,quantity)

       (SELECT     dbo.list_warehouses.warehouse, dbo.list_locations.location, dbo.so_main.somain, dbo.list_projects.name, SUM(dbo.inv_basic.quantity)

                      AS quantity

FROM         dbo.list_projects INNER JOIN

                      dbo.so_main ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN

                      dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN

                      dbo.inv_basic ON dbo.so_lines.id = dbo.inv_basic.so_linesid LEFT OUTER JOIN

                      dbo.sec_users_warehouses LEFT OUTER JOIN

                      dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN

                      dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN

                      dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid ON dbo.inv_basic.locationsid = dbo.list_locations.id LEFT OUTER JOIN

                      dbo.list_items ON dbo.inv_basic.itemsid = dbo.list_items.id

GROUP BY dbo.list_locations.location, dbo.list_items.itemnumber, dbo.inv_basic.quantity, dbo.list_warehouses.warehouse, dbo.sec_users.id, dbo.list_items.id,

                      dbo.list_warehouses.warehouse, dbo.so_main.somain, dbo.list_projects.name

       HAVING      (dbo.list_items.id = @cItemsid) AND (dbo.inv_basic.quantity > 0) and (dbo.sec_users.id = @cusersid))


return

END
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_view_location_summary_whs]    Script Date: 13-07-2022 05:10:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_view_location_summary_whs] (@citemsid uniqueidentifier,@cusersid uniqueidentifier, @warehouse varchar(50))  

RETURNS @viewloc_summary table (location varchar (50),somain varchar (50),name varchar(50),quantity smallint) 

AS 

BEGIN 

 DECLARE @invtypeid uniqueidentifier 

 set @invtypeid= (select invtypeid from list_items where id=@citemsid) 

 if @invtypeid=(select id from list_invtypes where invtype='SERIAL') 

 insert into  @viewloc_summary (location,somain,name,quantity)  

 (SELECT     dbo.list_locations.location, dbo.so_main.somain, dbo.list_projects.name, SUM(dbo.inv_serial.quantity)  

                      AS quantity 

FROM         dbo.list_projects INNER JOIN 

                      dbo.so_main ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN 

                      dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN 

                      dbo.inv_serial ON dbo.so_lines.id = dbo.inv_serial.so_linesid LEFT OUTER JOIN 

                      dbo.sec_users_warehouses LEFT OUTER JOIN 

                      dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN 

                      dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN 

                      dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid ON dbo.inv_serial.locationsid = dbo.list_locations.id LEFT OUTER JOIN 

                      dbo.list_items ON dbo.inv_serial.itemsid = dbo.list_items.id 

GROUP BY dbo.list_locations.location, dbo.list_items.itemnumber, dbo.inv_serial.quantity, dbo.list_warehouses.warehouse, dbo.sec_users.id, dbo.list_items.id,  

                      dbo.list_warehouses.warehouse, dbo.so_main.somain, dbo.list_projects.name 

 HAVING      (dbo.list_items.id = @cItemsid) AND (dbo.list_warehouses.warehouse = @warehouse) AND (dbo.inv_serial.quantity > 0) and (dbo.sec_users.id = @cusersid)) 

   

if @invtypeid=(select id from list_invtypes where invtype='BASIC') 

 insert into  @viewloc_summary (location,somain,name,quantity)  

 (SELECT  dbo.list_locations.location, dbo.so_main.somain, dbo.list_projects.name, SUM(dbo.inv_basic.quantity)  

                      AS quantity 

FROM         dbo.list_projects INNER JOIN 

                      dbo.so_main ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN 

                      dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN 

                      dbo.inv_basic ON dbo.so_lines.id = dbo.inv_basic.so_linesid LEFT OUTER JOIN 

                      dbo.sec_users_warehouses LEFT OUTER JOIN 

                      dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN 

                      dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN 

                      dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid ON dbo.inv_basic.locationsid = dbo.list_locations.id LEFT OUTER JOIN 

                      dbo.list_items ON dbo.inv_basic.itemsid = dbo.list_items.id 

GROUP BY dbo.list_locations.location, dbo.list_items.itemnumber, dbo.inv_basic.quantity, dbo.list_warehouses.warehouse, dbo.sec_users.id, dbo.list_items.id,  

                      dbo.list_warehouses.warehouse, dbo.so_main.somain, dbo.list_projects.name 

 HAVING      (dbo.list_items.id = @cItemsid) AND (dbo.list_warehouses.warehouse = @warehouse) AND (dbo.inv_basic.quantity > 0) and (dbo.sec_users.id = @cusersid)) 

   

return 

END
GO
-----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_view_location_detail]    Script Date: 13-07-2022 05:15:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_view_location_detail] 
(@citemsid uniqueidentifier,@cuserid uniqueidentifier) RETURNS TABLE

AS 
RETURN  

SELECT top 100 percent

ISNULL(dbo.po_main.pomain, '') AS pomain,

ISNULL(dbo.inv_serial.serno, '') AS serlot,

ISNULL(dbo.inv_serial.tagno, '') AS tagcol,

ISNULL(dbo.list_warehouses.warehouse, '') AS warehouse,

ISNULL(dbo.list_locations.location, '') AS location,

COUNT(dbo.inv_serial.id) AS quantity,

ISNULL(dbo.inv_serial.Cost, 0) AS cost, 

convert(varchar,dbo.inv_receipts.trans_date,101) AS expdate,

ISNULL(dbo.inv_serial.model,'') as color_model,

dbo.inv_serial.id,

'SERIAL' as invtype,

(select somain from so_lines inner join so_main on so_lines.somainid = so_main.id where so_lines.id=inv_serial.so_linesid) as actualso,

(select somain from so_lines inner join so_main on so_lines.somainid = so_main.id where so_lines.id=inv_serial.transferid) as currentso

FROM         dbo.sec_users_warehouses LEFT OUTER JOIN

  dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN

  dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id RIGHT OUTER JOIN

  dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid RIGHT OUTER JOIN

  dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid LEFT OUTER JOIN

  dbo.po_lines LEFT OUTER JOIN

  dbo.po_main ON dbo.po_lines.pomainid = dbo.po_main.id RIGHT OUTER JOIN

  dbo.inv_receipts ON dbo.po_lines.id = dbo.inv_receipts.sources_refid ON dbo.inv_serial.receiptsid = dbo.inv_receipts.id LEFT OUTER JOIN

                     

  dbo.so_lines ON  dbo.so_lines.id = CASE WHEN dbo.inv_serial.transferid IS NOT NULL THEN dbo.inv_serial.transferid ELSE dbo.inv_receipts.sources_refid END LEFT OUTER JOIN                     

  dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id LEFT OUTER JOIN

  --dbo.list_projects ON dbo.list_projects.id = dbo.so_main.projectid RIGHT OUTER JOIN                      

  dbo.list_items ON dbo.inv_serial.itemsid = dbo.list_items.id

                     

 GROUP BY ISNULL(dbo.po_main.pomain, ''),ISNULL(dbo.list_warehouses.warehouse, ''), ISNULL(dbo.list_locations.location, ''), ISNULL(dbo.inv_serial.cost, 0), dbo.inv_serial.quantity,

dbo.list_items.id,dbo.sec_users.id,dbo.inv_serial.serno,dbo.inv_serial.tagno,dbo.inv_serial.expiry,dbo.inv_serial.id,list_locations.invalid,dbo.inv_receipts.trans_date,

dbo.inv_serial.model,dbo.inv_serial.so_linesid,dbo.inv_serial.transferid

HAVING      (dbo.inv_serial.quantity > 0) and (dbo.list_items.id=@citemsid) and (dbo.sec_users.id=@cuserid) and list_locations.invalid=0

order by trans_date,serno 

END
GO

--UNION

-- SELECT  top 100 percent  

-- ISNULL(dbo.po_main.pomain, '') AS pomain,

-- ISNULL(dbo.inv_basic.lotno, '') as serlot ,

-- ISNULL(dbo.inv_basic.color, '') as tagcol ,

-- ISNULL(dbo.list_warehouses.warehouse, '') AS warehouse,

-- ISNULL(dbo.list_locations.location, '') AS location,

-- ISNULL(dbo.inv_basic.quantity, 0) AS quantity,

-- ISNULL(dbo.inv_basic.cost, 0) AS cost,

-- ISNULL(convert(varchar,dbo.inv_basic.expdate,101), '2075-01-01') as expdate,

-- dbo.inv_basic.color as color_model,

-- dbo.inv_basic.id,

-- '' as invtype,

-- '' as actualso,

-- '' as currentso 

-- FROM dbo.po_lines LEFT OUTER JOIN

-- dbo.po_main ON dbo.po_lines.pomainid = dbo.po_main.id RIGHT OUTER JOIN

-- dbo.list_items LEFT OUTER JOIN

-- dbo.inv_basic LEFT OUTER JOIN

-- dbo.inv_receipts ON dbo.inv_basic.sources_refid = dbo.inv_receipts.sources_refid LEFT OUTER JOIN

-- dbo.list_locations ON dbo.inv_basic.locationsid = dbo.list_locations.id LEFT OUTER JOIN

-- dbo.sec_users_warehouses LEFT OUTER JOIN

-- dbo.sec_users ON dbo.sec_users_warehouses.users_id = dbo.sec_users.id RIGHT OUTER JOIN

-- dbo.list_warehouses ON dbo.sec_users_warehouses.warehouses_id = dbo.list_warehouses.id ON

-- dbo.list_locations.warehousesid = dbo.list_warehouses.id ON dbo.list_items.id = dbo.inv_basic.itemsid ON

-- dbo.po_lines.id = dbo.inv_receipts.sources_refid

--  where sec_users_warehouses.users_id=@cuserid and inv_basic.itemsid=@citemsid and inv_basic.quantity>0  and list_locations.invalid=0

--order by lotno

 
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_translog_view]    Script Date: 14-07-2022 10:55:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_translog_view] (@date1 smalldatetime='',@date2 smalldatetime='',@where varchar(50))    
RETURNS @translog_view table (invtransid uniqueidentifier,itemnumber varchar(50),rev varchar(50),Description varchar(150),    
 source varchar(50),ref varchar(50), quantity decimal(18,8),transdate varchar(10),  createddate smalldatetime,  
 createdby varchar(50), serno varchar(50),tagno varchar(50),lotno varchar(50),    
 location varchar(50),warehouse varchar(50),cost money,notes varchar(max))    
    
AS    
BEGIN    
 declare @fromdate smalldatetime,@todate smalldatetime    
 if @date1='' set @fromdate=getdate()-30 else set @fromdate=@date1    
 if @date2='' set @todate=getdate() else set @todate=@date2    
    
insert into @translog_view  (invtransid, itemnumber ,rev ,Description ,    
 source ,ref ,quantity, transdate,createddate ,createdby,serno,tagno,lotno,location ,warehouse ,cost ,notes )    
 SELECT  TOP 1000      
   dbo.translog.id, 
   ISNULL(dbo.translog.itemnumber, '') AS itemnumber,     
   isnull(dbo.translog.revision,'') as rev,     
   ISNULL(dbo.translog.description, '') AS description,     
   ISNULL(dbo.translog.source, '') AS source,     
   ISNULL(dbo.translog.ref,'') AS ref,     
  case dbo.translog.invtype when 'SERIAL' then  1 else isnull(dbo.transloglot.quantity,dbo.translog.transqty) end as Quantity,    
   ISNULL(CONVERT(VARCHAR(10), dbo.translog.transdate, 101), '') as transdate,    
   dbo.translog.createddate as createddate,   
  ISNULL(dbo.translog.createdby, '') AS createdby,     
 case     
    dbo.translogsn.serno     
    when '' then dbo.translogsn.serno     
    else ISNULL('' +dbo.translogsn.serno, '')     
   end AS serno,     
 case     
    dbo.translogsn.tagno     
    when '' then dbo.translogsn.tagno     
    else ISNULL('' +dbo.translogsn.tagno, '')     
   end AS tagno,     
 case     
    dbo.transloglot.lotno     
    when '' then dbo.transloglot.lotno     
    else ISNULL('' +dbo.transloglot.lotno, '')     
   end AS lotno,    
   ISNULL(dbo.translog.location, '') AS location,     
  ISNULL(dbo.translog.warehouse, '') AS warehouse,     
   ISNULL(dbo.translog.cost, 0) AS cost,     
   isnull(dbo.translog.notes,'') as notes    
       
 FROM    dbo.transloglot RIGHT OUTER JOIN     
 dbo.translog ON dbo.transloglot.translogid = dbo.translog.id LEFT OUTER JOIN     
 dbo.list_itemtypes RIGHT OUTER JOIN     
 dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid ON dbo.translog.itemsid = dbo.list_items.id LEFT OUTER JOIN     
 dbo.translogsn ON dbo.translog.id = dbo.translogsn.translogid     
 where trans like @where+'%' and dbo.translog.transdate between @fromdate and @todate    
 order by createddate desc     
 RETURN     
END    
GO
--select * from [dbo].[fn_translog_view]  ('','','')  ('07/01/09','08/06/09') where lotqty<>transqty and transqty+lotqty<>0 and lotqty<>0

-------------------------------------------------------------------------------------------------------------------------------------
 

/****** Object:  UserDefinedFunction [dbo].[amics_fn_api_decrypt]    Script Date: 26-07-2022 04:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[amics_fn_api_decrypt]  
(@pwd varchar(50))  
   
RETURNS varchar(50)  
AS  
BEGIN  
 declare @decrypted varchar(50),@i smallint,@letter varchar (1)  
 set @decrypted=''  
 set @i=1  
 while @i < LEN(@pwd)  
  begin  
   set @Letter = SUBSTRING(@pwd, @i, 1)  
   set @decrypted = @decrypted +CHAR((ASCII(@Letter)-1)/2)      
   set @i=@i+1  
  end  
 RETURN @decrypted  
END
GO

------------------------------------------------------------------------------------------------------------------------------------- 

 
GO
CREATE FUNCTION [dbo].[amics_fn_api_invtype]
(@item varchar(50),@rev varchar(50))   
RETURNS varchar(50)
AS
BEGIN
       RETURN (select invtype from list_items inner join list_invtypes on
              list_items.invtypeid=list_invtypes.id where
			  list_items.itemnumber=@item and list_items.rev=@rev)

END

GO
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE FUNCTION [dbo].[amics_fn_api_itemtype]
(@item varchar(50),@rev varchar(50))     
RETURNS varchar(50)
AS
BEGIN

       RETURN (select itemtype from list_items inner join list_itemtypes on

              list_items.itemtypeid=list_itemtypes.id where

              list_items.itemnumber=@item and list_items.rev=@rev)

END  
GO
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  UserDefinedFunction [dbo].[fn_tax_decimal_soship]    Script Date: 8/1/2022 11:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE function [dbo].[amics_fn_api_tax_decimal_soship](@soshipid uniqueidentifier)
returns  decimal(4,4)
AS
BEGIN
declare @taxdec decimal(4,4)
---   get the taxpercent in decimals for this invoice to multiply with subtotal
Return (SELECT  top 1   dbo.list_taxcodes.taxpercent / 100

FROM         dbo.list_taxcodes RIGHT OUTER JOIN

  dbo.list_customers_shipto ON dbo.list_taxcodes.id = dbo.list_customers_shipto.taxcodesid RIGHT OUTER JOIN

  dbo.so_main ON dbo.list_customers_shipto.customersid = dbo.so_main.customersid RIGHT OUTER JOIN

  dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid RIGHT OUTER JOIN

  dbo.inv_pick ON dbo.so_lines.id = dbo.inv_pick.sources_refid

WHERE     (dbo.inv_pick.inv_soshipid = @soshipid) AND (dbo.so_main.customersid = dbo.list_customers_shipto.customersid)

                      AND (dbo.so_main.shiptoaddress5 = dbo.list_customers_shipto.address5))

 

END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 

