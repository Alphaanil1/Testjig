USE [master]
GO
/****** Object:  Database [ESD_Testjig_DB]    Script Date: 04-03-2024 14:51:33 ******/
CREATE DATABASE [ESD_Testjig_DB] 
GO
USE [ESD_Testjig_DB]
GO
/****** Object:  Table [dbo].[tbl_ErrorCode]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ErrorCode](
	[ErrorCodeId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorCode] [varchar](10) NULL,
	[ErrorMessage] [varchar](50) NULL,
	[TestCaseId] [int] NULL,
 CONSTRAINT [PK_tbl_ErrorCode] PRIMARY KEY CLUSTERED 
(
	[ErrorCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PCBType]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PCBType](
	[PCBTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PCBType] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [nchar](10) NULL,
 CONSTRAINT [PK_tbl_PCBType] PRIMARY KEY CLUSTERED 
(
	[PCBTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Result]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Result](
	[ResultId] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[SerialNo] [nvarchar](30) NULL,
	[PCBTypeID] [int] NULL,
	[PCBType] [nvarchar](20) NULL,
	[TestCaseID] [int] NULL,
	[Status] [nvarchar](20) NULL,
	[FrameToSend] [nvarchar](max) NULL,
	[ResponseFrame] [nvarchar](max) NULL,
	[TestType] [nvarchar](20) NULL,
	[Comment] [nvarchar](70) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[Date] [nvarchar](50) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [nchar](5) NULL,
 CONSTRAINT [PK_tbl_Result] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TestCases]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TestCases](
	[TestCaseID] [int] IDENTITY(1,1) NOT NULL,
	[TestCaseName] [nvarchar](30) NULL,
	[PCBTypeID] [int] NULL,
	[PCBType] [nvarchar](20) NULL,
	[TestType] [nvarchar](20) NULL,
	[Parameter] [nvarchar](100) NULL,
	[MinValue] [nvarchar](10) NULL,
	[MaxValue] [nvarchar](10) NULL,
	[FrameToSend] [nvarchar](50) NULL,
	[SendFrame] [nvarchar](50) NULL,
	[PassFrame] [nvarchar](50) NULL,
	[FailFrame] [nvarchar](50) NULL,
	[ResponseFrame] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [nchar](5) NULL,
	[IsSpecialTest] [nchar](3) NULL,
 CONSTRAINT [PK_tbl)TestCasesDummy] PRIMARY KEY CLUSTERED 
(
	[TestCaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_User]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address1] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](30) NULL,
	[State] [nvarchar](30) NULL,
	[Country] [nvarchar](30) NULL,
	[Pincode] [nvarchar](10) NULL,
	[Email] [nvarchar](35) NULL,
	[Mobile1] [nvarchar](15) NULL,
	[Mobile2] [nvarchar](15) NULL,
	[UserName] [nvarchar](30) NULL,
	[Password] [nvarchar](30) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[RoleID] [int] NULL,
	[Role] [nvarchar](15) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [nchar](5) NULL,
 CONSTRAINT [PK_tbl_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_UserRoles]    Script Date: 04-03-2024 14:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_UserRoles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Role] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [nchar](5) NULL,
 CONSTRAINT [PK_tbl_UserRoles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_ErrorCode] ON 
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (1, N'1000
', N'Response Timeout', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (2, N'1001
', N'CRC error', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (3, N'1002', N'Invalid payload length', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (4, N'1003', N'Invalid test number', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (5, N'1004
', N'Invalid sub test number', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (6, N'1005', N'Invalid PCB test number', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (7, N'1006', N'Invalid Sender/receiver number', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (8, N'1007', N'Invalid error code', NULL)
GO
INSERT [dbo].[tbl_ErrorCode] ([ErrorCodeId], [ErrorCode], [ErrorMessage], [TestCaseId]) VALUES (9, N'1008', N'Invalid payload', NULL)
GO
SET IDENTITY_INSERT [dbo].[tbl_ErrorCode] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_PCBType] ON 
GO
INSERT [dbo].[tbl_PCBType] ([PCBTypeID], [PCBType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (1, N'Control', NULL, NULL, NULL, NULL, N'Y         ')
GO
INSERT [dbo].[tbl_PCBType] ([PCBTypeID], [PCBType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (2, N'Relay', NULL, NULL, NULL, NULL, N'Y         ')
GO
SET IDENTITY_INSERT [dbo].[tbl_PCBType] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_TestCases] ON 
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (1, N'DC - DC converter I/P ', 1, N'Control', N'Manual', N'Input supply voltage', N'4.60V', N'5.40V', N'*12A010006DCDCIPCRCC#', N'*12A010006DCDCIPDFFA#', N'*21A010006DCDCIPB17C#', NULL, N'*21A010006DCDCIPCRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (2, N'DC - DC converter O/P', 1, N'Control', N'Manual', N'VCC - 3.3V, output of DCDC converter', N'3.00V', N'3.60V', N'*12A020006DCDCOPCRCC#', N'*12A020006DCDCOP0DA6#', N'*21A020006DCDCOP6320#', NULL, N'*21A020006DCDCOPCRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (3, N'LED', 1, N'Control', N'Manual', N'Observe the LED colours', NULL, NULL, N'*12A030003LEDCRCC#', N'*12A030003LED911E#', N'*21A030003LEDEC7B#', NULL, N'*21A030003LEDCRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (4, N'Relay - 10A', 1, N'Control', N'Manual', N'Relay - 10A contact', NULL, NULL, N'*12A040006RELAY1CRCC#', N'*12A040006RELAY19843#', N'*21A040006RELAY1F6C5#', NULL, N'*21A040006RELAY1CRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (5, N'Relay - 40A', 1, N'Control', N'Manual', N'Relay - 40A contact', NULL, NULL, N'*12A050006RELAY2CRCC#', N'*12A050006RELAY27069#', N'*21A050006RELAY21EEF#', NULL, N'*21A050006RELAY2CRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (6, N'RFID Card', 1, N'Control', N'Hybrid', N'Keep card in front of encoder
', NULL, NULL, N'*12A060004RFIDCRCC#', N'*12A060004RFID0331# ', N'*21A060005RFIDP5C30#', N'*21A060005RFIDF2EC7#', N'*21A060005RFIDxCRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (7, N'Encrypted RFID Card', 1, N'Control', N'Hybrid', N'Keep card in front of encoder
', NULL, NULL, N'*12A070004RFIDCRCC#', N'*12A070004RFIDE812# ', N'*21A070005RFIDP3375#', N'*21A070005RFIDF4182#', N'*21A070005RFIDxCRCC#', NULL, NULL, N'N    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (8, N'Internal flash', 1, N'Control', N'Auto', N'Read/Write operation', NULL, NULL, N'*12A070005FLASHCRCC#', N'*12A070005FLASH34F8#', N'*21A070006FLASHP6226#', N'*21A080006FLASHF9BE2#', N'*21A070006FLASHF10D1#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (9, N'DC - DC converter O/P 24V', 2, N'Relay', N'Manual', N'VCC - 24V', N'23.60V', N'24.40V', N'*12B010008DCDCOP24CRCC#', N'*12B010008DCDCOP24E9A7#', N'*21B010006DCDCOP2443BB#', NULL, N'*21B010006DCDCOP24CRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (10, N'DC - DC converter O/P', 2, N'Relay', N'Manual', N'VCC - 5V, output of DCDC converter', N'4.60V', N'5.40V', N'*12B020007DCDCOP5CRCC#', N'*12B020007DCDCOP5DB2B#', N'*21B020007DCDCOP5D043#', NULL, N'*21B020007DCDCOP5CRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (11, N'Relay - 10A', 2, N'Relay', N'Manual', N'Relay - 10A contact', NULL, NULL, N'*12B040006RELAY1CRCC#', N'*12B040006RELAY157E6#', N'*21B040006RELAY13960#', NULL, N'*21B040006RELAY1CRCC#', NULL, NULL, N'Y    ', NULL)
GO
INSERT [dbo].[tbl_TestCases] ([TestCaseID], [TestCaseName], [PCBTypeID], [PCBType], [TestType], [Parameter], [MinValue], [MaxValue], [FrameToSend], [SendFrame], [PassFrame], [FailFrame], [ResponseFrame], [CreatedBy], [CreatedDate], [IsActive], [IsSpecialTest]) VALUES (12, N'Relay - 40A', 2, N'Relay', N'Manual', N'Relay - 40A contact', NULL, NULL, N'*12B050006RELAY2CRCC#', N'*12B050006RELAY2BFCC#', N'*21B050006RELAY2D14A#', NULL, N'*21B050006RELAY2CRCC#', NULL, NULL, N'Y    ', NULL)
GO
SET IDENTITY_INSERT [dbo].[tbl_TestCases] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_User] ON 
GO
INSERT [dbo].[tbl_User] ([UserID], [Name], [Address1], [Address2], [City], [State], [Country], [Pincode], [Email], [Mobile1], [Mobile2], [UserName], [Password], [CreatedBy], [CreatedDate], [RoleID], [Role], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (1, N'Alpha', NULL, NULL, N'Pune', N'Maharashtra', N'India', NULL, NULL, NULL, NULL, N'admin', N'admin', NULL, CAST(N'2019-12-02T12:38:43.930' AS DateTime), 1, N'Admin', NULL, NULL, N'Y    ')
GO
INSERT [dbo].[tbl_User] ([UserID], [Name], [Address1], [Address2], [City], [State], [Country], [Pincode], [Email], [Mobile1], [Mobile2], [UserName], [Password], [CreatedBy], [CreatedDate], [RoleID], [Role], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (2, N'Mamata', NULL, NULL, NULL, NULL, NULL, NULL, N'', N'9856565655', NULL, N'Mamata', N'mamata', 1, CAST(N'2019-12-26T15:41:09.013' AS DateTime), 2, NULL, NULL, NULL, N'Y    ')
GO
INSERT [dbo].[tbl_User] ([UserID], [Name], [Address1], [Address2], [City], [State], [Country], [Pincode], [Email], [Mobile1], [Mobile2], [UserName], [Password], [CreatedBy], [CreatedDate], [RoleID], [Role], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (3, N'bhagyashri', NULL, NULL, NULL, NULL, NULL, NULL, N'', N'7894561232', NULL, N'bhagyashri', N'789456123', 1, CAST(N'2024-01-15T10:31:05.473' AS DateTime), 1, NULL, NULL, NULL, N'Y    ')
GO
INSERT [dbo].[tbl_User] ([UserID], [Name], [Address1], [Address2], [City], [State], [Country], [Pincode], [Email], [Mobile1], [Mobile2], [UserName], [Password], [CreatedBy], [CreatedDate], [RoleID], [Role], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (4, N'alpha', NULL, NULL, NULL, NULL, NULL, NULL, N'', N'9876543210', NULL, N'alpha', N'alpha', 1, CAST(N'2024-01-15T11:08:03.423' AS DateTime), 1, NULL, NULL, NULL, N'Y    ')
GO
SET IDENTITY_INSERT [dbo].[tbl_User] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_UserRoles] ON 
GO
INSERT [dbo].[tbl_UserRoles] ([RoleID], [Role], [CreatedBy], [CreatedDate], [IsActive]) VALUES (1, N'Admin', NULL, NULL, N'Y    ')
GO
INSERT [dbo].[tbl_UserRoles] ([RoleID], [Role], [CreatedBy], [CreatedDate], [IsActive]) VALUES (2, N'Operator', NULL, NULL, N'Y    ')
GO
SET IDENTITY_INSERT [dbo].[tbl_UserRoles] OFF
GO
ALTER TABLE [dbo].[tbl_PCBType]  WITH CHECK ADD  CONSTRAINT [FK_tbl_PCBType_tbl_PCBType] FOREIGN KEY([PCBTypeID])
REFERENCES [dbo].[tbl_PCBType] ([PCBTypeID])
GO
ALTER TABLE [dbo].[tbl_PCBType] CHECK CONSTRAINT [FK_tbl_PCBType_tbl_PCBType]
GO
/****** Object:  StoredProcedure [dbo].[GetErrorMsg]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <Create Date,13-11-2018,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetErrorMsg]
	-- Add the parameters for the stored procedure here
	@TestCaseId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT        ErrorCodeId, ErrorCode, ErrorMessage, TestCaseId
FROM            tbl_ErrorCode
WHERE  (TestCaseId=@TestCaseId)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_CompareADCMinMaxValue]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_CompareADCMinMaxValue] 
	-- Add the parameters for the stored procedure here
	@Current decimal(18,3),
	@PCBTypeId int,
	@TestcaseId int
AS
BEGIN
	DECLARE @Min decimal(18,3)
	DECLARE @Max decimal(18,3)

	SELECT 
		@Min=MinValue,
		@Max=MaxValue
	FROM tbl_TestCases
	WHERE (IsActive='Y') AND  (PCBTypeID=@PCBTypeId) AND (TestCaseID=@TestcaseId) 
	

SELECT   TestCaseID, TestCaseName, PCBTypeID, PCBType, MinValue, MaxValue
FROM     tbl_TestCases  
WHERE    (IsActive='Y') AND  (PCBTypeID=@PCBTypeId) AND (TestCaseID=@TestcaseId) 
AND ( @Current>=@Min AND @Current<=@Max)


--EXECUTE usp_CompareADCMinMaxValue @Current=4.101, @PCBTypeId=9, @TestcaseId = 63 

--EXECUTE usp_CompareADCMinMaxValue @Current=4.40, @PCBTypeId=1, @TestcaseId = 10 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_CompareMinMaxValue]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_CompareMinMaxValue]
	-- Add the parameters for the stored procedure here
	@Current nvarchar(50),
	@PCBTypeId int,
	@TestcaseId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
    SELECT   TestCaseID, TestCaseName, PCBTypeID, PCBType, MinValue, MaxValue
FROM     tbl_TestCases  
WHERE    (IsActive='Y') AND ((CONVERT(int,@Current)) between (CONVERT(int,MinValue)) AND (CONVERT(int,MaxValue)))
AND (PCBTypeID=@PCBTypeId) AND (TestCaseID=@TestcaseId)

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllUser]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAllUser] 
	-- Add the parameters for the stored procedure here
	--@Createdby int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT        UserID, Name
FROM            tbl_User
WHERE (IsActive='Y') -- AND (CreatedBy=@Createdby)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAlluserRole]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAlluserRole]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SELECT        RoleID, Role
FROM            tbl_UserRoles
WHERE IsActive='Y'
  
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAutoTestCases]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Bhagyashri Patil>
-- Create date: <11/18>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAutoTestCases]
	-- Add the parameters for the stored procedure here
	@PCBTypeid int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

                SELECT    *    
				FROM            [tbl_TestCases]
				WHERE (IsActive='Y') AND (PCBTypeID=@PCBTypeid) AND (TestType='Auto')
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDetailsBySerialNoWise]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <13-12-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetDetailsBySerialNoWise]
	
	@SerialNo nvarchar(50),
	@PcbTypeID int,
	@CreatedBy int,
	@FromDate nvarchar(20),
	@Todate nvarchar(20)
	--, @testcaseid int
AS

IF(@CreatedBy!=0)
BEGIN
		
SELECT tbl_Result.ResultId,tbl_Result.TestCaseID, tbl_Result.SerialNo, tbl_Result.PCBType, tbl_Result.Status, tbl_Result.Date, tbl_TestCases.TestCaseName, tbl_User.Name,tbl_Result.Comment
FROM     tbl_Result INNER JOIN
         tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID 	 INNER JOIN
                         tbl_User ON tbl_Result.UserID = tbl_User.UserID
WHERE (tbl_Result.IsActive='Y') AND (tbl_Result.SerialNo=@SerialNo) AND (tbl_Result.PCBTypeID=@PcbTypeID) AND (tbl_Result.CreatedBy=@CreatedBy)
AND
(CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
order by tbl_Result.ResultId desc

END

--///// Admin Panel //////--

IF(@CreatedBy=0)
BEGIN	
		
SELECT tbl_Result.ResultId,tbl_Result.TestCaseID, tbl_Result.SerialNo, tbl_Result.PCBType, tbl_Result.Status, tbl_Result.Date, tbl_TestCases.TestCaseName, tbl_User.Name,tbl_Result.Comment
FROM     tbl_Result INNER JOIN
         tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID INNER JOIN
                         tbl_User ON tbl_Result.UserID = tbl_User.UserID
WHERE (tbl_Result.IsActive='Y') AND (tbl_Result.SerialNo=@SerialNo) AND (tbl_Result.PCBTypeID=@PcbTypeID)
AND
(CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
order by tbl_Result.ResultId desc
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetErrorCode]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetErrorCode]
	-- Add the parameters for the stored procedure here
	@ErrorCode varchar(10),
	@TestCaseId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT        ErrorCodeId, ErrorCode, ErrorMessage, TestCaseId
	FROM            tbl_ErrorCode
	WHERE ErrorCode=@ErrorCode and (TestCaseId=@TestCaseId OR TestCaseId=0)

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetKeyboardStart]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <13-12-19>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetKeyboardStart]
	-- Add the parameters for the stored procedure here
	@PcbtypeID int,
	@Handler nvarchar(50)
AS

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF(@Handler='Keypad_Start')
BEGIN
SELECT        TestCaseID, TestCaseName, PCBTypeID, PCBType, TestType, FrameToSend, PassFrame, FailFrame, ResponseFrame, CreatedBy, CreatedDate,SendFrame
FROM            tbl_TestCases
WHERE (IsActive='Y') AND (PCBTypeID=@PcbtypeID)   AND (TestType='SplHybrid') --AND (TestCaseName='Keyboard Start')
  
END

IF(@Handler='Keypad_Stop')
BEGIN
SELECT        TestCaseID, TestCaseName, PCBTypeID, PCBType, TestType, FrameToSend, PassFrame, FailFrame, ResponseFrame, CreatedBy, CreatedDate,SendFrame
FROM            tbl_TestCases
WHERE (IsActive='Y') AND (PCBTypeID=@PcbtypeID) AND (TestCaseName='Capsense button Stop')
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetLoginDetails]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetLoginDetails] 
	-- Add the parameters for the stored procedure here
	@username nvarchar(50),
	@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT        UserID, Name, Address1, Address2, City, State, Country, Pincode, Email,
                 Mobile1, Mobile2, UserName, Password, IsActive
   FROM            tbl_User
    WHERE (IsActive='Y') AND (UserName=@username COLLATE SQL_Latin1_General_CP1_CS_AS) AND (Password=@Password COLLATE SQL_Latin1_General_CP1_CS_AS)
   --WHERE (IsActive='Y') AND (UserName=@username) AND (Password=@Password)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetManualTestCases]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <14/11/18>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetManualTestCases]
	-- Add the parameters for the stored procedure here
		@PCBTypeid int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	            SELECT    *    
				FROM            [tbl_TestCases]
				WHERE (IsActive='Y') AND (PCBTypeID=@PCBTypeid) AND (TestType='Manual')

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPCBStatus]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Bhagyashri Patil>
-- Create date: <21-01-2019>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetPCBStatus]
	-- Add the parameters for the stored procedure here
		--@FromDate nvarchar(20),
		--@Todate nvarchar(20),
		--@CreatedBy int
		@PCBTypeId int,
		@SerialNo nvarchar(20)
AS
--IF(@CreatedBy!=0)
BEGIN

CREATE TABLE #TEMP 
(SerialNumber VARCHAR(50),
PcbTypeID INT,
ResultId int,
Status Varchar(50) ,
testcaseid int 
)

insert into #TEMP
SELECT SerialNo ,PcbTypeID,ResultID, status ,testcaseid
FROM tbl_Result
WHERE ResultID IN (
    SELECT MAX(ResultID)
    FROM  tbl_Result
 
    GROUP BY testcaseid,SerialNo
) AND tbl_Result.IsActive='Y' 
AND tbl_Result.PCBTypeID=@PCBTypeId
AND tbl_Result.SerialNo=@SerialNo
select   SerialNumber ,PcbTypeID ,'Pass' As Status ,Max(ResultID) as ResultId  from #TEMP   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  =0

union
select   SerialNumber ,PcbTypeID ,'Fail' As Status ,Max(ResultID) as ResultId  from #TEMP   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  >0

drop table #TEMP 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPCBTestCases]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <3/12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetPCBTestCases]
	-- Add the parameters for the stored procedure here
	@PCBTypeid int,
	@TestType varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

      SELECT    *    
				FROM  [tbl_TestCases]
				WHERE (IsActive='Y') AND (PCBTypeID=@PCBTypeid) AND (TestType=@TestType)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPCBType]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetPCBType]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

        SELECT        PCBTypeID, PCBType, IsActive
		FROM            tbl_PCBType  
		WHERE IsActive='Y'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetRepeatTest]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetRepeatTest]
	-- Add the parameters for the stored procedure here
	@Teststatus nvarchar(20),
	@Createdby int,
	@Fromdate nvarchar(20),
	@Todate nvarchar(20)
AS
	SET NOCOUNT ON;

IF(@Teststatus='All' AND @Createdby!=0)
BEGIN
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'#temp')
DROP table #temp
--ELSE
 CREATE TABLE #temp
(
SerialNo nvarchar(50),
TestCaseID int,
TestCaseIDCount int,

yesno varchar(50)
)
 
INSERT INTO #temp 
     
SELECT SerialNo,TestCaseID,count(TestCaseID) as TestCount,
CASE    --AS [RepeatTest] ,
  --WHEN 0 THEN 'No'  
  WHEN COUNT(SerialNo)=1 THEN 'No'
  ELSE
    CASE when COUNT(TestCaseID)>1    
    then
  'Yes'
  
  end
END YesNo
FROM tbl_Result Where (IsActive='Y') AND (CreatedBy=@Createdby)  --and SerialNo='125'
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
GROUP BY SerialNo,TestCaseID ORDER BY SerialNo 

SELECT DISTINCT * FROM
(
    select 
        Row_number() over (partition by tbl_Result.Pcbtypeid,tbl_Result.serialno order by tbl_Result.ResultId desc) RNUM, 
        #temp.SerialNo,tbl_Result.ResultId,  #temp.yesno,tbl_Result.TestCaseID
   from #temp join tbl_Result on #temp.SerialNo=tbl_Result.SerialNo   
    INNER JOIN ( 
          select #temp.SerialNo, MAX(#temp.TestCaseIDCount) as TestCaseIDCount 
          from #temp 
          group by #temp.SerialNo
     ) as NewCount 
    ON #temp.SerialNo = NewCount.SerialNo 
       AND #temp.TestCaseIDCount = NewCount.TestCaseIDCount
    where    (CreatedBy=@Createdby)  AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
)x   where RNUM=1  ORDER BY SerialNo
--drop table #temp
END

IF(@Teststatus!='All'  AND @Createdby!=0)
BEGIN

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'#temp1')
DROP table #temp1
--ELSE
 CREATE TABLE #temp1
(
SerialNo nvarchar(50),
TestCaseID int,
TestCaseIDCount int,

yesno varchar(50)
)
 
INSERT INTO #temp1 
     
SELECT SerialNo,TestCaseID,count(TestCaseID) as TestCount,
CASE    --AS [RepeatTest] ,
  --WHEN 0 THEN 'No'  
  WHEN COUNT(SerialNo)=1 THEN 'No'
  ELSE
    CASE when COUNT(TestCaseID)>1    
    then
  'Yes'
  
  end
END YesNo
FROM tbl_Result Where (IsActive='Y') AND (CreatedBy=@Createdby) --and SerialNo='125'
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
GROUP BY SerialNo,TestCaseID ORDER BY SerialNo 

SELECT DISTINCT * FROM
(
    select 
        Row_number() over (partition by tbl_Result.Pcbtypeid,tbl_Result.serialno order by tbl_Result.ResultId desc) RNUM, 
        #temp1.SerialNo,tbl_Result.ResultId,  #temp1.yesno,tbl_Result.TestCaseID
   from #temp1 join tbl_Result on #temp1.SerialNo=tbl_Result.SerialNo   
    INNER JOIN ( 
          select #temp1.SerialNo, MAX(#temp1.TestCaseIDCount) as TestCaseIDCount 
          from #temp1
          group by #temp1.SerialNo
     ) as NewCount 
    ON #temp1.SerialNo = NewCount.SerialNo 
       AND #temp1.TestCaseIDCount = NewCount.TestCaseIDCount
    where    (CreatedBy=@Createdby)  and yesno=@Teststatus
	AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
)x   where RNUM=1  ORDER BY SerialNo
--drop table #temp1
END


--/////// For Admin Panel ////////-----


IF(@Teststatus='All' AND @Createdby=0)
BEGIN
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'#temp2')
DROP table #temp2
--ELSE
 CREATE TABLE #temp2
(
SerialNo nvarchar(50),
TestCaseID int,
TestCaseIDCount int,

yesno varchar(50)
)
 
INSERT INTO #temp2 
     
SELECT SerialNo,TestCaseID,count(TestCaseID) as TestCount,
CASE    --AS [RepeatTest] ,
  --WHEN 0 THEN 'No'  
  WHEN COUNT(SerialNo)=1 THEN 'No'
  ELSE
    CASE when COUNT(TestCaseID)>1    
    then
  'Yes'
  
  end
END YesNo
FROM tbl_Result Where (IsActive='Y') 
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
GROUP BY SerialNo,TestCaseID ORDER BY SerialNo 

SELECT DISTINCT * FROM
(
    select 
        Row_number() over (partition by tbl_Result.Pcbtypeid,tbl_Result.serialno order by tbl_Result.ResultId desc) RNUM, 
        #temp2.SerialNo,tbl_Result.ResultId,  #temp2.yesno,tbl_Result.TestCaseID
   from #temp2 join tbl_Result on #temp2.SerialNo=tbl_Result.SerialNo   
    INNER JOIN ( 
          select #temp2.SerialNo, MAX(#temp2.TestCaseIDCount) as TestCaseIDCount 
          from #temp2 
          group by #temp2.SerialNo
     ) as NewCount 
    ON #temp2.SerialNo = NewCount.SerialNo 
       AND #temp2.TestCaseIDCount = NewCount.TestCaseIDCount
    where     (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
)x   where RNUM=1  ORDER BY SerialNo
--drop table #temp2
END

IF(@Teststatus!='All'  AND @Createdby=0)
BEGIN

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'#temp3')
DROP table #temp3
--ELSE
 CREATE TABLE #temp3
(
SerialNo nvarchar(50),
TestCaseID int,
TestCaseIDCount int,

yesno varchar(50)
)
 
INSERT INTO #temp3 
     
SELECT SerialNo,TestCaseID,count(TestCaseID) as TestCount,
CASE    --AS [RepeatTest] ,
  --WHEN 0 THEN 'No'  
  WHEN COUNT(SerialNo)=1 THEN 'No'
  ELSE
    CASE when COUNT(TestCaseID)>1    
    then
  'Yes'
  
  end
END YesNo
FROM tbl_Result Where (IsActive='Y')
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
GROUP BY SerialNo,TestCaseID ORDER BY SerialNo 

SELECT DISTINCT * FROM
(
    select 
        Row_number() over (partition by tbl_Result.Pcbtypeid,tbl_Result.serialno order by tbl_Result.ResultId desc) RNUM, 
        #temp3.SerialNo,tbl_Result.ResultId,  #temp3.yesno,tbl_Result.TestCaseID
   from #temp3 join tbl_Result on #temp3.SerialNo=tbl_Result.SerialNo   
    INNER JOIN ( 
          select #temp3.SerialNo, MAX(#temp3.TestCaseIDCount) as TestCaseIDCount 
          from #temp3
          group by #temp3.SerialNo
     ) as NewCount 
    ON #temp3.SerialNo = NewCount.SerialNo 
       AND #temp3.TestCaseIDCount = NewCount.TestCaseIDCount
    where   yesno=@Teststatus
	AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
)x   where RNUM=1  ORDER BY SerialNo
--drop table #temp3
END























--   SELECT ResultId,
--CASE COUNT(DISTINCT SerialNo)   --AS [RepeatTest] ,
--  WHEN 0 THEN 'No'
--  WHEN 1 THEN 'No'
--  ELSE 'Yes'
--END
--FROM tbl_Result Where IsActive='Y'
--GROUP BY ResultId
GO
/****** Object:  StoredProcedure [dbo].[usp_GetReport]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetReport] 
	-- Add the parameters for the stored procedure here
	@PCBTypeId int,
	@Status nvarchar(10),
	@FromDate nvarchar(20),
	@Todate nvarchar(20),
	@CreatedBy int
	
AS
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF(@PCBTypeId=0 AND @Status='All' AND @CreatedBy!=0)
BEGIN	

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID 
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
WHERE         (tbl_Result.IsActive='Y') AND  (tbl_Result.CreatedBy=@CreatedBy) AND
(CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))

END

IF(@PCBTypeId!=0 AND @Status!='All' AND @CreatedBy!=0)
BEGIN

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
	WHERE     
    (tbl_Result.PCBTypeID=@PCBTypeId) AND (tbl_Result.Status=@Status) AND (tbl_Result.CreatedBy=@CreatedBy)
    AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))

END

IF(@PCBTypeId!=0 AND @Status='All' AND @CreatedBy!=0)
BEGIN	

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
    WHERE(tbl_Result.IsActive='Y') AND (CONVERT(date,tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105)) 
    AND (tbl_Result.PCBTypeID=@PCBTypeId)  AND (tbl_Result.CreatedBy=@CreatedBy)
	
END

IF(@PCBTypeId=0 AND @Status!='All' AND @CreatedBy!=0)
BEGIN

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
	WHERE(tbl_Result.IsActive='Y') AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105)) -- (CONVERT(varchar, CreatedDate, 105)>=@FromDate) AND (CONVERT(varchar, CreatedDate, 105)<=@Todate)
AND (tbl_Result.Status=@Status) AND (tbl_Result.CreatedBy=@CreatedBy)

END

--///////////////For Admin Panel//////////////////////

IF(@PCBTypeId=0 AND @Status='All' AND @CreatedBy=0)
BEGIN	
SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID 
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
WHERE         (tbl_Result.IsActive='Y') AND 
(CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))

END

IF(@PCBTypeId!=0 AND @Status!='All' AND @CreatedBy=0)
BEGIN

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
	WHERE     
    (tbl_Result.PCBTypeID=@PCBTypeId) AND (tbl_Result.Status=@Status)
    AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))

END

IF(@PCBTypeId!=0 AND @Status='All' AND @CreatedBy=0)
BEGIN	

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
    WHERE(tbl_Result.IsActive='Y') AND (CONVERT(date,tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105)) 
    AND (tbl_Result.PCBTypeID=@PCBTypeId)  

END

IF(@PCBTypeId=0 AND @Status!='All' AND @CreatedBy=0)
BEGIN

SELECT  ResultId,tbl_Result.PCBType,tbl_Result.TestCaseID,tbl_TestCases.TestCaseName,Date,tbl_User.Name From tbl_Result
INNER JOIN
    tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
	 INNER JOIN  tbl_User ON tbl_Result.UserID = tbl_User.UserID
	WHERE(tbl_Result.IsActive='Y') AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105)) -- (CONVERT(varchar, CreatedDate, 105)>=@FromDate) AND (CONVERT(varchar, CreatedDate, 105)<=@Todate)
AND (tbl_Result.Status=@Status) 

END



--exec usp_GetReport 0,'Pass','05-10-2017','06-10-2017'
--select createddate From tbl_result
--WHERE (CONVERT(date, CreatedDate, 105)>=convert(date,'05-09-2017',105)) And (CONVERT(date, CreatedDate, 105)<=convert(date,'26-09-2017',105))
GO
/****** Object:  StoredProcedure [dbo].[usp_GetReportTestStatus]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetReportTestStatus]
	-- Add the parameters for the stored procedure here	
	@FromDate nvarchar(20),
	@Todate nvarchar(20),
	@CreatedBy int
AS

IF(@CreatedBy!=0)
BEGIN

CREATE TABLE #TEMP 
(SerialNumber VARCHAR(50),
PcbTypeID INT,
ResultId int,
Status Varchar(50) ,
testcaseid int 
)

insert into #TEMP
SELECT SerialNo ,PcbTypeID,ResultID, status ,testcaseid
FROM tbl_Result
WHERE ResultID IN (
    SELECT MAX(ResultID)
    FROM  tbl_Result
 
    GROUP BY testcaseid,SerialNo
) AND tbl_Result.IsActive='Y' AND  tbl_Result.CreatedBy=@CreatedBy
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
 --and SerialNo='6532'
--select * from #TEMP 
-- select * from tbl_Result where SerialNo='1'
select   SerialNumber ,PcbTypeID ,'Pass' As Status ,Max(ResultID) as ResultId  from #TEMP   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  =0

union
select   SerialNumber ,PcbTypeID ,'Fail' As Status ,Max(ResultID) as ResultId  from #TEMP   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  >0

drop table #TEMP 

END

--/////// For Admin Panel /////////--

IF(@CreatedBy=0)
BEGIN

CREATE TABLE #TEMP1 
(SerialNumber VARCHAR(50),
PcbTypeID INT,
ResultId int,
Status Varchar(50) ,
testcaseid int 
)

insert into #TEMP1
SELECT SerialNo ,PcbTypeID,ResultID, status ,testcaseid
FROM tbl_Result
WHERE ResultID IN (
    SELECT MAX(ResultID)
    FROM  tbl_Result
 
    GROUP BY testcaseid,SerialNo
) AND tbl_Result.IsActive='Y' 
AND (CONVERT(date, tbl_Result.CreatedDate, 105)>=convert(date,@FromDate,105)) And (CONVERT(date, tbl_Result.CreatedDate, 105)<=convert(date,@Todate,105))
select   SerialNumber ,PcbTypeID ,'Pass' As Status ,Max(ResultID) as ResultId  from #TEMP1   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  =0

union
select   SerialNumber ,PcbTypeID ,'Fail' As Status ,Max(ResultID) as ResultId  from #TEMP1   group by  SerialNumber ,PcbTypeID  having   SUM(CASE WHEN status = 'Fail' THEN 1 ELSE 0 END)  >0

drop table #TEMP1 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetTestCaseStatus]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetTestCaseStatus] 
	-- Add the parameters for the stored procedure here
	@PCBTypeId int,
	@UserID int,
	@TestCaseId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		
			  SELECT *
			 FROM            tbl_Result
			
			 WHERE (IsActive='Y') AND (PCBTypeID=@PCBTypeId) AND (UserID=@UserID) AND (TestCaseID=@TestCaseId)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserRoleByUserId]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetUserRoleByUserId] 
	-- Add the parameters for the stored procedure here
	@userid int
AS
BEGIN

	SET NOCOUNT ON;
SELECT        UserID, Name, RoleID, Role
FROM            tbl_User WHERE (UserID=@userid) and (IsActive='Y')
   
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RptGetAllPCBTypeWise]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_RptGetAllPCBTypeWise]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

      SELECT        tbl_TestCases.TestCaseID, tbl_TestCases.TestCaseName, tbl_TestCases.PCBTypeID, tbl_TestCases.PCBType, tbl_TestCases.TestType, 
                         tbl_TestCases.FrameToSend, tbl_Result.SerialNo, tbl_Result.Status, tbl_Result.ResponseFrame, tbl_Result.CreatedBy, tbl_Result.CreatedDate,tbl_Result.Date, tbl_Result.ModifiedBy, tbl_Result.ModifiedDate,tbl_Result.ResultId
FROM            tbl_Result INNER JOIN
                         tbl_TestCases ON tbl_Result.TestCaseID = tbl_TestCases.TestCaseID
WHERE(tbl_Result.IsActive='Y') 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_SaveAutoTestCases]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_SaveAutoTestCases] 
	-- Add the parameters for the stored procedure here
	
	@UserID int,
	@SerialNo nvarchar(50),
	@PCBTypeID int,
	@PCBType nvarchar(50),
	@TestCaseID int,
	@Status nvarchar(50),
	@FrameToSend nvarchar(Max),
	@ResponseFrame nvarchar(Max),
	@CreatedBy int,
	@CurrentDate nvarchar(50)
	--@CreateDate DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 
	INSERT INTO tbl_Result
            (UserID, SerialNo, PCBTypeID, PCBType, TestCaseID, Status,FrameToSend, ResponseFrame,TestType,
			CreatedBy, CreatedDate,Date, IsActive)
    VALUES      (@UserID,@SerialNo,@PCBTypeID,@PCBType,@TestCaseID,@Status,@FrameToSend,@ResponseFrame,'Auto',@CreatedBy,GETDATE(),@CurrentDate,'Y')
END
GO
/****** Object:  StoredProcedure [dbo].[usp_SaveKeyPadTest]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11\2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_SaveKeyPadTest] 
	-- Add the parameters for the stored procedure here
	@UserID int,
	@SerialNo nvarchar(50),
	@PCBTypeID int,
	@PCBType nvarchar(50),
	@TestCaseID int,
	@Status nvarchar(50),
	@FrameToSend nvarchar(Max),
	@ResponseFrame nvarchar(Max),
	@CreatedBy int,
	@CurrentDate nvarchar(50)
	--, @Comment nvarchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  INSERT INTO tbl_Result
            (UserID, SerialNo, PCBTypeID, PCBType, TestCaseID, Status,FrameToSend, ResponseFrame,TestType,
			CreatedBy, CreatedDate,Date, IsActive)
    VALUES      (@UserID,@SerialNo,@PCBTypeID,@PCBType,@TestCaseID,@Status,@FrameToSend,@ResponseFrame,'SplHybrid',@CreatedBy,GETDATE(),@CurrentDate,'Y')

END
GO
/****** Object:  StoredProcedure [dbo].[usp_SaveManualELockCase1]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <11/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_SaveManualELockCase1]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@SerialNo nvarchar(50),
	@PCBTypeID int,
	@PCBType nvarchar(50),
	@TestCaseID int,
	@Status nvarchar(50),
	@FrameToSend nvarchar(Max),
	@ResponseFrame nvarchar(Max),
	@CreatedBy int,
	@CurrentDate nvarchar(50)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO tbl_Result
            (UserID, SerialNo, PCBTypeID, PCBType, TestCaseID, Status,FrameToSend, ResponseFrame,TestType,
			CreatedBy, CreatedDate,Date, IsActive)
    VALUES      (@UserID,@SerialNo,@PCBTypeID,@PCBType,@TestCaseID,@Status,@FrameToSend,@ResponseFrame,'Manual',@CreatedBy,GETDATE(),@CurrentDate,'Y')
   
   
END
GO
/****** Object:  StoredProcedure [dbo].[usp_SaveTestCaseResult]    Script Date: 04-03-2024 14:51:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bhagyashri Patil>
-- Create date: <12\2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_SaveTestCaseResult]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@SerialNo nvarchar(50),
	@PCBTypeID int,
	@PCBType nvarchar(50),
	@TestCaseID int,
	@TestType varchar(10),
	@Status nvarchar(50),
	@FrameToSend nvarchar(Max),
	@ResponseFrame nvarchar(Max),
	@CreatedBy int,
	@CurrentDate nvarchar(50),
	@OutputValue nvarchar(70)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO tbl_Result
            (UserID, SerialNo, PCBTypeID, PCBType, TestCaseID, Status,FrameToSend, ResponseFrame,TestType,
			CreatedBy, CreatedDate,Date, IsActive,Comment)
    VALUES      (@UserID,@SerialNo,@PCBTypeID,@PCBType,@TestCaseID,@Status,@FrameToSend,@ResponseFrame,@TestType,@CreatedBy,GETDATE(),@CurrentDate,'Y',@OutputValue)
END
GO
USE [master]
GO
ALTER DATABASE [ESD_Testjig_DB] SET  READ_WRITE 
GO
