using System.Collections;
using System.Timers;

namespace Invocie.api.Tests.TestModels
{
    public class PassDatausingMemberData : IEnumerable<object[]>
    {
        public static IEnumerable<object[]> PassDataToSuccess()
        {

            return new List<object[]>
            {
                new object[] {1},
                new object[] {2}
            };

        }

        public static IEnumerable<object[]> PassDatatofailed()
        {

            return new List<object[]>
            {
                new object[] {3}

            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return (IEnumerator<object[]>)PassDataToSuccess();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
