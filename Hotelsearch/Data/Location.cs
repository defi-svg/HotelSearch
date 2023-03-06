using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace hotelsearch.Data
{
    /// <summary>
    /// Coordinates of a geo location
    /// </summary>
    public class Location
    {
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Long { get; set; }
     
    }

    
    
}
