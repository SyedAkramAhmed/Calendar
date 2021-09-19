using Calendar.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calendar.API.Model
{
    public class EventModel : IValidatableObject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [EmailArrayRequiredValidation]
        public string[] RequiredAttendees { get; set; }
        [EmailArrayNotRequiredValidation]
        public string[] OptionalAttendees { get; set; }
        [Required]
        public bool AllDay { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End Date must be greater than Start Date");
            }
        }
    }
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }
        public DbSet<EventModel> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>().Property(e => e.RequiredAttendees)
             .HasConversion(x => string.Join(',', x), x => x.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<EventModel>().Property(e => e.OptionalAttendees)
             .HasConversion(x => string.Join(',', x), x => x.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
