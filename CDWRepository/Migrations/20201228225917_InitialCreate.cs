using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CDWRepository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(nullable: false),
                    FirstSubmissionDate = table.Column<DateTime>(nullable: false),
                    LastSubmissionDate = table.Column<DateTime>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true),
                    StackHash = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    SubmissionCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailSubscribers",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    Replied = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSubscribers", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "FeedTag",
                columns: table => new
                {
                    Tag = table.Column<string>(nullable: false),
                    Adult = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTag", x => x.Tag);
                });

            migrationBuilder.CreateTable(
                name: "FeedTypes",
                columns: table => new
                {
                    FeedTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTypes", x => x.FeedTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Param",
                columns: table => new
                {
                    ParamId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Choice = table.Column<string>(nullable: true),
                    Choices = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Values = table.Column<string>(nullable: true),
                    Required = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Param", x => x.ParamId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    HashStr = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedImages",
                columns: table => new
                {
                    FeedImageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    Attribution = table.Column<string>(nullable: true),
                    Published = table.Column<DateTime>(nullable: false),
                    RelLink = table.Column<string>(nullable: true),
                    Adult = table.Column<bool>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Watermarking = table.Column<string>(nullable: true),
                    DoNotShow = table.Column<bool>(nullable: false),
                    ReporterId = table.Column<string>(nullable: true),
                    RotateDegrees = table.Column<int>(nullable: false),
                    Documentation = table.Column<string>(nullable: true),
                    IsPortrait = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedImages", x => x.FeedImageId);
                    table.ForeignKey(
                        name: "FK_FeedImages_User_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceGroups",
                columns: table => new
                {
                    FeedSourceGroupId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BaseUri = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Adult = table.Column<bool>(nullable: false),
                    Shared = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    FormatUrlString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceGroups", x => x.FeedSourceGroupId);
                    table.ForeignKey(
                        name: "FK_FeedSourceGroups_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedTransforms",
                columns: table => new
                {
                    FeedTransformId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Shared = table.Column<bool>(nullable: false),
                    InputFeedTypeFeedTypeId = table.Column<int>(nullable: true),
                    OutputFeedTypeFeedTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTransforms", x => x.FeedTransformId);
                    table.ForeignKey(
                        name: "FK_FeedTransforms_FeedTypes_InputFeedTypeFeedTypeId",
                        column: x => x.InputFeedTypeFeedTypeId,
                        principalTable: "FeedTypes",
                        principalColumn: "FeedTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedTransforms_FeedTypes_OutputFeedTypeFeedTypeId",
                        column: x => x.OutputFeedTypeFeedTypeId,
                        principalTable: "FeedTypes",
                        principalColumn: "FeedTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedTransforms_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceID = table.Column<string>(nullable: false),
                    CustomerID = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    CustomerFieldId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoice_User_CustomerFieldId",
                        column: x => x.CustomerFieldId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceGroupParams",
                columns: table => new
                {
                    FeedSourceGroupId = table.Column<int>(nullable: false),
                    ParamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceGroupParams", x => new { x.FeedSourceGroupId, x.ParamId });
                    table.ForeignKey(
                        name: "FK_FeedSourceGroupParams_FeedSourceGroups_FeedSourceGroupId",
                        column: x => x.FeedSourceGroupId,
                        principalTable: "FeedSourceGroups",
                        principalColumn: "FeedSourceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceGroupParams_Param_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Param",
                        principalColumn: "ParamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSources",
                columns: table => new
                {
                    FeedSourceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    FeedTypeId = table.Column<int>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LoadChildren = table.Column<bool>(nullable: false),
                    ProducesArtifact = table.Column<bool>(nullable: false),
                    Adult = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Shared = table.Column<bool>(nullable: false),
                    GroupFeedSourceGroupId = table.Column<int>(nullable: true),
                    WebUrl = table.Column<string>(nullable: true),
                    FeedBaseUrl = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    LastChange = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSources", x => x.FeedSourceId);
                    table.ForeignKey(
                        name: "FK_FeedSources_FeedTypes_FeedTypeId",
                        column: x => x.FeedTypeId,
                        principalTable: "FeedTypes",
                        principalColumn: "FeedTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedSources_FeedSourceGroups_GroupFeedSourceGroupId",
                        column: x => x.GroupFeedSourceGroupId,
                        principalTable: "FeedSourceGroups",
                        principalColumn: "FeedSourceGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedSources_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceGroupFeedTransforms",
                columns: table => new
                {
                    FeedSourceGroupId = table.Column<int>(nullable: false),
                    FeedTransformId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceGroupFeedTransforms", x => new { x.FeedSourceGroupId, x.FeedTransformId });
                    table.ForeignKey(
                        name: "FK_FeedSourceGroupFeedTransforms_FeedSourceGroups_FeedSourceGroupId",
                        column: x => x.FeedSourceGroupId,
                        principalTable: "FeedSourceGroups",
                        principalColumn: "FeedSourceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceGroupFeedTransforms_FeedTransforms_FeedTransformId",
                        column: x => x.FeedTransformId,
                        principalTable: "FeedTransforms",
                        principalColumn: "FeedTransformId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedTransformParams",
                columns: table => new
                {
                    FeedTransformId = table.Column<int>(nullable: false),
                    ParamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTransformParams", x => new { x.FeedTransformId, x.ParamId });
                    table.ForeignKey(
                        name: "FK_FeedTransformParams_FeedTransforms_FeedTransformId",
                        column: x => x.FeedTransformId,
                        principalTable: "FeedTransforms",
                        principalColumn: "FeedTransformId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedTransformParams_Param_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Param",
                        principalColumn: "ParamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceChildren",
                columns: table => new
                {
                    FeedSourceParentId = table.Column<int>(nullable: false),
                    FeedSourceChildId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceChildren", x => new { x.FeedSourceParentId, x.FeedSourceChildId });
                    table.ForeignKey(
                        name: "FK_FeedSourceChildren_FeedSources_FeedSourceChildId",
                        column: x => x.FeedSourceChildId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceChildren_FeedSources_FeedSourceParentId",
                        column: x => x.FeedSourceParentId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceExampleImages",
                columns: table => new
                {
                    FeedSourceId = table.Column<int>(nullable: false),
                    ExampleImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceExampleImages", x => new { x.FeedSourceId, x.ExampleImageId });
                    table.ForeignKey(
                        name: "FK_FeedSourceExampleImages_FeedImages_ExampleImageId",
                        column: x => x.ExampleImageId,
                        principalTable: "FeedImages",
                        principalColumn: "FeedImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceExampleImages_FeedSources_FeedSourceId",
                        column: x => x.FeedSourceId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceFeedTags",
                columns: table => new
                {
                    FeedSourceId = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(nullable: false),
                    FeedTagTag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceFeedTags", x => new { x.FeedSourceId, x.Tag });
                    table.ForeignKey(
                        name: "FK_FeedSourceFeedTags_FeedSources_FeedSourceId",
                        column: x => x.FeedSourceId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceFeedTags_FeedTag_FeedTagTag",
                        column: x => x.FeedTagTag,
                        principalTable: "FeedTag",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceFeedTransforms",
                columns: table => new
                {
                    FeedSourceId = table.Column<int>(nullable: false),
                    FeedTransformId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceFeedTransforms", x => new { x.FeedSourceId, x.FeedTransformId });
                    table.ForeignKey(
                        name: "FK_FeedSourceFeedTransforms_FeedSources_FeedSourceId",
                        column: x => x.FeedSourceId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceFeedTransforms_FeedTransforms_FeedTransformId",
                        column: x => x.FeedTransformId,
                        principalTable: "FeedTransforms",
                        principalColumn: "FeedTransformId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedSourceParams",
                columns: table => new
                {
                    FeedSourceId = table.Column<int>(nullable: false),
                    ParamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSourceParams", x => new { x.FeedSourceId, x.ParamId });
                    table.ForeignKey(
                        name: "FK_FeedSourceParams_FeedSources_FeedSourceId",
                        column: x => x.FeedSourceId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedSourceParams_Param_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Param",
                        principalColumn: "ParamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscribables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsArtifact = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    WebUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SourceGroupFeedSourceGroupId = table.Column<int>(nullable: true),
                    FeedSourceId = table.Column<int>(nullable: true),
                    FeedTypeId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Added = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Adult = table.Column<bool>(nullable: false),
                    MaximumCacheCount = table.Column<int>(nullable: false),
                    CachePerTimeSpan = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribables_FeedSources_FeedSourceId",
                        column: x => x.FeedSourceId,
                        principalTable: "FeedSources",
                        principalColumn: "FeedSourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscribables_FeedTypes_FeedTypeId",
                        column: x => x.FeedTypeId,
                        principalTable: "FeedTypes",
                        principalColumn: "FeedTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscribables_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscribables_FeedSourceGroups_SourceGroupFeedSourceGroupId",
                        column: x => x.SourceGroupFeedSourceGroupId,
                        principalTable: "FeedSourceGroups",
                        principalColumn: "FeedSourceGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscribableExampleImages",
                columns: table => new
                {
                    SubscribableId = table.Column<int>(nullable: false),
                    ExampleImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribableExampleImages", x => new { x.SubscribableId, x.ExampleImageId });
                    table.ForeignKey(
                        name: "FK_SubscribableExampleImages_FeedImages_ExampleImageId",
                        column: x => x.ExampleImageId,
                        principalTable: "FeedImages",
                        principalColumn: "FeedImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscribableExampleImages_Subscribables_SubscribableId",
                        column: x => x.SubscribableId,
                        principalTable: "Subscribables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscribableFeedTransforms",
                columns: table => new
                {
                    SubscribableId = table.Column<int>(nullable: false),
                    FeedTransformId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribableFeedTransforms", x => new { x.SubscribableId, x.FeedTransformId });
                    table.ForeignKey(
                        name: "FK_SubscribableFeedTransforms_FeedTransforms_FeedTransformId",
                        column: x => x.FeedTransformId,
                        principalTable: "FeedTransforms",
                        principalColumn: "FeedTransformId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscribableFeedTransforms_Subscribables_SubscribableId",
                        column: x => x.SubscribableId,
                        principalTable: "Subscribables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedImages_ReporterId",
                table: "FeedImages",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceChildren_FeedSourceChildId",
                table: "FeedSourceChildren",
                column: "FeedSourceChildId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceExampleImages_ExampleImageId",
                table: "FeedSourceExampleImages",
                column: "ExampleImageId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceFeedTags_FeedTagTag",
                table: "FeedSourceFeedTags",
                column: "FeedTagTag");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceFeedTransforms_FeedTransformId",
                table: "FeedSourceFeedTransforms",
                column: "FeedTransformId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceGroupFeedTransforms_FeedTransformId",
                table: "FeedSourceGroupFeedTransforms",
                column: "FeedTransformId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceGroupParams_ParamId",
                table: "FeedSourceGroupParams",
                column: "ParamId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceGroups_OwnerId",
                table: "FeedSourceGroups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSourceParams_ParamId",
                table: "FeedSourceParams",
                column: "ParamId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSources_FeedTypeId",
                table: "FeedSources",
                column: "FeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSources_GroupFeedSourceGroupId",
                table: "FeedSources",
                column: "GroupFeedSourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedSources_OwnerId",
                table: "FeedSources",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTransformParams_ParamId",
                table: "FeedTransformParams",
                column: "ParamId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTransforms_InputFeedTypeFeedTypeId",
                table: "FeedTransforms",
                column: "InputFeedTypeFeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTransforms_OutputFeedTypeFeedTypeId",
                table: "FeedTransforms",
                column: "OutputFeedTypeFeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTransforms_OwnerId",
                table: "FeedTransforms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerFieldId",
                table: "Invoice",
                column: "CustomerFieldId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscribableExampleImages_ExampleImageId",
                table: "SubscribableExampleImages",
                column: "ExampleImageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribableFeedTransforms_FeedTransformId",
                table: "SubscribableFeedTransforms",
                column: "FeedTransformId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribables_FeedSourceId",
                table: "Subscribables",
                column: "FeedSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribables_FeedTypeId",
                table: "Subscribables",
                column: "FeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribables_OwnerId",
                table: "Subscribables",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribables_SourceGroupFeedSourceGroupId",
                table: "Subscribables",
                column: "SourceGroupFeedSourceGroupId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClientErrorLogs");

            migrationBuilder.DropTable(
                name: "EmailSubscribers");

            migrationBuilder.DropTable(
                name: "FeedSourceChildren");

            migrationBuilder.DropTable(
                name: "FeedSourceExampleImages");

            migrationBuilder.DropTable(
                name: "FeedSourceFeedTags");

            migrationBuilder.DropTable(
                name: "FeedSourceFeedTransforms");

            migrationBuilder.DropTable(
                name: "FeedSourceGroupFeedTransforms");

            migrationBuilder.DropTable(
                name: "FeedSourceGroupParams");

            migrationBuilder.DropTable(
                name: "FeedSourceParams");

            migrationBuilder.DropTable(
                name: "FeedTransformParams");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "SubscribableExampleImages");

            migrationBuilder.DropTable(
                name: "SubscribableFeedTransforms");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "FeedTag");

            migrationBuilder.DropTable(
                name: "Param");

            migrationBuilder.DropTable(
                name: "FeedImages");

            migrationBuilder.DropTable(
                name: "FeedTransforms");

            migrationBuilder.DropTable(
                name: "Subscribables");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "FeedSources");

            migrationBuilder.DropTable(
                name: "FeedTypes");

            migrationBuilder.DropTable(
                name: "FeedSourceGroups");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
