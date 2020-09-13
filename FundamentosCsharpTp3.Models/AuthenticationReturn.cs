using System;
using System.Collections.Generic;
using System.Text;

namespace FundamentosCsharpTp3.Models
{
    public class AuthenticationReturn
    {
        public bool Status { get; set; }
        public string Token { get; set; }
        public Guid Id { get; set; }
    }
}
