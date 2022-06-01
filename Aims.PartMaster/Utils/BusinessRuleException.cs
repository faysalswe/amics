using System;

namespace Amics.Core.utils
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string msg) : base(msg)
        {
        }
    }
}
