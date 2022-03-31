﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wakeApp.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? Created { get; set; }
        [NotMapped]
        public IFormFile? FileVideo { get; set; }
        public string VideoFile { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile FileImage { get; set; }
        public string ThumbImage { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User? User { get; set; }

    }
}
