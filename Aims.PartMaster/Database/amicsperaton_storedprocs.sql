
/****** Object:  StoredProcedure [dbo].[amics_sp_api_search_items]    Script Date: 12-07-2022 07:48:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_search_items]

@item varchar(50)='',

@rev varchar(50)='',

@description varchar(75)='',

@itemtype varchar(50) ='',

@itemclass varchar(50) ='',

@itemcode varchar(50) = '',

@warehouse varchar(50) = '',

@location varchar(50) = '',

@user1 varchar(50) = '',

@user2 varchar(50) = '',

@user3 varchar(50) = '',

@user4 varchar(50) = '',

@user5 varchar(50) = '',

@user6 varchar(50) = '',

@user7 varchar(50) = '',

@user8 varchar(50) = ''

AS

set NOCOUNT ON

BEGIN

declare @itemnumber varchar(50), @desc varchar(75),@type varchar(50), @class varchar(50), @code varchar(50),@vuser1 varchar(50),@vuser2 varchar(50)
,@vuser3 varchar(50),@vuser4 varchar(50),@vuser5 varchar(50),@vuser6 varchar(50),@vuser7 varchar(50),@vuser8 varchar(50)

set @itemnumber='%'+@item+'%'

set @desc='%'+@description+'%'

set @type='%'+@itemtype+'%'

set @class='%'+@itemclass+'%'

set @code='%'+@itemcode+'%'

set @vuser1 ='%'+@user1+'%'
set @vuser2 ='%'+@user2+'%'
set @vuser3 ='%'+@user3+'%'
set @vuser4 ='%'+@user4+'%'
set @vuser5 ='%'+@user5+'%'
set @vuser6 ='%'+@user6+'%'
set @vuser7 ='%'+@user7+'%'
set @vuser8 ='%'+@user8+'%'

select list_items.id, LTRIM(RTRIM(itemnumber)) as itemnumber,LTRIM(RTRIM(description)) as description,LTRIM(RTRIM(rev))

		as rev,isnull(itemtype ,' ') as itemtype , isnull(itemclass,' ') as itemclass,

		isnull(itemcode, ' ') as itemcode, isnull(list_uoms.uomref,'') as uomref,list_items.cost,list_items.dwgno,list_uoms.conversion from list_items

		left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id

		left join list_itemclass on list_items.itemclassid=list_itemclass.id

		left join list_itemcodes on list_items.itemcodeid=list_itemcodes.id

		left join list_uoms on list_items.uomid=list_uoms.id

		where itemnumber like @itemnumber and

		description like @desc and

		isnull(itemtype,'') like @type and

		isnull(itemclass,'') like @class and

		isnull(itemcode,'') like @code and

		user1 like @vuser1 and

		user2 like @vuser2 and

		user3 like @vuser3 and

		user4 like @vuser4 and

		user5 like @vuser5 and

		user6 like @vuser6 and

		user7 like @vuser7 and

		user8 like @vuser8 

		and flag_delete != '1'
		order by list_items.createddate desc
 
END
GO
------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_search_project_er]    Script Date: 12-07-2022 07:53:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_search_project_er]     
 @projectid varchar (50)='',     
 @projectname varchar (50)='',    
 @er varchar (50)='',     
 @budgetauthority varchar (50)='',
 @user2 varchar(50) =''      
 AS     
begin 
set @user2=rtrim(@user2)  
if @er != '' or @user2 != ''
begin
select distinct list_projects.project,list_projects.name,so_main.somain 
FROM dbo.so_main left join list_projects on so_main.projectid=list_projects.id
--inner join list_projects on list_projects.id = so_main.projectid    
where 
--list_projects.project like @projectid + '%' and list_projects.name like @projectname + '%' and 
--so_main.somain  like @er + '%' and so_main.user4 like @budgetauthority + '%' and so_main.user2 like '%'+@user2 + '%' order by somain desc
somain LIKE	case  when @er ='' then '%%' else '%'+@er+'%' end
and project like case  when @projectid ='' then '%%' else '%'+@projectid+'%' end    
and name like case  when @projectname ='' then '%%' else '%'+@projectname+'%' end  
and so_main.user4 Like case  when  @budgetauthority='' then '%%' else '%'+@budgetauthority+'%' end
and so_main.user2 Like case  when  @user2='' then '%%' else '%'+@user2+'%' end
order by  somain desc
end
else
begin
select distinct list_projects.project,list_projects.name,'' as somain from so_main inner join list_projects on list_projects.id = so_main.projectid    
where list_projects.project like '%' + @projectid + '%' and list_projects.name like '%'+ @projectname + '%' and so_main.user4 like '%' + @budgetauthority + '%'  
order by somain desc
end 
end   
Go
-----------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_delete_list_items]    Script Date: 12-07-2022 07:56:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_delete_list_items] @item varchar(50),@rev varchar(50) 
 AS 
 	declare @itemid uniqueidentifier,@found varchar(50) 
 BEGIN 
 	declare @tbl_exists table (message varchar(50)) 
 	set @itemid=(select id from list_items where itemnumber=@item and rev=@rev) 
 	if (select top 1 id from inv_serial where itemsid=@itemid and quantity>0) is not null insert into @tbl_exists values ('Serial Inventory Exists') 
 	if (select top 1 id from inv_basic where itemsid=@itemid and quantity>0) is not null insert into @tbl_exists values ('Basic / Lot  Inventory Exists') 
 	--if (select top 1 id from inv_wip where itemsid=@itemid and quantity>0) is not null insert into @tbl_exists values ('WIP  Inventory Exists') 
 	if (select top 1 id from items_bom where itemsid_parent=@itemid) is not null insert into @tbl_exists values ('BOM Exists') 
 	if (select top 1 id from items_bom where itemsid_child=@itemid) is not null insert into @tbl_exists values ('Used in BOM') 
 	
 	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[list_suppliers_items]') AND type in (N'U'))
 	begin
 	if (select top 1 id from list_suppliers_items where itemsid=@itemid) is not null insert into @tbl_exists values ('Supplier Items Exist') 
 	end 
 	
 	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[items_routings]') AND type in (N'U'))
 	begin
 	if (select top 1 id from items_routings where itemsid=@itemid) is not null insert into @tbl_exists values ('Routings Exist') 
 	end
 	
 	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[list_documents]') AND type in (N'U'))
 	begin
 	if (select top 1 id from list_documents where parentid=@itemid) is not null insert into @tbl_exists values ('Documents Attached') 
 	end
 	
 	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[so_lines]') AND type in (N'U'))
 	begin
 	if (select top 1 so_lines.id from so_lines inner join so_main on so_lines.somainid=so_main.id inner join list_status on so_main.statusid=list_status.id 
 		where itemsid=@itemid and status='OPEN') is not null insert into @tbl_exists values ('Used in Sales order Line item (Open)') 
    end
 		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[po_lines]') AND type in (N'U'))
 	begin
 	if (select top 1 po_lines.id from po_lines inner join po_main on po_lines.pomainid=po_main.id  inner join list_status on po_main.statusid=list_status.id 
 		where itemsid=@itemid and status='OPEN') is not null insert into @tbl_exists values ('Used in PO Line item (Open)') 
    end
 		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wo_bom]') and object_id = OBJECT_ID(N'[dbo].[wo_main]') AND type in (N'U'))
 	begin
 	if (select top 1 wo_bom.id from wo_bom inner join wo_main on wo_bom.wo_mainid=wo_main.id  inner join list_status on wo_main.statusid=list_status.id 
 		where itemsid=@itemid and wo_main.status='OPEN') is not null insert into @tbl_exists values ('Used in Work order Line item (Open)') 
    end
 		
 		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[inv_allocate]') AND type in (N'U'))
 	begin
 	if (select top 1 id from inv_allocate where itemsid=@itemid) is not null insert into @tbl_exists values ('Inventory Allocated') 
 	end
 	
 	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[list_status]') AND type in (N'U')and object_id = OBJECT_ID(N'[dbo].[wo_main]') AND type in (N'U'))
 	begin
 	if (SELECT top 1 dbo.wo_main.id FROM dbo.list_status RIGHT OUTER JOIN dbo.wo_main ON dbo.list_status.id = dbo.wo_main.statusid where wo_main.itemid=@itemid AND dbo.list_status.status = 'open') is not null insert into @tbl_exists values ('Work Order (Open)') 
	end
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rma_lines]') AND type in (N'U') and object_id = OBJECT_ID(N'[dbo].[rma_main]') AND type in (N'U') and object_id = OBJECT_ID(N'[dbo].[list_status]') AND type in (N'U'))
 	begin
	if (select top 1 rma_lines.id from rma_lines inner join rma_main on rma_lines.rmamainid=rma_main.id 
 		inner join list_status on rma_main.statusid=list_status.id 
 		where rma_lines.itemsid=@itemid and list_status.status='OPEN') is not null insert into @tbl_exists values ('RMA (Open)') 
    end
 		
 	if (select count(*) from @tbl_exists)=0 
 		update list_items set flag_delete=1 where id=@itemid 
 	select * from @tbl_exists	 
 END 
 GO
 --select  top 1 * from rma_lines

----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_transfer]    Script Date: 12-07-2022 07:58:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_chgloc_transfer]      
	@xfr_createdby varchar(50), 
	@toloc varchar(50), 
	@towh varchar(50), 
	@xfr_transnum int,
	@counter_solines smallint =1,
	@total_solines smallint = 0
AS
BEGIN
       declare   @tolocid uniqueidentifier,
         @loc varchar(50),@serialid uniqueidentifier,@basicid uniqueidentifier,@quantity decimal(18,8),
         @total_solines2 smallint,@total_items smallint,@counter_items smallint,
         @xfr_id uniqueidentifier,@xfr_solinesid uniqueidentifier, @xfr_invbasicid uniqueidentifier, @xfr_invbasicid2 uniqueidentifier, @xfr_invserialid uniqueidentifier, @xfr_quantity decimal(18,8),
         @somain varchar(50),@itemsid uniqueidentifier,@translogid uniqueidentifier,@item varchar(50),@descr varchar(150),@itemtype varchar(50),@invtype varchar(50),
         @fromlocid uniqueidentifier,@fromloc varchar(50),@fromwh varchar(50),@cost decimal(18,8),@serno varchar(50),@tagno varchar(50),@line smallint
    
       set @tolocid=(select top 1 list_locations.id from list_locations inner join list_warehouses on list_locations.warehousesid=list_warehouses.id
                                         where location=@toloc and warehouse=@towh order by list_locations.createddate desc )
 
       if @tolocid is not null
       begin
            --update list_next_number set translognum=translognum+1
            --set @xfr_transnum=(select translognum from list_next_number)
                     update inv_transfer_location set transnum=@xfr_transnum where createdby=@xfr_createdby
                     --set @counter_solines=1
                     set @total_solines2=(select count(*)  from dbo.inv_transfer_location where createdby=@xfr_createdby)

					if @total_solines > @total_solines2 
							set @total_solines = @total_solines2

                     while @counter_solines<=@total_solines
                     begin         -- Process one line at a time
                           set @xfr_id=(select id from (select row_number() over (order by [createddate]) as rowno ,id from dbo.inv_transfer_location where createdby=@xfr_createdby) as x where rowno=@counter_solines)
                           set @xfr_solinesid=(select solinesid from (select row_number() over (order by [createddate]) as rowno ,solinesid from dbo.inv_transfer_location where createdby=@xfr_createdby) as x where rowno=@counter_solines)
                           set @xfr_invbasicid=(select invbasicid from (select row_number() over (order by [createddate]) as rowno ,invbasicid from dbo.inv_transfer_location where createdby=@xfr_createdby) as x where rowno=@counter_solines)
                           set @xfr_invserialid=(select invserialid from (select row_number() over (order by [createddate]) as rowno ,invserialid from dbo.inv_transfer_location where createdby=@xfr_createdby) as x where rowno=@counter_solines)
                           set @xfr_quantity=(select quantity from (select row_number() over (order by [createddate]) as rowno ,quantity from dbo.inv_transfer_location where createdby=@xfr_createdby) as x where rowno=@counter_solines)
                           set @somain=(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@xfr_solinesid)
                           set @itemsid=(select itemsid from so_lines where id=@xfr_solinesid)
                           set @item=(select itemnumber from so_lines where id=@xfr_solinesid)
                           set @descr=(select description from so_lines where id=@xfr_solinesid)
                           set @line=(select linenum from so_lines where id=@xfr_solinesid)
                           set @invtype=isnull((select invtype from list_items left join list_invtypes on list_items.invtypeid=list_invtypes.id where list_items.id=@itemsid),'')
                           set @itemtype=isnull((select itemtype from list_items left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id where list_items.id=@itemsid),'')
                           if @xfr_invserialid is not null -- Serial Items Start
                           begin
                           print @xfr_invserialid
                                  set @fromlocid=(select locationsid from inv_serial where inv_serial.id=@xfr_invserialid)
                                  set @cost=(select cost from inv_serial where inv_serial.id=@xfr_invserialid)
                                  set @serno=(select serno from inv_serial where inv_serial.id=@xfr_invserialid)
                                  set @tagno=(select tagno from inv_serial where inv_serial.id=@xfr_invserialid)
                                  set @fromloc=(select location from inv_serial inner join list_locations on inv_serial.locationsid=list_locations.id  where inv_serial.id=@xfr_invserialid)
                                  print @fromloc
                                  set @fromwh=(select warehouse from list_locations inner join list_warehouses on list_locations.warehousesid=list_warehouses.id where list_locations.id=@fromlocid)
                                  set @translogid=(select id from translog where transnum=@xfr_transnum and user2=@somain and itemsid=@itemsid and location=@fromloc)
                                  -- Start insert and update
                                   update inv_serial set locationsid=@tolocid where id=@xfr_invserialid
                                  if @translogid is null
                                         begin                   
                                              set @translogid=newid()
                                              insert into translog (id,itemsid,trans,transnum,transdate,transqty,itemnumber,description,itemtype,warehouse,location,reason,                                 source,ref,invtype,notes,createdby,revision,createddate,modifieddate,location2,user2,cost,user1) values
                                                              (@translogid,@itemsid,'CHG LOC',@xfr_transnum,getdate(),1,@item,@descr,@itemtype,@fromwh,@fromloc,
                                                              'CHG LOC','','','SERIAL','',@xfr_createdby,'',getdate(),getdate(),@toloc,@somain,@cost,@line)
                                         end
                                  else update translog set transqty=transqty+1,location=@fromloc where id=@translogid
                                  insert into translogsn (translogid,serno,tagno,createdby,location,warehouse,cost) values (@translogid,@serno,@tagno,@xfr_createdby,@fromloc,@fromwh,@cost)
                                  -- End insert and update 
                           end  -- Serial Items End
                           if @xfr_invbasicid is not null -- Basic Items Start
                           begin
                                  set @fromlocid=(select locationsid from inv_basic where inv_basic.id=@xfr_invbasicid)
                                  set @fromloc=(select location from list_locations where id=@fromlocid)
                                  set @fromwh=(select warehouse from list_locations inner join list_warehouses on list_locations.warehousesid=list_warehouses.id where list_locations.id=@fromlocid)
                                  UPDATE inv_basic SET inv_basic.quantity = inv_basic.quantity-@xfr_quantity WHERE id = @xfr_invbasicid
                                  set @cost=(select cost from inv_basic where inv_basic.id=@xfr_invbasicid)
                                  set @xfr_invbasicid2=(select id from inv_basic where so_linesid =@xfr_solinesid and locationsid=@tolocid)
                                  ------------
                                  if @xfr_invbasicid2 is null
                                         insert into inv_basic (id,locationsid,quantity,so_linesid,itemsid,cost,sources_refid,createdby ) values
                                         (newid(),@tolocid,@xfr_quantity,@xfr_solinesid,@itemsid,@cost,@xfr_solinesid, @xfr_createdby)
                                  else
                                         UPDATE inv_basic SET inv_basic.quantity = inv_basic.quantity+@xfr_quantity where id=@xfr_invbasicid2
                                  ------------
                                  insert into translog (id,itemsid,trans,transnum,transdate,transqty,itemnumber,description,itemtype,warehouse,location,reason,
                                         source,ref,invtype,notes,createdby,revision,createddate,modifieddate,location2,user2,cost,user1) values
                                         (newid(),@itemsid,'CHG LOC',@xfr_transnum,getdate(),@xfr_quantity,@item,@descr,@itemtype,@fromwh,@fromloc,
                                         'CHG LOC','','','BASIC','',@xfr_createdby,'',getdate(),getdate(),@toloc,@somain,@cost,@line)
                           end                                                           -- Basic Items End
                           
						   update inv_transfer_location set addflag = @counter_solines where id= @xfr_id
						   set @counter_solines=@counter_solines+1


                     end
 
              end
End
Go
----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_receipts]    Script Date: 12-07-2022 08:01:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_receipts]        
 @rec_source varchar(50),        
 @rec_sourcesrefid uniqueidentifier,        
 @rec_extedid uniqueidentifier=null,        
 @rec_warehouse varchar(50),        
 @rec_location varchar(50),        
 @rec_item varchar(50),        
 @rec_rev varchar(50),        
 @rec_cost decimal(18,8)='0.0',        
 @rec_quantity decimal(18,8),        
 @rec_misc_reason varchar(50)='',        
 @rec_misc_ref varchar(50)='',        
 @rec_misc_source varchar(50)='',        
 @rec_notes varchar(max)='',        
 @rec_transdate smalldatetime,        
 @rec_user varchar(50),        
 @rec_transnum int,        
 @potype varchar(50)='',        
 @rec_account varchar(50)='',        
 @rec_packlist varchar(50)='',        
 @lic_plate_flag bit=null,        
 @rec_receiver smallint =0,
 @user2 varchar(50)=''     
        
 AS        
BEGIN        
 declare         
 @rec_itemid uniqueidentifier,        
 @rec_invbasicid uniqueidentifier,        
 @serialid  uniqueidentifier,        
 @rec_locationid uniqueidentifier,        
 @translogid uniqueidentifier,        
 @invmiscid uniqueidentifier,        
 @rec_sourcesid uniqueidentifier,        
 @rec_receiptsid uniqueidentifier,        
 @reasonid  uniqueidentifier,        
 @rec_invtypeid uniqueidentifier,        
 @rec_miscid uniqueidentifier,        
 @rec_serlotid uniqueidentifier,        
 @rec_translogid uniqueidentifier,        
 @basid uniqueidentifier,        
 @serid uniqueidentifier,        
 @lotid uniqueidentifier,        
 @noninvid uniqueidentifier,        
 @rec_description varchar(75),        
 @rec_itemtype varchar(50),        
 @rec_invtype varchar(50),        
 @rec_lot varchar(50),        
 @rec_color varchar(50),        
 @rec_quantity2 decimal(18,8),        
 @rec_batqty decimal(18,8),        
 @rec_lotqty decimal(18,8),        
 @rec_exp smalldatetime,        
 @routings_polinesid uniqueidentifier,         
 @wo_routingsid  uniqueidentifier,        
 @womain varchar(50),         
 @routings_po bit,        
 @conversion decimal (18,8),        
 @pur_uomid uniqueidentifier,        
 @stock_uomid uniqueidentifier,        
 @lic_plt varchar(50),        
 @dis_color varchar(50),        
 @newplate varchar(50),        
 @run_plate bit,        
 @rec_lic_plt varchar(50),        
 @soshipid uniqueidentifier,        
 @lic_plate VARCHAR(50),        
 @rec_sourcesrefid2 uniqueidentifier,      
 @rec_sourcesrefid_serial uniqueidentifier,      
 @solinesid uniqueidentifier      
         
  create table #mytable (color varchar(50), lic_plt varchar(50) )         
         
 if isnull(@rec_account,'')<>'' set @rec_notes='Charge Account: '+@rec_account+@rec_notes        
 set @routings_po=0        
        
 set @rec_invbasicid=(select newid())        
 set @invmiscid=(select newid())        
 set @rec_translogid=(select newid())        
 set @rec_receiptsid=(select newid())        
 set @rec_miscid=(select newid())        
 set @rec_translogid=(select newid())        
 if  @rec_transdate='' set @rec_transdate=getdate()        
        
 set @rec_Itemid=(select id from list_items where itemnumber=@rec_item and rev=@rec_rev)        
         
 set @rec_Locationid=dbo.amics_fn_api_warehouseid(@rec_warehouse,@rec_location)        
 set @rec_sourcesid=(select id from inv_sources where sourceref=@rec_source)        
 set @Reasonid=(select id from list_reasons where reason=@rec_misc_reason)        
 set @rec_description=(select description from list_items where id=@rec_itemid)        
 set @rec_Itemtype=(select list_itemtypes.itemtype from list_items inner join list_itemtypes on         
     list_items.itemtypeid=list_itemtypes.id where list_items.id=@rec_itemid)        
 set @rec_Invtype=(select list_invtypes.invtype from list_items inner join list_invtypes on         
     list_items.invtypeid=list_invtypes.id where list_items.id=@rec_itemid)        
 set @rec_invtypeid=(select invtypeid from list_items where id=@rec_itemid)        
 set @basid=(select id from list_invtypes where invtype='BASIC')        
 set @serid=(select id from list_invtypes where invtype='SERIAL')       
 set @lotid=(select id from list_invtypes where invtype='LOT')        
 set @noninvid=(select id from list_invtypes where invtype='NONINV')        
        
 set @rec_quantity2=@rec_quantity        
        
  set @stock_uomid=(select uomid from list_items where id=@rec_itemid)        
  set @conversion = (select conversion from list_uoms where id=@stock_uomid )        
  set @solinesid=@rec_sourcesrefid      
    
 if @rec_source='PO'        
 begin          
  set @rec_quantity=@rec_quantity*@conversion         
  set @rec_cost=(select unitcost from po_lines where id=@rec_sourcesrefid)        
  set @rec_cost=@rec_cost/@conversion        
  set @routings_polinesid=(select id from wo_routings_po where po_linesid=@rec_sourcesrefid)        
  if @routings_polinesid is null set @routings_po=0 else set @routings_po=1       
  set @solinesid=(select so_linesid from po_lines where id=@rec_sourcesrefid)     
 end        
 if @rec_source='WO'        
 begin          
  set @rec_quantity=@rec_quantity*@conversion         
  set @rec_cost=@rec_cost/@conversion        
 end        
        
 if @rec_source='MISC REC' and @routings_po=0        
  begin        
   insert into inv_misc (id,itemsid,ref,source,reasonid,createdby,account,serlotid) values         
    (@rec_miscid,@rec_itemid,@rec_misc_ref,@rec_misc_source,@reasonid,@rec_user,@rec_account,@rec_serlotid)        
   set @rec_sourcesrefid_serial = @rec_sourcesrefid      
      
      
   --set @rec_sourcesrefid=null        
  end        
        
 if @rec_invtypeid=@basid and @potype=''        
  begin           
   set @rec_sourcesrefid2=@rec_sourcesrefid        
   if (select yesorno  from list_company_options where optionid=58)=0        
   begin        
   set @rec_sourcesrefid2=null        
   end           
           
   exec amics_sp_api_update_invbasic         
   @invbasicid=@rec_invbasicid,@locationid=@rec_Locationid,@itemid=@rec_Itemid,        
   @cost=@rec_Cost,@quantity=@rec_quantity,@lotno='',@color='',@user=@rec_user,@sources_refid=@rec_sourcesrefid2        
        
        
   exec amics_sp_api_update_invreceipts        
   @invtypeid= @rec_invtypeid,@invreceiptsid=@rec_receiptsid,@sourcesid=@rec_sourcesid,        
   @sourcesrefid=@rec_sourcesrefid,@locationid =@rec_locationid,@invbasicid=@rec_invbasicid,        
   @quantity=@rec_quantity2,@transdate=@rec_transdate,@user=@rec_user,@recnum=@rec_receiver         
   ,@recnotes=@rec_notes         
        
---   Get license plate number for update translog        
   set @lic_plate = (select lic_plate_number from inv_basic where ID=@rec_invbasicid)          
  end        
 if @rec_invtypeid=@lotid and @potype='' and @routings_po=0        
  begin        
   set @rec_quantity=0        
---  CREATEing the following to include lic plate to inv_basic for lot items        
   declare cuMyCursor Cursor For (select qty,lotno,color,expdate,id from         
    inv_serlot where transnum=@rec_transnum)        
   Open cuMyCursor        
--   Check to see if company is using license plates - Bema        
   set @run_plate = (select yesorno from list_company_options where optionid=25)        
           
--   if using plates get number of pallets and get a lic_plate for each pallet        
   Fetch Next from cuMyCursor into @rec_batqty,@rec_lot,@rec_color,@rec_exp,@rec_serlotid        
   while @@fetch_status = 0        
   begin         
    set @rec_quantity=@rec_quantity+@rec_batqty        
    set @rec_invbasicid=(select newid())        
    set @rec_receiptsid=(select newid()) -- change the inv_receipts.id          
               
    if @run_plate = 1        
     begin        
     set @rec_lic_plt=(select lic_plt from #mytable where color=@rec_color)        
     if  @rec_lic_plt is null        
     begin        
     update list_next_number set lic_plate=lic_plate+1        
     set @newplate= (select lic_plate from list_next_number)        
     insert into #mytable values (@rec_color,@newplate)        
     end        
----  get License plate for sending to update transloglot        
     set @rec_lic_plt=(select lic_plt from #mytable where color=@rec_color)        
     end        
    if @run_plate = 0 or @run_plate is null        
    set @rec_lic_plt=''        
    if @rec_source='PO'        
    set @rec_lotqty =(@rec_batqty * @conversion)        
    else        
    set @rec_lotqty=@rec_batqty         
    if (select smallname from list_companies) = 'aerospace' and @rec_source='PO'     
    begin        
    exec amics_sp_api_update_invbasic_r2       
    @invbasicid=@rec_invbasicid,@locationid=@rec_Locationid,@itemid=@rec_Itemid,        
    @cost=@rec_Cost,@quantity=@rec_lotqty,@lotno=@rec_lot,@color=@rec_color,@user=@rec_user,@exp=@rec_exp,@sources_refid=@rec_sourcesrefid,@lic_plate=@rec_lic_plt,@extid=@rec_extedid        
    end        
    if (select smallname from list_companies) <> 'aerospace'        
    exec amics_sp_api_update_invbasic_r2         
    @invbasicid=@rec_invbasicid,@locationid=@rec_Locationid,@itemid=@rec_Itemid,        
    @cost=@rec_Cost,@quantity=@rec_lotqty,@lotno=@rec_lot,@color=@rec_color,@user=@rec_user,@exp=@rec_exp,@sources_refid=@rec_sourcesrefid,@lic_plate=@rec_lic_plt,@extid=@rec_extedid        
          
      
            
    exec amics_sp_api_update_invreceipts        
    @invtypeid= @rec_invtypeid,@invreceiptsid=@rec_receiptsid,@sourcesid=@rec_sourcesid,        
    @sourcesrefid=@rec_sourcesrefid,@locationid =@rec_locationid,@invbasicid=@rec_invbasicid,        
    @quantity=@rec_batqty,@transdate=@rec_transdate,@user=@rec_user,@recnum=@rec_receiver,@recnotes=@rec_notes         
        
            
  set @lic_plate = (select CAST(lic_plate_number AS VARCHAR(50)) from inv_basic where ID=@rec_invbasicid)        
  exec amics_sp_api_update_transloglot        
    @translogid  = @rec_translogid,        
    @quantity  = @rec_batqty,        
    @lotno   = @rec_lot,        
    @color   = @rec_color,        
    @user   = @rec_user,        
    @cost   = @rec_Cost,        
    @loc   = @rec_location,        
    @whs   = @rec_warehouse,        
    @lic_plate  = @lic_plate,        
    @lic_plate_flag = 0        
    Fetch Next from cuMyCursor into @rec_batqty,@rec_lot,@rec_color,@rec_exp,@rec_serlotid        
   end         
   close cuMyCursor        
   deallocate cuMyCursor        
  end        
          
 if @rec_invtypeid=@serid and (@potype='' or @potype='DROPSHIP') and @routings_po=0        
  begin        
 --set @rec_sourcesrefid_serial=@solinesid       
 exec amics_sp_api_update_invserial @itemid=@rec_itemid,@receiptsid=@rec_receiptsid,@locationid=@rec_Locationid,        
   @rec_translogid2=@rec_translogid,@cost=@rec_Cost,@rec_user2=@rec_user,@transnum=@rec_transnum,      
     @sourcesrefid=@solinesid     
     exec amics_sp_api_update_invreceipts        
     @invtypeid= @rec_invtypeid,@invreceiptsid=@rec_receiptsid,@sourcesid=@rec_sourcesid,        
     @sourcesrefid=@rec_sourcesrefid,@locationid =@rec_locationid,@invbasicid=@rec_invbasicid,        
     @quantity=@rec_batqty,@transdate=@rec_transdate,@user=@rec_user,@recnum=@rec_receiver,@recnotes=@rec_notes        
    
   set @rec_quantity=(select count(*) from inv_serlot where transnum=@rec_transnum)        
         
  end        
 if @rec_invtypeid=@noninvid or @routings_po=1 or @potype='DROPSHIP'     
  begin        
   exec amics_sp_api_update_invreceipts        
   @invtypeid= @rec_invtypeid,@invreceiptsid=@rec_receiptsid,@sourcesid=@rec_sourcesid,        
   @sourcesrefid=@rec_sourcesrefid,@locationid =@rec_locationid,@invbasicid=@rec_invbasicid,        
   @quantity=@rec_quantity,@transdate=@rec_transdate,@user=@rec_user,@recnum=@rec_receiver,        
   @recnotes=@rec_notes         
  end        
 if  @routings_po=1        
  begin        
   set @wo_routingsid=(select wo_routingsid from wo_routings_po where po_linesid=@rec_sourcesrefid)        
   set @womain=(select top 1 womain from wo_routings_po         
    inner join wo_routings on wo_routings_po.wo_routingsid=wo_routings.id        
    inner join wo_main on wo_routings.wo_mainid=wo_main.id        
    where wo_routings_po.wo_routingsid=@wo_routingsid)        
   set @rec_location='WO-'+rtrim(@womain) -- Receiving location is the work order        
        
   insert into wo_routings_cost (wo_routingsid,compcost,compqty,createdby)         
    values         
    (@wo_routingsid,@rec_cost,@rec_quantity,@rec_user)        
  end        
        
 if @rec_packlist <> ''        
 begin        
 set @soshipid=(select ID from inv_soship where packlist=@rec_packlist)        
 update inv_soship set void='1' where id=@soshipid        
 update inv_pick set void_quantity=pick_quantity where inv_soshipid=@soshipid        
 update inv_pick set pick_quantity=0 where inv_soshipid=@soshipid        
 end        
 set @lic_plate_flag=0        
  if (@rec_invtypeid = @lotid or @rec_invtypeid = @serid)        
  begin        
 set @lic_plate=''        
 set @lic_plate_flag=''        
  end        
 exec amics_sp_api_update_translog        
 @translogid  = @rec_translogid,        
 @itemid   = @rec_itemid,        
 @Invtype  = @rec_invtype,        
 @Itemtype  = @rec_itemtype,        
 @Rev   = @rec_rev,        
 @trans   = @rec_source,        
 @item   = @rec_Item,        
 @Description = @rec_description,         
 @Warehouse  = @rec_warehouse,        
 @Location  = @rec_location,        
 @Reason   = @rec_misc_reason,        
 @Source   = @rec_misc_source,        
 @Ref   = @rec_misc_ref,        
 @notes   = @rec_Notes,        
 @Quantity  = @rec_quantity,        
 @Cost   = @rec_cost,        
 @transdate  = @rec_transdate,        
 @user   = @rec_user,        
 @lic_plate  =   @lic_plate,        
 @lic_plate_flag = @lic_plate_flag ,        
 @recid   = @rec_receiptsid,
 @userfield2 = @user2        
         
drop table #mytable       

select 'Successfully Saved' as [message]
 
END      
GO
----------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_Inquiry]    Script Date: 12-07-2022 08:11:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_Inquiry]   
@part varchar(50)='',@lotno varchar(50)='',@serial varchar(50)='',@tag varchar(50)='',@location varchar(50) ='',   
@action smallint =1, @user varchar(50) ='', @desc varchar(50)='',@er varchar(50)='',@mdatIn varchar(50)=''   
AS   
BEGIN   
if @part<>'' set @part=rtrim(@part)+'%'   
if @location<>'' set @location=@location+'%'   
if @lotno<>'' set @lotno=rtrim(@lotno)+'%'   
if @serial<>'' set @serial=rtrim(@serial)+'%'   
if @tag<>'' set @tag=rtrim(@tag)+'%'   
if @desc<>'' set @desc=rtrim(@desc)+'%'   
if @er<>'' set @er='%'+rtrim(@er)+'%'  
if @mdatIn<>'' set @mdatIn='%'+rtrim(@mdatIn)+'%'  
  
declare @usersid uniqueidentifier  
set @usersid =(select id from sec_users where userid=@user)  
if @action=1   
-- PART NUMBER SEARCH  
SELECT dbo.list_items.itemnumber as itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,   
convert(varchar,convert(decimal(10, 0),ISNULL(ceiling(dbo.inv_basic.quantity), 0))) AS quantity,   
'' AS serial,  
ISNULL(dbo.inv_basic.lotno, '') AS lotno,   
ISNULL(dbo.inv_basic.color, '') AS color,   
ISNULL(dbo.inv_basic.lic_plate_number, '') AS lic_plate,   
'' as source,  
'' as ref, 
 ISNULL(dbo.inv_basic.cost, 0) as cost,  
 list_itemtypes.itemtype as itemtype,  
(select dbo.amics_fn_api_er((select inv_basic.id),null)) as er, '' as mdatin  
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE dbo.list_items.itemnumber like @part and dbo.inv_basic.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses)  
union all  
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,  
convert(varchar,convert(decimal(10, 0),1)) AS quantity,  
ISNULL(dbo.inv_serial.serno, '') AS serial,    
ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_serial.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref ,   ISNULL(dbo.inv_serial.cost, 0) as cost,list_itemtypes.itemtype as itemtype,  
(select dbo.amics_fn_api_er(null,(select inv_serial.id))) as er, '' as mdatin   
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid ON dbo.list_items.id = dbo.inv_serial.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE dbo.list_items.itemnumber like @part and dbo.inv_serial.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses where users_id=@usersid)  
order by itemnumber  
ELSE IF @action = 2   
begin 
-- ER SEARCH  
   
SELECT  dbo.list_items.itemnumber,  
 dbo.list_items.description,  
 dbo.list_locations.location,  
 --dbo.inv_basic.quantity AS quantity,   
 convert(varchar,convert(decimal(10, 0),ISNULL(ceiling(dbo.inv_basic.quantity), 0))) AS quantity, 
 '' as serial,  
 '' AS lotno,   
 '' AS color,   
 '' as source,   
 '' as ref, 0 as lic_plate,    
 ISNULL(dbo.inv_basic.cost, 0) as cost,list_itemtypes.itemtype as itemtype,  
 (select dbo.amics_fn_api_er((select inv_basic.id),null)) as er, dbo.so_main.mdat_in as mdatin  
 FROM            dbo.list_items LEFT OUTER JOIN  
        dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id RIGHT OUTER JOIN  
        dbo.list_warehouses RIGHT OUTER JOIN  
        dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid RIGHT OUTER JOIN  
        dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid RIGHT OUTER JOIN  
        dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id RIGHT OUTER JOIN  
        dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id  
 GROUP BY dbo.list_warehouses.warehouse, dbo.inv_basic.id, dbo.inv_basic.cost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, dbo.so_lines.quantity,   
        dbo.list_locations.location, dbo.inv_basic.quantity, dbo.so_main.somain, dbo.so_lines.id, dbo.so_main.mdat_in 
 HAVING  (dbo.so_main.somain like  @er) and (dbo.list_items.itemnumber is not null) and inv_basic.quantity>0  
   
 union all  
   
 SELECT  dbo.so_lines.itemnumber,   
 dbo.so_lines.description,   
 list_locations.location,   
 convert(varchar,convert(decimal(10, 2),1)) AS quantity,  
 ISNULL(dbo.inv_serial.serno, '') AS serial,   
 ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
 '' AS color,  
 '' as source,  
 '' as ref, 0 as lic_plate,
 ISNULL(dbo.inv_serial.cost, 0) as cost,  
 list_itemtypes.itemtype as itemtype,  
 (select dbo.amics_fn_api_er(null,(select inv_serial.id))) as er, dbo.so_main.mdat_in as mdatin 
 from so_main inner join   
   so_lines on so_main.id=so_lines.somainid left join   
   list_items on so_lines.itemsid=list_items.id left join   
   list_itemtypes on list_items.itemtypeid=list_itemtypes.id left join   
   inv_serial on so_lines.id=case when inv_serial.transferid is null then inv_serial.so_linesid else inv_serial.transferid end inner join   
   list_locations on inv_serial.locationsid=list_locations.id left join   
   list_warehouses on list_locations.warehousesid = list_warehouses.id
    where somain like @er and dbo.inv_serial.quantity > 0  
order by itemnumber  
end
ELSE IF @action = 3   
  
-- LOCATION SEARCH  
  
SELECT   
dbo.list_items.itemnumber,  
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,   
convert(varchar,convert(decimal(10, 0),ISNULL(ceiling(dbo.inv_basic.quantity), 0))) AS quantity,   
'' AS serial,    
ISNULL(dbo.inv_basic.lotno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_basic.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref ,   
ISNULL(dbo.inv_basic.cost, 0) as cost,list_itemtypes.itemtype as itemtype,  
 (select dbo.amics_fn_api_er((select inv_basic.id),null)) as er, '' as mdatin   
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.list_locations.location, '') like @location) and dbo.inv_basic.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses)  
union all  
  
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,  
convert(varchar,convert(decimal(10, 0),1)) AS quantity,  
ISNULL(dbo.inv_serial.serno, '') AS serial,    
ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_serial.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref , ISNULL(dbo.inv_serial.cost, 0) as cost,  
list_itemtypes.itemtype as itemtype  
,(select somain from so_main where id=inv_serial.so_mainid) as er, '' as mdatin   
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid ON dbo.list_items.id = dbo.inv_serial.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.list_locations.location, '') like @location) and dbo.inv_serial.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses)  
order by itemnumber  
  
ELSE IF @action = 4  
  
-- DESCRIPTION SEARCH  
  
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,   
convert(varchar,convert(decimal(10, 0),ISNULL(ceiling(dbo.inv_basic.quantity), 0))) AS quantity,   
'' AS serial,    
ISNULL(dbo.inv_basic.lotno, '') AS lotno,  
'' AS color,  
ISNULL(dbo.inv_basic.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref , ISNULL(dbo.inv_basic.cost, 0) as cost ,list_itemtypes.itemtype as itemtype,  
 (select dbo.amics_fn_api_er((select inv_basic.id),null)) as er, '' as mdatin  
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
--dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
--dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
--dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.list_items.description, '') like @desc) and dbo.inv_basic.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses)  
union all  
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,  
convert(varchar,convert(decimal(10, 0),1)) AS quantity,  
ISNULL(dbo.inv_serial.serno, '') AS serial,    
ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_serial.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref ,ISNULL(dbo.inv_serial.cost, 0) as cost ,list_itemtypes.itemtype as itemtype,  
(select somain from so_main where id=inv_serial.so_mainid) as er, '' as mdatin  
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
--dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
--dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
--dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid ON dbo.list_items.id = dbo.inv_serial.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.list_items.description, '') like @desc) and dbo.inv_serial.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses)  
order by itemnumber  
  
  
ELSE IF @action = 5  
  
-- SERIAL NUMBER SEARCH    
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,  
--ISNULL(ceiling(dbo.inv_serial.quantity), 0) AS quantity,   
convert(varchar,convert(decimal(10, 0),1)) AS quantity,  
ISNULL(dbo.inv_serial.serno, '') AS serial,    
ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_serial.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref , ISNULL(dbo.inv_serial.cost, 0) as cost ,list_itemtypes.itemtype as itemtype,  
(select dbo.amics_fn_api_er(null,(select inv_serial.id))) as er, '' as mdatin  
  
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid ON dbo.list_items.id = dbo.inv_serial.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.inv_serial.serno , '') like @serial) and dbo.inv_serial.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses )  
order by itemnumber  
  
ELSE IF @action = 6  
  
-- TAG NUMBER SEARCH    
SELECT   
dbo.list_items.itemnumber,   
dbo.list_items.description,   
ISNULL(dbo.list_locations.location, '') AS location,  
--ISNULL(ceiling(dbo.inv_serial.quantity), 0) AS quantity,   
convert(varchar,convert(decimal(10, 0),1)) AS quantity,  
ISNULL(dbo.inv_serial.serno, '') AS serial,    
ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
'' AS color,  
ISNULL(dbo.inv_serial.lic_plate_number, '') AS lic_plate,   
'' as source,   
'' as ref , ISNULL(dbo.inv_serial.cost, 0) as cost ,list_itemtypes.itemtype as itemtype,  
(select dbo.amics_fn_api_er(null,(select inv_serial.id))) as er, '' as mdatin  
  
FROM dbo.list_warehouses RIGHT OUTER JOIN   
dbo.list_uoms RIGHT OUTER JOIN   
dbo.list_items ON dbo.list_uoms.id = dbo.list_items.uomid LEFT OUTER JOIN   
dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN   
dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN   
dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN   
dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id RIGHT OUTER JOIN   
dbo.list_locations RIGHT OUTER JOIN   
dbo.inv_serial ON dbo.list_locations.id = dbo.inv_serial.locationsid ON dbo.list_items.id = dbo.inv_serial.itemsid ON   
dbo.list_warehouses.id = dbo.list_locations.warehousesid   
WHERE (ISNULL(dbo.inv_serial.tagno , '') like @tag) and dbo.inv_serial.quantity>0   
and dbo.list_warehouses.id in (select warehouses_id from sec_users_warehouses where users_id=@usersid)  
order by itemnumber  
 
ELSE IF @action = 7  
begin 
-- MDAT IN SEARCH     
SELECT  dbo.list_items.itemnumber,  
 dbo.list_items.description,  
 dbo.list_locations.location,  
 --dbo.inv_basic.quantity AS quantity,   
 convert(varchar,convert(decimal(10, 0),ISNULL(ceiling(dbo.inv_basic.quantity), 0))) AS quantity, 
 '' as serial,  
 '' AS lotno,   
 '' AS color,   
 '' as source,   
 '' as ref, 0 as lic_plate,      
 ISNULL(dbo.inv_basic.cost, 0) as cost,list_itemtypes.itemtype as itemtype,
 (select dbo.amics_fn_api_er((select inv_basic.id),null)) as er, dbo.so_main.mdat_in  as mdatin  
 FROM            dbo.list_items LEFT OUTER JOIN  
        dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id RIGHT OUTER JOIN  
        dbo.list_warehouses RIGHT OUTER JOIN  
        dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid RIGHT OUTER JOIN  
        dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid RIGHT OUTER JOIN  
        dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id RIGHT OUTER JOIN  
        dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id  
 GROUP BY dbo.list_warehouses.warehouse, dbo.inv_basic.id, dbo.inv_basic.cost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, dbo.so_lines.quantity,   
        dbo.list_locations.location, dbo.inv_basic.quantity, dbo.so_main.somain, dbo.so_lines.id, dbo.so_main.mdat_in 
 HAVING  (dbo.so_main.mdat_in like @mdatIn) and (dbo.list_items.itemnumber is not null) and inv_basic.quantity>0  
   
 union all  
   
 SELECT  dbo.so_lines.itemnumber,   
 dbo.so_lines.description,   
 list_locations.location,   
 convert(varchar,convert(decimal(10, 2),1)) AS quantity,  
 ISNULL(dbo.inv_serial.serno, '') AS serial,   
 ISNULL(dbo.inv_serial.tagno, '') AS lotno,   
 '' AS color,  
 '' as source,  
 '' as ref, 0 as lic_plate,
 ISNULL(dbo.inv_serial.cost, 0) as cost,  
 list_itemtypes.itemtype as itemtype,  
 (select dbo.amics_fn_api_er(null,(select inv_serial.id))) as er, dbo.so_main.mdat_in as mdatin  
 from so_main inner join   
   so_lines on so_main.id=so_lines.somainid left join   
   list_items on so_lines.itemsid=list_items.id left join   
   list_itemtypes on list_items.itemtypeid=list_itemtypes.id left join   
   inv_serial on so_lines.id=case when inv_serial.transferid is null then inv_serial.so_linesid else inv_serial.transferid end inner join   
   list_locations on inv_serial.locationsid=list_locations.id left join   
   list_warehouses on list_locations.warehousesid = list_warehouses.id
    where dbo.so_main.mdat_in like @mdatIn and dbo.inv_serial.quantity > 0  
order by itemnumber  
end
END
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_inv_somain_details_items_transfer_ship]    Script Date: 12-07-2022 08:14:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_inv_somain_details_items_transfer_ship]     
@so varchar(50),   
@item varchar(50),  
@user varchar(50),  
@solinesid uniqueidentifier = null, 
@screen  varchar(50) = 'LocTrans'    
AS    
BEGIN    
    
declare @invtype varchar(50)    
declare @test table (wh varchar(50), serid uniqueidentifier,basid uniqueidentifier,cost decimal(18,8),    
  sncost decimal(18,8),linenum smallint,itemtype varchar(50),itemnumber varchar(50),description varchar(150),quantity decimal(18,8),    
  loc varchar(50),available decimal(18,8),somain varchar(50),solinesid uniqueidentifier,serno varchar(50) , tagno varchar(50),total decimal(18,8))    
    
set @invtype=(select invtype from list_items inner join list_invtypes on list_items.invtypeid=list_invtypes.id where itemnumber=@item )    
  
if @screen ='LocTrans'    
begin  
 if @invtype='SERIAL'    
  begin    
   INSERT INTO @test EXEC [dbo].[amics_sp_api_inv_somain_details_items] @solinesid2=@solinesid
   select serid,wh,loc,1 as quantity,serno,tagno from @test where serid not in (select invserialid from inv_transfer_location where invserialid is not null and createdby=@user)    
  end    
 if @invtype='BASIC'    
 Begin    
   INSERT INTO @test EXEC [dbo].[amics_sp_api_inv_somain_details_items] @solinesid2=@solinesid
   select * from     
   (select basid,wh,loc,available-isnull((select quantity from inv_transfer_location where invbasicid=basid  and createdby=@user),0) as quantity,'' as serno,'' as tagno from @test) as x    
   where x.quantity>0    
 end    
end  
if @screen ='Shipment'   
begin  
 if @invtype='SERIAL'    
  begin    
   INSERT INTO @test EXEC [dbo].[amics_sp_api_inv_somain_details_items] @solinesid2=@solinesid
   select serid,wh,loc,1 as quantity,serno,tagno from @test where serid not in (select invserialid from inv_pick_ship where invserialid is not null and createdby=@user)    
  end    
 if @invtype='BASIC'    
 Begin    
   INSERT INTO @test EXEC [dbo].[amics_sp_api_inv_somain_details_items] @solinesid2=@solinesid
   select * from     
   (select basid,wh,loc,available-isnull((select quantity from inv_pick_ship where invbasicid=basid  and createdby=@user),0) as quantity,'' as serno,'' as tagno from @test) as x    
   where x.quantity>0    
 end    
end  
END -- END OF PROCEDURE    
GO
-- exec [dbo].[sp_inv_somain_details_items_transfer_ship] @so='tm-16-0205c',@item='LXB102DX',@user='vasu',@solinesid='4023AE3C-CD01-43EF-BBB0-E9C5E81B0618',@screen='LocTrans'
-- exec [dbo].[sp_inv_somain_details_items_transfer_ship] 'tx-16-78945','Testbas2',@user='admin'    
-- select * from so_main where somain='TM-16-0205C'
-- select * from so_lines where somainid='551CA3B2-068D-4D63-B1ED-4EB63737F5CD'

---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_warehouse_lookup]    Script Date: 12-07-2022 08:20:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_warehouse_lookup] 
	@whid uniqueidentifier = 0x0,
	@warehouse varchar(50) = ''
 AS 
 BEGIN  	
 
  declare  @EmptyGuid UNIQUEIDENTIFIER = 0x0
  if @whid = @EmptyGuid and @warehouse='' Select id,warehouse from list_warehouses order by warehouse
  else if @whid =@EmptyGuid and @warehouse!=''   Select id,warehouse from list_warehouses where warehouse like '%'+@warehouse+'%' order by warehouse
  else if @warehouse='' and   @whid !=@EmptyGuid Select id,warehouse from list_warehouses where id=@whid order by warehouse
  else Select id,warehouse from list_warehouses where warehouse like '%'+@warehouse+'%' or id=@whid order by warehouse

 END
 GO
 
 -------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_location_lookup]    Script Date: 12-07-2022 08:20:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_location_lookup]  
	@whid uniqueidentifier=0x0,
	@locid uniqueidentifier=0x0,
	@location varchar(50)=''
 AS 
  BEGIN  		
	  DECLARE @EmptyGuid UNIQUEIDENTIFIER = 0x0
	  if @whid = @EmptyGuid or @whid is null
		begin
			 Select warehousesid as warehouseId,id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as
				route from list_locations where  isnull(flag_delete,0)=0 order by location
		end
	  else if ( @locid = @EmptyGuid or @locid is null) and @location='' 
		begin
			Select warehousesid as warehouseId, id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as
			route from list_locations where warehousesid=@whid and isnull(flag_delete,0)=0 order by location
		end 
	  else if @location='' and (@locid = @EmptyGuid and @locid is not null)
		begin
			Select warehousesid as warehouseId ,id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as
			route from list_locations where warehousesid=@whid and list_locations.id=@locid and isnull(flag_delete,0)=0 order by location
		end	 
	  else if (@locid = @EmptyGuid or @locid is null) and @location != ''
		begin
			Select warehousesid as warehouseId,id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as
			route from list_locations where warehousesid=@whid and location like '%'+@location+'%' and isnull(flag_delete,0)=0 order by location
		end
	  else
		begin
			Select warehousesid as warehouseId,id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as
			route from list_locations where warehousesid=@whid and (location like '%'+@location+'%' or list_locations.id=@locid) and isnull(flag_delete,0)=0 order by location
		end	
 END
 GO
 -----------------------------------------------------------------------------------------------------------------------------------

 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_itemclass_lookup]    Script Date: 12-07-2022 08:22:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_itemclass_lookup] 
 	@id uniqueidentifier = 0x0,
	@itemclass varchar(50) = ''
	
 AS 
 BEGIN  		

	declare @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if @id = @EmptyGuid and @itemclass='' Select id,itemclass from list_itemclass order by itemclass
	else if @id != @EmptyGuid and @itemclass='' Select id,itemclass from list_itemclass where id=@id order by itemclass
	else if @id = @EmptyGuid and @itemclass != '' Select id,itemclass from list_itemclass where itemclass like '%'+@itemclass+'%' order by itemclass
	else Select id,itemclass from list_itemclass where itemclass like '%'+@itemclass+'%' or id=@id order by itemclass

 END
 GO
 ------------------------------------------------------------------------------------------------------------------------------------

 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_itemcode_lookup]    Script Date: 12-07-2022 08:25:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_itemcode_lookup] 
 	@id uniqueidentifier = 0x0,
	@itemcode varchar(50) = ''
	
 AS 
 BEGIN  	
    declare  @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if @id = @EmptyGuid and @itemcode='' Select id,itemcode from list_itemcodes order by itemcode
	else if @id != @EmptyGuid and @itemcode='' Select id,itemcode from list_itemcodes where id=@id order by itemcode
	else if @id = @EmptyGuid and @itemcode != '' Select id,itemcode from list_itemcodes where itemcode like '%'+ @itemcode +'%' order by itemcode
	else Select id,itemcode from list_itemcodes where itemcode like '%'+@itemcode+'%' or id=@id order by itemcode

 END
 GO
 --------------------------------------------------------------------------------------------------------------------------------------------
  
/****** Object:  StoredProcedure [dbo].[amics_sp_api_itemtype_lookup]    Script Date: 12-07-2022 08:26:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_itemtype_lookup] 
 	@id uniqueidentifier = 0x0,
	@itemtype varchar(50) = ''
	
 AS 
 BEGIN  		
	declare  @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if @id = @EmptyGuid and @itemtype='' Select id,itemtype from list_itemtypes order by itemtype
	else if @id != @EmptyGuid and @itemtype='' Select id,itemtype from list_itemtypes where id=@id order by itemtype
	else if @id = @EmptyGuid and @itemtype != '' Select id,itemtype from list_itemtypes where itemtype like '%'+ @itemtype +'%' order by itemtype
	else Select id,itemtype from list_itemtypes where itemtype like '%'+ @itemtype +'%' or id=@id order by itemtype

 END
 GO
 ---------------------------------------------------------------------------------------------------------------------------------------

 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_uom_lookup]    Script Date: 12-07-2022 08:27:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_uom_lookup] 
 	@id uniqueidentifier  = 0x0,
	@uomref varchar(50) = ''
	
 AS 
 BEGIN  		

    declare  @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if @id = @EmptyGuid and @uomref='' Select id, uomref as uom,uom1 as purchasinguom,conversion as factor from list_uoms order by uomref
	else if @id != @EmptyGuid and @uomref = '' Select id, uomref as uom,uom1 as purchasinguom,conversion as factor from list_uoms where id=@id order by uomref
	else if @id = @EmptyGuid and @uomref != '' Select id, uomref as uom,uom1 as purchasinguom,conversion as factor from list_uoms where uomref like '%'+ @uomref +'%' order by uomref
	else Select id, uomref as uom,uom1 as purchasinguom,conversion as factor from list_uoms where uomref like '%'+ @uomref +'%' or id=@id order by uomref

 END
 GO
 -------------------------------------------------------------------------------------------------------------------------------------------
 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_iteminfo]    Script Date: 12-07-2022 08:35:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_get_iteminfo]
@item varchar(50) = ''
,@rev varchar(10) = ''
,@itemsid uniqueidentifier=0x0
As
BEGIN

if @rev = ''
set @rev = '-'

if @itemsid!= 0x0
	select  items.id,items.itemnumber,items.rev,items.description,warehs.warehouse,loc.location,items.cost/(select conversion from list_uoms where id=uomid) as cost from list_items items inner join list_locations loc on items.locationsid=loc.id inner join list_warehouses warehs on loc.warehousesid=warehs.id where items.id =@itemsid and items.flag_delete=0 and loc.flag_delete=0
else
	select  items.id,items.itemnumber,items.rev,items.description,warehs.warehouse,loc.location,items.cost/(select conversion from list_uoms where id=uomid) as cost from list_items items inner join list_locations loc on items.locationsid=loc.id inner join list_warehouses warehs on loc.warehousesid=warehs.id where items.itemnumber=@item and items.Rev=@rev and items.flag_delete=0 and loc.flag_delete=0
END
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_reasoncode]    Script Date: 12-07-2022 08:42:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_get_reasoncode]
@reasoncode varchar(50) = ''
,@codefor varchar(50) = ''
As
BEGIN
declare @increase int = 2

if @codefor = 'INCREASE'
set @increase = 1
else if @codefor = 'DECREASE'
set @increase = 0

if @reasoncode = ''
select id,reason from list_reasons where increase=@increase
else
select id,reason from list_reasons where reason=@reasoncode and increase=@increase
END
GO

---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_list_company_options]    Script Date: 12-07-2022 08:42:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_get_list_company_options]
@optionid decimal(18,8) = 0
,@screenname varchar(50) = ''
As
BEGIN
 
--if @optionid != 0
--select id,optionid,description,yesorno,optionvalue from list_company_options5 where optionid=@optionid
--else
--select id,optionid,description,yesorno,optionvalue from list_company_options5 where area=@screenname

--select id,optionid,description,yesorno,optionvalue from list_company_options5  
if @optionid = 0  
	select id,optionid,description,yesorno,optionvalue from list_company_options    
else
	select id,optionid,description,yesorno,optionvalue from list_company_options where optionid=@optionid  
END
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_itembom_exist]    Script Date: 12-07-2022 08:43:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_itembom_exist] 
 	@parentid uniqueidentifier 	
 AS 
 BEGIN  		

	select count(*) as value from items_bom where itemsid_parent=@parentid

 END
 GO

---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_invtranlocdelete]    Script Date: 12-07-2022 08:44:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_invtranlocdelete] 
@createdby varchar(50)=''
AS
BEGIN  		 
	Delete from inv_transfer_location where createdby = @createdby
END
GO
-----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_list_defaults]    Script Date: 12-07-2022 08:47:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_get_list_defaults]
 @formname varchar(50) = ''
As
BEGIN
  select id,formname,textfields,[value] from LIST_DEFAULTS
END
GO
--------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_er_by_pn]    Script Date: 12-07-2022 08:48:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_get_er_by_pn]
 @itemsid uniqueidentifier = '',
 @somain varchar(50)
As
BEGIN
  select distinct(somain),id from so_main where somain like @somain + '%' and id in (select somainid from so_lines where itemsid = @itemsid)
END
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_er_items]    Script Date: 12-07-2022 09:12:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_view_er_items]    
@somain varchar(50)    
AS    
begin 
SELECT        TOP (100) PERCENT dbo.so_main.somain, dbo.so_lines.linenum, dbo.so_lines.id AS solinesid, dbo.so_lines.itemnumber, dbo.so_lines.description, 
                         dbo.list_itemtypes.itemtype, ISNULL(dbo.so_lines.quantity, 0) AS qty, dbo.list_items.id AS itemsid, dbo.list_invtypes.invtype, dbo.so_main.user2, 
                         SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) AS quantity
FROM  

dbo.list_locations AS list_locations_1 RIGHT OUTER JOIN
    dbo.list_itemtypes RIGHT OUTER JOIN

    dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN
		dbo.list_invtypes ON dbo.list_invtypes.id = dbo.list_items.invtypeid right outer join
    dbo.inv_basic RIGHT OUTER JOIN
    dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id LEFT OUTER JOIN
    dbo.list_locations ON dbo.inv_basic.locationsid = dbo.list_locations.id LEFT OUTER JOIN
    dbo.inv_serial ON 
	CASE WHEN inv_serial.transferid IS NOT NULL THEN inv_serial.transferid 
	ELSE inv_serial.so_linesid END = dbo.so_lines.id 
	ON dbo.list_items.id = dbo.so_lines.itemsid RIGHT OUTER JOIN
    dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id ON list_locations_1.id = dbo.inv_serial.locationsid
    
GROUP BY dbo.so_main.somain, dbo.so_lines.linenum, dbo.so_lines.id, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.list_itemtypes.itemtype, 
                         ISNULL(dbo.so_lines.quantity, 0), dbo.so_lines.quantity, dbo.list_items.id, dbo.list_invtypes.invtype, dbo.so_main.user2
HAVING      (dbo.so_main.somain = @somain) AND (dbo.so_lines.quantity > 0) AND 
SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) >0 
ORDER BY dbo.so_main.somain, dbo.so_lines.linenum
--select dbo.so_main.somain,  dbo.so_lines.linenum,  dbo.so_lines.id as solinesid,dbo.so_lines.itemnumber,dbo.so_lines.description,
--dbo.list_itemtypes.itemtype,dbo.so_lines.quantity,dbo.list_items.id as itemsid,dbo.list_invtypes.invtype,dbo.so_main.user2 from dbo.so_main     
--inner join dbo.so_lines on dbo.so_lines.somainid = dbo.so_main.id     
--left outer join dbo.list_items on dbo.list_items.id = dbo.so_lines.itemsid    
--left outer join dbo.list_itemtypes on dbo.list_itemtypes.id = dbo.list_items.itemtypeid    
--left outer join dbo.list_invtypes on dbo.list_invtypes.id = dbo.list_items.invtypeid    
    
--where dbo.so_main.somain = @somain 
--order by dbo.so_main.somain,dbo.so_lines.linenum    
End 
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_notes]    Script Date: 12-07-2022 09:14:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_view_notes] 
 	@itemsid uniqueidentifier =null
	
 AS 
 BEGIN  		
	
	Select id,isnull(linenum,1) as linenum,notesref,notes,parentid from list_notes_general where parentid=@itemsid order by linenum

 END
 GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_insert_inv_serlot]    Script Date: 12-07-2022 09:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_insert_inv_serlot] 
     @transnum  int 
     ,@serno varchar(50)=null
     ,@tagno varchar(50)=null  
     ,@lotno varchar(50)=null  
	 ,@model varchar(50)=null  
     ,@color varchar(50)=null  
     ,@qty decimal(18,8)  
     ,@createdby varchar(50) =''
     ,@expdate smalldatetime = null
As
BEGIN

if @serno is null or @serno =''
insert into inv_serlot(transnum,qty,lotno,color,expdate,createdby,createddate)values(@transnum,@qty,@lotno,@color,@expdate,@createdby,GETDATE())
else
insert into inv_serlot(transnum,qty,SerNo,TagNo,model,createdby,createddate)values(@transnum,@qty,@serno,@tagno,@model,@createdby,GETDATE())

select 'success' as [message]

END
GO

---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_items_bom]    Script Date: 12-07-2022 09:16:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_items_bom] @parentid uniqueidentifier  
as  
declare @invdec smallint,@curdec smallint  
begin  
set @invdec = (select optionvalue from list_company_options where optionid=7)  
set @curdec = (select optionvalue from list_company_options where optionid=8)  
  
SELECT Items_bom.itemsid_parent, Items_bom.itemsid_child,   
ISNULL(Items_bom.findno,'') as findno, List_items_a.itemnumber, List_items_a.rev,   
List_items_a.description, round(Items_bom.quantity,@invdec) as quantity, ISNULL(List_uoms.uomref,'') as uomref,   
 Items_bom.ref, Items_bom.comments
 , round(isnull(List_items_a.cost,0),@curdec) as cost
 --,(select [dbo].[fn_decimals] (isnull(List_items_a.cost,0),@curdec)) as cost
 ,  
 round((isnull(List_items_a.cost,0)*Items_bom.quantity),@curdec) as extcost,Items_bom.id,items_bom.linenum
 , (select isnull(dbo.list_itemtypes.itemtype,'') from  dbo.list_itemtypes where dbo.list_itemtypes.id = (select dbo.list_items.itemtypeid from dbo.list_items where id = items_bom.itemsid_child)) as itemtype
 FROM   dbo.items_bom Items_bom 
 INNER JOIN  dbo.list_items List_items  ON  Items_bom.itemsid_parent = List_items.id
 INNER JOIN dbo.list_items List_items_a ON  Items_bom.itemsid_child = List_items_a.id 
 LEFT OUTER JOIN dbo.list_uoms List_uoms ON  List_items_a.uomid = List_uoms.id 
 --LEFT OUTER JOIN dbo.list_itemtypes ON  list_itemtypes.id = List_items.itemtypeid 
 
 WHERE  Items_bom.itemsid_parent = @parentid  
 order by items_bom.linenum 
End
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_validate_sertag]    Script Date: 12-07-2022 09:17:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_validate_sertag]
     @itemsid uniqueidentifier
     ,@serno varchar(50)=null
     ,@tagno varchar(50)=null   
As
BEGIN
 
	if @serno is null or @serno =''
         select list_items.id as itemsid,inv_serial.id as serialid,list_items.itemnumber,list_items.rev,inv_serial.serno,inv_serial.tagno,inv_serial.locationsid 
		 from dbo.list_items inner join dbo.inv_serial on list_items.id=inv_serial.itemsid where tagno=@tagno and quantity > 0
     else
         select list_items.id as itemsid, inv_serial.id as serialid,list_items.itemnumber,list_items.rev,inv_serial.serno,inv_serial.tagno,inv_serial.locationsid 
		 from dbo.list_items inner join dbo.inv_serial on list_items.id=inv_serial.itemsid where serno=@serno and quantity > 0	  

END
GO

---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_items_po]    Script Date: 12-07-2022 09:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_items_po] @parentid uniqueidentifier
as
declare @invdec smallint,@curdec smallint
begin
set @invdec = (select optionvalue from list_company_options where optionid=7)
set @curdec = (select optionvalue from list_company_options where optionid=8)
SELECT     '' as dummy1,'' as dummy2,ROW_NUMBER() over (order by pomain) as linenum,dbo.po_lines.linenum as poline,dbo.po_main.pomain,dbo.po_main.id as pomainid, round(ISNULL(dbo.po_lines.quantity, 0),@invdec) AS quantity,
round(ISNULL(SUM(dbo.inv_receipts.recd_quantity), COUNT(dbo.inv_serial.id)),@curdec) as received, ISNULL(dbo.po_lines.job, '') AS somain, 
'' as trans_date, dbo.list_items.id,dbo.list_suppliers.supplier,'' as p10,'' as p11,'' as p12,'' as p13,'' as p14,'' as p15,'' as p16,'' as p17,'' as p18,'' as p19,'' as p20
FROM         dbo.po_lines LEFT OUTER JOIN
                      dbo.so_lines LEFT OUTER JOIN
                      dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id ON dbo.po_lines.so_linesid = dbo.so_lines.id LEFT OUTER JOIN
                      dbo.inv_receipts LEFT OUTER JOIN
                      dbo.inv_serial ON dbo.inv_receipts.id = dbo.inv_serial.receiptsid ON dbo.po_lines.id = dbo.inv_receipts.sources_refid RIGHT OUTER JOIN
                      dbo.list_items ON dbo.po_lines.itemsid = dbo.list_items.id RIGHT OUTER JOIN
                      dbo.po_main ON dbo.po_lines.pomainid = dbo.po_main.id RIGHT OUTER JOIN
                      dbo.list_suppliers ON dbo.list_suppliers.id = dbo.po_main.suppliersid
GROUP BY dbo.po_lines.linenum,dbo.list_items.itemnumber, dbo.po_main.pomain, dbo.po_main.id,dbo.po_lines.quantity, ISNULL(dbo.po_lines.job, ''),
                      dbo.list_items.id, dbo.po_main.podate,dbo.list_suppliers.supplier
having dbo.list_items.id=@parentid
End
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_pickqty_transloc]    Script Date: 12-07-2022 09:20:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_pickqty_transloc] 
 	@solinesid uniqueidentifier  = 0x0,
	@invserialid uniqueidentifier = 0x0,
	@invbasicid uniqueidentifier = 0x0
	
 AS
 BEGIN  	
	declare @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if (@invserialid != @EmptyGuid)
		begin
			Select isnull(quantity,0) as quantity,createdby from inv_transfer_location where solinesid =@solinesid  and invserialid=@invserialid 
			union all 
			Select isnull(quantity,0) as quantity,createdby from inv_pick_ship where solinesid =@solinesid  and invserialid=@invserialid 
			select isnull(quantity,0) as quantity,createdby from inv_serial where id = @invserialid
		end 
	else
		begin
			select isnull(quantity,0) as quantity,createdby from inv_transfer_location where solinesid =@solinesid and invbasicid=@invbasicid
			union all 
			select isnull(quantity,0) as quantity,createdby from inv_pick_ship where solinesid =@solinesid and invbasicid=@invbasicid
			select isnull(quantity,0) as quantity,createdby from inv_basic where id =@invbasicid
		end
 END
 GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_transcntchk]    Script Date: 12-07-2022 09:26:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_transcntchk] 
 	@createdby varchar(50)	
AS
BEGIN  		 
	Select COUNT(*) from inv_transfer_location where createdby=@createdby
END
GO

---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_transnumdate]    Script Date: 12-07-2022 09:27:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_transnumdate] 
 	@transnum varchar(50)	
AS
BEGIN  		 
	Select CONVERT(varchar, transdate,101) from translog where transnum = @transnum
END
GO

---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_listnextnumupd]    Script Date: 12-07-2022 09:29:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_listnextnumupd] 
 	
AS
BEGIN  		 
	Update list_next_number set list_next_number.translognum = list_next_number.translognum + 1
	select isnull(list_next_number.translognum,1) as translognum from list_next_number
END
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgloc_translocupdate]    Script Date: 12-07-2022 09:30:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgloc_translocupdate] 
@id uniqueidentifier=null,
@action int=0,
@quantity decimal=0,
@invbasicid uniqueidentifier=null,
@invserialid uniqueidentifier=null,
@solinesid uniqueidentifier=null,
@createdby varchar(50)=''

AS
BEGIN  	
	if @action = 1
		begin
			set @id = NEWID()
			if (@invbasicid is not null)
				insert into dbo.inv_transfer_location (id,solinesid,invbasicid,quantity,createddate,createdby) values (@id,@solinesid,@invbasicid,@quantity,GETDATE(),@createdby)					
			else
				insert into dbo.inv_transfer_location (id,solinesid,invserialid,quantity,createddate,createdby) values (@id,@solinesid,@invserialid,@quantity,GETDATE(),@createdby)
			
		end
	else --action 2
	begin
		if(@invbasicid is not null)
			delete from inv_transfer_location where invbasicid = @invbasicid and createdby = @createdby
		else
			delete from inv_transfer_location where invserialid = @invserialid and createdby = @createdby
	end 
	
END
GO
----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_item_bom]    Script Date: 12-07-2022 09:33:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_item_bom]
@actionflag smallint,@id uniqueidentifier=null,@itemsid_parent uniqueidentifier=null,@itemsid_child uniqueidentifier=null,
@linenum smallint,@quantity char(50)=null,@ref varchar(250)=null,@comments varchar(150)=null,
@createdby varchar(50)='',@findno varchar(50)=''
AS
BEGIN
declare @parlocid uniqueidentifier,@invtypeid uniqueidentifier
declare @tbl_exists table (message varchar(50))  

if @actionflag=1
	begin
		insert into items_bom
		(itemsid_parent,itemsid_child,linenum,quantity,ref,comments,createdby,findno) values 
		(@itemsid_parent,@itemsid_child,@linenum,@quantity ,@ref ,@comments ,@createdby ,@findno)

		if exists(select top 1 * from items_bom where itemsid_child=@itemsid_child)  
			insert into @tbl_exists values ('Successfully Saved')  				
	  
		select * from @tbl_exists  
	end 
if @actionflag=2
	begin
		update items_bom set 
		linenum=@linenum,itemsid_child=@itemsid_child ,
		quantity =@quantity ,ref =@ref, comments=@comments,
		findno=@findno where id=@id
	
		if exists(select top 1 * from items_bom where itemsid_child=@itemsid_child)  		
			if not exists(Select message from @tbl_exists)
				insert into @tbl_exists values ('Successfully Updated')  				
			else
				update @tbl_exists set message='Successfully Updated'
	  
		select * from @tbl_exists  
	end
if @actionflag=3
	begin
		delete items_bom where id=@id
		--update inv_serial set isassembled=0 where itemsid=@itemsid_child 
	
		if not exists(select top 1 * from items_bom where id=@id)  
			if not exists(Select message from @tbl_exists)
			insert into @tbl_exists values ('Successfully Deleted')  				
		else
			update @tbl_exists set message='Successfully Deleted'				
	  
		select * from @tbl_exists  
	end 
END
GO

-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_proj_items]    Script Date: 12-07-2022 09:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_view_proj_items]    
@projectid varchar(50)    
AS    
begin    
select dbo.so_main.somain,  dbo.so_lines.linenum,  dbo.so_lines.id as solinesid,
dbo.so_lines.itemnumber,dbo.so_lines.description,dbo.list_itemtypes.itemtype,
dbo.so_lines.quantity,dbo.list_items.id as itemsid,dbo.list_invtypes.invtype,dbo.so_main.user2 from dbo.so_main     
inner join dbo.so_lines on dbo.so_lines.somainid = dbo.so_main.id     
inner join dbo.list_projects on dbo.list_projects.id = dbo.so_main.projectid
left outer join dbo.list_items on dbo.list_items.id = dbo.so_lines.itemsid    
left outer join dbo.list_itemtypes on dbo.list_itemtypes.id = dbo.list_items.itemtypeid    
left outer join dbo.list_invtypes on dbo.list_invtypes.id = dbo.list_items.invtypeid    
    
where dbo.list_projects.project = @projectid 
order by dbo.so_main.somain,dbo.so_lines.linenum    
End    
GO
---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_notes_general]    Script Date: 12-07-2022 09:37:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_notes_general] 
 @actionflag		smallint, 
 @parentid			uniqueidentifier = null, 
 @id				uniqueidentifier = null, 
 @linenum			int =0,
 @notesref			varchar (50) ='', 
 @notes				varchar (max)='', 
 @createdby		varchar (50) = '', 
 @retmsg varchar(100)= '' OUTPUT 
 AS 
 BEGIN 
 	--if @actionflag = 1	 
 	--SET NOCOUNT ON 
 declare @tbl_exists table (message varchar(50))  
 Begin Try 
 	if @actionflag=1 
	begin
 		Insert into dbo.list_notes_general (parentid,linenum,notesref,notes,createdby) 
 		values (@parentid,@linenum,@notesref,@notes,@createdby) 
		
		if exists(select top 1 * from list_notes_general where parentid=@parentid and linenum=@linenum)  
			insert into @tbl_exists values ('Successfully Saved')  				
	  
		select * from @tbl_exists 
	end
 	if @actionflag=2 
	begin
 		update dbo.list_notes_general 
 			set dbo.list_notes_general.notesref=@notesref,linenum=@linenum, 
 				dbo.list_notes_general.notes=@notes 
 		where id=@id	
		
		if exists(select top 1 * from list_notes_general where id=@id)  		
			if not exists(Select message from @tbl_exists)
				insert into @tbl_exists values ('Successfully Updated')  				
			else
				update @tbl_exists set message='Successfully Updated'
	  
		select * from @tbl_exists  
	end
 	if @actionflag=3 
	begin
 		delete list_notes_general where id=@id 

		if not exists(select top 1 * from list_notes_general where id=@id )  
			if not exists(Select message from @tbl_exists)
			insert into @tbl_exists values ('Successfully Deleted')  				
		else
			update @tbl_exists set message='Successfully Deleted'				
	  
		select * from @tbl_exists  
	end
 End Try 
 Begin Catch 
 		Set @retmsg = (select text from sys.messages where message_id = 50001) 
 end Catch 
 END
 GO
----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_partmaster]    Script Date: 12-07-2022 09:38:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_maintain_partmaster] 
 @actionflag smallint,
 @id uniqueidentifier =null,
 @itemnumber varchar (50)='', 
 @rev varchar (50), 
 @description varchar (150) = '', 
 @SalesDescription varchar (150) = '', 
 @PurchaseDescription varchar (150) = '', 
 @invtypeidv varchar (50) = '', 
 @itemtypeidv varchar (50) = '', 
 @itemclassidv varchar (50) = '', 
 @itemcodeidv varchar (50) = '', 
 @uomid uniqueidentifier,  
 @conversion decimal = 1.0, -- Added by tony 06/12/2014 conversion default
 @cost decimal(18,8)=0, 
 @markup decimal(18,8)=0,
 @price  decimal(18,8)=0,
 @price2  decimal(18,8)=0,
 @price3  decimal(18,8)=0,
 @weight  decimal(18,8)=0,
 @reorder bit = 0, 
 @buyitem bit = 0, 
 @obsolete bit = 0 , 
 @cyclecount bit = 0, 
 @notes varchar(Max) = '', 
 @minimum decimal = 0.0, 
 @maximum decimal = 0.0, 
 @leadtime decimal = 0.0, 
 @decimalqty decimal = 0.0, 
 @warehouseidv varchar (50) = '', 
 @locationsidv varchar (50) = '', 
 @consignment bit = 0, 
 @freight  decimal(18,8)=0,
 @taxable bit = 0, 
 @imgpath varchar (50) = '', 
 @glsales varchar (50) = '', 
 @glinv varchar (50) = '', 
 @glcogs varchar (50) = '', 
 @dwgno varchar (50) = '', 
 @user1 varchar (50) = '', 
 @user2 varchar (50) = '', 
 @user3  decimal(18,8)=0,
 @userbit bit = 0, 
 @userbit2 bit = 0, 
 @userbit3 bit = 0, 
 @createdby varchar (50) = '', 
 @createddate datetime  = '', 
 @costed bit = 0, 
 @flag_delete bit = 0,
 @parent_updated bit = 0, 
 @bom_released bit =0,
 @retmsg varchar(100)= '' OUTPUT, 
 @inactive bit =0,
 @user4 varchar (50) = '', 
 @user5 varchar (50) = '', 
 @user6 varchar (50) = '', 
 @user7 varchar (50) = '', 
 @user8 varchar (50) = '', 
 @user9 varchar (50) = '', 
 @user10 varchar (50) = '', 
 @user11 varchar (50) = '', 
 @user12 varchar (50) = '', 
 @user13 varchar (50) = '', 
 @user14 varchar (50) = '', 
 @user15 varchar (50) = ''
 AS 
 Declare  
 @invtypeid uniqueidentifier, 
 @itemtypeid uniqueidentifier, 
 @itemtypeidv2 varchar(50), 
 @itemclassid uniqueidentifier,
 @itemclassidv2 varchar(50), 
 @itemcodeid uniqueidentifier, 
 @itemcodeidv2 varchar(50), 

 @locationsid uniqueidentifier, 
 @Deleteflag smallint ,
 @text1 varchar(max),
 @text2 varchar(50),
 @list	varchar(max),
 @text varchar(max),
 @strguid varchar(50),
 @curcost money
 BEGIN 
	set @itemnumber=upper(@itemnumber)
	set @description =upper(@description)
	set @createddate=getdate()
	set @list=''
	if @id is null
	set @id=(select id from list_items where itemnumber=@itemnumber and rev=@rev)
	if @id is null
	set @id=(select NEWID())
	set @curcost=(select cost from list_items where itemnumber=@itemnumber and rev=@rev)
	if @itemclassidv='' set @itemclassid= null else set @itemclassid=(select ID from list_itemclass where itemclass=@itemclassidv)
	if @invtypeidv='' set @invtypeid= null else set @invtypeid=(select ID from list_invtypes where invtype=@invtypeidv)
	if @itemtypeidv='' set @itemtypeid= null else set @itemtypeid=(select ID from list_itemtypes where itemtype=@itemtypeidv)
	if @itemcodeidv='' set @itemcodeid= null else set @itemcodeid=(select ID from list_itemcodes where itemcode=@itemcodeidv)
	set @locationsid=(select dbo.amics_fn_api_warehouseid(@warehouseidv,@locationsidv))
	if @actionflag=1 
	begin 
 		Insert into dbo.list_items	(id,itemnumber, rev, description, SalesDescription, PurchaseDescription, invtypeid, 
 									 itemtypeid, itemclassid, itemcodeid, uomid,   cost, 
 									 markup, price2, price3, weight, reorder, buyitem, obsolete, cyclecount, 
 									 notes, minimum, maximum, leadtime, decimalqty, locationsid, consignment, freight, 
 									 taxable, imgpath, glsales, glinv, glcogs, dwgno, user1, user2, user3, userbit, userbit2,userbit3,createdby, 
 									 createddate, costed, parent_updated,bom_released,inactive,user4,user5,user6,user7,user8,user9,user10,user11,user12,user13,user14,user15) 
 						values		(@id,@itemnumber, @rev , @description , @SalesDescription , @PurchaseDescription , @invtypeid , 
 									@itemtypeid , @itemclassid , @itemcodeid , @uomid ,   @cost , 
 									@markup  , @price2 , @price3 , @weight  , @reorder  , @buyitem , @obsolete  , @cyclecount , 
 									@notes , @minimum , @maximum , @leadtime , @decimalqty , @locationsid , @consignment , @freight  , 
 									@taxable , @imgpath , @glsales , @glinv , @glcogs , @dwgno , @user1 , @user2 , @user3 , @userbit, @userbit2 ,@userbit3 , 
 									@createdby , @createddate  , 1 , @parent_updated,@bom_released,@inactive,@user4,@user5,@user6,@user7,@user8,@user9,@user10,@user11,@user12,@user13,@user14,@user15 ) 
	end
	else
	begin	
		set @text1='Update list_items set '
		set @text2=' where id='+''''+CONVERT(varchar(255), @id)+''''
		set @itemclassidv2=isnull((select itemclass from list_items inner join list_itemclass on list_items.itemclassid=list_itemclass.id where list_items.id=@id),'')
		set @itemcodeidv2=isnull((select itemcode from list_items inner join list_itemcodes on list_items.itemclassid=list_itemcodes.id where list_items.id=@id),'')
		set @itemtypeidv2=isnull((select itemtype from list_items inner join list_itemtypes on list_items.itemtypeid=list_itemtypes.id where list_items.id=@id),'')
		if @itemnumber<>(select itemnumber from list_items where id=@id)					set @list=@list+',itemnumber='+''''+@itemnumber+''''
		if @rev<>(select rev from list_items where id=@id)									set @list=@list+',rev='+''''+@rev+''''
		if @description<>(select description from list_items where id=@id)					set @list=@list+',description='+''''+@description+''''
		if @SalesDescription <> (select isnull(salesdescription,'') from list_items where id=@id)		set @list=@list+',salesdescription='+''''+@salesdescription+''''
		if @PurchaseDescription <>(select isnull(PurchaseDescription,'') from list_items where id=@id) set @list=@list+',PurchaseDescription='+''''+@PurchaseDescription+''''
		if @imgpath <>(select imgpath from list_items where id=@id)							set @list=@list+',imgpath='+''''+@imgpath+''''
		if @glsales <>(select isnull(glsales,'') from list_items where id=@id)							set @list=@list+',glsales='+''''+@glsales+''''
		if @glinv <>(select isnull(glinv,'') from list_items where id=@id)								set @list=@list+',glinv='+''''+@glinv+''''
		if @glcogs <>(select isnull(glcogs,'') from list_items where id=@id)							set @list=@list+',glcogs='+''''+@glcogs+''''
		if @dwgno <>(select dwgno from list_items where id=@id)								set @list=@list+',dwgno='+''''+@dwgno+''''
		if @user1 <>(select user1 from list_items where id=@id)								set @list=@list+',user1='+''''+@user1+''''
		if @user2 <>(select user2 from list_items where id=@id)								set @list=@list+',user2='+''''+@user2+''''

		if @conversion <>(select conversion from list_items where id=@id)					set @list=@list+',conversion='+''''+convert(varchar(50), @conversion)+''''
		if @cost <>(select isnull(cost,0) from list_items where id=@id)								set @list=@list+',cost='+''''+convert(varchar(50), @cost)+''''
		if @markup <>(select isnull(markup,0) from list_items where id=@id)							set @list=@list+',markup='+''''+convert(varchar(50), @markup)+''''
		--if @price <>(select isnull(price,0) from list_items where id=@id)								set @list=@list+',price='+''''+convert(varchar(50), @price)+''''
		if @price2 <>(select isnull(price2,0) from list_items where id=@id)							set @list=@list+',price2='+''''+convert(varchar(50), @price2)+''''
		if @price3 <>(select isnull(price3,0) from list_items where id=@id)							set @list=@list+',price3='+''''+convert(varchar(50), @price3)+''''
		if @weight <>(select weight from list_items where id=@id)							set @list=@list+',weight='+''''+convert(varchar(50), @weight)+''''
		if @minimum <>(select minimum from list_items where id=@id)							set @list=@list+',minimum='+''''+convert(varchar(50), @minimum)+''''
		if @maximum <>(select maximum from list_items where id=@id)							set @list=@list+',maximum='+''''+convert(varchar(50), @maximum)+''''
		if @leadtime <>(select leadtime from list_items where id=@id)						set @list=@list+',leadtime='+''''+convert(varchar(50), @leadtime)+''''
		if @decimalqty <>(select decimalqty from list_items where id=@id)					set @list=@list+',decimalqty='+''''+convert(varchar(50), @decimalqty)+''''
		if @freight <>(select freight from list_items where id=@id)							set @list=@list+',freight='+''''+convert(varchar(50), @freight)+''''
		if @user3 <>(select user3 from list_items where id=@id)								set @list=@list+',user3='+''''+convert(varchar(50), @user3)+''''
		if @user4 <>(select user4 from list_items where id=@id) set @list=@list+',user4='+''''+@user4+''''
		if @user5 <>(select user5 from list_items where id=@id) set @list=@list+',user5='+''''+@user5+''''
		if @user6 <>(select user6 from list_items where id=@id) set @list=@list+',user6='+''''+@user6+''''
		if @user7 <>(select user7 from list_items where id=@id) set @list=@list+',user7='+''''+@user7+''''
		if @user8 <>(select user8 from list_items where id=@id) set @list=@list+',user8='+''''+@user8+''''
		if @user9 <>(select user9 from list_items where id=@id) set @list=@list+',user9='+''''+@user9+''''
		if @user10 <>(select user10 from list_items where id=@id) set @list=@list+',user10='+''''+@user10+''''
		if @user11 <>(select user11 from list_items where id=@id) set @list=@list+',user11='+''''+@user11+''''
		if @user12 <>(select user12 from list_items where id=@id) set @list=@list+',user12='+''''+@user12+''''
		if @user13 <>(select user13 from list_items where id=@id) set @list=@list+',user13='+''''+@user13+''''
		if @user14 <>(select user14 from list_items where id=@id) set @list=@list+',user14='+''''+@user14+''''
		if @user15 <>(select user15 from list_items where id=@id) set @list=@list+',user15='+''''+@user15+''''


		if @cyclecount <>(select isnull(cyclecount,0) from list_items where id=@id)						set @list=@list+',cyclecount='+''''+CONVERT(varchar(1), @cyclecount)+''''
		if @userbit <>(select isnull(userbit,0) from list_items where id=@id)							set @list=@list+',userbit='+''''+CONVERT(varchar(1), @userbit)+''''
		if @userbit2 <>(select isnull(userbit2,0) from list_items where id=@id)							set @list=@list+',userbit2='+''''+CONVERT(varchar(1), @userbit2)+''''
		if @userbit3 <>(select isnull(userbit3,0) from list_items where id=@id)							set @list=@list+',userbit3='+''''+CONVERT(varchar(1), @userbit3)+''''
		if @taxable <>(select isnull(taxable,0) from list_items where id=@id)							set @list=@list+',taxable='+''''+CONVERT(varchar(1), @taxable)+''''
		if @consignment <>(select isnull(consignment,0) from list_items where id=@id)					set @list=@list+',consignment='+''''+CONVERT(varchar(1), @consignment)+''''
		if @reorder <>(select isnull(reorder,0) from list_items where id=@id)							set @list=@list+',reorder='+''''+CONVERT(varchar(1), @reorder)+''''
		if @buyitem <>(select isnull(buyitem,0) from list_items where id=@id)							set @list=@list+',buyitem='+''''+CONVERT(varchar(1), @buyitem)+''''
		if @bom_released <>(select isnull(bom_released,0) from list_items where id=@id)					set @list=@list+',bom_released='+''''+CONVERT(varchar(1), @bom_released)+''''
		if @obsolete <>(select isnull(obsolete,0) from list_items where id=@id)							set @list=@list+',obsolete='+''''+CONVERT(varchar(1), @obsolete)+''''
		if @inactive <>(select isnull(inactive,0) from list_items where id=@id)							set @list=@list+',inactive='+''''+CONVERT(varchar(1), @inactive)+''''
		if @flag_delete <>(select isnull(flag_delete,0) from list_items where id=@id)					set @list=@list+',flag_delete='+''''+CONVERT(varchar(1), @flag_delete)+''''

		if @invtypeid<>(select invtypeid from list_items where id=@id) or 	((select invtypeid from list_items where id=@id) is null)						
			set @list=@list+',invtypeid='+''''+CONVERT(varchar(255), @invtypeid)+''''
		if @itemtypeidv<>@itemtypeidv2 and @itemtypeidv<>'' set @list=@list+',itemtypeid='+''''+CONVERT(varchar(255), @itemtypeid)+'''' else 
			if @itemtypeidv='' set @list=@list+',itemtypeid=null' else set @list=@list
		if @itemclassidv<>@itemclassidv2 and @itemclassidv<>'' set @list=@list+',itemclassid='+''''+CONVERT(varchar(255), @itemclassid)+'''' else 
			if @itemclassidv='' set @list=@list+',itemclassid=null' else set @list=@list 
		if @itemcodeidv<>@itemcodeidv2 and @itemcodeidv<>'' set @list=@list+',itemcodeid='+''''+CONVERT(varchar(255), @itemcodeid)+'''' else 
			if @itemcodeidv='' set @list=@list+',itemcodeid=null' else set @list=@list 
		if @locationsid<>(select locationsid from list_items where id=@id) or((select locationsid from list_items where id=@id) is null)						
			set @list=@list+',locationsid='+''''+CONVERT(varchar(255), @locationsid)+''''
		if @uomid<>(select uomid from list_items where id=@id) or	((select uomid from list_items where id=@id) is null)						
			set @list=@list+',uomid='+''''+CONVERT(varchar(255), @uomid)+''''
		if @list<>'' exec(@text1+'[createdby]='+''''+@createdby+''''+@list+@text2) else print 'Nothing to print'
		if @curcost<>@cost  exec dbo.amics_sp_api_costing_single @childid=@id
	end
 END 
 GO
 ----------------------------------------------------------------------------------------------------------------------------------------

 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_load_partmaster]    Script Date: 12-07-2022 09:58:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_load_partmaster] @item varchar(50) = '', @rev varchar(50) = ''          
AS          
 declare @childid varchar(50)          
          
set NOCOUNT ON          
        
Begin          
SELECT     TOP (100) PERCENT dbo.list_items.id,dbo.list_items.itemnumber, dbo.list_items.rev, dbo.list_items.dwgno, ISNULL(dbo.list_itemtypes.itemtype, ' ') AS itemtype,           
                      ISNULL(dbo.list_itemclass.itemclass, ' ') AS itemclass, ISNULL(dbo.list_itemcodes.itemcode, ' ') AS itemcode, ISNULL(dbo.list_items.cost, 0.00)           
                      AS cost, dbo.list_items.markup, ISNULL(dbo.list_items.price, 0.00) AS price, ISNULL(dbo.list_items.price2, 0.00) AS price2, ISNULL(dbo.list_items.weight, 0.00) AS weight,           
                      ISNULL(dbo.list_items.price3, 0.00) AS price3, ISNULL(dbo.list_items.Description,'') as Description,ISNULL(dbo.list_items.SalesDescription, '') AS SalesDescription, dbo.list_uoms.uomref,           
                      dbo.list_uoms.conversion, dbo.list_uoms.uom1 AS Purchasing_UOM, dbo.list_items.leadtime, ISNULL(dbo.list_items.PurchaseDescription, '')           
                      AS PurchaseDescription, dbo.list_items.minimum, dbo.list_items.maximum, dbo.list_items.glsales, dbo.list_items.glinv, dbo.list_items.glcogs,           
                      dbo.list_items.userbit,dbo.list_items.userbit2,dbo.list_items.notes, dbo.list_locations.location, dbo.list_warehouses.warehouse, dbo.list_items.buyitem, dbo.list_items.obsolete,           
                      ISNULL(dbo.list_invtypes.invtype, ' ') AS InvType, list_items_2.itemnumber AS Child_ItemNumber, list_items_2.rev AS Child_Rev,           
                      list_items_2.description AS Child_Description, list_uoms_1.uomref AS Child_Uom, list_items_2.cost AS Child_Cost, dbo.items_bom.linenum,           
                      ISNULL(dbo.items_bom.quantity, 0.0) AS quantity, dbo.list_items.user1 , dbo.list_items.user2, ISNULL(dbo.list_items.user3, 0.0) as user3, dbo.list_items.user4, dbo.list_items.user5  
                      , dbo.list_items.user6, dbo.list_items.user7, dbo.list_items.user8, dbo.list_items.user9, dbo.list_items.user10, dbo.list_items.user11, dbo.list_items.user12, dbo.list_items.user13  
                      , dbo.list_items.user14, dbo.list_items.user15       
FROM         dbo.list_warehouses RIGHT OUTER JOIN          
                      dbo.list_items AS list_items_2 LEFT OUTER JOIN          
                      dbo.list_uoms AS list_uoms_1 ON list_items_2.uomid = list_uoms_1.id RIGHT OUTER JOIN          
                      dbo.items_bom ON list_items_2.id = dbo.items_bom.itemsid_child RIGHT OUTER JOIN          
                      dbo.list_items ON dbo.items_bom.itemsid_parent = dbo.list_items.id LEFT OUTER JOIN          
                      dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id LEFT OUTER JOIN          
                      dbo.list_itemclass ON dbo.list_items.itemclassid = dbo.list_itemclass.id LEFT OUTER JOIN          
                      dbo.list_itemcodes ON dbo.list_items.itemcodeid = dbo.list_itemcodes.id LEFT OUTER JOIN          
                      dbo.list_uoms ON dbo.list_items.uomid = dbo.list_uoms.id LEFT OUTER JOIN          
                      dbo.list_invtypes ON dbo.list_items.invtypeid = dbo.list_invtypes.id LEFT OUTER JOIN          
                      dbo.list_locations ON dbo.list_items.locationsid = dbo.list_locations.id ON dbo.list_warehouses.id = dbo.list_locations.warehousesid          
WHERE     (dbo.list_items.itemnumber = @item) AND (dbo.list_items.rev = @rev)          
      
End  
GO
--------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_invbasic]    Script Date: 13-07-2022 02:28:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_update_invbasic]
	@Invbasicid uniqueidentifier,
	@Locationid uniqueidentifier,
	@Itemid uniqueidentifier,
	@Cost decimal(18,8),
	@Quantity decimal(18,8),
	@Lotno varchar(50),
	@Color varchar(50),
	@user varchar(50),
	@exp smalldatetime=null,
	@sources_refid uniqueidentifier,
	@lic_plate varchar(50)=''
AS
BEGIN
	declare	@Invbasicid_current uniqueidentifier,@quantity_new decimal(18,8),@quantity_prior decimal(18,8),@solinesid uniqueidentifier,@er varchar(50) 
	if @sources_refid is null
	set @invbasicid_current=(select id from inv_basic where itemsid=@itemid and locationsid=@locationid and cost=@cost and isnull(lotno,'')=isnull(@lotno,'') and isnull(color,'')=isnull(@color,'') and isnull(lic_plate,'')=isnull(@lic_plate,'') and sources_refid is null)	
	else
	set @invbasicid_current=(select id from inv_basic where itemsid=@itemid and locationsid=@locationid and cost=@cost and isnull(lotno,'')=isnull(@lotno,'') and isnull(color,'')=isnull(@color,'') and isnull(lic_plate,'')=isnull(@lic_plate,'')  and sources_refid=@sources_refid)

	if @invbasicid_current is null
	begin
		set @solinesid=(select so_linesid from po_lines where id=@sources_refid)
		if @solinesid is null set @solinesid=@sources_refid 
		set @er=(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@solinesid)
		insert into inv_basic (id,locationsid,itemsid,color,cost,quantity,lotno,createdby,expdate,sources_refid,lic_plate,so_linesid,er) values 
			(@invbasicid,@locationid,@itemid,@color,@cost,@quantity,@lotno,@user,@exp,@sources_refid,@lic_plate,@solinesid,@er)
	end
	else
		begin
			set @quantity_prior=(select quantity from inv_basic where id=@invbasicid_current)
			set @quantity_new=@quantity+isnull(@quantity_prior,0)
			set @solinesid=(select so_linesid from inv_basic where id=@Invbasicid_current)
			set @er=(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@solinesid)
			update inv_basic set quantity=@quantity_new,expdate=@exp,er=@er where id=@invbasicid_current
		end
END
GO
--------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_invbasic_r2]    Script Date: 13-07-2022 02:32:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_update_invbasic_r2]
	@Invbasicid uniqueidentifier,
	@Locationid uniqueidentifier,
	@Itemid uniqueidentifier,
	@Cost decimal(18,8),
	@Quantity decimal(18,8),
	@Lotno varchar(50),
	@Color varchar(50),
	@user varchar(50),
	@exp smalldatetime=null,
	@sources_refid uniqueidentifier,
	@lic_plate varchar(50)='',
	@extid uniqueidentifier
	
AS
BEGIN
	declare	@Invbasicid_current uniqueidentifier,@quantity_new decimal(18,8),@quantity_prior decimal(18,8),@solinesid uniqueidentifier
	if @sources_refid is null
		set @invbasicid_current=(select id from inv_basic where itemsid=@itemid and locationsid=@locationid and cost=@cost and isnull(lotno,'')=isnull(@lotno,'') and isnull(color,'')=isnull(@color,'') and isnull(lic_plate,'')=isnull(@lic_plate,'') and sources_refid is null)	
	else
		set @invbasicid_current=(select id from inv_basic where itemsid=@itemid and locationsid=@locationid and cost=@cost and isnull(lotno,'')=isnull(@lotno,'') and isnull(color,'')=isnull(@color,'') and isnull(lic_plate,'')=isnull(@lic_plate,'')  and sources_refid=@sources_refid)

	if @invbasicid_current is null
		begin
			set @solinesid=(select so_linesid from po_lines where id=@sources_refid)
			insert into inv_basic (id,locationsid,itemsid,color,cost,quantity,lotno,createdby,expdate,sources_refid,lic_plate,extendedid,so_linesid) values 
				(@invbasicid,@locationid,@itemid,@color,@cost,@quantity,@lotno,@user,@exp,@sources_refid,@lic_plate,@extid,@solinesid)
		end
	else
		begin
			set @quantity_prior=(select quantity from inv_basic where id=@invbasicid_current)
			set @quantity_new=@quantity+isnull(@quantity_prior,0)
			update inv_basic set quantity=@quantity_new,expdate=@exp where id=@invbasicid_current
		end
END
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_transloglot]    Script Date: 13-07-2022 02:33:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_update_transloglot]
	@translogid	uniqueidentifier,
	@quantity	decimal(18,8),
	@lotno		varchar(50),
	@color		varchar(50),
	@user		varchar(50),
	@cost		decimal(18,8),
	@loc		varchar(50),
	@whs		varchar(50),
	@lic_plate	varchar(50)=null,
	@lic_plate_flag bit=NULL
	
AS
BEGIN
insert into transloglot (translogid,Lotno,color,quantity,Createdby,cost,location,warehouse,lic_plate,lic_plate_flag) values 
	(@translogid,@lotno,@color,@quantity,@user,@cost,@loc,@whs,@lic_plate,@lic_plate_flag)
END
GO

----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_invserial]    Script Date: 13-07-2022 02:34:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amics_sp_api_update_invserial]
	@itemid uniqueidentifier,
	@receiptsid uniqueidentifier,
	@locationid uniqueidentifier,
	@rec_translogid2 uniqueidentifier,
	@cost money,
	@rec_user2 varchar(50),
	@transnum int,
	@lic_plate_flag bit=0,
	@sourcesrefid  uniqueidentifier
 AS
BEGIN
	declare @rec_serno varchar(50),@rec_tagno varchar(50),@rec_model varchar(50),
	@loc1 varchar(50),@whs1 varchar(50),@serid uniqueidentifier,@lic_plate varchar(50)
	,@somainid uniqueidentifier
	set @loc1	=(select location from list_locations where id=@locationid)
	set @whs1	=(SELECT dbo.list_warehouses.warehouse FROM dbo.list_warehouses RIGHT OUTER JOIN dbo.list_locations 
					ON dbo.list_warehouses.id = dbo.list_locations.warehousesid WHERE     dbo.list_locations.id = @locationid)
	declare cuMyCursor Cursor For (select id,serno,tagno,model from 
		inv_serlot where transnum=@transnum )
	Open cuMyCursor
	Fetch Next from cuMyCursor into @serid,@rec_serno,@rec_tagno,@rec_model
	while @@fetch_status = 0
	begin 
		-- set @serid=NEWID() -- Changed to use the same as inv_serlot.id 05/05/2012
		set @somainid=(select somainid from so_lines  where so_lines.id=@sourcesrefid)
		insert into inv_serial (id,itemsid,Receiptsid,Locationsid,cost,serno,tagno,model,createdby,so_linesid,so_mainid ) values
			(@serid,@itemid,@receiptsid,@locationid,@cost,@rec_serno,@rec_tagno,@rec_model,@rec_user2,@sourcesrefid,@somainid )
		set @lic_plate='S-'+(select CAST(lic_plate_number AS VARCHAR(45)) from inv_serial where ID=@serid)
		exec	amics_sp_api_update_translogsn
				@translogid		=	@rec_translogid2,
				@serno			=	@rec_serno,
				@tagno			=	@rec_tagno,
				@model			=	@rec_model,
				@user			=	@rec_user2,
				@cost			=	@cost,
				@loc			=	@loc1,
				@whs			=	@whs1,
				@lic_plate		=	@lic_plate,
				@lic_plate_flag =	@lic_plate_flag
		Fetch Next from cuMyCursor into @serid,@rec_serno,@rec_tagno,@rec_model

	end 
	close cuMyCursor
	deallocate cuMyCursor
END
GO
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_translog]    Script Date: 13-07-2022 02:34:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amics_sp_api_update_translog]
	@translogid		uniqueidentifier,
	@itemid			uniqueidentifier,
	@Invtype		varchar(50),
	@Itemtype		varchar(50),
	@Rev			varchar(50),
	@trans			varchar(50),
	@item			varchar(50),
	@Description	varchar(75), 
	@Warehouse		varchar(50),
	@Location		varchar(50),
	@Reason			varchar(50),
	@Source			varchar(50),
	@Ref			varchar(50),
	@notes			varchar(250),
	@Quantity		decimal(18,8),
	@Cost			decimal(18,8),
	@transdate		smalldatetime,
	@user			varchar(50),
	@lic_plate		varchar(50)=null,
	@lic_plate_flag bit=null,
	@recid			uniqueidentifier=null,
	@userfield2			varchar(50)=''
	
AS
BEGIN
	declare @curqty decimal(18,8)
if @Invtype='serial'
set @curqty=(select SUM(quantity) from inv_serial where itemsid=@itemid)
else 
set @curqty=(select SUM(quantity) from inv_basic where itemsid=@itemid)
insert into translog(id,Trans,itemsid,itemnumber,description,
	Warehouse,Location,itemtype,invtype,Reason,Source,
	Ref,revision,Notes,transqty,cost,transdate,CreatedDate,
	modifieddate,createdby,lic_plate,lic_plate_flag,inv_receiptsid,current_qty,user2) values 
	(@translogid,@trans,@itemid,@item,@description,
	@warehouse,@location,@itemtype,@invtype,@reason,@source,
	@ref,@rev,@notes,@quantity,@cost,@transdate,getdate(),getdate(),@user,@lic_plate,@lic_plate_flag,@recid,@curqty,@userfield2)

END
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_inv_somain_details_items]    Script Date: 13-07-2022 02:36:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_inv_somain_details_items] @solinesid2 uniqueidentifier
    --@somain varchar(50),
    --@itemnumber varchar(50)
AS
BEGIN
declare @itemnumber varchar(50)

set @itemnumber=(select itemnumber from so_lines where id=@solinesid2 )
	--- BASIC CALCULATIONS START----
	if (select top 1 invtype from list_items inner join list_invtypes on list_items.invtypeid=list_invtypes.id where itemnumber=@itemnumber ) = 'BASIC'
	SELECT dbo.list_warehouses.warehouse, NULL AS serialid, dbo.inv_basic.id as basicid, dbo.inv_basic.cost, 
	0 AS sncost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, 
	dbo.so_lines.quantity, dbo.list_locations.location, dbo.inv_basic.quantity AS available, dbo.so_main.somain, 
	dbo.so_lines.id AS solinesid, '' AS serno, '' AS tagno, SUM(SUM(ISNULL(dbo.inv_basic.quantity, 0))) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'


	FROM            dbo.list_items LEFT OUTER JOIN
							 dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id RIGHT OUTER JOIN
							 dbo.list_warehouses RIGHT OUTER JOIN
							 dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid RIGHT OUTER JOIN
							 dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid RIGHT OUTER JOIN
							 dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id RIGHT OUTER JOIN
							 dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id
	GROUP BY dbo.list_warehouses.warehouse, dbo.inv_basic.id, dbo.inv_basic.cost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, dbo.so_lines.quantity, 
							 dbo.list_locations.location, dbo.inv_basic.quantity, dbo.so_main.somain, dbo.so_lines.id
	HAVING        so_lines.id=@solinesid2  and inv_basic.quantity>0
--	HAVING        (dbo.list_items.itemnumber = @itemnumber) AND (dbo.so_main.somain = @somain) and inv_basic.quantity>0
	--- BASIC CALCULATIONS END
	else
	begin -- ELSE SERIAL CALCULATIONS START ---
	-- UNION statement is used for serial items to include both from solinesid and transferid in inv_serial
		SELECT        TOP (100) PERCENT dbo.list_warehouses.warehouse, dbo.inv_serial.id AS serialid, NULL AS basicid, 
				0 AS cost, dbo.inv_serial.cost AS sncost, dbo.so_lines.linenum,dbo.list_itemtypes.itemtype,
								  dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, list_locations.location, ISNULL(dbo.inv_serial.quantity, 0) 
								 AS Available, dbo.so_main.somain, dbo.so_lines.id AS solinesid, ISNULL(dbo.inv_serial.serno, '') AS serno, ISNULL(dbo.inv_serial.tagno, '') AS tagno,
			   SUM(dbo.inv_serial.quantity) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
		from so_main inner join 
		 so_lines on so_main.id=so_lines.somainid left join 
		 list_items on so_lines.itemsid=list_items.id left join 
		 list_itemtypes on list_items.itemtypeid=list_itemtypes.id left join 
		 inv_serial on so_lines.id=case when inv_serial.transferid is null then inv_serial.so_linesid else inv_serial.transferid end inner join 
		 list_locations on inv_serial.locationsid=list_locations.id left join 
		 list_warehouses on list_locations.warehousesid = list_warehouses.id where so_lines.id=@solinesid2  and inv_serial.quantity>0		
--		 list_warehouses on list_locations.warehousesid = list_warehouses.id where somain=@somain  and so_lines.itemnumber = @itemnumber and inv_serial.quantity>0		
		-- Commented on 022416
		--select * from (
		--SELECT     TOP (100) PERCENT 
		--	dbo.list_warehouses.warehouse,dbo.inv_serial.id as serialid,  null as basicid,
		--	0 as cost,dbo.inv_serial.cost as sncost,dbo.so_lines.linenum,dbo.list_itemtypes.itemtype,
		--	dbo.so_lines.itemnumber,dbo.so_lines.description,dbo.so_lines.quantity,list_locations_1.location AS location,
		--	ISNULL(dbo.inv_serial.quantity, 0) AS Available,dbo.so_main.somain,dbo.so_lines.id AS solinesid,
		--	ISNULL(dbo.inv_serial.serno, '') AS serno,ISNULL(dbo.inv_serial.tagno, '') AS tagno,
		--	SUM(dbo.inv_serial.quantity) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
		--FROM   -- inv_basic join is doing nothing
		--	dbo.list_locations AS list_locations_1 RIGHT OUTER JOIN
		--	dbo.list_itemtypes RIGHT OUTER JOIN
		--	dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid left OUTER JOIN
		--	--dbo.inv_basic RIGHT OUTER JOIN
		--	--dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id LEFT OUTER JOIN
		--	--dbo.inv_serial ON 
		--	--CASE WHEN inv_serial.transferid IS NOT NULL THEN inv_serial.transferid ELSE inv_serial.so_linesid END = dbo.so_lines.id 
		--	--dbo.so_lines.id = dbo.inv_serial.so_linesid 
		--	--ON dbo.list_items.id = dbo.so_lines.itemsid RIGHT OUTER JOIN
		--	dbo.inv_serial ON dbo.list_items.id = dbo.inv_serial.itemsid RIGHT OUTER JOIN						
		--	dbo.so_lines ON dbo.so_lines.id = dbo.inv_serial.transferid 
		--	or (dbo.so_lines.id = dbo.inv_serial.so_linesid and dbo.inv_serial.transferid is null) RIGHT OUTER JOIN	
			
		--	dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id ON list_locations_1.id = dbo.inv_serial.locationsid RIGHT OUTER JOIN
		--	dbo.list_warehouses ON  list_locations_1.warehousesid =  dbo.list_warehouses.id
		--GROUP BY dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
		--	dbo.so_main.somain, dbo.inv_serial.pickid, dbo.so_lines.id,ISNULL(dbo.inv_serial.serno, ''), ISNULL(dbo.inv_serial.tagno, ''),
		--	dbo.inv_serial.quantity,dbo.list_warehouses.warehouse, list_locations_1.location,  
		--	dbo.inv_serial.id,dbo.inv_serial.cost,inv_serial.transferid

		--HAVING (dbo.so_main.somain = @somain) AND (dbo.so_lines.quantity > 0) AND inv_serial.transferid is null and 
		--	ISNULL(dbo.inv_serial.quantity, 0) >0 AND 	dbo.so_lines.itemnumber= @itemnumber
		--	ORDER BY dbo.so_lines.linenum) as x
		-- End of Commented on 022416
			
	END  -- ELSE SERIAL CALCULATIONS END ---
END -- END OF PROCEDURE
GO

-----------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_costing_single]    Script Date: 13-07-2022 04:40:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_costing_single]
@childid uniqueidentifier
AS
BEGIN
	declare @parentid uniqueidentifier, @nqty decimal(18,8),@routcost money, @costed bit,@allcosted bit,
	@id_to_cost uniqueidentifier,@newcost money,@CONV decimal(18,8),@childcost decimal(18,8)
	update list_items set costed=0 where id in (select itemsid_parent from items_bom where itemsid_child=@childid )
	while (select top 1 id from list_items where costed=0) is not null
	begin
		declare cutocost cursor for (select id from list_items where costed=0)
		open cutocost
		fetch next from cutocost into @id_to_cost
		while @@fetch_status=0 
		begin
			set @allcosted=1
			set @newcost=0
			declare cuChildren cursor for (select itemsid_child,quantity  from items_bom where itemsid_parent=@id_to_cost ) 
			open cuChildren 
			Fetch Next from cuChildren into @childid,@nqty
			while @@fetch_status=0 
			begin 
				if (select costed from list_items where  id=@childid)=0 set @allcosted=0
				set @conv=(select isnull(conversion,1) from list_uoms where list_uoms.id=(select uomid from list_items where id=@childid))
				set @childcost =(select isnull(cost,0) from list_items where id=@childid)
				set @newcost=@newcost+(@childcost*@nqty/@conv)
				Fetch Next from cuchildren into @childid,@nqty
			end	
			close cuChildren
 			deallocate cuchildren
 			if @allcosted=1
 			begin
				set @newcost=(select dbo.amics_fn_api_bomcost (@id_to_cost))
 				update list_items set cost= @newcost,costed=1 where id=@id_to_cost 
 				update list_items set costed=0 where id in (select itemsid_parent from items_bom where itemsid_child=@id_to_cost )
 			end
			fetch next from cutocost into @id_to_cost
		end
		close cutocost
		deallocate cutocost	
 	end	
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_invreceipts]    Script Date: 13-07-2022 08:06:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_update_invreceipts]
	@invtypeid uniqueidentifier,
	@invreceiptsid uniqueidentifier,
	@sourcesid uniqueidentifier,
	@sourcesrefid uniqueidentifier,
	@locationid uniqueidentifier,
	@invbasicid uniqueidentifier,
	@quantity decimal(18,8),
	@transdate smalldatetime,
	@user varchar(50),
	@recnum smallint=0,
	@recnotes varchar(max)=''
AS
BEGIN

	insert into inv_receipts (id,sourcesid,sources_refid,recd_locationid,inv_basicid,recd_quantity,trans_date,createdby,RECEIVER,recd_notes  ) values 
		(@invreceiptsid,@sourcesid,@sourcesrefid,@locationid,@invbasicid,@quantity,@transdate,@user,@recnum,@recnotes  )

END
GO
---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_process_change_sertag]    Script Date: 13-07-2022 08:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_process_change_sertag]    
 @serialid uniqueidentifier,@serno_prior varchar(50),@serno_current varchar(50),    
 @tagno_prior varchar(50),@tagno_current varchar(50),@memo varchar(max),@createdby varchar(50),
 @model_prior varchar(50),@model_current varchar(50),
 @cost_prior decimal(18,8),@cost_current decimal(18,8)   

AS    

BEGIN    

 declare @newid1 uniqueidentifier,  -- @newid2 uniqueidentifier,    
 @itemsid uniqueidentifier,@locationsid uniqueidentifier,@revision varchar(30),    
 @itemtype varchar(50),@item varchar(50),@description varchar(250),    
 @serwarehouse varchar(50),@serlocation varchar(50),    
 @sercost decimal(18,8),@curdate smalldatetime,@user2 varchar(50)    
     
 set @newid1=(select newid())    
-- set @newid2=(select newid())    
 set @curdate=getdate()    
    
 set @itemsid=(select itemsid from inv_serial where inv_serial.id=@serialid)    
 SET @ITEM=(Select itemnumber from list_items where list_items.id=@itemsid)     
 set @revision=(Select rev from list_items where list_items.id=@itemsid)     
 SET @Description=(Select description from list_items where list_items.id=@itemsid)     
 set @itemtype=(select itemtype from list_items inner join list_itemtypes    
  on list_items.itemtypeid=list_itemtypes.id  where list_items.id=@itemsid)    
 set @locationsid=(select locationsid from inv_serial where inv_serial.id=@serialid)    
         
 set @serwarehouse=(select warehouse from list_locations inner join     
  list_warehouses on list_locations.warehousesid=list_warehouses.id where     
  list_locations.id=@locationsid)     
      
 set @serlocation=(select location from list_locations where list_locations.id=@locationsid)        
 set @sercost=(select cost from inv_serial where inv_serial.id=@serialid)     
 set @user2=(select dbo.amics_fn_api_er(null,@serialid)) 
    
 set @memo=char(13)+'Changed SN From:   '+ @serno_prior+' To: '+@serno_current+char(13)    
    +'  Changed TAG From:   '+@tagno_prior+' To: '+@tagno_current+char(10)+char(13)    
    +@memo    
     
 update inv_serial set serno=@serno_current, tagno=@tagno_current, model=@model_current, cost=@cost_current where id=@serialid   
  
 if (select id from inv_serial where serno=@serno_current and tagno=@tagno_current) is not null    
  begin    
   exec amics_sp_api_update_translog     
    @translogid  = @newid1,    
    @itemid   = @itemsid,    
    @Invtype  = 'SERIAL',    
    @rev=@revision,    
    @Itemtype  = @itemtype,    
    @trans   = 'CHG SERIAL/TAG',    
    @item   = @ITEM ,    
    @Description = @description,     
    @Warehouse  = @serwarehouse,    
    @Location  = @serlocation,    
    @Reason   = '',    
    @Source   = '',    
    @Ref   = '',    
    @notes   = @memo,    
    @Quantity  = 0,    
   -- @Cost   = @sercost,    
	@Cost=@cost_current,
    @transdate  = @curdate,    
    @user   = @createdby,
	@userfield2=@user2     

   exec amics_sp_api_update_translogsn    
    @translogid  = @newid1,    
    @serno   = @serno_current,    
    @tagno   = @tagno_current,    
    --@cost=@sercost, 
    @model=@model_current,   
    
    @cost=@cost_current,  
    @loc=@serlocation,    
    @whs=@serwarehouse,        
    @user   = @createdby    
      
     
   exec amics_sp_api_update_translogsn   
    @translogid  = @newid1,    
    @serno   = @serno_prior,    
    @tagno   = @tagno_prior,    
    --@cost=@sercost,    
    @cost=@cost_current,  
    @model=@model_current,
      
    @loc=@serlocation,      
    @whs=@serwarehouse,       
    @user   = @createdby    
      
 end          
         
END     
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_update_translogsn]    Script Date: 13-07-2022 08:51:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_update_translogsn]
	@translogid	uniqueidentifier,
	@serno		varchar(50),
	@tagno		varchar(50),
	@model		varchar(50)='',
	@user		varchar(50),
	@cost		decimal(18,8),
	@loc		varchar(50),
	@whs		varchar(50),
	@lic_plate	varchar(50)=NULL,
	@lic_plate_flag bit=NULL
	
AS
BEGIN
insert into translogsn (translogid,serno,tagno,model,createdby,cost,location,warehouse,lic_plate,lic_plate_flag) values 
	(@translogid,@serno,@tagno,@model,@user,@cost,@loc,@whs,@lic_plate,@lic_plate_flag)
END
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_bulk_transfer_view]    Script Date: 13-07-2022 09:58:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_bulk_transfer_view]
	@warehouse varchar(50),@location varchar(50)
AS
	declare @locationsid uniqueidentifier
BEGIN
	set @locationsid=
		(select list_locations.id from list_locations 
			inner join list_warehouses on list_locations.warehousesid=list_warehouses.id 
			where list_locations.location=@location and list_warehouses.warehouse=@warehouse)
	select itemnumber,rev,description,isnull(sum(isnull(dbo.inv_basic.quantity,0)),0) as Quantity,itemsid from inv_basic 
		inner join list_locations on inv_basic.locationsid=list_locations.id 
		inner join list_items on inv_basic.itemsid=list_items.id where 
		inv_basic.quantity>0 and list_locations.id=@locationsid
		group by itemnumber,description,itemsid,rev
	union
	select itemnumber,rev,description,isnull(sum(isnull(dbo.inv_serial.quantity,0)),0) as Quantity,itemsid from inv_serial 
		inner join list_locations on inv_serial.locationsid=list_locations.id 
		inner join list_items on inv_serial.itemsid=list_items.id where 
		inv_serial.quantity>0 and list_locations.id=@locationsid
		group by itemnumber,description,itemsid,rev
	order by itemnumber
END
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_bulk_transfer]    Script Date: 13-07-2022 10:01:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_bulk_transfer] @fromwh varchar(50),@fromloc varchar(50),@towh varchar(50),@toloc varchar(50),@bulk_xfr_user varchar(50)
as
BEGIN
declare @fromlocid uniqueidentifier,@tolocid uniqueidentifier,
@invbasicid uniqueidentifier,@invserialid uniqueidentifier,@itemsid uniqueidentifier,
@invtransid uniqueidentifier,@xfr_toloc varchar(50),@xfr_towh varchar(50),@xfr_createdby varchar(50),
@transqty decimal(18,8),@transnum int,@setnum int,@solinesid uniqueidentifier,@transferid uniqueidentifier

update list_next_number set translognum =translognum+1
set @transnum=(select translognum from list_next_number)
set @fromlocid=(select list_locations.id from list_locations 
inner join list_warehouses on list_locations.warehousesid=list_warehouses.id 
where list_locations.location=@fromloc and list_warehouses.warehouse=@fromwh)
delete inv_transfer_location where createdby=@bulk_xfr_user  
declare cuBasic Cursor For (select id,quantity,so_linesid from inv_basic where locationsid=@fromlocid and quantity>0 ) -- and itemsid=@itemsid)
Open cuBasic
Fetch Next from cuBasic into @invbasicid,@transqty,@solinesid
while @@fetch_status = 0
begin 
insert into inv_transfer_location (id,solinesid,invbasicid,quantity,createddate,createdby) 
values (newid(),@solinesid,@invbasicid,@transqty,getdate(),@bulk_xfr_user)
Fetch Next from cuBasic into @invbasicid,@transqty,@solinesid
end 
close cuBasic
deallocate cuBasic

declare cuSerial Cursor For (select id,quantity,so_linesid,transferid from inv_serial where locationsid=@fromlocid and quantity>0 ) -- and itemsid=@itemsid)
Open cuSerial
Fetch Next from cuSerial into @invserialid,@transqty,@solinesid,@transferid 
while @@fetch_status = 0
begin 
if @transferid is not null set @solinesid =@transferid else set @solinesid =@solinesid 
 
insert into inv_transfer_location (id,solinesid,invserialid,quantity,createddate,createdby) 
values (newid(),@solinesid,@invserialid,@transqty,getdate(),@bulk_xfr_user)
Fetch Next from cuSerial into @invserialid,@transqty,@solinesid,@transferid
end 
close cuSerial
deallocate cuSerial
--exec sp_essex_transfer5 @xfr_createdby=@bulk_xfr_user, @toloc=@xfr_toloc, @towh=@xfr_towh, @xfr_transnum=@transnum
END
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_list_messages]    Script Date: 14-07-2022 10:25:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_list_messages]
@messagetext varchar(200)

AS
BEGIN

	if ((@messagetext is null) or (@messagetext = ''))		
		select messagenum,messagetext from list_messages order by messagenum		
	else		
		select messagenum,messagetext from list_messages where (messagenum like '%'+ @messagetext +'%') or (messagetext like '%' + @messagetext + '%') order by messagenum				

END
GO
--------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_list_fieldproperties]    Script Date: 14-07-2022 10:24:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_list_fieldproperties]  	
	@labelnumber varchar(200) =''
	
 AS 
 BEGIN  		

	if (@labelnumber = '')  Select id,labelnumber,mylabel from list_field_properties order by labelnumber
    else Select id,labelnumber,mylabel from list_field_properties where labelnumber in (select val from dbo.split(@labelnumber,',')) order by labelnumber

 END
 GO
 --------------------------------------------------------------------------------------------------------------------------------------
 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_search_mdat_out]    Script Date: 14-07-2022 10:34:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_search_mdat_out]     
@mdatNum varchar(50)='',      
@er varchar (50)='',     
@packlistNum varchar(50)='',
@status varchar(50) =''      

As     
begin 

if @mdatNum = '' and @er = '' and @packlistNum = '' and @status =''
	begin
		select distinct inv_mdat_out.mdat_num,so_main.somain,inv_mdat_out.description,inv_mdat_out.createddate
		FROM dbo.so_main inner join inv_mdat_out on so_main.id=inv_mdat_out.somainId
		inner join list_status on inv_mdat_out.statusid=list_status.id order by createddate desc
	end
else 
	begin
		select distinct inv_mdat_out.mdat_num,somain,inv_mdat_out.description,inv_mdat_out.createddate
		FROM dbo.so_main inner join inv_mdat_out on so_main.id=inv_mdat_out.somainId
		inner join list_status on inv_mdat_out.statusid=list_status.id
		where so_main.somain LIKE case when @er ='' then '%%' else '%'+@er+'%' end
		and mdat_num like case when @mdatNum ='' then '%%' else '%'+@mdatNum+'%' end    
		and packlistnum like case when @packlistNum ='' then '%%' else '%'+@packlistNum+'%' end  
		and list_status.status Like case when @status='' then '%%' else '%'+@status+'%' end
		order by createddate desc
	end
End
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_mdat_out]    Script Date: 14-07-2022 10:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_view_mdat_out]     
@mdatNum varchar(50)=''   
AS     
begin 

if @mdatNum = ''
	begin
		select distinct inv_mdat_out.mdat_num,so_main.somain,list_status.status,inv_mdat_out.packlistnum,inv_mdat_out.submitted_date,
		inv_mdat_out.approved_date,inv_mdat_out.shipped_date,inv_mdat_out.createddate
		FROM dbo.so_main inner join inv_mdat_out on so_main.id=inv_mdat_out.somainId
		inner join list_status on inv_mdat_out.statusid=list_status.id  order by createddate desc
	end
else 
	begin
		select distinct inv_mdat_out.mdat_num,so_main.somain,list_status.status,inv_mdat_out.packlistnum,inv_mdat_out.submitted_date,
		inv_mdat_out.approved_date,inv_mdat_out.shipped_date,inv_mdat_out.createddate
		FROM dbo.so_main inner join inv_mdat_out on so_main.id=inv_mdat_out.somainId
		inner join list_status on inv_mdat_out.statusid=list_status.id
		where mdat_num=@mdatNum order by createddate desc
	end
End  
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_mdatout]    Script Date: 14-07-2022 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_mdatout] 
 @actionflag		smallint, 
 @id				uniqueidentifier = null, 
 @mdat_num			varchar(50),
 @somain			varchar(50),
 @description		varchar(50),
 @status			varchar(50),
 @packlistnum		int,
 @submitted_date	datetime=null,
 @approved_date		datetime=null,
 @shipped_date		datetime=null,
 @cancelled_date	datetime=null,
 @createdby			varchar(50),
 @shippingId		uniqueidentifier = null
 
 AS 
 BEGIN  		 
	 declare @somainId uniqueidentifier=null
	 declare @statusId uniqueidentifier=null
	 
	 if (@somain != '' or @somain is not null)
		set @somainId = (select id from so_main where somain=@somain)

	 if (@status != '' or @status is not null)
		set @statusId = (select id from list_status where status=@status)
	 
	 if (@submitted_date = '' or @submitted_date is null)
		set @submitted_date = GETDATE();
	 
	 if (@approved_date = '' or @approved_date is null)
		set @approved_date = null;	 
	    
 	 if (@shipped_date = '' or @shipped_date is null)
		set @shipped_date = null;

	 if (@cancelled_date = '' or @cancelled_date is null)
		set @cancelled_date = null;

 	if @actionflag=1 
		begin
			insert into dbo.inv_mdat_out values (newid(),@mdat_num,@somainId,@description,@statusid,@packlistnum,@submitted_date,@approved_date,@shipped_date,@cancelled_date,@createdby,getdate(),null,@createdby,getdate())			
		end
 	if @actionflag=2 
		begin
			update dbo.inv_mdat_out set mdat_num=@mdat_num,somainId=@somainId,description=@description,statusid=@statusid,packlistnum=@packlistnum,submitted_date=@submitted_date,approved_date=@approved_date,shipped_date=@shipped_date, cancelled_date=@cancelled_date, shippingid=@shippingId, modifiedby=@createdby, modifieddate=getdate() where id=@id  			
		end
 	if @actionflag=3 	
		begin
			delete from dbo.inv_mdat_out where id=@id							
		end
 END
 GO
 -------------------------------------------------------------------------------------------------------------------------------------
    
/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_transnum]    Script Date: 14-07-2022 11:01:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[amics_sp_api_get_transnum]
as
begin
	Update list_next_number set sp_rec=sp_rec+1
	SELECT list_next_number.sp_rec FROM list_next_number
End
GO
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_insert_inv_trans]    Script Date: 14-07-2022 11:07:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_insert_inv_trans]     
	    @TransNum int
        ,@InvBasicId uniqueidentifier = null
        ,@InvSerialId uniqueidentifier = null
        ,@ItemsId uniqueidentifier = null
        ,@Source varchar(50)=null         
        ,@FromLocationId uniqueidentifier = null
        ,@ToLocationId uniqueidentifier = null
        ,@TransQty  decimal(18,8) = 0  
        ,@BoxNum  varchar(50)=null  
        ,@Createdby  varchar(50)=null     
As
BEGIN
 
insert into inv_trans(transnum,source,itemsid,invbasicid,invserialid,transqty,boxnum,createdby,createddate)values
(
@TransNum
,@Source
,@ItemsId
,@InvBasicId
,@InvSerialId
,@TransQty
,@BoxNum
,@Createdby
,GETDATE()
)
 
select 'success' as [message]

END
GO

-------------------------------------------------------------------------------------------------------------------------------------
 
GO
/****** Object:  StoredProcedure [dbo].[amics_sp_api_pick]    Script Date: 8/1/2022 11:25:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_pick]
	@pick_transdate smalldatetime='01-01-00',
	@pick_sourcesrefid uniqueidentifier=null, 
	@pick_misc_reason varchar(50) ='',
	@pick_misc_ref varchar(50)='',
	@pick_misc_source varchar(50)='',
	@Pick_Shipvia varchar(50)='',
	@pick_source varchar(50)='',
	@Pick_Shipcharge varchar(50)='',
	@Pick_Trackingnum varchar(50)='',
	@pick_notes varchar(100)='', 
	@Pick_PackNote varchar(max)='',
	@Pick_InvoiceNote varchar(max)='',
	@Pick_SalesTax decimal (18,8)=0,
	@Pick_Shipdate smalldatetime='01-01-00',
	@pick_user varchar(50)='',
	@pick_transnum int=0,
	@pick_setnum int =0,
	@pick_warehouse varchar(50)='',
	@original_receiptsid uniqueidentifier=null
	
 AS
BEGIN
	-- Begin declare
	declare @prior_shipped decimal(18,8),@womtlid uniqueidentifier,@invmiscid uniqueidentifier,
		@pick_translogid uniqueidentifier,@invsoshipid uniqueidentifier,@sourcesid uniqueidentifier,
		@reasonid uniqueidentifier,@trans_source varchar(50),@trans_invbasicid uniqueidentifier,
		@trans_itemid uniqueidentifier,@trans_locid uniqueidentifier,@invpickid uniqueidentifier,
		@trans_invserialid uniqueidentifier,@trans_refid uniqueidentifier,@somainid uniqueidentifier,
		@distinct_itemsid uniqueidentifier,

		@trans_transqty decimal(18,8),@trans_transnum int,@trans_user varchar(50),
		@priorqty decimal(18,8),@trans_lot varchar(50),@trans_color varchar(50),

		@pick_packlist varchar(50),@Pick_Shiptoname varchar(50),
		@Pick_Shiptoaddress1 varchar(50),@Pick_Shiptoaddress2 varchar(50),
		@Pick_Shiptoaddress3 varchar(50),@Pick_Shiptoaddress4 varchar(50),
		@Pick_Shiptoaddress5 varchar(50),@Pick_Shiptoaddress6 varchar(50),
		
		@trans_serno varchar(50),@trans_tagno varchar(50),@trans_cost money,
		@trans_wh varchar(50),@trans_loc varchar(50),@trans_boxnum smallint,
		@trans_item varchar(50),@trans_rev varchar(50),@trans_desc varchar(75),
		@trans_type varchar(50),@trans_itemtype varchar(50),@pick_quantity decimal(18,8),@pick_cost money,
		@allocqty decimal (18,8),@packlist_next int, @invpick_updated uniqueidentifier
	-- End declare
		exec amics_sp_api_pick_fifo @fifo_transnum=@pick_transnum,@fifo_setnum=@pick_setnum,@fifo_warehouse=@pick_warehouse 
		
	-- Begin Misc Pick
	if @pick_source='MISC PICK'
		begin
			set @invmiscid=(select newid())
			set @pick_sourcesrefid=@invmiscid
			set @reasonid=(select id from list_reasons where reason=@pick_misc_reason)
		end
	else
		set @invmiscid=NULL
	-- End Misc Pick
	-- BEGIN MTL CHG
	if @pick_source='MTL CHG'
		set @womtlid=(select newid())
	-- END MTL CHG
	-- Begin Initialize
	set @invpickid=(select newid())
	set @invsoshipid=(select id from inv_soship where  packlist=@pick_packlist)
	if  @invsoshipid is null set @invsoshipid=(select newid())
	set @sourcesid=(select id from inv_sources where sourceref=@pick_source)
	set @priorqty=0
	if @pick_transdate='' set @pick_transdate=getdate()
	-- End Initialize
	-- Begin Process Transactions
	declare curdistinct cursor for  (select distinct itemsid from inv_trans where transnum=@pick_transnum )
	open curdistinct
	fetch next from curdistinct into @distinct_itemsid 
	while @@FETCH_STATUS =0 
	begin
	set @pick_translogid=(select newid())

	declare cuMyCursor Cursor For (select itemsid,invbasicid,invserialid,refid,transqty,boxnum from inv_trans where 
		transnum=@pick_transnum and itemsid=@distinct_itemsid )
	Open cuMyCursor
	Fetch Next from cuMyCursor into @trans_itemid,@trans_invbasicid,@trans_invserialid,@trans_refid,@trans_transqty,@trans_boxnum
	while @@fetch_status = 0
	
	begin 
		set @pick_quantity=0
		If @pick_source='SHIPPING'
		set @prior_shipped=
			(SELECT SUM(ISNULL(dbo.inv_pick.pick_quantity, 0)) FROM dbo.inv_pick
			INNER JOIN dbo.so_lines ON dbo.inv_pick.sources_refid=so_lines.id 
			where inv_pick.sources_refid=@trans_refid and inv_pick.inv_soshipid<>@invsoshipid)
		Else
		set @Prior_shipped=0
		
		if @trans_invserialid is not null	-- Serialized parts
		begin
			set @trans_transqty=1
			if @pick_quantity=0 -- DO it only once and not for every record in the loop
				set @pick_quantity=(select count(invserialid) from inv_trans where transnum=@pick_transnum and itemsid=@trans_itemid)
		end
		if @pick_quantity=0   -- Non serialized parts
			set @pick_quantity=(select SUM(transqty) from inv_trans where transnum=@pick_transnum and itemsid=@trans_itemid)
		select @trans_itemid=
			case 
				when @trans_invbasicid is not null then (select itemsid from inv_basic where id=@trans_invbasicid)
				when @trans_invserialid is not null then (select itemsid from inv_serial where id=@trans_invserialid)
			end
		select @pick_cost=
			case 
				when @trans_invbasicid is not null then (select isnull(cost,0) from inv_basic where id=@trans_invbasicid)
				when @trans_invserialid is not null then (select isnull(cost,0) from inv_serial where id=@trans_invserialid)
			end
		
		set @trans_item=(select itemnumber from list_items where id=@trans_itemid)
		set @trans_rev=(select rev from list_items where id=@trans_itemid)
		set @trans_desc=(select description from list_items where id=@trans_itemid)
		set	@trans_itemtype=dbo.amics_fn_api_itemtype(@trans_item,@trans_rev)
		set @trans_type=dbo.amics_fn_api_invtype(@trans_item,@trans_rev)

		if @trans_invbasicid is not null 
			set @trans_locid=(select locationsid from inv_basic where id=@trans_invbasicid)
		else
			set @trans_locid=(select locationsid from inv_serial where id=@trans_invserialid)

		if @trans_invbasicid is not null and @trans_type='LOT'
		begin
			set @trans_lot=(select lotno from inv_basic where id=@trans_invbasicid)
			set @trans_color=(select color from inv_basic where id=@trans_invbasicid)
			set @priorqty=(select quantity from inv_basic where id=@trans_invbasicid)
		end
		if @trans_invserialid is not null and @trans_type='SERIAL'
		begin
			set @trans_serno=(select serno from inv_serial where id=@trans_invserialid)
			set @trans_tagno=(select tagno from inv_serial where id=@trans_invserialid)
			set @priorqty=(select quantity from inv_serial where id=@trans_invserialid)
		end
		set @trans_wh =(SELECT dbo.list_warehouses.warehouse FROM dbo.list_warehouses INNER JOIN
				dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid where 
				list_locations.id=@trans_locid)
		set @trans_loc=(SELECT dbo.list_locations.location FROM dbo.list_locations where list_locations.id=@trans_locid)


		-- Start Updates and inserts within the loop
		insert into inv_pick (id,sourcesid,sources_refid,Inv_soshipid,
			inv_basicid,inv_serialid,Boxnum,prior_quantity,
			pick_quantity,prior_shipped,Trans_date,createdby) values 
			(@invpickid,@sourcesid,@trans_refid,@invsoshipid,
				@trans_invbasicid,@trans_invserialid,@trans_boxnum,@priorqty,
				@trans_transqty,@prior_shipped,@pick_transdate,@pick_user)
		If @pick_source='SUPPLIER KIT'
			insert into inv_supplierkit (po_linesid,invbasicid,invserialid,itemsid,quantity,createdby ) values 
				(@trans_refid,@trans_invbasicid,@trans_invserialid,@trans_itemid,
				@trans_transqty,@pick_user)
				
		if @trans_invbasicid is not null update inv_basic set quantity=quantity-@trans_transqty where id=@trans_invbasicid
		if @trans_invserialid is not null update inv_serial set quantity=0,pickid=@invpickid where id=@trans_invserialid
		if @pick_source='MISC PICK'
			insert into inv_misc (id,itemsid,ref,source,reasonid,createdby) values 
				(@invmiscid,@trans_itemid,@pick_misc_ref,@pick_misc_source,@reasonid,@pick_user)
		--BEGIN Update Inv_allocate  for wobomid or solinesid
		
		exec amics_sp_api_remove_allocate @refid=@trans_refid, @qty=@trans_transqty

		--END Update Inv_allocate  for wobomid or solinesid
		set @trans_transqty=@trans_transqty*-1
		if @trans_type='BASIC'
			exec amics_sp_api_update_transloglot
				@translogid		=	@pick_translogid,
				@quantity		=	@trans_transqty,
				@lotno			=	'',
				@color			=	'',
				@user			=	@pick_user,
				@cost			=	@pick_cost,
				@loc			=	@trans_loc,
				@whs			=	@trans_wh		
		if @trans_type='LOT'
			exec amics_sp_api_update_transloglot
				@translogid		=	@pick_translogid,
				@quantity		=	@trans_transqty,
				@lotno			=	@trans_lot,
				@color			=	@trans_color,
				@user			=	@pick_user,
				@cost			=	@pick_cost,
				@loc			=	@trans_loc,
				@whs			=	@trans_wh
		if @trans_type='SERIAL'
				exec	amics_sp_api_update_translogsn
				@translogid		=	@pick_translogid,
				@serno			=	@trans_serno,
				@tagno			=	@trans_tagno,
				@user			=	@pick_user,
				@cost			=	@pick_cost,
				@loc			=	@trans_loc,
				@whs			=	@trans_wh
				
		if @pick_source='WIP'
			exec amics_sp_api_pick_update_wip @transnum=@pick_transnum,@user=@pick_user
		-- End Updates and inserts within the loop
		Fetch Next from cuMyCursor into @trans_itemid,@trans_invbasicid,@trans_invserialid,@trans_refid,@trans_transqty,@trans_boxnum
	end 
	close cuMyCursor
	deallocate cuMyCursor
	--set @pick_quantity=(select SUM(transqty) from inv_trans where transnum=@pick_transnum and itemsid=@trans_itemid)*-1
		set @pick_quantity=@pick_quantity*-1
		exec amics_sp_api_update_translog
		@translogid		=	@pick_translogid,
		@itemid			=	@trans_itemid,
		@Invtype		=	@trans_type,
		@Itemtype		=	@trans_itemtype,
		@Rev			=	@trans_rev,
		@trans			=	@pick_source,
		@item			=	@trans_Item,
		@Description	=	@trans_desc, 
		@Warehouse		=	@trans_wh,
		@Location		=	@trans_loc,
		@Reason			=	@pick_misc_reason,
		@Source			=	@pick_misc_source,
		@Ref			=	@pick_misc_ref,
		@notes			=	@pick_Notes,
		@Quantity		=	@pick_quantity,
		@Cost			=	@pick_cost,
		@transdate		=	@pick_transdate,
		@user			=	@pick_user
	
	
	fetch next from curdistinct into @distinct_itemsid 
	end
	close curdistinct
	deallocate curdistinct

	-- Start Updates and inserts outside the loop
	set @invpick_updated=(select top 1 ID from inv_pick where id=@invpickid) 
	if @invpick_updated is not null 
	begin
		If @pick_source='SHIPPING'
		begin
			update list_next_number set packlist=packlist+1
			set @pick_packlist=(select rtrim(isnull(packlist_prefix,'')) from list_prefix_suffix)
			set @packlist_next=(select packlist from list_next_number)
			set @pick_packlist=@pick_packlist+rtrim(CAST(@packlist_next AS varchar(50)))
			set @pick_packlist=@pick_packlist+(select rtrim(isnull(packlist_suffix,'')) from list_prefix_suffix)
		
			set @trans_refid=(select top 1 refid from inv_trans where transnum=@pick_transnum)
			set @somainid=(select so_main.id from so_main inner join so_lines on so_main.id=so_lines.somainid where so_lines.id=@trans_refid)
			set @Pick_Shiptoname=(select shiptoname from so_main where id=@somainid)
			set @Pick_Shiptoaddress1=(select shiptoaddress1 from so_main where id=@somainid)
			set @Pick_Shiptoaddress2=(select shiptoaddress2 from so_main where id=@somainid)
			set @Pick_Shiptoaddress3=(select shiptoaddress3 from so_main where id=@somainid)
			set @Pick_Shiptoaddress4=(select shiptoaddress4 from so_main where id=@somainid)
			set @Pick_Shiptoaddress5=(select shiptoaddress5 from so_main where id=@somainid)
			set @Pick_Shiptoaddress6=(select shiptoaddress6 from so_main where id=@somainid)
			set @pick_salestax=(select dbo.amics_fn_api_tax_decimal_soship (@invsoshipid))
			insert into inv_soship (id,packlist,shiptoname,shiptoaddress1,shiptoaddress2,
				shiptoaddress3,shiptoaddress4,shiptoaddress5,shiptoaddress6,
				shipvia,Trackingnum,PackNote,Invoicenote,ShipCharge,SalesTax,shipdate,createdby,invoiced) values
				(@invsoshipid,@pick_packlist,@Pick_Shiptoname,@Pick_Shiptoaddress1,@Pick_Shiptoaddress2,
					@Pick_Shiptoaddress3,@Pick_Shiptoaddress4,@Pick_Shiptoaddress5,
					@Pick_Shiptoaddress6,@pick_shipvia,@Pick_Trackingnum,@pick_packnote,
					@Pick_InvoiceNote,
					@pick_shipcharge,@pick_salestax,@pick_shipdate,@pick_user,0)
		end
		if @pick_source ='UNDO POREC'
		begin
			set @trans_refid=(select top 1 refid from inv_trans where transnum=@pick_transnum)
			insert into inv_receipts (sourcesid,sources_refid,recd_locationid,inv_basicid,
				recd_quantity,trans_date,createdby,original_receiptsid,recd_notes ) values 
				(@sourcesid,@trans_refid,@trans_locid,@trans_invbasicid,
				@pick_quantity*-1,@pick_transdate,@pick_user,@original_receiptsid,@pick_notes )
		end
	end	
	-- End Updates and inserts outside the loop
		If @pick_source='SHIPPING' --and  @invpick_updated is not null  
			select @pick_packlist as packlist	else  select '0' as packlist 	
	-- End Process Transactions

	select 'Successfull Saved'
END
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_pick_fifo]    Script Date: 15-07-2022 07:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_pick_fifo]
	@fifo_transnum int,@fifo_warehouse varchar(50), @fifo_setnum int =0
AS
BEGIN	-- Begin of Procedure
	declare @transid uniqueidentifier,@trans_itemid uniqueidentifier,@trans_invbasicid uniqueidentifier,
			@trans_invserialid uniqueidentifier,@trans_refid uniqueidentifier,
			@basid uniqueidentifier,@lotid uniqueidentifier,@serid uniqueidentifier,
			@invtypeid uniqueidentifier,
			@fromlocid uniqueidentifier,@tolocid uniqueidentifier,@whid uniqueidentifier,
			@basicid uniqueidentifier,@serialid uniqueidentifier,@basicqty decimal(18,8),
			@trans_transqty decimal(18,8),@trans_boxnum smallint,
			@source varchar(50),@createdby varchar(50),@fromloc varchar(50),
			@remainqty decimal(18,8),@tmpqty decimal(18,8)
	-- End of Declare
	set @whid=(select ID from list_warehouses where warehouse=@fifo_warehouse)
	set @basid=(select id from list_invtypes where invtype='BASIC')
	set @lotid=(select id from list_invtypes where invtype='LOT')
	set @serid=(select id from list_invtypes where invtype='SERIAL')
-- Begin of Basic Processing
	declare cuFIFO Cursor For (select id,source,itemsid,refid,transqty,createdby,boxnum
		 from inv_trans where transnum=@fifo_transnum and itemsid is not null and invbasicid is null and invserialid is null)
	Open cuFIFO
	Fetch Next from cuFIFO into @transid,@source,@trans_itemid,@trans_refid,@trans_transqty,@createdby,@trans_boxnum
	while @@fetch_status = 0
	begin -- Begin of cuFIFO Processing
		set @invtypeid=(select invtypeid from list_items where id=@trans_itemid)
		set @remainqty=@trans_transqty
		if @invtypeid=@basid or @invtypeid=@lotid		-- Basic or Lot
		begin	-- Begin of Basic/Lot Processing
			declare cuAvailable Cursor For 
				(select inv_basic.id,inv_basic.quantity,inv_basic.locationsid from inv_basic inner join 
					list_locations on inv_basic.locationsid=list_locations.id 
					where (itemsid=@trans_itemid  and inv_basic.quantity>0 and list_locations.warehousesid=@whid 
					and list_locations.invalid=0)  ) order by inv_basic.createddate 
			open cuAvailable
			fetch next from cuAvailable into @basicid,@basicqty,@fromlocid
			while @@fetch_status = 0  and @remainqty>0
			begin	-- Begin of cuAvailable
				if @remainqty-@basicqty>0	set @tmpqty=@basicqty else set @tmpqty=@remainqty
				insert into inv_trans (source,itemsid,invbasicid,refid,fromlocid,transqty,createdby,boxnum,transnum,setnum ) 
					values (@source,@trans_itemid,@basicid,@trans_refid,@fromlocid,@tmpqty,@createdby,
								@trans_boxnum,@fifo_transnum,@fifo_setnum)
				set @remainqty=@remainqty-@tmpqty
				fetch next from cuAvailable into @basicid,@basicqty,@fromlocid
			end	-- End of cuAvailable
			close cuAvailable
			deallocate cuAvailable
			delete inv_trans where id=@transid
		end		-- End of Basic/LOt Processing
		else											-- Serial
		begin	-- Begin of Serial Processing
			declare cuAvailableSerial Cursor For 
				(select inv_serial.id,locationsid from inv_serial inner join list_locations on inv_serial.locationsid=list_locations.id 
					 where (itemsid=@trans_itemid  and inv_serial.quantity>0 and list_locations.warehousesid=@whid 
					 and list_locations.invalid=0)  ) order by inv_serial.createddate 
			open cuAvailableSerial
			fetch next from cuAvailableSerial into @serialid,@fromlocid
			while @@fetch_status = 0  and @remainqty>0
			begin	-- Begin of cuAvailableSerial Processing
				if @remainqty-1>0	set @tmpqty=1 else set @tmpqty=@remainqty
				insert into inv_trans (source,itemsid,invserialid,refid,fromlocid,transqty,createdby,boxnum,transnum,setnum ) 
					values (@source,@trans_itemid,@serialid,@trans_refid,@fromlocid,@tmpqty,@createdby,
								@trans_boxnum,@fifo_transnum,@fifo_setnum)
				set @remainqty=@remainqty-@tmpqty
				fetch next from cuAvailableSerial into @serialid,@fromlocid
			end  -- End of cuAvailableSerial Processing
			close cuAvailableSerial
			deallocate cuAvailableSerial
			delete inv_trans where id=@transid
		end -- End of Serial Processing
		Fetch Next from cuFIFO into @transid,@source,@trans_itemid,@trans_refid,@trans_transqty,@createdby,@trans_boxnum
	end -- End of cuFIFO Processing
	close cuFIFO
	deallocate cuFIFO
END	-- End of Procedure
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_remove_allocate]    Script Date: 15-07-2022 07:53:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [dbo].[amics_sp_api_remove_allocate] @refid uniqueidentifier, @qty decimal (18,0)

AS
begin
 declare
 @curqty decimal(18,8)
 begin
	 set @curqty=(select quantity from inv_allocate where sources_refid=@refid)
	 if @curqty <= @qty
		delete inv_allocate where sources_refid=@refid
	 else
		update inv_allocate set quantity=(@curqty-@qty) where sources_refid=@refid
	end
 End
 GO
 
--------------------------------------------------------------------------------------------------------------------------------------
 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_pick_update_wip]    Script Date: 15-07-2022 07:59:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_pick_update_wip] 
 	@transnum	int,@user varchar(50) 
 AS 
 BEGIN 
 	declare @invwipid uniqueidentifier,@basid uniqueidentifier,@serid uniqueidentifier, 
 		@itemid uniqueidentifier,@sourcerefid uniqueidentifier, 
 		@qty decimal(18,8),	@tempqty int 
 	declare cuMyItemCursor Cursor For (select distinct itemsid from inv_trans where transnum=@transnum ) 
 	Open cuMyItemCursor 
 	Fetch Next from cuMyItemCursor into @itemid 
 	while @@fetch_status = 0 
 	begin 
 		set @tempqty=0	 
 		set @invwipid=(select newid()) 
 		declare cuMyCursor Cursor For 
 			(select invbasicid,invserialid,refid,transqty from inv_trans where transnum=@transnum 
 				and itemsid=@itemid) 
 		Open cuMyCursor 
 		Fetch Next from cuMyCursor into @basid,@serid,@sourcerefid,@qty 
 		while @@fetch_status = 0 
 		begin 
 			if @basid is not null	-- Basic parts 
 				insert into dbo.inv_wip values 
 					(@invwipid,@basid,@qty,@user,getdate(),@sourcerefid) 
 			if @serid is not null	-- Serialized parts 
 			begin 
 				set @tempqty=@tempqty+1 
 				insert into dbo.inv_wipsn (inv_wipid,invserialid,createdby,createddate) values 
 					(@invwipid,@serid,@user,getdate()) 
 			end 
 		Fetch Next from cuMyCursor into @basid,@serid,@sourcerefid,@qty 
 		end 
 		if @serid is not null	-- Consolidate quantity and then write to inv_wip 
 			insert into dbo.inv_wip (id,quantity,createdby,createddate,sources_refid) values 
 				(@invwipid,@tempqty,@user,getdate(),@sourcerefid) 
 		close cuMyCursor 
 		deallocate cuMyCursor 
 	Fetch Next from cuMyItemCursor into @itemid 
 	close cuMyItemCursor 
 	deallocate cuMyItemCursor 
 	end 
 END 
 GO
-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_userlist]    Script Date: 25-07-2022 11:58:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_get_userlist] 
@userid varchar(50)
AS
BEGIN
declare @pwdValue varchar(10)
set @pwdValue = (select yesorno from dbo.list_company_options where optionid='60')

	if (@userid is null or @userid ='')
		select * from sec_users
	else
		begin
			if (@pwdValue = 1)
				select sec_users.id,sec_users.userid,sec_users.signature, sec_users.[user],sec_users.firstname,sec_users.lastname, dbo.amics_fn_api_decrypt(sec_users.password)  as password, 
				sec_users.email, sec_users.buyer, sec_users.salesperson, sec_users.webaccess,sec_users.employee,sec_users.forgotpwdans,list_warehouses.warehouse 
				from sec_users left outer join list_warehouses on sec_users.defaultwh =  list_warehouses.id where  sec_users.id = @userid
			else
				select sec_users.id,sec_users.userid,sec_users.signature, sec_users.[user],sec_users.firstname,sec_users.lastname, sec_users.password,sec_users.email,
				sec_users.buyer, sec_users.salesperson, sec_users.webaccess,sec_users.employee,sec_users.forgotpwdans,list_warehouses.warehouse
				from sec_users inner join list_warehouses on sec_users.defaultwh =  list_warehouses.id where  sec_users.id =  @userid
		end

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_secuseraccess]    Script Date: 25-07-2022 11:58:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_get_secuseraccess] 
@userid varchar(50)

AS
BEGIN		
	if (@userid is null or @userid ='')
		select id as accessid,access,module,0 as readonly,0 as onthefly from sec_access where accessnumber > 999
	else
		select * from sec_users_access join sec_access on sec_users_access.accessid=sec_access.id where sec_users_access.users_id=@userid and sec_access.accessnumber > 999
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_get_secwhaccess]    Script Date: 25-07-2022 11:58:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_get_secwhaccess] 
@userid varchar(50)

AS
BEGIN		
	if (@userid is null or @userid ='')
		select id as warehouses_id,Warehouse,0 as transok from list_warehouses order by warehouse
	else
		select * from sec_users_warehouses join list_warehouses on sec_users_warehouses.warehouses_id=list_warehouses.id where sec_users_warehouses.users_id=@userid order by warehouse

END
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:StoredProcedure [dbo].[amics_sp_api_validate_warehouse]    Script Date: 25-07-2022 11:58:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_validate_warehouse] 
@warehouse varchar(50)

AS
BEGIN		
	select id from list_warehouses where warehouse=@warehouse	
END
Go
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:StoredProcedure [dbo].[amics_sp_api_validate_userId]    Script Date: 25-07-2022 11:58:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_validate_userId] 
@userid varchar(50)

AS
BEGIN		
	select id from sec_users where userid=@userid
END
GO

-----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_secuser]    Script Date: 27-07-2022 09:36:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_secuser] 
@actionflag smallint,
@id uniqueidentifier = null,
@userid varchar(50),
@firstname varchar(50),
@lastname varchar(50),
@password varchar(50),
@warehouse varchar(50),
@email varchar(50),
@buyer bit,
@salesperson bit,
@webaccess bit,
@signature varchar(50),
@employee bit,
@user bit,
@forgotpwdans varchar(50),
@dbName varchar(50)

AS
BEGIN
declare @pwdValue varchar(10)
declare @encryptpwd varchar(30)
declare @warehouseId uniqueidentifier
set @pwdValue = (select yesorno from dbo.list_company_options where optionid='60')
set @warehouseId=(select top 1 dbo.list_warehouses.id from dbo.list_warehouses where warehouse=@warehouse)

if (@password != '******')
	set @encryptpwd = (select dbo.amics_fn_api_encrypt(@password))

	-----------sec_users -------------
if (@actionflag = 1)
	begin
		if (@pwdValue = 1)
			Insert into sec_users (id, userid, firstname, lastname, password, defaultwh, email, buyer, salesperson, createdby, createddate, webaccess, signature, employee, [user],forgotpwdans) values 
			(NEWID(), @userid, @firstname , @lastname, @encryptpwd, @warehouseId, @email, @buyer, @salesperson, @userid, getdate(), @webaccess, @signature, @employee, @user, @forgotpwdans)			
		else
			Insert into sec_users (id, userid, firstname, lastname, password, defaultwh, email, buyer, salesperson, createdby, createddate, webaccess, signature, employee, [user],forgotpwdans) values 
			(NEWID(), @userid, @firstname , @lastname, @password, @warehouseId, @email, @buyer, @salesperson, @userid, getdate(), @webaccess, @signature, @employee, @user, @forgotpwdans)
	end	

else if (@actionflag = 2)
	begin
		if (@pwdValue = 1 and @encryptpwd = '')
			update sec_users set userid = @userid, firstname = @firstname, lastname = @lastname, defaultwh=@warehouseId, email=@email, buyer=@buyer, salesperson=@salesperson, webaccess =@webaccess, signature = @signature, employee = @employee, [user] =@user,forgotpwdans =@forgotpwdans where id =@id
		else if (@pwdValue = 1 and @encryptpwd != '')
			update sec_users set userid = @userid, firstname = @firstname, lastname = @lastname, password=@encryptpwd, defaultwh=@warehouseId, email=@email, buyer=@buyer, salesperson=@salesperson, webaccess =@webaccess, signature = @signature, employee = @employee, [user] =@user,forgotpwdans =@forgotpwdans where id =@id
		else if (@pwdValue = 0 and @password != '******')
			update sec_users set userid = @userid, firstname = @firstname, lastname = @lastname, password=@password, defaultwh=@warehouseId, email=@email, buyer=@buyer, salesperson=@salesperson, webaccess =@webaccess, signature = @signature, employee = @employee, [user] =@user,forgotpwdans =@forgotpwdans where id =@id
		else
			update sec_users set userid = @userid, firstname = @firstname, lastname = @lastname, defaultwh=@warehouseId, email=@email, buyer=@buyer, salesperson=@salesperson, webaccess =@webaccess, signature = @signature, employee = @employee, [user] =@user,forgotpwdans =@forgotpwdans where id =@id
			
	end

else if (@actionflag = 3)
	delete sec_users where id = @id

	-----------login_cred -------------
if (@actionflag = 1)
	begin
		 if (@pwdValue = 1)
			insert into login_cred (id, username, password, dbname, createddate) values (NEWID(), @userid, @encryptpwd, @dbname, GETDATE())
		 else
			insert into login_cred (id, username, password, dbname, createddate) values (NEWID(), @userid, @password, @dbName, GETDATE())			
	end	
else if (@actionflag = 2)
	begin
		if (@pwdValue = 1 and @encryptpwd = '')
			update login_cred set username = @userid where userName =@userid
		else if (@pwdValue = 1 and @encryptpwd != '')
			update login_cred set username = @userid, password=@encryptpwd where userName = @userid
		else if (@pwdValue = 0 and @password != '******')
			update login_cred set username = @userid, password=@password where userName = @userid
		else
			update login_cred set username = @userid where userName = @userid
	end			
else if (@actionflag = 3)
	delete login_cred where username =@userid

END
GO

-----------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_secuseraccess]    Script Date: 27-07-2022 09:36:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_secuseraccess] 
@actionflag smallint,
@id uniqueidentifier = null,
@userid uniqueidentifier = null,
@accessid uniqueidentifier = null,
@readonly bit,
@onthefly bit,
@createdby varchar(50)

AS
BEGIN	

	if (@actionflag = 1 or @actionflag = 2)		
		insert into sec_users_access (id,users_id,accessid,readonly,onthefly,createdby,createddate) values(NEWID(), @userid, @accessid, @readonly, @onthefly, @createdby, GETDATE())
		
END
GO
--------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_secwarehouseaccess]    Script Date: 27-07-2022 09:36:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_secwarehouseaccess] 
@actionflag smallint,
@id uniqueidentifier = null,
@userid uniqueidentifier = null,
@warehouseid uniqueidentifier = null,
@readonly bit,
@createdby varchar(50)

AS
BEGIN
	
	if (@actionflag = 1 or @actionflag = 2)		
		insert into sec_users_warehouses (id,users_id,warehouses_id,transok,createdby,createddate)  values(NEWID(), @userid, @warehouseid, @readonly, @createdby, GETDATE())		

END
GO

---------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgLoc_basic_selecteditems]    Script Date: 03-08-2022 02:48:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_chgLoc_basic_selecteditems]     
@itemsId uniqueidentifier,   
@userId varchar(50),  
@soLinesId uniqueidentifier
AS    
BEGIN    
    select itl.id,itl.invbasicid,itl.quantity,list_locations.location,list_warehouses.warehouse,itl.createdby  from dbo.inv_transfer_location as itl 
    left outer join dbo.inv_basic on inv_basic.id = itl.invbasicid 
    left outer join dbo.list_items on list_items.id = inv_basic.itemsid 
    left outer join dbo.list_locations on list_locations.id = inv_basic.locationsid 
    left outer join dbo.list_warehouses on list_warehouses.id = list_locations.warehousesid 
    where list_items.id = @itemsId and itl.createdby = @userId and itl.solinesid=@soLinesId
END
GO

-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_chgLoc_serial_selecteditems]    Script Date: 03-08-2022 02:48:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_chgLoc_serial_selecteditems]     
@itemsId uniqueidentifier,   
@userId varchar(50),  
@soLinesId uniqueidentifier
AS    
BEGIN    
		select itl.id,itl.invserialid,itl.quantity, inv_serial.serno,inv_serial.tagno,inv_serial.model,list_locations.location,list_warehouses.warehouse,itl.createdby  from dbo.inv_transfer_location as itl 
        inner join dbo.inv_serial on inv_serial.id = itl.invserialid 
        inner join dbo.list_items on list_items.id = inv_serial.itemsid 
        left outer join dbo.list_locations on list_locations.id = inv_serial.locationsid 
        left outer join dbo.list_warehouses on list_warehouses.id = list_locations.warehousesid 
        where list_items.id= @itemsId and itl.createdby = @userId and itl.solinesid=@soLinesId
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_serial_selecteditems]    Script Date: 08-08-2022 05:18:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_serial_selecteditems]     
@itemsId uniqueidentifier,   
@userId varchar(50),  
@soLinesId uniqueidentifier
AS    
BEGIN    

 select itl.id,itl.invserialid,itl.quantity,inv_serial.serno,inv_serial.tagno,inv_serial.model,list_locations.location,list_warehouses.warehouse,itl.createdby  
 from dbo.inv_pick_ship as itl 
 inner join dbo.inv_serial on inv_serial.id = itl.invserialid 
 inner join dbo.list_items on list_items.id = inv_serial.itemsid 
 left outer join dbo.list_locations on list_locations.id = inv_serial.locationsid 
 left outer join dbo.list_warehouses on list_warehouses.id = list_locations.warehousesid 
 where list_items.id=@itemsId and itl.createdby = @userId and itl.solinesid=@soLinesId
		
END

----------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_basic_selecteditems]    Script Date: 08-08-2022 05:18:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_basic_selecteditems]     
@itemsId uniqueidentifier,   
@userId varchar(50),  
@soLinesId uniqueidentifier
AS    
BEGIN    

select itl.id,itl.invbasicid,itl.quantity,list_locations.location,list_warehouses.warehouse,itl.createdby  from dbo.inv_pick_ship as itl 
left outer join dbo.inv_basic on inv_basic.id = itl.invbasicid 
left outer join dbo.list_items on list_items.id = inv_basic.itemsid 
left outer join dbo.list_locations on list_locations.id = inv_basic.locationsid 
left outer join dbo.list_warehouses on list_warehouses.id = list_locations.warehousesid 
where list_items.id=@itemsId and itl.createdby = @userId and itl.solinesid=@soLinesId
		
END
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_ship]    Script Date: 23-08-2022 09:13:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_ship] @user varchar(50)='',@mdatout varchar(50) 

AS 

BEGIN 

       declare @packlist varchar(50),@invsoshipid uniqueidentifier,@xfr_transnum int,@prefix varchar(50),@shipvia varchar(50) = '' ,

              @total_solines smallint ,@counter_solines smallint ,@ship_id uniqueidentifier,@ship_solinesid uniqueidentifier,

              @ship_invbasicid uniqueidentifier,@ship_invserialid uniqueidentifier,@ship_quantity decimal(18,8),@somain varchar(50),@itemsid uniqueidentifier,

              @item varchar(50),@descr varchar(150),@line smallint,@invtype varchar(50),@itemtype varchar(50),

              @translogid uniqueidentifier,@locationsid uniqueidentifier,@warehouse varchar(50),@location varchar(50),@serno varchar(50),@tagno varchar(50),

              @cost decimal(18,8),@serialcounter smallint,@basiccounter smallint,@today smalldatetime

 

       if @user!='' 

       begin 

              set @today=getdate()

              set @prefix=(select packlist_prefix from list_prefix_suffix) 

              update list_next_number set translognum=translognum+1 

              set @xfr_transnum=(select translognum from list_next_number) 

              update list_next_number set packlist=packlist+1 

              set @packlist=@prefix+(select cast(packlist as varchar(50)) from list_next_number) 

              set @invsoshipid=newid() 

              set @shipvia=(select top 1 isnull(FieldValue,'') from inv_pick_ship 

                     inner join so_lines on inv_pick_ship.solinesid=so_lines.id 

                     inner join so_main on so_lines.somainid =so_main.id 

                     inner join list_dropdowns on so_main.shipviaid=list_dropdowns.id ) 

               

              INSERT INTO inv_soship (packlist, id,shiptoname,shiptoaddress1,shiptoaddress2,shiptoaddress3,shiptoaddress4,shiptoaddress5,shiptoaddress6, 

                      shipdate, createddate, createdby, invoiced,shipvia,packnote,trackingnum,invoicenote) select @packlist,@invsoshipid, 

                             'Maryland Procurement Office','9800 Savage Road','','Ft. George G. Meade','MD','20755','USA', 

                             getdate(),getdate(),@user,0,@shipvia,'',@mdatout,''

                             Insert into inv_pick ( sourcesid, sources_refid, Inv_soshipid,inv_serialid,inv_basicid,prior_quantity,pick_quantity,prior_shipped,Trans_date,createdby) 

                                          Select (select top 1 id from inv_sources where sourceref='SHIPPING'),solinesid,@invsoshipid,invserialid,invbasicid,1,quantity,0,getdate(),@user 

                                          from dbo.inv_pick_ship where not exists  (select id from inv_pick where inv_soshipid=@invsoshipid and invserialid=inv_pick_ship.invserialid ) 

                                          and createdby=@user 

              update inv_serial set quantity=0 where id in (select invserialid from inv_pick_ship where createdby=@user) 

              UPDATE inv_basic SET inv_basic.quantity = inv_basic.quantity-inv_pick_ship.quantity 

                                          FROM inv_pick_ship, inv_basic WHERE inv_pick_ship.invbasicid = inv_basic.id and inv_pick_ship.createdby=@user 

   

              insert into translog (id,itemsid,trans,transnum,transdate,transqty,itemnumber,description,itemtype,warehouse,location,reason, 

                                                       source,ref,invtype,notes,createdby,revision,createddate,modifieddate,location2,user2,cost) 

                                  select distinct newid(),itemsid,'SHIP',@xfr_transnum,@today,count(inv_serial.itemsid),itemnumber,description, 

                                  isnull(itemtype,'') as itemtype,warehouse,location,'SHIP',@mdatout,@packlist,'SERIAL','',@user,'',@today,@today,'', 

                                  (select somain from so_main where id = (select somainid from so_lines where id=inv_pick_ship.solinesid)),inv_serial.cost

                                  from inv_pick_ship inner join inv_serial on inv_pick_ship.invserialid =inv_serial.id 

                                  left join list_locations on inv_serial.locationsid=list_locations.id 

                                  left join list_warehouses on list_locations.warehousesid=list_warehouses.id 

                                  left join list_items on inv_serial.itemsid=list_items.id 

                                  left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id 

                                                where  inv_pick_ship.createdby=@user

                                                   group by itemsid,itemnumber,description,itemtype,warehouse,location,solinesid,inv_serial.cost

 

              insert into translogsn (id,translogid, serno,tagno,createdby,location,warehouse,cost) 

                                  select  newid(),(select id from translog where itemsid=so_lines.itemsid  and transnum=@xfr_transnum and user2=so_main.somain and cost=inv_serial.cost

                                  and location=list_locations.location), 

                                  serno,tagno,@user,location,warehouse,inv_serial.cost from inv_pick_ship 

                                  inner join so_lines on inv_pick_ship.solinesid =so_lines.id

                                  inner join so_main on so_lines.somainid=so_main.id

                                  inner join inv_serial on inv_pick_ship.invserialid=inv_serial.id 

                                  inner join list_locations on inv_serial.locationsid=list_locations.id 

                                  inner join list_warehouses on list_locations.warehousesid=list_warehouses.id 

 

              insert into translog (id,itemsid,trans,transnum,transdate,transqty,itemnumber,description,itemtype,warehouse,location,reason, 

                             source,ref,invtype,notes,createdby,revision,createddate,modifieddate,location2,user1,user2,cost) 

                             select distinct newid(),itemsid,'SHIP',@xfr_transnum,@today,inv_pick_ship.quantity,itemnumber,description,itemtype, 

                             warehouse,location,'SHIP',@mdatout,@packlist,'BASIC','',@user,'',@today,@today,'', 

                             (select linenum from so_lines where id=inv_pick_ship.solinesid),

                             (select somain from so_main where id = (select somainid from so_lines where id=inv_pick_ship.solinesid)),inv_basic.cost

                             from inv_pick_ship 

                             inner join inv_basic on inv_pick_ship.invbasicid=inv_basic.id 

                             left join list_items on inv_basic.itemsid=list_items.id 

                             left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id 

                             left join list_locations on inv_basic.locationsid=list_locations.id 

                             left join list_warehouses on list_locations.warehousesid=list_warehouses.id 

                             where inv_pick_ship.createdby=@user 

              delete inv_pick_ship where createdby =@user

              select @packlist

              end

END
GO
----------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_pickshipcntchk] Script Date: 08-08-2022 06:36:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_pickshipcntchk] 
 	@createdby varchar(50)	
AS
BEGIN  		 	
	Select COUNT(*) from inv_pick_ship where createdby=@createdby
END


---------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_invshippickdelete]    Script Date: 08-08-2022 07:38:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_invshippickdelete] 
@createdby varchar(50)=''
AS
BEGIN  		 
	delete from inv_pick_ship where createdby = @createdby
END
GO


---------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_pickship_update]    Script Date: 08-08-2022 07:39:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_pickship_update] 
@id uniqueidentifier=null,
@action int=0,
@quantity decimal=0,
@invbasicid uniqueidentifier=null,
@invserialid uniqueidentifier=null,
@solinesid uniqueidentifier=null,
@createdby varchar(50)=''

AS
BEGIN  	
	if @action = 1
		begin
			set @id = NEWID()
			if (@invbasicid is not null)
				insert into dbo.inv_pick_ship (id,solinesid,invbasicid,quantity,createddate,createdby) values (NEWID(),@solinesid,@invbasicid,@quantity,GETDATE(),@createdby)
			else
				insert into dbo.inv_pick_ship (id,solinesid,invserialid,quantity,createddate,createdby) values (NEWID(),@solinesid,@invserialid,@quantity,GETDATE(),@createdby)
			
		end
	else --action 2
	begin
		if(@invbasicid is not null)
			 delete from inv_pick_ship where invbasicid = @invbasicid and createdby = @createdby
		else
			 delete from inv_pick_ship where invserialid = @invserialid and createdby = @createdby
	end 
	
END
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_shipment_pickqty_toship]    Script Date: 08-08-2022 06:36:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_shipment_pickqty_toship] 
 	@solinesid uniqueidentifier  = 0x0,
	@invserialid uniqueidentifier = 0x0,
	@invbasicid uniqueidentifier = 0x0
	
 AS
 BEGIN   
	declare @EmptyGuid UNIQUEIDENTIFIER = 0x0  
	if (@invserialid != @EmptyGuid)
		begin
			select isnull(quantity,0) as quantity,createdby from inv_pick_ship where solinesid =@solinesid  and invserialid=@invserialid 
			union all 
			select isnull(quantity,0) as quantity,createdby from inv_transfer_location where solinesid =@solinesid  and invserialid=@invserialid  
			select isnull(quantity,0) as quantity,createdby from inv_serial where id =@invserialid
		end 
	else
		begin			
			 select isnull(quantity,0) as quantity,createdby from inv_pick_ship where solinesid =@solinesid  and invbasicid=@invbasicid 
			 union all 
			 select isnull(quantity,0) as quantity,createdby from inv_transfer_location where solinesid =@solinesid and invbasicid=@invbasicid  
			 select isnull(quantity,0) as quantity,createdby from inv_basic where id =@invbasicid
		end
 END
 GO

 ----------------------------------------------------------------------------------------------------------------------------------------------

 
/****** Object:  StoredProcedure [dbo].[amics_sp_api_search_SoOnOpen]    Script Date: 09-08-2022 08:54:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_search_SoOnOpen]     
AS     
BEGIN 		
	Select distinct top 10 dbo.so_main.somain, dbo.so_main.id,convert(varchar,dbo.so_main.sodate,101) as sodate,dbo.so_main.customername,
	dbo.so_main.createddate FROM dbo.so_main order by createddate desc
END
GO

---------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_so_search]    Script Date: 09-08-2022 09:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROCEDURE [dbo].[amics_sp_api_so_search]   
 @somain    varchar (50)='',   
 @user3        varchar (50)='',  -- Requestor  
 @status    varchar (50)='',  
 @projectid    varchar (50)='',  
 @projectname    varchar (50)='',  
 @searchflag int = 3,
 @mdatIn varchar(50) = ''
  
 AS  
   
Begin  
 SELECT distinct( dbo.so_main.somain), dbo.so_main.id, convert(varchar,dbo.so_main.sodate,101) as sodate, dbo.so_main.customername,so_main.createddate  
FROM dbo.so_main left join list_projects on so_main.projectid=list_projects.id
where 
somain LIKE	case  when @somain ='' then '%%' else '%'+@somain+'%' end
and project like case  when @projectid ='' then '%%' else @projectid+'%' end    
and name like case  when @projectname ='' then '%%' else @projectname+'%' end    
and statusid = case when @status!='' then (select id from list_status where status=@status) else statusid end 
and user3 LIKE	case  when @user3 ='' then '%%' else @user3+'%' end
and year(sodate) =case when @searchflag =1 then year(getdate()) when @searchflag=2 then year(getdate())-1 else year(sodate) end 
and mdat_in LIKE case  when @mdatIn ='' then '%%' else '%'+@mdatIn+'%' end
order by so_main.somain  desc
End
Go
--exec [dbo].[sp_search_essex_so5]    @somain='0497'

-----------------------------------------------------------------------------------------------------------------------------------------------

--Sp_View_SoMain_Essex_5
/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_somain]    Script Date: 10-08-2022 10:56:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_view_somain]
@soMainId uniqueidentifier =null
As
BEGIN
SELECT dbo.so_main.id,dbo.list_customers.id as customerid,so_main.contactsid, 
 dbo.so_main.somain,
 convert(varchar,dbo.so_main.sodate,101)as sodate, 
 '' AS ordertype,
--(select list_dropdowns.FieldValue from list_dropdowns where list_dropdowns.id=dbo.so_main.ordertypeid) AS ordertype,
ISNULL(dbo.so_main.user1,'') as user1,
ISNULL(dbo.so_main.user2,'') as user2,
ISNULL(dbo.so_main.user3,'') as user3,
ISNULL(dbo.so_main.user4,'') as user4,
ISNULL(dbo.so_main.user5,'') as user5,
ISNULL(dbo.so_main.user6,'') as user6,
ISNULL(dbo.so_main.user7,'') as user7,
ISNULL(dbo.so_main.user8,'') as user8,
ISNULL(dbo.list_status.status, '') AS status,
(select list_dropdowns.FieldValue from list_dropdowns where list_dropdowns.id = dbo.so_main.shipviaid) AS shipvia,
(select list_dropdowns.FieldValue from list_dropdowns where list_dropdowns.id = dbo.so_main.termsid) AS terms,
(select list_dropdowns.FieldValue from list_dropdowns where list_dropdowns.id = dbo.so_main.fobid)AS fob,
    
 dbo.so_main.billtoname,
 dbo.so_main.billtoaddress1,
 dbo.so_main.billtoaddress2,
 dbo.so_main.billtoaddress3,
 dbo.so_main.billtoaddress4, 
 dbo.so_main.billtoaddress5,
 dbo.so_main.billtoaddress6,
 dbo.so_main.shiptoname,
 dbo.so_main.shiptoaddress1,
 dbo.so_main.shiptoaddress2, 
 dbo.so_main.shiptoaddress3,
 dbo.so_main.shiptoaddress4,
 dbo.so_main.shiptoaddress5,
 dbo.so_main.shiptoaddress6,
 ISNULL(dbo.so_main.sonotes,'') as sonotes, 
 ISNULL(dbo.so_main.shipnotes,'') as shipnotes, 
 ISNULL(dbo.so_main.invoicenotes,'') as invoicenotes,
 ISNULL(dbo.list_customers.acctmsg, '') AS acctmsg,
 ISNULL(dbo.so_main.custpo, '') AS custpo, 
 ISNULL(dbo.so_main.salesperson, '') AS salesperson,
 ISNULL(dbo.so_main.customername, '') AS customername,
 ISNULL(dbo.list_customers.smallname,'') AS smallname, 
 --ISNULL(dbo.list_customers.customer, '') AS customer,
 ISNULL(dbo.list_customers.fax, '') AS fax,
 ISNULL(dbo.list_customers.phone, '') AS phone,
 ISNULL(dbo.so_main.commission, 0) AS commission,  
 ISNULL(dbo.so_main.salestax, 0) AS salestax,
 dbo.list_customer_contacts.name as contactName,dbo.list_customer_contacts.phone as contactPhone,
 dbo.list_customer_contacts.email as contactEmail,dbo.list_customer_contacts.fax as contactFax,
  convert(varchar,dbo.so_main.est,101)as est, 
  convert(varchar,dbo.so_main.reqdate,101)as reqdate, 
  convert(varchar,dbo.so_main.prcomp,101)as prcomp, 
  convert(varchar,dbo.so_main.funded,101)as funded, 
 so_main.budget,so_main.mdat_in,so_main.org,
 dbo.list_projects.id as projectUid, dbo.list_projects.project as projectid, dbo.list_projects.name as projectName,dbo.list_contracts.contractnum
 ,isnull(dbo.list_contracts.markup1,1) as markup1
 ,isnull(dbo.list_contracts.markup2,1) as markup2 
 FROM dbo.so_main LEFT OUTER JOIN 
 dbo.list_customers ON dbo.so_main.customersid = dbo.list_customers.id LEFT OUTER JOIN 
 dbo.list_customer_contacts ON dbo.so_main.contactsid = dbo.list_customer_contacts.id LEFT OUTER JOIN  
 dbo.list_projects ON dbo.so_main.projectid = dbo.list_projects.id LEFT OUTER JOIN  
 --dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid LEFT OUTER JOIN  
 dbo.list_status ON dbo.so_main.statusid = dbo.list_status.id LEFT OUTER JOIN  
 dbo.list_contracts ON dbo.list_contracts.id = dbo.list_projects.contractid

 where so_main.id= @soMainId
END
GO

---------------------------------------------------------------------------------------------------------------------

--sp_view_solines_essex
/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_solines]    Script Date: 10-08-2022 12:03:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_solines]
--SOLINES>COST(20)*MARKUP1(1.1)*MARKUP2(1.3)
--COST_U --> SOLINES.COST or price
--COST_L --> SOLINes.cost * SOLines.markup
--POCOST_U --> POLINES.unitcost 
--POCOST_L --> POLINES.UNITCOST* SOLines.markup
@somain varchar(50)
AS
BEGIN
     select so_lines.id,list_itemtypes.itemtype,so_lines.linenum,dbo.so_lines.itemnumber,so_lines.revision,dbo.so_lines.description,
  so_lines.itemsid,'' as customerItem, 
     so_lines.quantity,SUM(isnull(inv_pick.pick_quantity,0)) as pickedQty,uom,so_lines.user1,so_lines.unitcost as socost,
     ISNULL(dbo.so_lines.markup,1) as markup, 
     ISNULL(dbo.so_lines.unitcost,0) as COST_U, 
     ISNULL(dbo.so_lines.unitcost,0)*ISNULL(dbo.so_lines.markup,1) as COST_L,  
     ISNULL(dbo.po_lines.unitcost,0) as POCOST_U,
     ISNULL(dbo.po_lines.unitcost,0)*ISNULL(dbo.so_lines.markup,1) as POCOST_L,
     (so_lines.quantity*so_lines.unitcost) as extCost,
     convert(varchar,estshipdate,1) as estshipdate,          
     convert(varchar,shipdate,1) as shipdate,so_lines.warranty_years,so_lines.warranty_days,
     solinesnotes, dbo.list_items.obsolete 
     from so_lines 
--     LEFT OUTER JOIN list_customers_items on list_customers_items.id = so_lines.customers_itemsid   
     LEFT OUTER JOIN inv_pick on inv_pick.sources_refid = so_lines.id 
     LEFT OUTER JOIN list_items ON list_items.id = so_lines.itemsid 
     LEFT OUTER JOIN list_itemtypes on list_itemtypes.id = list_items.itemtypeid 
     LEFT OUTER JOIN so_main on so_main.id = so_lines.somainid    
     --LEFT OUTER JOIN list_contracts on list_contracts.id = so_main.contactsid
     LEFT OUTER JOIN po_lines on po_lines.so_linesid = so_lines.id 
     where somainid=(select top 1 id from so_main where  somain=@somain)  
     group by 
     so_lines.id,list_itemtypes.itemtype,so_lines.linenum,dbo.so_lines.itemnumber,so_lines.revision,
     dbo.so_lines.description,so_lines.itemsid,so_lines.quantity,uom,so_lines.user1,  
     so_lines.unitcost,dbo.so_lines.markup,dbo.list_items.markup,dbo.list_items.cost,estshipdate,shipdate,so_lines.warranty_years,
     so_lines.warranty_days,solinesnotes,dbo.list_items.obsolete,dbo.po_lines.unitcost order by linenum  
END
GO
---------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_validate_so]    Script Date: 10-08-2022 01:04:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[amics_sp_api_validate_so]
	@Project varchar(50),
	@Status varchar(50),
	@Shipvia varchar(50)	
As
Begin

Declare @projid uniqueidentifier, @statusid uniqueidentifier, @shipviaid uniqueidentifier
declare @messages varchar(250)

set @projid=(Select id from list_projects where project=@Project)
set @statusid=(select id from list_status where status=@Status)
set @shipviaid=(select id from list_dropdowns where ColNum='942' and FieldValue=@Shipvia)

create table #validataions1 (results varchar(250))

if @projid is null and @Project != ''
begin 
set @messages  = (select messagetext from list_messages where messagenum =182)
insert into #validataions1 values (@messages + ' Project Id')
end

if @statusid is null and @Status != ''
begin 
set @messages = (select messagetext from list_messages where messagenum =182)
insert into #validataions1 values (@messages + ' Status')
end

if @shipviaid is null and @Shipvia !=''
begin
set @messages=(select messagetext from list_messages where messagenum=182)
insert into #validataions1 values (@messages + ' Ship Via')
end

select * from #validataions1
drop table #validataions1

End
GO
---------------------------------------------------------------------------------------------------------------------------------------------
-- sp_maintain_Essex_so5
/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_so]    Script Date: 10-08-2022 02:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_maintain_so]  	
 @id	uniqueidentifier=null,  	
 @actionflag smallint,  	
 @somain varchar(50),  	
 @sodate smalldatetime=null,  	
 @custpo varchar(50)='',  	
 @salesperson varchar(50)='',  	
 @salestax money=0,  	
 @commission decimal(18,2)=0,  	
 @customername varchar(50)='',  	
 @status varchar(50)='',  	
 @terms varchar(50)='',  	
 @fob varchar(50)='',  	
 @shipvia varchar(50)='',  	
 @ordertype varchar(50)='',  	
 @billtoname varchar(50)='',	  	
 @billtoaddress1 varchar(50)='',	  	
 @billtoaddress2 varchar(50)='',  	
 @billtoaddress3 varchar(50)='',	  	
 @billtoaddress4 varchar(50)='',	  	
 @billtoaddress5 varchar(50)='',  	
 @billtoaddress6 varchar(50)='',	  	
 @shiptoname varchar(50)='',	  	
 @shiptoaddress1 varchar(50)='',  	
 @shiptoaddress2 varchar(50)='',	  	
 @shiptoaddress3 varchar(50)='',	  	
 @shiptoaddress4 varchar(50)='',  	
 @shiptoaddress5 varchar(50)='',	  	
 @shiptoaddress6 varchar(50)='',	  	
 @Contactsid uniqueidentifier=null,  	
 @sonotes text='',  	
 @shipnotes text='',	  	
 @salespersonnotes text='',  	
 @packnotes text='',  	
 @invoicenotes text='',  	
 @createdby varchar(50)=null,  	
 @user1 varchar(50)='',  
 @user2 varchar(50)='',  
 @user3 varchar(50)='',  
 @user4 varchar(50)='',  
 @user5  varchar(50)='', 
 @user6  varchar(50)='', 
 @user7 varchar(50)='',  
 @user8 varchar(50)='', 
 @est varchar(50)='', 
 @reqdate varchar(50)='', 
 @prcomp varchar(50)='', 
 @funded varchar(50)='',
 @projectid uniqueidentifier=null,  
 @budget varchar(50)='',
 @mdatIn varchar(50)='',
 @org varchar(50)=''
 
 AS  
 Declare  
 @termsid uniqueidentifier,  
 @fobid uniqueidentifier,  
 @shipviaid uniqueidentifier, 
 @ordertypeid uniqueidentifier,  
 @statusid uniqueidentifier,  
 @customersid uniqueidentifier  
 BEGIN  
 Set @termsid = (select TOP 1 id from list_dropdowns where ColNum='940' AND FieldValue = @terms)  
 Set @fobid = (select TOP 1 id from list_dropdowns where ColNum='941' AND FieldValue  = @fob)  
 Set @shipviaid = (select TOP 1 id from list_dropdowns where ColNum='942' AND FieldValue = @shipvia)  
 Set @ordertypeid = (select TOP 1 id from list_dropdowns where ColNum='268' AND FieldValue  = @ordertype)  
 --set @somain=upper(@somain)
 if @ordertypeid IS null  	
 begin  		
 insert into dbo.list_dropdowns (ID,ColNum,FieldName,FieldValue,createdby,CreatedDate) values ('D5DB79D8-7BFA-4174-B374-7B8CA3D0362A',268,'Sales Order Type','RMA','IMPORT',GETDATE())  		
 set @ordertypeid='D5DB79D8-7BFA-4174-B374-7B8CA3D0362A'  	
 end	  
 Set @statusid = (Select top 1 id from dbo.list_status where status = @status)  
 Set @customersid ='924E5C51-BF60-42EB-8781-EA91D4275688' -- (Select top 1 id from dbo.list_customers where customer = @customername)  	
 if @actionflag=1 
 SET @id = NEWID()  	 
 if @actionflag=1  		
 insert into so_main (id, somain,sodate,custpo,salesperson,salestax,commission,customername,customersid ,statusid,termsid,
 fobid,shipviaid ,ordertypeid ,billtoname ,billtoaddress1,billtoaddress2,
 billtoaddress3,billtoaddress4,billtoaddress5,billtoaddress6,shiptoname,shiptoaddress1,
 shiptoaddress2,shiptoaddress3,shiptoaddress4,shiptoaddress5,shiptoaddress6,contactsid,
 sonotes,shipnotes,salespersonnotes,packnotes,invoicenotes,createdby,user1,user2,user3,user4,user5,user6,user7,user8,projectid,budget,est,reqdate,prcomp,funded,mdat_in,org) 
 values(
 @id, upper(@somain),@sodate,upper(@custpo),upper(@salesperson),@salestax,@commission,upper(@customername),@customersid ,@statusid,@termsid,
 @fobid,@shipviaid ,@ordertypeid ,upper(@billtoname) ,@billtoaddress1,@billtoaddress2,
 @billtoaddress3,@billtoaddress4,@billtoaddress5,@billtoaddress6,@shiptoname,@shiptoaddress1,
 @shiptoaddress2,@shiptoaddress3,@shiptoaddress4,@shiptoaddress5,@shiptoaddress6,@contactsid,
 @sonotes,@shipnotes,@salespersonnotes,@packnotes,@invoicenotes,@createdby,
 upper(@user1),upper(@user2),upper(@user3),upper(@user4),upper(@user5),upper(@user6),
 upper(@user7),upper(@user8),@projectid,@budget,@est,@reqdate,@prcomp,@funded,@mdatIn,@org)
 else if @actionflag=2
 update so_main set somain=upper(@somain),sodate=@sodate,custpo=upper(@custpo),salesperson=upper(@salesperson),salestax=@salestax,
 commission=@commission,customername=upper(@customername),customersid=@customersid,
 statusid=@statusid,termsid=@termsid,fobid=@fobid,shipviaid=@shipviaid,ordertypeid =@ordertypeid,
 billtoname =@billtoname,billtoaddress1=@billtoaddress1,billtoaddress2=@billtoaddress2,
 billtoaddress3=@billtoaddress3,billtoaddress4=@billtoaddress4,billtoaddress5=@billtoaddress5,
 billtoaddress6=@billtoaddress6,shiptoname=@shiptoname,shiptoaddress1=@shiptoaddress1,
 shiptoaddress2=@shiptoaddress2,shiptoaddress3=@shiptoaddress3,shiptoaddress4=@shiptoaddress4,
 shiptoaddress5=@shiptoaddress5,shiptoaddress6=@shiptoaddress6,contactsid=@contactsid,
 sonotes=@sonotes,shipnotes=@shipnotes,salespersonnotes=@salespersonnotes,packnotes=@packnotes,invoicenotes=@invoicenotes,user1=upper(@user1)
 ,user2=UPPER(@user2),user3=upper(@user3),user4=upper(@user4),user5=upper(@user5),user6=upper(@user6),user7=upper(@user7),user8=upper(@user8),
 projectid=@projectid,budget=@budget,est=@est,reqdate=@reqdate,prcomp=@prcomp,funded=@funded,mdat_in=@mdatIn,org=@org
 where id=@id  
 else if @actionflag=3
 delete from so_main where id=@id 
 select @id as id  
 
 END
 GO
 --------------------------------------------------------------------------------------------------------------------------------------------

 --sp_maintain_solines5_essex
/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_solines]    Script Date: 10-08-2022 03:06:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_maintain_solines]  
 @somain varchar(50)= '',  
 @customersItem varchar(50)='',  
 @customers_itemsid uniqueidentifier=null,  
 @line int = 0,  
 @itemnumber varchar(50)= '',  
 @rev varchar(50)= '',  
 @description varchar(150)= '',  
 @qty decimal(18,8)=null,  
 @uom varchar(50)= '',  
 @unitcost decimal(18,8)=0,  
 @markup decimal(18,8)=1,  
 @extcost decimal(18,8)=0,  
 @estshipdate datetime= '',  
 @shipdate datetime= '',  
 @warrantyyear varchar(50)= null,  
 @warrantydays varchar(50)= null,  
 @createdby varchar(50)= '',  
 @actionflag smallint,  
 @solineid varchar(50)='',  
 @solinesnotes varchar(MAX)=''  ,
 @user1 varchar(50)='',  
 @linenum smallint = 0,
 @markupflag smallint = 0 
 As  
 Declare @id uniqueidentifier,  
 @somainid uniqueidentifier,  
 @itemsid uniqueidentifier,  
 @locationsid uniqueidentifier,  
 @uomid uniqueidentifier, 
 @projectid uniqueidentifier,
 @contractid uniqueidentifier, 
 @somarkup decimal(18,8) = 1
 BEGIN  
 
 Set @id = NEWID()  
 Set @somainid = (Select top 1 id from dbo.so_main where somain=@somain)  
 Set @customers_itemsid = (Select top 1 id from dbo.list_customers_items where customer_item=@customersItem)  
 Set @itemsid=(select top 1 id from dbo.list_items where itemnumber=@itemnumber and rev=@rev)  
 Set @uomid=(select top 1 id from dbo.list_uoms where uomref =@uom)  
 Set @projectid =(select top 1 projectid from dbo.so_main where somain=@somain)  
 Set @contractid =(select top 1 contractid from dbo.list_projects where id=@projectid)  
 Set @somarkup = (select (isnull(markup1,1)) *  (isnull(markup2,1)) from list_contracts where id=@contractid)
 Set @somarkup = ISNULL(@somarkup,1)   

 print @contractid
 if @actionflag=1  
 
   insert into so_lines (id,somainid,customers_itemsid,itemsid,linenum,itemnumber,revision,quantity,uom,unitcost,estshipdate,
   shipdate,description,warranty_years,warranty_days,createdby,createddate,solinesnotes,user1,markup)
   values  
   (@id,@somainid,@customers_itemsid,@itemsid,@line,@itemnumber,@rev,@qty,@uom,@unitcost,@estshipdate,@shipdate,@description,
   @warrantyyear,@warrantydays,@createdby,getdate(),@solinesnotes,@user1,@somarkup)  
   
 else if @actionflag=2   
 if @markupflag = 1
   update so_lines set customers_itemsid=@customers_itemsid,
   itemsid=@itemsid,linenum=@line,itemnumber=@itemnumber,revision=@rev,quantity=@qty,uom=@uom,
   unitcost=@unitcost,estshipdate=@estshipdate,shipdate=@shipdate,description=@description,warranty_years=@warrantyyear,
   warranty_days=@warrantydays,solinesnotes=@solinesnotes,user1=@user1,markup=@somarkup where id= @solineid
 else
   update so_lines set customers_itemsid=@customers_itemsid,
   itemsid=@itemsid,linenum=@line,itemnumber=@itemnumber,revision=@rev,quantity=@qty,uom=@uom,
   unitcost=@unitcost,estshipdate=@estshipdate,shipdate=@shipdate,description=@description,warranty_years=@warrantyyear,
   warranty_days=@warrantydays,solinesnotes=@solinesnotes,user1=@user1 where id= @solineid
 
 
 else if @actionflag=3  
   delete from so_lines where id = @solineid
   
 END
 GO

 -----------------------------------------------------------------------------------------------------------------------------------------

 --Below solines query used for copy to new and project transfer view(top grid populate)
/****** Object:  StoredProcedure [dbo].[amics_sp_api_so_lines_copy]    Script Date: 11-08-2022 08:52:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_so_lines_copy] 
@somain varchar(50)=''
AS
BEGIN  	
	
Select so_lines.id,list_itemtypes.itemtype,linenum,dbo.so_lines.itemnumber,revision,dbo.so_lines.description,so_lines.itemsid,list_customers_items.customer_item as customerItem,
quantity,SUM(isnull(inv_pick.pick_quantity,0)) as pickedQty,uom,so_lines.user1,unitcost,ISNULL(dbo.so_lines.markup,1) as markup,
(quantity*unitcost) as extCost, convert(varchar,estshipdate,1) as estshipdate, convert(varchar,shipdate,1) as shipdate,warranty_years,warranty_days, 
ISNULL(dbo.list_items.cost,1) as cost, ISNULL(dbo.so_lines.unitcost,1)*ISNULL(dbo.list_items.markup,1) as pricemarkup,
ISNULL(dbo.list_items.cost,1)*ISNULL(dbo.list_items.markup,1) as pmcostmarkup,
solinesnotes, dbo.list_items.obsolete,isnull(case when list_items.invtypeid='A834DD54-EFE3-451E-8AC1-4ABFD866F5C6' then (select isnull(sum(quantity),0) from inv_serial 
where inv_serial.so_linesid=so_lines.id and inv_serial.transferid is null) + (select isnull(sum(quantity),0) from inv_serial where inv_serial.transferid=so_lines.id)  
else (select sum(quantity) from inv_basic where inv_basic.so_linesid=so_lines.id and inv_basic.itemsid= dbo.list_items.id) end,0 ) as availqty from so_lines left outer join list_customers_items on list_customers_items.id = so_lines.customers_itemsid 
left outer join inv_pick on inv_pick.sources_refid = so_lines.id LEFT OUTER JOIN list_items ON list_items.id = so_lines.itemsid left outer join list_itemtypes on list_itemtypes.id = list_items.itemtypeid where somainid=(select top 1 id from so_main where  somain=@somain) 
group by so_lines.id,list_itemtypes.itemtype,linenum,dbo.so_lines.itemnumber,revision,dbo.so_lines.description,so_lines.itemsid,list_customers_items.customer_item,quantity,uom,so_lines.user1, 
unitcost,dbo.so_lines.markup,dbo.list_items.markup,dbo.list_items.cost,estshipdate,shipdate,warranty_years,warranty_days,solinesnotes,dbo.list_items.obsolete,list_items.invtypeid,list_items.id order by linenum 

END

---------------------------------------------------------------------------------------------------------------------------------------------

--sp_proj_transfer5
/****** Object:  StoredProcedure [dbo].[amics_sp_api_proj_transfer]    Script Date: 12-08-2022 03:44:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_proj_transfer]

    @xfr_transnum int,@xfr_user varchar(50),@xfr_trans varchar(50)='CHG LOC',@xfr_source2 varchar(50)=''

    ,@lic_plate_flag bit =0

    ,@xfr_somain varchar(50)=''

    ,@xfr_tosomain varchar(50)=''

    ,@xfr_fromline varchar(50)=''

    ,@xfr_toline varchar(50)=''

    ,@xfr_tosolinesid uniqueidentifier = null

AS

BEGIN

    -- Begin declare

    declare @xfr_translogid1 uniqueidentifier,@xfr_translogid2 uniqueidentifier,@sourcesid uniqueidentifier,

        @reasonid uniqueidentifier,@xfr_source varchar(50),

        @xfr_invbasicid_from uniqueidentifier,@xfr_invbasicid_to uniqueidentifier,

        @xfr_invbasicid_exist uniqueidentifier,@xfr_invserialid_exist uniqueidentifier,

        @xfr_itemid uniqueidentifier,@xfr_fromlocid uniqueidentifier,@xfr_tolocid uniqueidentifier,

        @xfr_invserialid uniqueidentifier,@xfr_refid uniqueidentifier,@invxfrid uniqueidentifier,

        @xfr_transqty decimal(18,8),@xfr_licplt varchar(50),@xfr_expdate smalldatetime,

        @xfr_priorqty_from decimal(18,8),@xfr_priorqty_to decimal(18,8),@xfr_lot varchar(50),@xfr_color varchar(50),

        @xfr_serno varchar(50),@xfr_tagno varchar(50),@xfr_cost money,@xfr_model varchar(50),

        @xfr_fromwh varchar(50),@xfr_fromloc varchar(50),@xfr_towh varchar(50),@xfr_toloc varchar(50),

        @xfr_item varchar(50),@xfr_rev varchar(50),@xfr_desc varchar(75),

        @xfr_type varchar(50),@xfr_itemtype varchar(50),@xfr_transdate smalldatetime,@xfr_sources_refid uniqueidentifier,

        @from_warehouse varchar(50),@lic_plate varchar(50),@xfr_notes varchar(max),@xfr_extid uniqueidentifier,

        @somainid uniqueidentifier,@er varchar(50),@solinesid uniqueidentifier--,@somain1 varchar(50),@somain2 varchar(50)


    declare @xfr_somainid uniqueidentifier = null,@xfr_tosomainid uniqueidentifier = null,@xfr_solinesid uniqueidentifier = null

    set @xfr_somainid= (select id from so_main where somain=@xfr_somain)

    set @xfr_tosomainid= (select id from so_main where somain=@xfr_tosomain)

    set @xfr_solinesid= (select id from so_lines where somainid=@xfr_somainid and linenum=@xfr_fromline)

--    set @xfr_tosolinesid= (select id from so_lines where somainid=@xfr_tosomainid and linenum=@xfr_toline)
    -- End declare

        set @from_warehouse=(select top 1 warehouse from inv_trans inner join list_locations on inv_trans.fromlocid=list_locations.id

            inner join list_warehouses on list_locations.warehousesid=list_warehouses.id where inv_trans.transnum=@xfr_transnum)

        exec amics_sp_api_pick_fifo @fifo_transnum=@xfr_transnum,@fifo_warehouse=@from_warehouse

    -- Begin Initialize

    set @xfr_translogid1=(select newid())

    set @xfr_translogid2=(select newid())

    set @invxfrid=(select newid())

    set @xfr_transdate=getdate()

    set @sourcesid=(select id from inv_sources where sourceref=@xfr_source)

    -- End Initialize

    -- Begin Process Transactions

    declare cuMyCursor Cursor For (select itemsid,invbasicid,invserialid,refid,tolocid,transqty,notes

         from inv_trans where transnum=@xfr_transnum )

    Open cuMyCursor

    Fetch Next from cuMyCursor into @xfr_itemid,@xfr_invbasicid_from,@xfr_invserialid,@xfr_refid,@xfr_tolocid,@xfr_transqty,@xfr_notes

    while @@fetch_status = 0

    begin

        select @xfr_itemid=

            case

                when @xfr_invbasicid_from is not null then (select itemsid from inv_basic where id=@xfr_invbasicid_from)

                when @xfr_invserialid is not null then (select itemsid from inv_serial where id=@xfr_invserialid)

            end

        select @xfr_cost=

            case

                when @xfr_invbasicid_from is not null then (select cost from inv_basic where id=@xfr_invbasicid_from)

                when @xfr_invserialid is not null then (select cost from inv_serial where id=@xfr_invserialid)

            end

       

        set @xfr_item=(select itemnumber from list_items where id=@xfr_itemid)

        set @xfr_rev=(select rev from list_items where id=@xfr_itemid)

        set @xfr_desc=(select description from list_items where id=@xfr_itemid)

        set    @xfr_itemtype=dbo.amics_fn_api_itemtype(@xfr_item,@xfr_rev)

        set @xfr_type=dbo.amics_fn_api_invtype(@xfr_item,@xfr_rev)
		

        if @xfr_invbasicid_from is not null

            set @xfr_fromlocid=(select locationsid from inv_basic where id=@xfr_invbasicid_from)

        else

            set @xfr_fromlocid=(select locationsid from inv_serial where id=@xfr_invserialid)

        if @xfr_invbasicid_from is not null and @xfr_type='BASIC'

        begin

            set @xfr_lot=''

            set @xfr_priorqty_from=(select quantity from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_sources_refid=(select sources_refid from inv_basic where id=@xfr_invbasicid_from)

        end

        if @xfr_invbasicid_from is not null and @xfr_type='LOT'

        begin

            set @xfr_lot=(select lotno from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_lot=isnull(@xfr_lot,'')

            set @xfr_color=(select color from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_priorqty_from=(select quantity from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_sources_refid=(select sources_refid from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_licplt=(select lic_plate from inv_basic where id=@xfr_invbasicid_from)           

            set @xfr_expdate=(select expdate from inv_basic where id=@xfr_invbasicid_from)

            set @xfr_extid=(select extendedid from inv_basic where id=@xfr_invbasicid_from)

        end

        if @xfr_invserialid is not null and @xfr_type='SERIAL'

        begin

            set @xfr_serno=(select serno from inv_serial where id=@xfr_invserialid)

            set @xfr_tagno=(select tagno from inv_serial where id=@xfr_invserialid)

            set @xfr_model=(select model from inv_serial where id=@xfr_invserialid)

            set @xfr_priorqty_from=(select quantity from inv_serial where id=@xfr_invserialid)

            set @lic_plate=(select lic_plate_number from inv_serial where id=@xfr_invserialid)

                     set @somainid=(select somainid from so_lines  where so_lines.id=@xfr_tosolinesid)

                     --set @somain1=(select somain from so_main where id=@somainid)

            update inv_serial set transferid=@xfr_tosolinesid,so_mainid=@somainid from inv_serial where inv_serial.id = @xfr_invserialid

        end

        set @xfr_fromwh =(SELECT dbo.list_warehouses.warehouse FROM dbo.list_warehouses INNER JOIN

                dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid where

                list_locations.id=@xfr_fromlocid)

        set @xfr_towh =(SELECT dbo.list_warehouses.warehouse FROM dbo.list_warehouses INNER JOIN

                dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid where

                list_locations.id=@xfr_tolocid)

        set @xfr_fromloc=(SELECT dbo.list_locations.location FROM dbo.list_locations where list_locations.id=@xfr_fromlocid)

        set @xfr_toloc=(SELECT dbo.list_locations.location FROM dbo.list_locations where list_locations.id=@xfr_tolocid)
		
----- declate project transfer location--------

       declare @xfr_fromwhid uniqueidentifier,@xfr_towhid uniqueidentifier    

       set @xfr_fromwhid =(SELECT dbo.list_warehouses.id FROM dbo.list_warehouses INNER JOIN

                dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid where

                list_locations.id=@xfr_fromlocid)

    

        set @xfr_towhid =(SELECT dbo.list_warehouses.id FROM dbo.list_warehouses INNER JOIN

                dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid where

                list_locations.id=@xfr_tolocid)

------------------------------------------------
                

        -- Start Updates and inserts within the loop

    insert into inv_transfer (locationsid_from,locationsid_to,inv_basicid_from,inv_basicid_to,

        inv_serialid,priorqty,priorqty2,thisqty,createdby,transfernum,notes)

        values (@xfr_Fromlocid,@xfr_tolocid,@xfr_invbasicid_from,@xfr_invbasicid_to,@xfr_invserialid,

        @xfr_priorqty_from,@xfr_priorqty_to,@xfr_transqty,@xfr_user,@xfr_transnum,@xfr_notes)

 

        if @xfr_invbasicid_from is not null

        begin

            update inv_basic set quantity=(quantity-@xfr_transqty) where id=@xfr_invbasicid_from

            if @xfr_sources_refid is null

            set @xfr_invbasicid_exist=(select id from inv_basic where itemsid=@xfr_itemid

                and locationsid=@xfr_tolocid and cost=@xfr_cost and isnull(lotno,'')=isnull(@xfr_lot,'') and isnull(color,'')=isnull(@xfr_color,'') and lic_plate=@xfr_licplt and sources_refid is null)

            else

            set @xfr_invbasicid_exist=(select id from inv_basic where itemsid=@xfr_itemid

                and locationsid=@xfr_tolocid and cost=@xfr_cost and isnull(lotno,'')=isnull(@xfr_lot,'') and isnull(color,'')=isnull(@xfr_color,'') and sources_refid=@xfr_sources_refid and lic_plate=@xfr_licplt)

       

            if @xfr_invbasicid_exist is not null

                begin

                                  set @solinesid=(select so_linesid from inv_basic where id=@xfr_invbasicid_exist)

                                  set @er=(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@solinesid)

                       update inv_basic set quantity=quantity+@xfr_transqty,er=@er where id=@xfr_invbasicid_exist

                          set @lic_plate = (select lic_plate_number from inv_basic where id=@xfr_invbasicid_exist)

                end

            else

                begin

               

                   declare @invbasicid uniqueidentifier 

                    set @invbasicid =(select newid())

                                  set @er=(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=@xfr_tosolinesid)

                   

                    insert into inv_basic (id,locationsid,itemsid,quantity,cost,lotno,createdby,color,sources_refid,lic_plate,expdate,extendedid,so_linesid,er) values

                    (@invbasicid,@xfr_tolocid,@xfr_itemid,@xfr_transqty,@xfr_cost,@xfr_lot,@xfr_user,@xfr_color,@xfr_sources_refid,@xfr_licplt,@xfr_expdate,@xfr_extid,@xfr_tosolinesid,@er)

                   

                    if @xfr_sources_refid is null

                        set @lic_plate=(select lic_plate_number from inv_basic where itemsid=@xfr_itemid

                        and locationsid=@xfr_tolocid and cost=@xfr_cost and isnull(lotno,'')=isnull(@xfr_lot,'') and isnull(color,'')=isnull(@xfr_color,'')  and lic_plate=@xfr_licplt and sources_refid is null)

                    else

                        set @lic_plate=(select lic_plate_number from inv_basic where itemsid=@xfr_itemid

                        and locationsid=@xfr_tolocid and cost=@xfr_cost and isnull(lotno,'')=isnull(@xfr_lot,'') and isnull(color,'')=isnull(@xfr_color,'')  and lic_plate=@xfr_licplt and sources_refid=@xfr_sources_refid)
						              

        insert into dbo.translog_project(id,itemsid,somainid,tosomainid,solinesid,tosolinesid,fromwhsid,fromlocid,towhsid,tolocid,basicid,transqty,translogid,createddate,createdby,translogid2)

        values(NEWID(),@xfr_itemid,@xfr_somainid,@xfr_tosomainid,@xfr_solinesid,@xfr_tosolinesid,@xfr_fromwhid,@xfr_fromlocid,@xfr_towhid,@xfr_tolocid,@invbasicid,@xfr_transqty,@xfr_translogid1,GETDATE(),@xfr_user,@xfr_translogid2)

            end

        end

        if @xfr_invserialid is not null

        begin     

                     set @solinesid =(select isnull(inv_serial.transferid,inv_serial.so_linesid) from inv_serial where id=@xfr_invserialid)

                     set @somainid=(select somainid from so_lines  where so_lines.id=@solinesid )

--                   set @somain2=(select somain from so_main where id=@somainid)

            update inv_serial set quantity=0,so_mainid=@somainid where id=@xfr_invserialid

            update inv_serial set locationsid=@xfr_tolocid,quantity=1,so_mainid=@somainid where id=@xfr_invserialid

            set @lic_plate = (select lic_plate_number from inv_serial where id=@xfr_invserialid)

           

        end

        if @xfr_type='LOT'

        begin

            exec amics_sp_api_update_transloglot

                @translogid        =    @xfr_translogid1,

                @quantity        =    @xfr_transqty,

                @lotno            =    @xfr_lot,

                @color            =    @xfr_color,

                @user            =    @xfr_user,

                @cost            =    @xfr_Cost,

                @loc            =    @xfr_fromloc,

                @whs            =    @xfr_fromwh

               

            exec amics_sp_api_update_transloglot

                @translogid        =    @xfr_translogid2,

                @quantity        =    @xfr_transqty,

                @lotno            =    @xfr_lot,

                @color            =    @xfr_color,

                @user            =    @xfr_user,

                @cost            =    @xfr_Cost,

                @loc            =    @xfr_toloc,

                @whs            =    @xfr_towh,

                @lic_plate        =   @lic_plate,

                @lic_plate_flag    =    @lic_plate_flag

        end

        if @xfr_type='SERIAL'

        begin

                exec  amics_sp_api_update_translogsn

                @translogid        =    @xfr_translogid1,

                @serno            =    @xfr_serno,

                @tagno            =    @xfr_tagno,

                @model            =    @xfr_model,

                @user            =    @xfr_user,

                @cost            =    @xfr_cost,

                @loc            =    @xfr_fromloc,

                @whs            =    @xfr_fromwh,

                @lic_plate        =   @lic_plate,

                @lic_plate_flag    =    @lic_plate_flag

                

                exec  amics_sp_api_update_translogsn

                @translogid        =    @xfr_translogid2,

                @serno            =    @xfr_serno,

                @tagno            =    @xfr_tagno,

                @model            =    @xfr_model,

                @user            =    @xfr_user,

                @cost            =    @xfr_Cost,

                @loc            =    @xfr_toloc,

                @whs            =    @xfr_towh,

                @lic_plate        =   @lic_plate,

                @lic_plate_flag    =    @lic_plate_flag

    

 

    

 

                       

        insert into dbo.translog_project(id,itemsid,somainid,tosomainid,solinesid,tosolinesid,fromwhsid,fromlocid,towhsid,tolocid,serialid,transqty,translogid,createddate,createdby,translogid2)

        values(NEWID(),@xfr_itemid,@xfr_somainid,@xfr_tosomainid,@xfr_solinesid,@xfr_tosolinesid,@xfr_fromwhid,@xfr_fromlocid,@xfr_towhid,@xfr_tolocid,@xfr_invserialid,1.00,@xfr_translogid1,GETDATE(),@xfr_user,@xfr_translogid2)

       

        --insert into dbo.translog_project(id,itemsid,somainid,tosomainid,solinesid,tosolinesid,fromwhsid,fromlocid,towhsid,tolocid,serialid,transqty,translogid,createddate,createdby)

        --values(NEWID(),@xfr_itemid,@xfr_somainid,@xfr_tosomainid,@xfr_solinesid,@xfr_tosolinesid,@xfr_fromwhid,@xfr_fromlocid,@xfr_towhid,@xfr_tolocid,@xfr_invserialid,-1,@xfr_translogid2,GETDATE(),@xfr_user)

                

        end

        -- End Updates and inserts within the loop

               

    Fetch Next from cuMyCursor into @xfr_itemid,@xfr_invbasicid_from,@xfr_invserialid,@xfr_refid,@xfr_tolocid,@xfr_transqty,@xfr_notes

    end

    close cuMyCursor

    deallocate cuMyCursor

    if @xfr_type='SERIAL' set @xfr_transqty=(select count(*) from translogsn where translogid=@xfr_translogid1)

    if @xfr_type='SERIAL' set @lic_plate=null

    if @xfr_type='SERIAL' set @lic_plate_flag=null

    if @xfr_type='LOT' set @xfr_transqty=(select sum(quantity) from transloglot where translogid=@xfr_translogid1)

    else set @xfr_transqty=@xfr_transqty

    if @xfr_type='LOT' set @lic_plate=null

    if @xfr_type='LOT' set @lic_plate_flag=null

   

    set @xfr_transqty=@xfr_transqty*-1

    exec amics_sp_api_update_translog

    @translogid        =    @xfr_translogid1,

    @itemid            =    @xfr_itemid,

    @Invtype        =    @xfr_type,

    @Itemtype        =    @xfr_itemtype,

    @Rev            =    @xfr_rev,

    @trans            =    @xfr_trans,

    @item            =    @xfr_Item,

    @Description    =    @xfr_desc,

    @Warehouse        =    @xfr_fromwh,

    @Location        =    @xfr_fromloc,

    @Reason            =    @xfr_source2,

    @Source            =    '',

    @Ref            =    '',

    @notes            =    @xfr_notes,

    @Quantity        =    @xfr_transqty,

    @Cost            =    @xfr_cost,

    @transdate        =    @xfr_transdate,

    @user            =    @xfr_user,

       @userfield2          =      @xfr_somain

 

WAITFOR DELAY '000:00:02'    -- wait for 2 seconds

 

    set @xfr_transqty=@xfr_transqty*-1

    exec amics_sp_api_update_translog

    @translogid        =    @xfr_translogid2,

    @itemid            =    @xfr_itemid,

    @Invtype        =    @xfr_type,

    @Itemtype        =    @xfr_itemtype,

    @Rev            =    @xfr_rev,

    @trans            =    @xfr_trans,

    @item            =    @xfr_Item,

    @Description    =    @xfr_desc,

    @Warehouse        =    @xfr_towh,

    @Location        =    @xfr_toloc,

    @Reason            =    @xfr_source2,

    @Source            =    '',

    @Ref            =    '',

    @notes            =    @xfr_notes,

    @Quantity        =    @xfr_transqty,

    @Cost            =    @xfr_cost,

    @transdate        =    @xfr_transdate,

    @user            =    @xfr_user,

    @lic_plate        =   @lic_plate,

    @lic_plate_flag    =    @lic_plate_flag ,

    @userfield2          =      @xfr_tosomain

    -- End Updates and inserts outside the loop 

    -- End Process Transactions

END
GO
 
----------------------------------------------------------------------------------------------------------------------------------------

--sp_inv_somain_details_items_combined
/****** Object:  StoredProcedure [dbo].[amics_sp_api_inv_somain_details_items_combined]    Script Date: 12-08-2022 03:56:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE [dbo].[amics_sp_api_inv_somain_details_items_combined]
    @somain varchar(50),
    @itemnumber varchar(50)
AS
BEGIN

declare @solinesid uniqueidentifier,@linenum smallint,@itemtype varchar(50),@desc varchar(150),@qty decimal (18,0),@itemsid uniqueidentifier
--- BASIC CALCULATIONS START----
	if (select top 1 invtype from list_items inner join list_invtypes on list_items.invtypeid=list_invtypes.id where itemnumber=@itemnumber ) = 'BASIC'
	SELECT dbo.list_warehouses.warehouse, NULL AS serialid, dbo.inv_basic.id as basicid, dbo.inv_basic.cost, 
	0 AS sncost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, 
	dbo.so_lines.quantity, dbo.list_locations.location, dbo.inv_basic.quantity AS available, dbo.so_main.somain, 
	dbo.so_lines.id AS solinesid, '' AS serno, '' AS tagno, SUM(SUM(ISNULL(dbo.inv_basic.quantity, 0))) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
	FROM            dbo.list_items LEFT OUTER JOIN
	 dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id RIGHT OUTER JOIN
	 dbo.list_warehouses RIGHT OUTER JOIN
	 dbo.list_locations ON dbo.list_warehouses.id = dbo.list_locations.warehousesid RIGHT OUTER JOIN
	 dbo.inv_basic ON dbo.list_locations.id = dbo.inv_basic.locationsid ON dbo.list_items.id = dbo.inv_basic.itemsid RIGHT OUTER JOIN
	 dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id RIGHT OUTER JOIN
	 dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id
	GROUP BY dbo.list_warehouses.warehouse, dbo.inv_basic.id, dbo.inv_basic.cost, dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber, dbo.list_items.description, dbo.so_lines.quantity, 
	 dbo.list_locations.location, dbo.inv_basic.quantity, dbo.so_main.somain, dbo.so_lines.id
	HAVING        (dbo.list_items.itemnumber = @itemnumber) AND (dbo.so_main.somain != @somain) AND isnull(dbo.inv_basic.quantity,0) >0
--- BASIC CALCULATIONS END
else  -- Serial items 
	begin -- ELSE SERIAL CALCULATIONS START ---
		set @solinesid=(select so_lines.id  from so_lines inner join so_main on so_lines.somainid=so_main.id where somain=@somain and itemnumber=@itemnumber)
		set @itemsid=(select top 1  itemsid from so_lines where id=@solinesid )
		set @linenum=(select linenum from so_lines where id=@solinesid)
		set @desc=(select description from so_lines where id=@solinesid)
		set @qty=(select quantity from so_lines where id=@solinesid)
		set @itemtype =(select isnull(itemtype,'') as itemtype from so_lines 
			left join list_items on so_lines.itemsid =list_items.id 
			left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id
			where so_lines.id=@solinesid )
		select * from ( SELECT     TOP (100) PERCENT 
			dbo.list_warehouses.warehouse,dbo.inv_serial.id as serialid,  null as basicid,0 as cost,dbo.inv_serial.cost as sncost,
			@linenum as linenum,@itemtype as itemtype ,@itemnumber as itemnumber,@desc as description,@qty as quantity,
			list_locations.location AS location,ISNULL(dbo.inv_serial.quantity, 0) AS Available,
			isnull(
				(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=inv_serial.transferid),
				(select somain from so_lines inner join so_main on so_lines.somainid=so_main.id where so_lines.id=inv_serial.so_linesid)
				) as somain
			,@solinesid as solinesid ,
			ISNULL(dbo.inv_serial.serno, '') AS serno,ISNULL(dbo.inv_serial.tagno, '') AS tagno, 1 as total
			FROM   inv_serial 
			left join list_locations on inv_serial.locationsid=list_locations.id 
			left join list_warehouses on list_locations.warehousesid =list_warehouses.id 
			where inv_serial.itemsid=@itemsid and inv_serial.quantity>0) as x where somain!=@somain 

	END
END -- END OF PROCEDURE
--UNION
--select * from (
--SELECT     TOP (100) PERCENT 
--dbo.list_warehouses.warehouse,dbo.inv_serial.id as serialid, '' as basicid,
--0 as cost,dbo.inv_serial.cost as sncost,dbo.so_lines.linenum,dbo.list_itemtypes.itemtype,
--dbo.so_lines.itemnumber,dbo.so_lines.description,dbo.so_lines.quantity,list_locations_1.location AS location,
--ISNULL(dbo.inv_serial.quantity, 0) AS Available,
--dbo.so_main.somain,dbo.so_lines.id AS solinesid,ISNULL(dbo.inv_serial.serno, '') AS serno, 
--ISNULL(dbo.inv_serial.tagno, '') AS tagno,1 as total
----SUM(ISNULL(dbo.inv_serial.quantity, 0)) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
--FROM   -- inv_basic join is doing nothing
--dbo.list_locations AS list_locations_1 RIGHT OUTER JOIN
--dbo.list_itemtypes RIGHT OUTER JOIN
--dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN
--dbo.inv_serial ON dbo.list_items.id = dbo.inv_serial.itemsid RIGHT OUTER JOIN 
--dbo.so_lines ON dbo.so_lines.id = dbo.inv_serial.transferid or (dbo.so_lines.id = dbo.inv_serial.so_linesid and dbo.inv_serial.transferid is null) RIGHT OUTER JOIN 
--dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id ON list_locations_1.id = dbo.inv_serial.locationsid RIGHT OUTER JOIN
--dbo.list_warehouses ON  list_locations_1.warehousesid =  dbo.list_warehouses.id
--GROUP BY dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
--dbo.so_main.somain, dbo.inv_serial.pickid, dbo.so_lines.id, 
--ISNULL(dbo.inv_serial.serno, ''), ISNULL(dbo.inv_serial.tagno, ''),
--dbo.inv_serial.quantity,dbo.list_warehouses.warehouse, list_locations_1.location,  
--dbo.inv_serial.id ,dbo.inv_serial.cost,inv_serial.transferid

--HAVING (dbo.so_main.somain !=  @somain) AND (dbo.so_lines.quantity > 0) AND inv_serial.transferid is not null and 
--ISNULL(dbo.inv_serial.quantity, 0) >0 AND 
--dbo.so_lines.itemnumber= @itemnumber
--ORDER BY dbo.so_lines.linenum
--)
 --as y


-----------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_project_transfer]    Script Date: 12-08-2022 06:51:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_project_transfer]
  @somain varchar(50),
  @actionflag int =0
  As
  Begin
      if @actionflag = 0 -- FROM PROJECT
      begin
            
            
			select distinct Mfr,[Part Number],[Description], [From ER], sum(transqty) as [Task Qty],[To ER],[Trans Date],itemsid,somainid,tosomainid,invtype from ( 
			SELECT TOP (100) PERCENT dbo.list_itemtypes.itemtype AS MFR, dbo.list_items.itemnumber AS [Part Number], dbo.list_items.description AS Description, 
			(SELECT somain FROM dbo.so_main WHERE (id = dbo.translog_project.somainid)) AS [From ER], sum(dbo.translog_project.transqty) as transqty, 
			(SELECT somain FROM dbo.so_main AS so_main_1 WHERE (id = dbo.translog_project.tosomainid)) AS [To ER],convert(varchar,dbo.translog_project.createddate,101) AS [Trans Date], 
			dbo.translog_project.itemsid,dbo.translog_project.somainid, dbo.translog_project.tosomainid, dbo.translog_project.solinesid, dbo.translog_project.serialid,dbo.list_invtypes.invtype FROM 
			dbo.list_itemtypes RIGHT OUTER JOIN dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN 
			dbo.translog_project ON dbo.list_items.id = dbo.translog_project.itemsid  RIGHT OUTER JOIN 
			dbo.list_invtypes ON dbo.list_invtypes.id = dbo.list_items.invtypeid 
			group by dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber,Description,dbo.translog_project.somainid, 
			dbo.translog_project.tosomainid,dbo.translog_project.createddate,dbo.translog_project.itemsid, 
			dbo.translog_project.solinesid,dbo.translog_project.serialid,dbo.list_invtypes.invtype 
			ORDER BY MFR) as x group by mfr,[Part Number],[Description], [From ER], transqty,[To ER],[Trans Date],itemsid,somainid,tosomainid,invtype 
			having [From ER]=@somain order by mfr 
             
		 
	 end
	 else -- TO PROJECT
	 begin	     
		   select distinct Mfr,[Part Number],[Description], [From ER], sum(transqty) as [Task Qty],[To ER],[Trans Date],itemsid,somainid,tosomainid,invtype from ( 
             SELECT TOP (100) PERCENT dbo.list_itemtypes.itemtype AS MFR, dbo.list_items.itemnumber AS [Part Number], dbo.list_items.description AS Description, 
             (SELECT somain FROM dbo.so_main WHERE (id = dbo.translog_project.somainid)) AS [From ER], sum(dbo.translog_project.transqty) as transqty, 
             (SELECT somain FROM dbo.so_main AS so_main_1 WHERE (id = dbo.translog_project.tosomainid)) AS [To ER],convert(varchar,dbo.translog_project.createddate,101) AS [Trans Date], 
             dbo.translog_project.itemsid,dbo.translog_project.somainid, dbo.translog_project.tosomainid, dbo.translog_project.solinesid, dbo.translog_project.serialid,dbo.list_invtypes.invtype FROM 
             dbo.list_itemtypes RIGHT OUTER JOIN dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN 
             dbo.translog_project ON dbo.list_items.id = dbo.translog_project.itemsid  RIGHT OUTER JOIN 
             dbo.list_invtypes ON dbo.list_invtypes.id = dbo.list_items.invtypeid 
             group by dbo.list_itemtypes.itemtype, dbo.list_items.itemnumber,Description,dbo.translog_project.somainid, 
             dbo.translog_project.tosomainid,dbo.translog_project.createddate,dbo.translog_project.itemsid, 
             dbo.translog_project.solinesid,dbo.translog_project.serialid,dbo.list_invtypes.invtype 
             ORDER BY MFR) as x group by mfr,[Part Number],[Description], [From ER], transqty,[To ER],[Trans Date],itemsid,somainid,tosomainid,invtype 
             having [To ER]=@somain order by mfr 
	 end
End
Go


-----------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_project_transfer_details]    Script Date: 15-08-2022 02:59:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_project_transfer_details]
@itemsid uniqueidentifier,
@fromsomainid uniqueidentifier,
@tosomainid uniqueidentifier,
@tranoption varchar(10),
@itemtype varchar(10)
As
begin
if @tranoption='ERIN'
begin
   if @itemtype = 'SERIAL'
begin
select serno,tagno,inv_serial.cost as cost from translog_project 
inner join inv_serial on translog_project.serialid=inv_serial.id
 where translog_project.itemsid=@itemsid and somainid=@fromsomainid and tosomainid=@tosomainid
end
else
begin
   select location,transqty,inv_basic.cost as cost from translog_project 
   inner join inv_basic on translog_project.basicid=inv_basic.id inner join list_locations on list_locations.id=inv_basic.locationsid 
   where translog_project.itemsid=@itemsid and somainid=@fromsomainid and tosomainid=@tosomainid
end
end
else if @tranoption='EROUT'
begin
   if @itemtype = 'SERIAL'
begin
select serno,tagno,inv_serial.cost as cost from translog_project 
inner join inv_serial on translog_project.serialid=inv_serial.id
 where translog_project.itemsid=@itemsid and somainid=@tosomainid and tosomainid =@fromsomainid
end
else
begin
   select location,transqty,inv_basic.cost as cost from translog_project 
   inner join inv_basic on translog_project.basicid=inv_basic.id inner join list_locations on list_locations.id=inv_basic.locationsid 
   where translog_project.itemsid=@itemsid and somainid=@fromsomainid and tosomainid=@tosomainid
end
end 
end
Go

--------------------------------------------------------------------------------------------------------------------------------------------------

--Sales Order - Option Shipments
/****** Object:  StoredProcedure [dbo].[amics_sp_api_soshipment_validpacklstnum]    Script Date: 15-08-2022 06:46:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_soshipment_validpacklstnum]     
@packlstnum varchar(50)

AS    
BEGIN   
	
	Select distinct packlist from dbo.inv_soship inner join inv_pick on  inv_pick.inv_soshipid=inv_soship.id where packlist=@packlstnum and inv_pick.pick_quantity>0

End
Go

-----------------------------------------------------------------------------------------------------------------------------------------------
--Sales Order - Option Shipments
/****** Object:  StoredProcedure [dbo].[amics_sp_api_so_getshipments]    Script Date: 15-08-2022 06:46:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_so_getshipments]     
@somain varchar(50)

AS    
BEGIN    
Select  inv_soship.packlist, '' as custpo, list_items.itemnumber,list_items.rev as revision,list_items.description,convert(varchar,inv_soship.shipdate,1) as shipdate , 
SUM(ISNULL(dbo.inv_pick.pick_quantity, 0)) AS shippedqty, convert(varchar,dbo.so_lines.estshipdate,1) as estshipdate, inv_pick.createdby,
inv_soship.trackingnum as mdatout from  so_lines 
inner join list_items on list_items.id = so_lines.itemsid 
inner join inv_pick on inv_pick.sources_refid =so_lines.id 
inner join inv_soship on inv_soship.id = inv_pick.inv_soshipid 
inner join so_main on so_main.id = so_lines.somainid where so_main.id = @somain  and dbo.inv_pick.pick_quantity>0 
group by inv_soship.packlist,list_items.itemnumber,list_items.rev,list_items.description,inv_soship.shipdate,dbo.so_lines.estshipdate, 
inv_pick.createdby,inv_soship.trackingnum 
End
Go
----------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_sopo]    Script Date: 16-08-2022 02:57:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure  [dbo].[amics_sp_api_view_sopo]    
 @somainid uniqueidentifier
as
BEGIN 
SELECT '' AS dummy1, '' AS dummy2, x.itemtype, x.itemnumber, x.description, x.pomain, x.Ordered, x.unitcost, 
x.unitcost*x.ordered AS extcost, x.deliverydate, x.Received, x.status, x.requisition, 
x.solineitem, x.polinesid, x.transferpo, x.itemnumber2, x.description2,x.invtype,x.pomainid FROM  

(SELECT        ISNULL(dbo.list_itemtypes.itemtype, '') AS itemtype, ISNULL(dbo.so_lines.itemnumber, dbo.po_lines.poitem) AS itemnumber, ISNULL(dbo.so_lines.description, 
                         dbo.po_lines.description) AS description, ISNULL(dbo.po_main.pomain, '') AS pomain, ISNULL(dbo.po_lines.quantity, 0) AS Ordered, ISNULL(dbo.po_lines.unitcost, 0) 
                         AS unitcost, dbo.po_lines.unitcost * dbo.po_lines.quantity AS extcost, ISNULL(SUM(dbo.inv_receipts.recd_quantity), 0) + ISNULL(SUM(dbo.amics_v_api_serialqty.serialqty), 0) 
                         AS Received, dbo.po_lines.deliverydate, ISNULL(dbo.list_status.status, '') AS status, dbo.so_lines.id AS solinesid, dbo.so_lines.somainid, 
                         ISNULL(dbo.po_main.requisition, '') AS requisition, dbo.so_lines.linenum, dbo.po_main.id AS pomainid, ISNULL(dbo.so_lines.itemnumber, '') AS solineitem, 
                         dbo.po_lines.id AS polinesid, ISNULL(dbo.po_main.transferpo, 0) AS transferpo, ISNULL(dbo.po_lines.poitem, dbo.so_lines.itemnumber) AS itemnumber2, 
                         ISNULL(dbo.po_lines.description, dbo.so_lines.description) AS description2,dbo.list_invtypes.invtype
FROM            dbo.list_items LEFT OUTER JOIN
                         dbo.list_itemtypes ON dbo.list_items.itemtypeid = dbo.list_itemtypes.id RIGHT OUTER JOIN
                         dbo.inv_receipts RIGHT OUTER JOIN
                         dbo.so_lines INNER JOIN
                         dbo.po_lines ON dbo.so_lines.id = dbo.po_lines.so_linesid ON dbo.inv_receipts.sources_refid = dbo.po_lines.id ON 
                         dbo.list_items.id = dbo.po_lines.itemsid LEFT OUTER JOIN
                         dbo.amics_v_api_serialqty ON dbo.inv_receipts.id = dbo.amics_v_api_serialqty.id LEFT OUTER JOIN
                         dbo.list_invtypes ON dbo.list_invtypes.id = dbo.list_items.invtypeid LEFT OUTER JOIN
                         dbo.list_status RIGHT OUTER JOIN
                         dbo.po_main ON dbo.list_status.id = dbo.po_main.statusid ON dbo.po_lines.pomainid = dbo.po_main.id
GROUP BY ISNULL(dbo.list_itemtypes.itemtype, ''), ISNULL(dbo.po_lines.poitem, dbo.so_lines.itemnumber), ISNULL(dbo.so_lines.itemnumber, dbo.po_lines.poitem), 
                         ISNULL(dbo.po_lines.description, dbo.so_lines.description), ISNULL(dbo.so_lines.description, dbo.po_lines.description), ISNULL(dbo.po_main.pomain, ''), 
                         ISNULL(dbo.po_lines.quantity, 0), ISNULL(dbo.po_lines.unitcost, 0), dbo.po_lines.unitcost * dbo.po_lines.quantity, dbo.po_lines.deliverydate, 
                         ISNULL(dbo.list_status.status, ''), dbo.so_lines.id, dbo.so_lines.somainid, ISNULL(dbo.po_main.requisition, ''), dbo.so_lines.linenum, dbo.po_main.id, 
                         ISNULL(dbo.so_lines.itemnumber, ''), dbo.po_lines.id, ISNULL(dbo.po_main.transferpo, 0),dbo.list_invtypes.invtype
) as x 

WHERE  x.somainid = @somainid  ORDER BY x.linenum

END
GO

---------------------------------------------------------------------------------------------------------------------------------------------

--sp_view_sopocreate_essex
/****** Object:  StoredProcedure [dbo].[amics_sp_api_view_sopocreate]    Script Date: 16-08-2022 03:28:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_view_sopocreate]
@somain varchar(50)
As
BEGIN
select so_lines.id,list_itemtypes.itemtype,linenum,dbo.so_lines.itemnumber,revision,
dbo.so_lines.description,so_lines.itemsid,list_customers_items.customer_item as customerItem, 
quantity,SUM(isnull(inv_pick.pick_quantity,0)) as pickedQty,uom,so_lines.user1,unitcost,ISNULL(dbo.so_lines.markup,1) as markup, 
(quantity*unitcost) as extCost, convert(varchar,estshipdate,1) as estshipdate, 
convert(varchar,shipdate,1) as shipdate,warranty_years,warranty_days,  
ISNULL(dbo.list_items.cost,1) as cost, 
ISNULL(dbo.so_lines.unitcost,1)*ISNULL(dbo.list_items.markup,1) as pricemarkup, 
ISNULL(dbo.list_items.cost,1)*ISNULL(dbo.list_items.markup,1) as pmcostmarkup, 
solinesnotes, dbo.list_items.obsolete, 
(quantity - (select ISNULL(SUM(ISNULL(po_lines.quantity,0)),0) from po_lines where so_linesid=so_lines.id)) as sopoqty
 from so_lines left outer join list_customers_items on list_customers_items.id = so_lines.customers_itemsid   
left outer join inv_pick on inv_pick.sources_refid = so_lines.id LEFT OUTER JOIN list_items ON list_items.id = so_lines.itemsid left outer join list_itemtypes on list_itemtypes.id = list_items.itemtypeid where somainid=(select top 1 id from so_main where  somain=@somain)  
and (quantity - (select ISNULL(SUM(po_lines.quantity),0) from po_lines where so_linesid=so_lines.id)) > 0
and dbo.list_items.userbit2 = 1
group by so_lines.id,list_itemtypes.itemtype,linenum,dbo.so_lines.itemnumber,revision,dbo.so_lines.description,so_lines.itemsid,list_customers_items.customer_item,quantity,uom,so_lines.user1,  
unitcost,dbo.so_lines.markup,dbo.list_items.markup,dbo.list_items.cost,estshipdate,shipdate,warranty_years,warranty_days,solinesnotes,dbo.list_items.obsolete order by linenum  

END
GO

--------------------------------------------------------------------------------------------------------------------------------------------------
--sp_summarized_bom_solines_po5
/****** Object:  StoredProcedure [dbo].[amics_sp_api_summarized_bom_solines_po]    Script Date: 16-08-2022 04:49:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[amics_sp_api_summarized_bom_solines_po]  @somain varchar(50)   
 AS   
  declare @sumbom table (parentid2 uniqueidentifier,childid2 uniqueidentifier, item2 varchar(50),rev varchar(50),qty2 decimal (18,8),  
   descript2 varchar(150),cost2 money,reclevel2 smallint,factor2 decimal (18,8))   
  declare @sumbom_final table (childid uniqueidentifier,itemnumber varchar(50),description varchar(150),rev varchar(50),totalqty decimal(18,8),uom varchar(50),unitcost decimal(18,8))   
  declare @itemsid uniqueidentifier,@qty decimal(18,8),@item varchar(50),@rev2 varchar(50),@description varchar(150),  
   @itemsid_parent uniqueidentifier,@itemsid_child uniqueidentifier,@cost money,@rec_level smallint,@factor decimal(18,8),  
   @item_exist varchar(50),@buy smallint,@allocated decimal(18,8),@onhand decimal(18,8),@avail decimal(18,8),@uom varchar(50),@unitcost decimal(18,8)  
 BEGIN   
   
  declare cuMyCursor Cursor For (select itemsid,quantity,itemnumber,so_lines.description,revision from so_lines   
   inner join so_main on so_lines.somainid=so_main.id where somain=@somain ) -- Sales Order Line items  
  Open cuMyCursor   
  Fetch Next from cuMyCursor into @itemsid,@qty,@item,@description,@rev2 -- Scan thru sales order lines  
  while @@fetch_status = 0   
  begin   
   insert into @sumbom values(null,@itemsid,@item,'',@qty,@description,0,0,0)   
--   insert into @sumbom (item2,descript2,childid2,qty2) values(@item,@description,@itemsid,@qty)   
   insert into @sumbom exec amics_sp_api_summarized_bom @itemsid,@qty  -- Inserts all summarized bom for that sales order line item  
   Fetch Next from cuMyCursor into @itemsid,@qty,@item,@description,@rev2 -- Scan thru sales order lines  
  end   
  close cuMyCursor   
  deallocate cuMyCursor   
 ---------  The following code combines the requirement  
  declare cuMyCursorFinal Cursor For (select childid2,item2,descript2,qty2 from @sumbom)  
  open cuMyCursorFinal   
  fetch next from cuMyCursorFinal into @itemsid,@item,@description,@qty  
  while @@fetch_status=0   
  begin  
   set @buy=(select buyitem from list_items where id=@itemsid)  
   if @buy=1 -- Select only make items   
   begin  
    set @rev2=(select rev from list_items where id=@itemsid)  
    set @allocated=(select sum(quantity) from inv_allocate where itemsid=@itemsid and quantity>0)  
   set @onhand =(select SUM(quantity) from inv_basic inner join list_locations on inv_basic.locationsid=list_locations.id where itemsid=@itemsid and quantity>0 and list_locations.invalid<>1)   
   set @avail  =ISNULL(@onhand,0)-ISNULL(@allocated,0)  
    set @item_exist=(select itemnumber from @sumbom_final where childid=@itemsid)  
      
    set @uom=(select uomref from list_uoms where id =(select top 1 uomid from list_items where id=@itemsid))  
    set @unitcost=(select cost from list_items where id=@itemsid)  
      
     if @item_exist is null  
      if @avail<=0 insert into @sumbom_final values (@itemsid,@item,@description,@rev2,@qty,@uom,@unitcost)  
     --insert into @sumbom_final values (@itemsid,@item,@description,@rev2,@qty)  
    else  
     update @sumbom_final set totalqty=totalqty+@qty where childid=@itemsid  
     update @sumbom_final set uom= @uom   
     update @sumbom_final set unitcost= @unitcost      
       
  end  
   fetch next from cuMyCursorFinal into @itemsid,@item,@description,@qty  
  end  
  close cuMyCursorFinal   
  deallocate cuMyCursorFinal   
    
 select * from @sumbom_final order by itemnumber --where itemnumber like 'MSVA-38%'  
 END   

--------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_summarized_bom]    Script Date: 16-08-2022 08:56:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_api_summarized_bom]
    @StartProductID [uniqueidentifier],@nqty decimal(18,8)
AS
    
BEGIN
	declare @qty2 float ;
	SET NOCOUNT ON;
    WITH [BOM_cte]([itemsid_parent], [itemsid_child], [itemnumber] , [rev],[qty], [description], [cost], [RecursionLevel],[factor] ) -- CTE name and columns
    AS (
		SELECT bom1.[itemsid_parent], bom1.[itemsid_child], p.[itemnumber] , p.[rev],cast(isnull(@nqty*bom1.[quantity],0)as decimal(18,8))   , p.[description], p.[cost],  1,cast(@nqty as decimal(30,8)) as factor -- Get the initial list of components for the bike assembly
        FROM [dbo].[items_bom] bom1
            INNER JOIN [dbo].[list_items] p 
            ON bom1.[itemsid_child] = p.[id] 
        WHERE bom1.[itemsid_parent] = @StartProductID 
        UNION ALL
        SELECT bom.[itemsid_parent], bom.[itemsid_child], p.[itemnumber] , p.[rev],cast(isnull(cte.qty*bom.[quantity],0) as decimal(18,8)), p.[description], p.[cost],  [RecursionLevel] + 1,cast(factor as decimal(30,8)) as factor -- Join recursive member to anchor
        FROM [BOM_cte] cte
            INNER JOIN [dbo].[items_bom] bom 
            ON bom.[itemsid_parent] = cte.[itemsid_child]
            INNER JOIN [dbo].[list_items] p 
            ON bom.[itemsid_child] = p.[id] 
		) 
        -- Outer select from the CTE
    SELECT b.[itemsid_parent], b.[itemsid_child], b.[itemnumber] as 'Item Number',b.[rev],cast(sum(b.[qty])as Decimal(18,8)) as quantity   , b.[Description] as Description, b.[Cost] as cost , b.[RecursionLevel],@nqty*b.[qty] as factor
	FROM [BOM_cte] b
    GROUP BY b.[itemsid_child], b.[itemnumber],b.[rev],b.[itemsid_parent], b.[RecursionLevel], b.[description],b.[qty],b.[cost]
    ORDER BY b.[itemsid_parent], b.[itemsid_child]
    OPTION (MAXRECURSION 25) 

   END;

-----------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_list_documents]    Script Date: 16-08-2022 03:28:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[amics_sp_api_list_documents]
@soMainId varchar(50)
As
BEGIN

 Select id, parentid,linenum,filename from list_documents where parentid=@soMainId

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------------

--sp_report_soprint_essex5
/****** Object:  StoredProcedure [dbo].[amics_sp_rpt_soprint]    Script Date: 17-08-2022 01:22:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_rpt_soprint]
@somain varchar(50)
AS


declare
@compaddress1 varchar(50),@compaddress2 varchar(50),@compaddress3 varchar(50),
@compaddress4 varchar(50),@compaddress5 varchar(50),@compaddress6 varchar(50)

BEGIN

set @compaddress1=(select top 1 address1 from list_companies)
set @compaddress2=(select top 1 address2 from list_companies)
set @compaddress3=(select top 1 address3 from list_companies)
set @compaddress4=(select top 1 address4 from list_companies)
set @compaddress5=(select top 1 address5 from list_companies)
set @compaddress6=(select top 1 address6 from list_companies)

SELECT        
(select dbo.formataddress (@compaddress1,@compaddress2,@compaddress3,@compaddress4,@compaddress5,@compaddress6)) as compaddress,
rtrim(isnull(dbo.so_main.somain,'')) as somain, 
rtrim(isnull(dbo.so_main.user2,'')) as erdesc, 
rtrim(isnull(dbo.so_main.customername,'')) as customername, 
rtrim(isnull(dbo.list_projects.project,'')) as project, 
rtrim(isnull(dbo.list_projects.name,'')) as projectname, 
rtrim(isnull(dbo.list_status.status,'')) as status, 
rtrim(isnull(dbo.so_main.user1 ,'')) AS budauth, 
rtrim(isnull(dbo.list_shipvia.shipvia,'')) as shipvia, 
dbo.so_lines.linenum, 
rtrim(isnull(dbo.so_lines.itemnumber,'')) as itemnumber, 
rtrim(isnull(dbo.so_lines.description,'')) AS partdesc, 
isnull(dbo.so_lines.quantity,0) as quantity, 
isnull(dbo.so_lines.unitcost,0) as unitcost,
isnull(dbo.so_main.sonotes,'') as sonotes,
isnull(dbo.so_lines.solinesnotes,'') as solinesnotes
FROM            dbo.list_shipvia RIGHT OUTER JOIN
                         dbo.so_main ON dbo.list_shipvia.id = dbo.so_main.shipviaid LEFT OUTER JOIN
                         dbo.list_status ON dbo.so_main.statusid = dbo.list_status.id LEFT OUTER JOIN
                         dbo.list_projects ON dbo.so_main.projectid = dbo.list_projects.id LEFT OUTER JOIN
                         dbo.so_lines ON dbo.so_main.id = dbo.so_lines.somainid
WHERE        (dbo.so_main.somain = @somain)
order by linenum

END
GO
---------------------------------------------------------------------------------------------------------------------------------

--sp_report_er_postatus5
/****** Object:  StoredProcedure [dbo].[amics_sp_rpt_er_postatus]    Script Date: 17-08-2022 01:25:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_rpt_er_postatus] @somain varchar(50)
AS
BEGIN
select x.linenum,x.itemtype,x.itemnumber,x.description,x.pomain,x.poqty,sum(x.recd_quantity) as recd_quantity,x.unitcost,x.extcost,x.deliverydate,x.user2 from 
(select so_lines.linenum, isnull(itemtype,'') as itemtype ,isnull(po_lines.poitem,'') as itemnumber,
isnull(po_lines.description,'') as description,isnull(pomain,'') as pomain,
isnull(po_lines.quantity,0) as poqty,
ISNULL(CONVERT(VARCHAR(10), dbo.po_lines.deliverydate, 101), '') as deliverydate, 
sum(isnull(inv_receipts.recd_quantity,0))+(select count(id) from inv_serial where receiptsid=inv_receipts.id) as recd_quantity,
po_lines.unitcost,po_lines.quantity*po_lines.unitcost as extcost,so_main.user2
from so_main 
left join so_lines on so_main.id=so_lines.somainid
left join po_lines on so_lines.id=po_lines.so_linesid
left join po_main on po_lines.pomainid=po_main.id
left join list_items on po_lines.itemsid=list_items.id 
left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id 
left join inv_receipts on po_lines.id=inv_receipts.sources_refid
where somain=@somain and so_lines.quantity>0 and po_main.pomain!=''
group by so_lines.linenum,itemtype,po_lines.poitem,po_lines.description,pomain,po_lines.quantity,deliverydate,recd_quantity,inv_receipts.id,po_lines.linenum,po_lines.unitcost,so_main.user2
) as x
group by  x.linenum,x.itemtype,x.itemnumber,x.description,x.pomain,x.poqty,x.unitcost,x.extcost,x.deliverydate,x.user2 order by itemtype,linenum

END
GO
-- exec [dbo].[sp_report_er_postatus5] 'TF-11-0268'

---------------------------------------------------------------------------------------------------------------------------------------

--sp_report_erinv5
/****** Object:  StoredProcedure [dbo].[amics_sp_rpt_erinv]    Script Date: 17-08-2022 01:28:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amics_sp_rpt_erinv]
	@somain varchar(50)
AS
BEGIN
SELECT        TOP (100) PERCENT dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
                         CASE list_invtypes.invtype WHEN 'Serial' THEN list_locations.location ELSE list_locations_1.location END AS location, 
						     SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) AS Available,
						 dbo.so_main.somain, dbo.so_lines.id AS solinesid, ISNULL(dbo.inv_serial.serno, '') 
                         AS serno, ISNULL(dbo.inv_serial.tagno, '') AS tagno, STR(SUM(ISNULL(dbo.inv_basic.quantity, 0)), 4, 0) AS locqty,
    SUM(SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0))) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
FROM            dbo.list_itemtypes RIGHT OUTER JOIN
                         dbo.list_invtypes RIGHT OUTER JOIN
                         dbo.list_items ON dbo.list_invtypes.id = dbo.list_items.invtypeid ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN
                         dbo.inv_basic RIGHT OUTER JOIN
                         dbo.inv_serial RIGHT OUTER JOIN
                         dbo.so_lines ON ISNULL(dbo.inv_serial.transferid, dbo.inv_serial.so_linesid) = dbo.so_lines.id LEFT OUTER JOIN
                         dbo.list_locations ON dbo.inv_serial.locationsid = dbo.list_locations.id ON dbo.inv_basic.so_linesid = dbo.so_lines.id LEFT OUTER JOIN
                         dbo.list_locations AS list_locations_1 ON dbo.inv_basic.locationsid = list_locations_1.id ON dbo.list_items.id = dbo.so_lines.itemsid RIGHT OUTER JOIN
                         dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id
GROUP BY dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
                         CASE list_invtypes.invtype WHEN 'Serial' THEN list_locations.location ELSE list_locations_1.location END, dbo.so_main.somain, dbo.so_lines.id, ISNULL(dbo.inv_serial.serno, ''), ISNULL(dbo.inv_serial.tagno, 
                         ''), dbo.list_invtypes.invtype
HAVING      (dbo.so_main.somain = @somain) AND (dbo.so_lines.quantity > 0) AND 
SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) >0 
order by linenum,TAGNO,SERNO

END
-- COMMENTED BELOW ON 04/28/16
--BEGIN
--SELECT     TOP (100) PERCENT 
--	dbo.so_lines.linenum,
--	dbo.list_itemtypes.itemtype,
--	dbo.so_lines.itemnumber,
--	dbo.so_lines.description,
--	dbo.so_lines.quantity,
--    ISNULL(ISNULL(dbo.list_locations.location, list_locations_1.location), '') AS location,
--    SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) AS Available,
--    dbo.so_main.somain,
--    dbo.so_lines.id AS solinesid,
--    ISNULL(dbo.inv_serial.serno, '') AS serno, 
--    ISNULL(dbo.inv_serial.tagno, '') AS tagno,
--	str(SUM(ISNULL(dbo.inv_basic.quantity, 0)),4,0) as locqty,
--    SUM(SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0))) OVER(PARTITION BY dbo.so_lines.ID) AS 'Total'
--FROM   
--	dbo.list_locations AS list_locations_1 RIGHT OUTER JOIN
--    dbo.list_itemtypes RIGHT OUTER JOIN
--    dbo.list_items ON dbo.list_itemtypes.id = dbo.list_items.itemtypeid RIGHT OUTER JOIN
--    dbo.inv_basic RIGHT OUTER JOIN
--    dbo.so_lines ON dbo.inv_basic.so_linesid = dbo.so_lines.id LEFT OUTER JOIN
--    dbo.list_locations ON dbo.inv_basic.locationsid = dbo.list_locations.id LEFT OUTER JOIN
--    dbo.inv_serial ON 
--	CASE WHEN inv_serial.transferid IS NOT NULL THEN inv_serial.transferid 
--	ELSE inv_serial.so_linesid END = dbo.so_lines.id 
----	dbo.so_lines.id = dbo.inv_serial.so_linesid 
--	ON dbo.list_items.id = dbo.so_lines.itemsid RIGHT OUTER JOIN
--    dbo.so_main ON dbo.so_lines.somainid = dbo.so_main.id ON list_locations_1.id = dbo.inv_serial.locationsid
--GROUP BY dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
--         ISNULL(ISNULL(dbo.list_locations.location, list_locations_1.location), ''), dbo.so_main.somain, dbo.inv_serial.pickid, dbo.so_lines.id, 
--         ISNULL(dbo.inv_serial.serno, ''), ISNULL(dbo.inv_serial.tagno, '')
--		--,dbo.inv_basic.quantity
--					 --,dbo.inv_serial.serno
--HAVING      (dbo.so_main.somain = @somain) AND (dbo.so_lines.quantity > 0) AND 
--SUM(ISNULL(dbo.inv_basic.quantity, 0) + ISNULL(dbo.inv_serial.quantity, 0)) >0 
--order by linenum,itemtype,location,serno
--end 
-- COMMENTED ABOVE ON 04/28/16
--union
--SELECT     TOP (100) PERCENT 
--	dbo.so_lines.linenum,
--	dbo.list_itemtypes.itemtype,
--	dbo.so_lines.itemnumber,
--	dbo.so_lines.description,
--	dbo.so_lines.quantity,
--    ISNULL(dbo.list_locations.location,'') AS location,
--    ISNULL(dbo.inv_serial.quantity, 0) AS Available,
--    dbo.so_main.somain,
--    dbo.so_lines.id AS solinesid,
--    ISNULL(dbo.inv_serial.serno, '') AS serno, 
--    ISNULL(dbo.inv_serial.tagno, '') AS tagno,
--    sum(dbo.inv_serial.quantity) AS 'Total'

--from so_lines left join list_items on so_lines.itemsid=list_items.id left join list_itemtypes on list_items.itemtypeid=list_itemtypes.id 
--left join inv_serial on so_lines.id=inv_serial.transferid left join list_locations on inv_serial.locationsid=list_locations.id 
--left join so_main on so_lines.somainid=so_main.id 
--GROUP BY dbo.so_lines.linenum, dbo.list_itemtypes.itemtype, dbo.so_lines.itemnumber, dbo.so_lines.description, dbo.so_lines.quantity, 
--                      ISNULL(dbo.list_locations.location, ''), dbo.so_main.somain,  dbo.so_lines.id, 
--                      ISNULL(dbo.inv_serial.serno, ''), ISNULL(dbo.inv_serial.tagno, ''),dbo.inv_serial.quantity,dbo.inv_serial.serno
--HAVING      (dbo.so_main.somain = @somain) AND (dbo.so_lines.quantity > 0) AND 
--ISNULL(dbo.inv_serial.quantity, 0) >0
----(dbo.inv_serial.pickid IS NULL or dbo.inv_serial.quantity > 0)
--ORDER BY dbo.so_lines.linenum -- dbo.list_itemtypes.itemtype
--END


-- exec sp_report_erinv5 'ertm02262-1'

-------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[amics_sp_api_loadcontract]    Script Date: 25-08-2022 05:01:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[amics_sp_api_loadcontract]   
@contract varchar(50)='',
@project varchar(50)=''
AS   
BEGIN   

if (@contract != '' or @contract is not null) and (@project != '' or @project is not null)
	begin    
	 select distinct list_contracts.id,list_contracts.contractnum,list_contracts.description,list_contracts.markup1,list_contracts.markup2,
	 list_contracts.createdby,list_contracts.createddate from list_contracts left outer join list_projects on list_contracts.id = list_projects.contractid 
	 where contractnum like '%'+ @contract +'%' and project like '%'+ @project +'%' order by contractnum asc
	 end
else if (@contract != '' or @contract is not null) and (@project = '' or @project is null)
	begin	
	 select distinct list_contracts.id,list_contracts.contractnum,list_contracts.description,list_contracts.markup1,list_contracts.markup2,
	 list_contracts.createdby,list_contracts.createddate from list_contracts left outer join list_projects on list_contracts.id = list_projects.contractid 
	 where contractnum like '%'+ @Contract +'%' order by contractnum asc
	 end
else if (@contract = '' or @contract is null) and (@project != '' or @project is not null)
	begin	
	select distinct list_contracts.id,list_contracts.contractnum,list_contracts.description,list_contracts.markup1,list_contracts.markup2,
	list_contracts.createdby,list_contracts.createddate from list_contracts left outer join list_projects on list_contracts.id = list_projects.contractid
	where project like '%'+ @Project +'%'order by contractnum asc
	end
else
	begin
	select distinct list_contracts.id,list_contracts.contractnum,list_contracts.description,list_contracts.markup1,list_contracts.markup2,
	list_contracts.createdby,list_contracts.createddate from list_contracts left outer join list_projects on list_contracts.id = list_projects.contractid 
	order by contractnum asc
	end
END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_load_projects]    Script Date: 24-08-2022 01:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_load_projects]   
@contractid varchar(50)
AS   
BEGIN  

select list_projects.id,project,name,manager,contact1,contact2,notes,address1,address2,address3,address4,address5,address6,address7,shipnotes,
contractid,isnull(list_warehouses.warehouse,'') as warehouse from list_projects 
left outer join list_warehouses on list_warehouses.id = list_projects.warehousesid 
where contractid=@contractid  order by list_projects.project asc

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_contracts]    Script Date: 24-08-2022 01:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_maintain_contracts]   
@id uniqueidentifier=null,
@contractnum varchar(50),
@description varchar(50),
@markup1 decimal(18,8),
@markup2 decimal(18,8),
@createdby varchar(50),
@actionFlag smallint
AS   
BEGIN   

if (@actionFlag = 1)                 
    insert into list_contracts (id,contractnum,description,markup1,markup2,createdby,createddate) values (NEWID(),@contractnum,@description,
	@markup1,@markup2,@createdby,GETDATE())
else if (@actionFlag = 2)   
    update list_contracts set contractnum=@contractnum,description=@description,markup1=@markup1,markup2=@markup2 where id=@id
else if (@actionFlag = 3)
    delete from list_contracts where id=@id

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[amics_sp_api_maintain_projects]    Script Date: 24-08-2022 01:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amics_sp_api_maintain_projects]   
@id uniqueidentifier=null,
@contractid uniqueidentifier=null,
@project varchar(50),
@name varchar(50),
@createdby varchar(50),
@actionFlag smallint
AS   
BEGIN   

if (@actionFlag = 1)                 
    insert into list_projects (id,project,name,createdby,createddate,contractid) values (NEWID(),@project,@name,@createdby,GETDATE(),@contractid)
else if (@actionFlag = 2)   
    update list_projects set project=@project,name=@name,contractid=@contractid where id=@id
else if (@actionFlag = 3)
    delete from list_projects where id=@id

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------