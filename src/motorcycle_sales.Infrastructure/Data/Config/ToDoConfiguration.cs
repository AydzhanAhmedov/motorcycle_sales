using motorcycle_sales.Core.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace motorcycle_sales.Infrastructure.Data.Config;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
  public void Configure(EntityTypeBuilder<ToDoItem> builder)
  {
    builder.Property(t => t.Title)
        .IsRequired();
  }
}
