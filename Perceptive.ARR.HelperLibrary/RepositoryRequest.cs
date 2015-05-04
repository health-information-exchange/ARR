using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptive.ARR.HelperLibrary
{
    public class RepositoryRequest
    {
        public MessageProtocol Protocol { get; set; }
        public string IP { get; set; }
        public byte[] Data { get; set; }
        public int ByteCount { get; set; }
        public SyslogMessage ValidMessage { get; set; }
        public DateTime RequestReceiveDateTime { get; set; }

        public RepositoryRequest()
        {
            RequestReceiveDateTime = DateTime.Now;
        }
    }
}
