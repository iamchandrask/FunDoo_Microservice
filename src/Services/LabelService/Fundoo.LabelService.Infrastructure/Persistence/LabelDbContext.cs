using Fundoo.LabelService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.LabelService.Infrastructure.Persistence;

public class LabelDbContext : DbContext
{
    public LabelDbContext(DbContextOptions<LabelDbContext> options) : base(options)
    {
    }

    public DbSet<Label> Labels => Set<Label>();
    public DbSet<NoteLabel> NotesLabels => Set<NoteLabel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Label>(entity =>
        {
            entity.ToTable("Labels");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => new { x.UserId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<NoteLabel>(entity =>
        {
            entity.ToTable("NotesLabels");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.NoteId, x.LabelId }).IsUnique();
        });
    }
}
