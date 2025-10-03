using Grocery.Core.Helpers;
using System.Text;

namespace TestCore
{
    public class TestHelpers
    {
        [SetUp]
        public void Setup()
        {
        }


        //Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        //Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            string password = "user1";
            // geldige base64 salt en een dummy base64 hash van 32 bytes nullen
            string salt = Convert.ToBase64String(Encoding.UTF8.GetBytes("RandomSalt123456"));
            string wrongHash = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
            string storedHash = $"{salt}.{wrongHash}";

            Assert.IsFalse(PasswordHelper.VerifyPassword(password, storedHash));
        }

        [TestCase("user1")]
        [TestCase("user3")]
        [TestCase("userX")]
        public void TestPasswordHelperReturnsFalse(string password)
        {
            string salt = Convert.ToBase64String(Encoding.UTF8.GetBytes("RandomSalt123456"));
            string wrongHash = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
            string storedHash = $"{salt}.{wrongHash}";

            Assert.IsFalse(PasswordHelper.VerifyPassword(password, storedHash));
        }
    }
}