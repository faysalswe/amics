using System;

namespace Amics.web.utils
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string msg) : base(msg)
        {
        }
    }
}
