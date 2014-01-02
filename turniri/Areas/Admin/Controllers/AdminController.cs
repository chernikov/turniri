using System.Web.Mvc;
using turniri.Controllers;
using turniri.Model;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Drawing;
using turniri.Tools;
using System.Net;
using System.IO;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,editor,seller")]
    public abstract class AdminController : BaseController
    {
        private static string VideoFolder = "/Media/files/videos/";
        private static string VideoThumbSize = "VideoThumbSize";

        protected override void Initialize(RequestContext requestContext)
        {
            CultureInfo ci = new CultureInfo("ru");

            Thread.CurrentThread.CurrentCulture = ci;
            base.Initialize(requestContext);
        }

        public override Cart GetCart()
        {
            int? id = GetCartID();
            if (id.HasValue)
            {
                int ID = GetCartID().Value;
                var cart = Repository.Carts.FirstOrDefault(p => p.ID == ID);

                if (cart != null)
                {
                    if (CurrentUser != null)
                    {
                        cart.Manager = CurrentUser;
                        cart.CreatedByManager = true;
                    }
                    return cart;
                }
            }
            var newCart = CreateCart();
            return newCart;
        }

        public string SaveImageFromUrl(string url)
        {
            var webClient = new WebClient();

            var bytes = webClient.DownloadData(url);
            var ms = new MemoryStream(bytes);

            var thumbUrl = string.Format("{0}{1}.jpg", VideoFolder, StringExtension.GenerateNewFile());
            var thumbSizes = Config.IconSizes.FirstOrDefault(c => c.Name == VideoThumbSize);
            if (thumbSizes != null)
            {
                var thumbSize = new Size(thumbSizes.Width, thumbSizes.Height);
                PreviewCreator.CreateAndSaveFitToSize(ms, thumbSize, Server.MapPath(thumbUrl));
            }
            return thumbUrl;
        }
    }
}
