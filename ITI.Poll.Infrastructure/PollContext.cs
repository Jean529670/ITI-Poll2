using System.Threading.Tasks;
using ITI.Poll.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI.Poll.Infrastructure
{
    public class PollContext : DbContext, IUnitOfWork
    {
        public PollContext(DbContextOptions<PollContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ITI.Poll.Model.Poll> Polls { get; set; }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<Proposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("poll");
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PollConfiguration());
            modelBuilder.ApplyConfiguration(new ProposalConfiguration());
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
        }

        Task IUnitOfWork.SaveChanges() => SaveChangesAsync();
    }

    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tUser");
            builder.Property(u => u.UserId);
            builder.Property(u => u.Email);
            builder.Property(u => u.Nickname);
            builder.Property(u => u.PasswordHash);
            builder.Property(u => u.IsDeleted);
        }
    }

    public sealed class PollConfiguration : IEntityTypeConfiguration<ITI.Poll.Model.Poll>
    {
        public void Configure(EntityTypeBuilder<ITI.Poll.Model.Poll> builder)
        {
            builder.ToTable("tPoll");
            builder.Property(p => p.PollId);
            builder.Property(p => p.AuthorId);
            builder.Property(p => p.Question);
            builder.Property(p => p.IsDeleted);
            
            builder.HasMany(p => p.Proposals)
                .WithOne(p => p.Poll);

            builder.HasMany(p => p.Guests)
                .WithOne(g => g.Poll);
        }
    }

    public sealed class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.ToTable("tProposal");
            builder.Property(p => p.ProposalId);
            builder.Property(p => p.Text);
            
            builder.HasOne(p => p.Poll)
                .WithMany(p => p.Proposals);
        }
    }

    public sealed class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("tGuest");
            builder.HasKey(g => new { g.PollId, g.UserId });

            builder.Property(g => g.UserId);
            
            builder.HasOne(g => g.Poll)
                .WithMany(p => p.Guests);

            builder.HasOne(g => g.Vote)
                .WithMany(p => p.Voters)
                .HasForeignKey("VoteId");
        }
    }
}