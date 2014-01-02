using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Tools
{
    [Serializable]
    public class ErrorSerialize
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ErrorSerialize()
        {
            this.TimeStamp = DateTime.Now;
        }

        public ErrorSerialize(string Message)
            : this()
        {
            this.Message = Message;
        }

        public ErrorSerialize(System.Exception ex)
            : this(ex.Message)
        {
            this.StackTrace = ex.StackTrace;
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}