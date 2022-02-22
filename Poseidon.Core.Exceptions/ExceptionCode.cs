using Poseidon.Common.Util;

namespace Poseidon.Core.Exceptions
{
    public class ExceptionCode : Enumeration
    {
        public static ExceptionCode ValidationException = new(1001, nameof(ValidationException));
        public static ExceptionCode UnexpectedException = new(1001, nameof(UnexpectedException));

        private ExceptionCode(int id, string name)
            : base(id, name)
        {
        }
    }
}