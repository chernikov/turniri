using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Serialization;
using turniri.Global.Config;
using turniri.Model;

namespace turniri.Tools.Qiwi
{
    /// <summary>
    /// Job which runs regularily in the service and checks invoice statuses
    /// </summary>
    public class QiwiProcessorJob
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Start(IRepository repository, IConfig config)
        {
            MoneyNotify moneyNotify = null;
            try
            {
                QiwiInvoiceStatusRequest request = new QiwiInvoiceStatusRequest();
                request.Password.Value = config.QiwiPassword;
                request.TerminalId = config.QiwiID;
                var recharges = repository.Recharges.Where(p => p.Provider == (int)Recharge.ProviderType.Qiwi && !p.IsSubmitted);
                if (recharges.Any())
                {
                    request.Invoices = recharges.Select(p => new QiwiInvoiceRequest()
                    {
                        TransactionId = p.ID.ToString()
                    }).ToList();

                    HttpFetch http = new HttpFetch("http://ishop.qiwi.ru/xml");
                    http.PostData = request.ToXml();
                    string replyData = http.Process();

                    moneyNotify = new MoneyNotify()
                    {
                        AddedDate = DateTime.Now,
                        Type = (int)MoneyNotify.TypeEnum.Qiwi,
                        Data = replyData,
                        IsSuccess = null
                    };
                    repository.CreateMoneyNotify(moneyNotify);

                    QiwiInvoiceStatusReply reply = QiwiInvoiceStatusReply.FromXml(replyData);
                    if (reply.Result.Code == QiwiResultCode.ResultCode.Success)
                    {
                        if (reply.Invoices.Any())
                        {
                            foreach (QiwiInvoiceStatus invoice in reply.Invoices)
                            {
                                if (invoice.Status == QiwiInvoiceStatus.StatusEnum.Paid)
                                {
                                    var id = Int32.Parse(invoice.Id);

                                    var recharge = repository.Recharges.FirstOrDefault(p => p.ID == id);
                                    if (recharge != null && !recharge.IsSubmitted)
                                    {
                                        try
                                        {
                                            if (recharge.CartID.HasValue)
                                            {

                                                var webClient = new WebClient();
                                                var result = webClient.DownloadString(string.Format("{0}/Cart/ProcessQiwiRecharge/{1}", config.Host, recharge.ID));
                                                logger.Debug(string.Format("{0} : {1}", recharge.ID, result));
                                            }
                                            else
                                            {
                                                var moneyDetail = new MoneyDetail()
                                                {
                                                    UserID = recharge.UserID,
                                                    Description = recharge.Description,
                                                    SumGold = recharge.Sum
                                                };
                                                var moneyFee = repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeQiwi);
                                                if (moneyFee != null)
                                                {
                                                    moneyDetail.MoneyFeeID = moneyFee.ID;
                                                    moneyDetail.SumGold = moneyDetail.SumGold * (1 - moneyFee.PercentFee / 100);
                                                }
                                                var guid = Guid.NewGuid();
                                                repository.CreateMoneyDetail(moneyDetail, guid);
                                                recharge.MoneyDetailID = moneyDetail.ID;
                                                repository.SubmitMoney(guid);
                                            }
                                            recharge.IsSubmitted = true;
                                            repository.UpdateRecharge(recharge);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex.Message);
                                        }
                                    }
                                }
                                else if (invoice.Status == QiwiInvoiceStatus.StatusEnum.PendingByConsole ||
                                   invoice.Status == QiwiInvoiceStatus.StatusEnum.PendingByUser ||
                                   invoice.Status == QiwiInvoiceStatus.StatusEnum.Cancelled ||
                                   invoice.Status == QiwiInvoiceStatus.StatusEnum.TimedOut)
                                {
                                    var id = Int32.Parse(invoice.Id);
                                    var recharge = repository.Recharges.FirstOrDefault(p => p.ID == id);
                                    if (recharge != null && !recharge.IsSubmitted)
                                    {
                                        repository.RemoveRecharge(recharge.ID);
                                    }
                                }
                            }
                        }
                        else
                        {
                            repository.RemoveMoneyNotify(moneyNotify.ID);
                            moneyNotify = null;
                        }
                    }
                    else
                    {
                        throw new WebException("Result Code = " + reply.Result.Code.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                moneyNotify.IsSuccess = false;
                try
                {
                    var serialize = new XmlSerializer(typeof(ErrorSerialize));
                    var stringWriter = new StringWriter();
                    serialize.Serialize(stringWriter, new ErrorSerialize(ex));
                    stringWriter.Flush();
                    moneyNotify.Exception = stringWriter.ToString();
                }
                catch
                {
                    moneyNotify.Exception = "Too Short: " +ex.Message;
                }
            }
            finally
            {
                if (moneyNotify != null)
                {
                    repository.UpdateMoneyNotify(moneyNotify);
                }
            }
        }
        
    }
}