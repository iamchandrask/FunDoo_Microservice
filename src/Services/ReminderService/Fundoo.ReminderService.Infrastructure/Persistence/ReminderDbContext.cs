using Fundoo.ReminderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.ReminderService.Infrastructure.Persistence;

public class ReminderDbContext : DbContext
{
    public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options)
    {
    }

    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.ToTable("Reminders");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Message).HasMaxLength(500);
            entity.HasIndex(x => x.NoteId);
        });
    }
}