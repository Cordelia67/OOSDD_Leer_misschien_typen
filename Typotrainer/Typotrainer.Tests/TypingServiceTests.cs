using NUnit.Framework;
using Typotrainer.Core.Services;

namespace Typotrainer.Core.Tests
{
    [TestFixture]
    public class TypingServiceTests
    {
        private TypingService _typingService;

        // Setup runt voor elke test
        [SetUp]
        public void SetUp()
        {
            _typingService = new TypingService();
        }

        [Test]
        public void IsCorrectLetter_WithCorrectLetter_ReturnsTrue()
        {
            // Arrange 
            string correctZin = "Hallo";
            int index = 0;
            char typedChar = 'H';

            // Act 
            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            // Assert 
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsCorrectLetter_WithIncorrectLetter_ReturnsFalse()
        {
            // Arrange
            string correctZin = "Hallo";
            int index = 0;
            char typedChar = 'h'; // kleine letter in plaats van hoofdletter

            // Act
            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}