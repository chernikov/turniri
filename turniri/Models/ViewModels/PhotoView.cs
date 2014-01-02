using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class PhotoView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

		public int PhotoAlbumID {get; set; }

		public string FilePath {get; set; }

		public string AlbumPreviewPath {get; set; }

		public string AvatarPath {get; set; }

		public string SmallPath {get; set; }

		public string Name {get; set; }

		public string Description {get; set; }

		public bool IsAlbumPreview {get; set; }
    }
}