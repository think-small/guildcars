﻿using System.ComponentModel.DataAnnotations;

namespace GuildCars.Models
{
    public class TransmissionType
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
