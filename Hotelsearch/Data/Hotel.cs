using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hotelsearch.Data
{
    /// <summary>
    /// Hotel property
    /// </summary>
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public Location GeoLocation { get; set; }

        /// <summary>
        /// used to sort the list after getting user geo location
        /// </summary>
        public double Distance { get; set; }

    }
}
