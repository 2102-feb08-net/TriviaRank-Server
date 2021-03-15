using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class TriviaRankContext : DbContext
    {
        public TriviaRankContext()
        {
        }

        public TriviaRankContext(DbContextOptions<TriviaRankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<FriendInviteOutbox> FriendInviteOutboxes { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameInviteOutbox> GameInviteOutboxes { get; set; }
        public virtual DbSet<GamePlayer> GamePlayers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("Friend");

                entity.HasOne(d => d.FriendNavigation)
                    .WithMany(p => p.FriendFriendNavigations)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friend__FriendId__531856C7");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.FriendPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friend__PlayerId__5224328E");
            });

            modelBuilder.Entity<FriendInviteOutbox>(entity =>
            {
                entity.ToTable("FriendInviteOutbox");

                entity.HasOne(d => d.Invited)
                    .WithMany(p => p.FriendInviteOutboxInviteds)
                    .HasForeignKey(d => d.InvitedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FriendInv__Invit__31B762FC");

                entity.HasOne(d => d.Inviter)
                    .WithMany(p => p.FriendInviteOutboxInviters)
                    .HasForeignKey(d => d.InviterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FriendInv__Invit__30C33EC3");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__Game__OwnerId__42E1EEFE");
            });

            modelBuilder.Entity<GameInviteOutbox>(entity =>
            {
                entity.ToTable("GameInviteOutbox");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameInviteOutboxes)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameInvit__GameI__55F4C372");

                entity.HasOne(d => d.Invited)
                    .WithMany(p => p.GameInviteOutboxes)
                    .HasForeignKey(d => d.InvitedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameInvit__Invit__56E8E7AB");
            });

            modelBuilder.Entity<GamePlayer>(entity =>
            {
                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamePlayers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GamePlaye__GameI__46B27FE2");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GamePlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GamePlaye__Playe__47A6A41B");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Body).HasMaxLength(400);

                entity.HasOne(d => d.From)
                    .WithMany(p => p.MessageFroms)
                    .HasForeignKey(d => d.FromId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__FromId__4E53A1AA");

                entity.HasOne(d => d.To)
                    .WithMany(p => p.MessageTos)
                    .HasForeignKey(d => d.ToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__ToId__4F47C5E3");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.HasIndex(e => e.Username, "UQ__Player__536C85E4AB2BC322")
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('PASSWORD')");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.Property(e => e.Selection)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerStatistics)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK__PlayerSta__Playe__3F115E1A");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.QuestionId, e.GameId })
                    .HasName("PK__Question__1EE2526A0F23A441");

                entity.ToTable("Question");

                entity.Property(e => e.PlayerAnswer)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__GameId__5AB9788F");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__Player__5BAD9CC8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
