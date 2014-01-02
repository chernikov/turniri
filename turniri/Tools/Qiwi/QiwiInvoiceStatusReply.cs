using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace turniri.Tools.Qiwi
{

    /// <summary>
    /// Qiwi invoice status XML reply
    /// </summary>
    [XmlRoot("response")]
    public class QiwiInvoiceStatusReply
    {
        public QiwiInvoiceStatusReply()
        {
            this.Result = new QiwiResultCode();
            this.Invoices = new List<QiwiInvoiceStatus>();
        }

        [XmlArray("bills-list")]
        [XmlArrayItem("bill")]
        public List<QiwiInvoiceStatus> Invoices { get; set; }
        [XmlElement("result-code")]
        public QiwiResultCode Result { get; set; }

        public static QiwiInvoiceStatusReply FromXml(string s)
        {
            return Utilities.XMLDeserialize<QiwiInvoiceStatusReply>(s);
        }

        public string ToXml()
        {
            return Utilities.XMLSerialize<QiwiInvoiceStatusReply>(this);
        }
    }

    public class QiwiResultCode
    {
        public enum ResultCode
        {
            /// <summary>
            /// Успех
            /// </summary>
            Success = 0,

            /// <summary>
            /// Сервер занят, повторите запрос позже
            /// </summary>
            Busy = 13,

            /// <summary>
            /// Ошибка авторизации (неверный логин/пароль)
            /// </summary>
            AuthorizationFailed = 150,

            /// <summary>
            /// Счет не найден
            /// </summary>
            InvoiceNotFound = 210,

            /// <summary>
            /// Счет с таким txn-id уже существует
            /// </summary>
            DuplicateInvoice = 215,

            /// <summary>
            /// Сумма слишком мала
            /// </summary>
            TooSmallAmount = 241,

            /// <summary>
            /// Превышена максимальная сумма платежа – 15 000р.
            /// </summary>
            MaximumAmountExceeded = 242,

            /// <summary>
            /// Превышение максимального интервала получения списка счетов
            /// </summary>
            MaximumIntervalExceeded = 278,

            /// <summary>
            /// Агента не существует в системе
            /// </summary>
            AgentNotFound = 298,

            /// <summary>
            /// Неизвестная ошибка
            /// </summary>
            UnknownError = 300,

            /// <summary>
            /// Ошибка шифрования
            /// </summary>
            EncryptError = 330,

            /// <summary>
            /// Не пройден контроль IP-адреса
            /// </summary>
            InvalidIp = 339,

            /// <summary>
            /// Превышено максимальное кол-во одновременно выполняемых запросов
            /// </summary>
            TooManyRequests = 370
        }

        [XmlIgnore]
        public ResultCode Code { get; set; }

        [XmlText]
        public int CodeXml
        {
            get { return (int)this.Code; }
            set { this.Code = (ResultCode)value; }
        }

        [XmlAttribute("fatal")]
        public Boolean Fatal { get; set; }
    }

    public class QiwiInvoiceStatus
    {
        public enum StatusEnum
        {
            NotSet = 0,

            /// <summary>
            /// Выставлен
            /// </summary>
            Issued = 50,

            /// <summary>
            /// Проводится
            /// </summary>
            Processing = 52,

            /// <summary>
            /// Оплачен
            /// </summary>
            Paid = 60,

            /// <summary>
            /// Отменен (ошибка на терминале)
            /// </summary>
            PendingByConsole = 150,

            /// <summary>
            /// Отменен (ошибка авторизации: недостаточно 
            /// средств на балансе, отклонен абонентом при 
            /// оплате с лицевого счета оператора сотовой связи 
            /// и т.п.).
            /// </summary>
            PendingByUser = 151,

            /// <summary>
            /// Отменен
            /// </summary>
            Cancelled = 160,

            /// <summary>
            /// Отменен (Истекло время)
            /// </summary>
            TimedOut = 161
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlIgnore]
        public StatusEnum Status { get; set; }

        [XmlAttribute("status")]
        public int StatusXml
        {
            get { return (int)this.Status; }
            set { this.Status = (StatusEnum)value; }
        }

        [XmlIgnore]
        public double Sum { get; set; }

        [XmlAttribute("sum")]
        public string SumXml
        {
            get
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";

                return String.Format(nfi, "{0:0.00}", this.Sum);
            }

            set
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                this.Sum = double.Parse(value, nfi);
            }
        }
    }
}