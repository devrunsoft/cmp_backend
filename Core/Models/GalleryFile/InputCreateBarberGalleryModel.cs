using System;

using Microsoft.AspNetCore.Http;

namespace Barbara.Core.Models.GalleryFile
{
    public class InputCreateBarberGalleryModel
    {
        public List<IFormFile> PictureFile { get; set; } = null!;
        public string? Desc { get; set; }
    }
}

