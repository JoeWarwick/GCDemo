using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CDWRepository
{
    public class CDWSVCModel : DbContext
    {
        public CDWSVCModel(DbContextOptions options) : base(options) {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite($"Filename=cdwdb.db3");
        }
        public DbSet<FeedSourceGroup> FeedSourceGroups { get; set; }
        public DbSet<FeedSource> FeedSources { get; set; }
        public DbSet<DbFeedType> FeedTypes { get; set; }
        public DbSet<Subscribable> Subscribables { get; set; }
        public DbSet<EmailSubscriber> EmailSubscribers { get; set; }
        public DbSet<FeedImage> FeedImages { get; set; }
        public DbSet<FeedTransform> FeedTransforms { get; set; }
        public DbSet<ClientErrorLog> ClientErrorLogs { get; set; }
        public DbSet<CDWSVCUser> CDWSVCUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<CDWSVCUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            modelBuilder.Entity<SubscribableExampleImages>()
                .HasKey(se => new { se.SubscribableId, se.ExampleImageId });
            modelBuilder.Entity<SubscribableExampleImages>()
                .HasOne(se => se.Subscribable)
                .WithMany(s => s.ExampleImages)
                .HasForeignKey(se => se.SubscribableId);
            modelBuilder.Entity<SubscribableExampleImages>()
                .HasOne(se => se.ExampleImage)
                .WithMany(s => s.Subscribables)
                .HasForeignKey(se => se.SubscribableId);
            modelBuilder.Entity<SubscribableFeedTransforms>()
                .HasKey(se => new { se.SubscribableId, se.FeedTransformId });
            modelBuilder.Entity<SubscribableFeedTransforms>()
                .HasOne(se => se.Subscribable)
                .WithMany(s => s.FeedTransforms)
                .HasForeignKey(se => se.SubscribableId);
            modelBuilder.Entity<SubscribableFeedTransforms>()
                .HasOne(se => se.FeedTransform)
                .WithMany(s => s.Subscribables)
                .HasForeignKey(se => se.SubscribableId);
            modelBuilder.Entity<FeedSourceExampleImages>()
                .HasKey(se => new { se.FeedSourceId, se.ExampleImageId });
            modelBuilder.Entity<FeedSourceExampleImages>()
                .HasOne(se => se.FeedSource)
                .WithMany(s => s.ExampleImages)
                .HasForeignKey(se => se.FeedSourceId);
            modelBuilder.Entity<FeedSourceExampleImages>()
                .HasOne(se => se.ExampleImage)
                .WithMany(s => s.FeedSources)
                .HasForeignKey(se => se.FeedSourceId);
            modelBuilder.Entity<FeedSourceFeedTransforms>()
                .HasKey(se => new { se.FeedSourceId, se.FeedTransformId });
            modelBuilder.Entity<FeedSourceFeedTransforms>()
                .HasOne(se => se.FeedSource)
                .WithMany(s => s.FeedTransforms)
                .HasForeignKey(se => se.FeedSourceId);
            modelBuilder.Entity<FeedSourceFeedTransforms>()
                .HasOne(se => se.FeedTransform)
                .WithMany(s => s.FeedSources)
                .HasForeignKey(se => se.FeedSourceId);
        }
    }
    public class SubscribableExampleImages
    {
        public int SubscribableId { get; set; }
        public Subscribable Subscribable { get; set; }
        public int ExampleImageId { get; set; }
        public FeedImage ExampleImage { get; set; }
    }

    public class SubscribableFeedTransforms
    {
        public int SubscribableId { get; set; }
        public Subscribable Subscribable { get; set; }
        public int FeedTransformId { get; set; }
        public FeedTransform FeedTransform { get; set; }
    }

    public class FeedSourceExampleImages
    {
        public int FeedSourceId { get; set; }
        public FeedSource FeedSource { get; set; }
        public int ExampleImageId { get; set; }
        public FeedImage ExampleImage { get; set; }
    }

    public class FeedSourceFeedTransforms
    {
        public int FeedSourceId { get; set; }
        public FeedSource FeedSource { get; set; }
        public int FeedTransformId { get; set; }
        public FeedTransform FeedTransform { get; set; }
    }

    public class CDWSVCUser : IdentityUser
    {
        public string AvatarUrl { get; set; }
        public string HashStr { get; set; }
        public string FirstName { get; set; }
        public ICollection<Invoice> InvoiceCollection { get; internal set; }

        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<CDWSVCUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateAsync(this);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class LookupBundle
    {
        public IEnumerable<DbFeedType> FeedTypes { get; set; }
    }

    public abstract class Subscribable
    {
        [Key]
        public int Id { get; set; }
        public bool IsArtifact { get; set; } = true;
        public int Rating { get; set; } = 0;
        public string Name { get; set; }
        public string Url { get; set; }
        public string WebUrl { get; set; }
        public string Description { get; set; }
        public virtual FeedSourceGroup SourceGroup { get; set; }
        public virtual FeedSource FeedSource { get; set; }
        public virtual DbFeedType FeedType { get; set; }
        public CDWSVCUser Owner { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;
        public bool Adult { get; set; } = false;
        public virtual ICollection<SubscribableExampleImages> ExampleImages { get; set; }
        public virtual ICollection<SubscribableFeedTransforms> FeedTransforms { get; set; }
        public int MaximumCacheCount { get; set; } = 0;
        public long CachePerTimeSpan { get; set; } = new TimeSpan(24, 0, 0).Ticks; //day
    }

    public class FeedSet : Subscribable
    {
        public List<Subscribable> Feeds { get; set; }
    }

    public class Feed : Subscribable
    {
        public int Status { get; set; } = 0;
        public DateTime? LastFetchDate { get; set; }
        public string LastError { get; set; }
    }

    public class FeedImage
    {
        [Key]
        public int FeedImageId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string Attribution { get; set; }
        public DateTime Published { get; set; }
        public string RelLink { get; set; }
        public bool Adult { get; set; } = false;
        public int Score { get; set; }
        public string Watermarking { get; set; } = "{0}";
        public bool DoNotShow { get; set; } = false;
        public CDWSVCUser Reporter { get; set; }
        public int RotateDegrees { get; set; } = 0;
        public string Documentation { get; set; }
        public bool IsPortrait { get; set; }
        public ICollection<SubscribableExampleImages> Subscribables { get; set; }
        public ICollection<FeedSourceExampleImages> FeedSources { get; set; }
    }

    public class Param
    {
        [Key]
        public int ParamId { get; set; }
        public string Type { get; set; } = "Format";
        public string Key { get; set; }
        public string Desc { get; set; }
        public string Choice { get; set; }
        public Dictionary<string, string> Choices { get; set; }
        public string Value { get; set; }
        public string[] Values { get; set; }
        public bool Required { get; set; } = false;
    }

    public class FeedTransform
    {
        [Key]
        public int FeedTransformerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public virtual List<Param> Params { get; set; }
        public virtual CDWSVCUser Owner { get; set; }
        public bool Shared { get; set; } = true;
        public DbFeedType InputFeedType { get; set; }
        public DbFeedType OutputFeedType { get; set; }
        public ICollection<SubscribableFeedTransforms> Subscribables { get; set; }
        public ICollection<FeedSourceFeedTransforms> FeedSources { get; set; }
    }

    public class FeedSource
    {
        [Key]
        public int FeedSourceId { get; set; }
        public string Name { get; set; }
        public virtual DbFeedType FeedType { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public virtual List<FeedTag> Tags { get; set; }
        public bool LoadChildren { get; set; } = true;
        public bool ProducesArtifact { get; set; } = false;
        public bool Adult { get; set; } = false;
        public int Rating { get; set; }
        public bool Shared { get; set; } = false;
        public virtual FeedSourceGroup Group { get; set; }
        public virtual ICollection<FeedSourceFeedTransforms> FeedTransforms { get; set; }
        public string WebUrl { get; set; }
        public string FeedBaseUrl { get; set; }
        public CDWSVCUser Owner { get; set; }
        public virtual ICollection<FeedSourceExampleImages> ExampleImages { get; set; }
        public virtual List<Subscribable> Feeds { get; set; }
        public virtual List<Param> Params { get; set; }
        public DateTime LastChange { get; set; } = DateTime.Now;
    }

    public class FeedTag
    {
        [Key]
        public string Tag { get; set; }
        public bool Adult { get; set; } = false;
        public virtual List<FeedSource> FeedSources { get; set; }

    }

    public class FeedSourceGroup
    {
        [Key]
        public int FeedSourceGroupId { get; set; }
        public string ShortName { get; set; }
        public string Site { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string BaseUri { get; set; }
        public virtual CDWSVCUser Owner { get; set; }
        public bool Adult { get; set; }
        public bool Shared { get; set; }
        public int Rating { get; set; }
        public List<Param> Params { get; set; }
        public List<FeedTransform> FeedTransforms { get; set; }
        public string FormatUrlString { get; internal set; }
    }

    public class EmailSubscriber
    {
        [Key]
        public string Email { get; set; }
        public DateTime DateSent { get; set; }
        public bool Replied { get; set; }
    }

    public enum FeedKind
    {
        XML,
        CSV,
        TXT
    }

    [DataContract(Namespace = "")]
    public class DbFeedType
    {
        [Key]
        public int FeedTypeId { get; set; }
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string Version { get; set; }
    }

    public class Subscription //: SubscriptionEntityBase
    {

    }

    public class ClientErrorLog
    {
        [Key]
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime FirstSubmissionDate { get; set; }

        public DateTime LastSubmissionDate { get; set; } = DateTime.Now;

        public String ErrorMessage { get; set; }

        public String StackHash { get; set; }

        public String StackTrace { get; set; }

        public int SubmissionCount { get; set; } = 1;

    }

    public class Invoice
    {
        [Key]
        public string InvoiceID { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }

        public CDWSVCUser CustomerField { get; set; }

    }
}
