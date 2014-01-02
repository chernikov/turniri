using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace turniri.Tools
{
    /// <summary>
    /// Class for make preview images
    /// </summary>
    public static class PreviewCreator
    {
        public static int Quality = 95;

        /// <summary>
        /// Check size 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="previewSize"></param>
        /// <returns></returns>
        public static bool CheckSize(Stream stream, Size previewSize)
        {
            Bitmap image = null;
            try
            {
                image = new Bitmap(stream);
                return (image.Width == previewSize.Width) && (image.Height == previewSize.Height);
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// Create preview from memory stream
        /// 
        /// Preview is small image witch get from base image using rules:
        /// if size of image less that preview image - put in on center of preview
        /// if heigth  less than width - get middle part and resize to preview 
        ///if heigth more than width - get top part and resize to preview 
        /// </summary>
        /// <param name="stream">memory stream</param>
        /// <param name="previewSize">size of preview image</param>
        /// <param name="fileName">file name to save</param>
        /// <param name="grayscale">grayscaled image</param>
        public static void CreateAndSavePreview(Stream stream, Size previewSize, string fileName, bool grayscale = false)
        {
            var image = new Bitmap(stream);
            Bitmap newBitmap = null;
            try
            {
                if ((image.Width <= previewSize.Width) && (image.Height <= previewSize.Height))
                {
                    newBitmap = BaseImageOnCenter(previewSize, image);
                    SaveJpeg(fileName, newBitmap, grayscale);
                }
                else
                {

                    var widthCoeff = (float)image.Width / previewSize.Width;
                    var heightCoeff = (float)image.Height / previewSize.Height;
                    if (widthCoeff > heightCoeff)
                    {
                        //horizontal based image - get center part
                        newBitmap = HorizontalCenterCut(previewSize, image, heightCoeff);
                    }
                    else
                    {
                        newBitmap = VerticalTopCut(previewSize, image, widthCoeff);
                    }
                    SaveJpeg(fileName, newBitmap, grayscale);
                }
            }
            finally
            {
                image.Dispose();
                if (newBitmap != null)
                {
                    newBitmap.Dispose();
                }
            }
        }


       

        /// <summary>
        /// Create and save image
        /// </summary>
        /// <param name="stream">Stream with image</param>
        /// <param name="maxSize">max size</param>
        /// <param name="fileName">filename to save</param>
        /// <param name="grayscale">grayscale</param>
        public static void CreateAndSaveImage(Stream stream, Size maxSize, string fileName,  out Size outSize, bool grayscale = false)
        {
            outSize = new Size();
            var image = new Bitmap(stream);
            Bitmap newBitmap = null;
            try
            {
                if (image.Width <= maxSize.Width && image.Height <= maxSize.Height)
                {
                    outSize = new Size(image.Width, image.Height);
                    SaveJpeg(fileName, image, grayscale);
                }
                else
                {
                    newBitmap = ScaleResize(maxSize, image, out outSize);
                    SaveJpeg(fileName, newBitmap, grayscale);
                }

            }
            finally
            {
                image.Dispose();
                if (newBitmap != null)
                {
                    newBitmap.Dispose();
                }
            }
        }

        public static void CreateAndSaveImage(Stream stream, Size maxSize, string fileName, bool grayscale = false)
        {
            Size size;
            CreateAndSaveImage(stream, maxSize, fileName, out size, grayscale);
        }

        /// <summary>
        /// Create and save image
        /// </summary>
        /// <param name="stream">Stream with image</param>
        /// <param name="maxSize">max size</param>
        /// <param name="fileName">filename to save</param>
        /// <param name="grayscale">grayscale</param>
        public static void CreateAndSaveImagePng(Stream stream, string fileName, bool grayscale = false)
        {
            var image = new Bitmap(stream);
            try
            {
                SavePng(fileName, image, grayscale);
            }
            finally
            {
                image.Dispose();
            }
        }

        /// <summary>
        /// Create and save image
        /// </summary>
        /// <param name="stream">Stream with image</param>
        /// <param name="maxSize">max size</param>
        /// <param name="fileName">filename to save</param>
        /// <param name="grayscale">grayscale</param>
        public static void CreateAndSaveFitToSize(Stream stream, Size previewSize, string fileName, bool grayscale = false)
        {
            var image = new Bitmap(stream);
            Bitmap newBitmap = null;
            try
            {
                var widthCoeff = (float)image.Width / previewSize.Width;
                var heightCoeff = (float)image.Height / previewSize.Height;
                if (widthCoeff > heightCoeff)
                {
                    //horizontal based image - get center part
                    newBitmap = HorizontalCenterCut(previewSize, image, heightCoeff);
                }
                else
                {
                    newBitmap = VerticalTopCut(previewSize, image, widthCoeff);
                }
                SaveJpeg(fileName, newBitmap, grayscale);
            }
            finally
            {
                image.Dispose();
                if (newBitmap != null)
                {
                    newBitmap.Dispose();
                }
            }
        }


        /// <summary>
        /// Avatar is small image based on those rules:
        /// if image horizontal directed - make center preview cut
        /// if image vertical directed - make scale resizing 
        /// </summary>
        /// <param name="stream">Stream with  image</param>
        /// <param name="maxSize">Max size</param>
        /// <param name="fileName">filename to  save</param>
        /// <param name="grayscale">grayscale</param>
        public static void CreateAndSaveAvatar(Stream stream, Size maxSize, string fileName, bool grayscale = false)
        {
            var image = new Bitmap(stream);
            Bitmap newBitmap = null;
            try
            {
                if (image.Width <= maxSize.Width && image.Height <= maxSize.Height)
                {
                    newBitmap = BaseImageOnCenter(maxSize, image);
                }
                else
                {
                    var widthCoeff = (float)image.Width / maxSize.Width;
                    var heightCoeff = (float)image.Height / maxSize.Height;
                    if (widthCoeff < heightCoeff)
                    {
                        newBitmap = HorizontalCenterCut(maxSize, image, heightCoeff);
                    }
                    else
                    {
                        newBitmap = ScaleResize(maxSize, image);
                    }
                }
                SaveJpeg(fileName, newBitmap, grayscale);
            }
            finally
            {
                image.Dispose();
                if (newBitmap != null)
                {
                    newBitmap.Dispose();
                }
            }
        }

        private static Bitmap HorizontalCenterCut(Size previewSize, Bitmap image, float heightCoeff)
        {
            Bitmap newBitmap = new Bitmap(previewSize.Width, previewSize.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);
            //x is difference on horizontal to cut image
            float x = ((float)image.Width / 2) - ((float)previewSize.Width / 2) * heightCoeff;
            MakeGraphics(previewSize, graphics);
            graphics.DrawImage(image,
                               new RectangleF(-1, -1, (float)previewSize.Width + 2, (float)previewSize.Height + 2),
                               new RectangleF(x, 0f, previewSize.Width * heightCoeff, image.Height), GraphicsUnit.Pixel);
            return newBitmap;
        }

        private static Bitmap BaseImageOnCenter(Size previewSize, Bitmap image)
        {
            Bitmap newBitmap = new Bitmap(previewSize.Width, previewSize.Height);
            var x = ((float)previewSize.Width / 2) - ((float)image.Width / 2);
            var y = ((float)previewSize.Height / 2) - ((float)image.Height / 2);
            Graphics graphics = Graphics.FromImage(newBitmap);
            MakeGraphics(previewSize, graphics);
            graphics.DrawImage(image,
                               new RectangleF(x, y, image.Width, image.Height),
                               new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            return newBitmap;
        }


        private static Bitmap VerticalTopCut(Size previewSize, Bitmap image, float widthCoeff)
        {
            Bitmap newBitmap = new Bitmap(previewSize.Width, previewSize.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);
            //calculate top heitght
            var height = previewSize.Height * widthCoeff;
            MakeGraphics(previewSize, graphics);
            graphics.DrawImage(image,
                               new RectangleF(-1, -1, (float)previewSize.Width + 2, (float)previewSize.Height + 2),
                               new RectangleF(0f, 0f, image.Width, height), GraphicsUnit.Pixel);
            return newBitmap;
        }
        private static Bitmap ScaleResize(Size maxSize, Bitmap image)
        {
            Size size;
            return ScaleResize(maxSize, image, out size);
        }

        private static Bitmap ScaleResize(Size maxSize, Bitmap image, out Size outSize)
        {
            var widthCoeff = (float)image.Width / maxSize.Width;
            var heightCoeff = (float)image.Height / maxSize.Height;
            var coeff = widthCoeff < heightCoeff ? heightCoeff : widthCoeff;
            var newSize = new Size((int)(image.Width / coeff), (int)(image.Height / coeff));
            var newBitmap = new Bitmap(newSize.Width, newSize.Height);
            var graphics = Graphics.FromImage(newBitmap);
            MakeGraphics(maxSize, graphics);
            graphics.DrawImage(image,
                               new RectangleF(-1, -1, (float)newSize.Width + 2, (float)newSize.Height + 2),
                               new RectangleF(0f, 0f, image.Width, image.Height), GraphicsUnit.Pixel);
            outSize = newSize;
            return newBitmap;
        }

        private static void MakeGraphics(Size previewSize, Graphics graphics)
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.FillRectangle(Brushes.White, 0, 0, previewSize.Width, previewSize.Height);
        }

        private static void SaveJpeg(string path, Bitmap image, bool grayscale)
        {
            //ensure the quality is within the correct range
            if ((Quality < 0) || (Quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", Quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentOutOfRangeException("path can't be empty");
            }

            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            //create an encoder parameter for the image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, Quality);
            //get the jpeg codec
            var jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            var encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            if (grayscale)
            {
                var grayscaleImage = MakeGrayscale(image);
                grayscaleImage.Save(path, jpegCodec, encoderParams);
                grayscaleImage.Dispose();
            }
            else
            {
                image.Save(path, jpegCodec, encoderParams);
            }
        }

        private static void SavePng(string path, Bitmap image, bool grayscale)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentOutOfRangeException("path can't be empty");
            }

            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            //create an encoder parameter for the image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, 10L);
            //get the jpeg codec
            var pngCodec = GetEncoderInfo("image/png");

            //create a collection of all parameters that we will pass to the encoder
            var encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            if (grayscale)
            {
                var grayscaleImage = MakeGrayscalePng(image);
                grayscaleImage.Save(path, pngCodec, encoderParams);
                grayscaleImage.Dispose();
            }
            else
            {
                image.Save(path, pngCodec, encoderParams);
            }
        }

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> _encoders;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (_encoders == null)
                {
                    _encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (_encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        _encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return _encoders;
            }
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        private static Bitmap MakeGrayscale(Bitmap original)
        {
            //make an empty bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    var originalColor = original.GetPixel(i, j);


                    //create the grayscale version of the pixel
                    var grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));

                    //create the color object
                    var newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        private static Bitmap MakeGrayscalePng(Bitmap original)
        {
            //make an empty bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    var originalColor = original.GetPixel(i, j);


                    //create the grayscale version of the pixel
                    var grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));
                    var alfa = originalColor.A;
                    //create the color object
                    var newColor = Color.FromArgb(alfa, grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// types witch supported for scale
        /// </summary>
        /// <param name="mimeType">mimeType</param>
        /// <returns></returns>
        public static bool SupportMimeType(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpg":
                case "image/jpeg":
                case "image/png":
                case "image/gif":
                    return true;
            }
            return false;
        }
    }
}
