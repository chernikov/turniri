using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using turniri.Tools.Video;

namespace turniri.UnitTest.Test
{
    [TestFixture]
    public class VideoHelperTest
    {
        [Test]
        public void GetVideoThumbByUrl_GetRutubeVideoThrumb_GetImageUrl()
        {
            var url =
                VideoHelper.GetVideoThumbByUrl("http://rutube.ru/video/d10bd48a20008d24859f7e0a74237862/#.ULNo_YfqnMp");

            Assert.AreEqual(url, "http://tub.rutube.ru/thumbs-wide/84/2f/842f1f98d1e3d23bb8869bfea035b2aa-1.jpg");
        }

        [Test]
        public void GetRutubeVideoByUrl_GetRutubeVideoCode_GetEmbedCode()
        {
            var url =
                VideoHelper.GetVideoByUrl("http://rutube.ru/video/d10bd48a20008d24859f7e0a74237862/#.ULNo_YfqnMp");
            Assert.AreEqual(url, "<OBJECT width=\"800\" height=\"600\" id=\"participantID\"><PARAM name=\"movie\" value=\"http://video.rutube.ru/842f1f98d1e3d23bb8869bfea035b2aa\"></PARAM><PARAM name=\"wmode\" value=\"opaque\"></PARAM><PARAM name=\"allowFullScreen\" value=\"true\"></PARAM><PARAM name=\"allowscriptaccess\" value=\"always\"></PARAM><EMBED src=\"http://video.rutube.ru/842f1f98d1e3d23bb8869bfea035b2aa\" type=\"application/x-shockwave-flash\" wmode=\"opaque\" width=\"800\" height=\"600\" allowFullScreen=\"true\" allowscriptaccess=\"always\"></EMBED></OBJECT>");
        }
    }
}
