USE GCEFTest;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE ROUTINE_NAME = 'RebuildTables')
	DROP PROCEDURE RebuildTables;
GO

CREATE PROCEDURE RebuildTables AS
BEGIN
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ImagePaths')
		DROP TABLE ImagePaths;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SaleRecords')
		DROP TABLE SaleRecords;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PurchaseTypes')
		DROP TABLE PurchaseTypes;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'VehicleDetails')
		DROP TABLE VehicleDetails;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Details')
		DROP TABLE Details;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Vehicles')
		DROP TABLE Vehicles;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Models')
		DROP TABLE Models;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Makes')
		DROP TABLE Makes;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BodyStyles')
		DROP TABLE BodyStyles;
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TransmissionTypes')
		DROP TABLE TransmissionTypes;
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
		DROP TABLE AspNetUserRoles;
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
		DROP TABLE AspNetUsers;
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
		DROP TABLE AspNetRoles;

	

	CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](10) NULL,
	 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]





	CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]







	CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[RoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[AspNetRoles] ([Id])

	ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]

	ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
	REFERENCES [dbo].[AspNetUsers] ([Id])

	ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]






	CREATE TABLE [dbo].[TransmissionTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	 CONSTRAINT [PK_dbo.TransmissionTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	CREATE TABLE [dbo].[BodyStyles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	 CONSTRAINT [PK_dbo.BodyStyles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]




	CREATE TABLE [dbo].[Makes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	 CONSTRAINT [PK_dbo.Makes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]




	CREATE TABLE [dbo].[Models](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[MakeId] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.Models] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Models]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Models_dbo.Makes_MakeId] FOREIGN KEY([MakeId])
	REFERENCES [dbo].[Makes] ([Id])

	ALTER TABLE [dbo].[Models] CHECK CONSTRAINT [FK_dbo.Models_dbo.Makes_MakeId]




	CREATE TABLE [dbo].[PurchaseTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	 CONSTRAINT [PK_dbo.PurchaseTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]





	CREATE TABLE [dbo].[Vehicles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Color] [nvarchar](20) NOT NULL,
	[Interior] [nvarchar](20) NOT NULL,
	[Mileage] [decimal](18, 2) NOT NULL,
	[VIN] [nvarchar](17) NOT NULL,
	[MSRP] [decimal](18, 2) NOT NULL,
	[SalePrice] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[OwnerId] [nvarchar](max) NULL,
	[ModelId] [int] NOT NULL,
	[BodyStyleId] [int] NOT NULL,
	[TransmissionTypeId] [int] NOT NULL,
	[IsNew] [bit] NOT NULL,
	[HighwayMpg] [smallint] NOT NULL,
	[CityMpg] [smallint] NOT NULL,
	[Engine] [nvarchar](20) NOT NULL,
	 CONSTRAINT [PK_dbo.Vehicles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((0)) FOR [BodyStyleId]

	ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((0)) FOR [TransmissionTypeId]

	ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((0)) FOR [IsNew]

	ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((0)) FOR [HighwayMpg]

	ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((0)) FOR [CityMpg]

	ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Vehicles_dbo.BodyStyles_BodyStyleId] FOREIGN KEY([BodyStyleId])
	REFERENCES [dbo].[BodyStyles] ([Id])

	ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_dbo.Vehicles_dbo.BodyStyles_BodyStyleId]

	ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Vehicles_dbo.Models_ModelId] FOREIGN KEY([ModelId])
	REFERENCES [dbo].[Models] ([Id])

	ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_dbo.Vehicles_dbo.Models_ModelId]

	ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Vehicles_dbo.TransmissionTypes_TransmissionTypeId] FOREIGN KEY([TransmissionTypeId])
	REFERENCES [dbo].[TransmissionTypes] ([Id])

	ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_dbo.Vehicles_dbo.TransmissionTypes_TransmissionTypeId]
	




	CREATE TABLE [dbo].[ImagePaths](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](max) NULL,
	[VehicleId] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.ImagePaths] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[ImagePaths]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImagePaths_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
	REFERENCES [dbo].[Vehicles] ([Id])

	ALTER TABLE [dbo].[ImagePaths] CHECK CONSTRAINT [FK_dbo.ImagePaths_dbo.Vehicles_VehicleId]




	CREATE TABLE [dbo].[SaleRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [nvarchar](50) NULL,
	[EmployeeId] [nvarchar](50) NULL,
	[VehicleId] [int] NOT NULL,
	[PurchasePrice] [decimal](18, 2) NOT NULL,
	[ExpectedSalePrice] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[TradeInId] [int] NULL,
	[PurchaseTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.SaleRecords] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[SaleRecords] ADD  DEFAULT ((0)) FOR [PurchaseTypeId]

	ALTER TABLE [dbo].[SaleRecords]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SaleRecords_dbo.PurchaseTypes_PurchaseTypeId] FOREIGN KEY([PurchaseTypeId])
	REFERENCES [dbo].[PurchaseTypes] ([Id])

	ALTER TABLE [dbo].[SaleRecords] CHECK CONSTRAINT [FK_dbo.SaleRecords_dbo.PurchaseTypes_PurchaseTypeId]

	ALTER TABLE [dbo].[SaleRecords]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SaleRecords_dbo.Vehicles_TradeInId] FOREIGN KEY([TradeInId])
	REFERENCES [dbo].[Vehicles] ([Id])

	ALTER TABLE [dbo].[SaleRecords] CHECK CONSTRAINT [FK_dbo.SaleRecords_dbo.Vehicles_TradeInId]

	ALTER TABLE [dbo].[SaleRecords]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SaleRecords_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
	REFERENCES [dbo].[Vehicles] ([Id])

	ALTER TABLE [dbo].[SaleRecords] CHECK CONSTRAINT [FK_dbo.SaleRecords_dbo.Vehicles_VehicleId]



	


	CREATE TABLE [dbo].[Details](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsKeyFeature] [bit] NOT NULL,
	 CONSTRAINT [PK_dbo.Details] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]




	CREATE TABLE [dbo].[VehicleDetails](
	[Detail_Id] [int] NOT NULL,
	[Vehicle_Id] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.VehicleDetails] PRIMARY KEY CLUSTERED 
	(
		[Vehicle_Id] ASC,
		[Detail_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[VehicleDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetailVehicles_dbo.Details_Detail_Id] FOREIGN KEY([Detail_Id])
	REFERENCES [dbo].[Details] ([Id])

	ALTER TABLE [dbo].[VehicleDetails] CHECK CONSTRAINT [FK_dbo.DetailVehicles_dbo.Details_Detail_Id]

	ALTER TABLE [dbo].[VehicleDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetailVehicles_dbo.Vehicles_Vehicle_Id] FOREIGN KEY([Vehicle_Id])
	REFERENCES [dbo].[Vehicles] ([Id])

	ALTER TABLE [dbo].[VehicleDetails] CHECK CONSTRAINT [FK_dbo.DetailVehicles_dbo.Vehicles_Vehicle_Id]
END
GO