using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using turniri.Global.Config;

namespace turniri.Tools.Qiwi
{
    /// <summary>
    /// Qiwi invoice status XML request
    /// </summary>
    [XmlRoot("request")]
    public class QiwiInvoiceStatusRequest
    {
        public QiwiInvoiceStatusRequest()
        {
            this.ProtocolVersion = "4.00";
            this.RequestType = 33;
            this.Password = new QiwiExtraRequest();
            this.Password.Name = "password";
            this.Invoices = new List<QiwiInvoiceRequest>();
        }

        [XmlArray("bills-list")]
        [XmlArrayItem("bill")]
        public List<QiwiInvoiceRequest> Invoices { get; set; }

        [XmlElement("extra")]
        public QiwiExtraRequest Password { get; set; }

        [XmlElement("protocol-version")]
        public string ProtocolVersion { get; set; }

        [XmlElement("request-type")]
        public int RequestType { get; set; }

        [XmlElement("terminal-id")]
        public string TerminalId { get; set; }

        public string ToXml()
        {
            return Utilities.XMLSerialize<QiwiInvoiceStatusRequest>(this);
        }
    }

    public class QiwiExtraRequest
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class QiwiInvoiceRequest
    {
        [XmlAttribute("txn-id")]
        public string TransactionId { get; set; }
    }
}