using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApplication.Persistence.Configurations
{
    public class PlaceEntityConfiguration : IEntityTypeConfiguration<PlaceEntity>
    {
        public void Configure(EntityTypeBuilder<PlaceEntity> builder)
        {
            builder.HasData(
                new PlaceEntity { Id = Guid.Parse("0225d3f4-33d2-4068-8275-ee56e4681e3e"), Name = "Mogilev Palace of Culture" },
                new PlaceEntity { Id = Guid.Parse("f131fdd7-e0ac-465f-8ec0-3056095ab06b"), Name = "Central Park of Culture and Recreation" },
                new PlaceEntity { Id = Guid.Parse("b8160578-9e2b-4885-bdc1-9b55618240ec"), Name = "Atrium Shopping Center" },
                new PlaceEntity { Id = Guid.Parse("5b685bbd-051f-4bc1-9dbb-8cd4c38217d0"), Name = "Museum of History of Mogilev" },
                new PlaceEntity { Id = Guid.Parse("2d747b20-1a65-48cf-986a-2a97e557c34a"), Name = "Thousand Years of Mogilev Square" },
                new PlaceEntity { Id = Guid.Parse("0559587c-b36d-4d42-b0f7-68639bf4e951"), Name = "Cathedral of St. Stanislaus" },
                new PlaceEntity { Id = Guid.Parse("d9cadc88-99da-4eea-8ce6-9af7431642e1"), Name = "Victory Park" },
                new PlaceEntity { Id = Guid.Parse("41dd7358-2cc4-4953-b141-659f1a7b8eea"), Name = "Mogilev Zoo" },
                new PlaceEntity { Id = Guid.Parse("94d29087-2af2-44df-9484-d3d8b0a4c3ce"), Name = "Europe Shopping Center" },
                new PlaceEntity { Id = Guid.Parse("d40dc058-d7c6-4dbf-92b8-ca9936a1362f"), Name = "Olympic Sports Complex" }
            );
        }
    }
}
