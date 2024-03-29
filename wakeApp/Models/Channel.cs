﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace wakeApp.Models
{
    public class Channel
    {
        public int Id { get; set; }
        [Display(Name = "Nome do Canal")]
        public string ChannelName { get; set; } = string.Empty;
        [Display(Name = "Subtitulo do Canal")]
        public string SubtitleChannel { get; set; } = string.Empty;
        [Display(Name = "Descrição do Canal")]
        public string ChannelDescription { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime CreatedChannel { get; set; }
        public string ImageBanner { get; set; } = string.Empty;
        public string IconChannel { get; set; } = string.Empty;
        public int CountFollows { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public List<PostVideo>? PostVideos { get; set; }
    }
}
