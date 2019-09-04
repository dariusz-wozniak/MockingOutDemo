using Moq;
using NUnit.Framework;

namespace MockingOutDemo
{
    public class MockingOut
    {
        [Test]
        public void Test()
        {
            // Setup the mocking of out:
            var numberConverter = Mock.Of<INumberParser>();

            Mock.Get(numberConverter)
                .Setup(x => x.TryParse(It.IsAny<string>(), out It.Ref<INumber>.IsAny))
                .Returns(new TryParse((string s, out INumber n) =>
                {
                    n = Mock.Of<INumber>(num => num.Int32 == 456);
                    return true;
                }));

            // We can try to test our now...:
            var tryGet = numberConverter.TryParse("any number", out var number);

            // ...and check if values are properly set:
            Assert.That(tryGet, Is.True);
            Assert.That(number.Int32, Is.EqualTo(456));
        }

        [Test]
        public void Test2()
        {
            // Setup the mocking of out:
            var numberConverter = Mock.Of<INumberParser>();

            var mockedNumber = Mock.Of<INumber>();

            Mock.Get(numberConverter)
                .Setup(x => x.TryParse(It.IsAny<string>(), out mockedNumber))
                .Callback(() => mockedNumber.Int32 = 456)
                .Returns(true);

            // We can try to test our now...:
            var tryGet = numberConverter.TryParse("any number", out var number);

            // ...and check if values are properly set:
            Assert.That(tryGet, Is.True);
            Assert.That(number.Int32, Is.EqualTo(456));
        }

        private delegate bool TryParse(string s, out INumber n);
    }

    public interface INumberParser
    {
        bool TryParse(string numberToBeParsed, out INumber number);
    }

    public interface INumber
    {
        int Int32 { get; set; }
    }
}
