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
        public void IsCorrectLetter_CorrectLetter_ReturnsTrue()
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
        public void IsCorrectLetter_IncorrectLetter_ReturnsFalse()
        {
            string correctZin = "Hallo";
            int index = 0;
            char typedChar = 'h'; // kleine letter in plaats van hoofdletter

            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectLetter_NullString_ReturnsFalse()
        {
            string correctZin = null;
            int index = 0;
            char typedChar = 'H';

            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectLetter_EmptyLetter_ReturnsFalse()
        {
            string correctZin = "Hallo";
            int index = 0;
            char typedChar = ' ';

            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectLetter_NegativeIndex_ReturnsFalse()
        {
            string correctZin = "Hello";
            int index = -1;
            char typedChar = 'H';

            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectLetter_IndexOutOfRange_ReturnsFalse()
        {
            string correctZin = "Hello";
            int index = 10; // beyond the string length
            char typedChar = 'H';

            bool result = _typingService.IsCorrectLetter(correctZin, index, typedChar);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsCorrectLetter_CorrectFullWord_ReturnsTrue()
        {
            // Arrange
            string correctZin = "Test";

            // Act & Assert
            Assert.That(_typingService.IsCorrectLetter(correctZin, 0, 'T'), Is.True);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 1, 'e'), Is.True);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 2, 's'), Is.True);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 3, 't'), Is.True);
        }

        [Test]
        public void IsCorrectLetter_IncorrectFullWord_ReturnsExpectedResults()
        {
            string correctZin = "Test";

            Assert.That(_typingService.IsCorrectLetter(correctZin, 0, 'T'), Is.True);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 1, 'a'), Is.False);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 2, 's'), Is.True);
            Assert.That(_typingService.IsCorrectLetter(correctZin, 3, 't'), Is.True);
        }
    }
}