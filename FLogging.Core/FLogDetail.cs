using System;
using System.Collections.Generic;
using System.Text;

namespace FLogging.Core
{
    public class FLogDetail
    {
        public FLogDetail()
        {

        }
        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }
        public string Product { get => product; set => product = value; }

        private string product;

    }
}
