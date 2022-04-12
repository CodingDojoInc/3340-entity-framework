using System.ComponentModel.DataAnnotations;

namespace LostInTheWoods.Models
{
    public class Trail
    {
        public int TrailId {get;set;}
        [Required]
        public string Name {get;set;}
        [Required]
        public float Length {get;set;}
        [Range(-90, 90)]
        [Required]
        public float Latitude {get;set;}
        [Range(-180, 180)]
        [Required]
        public float Longitude {get;set;}
        [Required]
        [Display(Name="Elevation Change")]
        public float ElevationGain {get;set;}
        [Required]
        public string Description {get;set;}
    }

}