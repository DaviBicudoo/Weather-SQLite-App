using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Weather_SQLite_App.Models
{
    public class Weather
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? Visibility { get; set; }
        public double? MaxTemperature { get; set; }
        public double? MinTemperature { get; set; }
        public double? Temperature { get; set; }
        public string? Sunrise { get; set; }
        public string? Sunset { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public double? WindSpeed { get; set; }
    }
}
