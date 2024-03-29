USE [master]
GO
/****** Object:  Database [Notes-MarketPlace]    Script Date: 26-03-2021 17:38:55 ******/
CREATE DATABASE [Notes-MarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Notes-MarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.JAYMINMSSQL\MSSQL\DATA\Notes-MarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Notes-MarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.JAYMINMSSQL\MSSQL\DATA\Notes-MarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Notes-MarketPlace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Notes-MarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Notes-MarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Notes-MarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Notes-MarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Notes-MarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Notes-MarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [Notes-MarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [Notes-MarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Notes-MarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Notes-MarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Notes-MarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Notes-MarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Notes-MarketPlace', N'ON'
GO
ALTER DATABASE [Notes-MarketPlace] SET QUERY_STORE = OFF
GO
USE [Notes-MarketPlace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CoutryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModofiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries_1] PRIMARY KEY CLUSTERED 
(
	[CoutryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[DownloadsID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[DownloadsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ReferenceID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[DataValue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[ReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[SellerNotesID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberOfPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotePreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotes] PRIMARY KEY CLUSTERED 
(
	[SellerNotesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachments]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachments](
	[AttachmentsID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[AttachmentSize] [bigint] NULL,
 CONSTRAINT [PK_SellerNotesAttachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedBy] [int] NOT NULL,
	[AgaintsDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[ReviewsID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgaintsDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[ReviewsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[SystemID] [int] IDENTITY(1,1) NOT NULL,
	[keys] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModofiedDate] [datetime] NULL,
	[ModofiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[SystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserProfileID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[PhoneNumberCountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Desciption] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26-03-2021 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[VerificationCode] [varchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([CoutryID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModofiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'INDIA', N'91', CAST(N'2021-02-18T15:21:55.330' AS DateTime), 25, CAST(N'2021-02-18T15:21:55.000' AS DateTime), 25, 1)
INSERT [dbo].[Countries] ([CoutryID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModofiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'AUS', N'50', CAST(N'2021-02-18T15:22:12.147' AS DateTime), 28, CAST(N'2021-02-18T15:22:12.000' AS DateTime), 28, 1)
INSERT [dbo].[Countries] ([CoutryID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModofiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'USA', N'20', CAST(N'2021-03-13T22:43:54.933' AS DateTime), 25, CAST(N'2021-03-13T22:43:54.933' AS DateTime), 25, 1)
INSERT [dbo].[Countries] ([CoutryID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModofiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Canada', N'13', CAST(N'2021-03-13T22:44:09.280' AS DateTime), 25, CAST(N'2021-03-13T22:44:09.280' AS DateTime), 25, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1133, 92, 48, 48, 1, N'/Members/48/92/Attachements/Attachement1_24032021.pdf;', 0, NULL, 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-24T18:15:23.940' AS DateTime), 48, CAST(N'2021-03-24T18:15:23.940' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1134, 96, 49, 48, 1, N'/Members/49/96/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T19:44:10.227' AS DateTime), 1, CAST(700 AS Decimal(18, 0)), N'.Net by GS', N'IT', CAST(N'2021-03-24T18:30:23.290' AS DateTime), 48, CAST(N'2021-03-25T19:44:10.227' AS DateTime), 49, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1135, 97, 49, 48, 0, NULL, 0, CAST(N'2021-03-24T18:03:27.400' AS DateTime), 1, CAST(800 AS Decimal(18, 0)), N'Paid by GS', N'MBA', CAST(N'2021-03-24T18:03:27.400' AS DateTime), 48, CAST(N'2021-03-24T18:03:27.400' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1136, 93, 48, 49, 1, N'/Members/48/93/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T18:39:07.417' AS DateTime), 0, NULL, N'Free by S3', N'CA', CAST(N'2021-03-24T18:04:02.660' AS DateTime), 49, CAST(N'2021-03-25T18:39:07.417' AS DateTime), 49, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1137, 96, 49, 49, 0, NULL, 0, CAST(N'2021-03-24T18:04:45.240' AS DateTime), 1, CAST(700 AS Decimal(18, 0)), N'.Net by GS', N'IT', CAST(N'2021-03-24T18:04:45.240' AS DateTime), 49, CAST(N'2021-03-24T18:04:45.240' AS DateTime), 49, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1138, 92, 48, 49, 1, N'/Members/48/92/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T18:39:01.980' AS DateTime), 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-24T18:15:12.083' AS DateTime), 49, CAST(N'2021-03-25T18:39:01.980' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1139, 97, 49, 49, 1, N'/Members/49/97/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T18:38:56.337' AS DateTime), 1, CAST(800 AS Decimal(18, 0)), N'Paid by GS', N'MBA', CAST(N'2021-03-24T18:30:10.703' AS DateTime), 49, CAST(N'2021-03-25T18:38:56.337' AS DateTime), 49, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1140, 93, 48, 48, 1, N'/Members/48/93/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T09:51:06.893' AS DateTime), 0, NULL, N'Free by S3', N'CA', CAST(N'2021-03-25T09:51:06.893' AS DateTime), 48, CAST(N'2021-03-25T09:51:06.893' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1141, 92, 48, 48, 1, N'/Members/48/92/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T11:56:12.460' AS DateTime), 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-25T11:55:06.950' AS DateTime), 48, CAST(N'2021-03-25T11:56:12.460' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1142, 92, 48, 48, 1, N'/Members/48/92/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T12:06:45.013' AS DateTime), 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-25T12:00:34.787' AS DateTime), 48, CAST(N'2021-03-25T12:06:45.013' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1143, 93, 48, 48, 1, N'/Members/48/93/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T12:01:28.923' AS DateTime), 0, NULL, N'Free by S3', N'CA', CAST(N'2021-03-25T12:01:28.923' AS DateTime), 48, CAST(N'2021-03-25T12:01:28.923' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1144, 92, 48, 48, 0, NULL, 0, NULL, 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-26T15:38:34.847' AS DateTime), 48, CAST(N'2021-03-26T15:38:34.847' AS DateTime), 48, 1)
INSERT [dbo].[Downloads] ([DownloadsID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1145, 92, 48, 49, 0, NULL, 0, NULL, 1, CAST(400 AS Decimal(18, 0)), N'python by s3', N'ComputerScience', CAST(N'2021-03-26T15:40:38.080' AS DateTime), 49, CAST(N'2021-03-26T15:40:38.080' AS DateTime), 49, 1)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'ComputerScience', N'this Computer Science book', CAST(N'2021-02-18T15:17:35.337' AS DateTime), 25, CAST(N'2021-02-18T15:17:35.337' AS DateTime), 25, 1)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'IT', N'this IT book', CAST(N'2021-02-18T15:18:20.800' AS DateTime), 25, CAST(N'2021-02-18T15:18:20.800' AS DateTime), 25, 1)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'CA', N'this is CA', CAST(N'2021-03-13T18:33:19.543' AS DateTime), 28, CAST(N'2021-03-13T18:58:32.590' AS DateTime), 28, 1)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'MBA', N'this is MBA book', CAST(N'2021-03-13T21:06:50.870' AS DateTime), 25, CAST(N'2021-03-13T21:06:50.870' AS DateTime), 25, 1)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'STORY BOOK', N'this is Story Book...', CAST(N'2021-03-13T21:07:41.770' AS DateTime), 25, CAST(N'2021-03-13T21:08:12.493' AS DateTime), 25, 1)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Account-2', N'this is ACcount Book', CAST(N'2021-03-13T21:08:35.827' AS DateTime), 25, CAST(N'2021-03-13T21:57:30.580' AS DateTime), 25, 0)
INSERT [dbo].[NoteCategories] ([CategoryID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'dummy', N'dummy add by super admin', CAST(N'2021-03-14T00:07:19.080' AS DateTime), 28, CAST(N'2021-03-14T00:07:19.080' AS DateTime), 28, 1)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([TypeID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Handwritten notes,', N'this Handwritten notes,', CAST(N'2021-02-18T15:19:49.013' AS DateTime), 28, CAST(N'2021-02-18T15:19:49.013' AS DateTime), 28, 1)
INSERT [dbo].[NoteTypes] ([TypeID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'UniverSityNote', N'this University Note book', CAST(N'2021-02-18T15:20:34.973' AS DateTime), 28, CAST(N'2021-03-13T21:42:47.680' AS DateTime), 28, 1)
INSERT [dbo].[NoteTypes] ([TypeID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Self Written', N'this self written type', CAST(N'2021-03-13T21:45:01.963' AS DateTime), 25, CAST(N'2021-03-13T21:45:01.963' AS DateTime), 25, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Male', N'M', N'Gender', CAST(N'2021-02-18T15:23:49.590' AS DateTime), NULL, CAST(N'2021-02-18T15:23:49.590' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Female', N'Fe', N'Gender', CAST(N'2021-02-18T15:24:12.877' AS DateTime), NULL, CAST(N'2021-02-18T15:24:12.877' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Unknown', N'U', N'Gender', CAST(N'2021-02-18T15:24:50.320' AS DateTime), NULL, CAST(N'2021-02-18T15:24:50.320' AS DateTime), NULL, 0)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Paid', N'P', N'Selling Mode', CAST(N'2021-02-18T15:25:11.453' AS DateTime), NULL, CAST(N'2021-02-18T15:25:11.453' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Draft', N'Draft', N'Note Status', CAST(N'2021-02-18T15:26:08.070' AS DateTime), NULL, CAST(N'2021-02-18T15:26:08.070' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Submitted For Review', N'Submitted For Review', N'Note Status', CAST(N'2021-02-18T15:26:39.517' AS DateTime), NULL, CAST(N'2021-02-18T15:26:39.517' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'In Review', N'In Review', N'Note Status', CAST(N'2021-02-18T15:27:01.120' AS DateTime), NULL, CAST(N'2021-02-18T15:27:01.120' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Published', N'Approved', N'Note Status', CAST(N'2021-02-18T15:27:23.647' AS DateTime), NULL, CAST(N'2021-02-18T15:27:23.647' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Rejected', N'Rejected', N'Note Status', CAST(N'2021-02-18T15:27:48.277' AS DateTime), NULL, CAST(N'2021-02-18T15:27:48.277' AS DateTime), NULL, 1)
INSERT [dbo].[ReferenceData] ([ReferenceID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Removed', N'Removed', N'Note Status', CAST(N'2021-02-18T15:28:17.310' AS DateTime), NULL, CAST(N'2021-02-18T15:28:17.310' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (92, 48, 9, 47, NULL, CAST(N'2000-08-25T17:57:27.000' AS DateTime), N'python by s3', 1, N'/Members/48/92/DP_24032021.jpg', 2, 500, N'it is very nice book..', N'Gec bhavnagar', 4, N'python readning', N'26', N'jayesh chavda', 1, CAST(400 AS Decimal(18, 0)), N'/Members/48/92/Preview_24032021.pdf', CAST(N'2021-03-24T17:33:21.997' AS DateTime), 48, CAST(N'2021-03-24T17:57:27.467' AS DateTime), 47, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (93, 48, 9, 47, NULL, CAST(N'2018-02-09T17:57:34.000' AS DateTime), N'Free by S3', 6, N'/Members/48/93/DP_24032021.jpg', 4, 250, N'very Beautiful book...', N'Ahmedabad', 5, N'Enjoy', N'12', N'Nitish patel', 0, NULL, NULL, CAST(N'2021-03-24T17:37:23.283' AS DateTime), 48, CAST(N'2021-03-24T17:57:34.823' AS DateTime), 47, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (94, 48, 10, 47, N'Rejected by Mehul Parmar admin', NULL, N'Paid by S3', 9, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 2, 200, N'oejdjedjenwdcnd', N'fewf', 1, N'.Net Readning', N'26', N'Sweta mam', 1, CAST(350 AS Decimal(18, 0)), N'/Members/48/94/Preview_24032021.pdf', CAST(N'2021-03-24T17:40:57.290' AS DateTime), 48, CAST(N'2021-03-24T17:56:55.227' AS DateTime), 47, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (95, 48, 7, NULL, NULL, NULL, N'S3 FRee', 11, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1, 150, N'ojjjhbhvvgiooijoi', NULL, 5, NULL, NULL, NULL, 0, NULL, NULL, CAST(N'2021-03-24T17:41:53.793' AS DateTime), 48, CAST(N'2021-03-26T17:27:06.913' AS DateTime), 48, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (96, 49, 9, 46, NULL, CAST(N'2021-03-24T17:54:55.867' AS DateTime), N'.Net by GS', 2, N'/Members/49/96/DP_24032021.jpg', 1, NULL, N'looking good book...!!!', NULL, 1, NULL, NULL, NULL, 1, CAST(700 AS Decimal(18, 0)), N'/Members/49/96/Preview_24032021.pdf', CAST(N'2021-03-24T17:44:08.183' AS DateTime), 49, CAST(N'2021-03-24T17:54:55.867' AS DateTime), 46, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (97, 49, 9, 47, NULL, CAST(N'1995-01-01T17:57:39.000' AS DateTime), N'Paid by GS', 8, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1, 200, N'byggygggg', N'Ahmedabad', 1, N'python ', N'26', N'karan sir', 1, CAST(800 AS Decimal(18, 0)), N'/Members/49/97/Preview_24032021.pdf', CAST(N'2021-03-24T17:48:09.777' AS DateTime), 49, CAST(N'2021-03-24T17:57:39.333' AS DateTime), 47, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (98, 49, 10, 46, N'Rejected by jamu makwana', NULL, N'Free by GS', 9, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1, NULL, N'uggrgfirfigrf', NULL, 1, NULL, NULL, NULL, 0, NULL, NULL, CAST(N'2021-03-24T17:51:25.487' AS DateTime), 49, CAST(N'2021-03-24T17:55:24.690' AS DateTime), 46, 1)
INSERT [dbo].[SellerNotes] ([SellerNotesID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotePreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (99, 49, 7, NULL, NULL, NULL, N'GS Free', 2, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1, NULL, N'forjrtjgjhptjtbb', NULL, 1, NULL, NULL, NULL, 0, NULL, NULL, CAST(N'2021-03-24T17:52:57.687' AS DateTime), 49, CAST(N'2021-03-26T17:28:32.090' AS DateTime), 49, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttachments] ON 

INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (109, 92, N'Attachement1_24032021.pdf;', N'/Members/48/92/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:33:22.223' AS DateTime), 48, CAST(N'2021-03-24T17:35:11.753' AS DateTime), 48, 1, 359)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (110, 93, N'Attachement1_24032021.pdf;', N'/Members/48/93/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:37:23.300' AS DateTime), 48, CAST(N'2021-03-24T17:39:09.583' AS DateTime), 48, 1, 3774)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (111, 94, N'Attachement1_24032021.pdf;', N'/Members/48/94/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:40:57.457' AS DateTime), 48, CAST(N'2021-03-24T17:42:54.147' AS DateTime), 48, 1, 145)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (112, 95, N'Attachement1_26032021.pdf;', N'/Members/48/95/Attachements/Attachement1_26032021.pdf;', CAST(N'2021-03-24T17:41:53.883' AS DateTime), 48, CAST(N'2021-03-26T17:27:07.633' AS DateTime), 48, 1, 145)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (113, 96, N'Attachement1_24032021.pdf;', N'/Members/49/96/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:44:08.600' AS DateTime), 49, CAST(N'2021-03-24T17:45:47.853' AS DateTime), 49, 1, 145)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (114, 97, N'Attachement1_24032021.pdf;', N'/Members/49/97/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:48:10.160' AS DateTime), 49, CAST(N'2021-03-24T17:50:22.340' AS DateTime), 49, 1, 359)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (115, 98, N'Attachement1_24032021.pdf;', N'/Members/49/98/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T17:51:25.567' AS DateTime), 49, CAST(N'2021-03-24T17:52:14.633' AS DateTime), 49, 1, 145)
INSERT [dbo].[SellerNotesAttachments] ([AttachmentsID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [AttachmentSize]) VALUES (116, 99, N'Attachement1_26032021.pdf;', N'/Members/49/99/Attachements/Attachement1_26032021.pdf;', CAST(N'2021-03-24T17:52:57.967' AS DateTime), 49, CAST(N'2021-03-26T17:28:32.110' AS DateTime), 49, 1, 359)
SET IDENTITY_INSERT [dbo].[SellerNotesAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] ON 

INSERT [dbo].[SellerNotesReportedIssues] ([ReportID], [NoteID], [ReportedBy], [AgaintsDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1023, 96, 48, 1134, N'nuihuhohp', CAST(N'2021-03-26T17:27:23.573' AS DateTime), 48, CAST(N'2021-03-26T17:27:23.573' AS DateTime), 48, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (42, N'SupportEmailID', N'makwanajaymin2033@gmail.com', CAST(N'2021-03-24T16:36:38.640' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.207' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (43, N'SupportEmailIDPassword', N'JAmU#1570@', CAST(N'2021-03-24T16:36:39.527' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.643' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (44, N'SupportPhoneNumber', N'9081594389', CAST(N'2021-03-24T16:36:39.530' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.657' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (45, N'EmailIDs', N'jayminmakwana0409@gmail.com,thegreenskull2020@gmail.com,smartstudent2020system@gmail.com', CAST(N'2021-03-24T16:36:39.533' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.677' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (46, N'FBURL', N'https://www.facebook.com/', CAST(N'2021-03-24T16:36:39.537' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.690' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (47, N'LinkedInURL', N'https://www.LinkedIn.com/', CAST(N'2021-03-24T16:36:39.540' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.707' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (48, N'TwitterURL', N'https://www.twittwe.com/', CAST(N'2021-03-24T16:36:39.547' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.720' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (49, N'DefaultNotePicture', N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', CAST(N'2021-03-24T16:36:39.550' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.733' AS DateTime), 43, 1)
INSERT [dbo].[SystemConfigurations] ([SystemID], [keys], [Value], [CreatedDate], [CreatedBy], [ModofiedDate], [ModofiedBy], [IsActive]) VALUES (50, N'DefaultUserPicture', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', CAST(N'2021-03-24T16:36:39.613' AS DateTime), 43, CAST(N'2021-03-24T17:00:06.760' AS DateTime), 43, 1)
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([UserProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1030, 43, NULL, NULL, N'jayminmakwana15702033@gmail.com', N'91', N'6351662862', N'/Members/43/DP_24032021.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T16:37:28.697' AS DateTime), 43, NULL, NULL, NULL)
INSERT [dbo].[UserProfile] ([UserProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1033, 46, NULL, NULL, N'makwanajaymin@gmail.com', N'91', N'9874561321', N'/Members/46/DP_24032021.png', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T17:01:04.437' AS DateTime), 43, CAST(N'2021-03-24T17:07:54.600' AS DateTime), 46, 1)
INSERT [dbo].[UserProfile] ([UserProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1034, 47, NULL, NULL, N'mehulparmar109@gmail.com', N'91', N'9778454441', N'/Members/47/DP_24032021.png', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T17:03:10.970' AS DateTime), 43, CAST(N'2021-03-24T17:05:49.833' AS DateTime), 47, 1)
INSERT [dbo].[UserProfile] ([UserProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1035, 48, CAST(N'2000-08-25T00:00:00.000' AS DateTime), 2, NULL, N'91', N'9124563412', N'/Members/48/DP_24032021.png', N'panshina', N'Nadi kathe edited', N'Limbdi ', N'Gujarat', N'363423', N'INDIA', N'GTU', N'Gec Bhavnaagar', CAST(N'2021-03-24T17:20:05.143' AS DateTime), 48, NULL, NULL, NULL)
INSERT [dbo].[UserProfile] ([UserProfileID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1036, 49, CAST(N'2000-02-16T00:00:00.000' AS DateTime), 1, NULL, N'91', N'9714211415', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', N'sukhsagar ', N'bhavnagar edited', N'Bhavnagar', N'Gujarat', N'363435', N'AUS', N'Nirma', N'Nirma College', CAST(N'2021-03-24T17:24:01.457' AS DateTime), 49, CAST(N'2021-03-24T18:07:08.700' AS DateTime), 49, 1)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([RoleID], [Name], [Desciption], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Member', N'this is Member', CAST(N'2021-02-17T21:57:02.060' AS DateTime), NULL, CAST(N'2021-02-17T21:57:02.060' AS DateTime), NULL, 1)
INSERT [dbo].[UserRoles] ([RoleID], [Name], [Desciption], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Admin', N'this is Admin', CAST(N'2021-02-17T21:57:30.007' AS DateTime), NULL, CAST(N'2021-02-17T21:57:30.007' AS DateTime), NULL, 1)
INSERT [dbo].[UserRoles] ([RoleID], [Name], [Desciption], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'SuperAdmin', N'this is SuperAdmin', CAST(N'2021-02-17T21:57:45.623' AS DateTime), NULL, CAST(N'2021-02-17T21:57:45.623' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [VerificationCode]) VALUES (43, 3, N'Jaymin', N'Makwana', N'jayminmakwana1570@gmail.com', N'0e7517141fb53f21ee439b355b5a1d0a', 1, CAST(N'2021-03-24T16:20:57.303' AS DateTime), 43, CAST(N'2021-03-24T16:20:57.303' AS DateTime), 43, 1, NULL)
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [VerificationCode]) VALUES (46, 2, N'Jamu', N'Makwana', N'makwanajaymin2033@gmail.com', N'0e7517141fb53f21ee439b355b5a1d0a', 1, CAST(N'2021-03-24T17:00:59.663' AS DateTime), 43, CAST(N'2021-03-24T17:00:59.663' AS DateTime), 43, 1, N'3a38cd2a-d7f4-4c38-9497-6e5606848240')
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [VerificationCode]) VALUES (47, 2, N'Mehul', N'Parmar', N'jayminmakwana0409@gmail.com', N'0e7517141fb53f21ee439b355b5a1d0a', 1, CAST(N'2021-03-24T17:03:05.593' AS DateTime), 43, CAST(N'2021-03-24T17:03:27.527' AS DateTime), 43, 1, N'c09042af-75ab-4436-9474-d98bac6c6e3a')
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [VerificationCode]) VALUES (48, 1, N'Smart', N'Student', N'smartstudent2020system@gmail.com', N'0c2d680cc5c704ad1c4886718f6080cb', 1, CAST(N'2021-03-24T17:17:38.873' AS DateTime), NULL, NULL, NULL, 1, N'c3278e23-3310-4cbb-bb5b-0ef2e7bef22f')
INSERT [dbo].[Users] ([UserID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [VerificationCode]) VALUES (49, 1, N'Green', N'Skull', N'thegreenskull2020@gmail.com', N'55ef9f8710eebf3b1ef4acbdcdc36a1e', 1, CAST(N'2021-03-24T17:21:14.577' AS DateTime), NULL, NULL, NULL, 1, N'06f66cd1-7840-4e7f-b193-95969987f822')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Unique_EmailID_Users]    Script Date: 26-03-2021 17:38:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Unique_EmailID_Users] ON [dbo].[Users]
(
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsSellerHasAllowedDownload]  DEFAULT ((0)) FOR [IsSellerHasAllowedDownload]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsAttachmentDownloaded]  DEFAULT ((0)) FOR [IsAttachmentDownloaded]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsPaid]  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_IsPaid]  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesAttachments] ADD  CONSTRAINT [DF_SellerNotesAttachments_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesAttachments] ADD  CONSTRAINT [DF_SellerNotesAttachments_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[SellerNotesAttachments] ADD  CONSTRAINT [DF_SellerNotesAttachments_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] ADD  CONSTRAINT [DF_SellerNotesReportedIssues_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] ADD  CONSTRAINT [DF_SellerNotesReportedIssues_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] ADD  CONSTRAINT [DF_SellerNotesReportedIssues_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  CONSTRAINT [DF_SellerNotesReviews_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  CONSTRAINT [DF_SellerNotesReviews_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  CONSTRAINT [DF_SellerNotesReviews_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [DF_SystemConfigurations_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [DF_SystemConfigurations_ModofiedDate]  DEFAULT (getdate()) FOR [ModofiedDate]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  CONSTRAINT [DF_SystemConfigurations_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [DF_UserProfile_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [DF_UserProfile_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [DF_UserProfile_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (lower(CONVERT([varchar](32),hashbytes('MD5','Admin@123'),(2)))) FOR [Password]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsEmailVerified]  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([SellerNotesID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_SellerNotes]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users1] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users1]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([CoutryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Countries]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteCategories]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([TypeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteTypes]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_ReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[ReferenceData] ([ReferenceID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_ReferenceData]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([UserID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users1]
GO
ALTER TABLE [dbo].[SellerNotesAttachments]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttachments_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([SellerNotesID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesAttachments] CHECK CONSTRAINT [FK_SellerNotesAttachments_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Downloads] FOREIGN KEY([AgaintsDownloadID])
REFERENCES [dbo].[Downloads] ([DownloadsID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([SellerNotesID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users] FOREIGN KEY([ReportedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Downloads] FOREIGN KEY([AgaintsDownloadsID])
REFERENCES [dbo].[Downloads] ([DownloadsID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([SellerNotesID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Users] FOREIGN KEY([ReviewedByID])
REFERENCES [dbo].[Users] ([UserID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Users]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_ReferenceData] FOREIGN KEY([Gender])
REFERENCES [dbo].[ReferenceData] ([ReferenceID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_ReferenceData]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([RoleID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoles]
GO
USE [master]
GO
ALTER DATABASE [Notes-MarketPlace] SET  READ_WRITE 
GO
