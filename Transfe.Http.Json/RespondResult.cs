using System;
using System.Collections.Generic;
using System.Text;

namespace Transfe.Http.Json
{
    public class RespondResult
    {
        public bool Result { get; set; } = true;
        public string Message { get; set; }
        public string[] Details { get; set; }
    }
}
