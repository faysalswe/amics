
/****** Object:  Table [dbo].[inv_allocate]    Script Date: 13-07-2022 11:36:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_allocate](
	[id] [uniqueidentifier] ROWGUIDCOL  NULL,
	[itemsid] [uniqueidentifier] NULL,
	[invbasicid] [uniqueidentifier] NULL,
	[quantity] [decimal](18, 8) NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[warehouseid] [uniqueidentifier] NULL,
	[sources_refid] [uniqueidentifier] NULL,
	[solinesid] [uniqueidentifier] NULL,
	[wobomid] [uniqueidentifier] NULL,
	[polinesid] [uniqueidentifier] NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[user3] [varchar](50) NULL,
	[notes] [varchar](max) NULL,
	[linenum] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_allocate] ADD  CONSTRAINT [DF_inv_allocate_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_allocate] ADD  CONSTRAINT [DF_inv_allocate_createddate]  DEFAULT (getdate()) FOR [createddate]
GO


------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[inv_basic]    Script Date: 13-07-2022 11:37:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_basic](
	[id] [uniqueidentifier] NOT NULL,
	[locationsid] [uniqueidentifier] NULL,
	[itemsid] [uniqueidentifier] NULL,
	[so_linesid] [uniqueidentifier] NULL,
	[quantity] [decimal](18, 6) NULL,
	[datecounted] [datetime] NULL,
	[countnum] [decimal](18, 0) NULL,
	[lotno] [varchar](50) NULL,
	[notes] [text] NULL,
	[warndate] [smalldatetime] NULL,
	[expdate] [smalldatetime] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[color] [varchar](50) NULL,
	[sources_refid] [uniqueidentifier] NULL,
	[lic_plate] [varchar](50) NULL,
	[lic_plate_number] [int] IDENTITY(1,1) NOT NULL,
	[cost] [decimal](18, 8) NULL,
	[extendedid] [uniqueidentifier] NULL,
	[er] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_basic] ADD  CONSTRAINT [DF_inv_basic_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_basic] ADD  CONSTRAINT [DF_inv_basic_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_misc]    Script Date: 13-07-2022 11:38:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_misc](
	[id] [uniqueidentifier] NOT NULL,
	[itemsid] [uniqueidentifier] NULL,
	[reasonid] [uniqueidentifier] NULL,
	[projectid] [uniqueidentifier] NULL,
	[ref] [varchar](50) NULL,
	[source] [varchar](50) NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[account] [varchar](50) NULL,
	[serlotid] [uniqueidentifier] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_misc] ADD  CONSTRAINT [DF_inv_misc_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[inv_pick]    Script Date: 13-07-2022 11:39:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_pick](
	[id] [uniqueidentifier] NOT NULL,
	[transnum] [int] IDENTITY(1,1) NOT NULL,
	[inv_basicid] [uniqueidentifier] NULL,
	[inv_serialid] [uniqueidentifier] NULL,
	[inv_soshipid] [uniqueidentifier] NULL,
	[sourcesid] [uniqueidentifier] NULL,
	[sources_refid] [uniqueidentifier] NULL,
	[usersid] [uniqueidentifier] NULL,
	[compline] [int] NULL,
	[prior_quantity] [decimal](18, 0) NULL,
	[pick_quantity] [decimal](18, 8) NULL,
	[trans_date] [datetime] NULL,
	[picknotes] [text] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[boxnum] [decimal](18, 0) NULL,
	[prior_shipped] [decimal](18, 0) NULL,
	[warranty_expires] [smalldatetime] NULL,
	[scrap] [bit] NULL,
	[void_quantity] [decimal](18, 8) NULL,
	[notes] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_pick] ADD  CONSTRAINT [DF_inv_pick_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_pick] ADD  CONSTRAINT [DF_inv_pick_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[inv_pick] ADD  DEFAULT ((0)) FOR [prior_shipped]
GO


-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_pick_ship]    Script Date: 13-07-2022 11:40:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_pick_ship](
	[id] [uniqueidentifier] NULL,
	[solinesid] [uniqueidentifier] NULL,
	[invbasicid] [uniqueidentifier] NULL,
	[invserialid] [uniqueidentifier] NULL,
	[quantity] [decimal](18, 0) NULL,
	[createddate] [datetime] NULL,
	[createdby] [varchar](50) NULL
) ON [PRIMARY]
GO
-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[inv_receipts]    Script Date: 13-07-2022 11:41:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_receipts](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[transnum] [int] IDENTITY(1,1) NOT NULL,
	[sourcesid] [uniqueidentifier] NULL,
	[sources_refid] [uniqueidentifier] NULL,
	[inv_basicid] [uniqueidentifier] NULL,
	[users_id] [uniqueidentifier] NULL,
	[recd_quantity] [decimal](18, 8) NULL,
	[prior_quantity] [decimal](18, 8) NULL,
	[trans_date] [datetime] NULL,
	[recd_notes] [varchar](max) NULL,
	[void] [bit] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[recd_locationid] [uniqueidentifier] NULL,
	[warranty_expires] [smalldatetime] NULL,
	[qbrectxnid] [varchar](50) NULL,
	[print_report] [smallint] NULL,
	[original_receiptsid] [uniqueidentifier] NULL,
	[print_label] [bit] NULL,
	[receiver] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_receipts] ADD  CONSTRAINT [DF_inv_receipts_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_receipts] ADD  CONSTRAINT [DF_inv_receipts_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[inv_receipts] ADD  DEFAULT (newid()) FOR [recd_locationid]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_serial]    Script Date: 13-07-2022 11:46:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_serial](
	[id] [uniqueidentifier] NOT NULL,
	[itemsid] [uniqueidentifier] NULL,
	[so_linesid] [uniqueidentifier] NULL,
	[so_mainid] [uniqueidentifier] NULL,
	[so_mainid_allocated] [uniqueidentifier] NULL,
	[locationsid] [uniqueidentifier] NULL,
	[receiptsid] [uniqueidentifier] NULL,
	[pickid] [uniqueidentifier] NULL,
	[transferid] [uniqueidentifier] NULL,
	[users_id] [uniqueidentifier] NULL,
	[quantity] [decimal](18, 0) NULL,
	[serno] [varchar](50) NULL,
	[tagno] [varchar](50) NULL,
	[model] [varchar](50) NULL,
	[mfr] [varchar](50) NULL,
	[notes] [text] NULL,
	[expiry] [datetime] NULL,
	[user15] [varchar](50) NULL,
	[user16] [varchar](50) NULL,
	[user17] [varchar](50) NULL,
	[user18] [varchar](50) NULL,
	[user19] [varchar](50) NULL,
	[user20] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[expdate] [smalldatetime] NULL,
	[lic_plate] [varchar](10) NULL,
	[lic_plate_number] [int] IDENTITY(1,1) NOT NULL,
	[cost] [decimal](18, 8) NULL,
 CONSTRAINT [PK_inv_serial] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_serial] ADD  CONSTRAINT [DF_inv_serial_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_serial] ADD  CONSTRAINT [DF_inv_serial_quantity]  DEFAULT ((1)) FOR [quantity]
GO

ALTER TABLE [dbo].[inv_serial] ADD  CONSTRAINT [DF_inv_serial_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[inv_serial] ADD  DEFAULT ('S-') FOR [lic_plate]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_serlot]    Script Date: 13-07-2022 11:47:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_serlot](
	[id] [uniqueidentifier] NULL,
	[transnum] [int] NULL,
	[serno] [varchar](50) NULL,
	[tagno] [varchar](50) NULL,
	[lotno] [varchar](50) NULL,
	[color] [varchar](50) NULL,
	[qty] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[expdate] [smalldatetime] NULL,
	[model] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT ((0)) FOR [transnum]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [serno]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [tagno]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [lotno]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [color]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [qty]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[inv_serlot] ADD  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_soship]    Script Date: 13-07-2022 11:48:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_soship](
	[id] [uniqueidentifier] NULL,
	[packlist] [varchar](50) NULL,
	[shiptoname] [varchar](50) NULL,
	[shiptoaddress1] [varchar](50) NULL,
	[shiptoaddress2] [varchar](50) NULL,
	[shiptoaddress3] [varchar](50) NULL,
	[shiptoaddress4] [varchar](50) NULL,
	[shiptoaddress5] [varchar](50) NULL,
	[shiptoaddress6] [varchar](50) NULL,
	[shipvia] [varchar](50) NULL,
	[shipdate] [datetime] NULL,
	[packnote] [varchar](max) NULL,
	[shipcharge] [money] NULL,
	[salestax] [money] NULL,
	[trackingnum] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[createdby] [varchar](50) NULL,
	[invoiced] [bit] NULL,
	[invoicenote] [varchar](max) NULL,
	[void] [bit] NULL,
	[qbinvtxnid] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_soship] ADD  CONSTRAINT [DF_inv_soship_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[inv_soship] ADD  CONSTRAINT [DF_inv_soship_invoiced]  DEFAULT ('0') FOR [invoiced]
GO
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_sources]    Script Date: 13-07-2022 11:49:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_sources](
	[id] [uniqueidentifier] NOT NULL,
	[sourceref] [varchar](50) NULL,
	[Increase] [bit] NULL,
 CONSTRAINT [PK_sources] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_sources] ADD  CONSTRAINT [DF_sources_id]  DEFAULT (newid()) FOR [id]
GO
-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_trans]    Script Date: 13-07-2022 11:50:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_trans](
	[id] [uniqueidentifier] ROWGUIDCOL  NULL,
	[source] [varchar](50) NULL,
	[invbasicid] [uniqueidentifier] NULL,
	[invserialid] [uniqueidentifier] NULL,
	[itemsid] [uniqueidentifier] NULL,
	[refid] [uniqueidentifier] NULL,
	[fromlocid] [uniqueidentifier] NULL,
	[tolocid] [uniqueidentifier] NULL,
	[transqty] [decimal](18, 8) NULL,
	[boxnum] [smallint] NULL,
	[transnum] [int] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [smalldatetime] NULL,
	[setnum] [int] NULL,
	[scrap] [bit] NULL,
	[notes] [varchar](max) NULL,
	[weight] [decimal](8, 4) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_trans] ADD  CONSTRAINT [DF_inv_trans_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_trans] ADD  CONSTRAINT [DF_inv_trans_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_transfer]    Script Date: 13-07-2022 11:51:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_transfer](
	[id] [uniqueidentifier] NULL,
	[transfernum] [int] NULL,
	[locationsid_from] [uniqueidentifier] NULL,
	[locationsid_to] [uniqueidentifier] NULL,
	[inv_basicid_from] [uniqueidentifier] NULL,
	[inv_basicid_to] [uniqueidentifier] NULL,
	[inv_serialid] [uniqueidentifier] NULL,
	[so_mainid] [uniqueidentifier] NULL,
	[priorqty] [decimal](18, 0) NULL,
	[priorqty2] [decimal](18, 0) NULL,
	[thisqty] [decimal](18, 0) NULL,
	[trans_date] [datetime] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[notes] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[inv_transfer] ADD  CONSTRAINT [DF_v_invtransfer_serial_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[inv_transfer] ADD  CONSTRAINT [DF_v_invtransfer_serial_created]  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[inv_transfer_location]    Script Date: 13-07-2022 01:37:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_transfer_location](
	[id] [uniqueidentifier] ROWGUIDCOL  NULL,
	[solinesid] [uniqueidentifier] NULL,
	[invbasicid] [uniqueidentifier] NULL,
	[invserialid] [uniqueidentifier] NULL,
	[quantity] [decimal](18, 0) NULL,
	[createddate] [datetime] NULL,
	[createdby] [varchar](50) NULL,
	[addflag] [bit] NULL,
	[cost] [decimal](18, 8) NULL,
	[transnum] [int] NULL
) ON [PRIMARY]
GO


-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[items_bom]    Script Date: 13-07-2022 01:38:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[items_bom](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[linenum] [int] NULL,
	[itemsid_parent] [uniqueidentifier] NOT NULL,
	[itemsid_child] [uniqueidentifier] NOT NULL,
	[quantity] [float] NULL,
	[ref] [varchar](250) NULL,
	[comments] [varchar](150) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[findno] [varchar](50) NULL,
	[bom_costed] [bit] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF_items_bom_id_1__51]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF_items_bom_linenum_1__51]  DEFAULT ((0)) FOR [linenum]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF_items_bom_ref_1__51]  DEFAULT (space((1))) FOR [ref]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF_items_bom_comments_2__51]  DEFAULT (space((1))) FOR [comments]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF__valesoft___creat__7DEF3CE1]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[items_bom] ADD  CONSTRAINT [DF__valesoft___creat__7EE3611A]  DEFAULT (getdate()) FOR [createddate]
GO

---------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_company_options]    Script Date: 13-07-2022 01:39:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_company_options](
	[id] [uniqueidentifier] NULL,
	[optionid] [decimal](18, 0) NULL,
	[description] [varchar](250) NULL,
	[yesorno] [bit] NULL,
	[optionvalue] [decimal](18, 0) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[area] [varchar](50) NULL
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_documents]    Script Date: 13-07-2022 01:40:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_documents](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[parentid] [uniqueidentifier] NULL,
	[linenum] [smallint] NULL,
	[filename] [varchar](150) NULL,
	[rev] [varchar](50) NULL,
	[sourceid] [uniqueidentifier] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[always] [bit] NULL,
	[prn_so] [bit] NULL,
	[prn_po] [bit] NULL,
	[prn_wo] [bit] NULL,
	[comments] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_documents] ADD  CONSTRAINT [DF_documents_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_documents] ADD  CONSTRAINT [DF_Documents_createddate]  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------



/****** Object:  Table [dbo].[list_invtypes]    Script Date: 13-07-2022 01:41:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_invtypes](
	[id] [uniqueidentifier] NULL,
	[invtype] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[isactive] [bit] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_invtypes] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_invtypes] ADD  DEFAULT (space((1))) FOR [invtype]
GO

ALTER TABLE [dbo].[list_invtypes] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_invtypes] ADD  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[list_itemclass]    Script Date: 13-07-2022 01:42:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_itemclass](
	[id] [uniqueidentifier] NULL,
	[itemclass] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_itemclass] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_itemclass] ADD  DEFAULT (space((1))) FOR [itemclass]
GO

ALTER TABLE [dbo].[list_itemclass] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_itemclass] ADD  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_itemcodes]    Script Date: 13-07-2022 01:43:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_itemcodes](
	[id] [uniqueidentifier] NULL,
	[itemcode] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_itemcodes] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_itemcodes] ADD  DEFAULT (space((1))) FOR [itemcode]
GO

ALTER TABLE [dbo].[list_itemcodes] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_itemcodes] ADD  DEFAULT (getdate()) FOR [createddate]
GO
--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_items]    Script Date: 13-07-2022 01:43:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_items](
	[id] [uniqueidentifier] NULL,
	[itemnumber] [varchar](50) NOT NULL,
	[rev] [varchar](50) NULL,
	[description] [nvarchar](150) NULL,
	[SalesDescription] [varchar](150) NULL,
	[PurchaseDescription] [varchar](150) NULL,
	[invtypeid] [uniqueidentifier] NULL,
	[itemtypeid] [uniqueidentifier] NULL,
	[itemclassid] [uniqueidentifier] NULL,
	[itemcodeid] [uniqueidentifier] NULL,
	[uomid] [uniqueidentifier] NULL,
	[uompid] [uniqueidentifier] NULL,
	[uomsid] [uniqueidentifier] NULL,
	[conversion] [decimal](18, 8) NULL,
	[cost] [money] NULL,
	[markup] [decimal](18, 4) NULL,
	[price2] [money] NULL,
	[price3] [money] NULL,
	[weight] [decimal](18, 8) NULL,
	[reorder] [bit] NULL,
	[buyitem] [bit] NULL,
	[obsolete] [bit] NULL,
	[cyclecount] [bit] NULL,
	[notes] [text] NULL,
	[minimum] [decimal](18, 8) NULL,
	[maximum] [decimal](18, 8) NULL,
	[leadtime] [decimal](18, 8) NULL,
	[decimalqty] [decimal](18, 8) NULL,
	[locationsid] [uniqueidentifier] NULL,
	[consignment] [bit] NULL,
	[freight] [money] NULL,
	[taxable] [bit] NULL,
	[imgpath] [varchar](50) NULL,
	[glsales] [varchar](50) NULL,
	[glinv] [varchar](50) NULL,
	[glcogs] [varchar](50) NULL,
	[dwgno] [varchar](50) NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[user3] [decimal](18, 8) NULL,
	[userbit] [bit] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[costed] [bit] NULL,
	[parent_updated] [bit] NULL,
	[flag_delete] [bit] NULL,
	[counter_serno] [smallint] NULL,
	[cost_temp] [money] NULL,
	[userbit2] [bit] NULL,
	[userbit3] [bit] NULL,
	[bom_released] [bit] NULL,
	[inactive] [bit] NULL,
	[test] [nvarchar](50) NULL,
	[user4] [varchar](50) NULL,
	[user5] [varchar](50) NULL,
	[user6] [varchar](50) NULL,
	[user7] [varchar](50) NULL,
	[user8] [varchar](50) NULL,
	[user9] [varchar](50) NULL,
	[user10] [varchar](50) NULL,
	[user11] [varchar](50) NULL,
	[user12] [varchar](50) NULL,
	[user13] [varchar](50) NULL,
	[user14] [varchar](50) NULL,
	[user15] [varchar](50) NULL,
	[cyclecounted] [smalldatetime] NULL,
	[price]  AS ([cost]*[markup]),
	[price4] [decimal](18, 8) NULL,
	[price5] [decimal](18, 8) NULL,
	[price6] [decimal](18, 8) NULL,
	[price7] [decimal](18, 8) NULL,
	[price8] [decimal](18, 8) NULL,
	[price9] [decimal](18, 8) NULL,
	[price10] [decimal](18, 8) NULL,
	[cyclecnt] [smallint] NULL,
	[reclocationsid] [uniqueidentifier] NULL,
	[modifieddate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_id_1__99]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF__valesoft___itemn__6EACF951]  DEFAULT (space((1))) FOR [itemnumber]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_rev_11__99]  DEFAULT ('-') FOR [rev]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_description_2__99]  DEFAULT (space((1))) FOR [description]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_conversion]  DEFAULT ((1)) FOR [conversion]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_cost_3__99]  DEFAULT ((0)) FOR [cost]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_price2]  DEFAULT ((0)) FOR [price2]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_price3]  DEFAULT ((0)) FOR [price3]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_weight_4__99]  DEFAULT ((0)) FOR [weight]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_reorder]  DEFAULT ((1)) FOR [reorder]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_obsolete]  DEFAULT ((0)) FOR [obsolete]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_minimum_5__99]  DEFAULT ((0)) FOR [minimum]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_maximum_6__99]  DEFAULT ((0)) FOR [maximum]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_leadtime_7__99]  DEFAULT ((0)) FOR [leadtime]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_decimalqty_8__99]  DEFAULT ((0)) FOR [decimalqty]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_freight_9__99]  DEFAULT ((0)) FOR [freight]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_imgpath_10__99]  DEFAULT (space((1))) FOR [imgpath]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_dwgno]  DEFAULT (space((1))) FOR [dwgno]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_User1]  DEFAULT (space((1))) FOR [user1]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_User2]  DEFAULT (space((1))) FOR [user2]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_User3]  DEFAULT ((0)) FOR [user3]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_createdby_12__99]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_createddate_13__99]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_flag_delete]  DEFAULT ((0)) FOR [flag_delete]
GO

ALTER TABLE [dbo].[list_items] ADD  DEFAULT ((1001)) FOR [counter_serno]
GO

ALTER TABLE [dbo].[list_items] ADD  DEFAULT ((0)) FOR [bom_released]
GO

ALTER TABLE [dbo].[list_items] ADD  CONSTRAINT [DF_list_items_cyclecounted]  DEFAULT ('1900-01-01') FOR [cyclecounted]
GO

ALTER TABLE [dbo].[list_items] ADD  DEFAULT (getdate()) FOR [modifieddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_itemtypes]    Script Date: 13-07-2022 01:44:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_itemtypes](
	[id] [uniqueidentifier] NULL,
	[itemtype] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_itemtypes] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_itemtypes] ADD  DEFAULT (space((1))) FOR [itemtype]
GO

ALTER TABLE [dbo].[list_itemtypes] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_itemtypes] ADD  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_locations]    Script Date: 13-07-2022 01:53:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_locations](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[warehousesid] [uniqueidentifier] NULL,
	[groupid] [uniqueidentifier] NULL,
	[location] [varchar](50) NULL,
	[invalid] [bit] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[flag_delete] [bit] NULL,
	[sequenceno] [smallint] NULL,
	[route] [smallint] NULL,
	[user1] [smallint] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_locations] ADD  CONSTRAINT [DF__list_locatio__id__2A6CDD9D]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_locations] ADD  CONSTRAINT [DF__list_loca__locat__2B6101D6]  DEFAULT (space((1))) FOR [location]
GO

ALTER TABLE [dbo].[list_locations] ADD  CONSTRAINT [DF__list_loca__creat__2C55260F]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_locations] ADD  CONSTRAINT [DF__list_loca__creat__2D494A48]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[list_locations] ADD  CONSTRAINT [DF_list_locations_flag_delete]  DEFAULT ((0)) FOR [flag_delete]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_next_number]    Script Date: 13-07-2022 01:54:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_next_number](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[packlist] [int] NOT NULL,
	[so] [int] NULL,
	[po] [int] NULL,
	[poreq] [int] NULL,
	[wo] [int] NULL,
	[rma] [int] NULL,
	[transfer] [int] NULL,
	[womainid] [uniqueidentifier] NULL,
	[pomainid] [uniqueidentifier] NULL,
	[somainid] [uniqueidentifier] NULL,
	[rmamainid] [uniqueidentifier] NULL,
	[option_po] [int] NULL,
	[option_so] [int] NULL,
	[option_wo] [int] NULL,
	[option_rma] [int] NULL,
	[last_wo] [int] NULL,
	[last_po] [int] NULL,
	[last_so] [int] NULL,
	[web_ticket] [int] NULL,
	[sp_rec] [int] NULL,
	[setnum] [int] NULL,
	[tagno] [int] NULL,
	[serno] [int] NULL,
	[lotno] [int] NULL,
	[lic_plate] [int] NULL,
	[mrbnum] [int] NULL,
	[receiver] [smallint] NULL,
	[cyclecnt] [smallint] NULL,
	[translognum] [int] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_next_number] ADD  CONSTRAINT [DF_so_packlist_number_id]  DEFAULT (newid()) FOR [id]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_notes_general]    Script Date: 13-07-2022 01:54:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_notes_general](
	[id] [uniqueidentifier] NULL,
	[parentid] [uniqueidentifier] NULL,
	[linenum] [smallint] NULL,
	[notesref] [varchar](250) NULL,
	[notes] [varchar](max) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[entryid] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT ((0)) FOR [linenum]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT (space((1))) FOR [notesref]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT (space((1))) FOR [notes]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_notes_general] ADD  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_projects]    Script Date: 13-07-2022 01:55:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_projects](
	[id] [uniqueidentifier] NULL,
	[project] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[manager] [varchar](50) NULL,
	[contact1] [varchar](50) NULL,
	[contact2] [varchar](50) NULL,
	[notes] [text] NULL,
	[address1] [varchar](50) NULL,
	[address2] [varchar](50) NULL,
	[address3] [varchar](50) NULL,
	[address4] [varchar](50) NULL,
	[address5] [varchar](50) NULL,
	[address6] [varchar](50) NULL,
	[address7] [varchar](50) NULL,
	[shipnotes] [text] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[contractid] [uniqueidentifier] NULL,
	[warehousesid] [uniqueidentifier] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_projects] ADD  CONSTRAINT [DF__jobs__id__2B36E4A4]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_projects] ADD  CONSTRAINT [DF__jobs__jobid__2C2B08DD]  DEFAULT (space((1))) FOR [project]
GO

ALTER TABLE [dbo].[list_projects] ADD  CONSTRAINT [DF__jobs__descriptio__2D1F2D16]  DEFAULT (space((1))) FOR [name]
GO

ALTER TABLE [dbo].[list_projects] ADD  CONSTRAINT [DF__jobs__createdby__2F077588]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_projects] ADD  CONSTRAINT [DF__jobs__createddat__2FFB99C1]  DEFAULT (getdate()) FOR [createddate]
GO

-----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_reasons]    Script Date: 13-07-2022 01:56:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_reasons](
	[id] [uniqueidentifier] NULL,
	[reason] [varchar](50) NULL,
	[description] [varchar](50) NULL,
	[increase] [bit] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_reasons] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_reasons] ADD  DEFAULT (space((1))) FOR [reason]
GO

ALTER TABLE [dbo].[list_reasons] ADD  DEFAULT (space((1))) FOR [description]
GO

ALTER TABLE [dbo].[list_reasons] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_reasons] ADD  DEFAULT (getdate()) FOR [createddate]
GO

-----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_status]    Script Date: 13-07-2022 01:57:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_status](
	[id] [uniqueidentifier] NOT NULL,
	[status] [varchar](50) NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[flag_delete] [bit] NULL,
 CONSTRAINT [PK_list_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_status] ADD  CONSTRAINT [DF__list_status__id__253E142C]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_status] ADD  CONSTRAINT [DF__list_stat__statu__26323865]  DEFAULT (space((1))) FOR [status]
GO

ALTER TABLE [dbo].[list_status] ADD  CONSTRAINT [DF_list_statu_createdby_1__51]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_status] ADD  CONSTRAINT [DF_list_statu_createddate_2__51]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[list_status] ADD  DEFAULT ((0)) FOR [flag_delete]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_suppliers]    Script Date: 13-07-2022 01:57:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_suppliers](
	[id] [uniqueidentifier] NOT NULL,
	[smallname] [varchar](50) NOT NULL,
	[supplier] [varchar](100) NULL,
	[notes] [varchar](max) NULL,
	[suppliertypeid] [uniqueidentifier] NULL,
	[termsid] [uniqueidentifier] NULL,
	[statusid] [uniqueidentifier] NULL,
	[shipviaid] [uniqueidentifier] NULL,
	[fobid] [uniqueidentifier] NULL,
	[customernum] [varchar](50) NULL,
	[employeeid] [uniqueidentifier] NULL,
	[phone] [varchar](50) NULL,
	[fax] [varchar](50) NULL,
	[user8] [varchar](50) NULL,
	[user9] [varchar](50) NULL,
	[user10] [varchar](50) NULL,
	[user11] [decimal](18, 0) NULL,
	[user12] [decimal](18, 0) NULL,
	[user13] [bit] NULL,
	[user14] [bit] NULL,
	[active] [bit] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[supplieremail] [varchar](50) NULL,
	[approved] [bit] NULL,
	[supplier_key] [bit] NULL,
	[approved_date] [smalldatetime] NULL,
	[cagecode] [varchar](50) NULL,
 CONSTRAINT [PK_list_suppliers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supplie__id__3E3ECC20]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__suppi__3F32F059]  DEFAULT (space((1))) FOR [smallname]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_suppl__name__40271492]  DEFAULT (space((1))) FOR [supplier]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__custo__44EBC9AF]  DEFAULT (space((1))) FOR [customernum]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__phone__46D41221]  DEFAULT (space((1))) FOR [phone]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_suppli__fax__47C8365A]  DEFAULT (space((1))) FOR [fax]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__user1__48BC5A93]  DEFAULT (space((1))) FOR [user8]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__user2__49B07ECC]  DEFAULT (space((1))) FOR [user9]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__user3__4AA4A305]  DEFAULT (space((1))) FOR [user10]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__user4__4B98C73E]  DEFAULT ((0)) FOR [user11]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__user5__4C8CEB77]  DEFAULT ((0)) FOR [user12]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__creat__4D810FB0]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_suppliers] ADD  CONSTRAINT [DF__list_supp__creat__4E7533E9]  DEFAULT (getdate()) FOR [createddate]
GO

-------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_suppliers_items]    Script Date: 13-07-2022 01:58:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_suppliers_items](
	[id] [uniqueidentifier] NOT NULL,
	[suppliersid] [uniqueidentifier] NOT NULL,
	[itemsid] [uniqueidentifier] NULL,
	[line] [decimal](18, 0) NULL,
	[supplier_item] [varchar](50) NULL,
	[supplier_uomid] [uniqueidentifier] NULL,
	[supplier_quantity] [decimal](18, 0) NULL,
	[supplier_price] [money] NULL,
	[User1] [varchar](50) NULL,
	[User2] [varchar](50) NULL,
	[User3] [money] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[line_pm] [decimal](18, 0) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supplie__id__34A0534D]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__suppl__35947786]  DEFAULT (space((1))) FOR [supplier_item]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__suppl__36889BBF]  DEFAULT ((0)) FOR [supplier_quantity]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__suppl__377CBFF8]  DEFAULT ((0)) FOR [supplier_price]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__User1__3870E431]  DEFAULT (space((1))) FOR [User1]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__User2__3965086A]  DEFAULT ((0)) FOR [User2]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__User3__3A592CA3]  DEFAULT ((0)) FOR [User3]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__creat__3B4D50DC]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_suppliers_items] ADD  CONSTRAINT [DF__list_supp__creat__3C417515]  DEFAULT (getdate()) FOR [createddate]
GO

-----------------------------------------------------------------------------------------------------------------------------------



/****** Object:  Table [dbo].[list_uoms]    Script Date: 13-07-2022 01:59:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_uoms](
	[id] [uniqueidentifier] NULL,
	[uomref] [varchar](50) NULL,
	[uom1] [varchar](50) NULL,
	[uom2] [varchar](50) NULL,
	[conversion] [decimal](18, 4) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_uoms] ADD  CONSTRAINT [DF__list_uoms__id__147D9C7E]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_uoms] ADD  CONSTRAINT [DF__list_uoms__uom__1571C0B7]  DEFAULT (space((1))) FOR [uom1]
GO

ALTER TABLE [dbo].[list_uoms] ADD  CONSTRAINT [DF__list_uoms__creat__1665E4F0]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_uoms] ADD  CONSTRAINT [DF__list_uoms__creat__175A0929]  DEFAULT (getdate()) FOR [createddate]
GO


-----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_warehouses]    Script Date: 13-07-2022 02:00:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_warehouses](
	[id] [uniqueidentifier] NULL,
	[warehouse] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[list_warehouses] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[list_warehouses] ADD  DEFAULT (space((1))) FOR [warehouse]
GO

ALTER TABLE [dbo].[list_warehouses] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[list_warehouses] ADD  DEFAULT (getdate()) FOR [createddate]
GO


-----------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[po_lines]    Script Date: 13-07-2022 02:01:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[po_lines](
	[id] [uniqueidentifier] NULL,
	[pomainid] [uniqueidentifier] NULL,
	[suppliers_itemsid] [uniqueidentifier] NULL,
	[itemsid] [uniqueidentifier] NULL,
	[locationsid] [uniqueidentifier] NULL,
	[so_linesid] [uniqueidentifier] NULL,
	[linenum] [smallint] NULL,
	[poitem] [varchar](50) NULL,
	[revision] [varchar](50) NULL,
	[description] [varchar](150) NULL,
	[quantity] [decimal](18, 8) NULL,
	[uomid] [uniqueidentifier] NULL,
	[unitcost] [money] NULL,
	[deliverydate] [datetime] NULL,
	[promiseddate] [datetime] NULL,
	[polinesnotes] [varchar](max) NULL,
	[glinv] [varchar](50) NULL,
	[user1] [decimal](18, 0) NULL,
	[user2] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[user3] [varchar](50) NULL,
	[warranty_days] [decimal](18, 0) NULL,
	[warranty_years] [decimal](18, 0) NULL,
	[job] [varchar](50) NULL,
	[qblinetxnid] [varchar](50) NULL,
	[jobid] [varchar](50) NULL,
	[womainid] [uniqueidentifier] NULL,
	[woroutid] [uniqueidentifier] NULL,
	[paid] [bit] NULL,
	[user4] [varchar](50) NULL,
	[user5] [varchar](50) NULL,
	[user6] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__id__1CE8C54D]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__linenu__1DDCE986]  DEFAULT ((0)) FOR [linenum]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__itemnu__1ED10DBF]  DEFAULT (space((1))) FOR [poitem]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__revisi__1FC531F8]  DEFAULT (space((1))) FOR [revision]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__descri__20B95631]  DEFAULT (space((1))) FOR [description]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__quanti__21AD7A6A]  DEFAULT ((0)) FOR [quantity]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__unitco__2395C2DC]  DEFAULT ((0)) FOR [unitcost]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__user1__276653C0]  DEFAULT (space((1))) FOR [glinv]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__create__285A77F9]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF__po_lines__create__294E9C32]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF_po_lines_warranty_days]  DEFAULT ((0)) FOR [warranty_days]
GO

ALTER TABLE [dbo].[po_lines] ADD  CONSTRAINT [DF_po_lines_warranty_years]  DEFAULT ((0)) FOR [warranty_years]
GO

ALTER TABLE [dbo].[po_lines] ADD  DEFAULT (space((1))) FOR [jobid]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[po_main]    Script Date: 13-07-2022 02:02:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[po_main](
	[id] [uniqueidentifier] NOT NULL,
	[pomain] [varchar](50) NOT NULL,
	[podate] [datetime] NULL,
	[requisition] [varchar](50) NULL,
	[revision] [varchar](50) NULL,
	[customerpo] [varchar](50) NULL,
	[buyer] [varchar](50) NULL,
	[glcode] [varchar](50) NULL,
	[apacct] [varchar](50) NULL,
	[smallname] [varchar](50) NULL,
	[suppliersid] [uniqueidentifier] NULL,
	[statusid] [uniqueidentifier] NULL,
	[termsid] [uniqueidentifier] NULL,
	[fobid] [uniqueidentifier] NULL,
	[shipviaid] [uniqueidentifier] NULL,
	[suppliername] [varchar](50) NULL,
	[supplieraddress1] [varchar](50) NULL,
	[supplieraddress2] [varchar](50) NULL,
	[supplieraddress3] [varchar](50) NULL,
	[supplieraddress4] [varchar](50) NULL,
	[supplieraddress5] [varchar](50) NULL,
	[supplieraddress6] [varchar](50) NULL,
	[billtoname] [varchar](50) NULL,
	[billtoaddress1] [varchar](50) NULL,
	[billtoaddress2] [varchar](50) NULL,
	[billtoaddress3] [varchar](50) NULL,
	[billtoaddress4] [varchar](50) NULL,
	[billtoaddress5] [varchar](50) NULL,
	[billtoaddress6] [varchar](50) NULL,
	[shiptoname] [varchar](50) NULL,
	[shiptoaddress1] [varchar](50) NULL,
	[shiptoaddress2] [varchar](50) NULL,
	[shiptoaddress3] [varchar](50) NULL,
	[shiptoaddress4] [varchar](50) NULL,
	[shiptoaddress5] [varchar](50) NULL,
	[shiptoaddress6] [varchar](50) NULL,
	[attention] [varchar](50) NULL,
	[phone] [varchar](50) NULL,
	[fax] [varchar](50) NULL,
	[buyernotes] [varchar](max) NULL,
	[ponotes] [varchar](max) NULL,
	[receivingnotes] [varchar](max) NULL,
	[revisionnotes] [varchar](max) NULL,
	[transferpo] [bit] NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[user3] [varchar](50) NULL,
	[user4] [varchar](50) NULL,
	[user5] [varchar](50) NULL,
	[user10] [varchar](max) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[qbpotxnid] [varchar](50) NULL,
	[requisition_date] [smalldatetime] NULL,
	[required_date] [smalldatetime] NULL,
	[requested_by] [varchar](50) NULL,
	[potype] [varchar](50) NULL,
	[contactsid] [uniqueidentifier] NULL,
	[po_approved] [bit] NULL,
	[po_disapproved] [bit] NULL,
	[modifieddate] [smalldatetime] NULL,
	[user6] [varchar](50) NULL,
	[user7] [varchar](50) NULL,
	[user8] [varchar](50) NULL,
	[user9] [varchar](50) NULL,
	[modifiedby] [varchar](50) NULL,
	[cofc] [bit] NULL,
	[locationsid] [uniqueidentifier] NULL,
	[suppliersid2] [uniqueidentifier] NULL,
	[suppliersid3] [uniqueidentifier] NULL,
 CONSTRAINT [PK_po_main] PRIMARY KEY CLUSTERED 
(
	[pomain] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__id__5A5BE7E1]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__pomain__5B500C1A]  DEFAULT (space((1))) FOR [pomain]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__podate__5C443053]  DEFAULT (getdate()) FOR [podate]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__porev__5D38548C]  DEFAULT (space((1))) FOR [revision]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__custpo__5E2C78C5]  DEFAULT (space((1))) FOR [customerpo]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__buyer__5F209CFE]  DEFAULT (space((1))) FOR [buyer]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__glcode__6108E570]  DEFAULT (space((1))) FOR [glcode]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__apacct__61FD09A9]  DEFAULT (space((1))) FOR [apacct]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__vendor__62F12DE2]  DEFAULT (space((1))) FOR [smallname]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__63E5521B]  DEFAULT (space((1))) FOR [suppliername]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__64D97654]  DEFAULT (space((1))) FOR [supplieraddress1]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__65CD9A8D]  DEFAULT (space((1))) FOR [supplieraddress2]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__66C1BEC6]  DEFAULT (space((1))) FOR [supplieraddress3]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__67B5E2FF]  DEFAULT (space((1))) FOR [supplieraddress4]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__68AA0738]  DEFAULT (space((1))) FOR [supplieraddress5]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__supplie__699E2B71]  DEFAULT (space((1))) FOR [supplieraddress6]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billton__6A924FAA]  DEFAULT (space((1))) FOR [billtoname]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__6B8673E3]  DEFAULT (space((1))) FOR [billtoaddress1]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__6C7A981C]  DEFAULT (space((1))) FOR [billtoaddress2]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__6D6EBC55]  DEFAULT (space((1))) FOR [billtoaddress3]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__6E62E08E]  DEFAULT (space((1))) FOR [billtoaddress4]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__6F5704C7]  DEFAULT (space((1))) FOR [billtoaddress5]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__billtoa__704B2900]  DEFAULT (space((1))) FOR [billtoaddress6]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__713F4D39]  DEFAULT (space((1))) FOR [shiptoaddress1]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__72337172]  DEFAULT (space((1))) FOR [shiptoaddress2]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__732795AB]  DEFAULT (space((1))) FOR [shiptoaddress3]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__741BB9E4]  DEFAULT (space((1))) FOR [shiptoaddress4]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__750FDE1D]  DEFAULT (space((1))) FOR [shiptoaddress5]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__shiptoa__76040256]  DEFAULT (space((1))) FOR [shiptoaddress6]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__attenti__76F8268F]  DEFAULT (space((1))) FOR [attention]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__user1__78E06F01]  DEFAULT (space((1))) FOR [user1]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__user2__79D4933A]  DEFAULT (space((1))) FOR [user2]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__user3__7AC8B773]  DEFAULT (space((1))) FOR [user3]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__user4__7BBCDBAC]  DEFAULT (space((1))) FOR [user4]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__user5__7CB0FFE5]  DEFAULT (space((1))) FOR [user5]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__created__7F8D6C90]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[po_main] ADD  CONSTRAINT [DF__po_main__created__008190C9]  DEFAULT (getdate()) FOR [createddate]
GO


------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[sec_users]    Script Date: 13-07-2022 02:03:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sec_users](
	[id] [uniqueidentifier] NULL,
	[userid] [varchar](50) NULL,
	[firstname] [varchar](50) NULL,
	[lastname] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[defaultwh] [uniqueidentifier] NULL,
	[email] [varchar](50) NULL,
	[buyer] [bit] NULL,
	[salesperson] [bit] NULL,
	[commission] [decimal](2, 2) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[webaccess] [bit] NULL,
	[signature] [varchar](50) NULL,
	[web_access_readonly] [int] NULL,
	[employee] [bit] NULL,
	[user] [bit] NULL,
	[bom_release] [bit] NULL,
	[addlocation] [int] NULL,
	[view_price] [bit] NULL,
	[flag_delete] [bit] NULL,
	[ADDITEMNO] [bit] NULL,
	[ADDWAREHOUSE] [bit] NULL,
	[ADDREV] [bit] NULL,
	[ADDITEMCLASS] [bit] NULL,
	[ADDITEMCODE] [bit] NULL,
	[ADDUOMREF] [bit] NULL,
	[ADDLOTNO] [bit] NULL,
	[ADDREASONCODEIN] [bit] NULL,
	[ADDREASONCODEOUT] [bit] NULL,
	[forgotpwdans] [varchar](50) NULL,
	[theme] [varchar](50) NULL,
	[menumode] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__id__5A1BF0BF]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__first__5B1014F8]  DEFAULT (space((1))) FOR [firstname]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__lastn__5C043931]  DEFAULT (space((1))) FOR [lastname]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__passw__5CF85D6A]  DEFAULT (space((1))) FOR [password]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__email__5DEC81A3]  DEFAULT (space((1))) FOR [email]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF_sec_users_buyer]  DEFAULT ((0)) FOR [buyer]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF_sec_users_salesperson]  DEFAULT ((0)) FOR [salesperson]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__creat__5EE0A5DC]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF__sec_users__creat__5FD4CA15]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[sec_users] ADD  CONSTRAINT [DF_sec_users_webaccess]  DEFAULT ((0)) FOR [webaccess]
GO

ALTER TABLE [dbo].[sec_users] ADD  DEFAULT ((0)) FOR [bom_release]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'da077f35-e371-4930-9eaa-f1a7e98e040c' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sec_users', @level2type=N'COLUMN',@level2name=N'id'
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[sec_users_warehouses]    Script Date: 13-07-2022 02:03:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sec_users_warehouses](
	[id] [uniqueidentifier] NULL,
	[users_id] [uniqueidentifier] NULL,
	[warehouses_id] [uniqueidentifier] NULL,
	[transok] [bit] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sec_users_warehouses] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[sec_users_warehouses] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[sec_users_warehouses] ADD  DEFAULT (getdate()) FOR [createddate]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[so_lines]    Script Date: 13-07-2022 02:04:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[so_lines](
	[id] [uniqueidentifier] NOT NULL,
	[somainid] [uniqueidentifier] NULL,
	[customers_itemsid] [uniqueidentifier] NULL,
	[itemsid] [uniqueidentifier] NULL,
	[linenum] [decimal](18, 0) NULL,
	[itemnumber] [varchar](50) NULL,
	[description] [varchar](150) NULL,
	[uom] [varchar](50) NULL,
	[glcode] [varchar](50) NULL,
	[quantity] [decimal](18, 8) NULL,
	[estunitcost] [money] NULL,
	[unitcost] [money] NULL,
	[markup] [decimal](18, 8) NULL,
	[transfercost] [money] NULL,
	[estshipdate] [smalldatetime] NULL,
	[shipdate] [smalldatetime] NULL,
	[solinesnotes] [varchar](max) NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[user3] [decimal](18, 0) NULL,
	[user4] [decimal](18, 0) NULL,
	[user5] [money] NULL,
	[user6] [money] NULL,
	[user7] [datetime] NULL,
	[user8] [datetime] NULL,
	[user9] [bit] NULL,
	[user10] [bit] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[revision] [varchar](50) NULL,
	[warranty_days] [decimal](18, 0) NULL,
	[warranty_years] [decimal](18, 0) NULL,
	[custitem] [varchar](50) NULL,
	[create_wo] [bit] NULL,
	[incl_low_lev] [bit] NULL,
	[allocate] [bit] NULL,
	[fullprice] [money] NULL,
	[modifiedby] [varchar](50) NULL,
	[modifieddate] [smalldatetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__id__26081B33]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__linenu__26FC3F6C]  DEFAULT ((0)) FOR [linenum]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__itemnu__27F063A5]  DEFAULT (space((1))) FOR [itemnumber]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__descri__28E487DE]  DEFAULT (space((1))) FOR [description]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__uom__29D8AC17]  DEFAULT (space((1))) FOR [uom]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__glcode__2ACCD050]  DEFAULT (space((1))) FOR [glcode]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__quanti__2BC0F489]  DEFAULT ((0)) FOR [quantity]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__unitco__2CB518C2]  DEFAULT ((0)) FOR [unitcost]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF_so_lines_estshipdate]  DEFAULT (getdate()) FOR [estshipdate]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__dateto__2DA93CFB]  DEFAULT (getdate()) FOR [shipdate]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__user1__2E9D6134]  DEFAULT (space((1))) FOR [user1]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__create__2F91856D]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF__so_lines__create__3085A9A6]  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF_so_lines_warranty_days]  DEFAULT ((0)) FOR [warranty_days]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF_so_lines_warranty_years]  DEFAULT ((0)) FOR [warranty_years]
GO

ALTER TABLE [dbo].[so_lines] ADD  CONSTRAINT [DF_so_lines_modifieddate]  DEFAULT (getdate()) FOR [modifieddate]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[so_main]    Script Date: 13-07-2022 02:05:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[so_main](
	[id] [uniqueidentifier] NOT NULL,
	[somain] [varchar](50) NOT NULL,
	[description] [varchar](50) NULL,
	[sodate] [datetime] NULL,
	[reqdate] [datetime] NULL,
	[custpo] [varchar](50) NULL,
	[salesperson] [varchar](50) NULL,
	[salestax] [money] NULL,
	[commission] [decimal](18, 2) NULL,
	[glcode] [varchar](50) NULL,
	[aracct] [varchar](50) NULL,
	[budget] [money] NULL,
	[customername] [varchar](50) NULL,
	[customersid] [uniqueidentifier] NULL,
	[projectid] [uniqueidentifier] NULL,
	[statusid] [uniqueidentifier] NULL,
	[termsid] [uniqueidentifier] NULL,
	[fobid] [uniqueidentifier] NULL,
	[shipviaid] [uniqueidentifier] NULL,
	[ordertypeid] [uniqueidentifier] NULL,
	[billtoname] [varchar](50) NULL,
	[billtoaddress1] [varchar](50) NULL,
	[billtoaddress2] [varchar](50) NULL,
	[billtoaddress3] [varchar](50) NULL,
	[billtoaddress4] [varchar](50) NULL,
	[billtoaddress5] [varchar](50) NULL,
	[billtoaddress6] [varchar](50) NULL,
	[shiptoname] [varchar](50) NULL,
	[shiptoaddress1] [varchar](50) NULL,
	[shiptoaddress2] [varchar](50) NULL,
	[shiptoaddress3] [varchar](50) NULL,
	[shiptoaddress4] [varchar](50) NULL,
	[shiptoaddress5] [varchar](50) NULL,
	[shiptoaddress6] [varchar](50) NULL,
	[contactsid] [uniqueidentifier] NULL,
	[sonotes] [text] NULL,
	[shipnotes] [text] NULL,
	[salespersonnotes] [text] NULL,
	[packnotes] [text] NULL,
	[invoicenotes] [text] NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[user3] [varchar](50) NULL,
	[user4] [varchar](50) NULL,
	[user9] [text] NULL,
	[user10] [text] NULL,
	[user12] [money] NULL,
	[user13] [money] NULL,
	[user14] [datetime] NULL,
	[user15] [datetime] NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[est] [smalldatetime] NULL,
	[funded] [smalldatetime] NULL,
	[prcomp] [smalldatetime] NULL,
	[pocosts] [varchar](50) NULL,
	[womtlcosts] [varchar](50) NULL,
	[wolbrcosts] [varchar](50) NULL,
	[woohcost] [varchar](50) NULL,
	[socost] [varchar](50) NULL,
	[statusid_prior] [uniqueidentifier] NULL,
	[statusid_change] [smalldatetime] NULL,
	[statusid_change_by] [varchar](50) NULL,
	[modifieddate] [smalldatetime] NULL,
	[user6] [varchar](200) NULL,
	[user5] [varchar](50) NULL,
	[user7] [varchar](200) NULL,
	[user8] [varchar](200) NULL,
	[mdat_in] [varchar](50) NULL,
	[org] [varchar](50) NULL,
 CONSTRAINT [PK_so_main] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__id__385BD598]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__somain__394FF9D1]  DEFAULT (space((1))) FOR [somain]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__sodate__3A441E0A]  DEFAULT (getdate()) FOR [sodate]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__custpo__3B384243]  DEFAULT (space((1))) FOR [custpo]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__salespe__3C2C667C]  DEFAULT (space((1))) FOR [salesperson]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__glcode__3E14AEEE]  DEFAULT (space((1))) FOR [glcode]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__aracct__3F08D327]  DEFAULT (space((1))) FOR [aracct]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__custome__3FFCF760]  DEFAULT (space((1))) FOR [customername]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billton__40F11B99]  DEFAULT (space((1))) FOR [billtoname]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__41E53FD2]  DEFAULT (space((1))) FOR [billtoaddress1]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__42D9640B]  DEFAULT (space((1))) FOR [billtoaddress2]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__43CD8844]  DEFAULT (space((1))) FOR [billtoaddress3]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__44C1AC7D]  DEFAULT (space((1))) FOR [billtoaddress4]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__45B5D0B6]  DEFAULT (space((1))) FOR [billtoaddress5]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__billtoa__46A9F4EF]  DEFAULT (space((1))) FOR [billtoaddress6]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shipton__479E1928]  DEFAULT (space((1))) FOR [shiptoname]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__48923D61]  DEFAULT (space((1))) FOR [shiptoaddress1]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__4986619A]  DEFAULT (space((1))) FOR [shiptoaddress2]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__4A7A85D3]  DEFAULT (space((1))) FOR [shiptoaddress3]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__4B6EAA0C]  DEFAULT (space((1))) FOR [shiptoaddress4]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__4C62CE45]  DEFAULT (space((1))) FOR [shiptoaddress5]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__shiptoa__4D56F27E]  DEFAULT (space((1))) FOR [shiptoaddress6]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__user1__4E4B16B7]  DEFAULT (space((1))) FOR [user1]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__user2__4F3F3AF0]  DEFAULT (space((1))) FOR [user2]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__user3__50335F29]  DEFAULT (space((1))) FOR [user3]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__user4__51278362]  DEFAULT (space((1))) FOR [user4]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__created__5403F00D]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[so_main] ADD  CONSTRAINT [DF__so_main__created__54F81446]  DEFAULT (getdate()) FOR [createddate]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[translog]    Script Date: 13-07-2022 02:06:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[translog](
	[id] [uniqueidentifier] NULL,
	[itemsid] [uniqueidentifier] NULL,
	[trans] [varchar](50) NULL,
	[transdate] [smalldatetime] NULL,
	[transqty] [float] NULL,
	[itemnumber] [varchar](50) NULL,
	[description] [varchar](150) NULL,
	[itemtype] [varchar](50) NULL,
	[warehouse] [varchar](50) NULL,
	[location] [varchar](50) NULL,
	[reason] [varchar](50) NULL,
	[source] [varchar](50) NULL,
	[ref] [varchar](50) NULL,
	[invtype] [varchar](50) NULL,
	[notes] [varchar](max) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[revision] [varchar](50) NULL,
	[modifieddate] [datetime] NULL,
	[user1] [varchar](50) NULL,
	[user2] [varchar](50) NULL,
	[lic_plate] [varchar](50) NULL,
	[lic_plate_flag] [bit] NULL,
	[cost] [decimal](18, 8) NULL,
	[inv_receiptsid] [uniqueidentifier] NULL,
	[current_qty] [decimal](9, 2) NULL,
	[project] [varchar](50) NULL,
	[transnum] [int] NULL,
	[location2] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [trans]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (getdate()) FOR [transdate]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT ((0)) FOR [transqty]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [itemnumber]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [description]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [itemtype]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [warehouse]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [location]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [reason]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [source]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [ref]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [invtype]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT (getdate()) FOR [createddate]
GO

ALTER TABLE [dbo].[translog] ADD  DEFAULT ('') FOR [location2]
GO
-------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[transloglot]    Script Date: 13-07-2022 02:06:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transloglot](
	[id] [uniqueidentifier] ROWGUIDCOL  NULL,
	[translogid] [uniqueidentifier] NULL,
	[quantity] [float] NULL,
	[lotno] [varchar](50) NULL,
	[expdate] [datetime] NULL,
	[color] [varchar](50) NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [smalldatetime] NULL,
	[location] [varchar](50) NULL,
	[warehouse] [varchar](50) NULL,
	[lic_plate_flag] [bit] NULL,
	[lic_plate] [varchar](50) NULL,
	[cost] [decimal](18, 8) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[transloglot] ADD  CONSTRAINT [DF_v_transloglot_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[transloglot] ADD  CONSTRAINT [DF_v_transloglot_createddate]  DEFAULT (getdate()) FOR [createddate]
GO


----------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[translogsn]    Script Date: 13-07-2022 02:07:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[translogsn](
	[id] [uniqueidentifier] NULL,
	[translogid] [uniqueidentifier] NULL,
	[serno] [varchar](50) NULL,
	[tagno] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[location] [varchar](50) NULL,
	[warehouse] [varchar](50) NULL,
	[lic_plate_flag] [bit] NULL,
	[lic_plate] [varchar](50) NULL,
	[model] [varchar](50) NULL,
	[cost] [decimal](18, 8) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[translogsn] ADD  CONSTRAINT [DF__translogsn__id__55799938]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[translogsn] ADD  CONSTRAINT [DF__translogs__serno__566DBD71]  DEFAULT (space((1))) FOR [serno]
GO

ALTER TABLE [dbo].[translogsn] ADD  CONSTRAINT [DF__translogs__tagno__5761E1AA]  DEFAULT (space((1))) FOR [tagno]
GO

ALTER TABLE [dbo].[translogsn] ADD  CONSTRAINT [DF__translogs__creat__585605E3]  DEFAULT (space((1))) FOR [createdby]
GO

ALTER TABLE [dbo].[translogsn] ADD  CONSTRAINT [DF__translogs__creat__594A2A1C]  DEFAULT (getdate()) FOR [createddate]
GO

--------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_field_properties]    Script Date: 14-07-2022 10:43:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_field_properties](
	[id] [uniqueidentifier] NULL,
	[labelnumber] [decimal](18, 0) NULL,
	[defaultlabel] [varchar](50) NULL,
	[mylabel] [nvarchar](150) NULL,
	[visible] [bit] NULL,
	[controlname] [varchar](50) NULL,
	[left] [smallint] NULL,
	[top] [smallint] NULL,
	[screenname] [varchar](50) NULL,
	[helptext] [varchar](50) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL,
	[tabnumber] [smallint] NULL,
	[tabstop] [bit] NULL,
	[udf] [bit] NULL,
	[mandatry] [bit] NULL,
	[grdcolwidth] [smallint] NULL
) ON [PRIMARY]

GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[list_messages]    Script Date: 14-07-2022 10:46:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[list_messages](
	[id] [uniqueidentifier] NULL,
	[messagenum] [decimal](18, 0) NULL,
	[defaulttext] [varchar](200) NULL,
	[messagetext] [varchar](200) NULL,
	[createdby] [varchar](50) NOT NULL,
	[createddate] [datetime] NOT NULL
) ON [PRIMARY]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[login_cred]    Script Date: 14-07-2022 03:56:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[login_cred](
	[id] [uniqueidentifier] NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[dbname] [varchar](50) NULL,
	[createddate] [smalldatetime] NULL
) ON [PRIMARY]
GO

------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[inv_mdat_out]    Script Date: 15-07-2022 05:27:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inv_mdat_out](
	[id] [uniqueidentifier] NOT NULL,
	[mdat_num] [varchar](50) NOT NULL,
	[somainId] [uniqueidentifier] NOT NULL,
	[description] [varchar](50) NULL,
	[statusid] [uniqueidentifier] NULL,
	[packlistnum] [int] NULL,
	[submitted_date] [datetime] NULL,
	[approved_date] [datetime] NULL,
	[shipped_date] [datetime] NULL,
	[cancelled_date] [datetime] NULL,
	[createdby] [varchar](50) NULL,
	[createddate] [datetime] NULL,
	[shippingid] [uniqueidentifier] NULL,
	[modifiedby] [varchar](50) NULL,
	[modifieddate] [datetime] NULL
) ON [PRIMARY]
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------






