using System;

namespace Amics.Api.utils
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string msg) : base(msg)
        {
        }
    }
}
