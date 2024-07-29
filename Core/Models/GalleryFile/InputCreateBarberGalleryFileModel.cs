using System;

using Microsoft.AspNetCore.Http;

namespace Barbara.Core.Models.GalleryFile
{
    public class InputCreateBarberGalleryFileModel
    {
        public long GalleryId { get; set; }
        public IFormFile PictureFile { get; set; } = null!;
    }
}

