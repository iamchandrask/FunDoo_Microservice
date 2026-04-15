using Fundoo.CollaborationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.CollaborationService.Infrastructure.Persistence;

public class CollaborationDbContext : DbContext
{
    public CollaborationDbContext(DbContextOptions<CollaborationDbContext> options) : base(options)
    {
    }

    public DbSet<Collaborator> Collaborators => Set<Collaborator>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collaborator>(entity =>
        {
            entity.ToTable("Collaborators");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CollaboratorEmail).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => new { x.NoteId, x.CollaboratorEmail }).IsUnique();
        });
    }
}