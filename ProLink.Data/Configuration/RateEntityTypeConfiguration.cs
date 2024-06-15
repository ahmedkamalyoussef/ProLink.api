using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Configuration
{
    public class RateEntityTypeConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            //builder
            //    .HasOne(r => r.Rater)
            //    .WithMany(u => u.SentRates)
            //    .HasForeignKey(r => r.RaterId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder
            //    .HasOne(r => r.Rated)
            //    .WithMany(r => r.ReceivedRates)
            //    .HasForeignKey(r => r.RatedId)
            //    .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(r => r.RatedJob)
                .WithOne(j => j.Rate)
                .HasForeignKey<Rate>(r => r.RatedJobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
