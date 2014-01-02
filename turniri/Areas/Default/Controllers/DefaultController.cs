using System.Linq;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Resources;
using turniri.Controllers;
using turniri.Global;
using turniri.Helpers;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools.Video;
using turniri.Tools.Mail;
using System.Collections.Generic;
using System;
using System.Text;

namespace turniri.Areas.Default.Controllers
{
    public abstract class DefaultController : BaseController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            var ci = Config.Culture;
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            base.Initialize(requestContext);
        }

        protected Comment CreateBasicComment(CommentView commentView)
        {
            var comment = (Comment)ModelMapper.Map(commentView, typeof(CommentView), typeof(Comment));
            if (!string.IsNullOrWhiteSpace(comment.VideoUrl))
            {
                comment.VideoCode = VideoHelper.GetVideoByUrl(comment.VideoUrl);
            }
            comment.UserID = CurrentUser.ID;
            comment.ID = 0;
            Repository.CreateComment(comment);
            return comment;
        }


        protected void SendNewPrivateMessage(Subject subject, Message message)
        {
            var receiver = Repository.Users.FirstOrDefault(p => p.ID == message.ReceiverID);

            if (receiver != null)
            {

                Repository.CreateMessage(message);
                NotifyMail.SendNotify(Config, "NewMessage", receiver.Email,
                                          (u, format) => string.Format(format, subject.Name, HostName),
                                          (u, format) => string.Format(format, subject.Name, HostName),
                                          receiver);
            }
        }

        protected void ProcessCart(Cart cart)
        {
            var listOfCodes = new List<ProductCode>();
            var guid = Guid.NewGuid();
            foreach (var item in cart.SubCartProducts)
            {
                if (item.Product.Type == (int)Product.TypeEnum.GoldMoney)
                {
                    if (cart.UserID != null)
                    {
                        var moneyDetail = new MoneyDetail()
                        {
                            UserID = cart.UserID,
                            CartID = cart.ID,
                            GlobalUniqueID = Identity,
                            SumGold = item.Quantity,
                            Description = "Покупка золотых ТИ",
                        };
                        Repository.CreateMoneyDetail(moneyDetail, guid);

                        var recharge = new Recharge()
                        {
                            UserID = cart.UserID,
                            MoneyDetailID = moneyDetail.ID,
                            Sum = moneyDetail.SumGold,
                            Description = "Покупка золотых ТИ",
                            IsSubmitted = true
                        };
                        Repository.CreateRecharge(recharge);
                    }
                }
                if (item.Product.IsCodeTyped)
                {
                    if (!item.ProductPrice.Preorder)
                    {
                        var codes = Repository.ProductCodes.Where(p => p.CartProductID == item.ID);

                        foreach (var code in codes)
                        {
                            Repository.SellProductCode(code);

                            if (code.Product.Type == (int)Product.TypeEnum.Code)
                            {
                                listOfCodes.Add(code);
                            }
                        }
                    }
                    else
                    {
                        //Уведомления всем продавцам
                        var sellerRole = Repository.Roles.FirstOrDefault(p => p.Code == "seller");

                        var sellers = Repository.Users.Where(p => p.UserRoles.Any(r => r.RoleID == sellerRole.ID));
                        if (sellers != null && sellers.Count() > 0 && cart.Customer != null)
                        {

                            var distribution = new Distribution()
                            {
                                UserID = cart.Customer.ID,
                                Subject = string.Format("Заказ №{0} оплачен", cart.ID),
                                Name = "Уведомления о заказе",
                                Body = cart.Description,
                                IsStart = true
                            };
                            Repository.CreateDistribution(distribution);

                            foreach (var seller in sellers)
                            {
                                if (!string.IsNullOrWhiteSpace(seller.Email))
                                {
                                    Repository.PushMail(new Model.Mail()
                                    {
                                        UserID = seller.ID,
                                        DistributionID = distribution.ID,
                                        Email = seller.Email,
                                        Subject = distribution.Subject,
                                        Body = distribution.Body
                                    });
                                }
                            }

                        }


                    }
                }
            }
            Repository.SubmitMoney(guid);

            if (listOfCodes != null && listOfCodes.Count > 0)
            {
                SendMailOfCodes(cart.ID, cart.Email, listOfCodes);
            }

            //TODO: Тут товары которые мы шлем реально
            cart.OrderType = (int)Cart.OrderTypes.Delivered;

            Repository.UpdateCart(cart);
            CreateCart();
        }


    }
}
