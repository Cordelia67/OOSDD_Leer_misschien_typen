using NUnit.Framework;
using Moq;
using Typotrainer.Core.Services;
using Typotrainer.Core.Models;
using Typotrainer.Core.Interfaces;
using System.Text;

namespace Typotrainer.Tests
{
    [TestFixture]
    public class SentenceServiceTests
    {
        private Mock<IFileProvider> _mockFileProvider;
        private SentenceService _sentenceService;

        [SetUp]
        public void SetUp()
        {
            _mockFileProvider = new Mock<IFileProvider>();

            // Set up the mock to return test data for each difficulty
            SetupMockFileProvider("makkelijk.txt", "Makkelijke zin 1\nMakkelijke zin 2\nMakkelijke zin 3");
            SetupMockFileProvider("middelmatig.txt", "Middelmatige zin 1\nMiddelmatige zin 2/nMiddelmatige zin 3");
            SetupMockFileProvider("moeilijk.txt", "Moeilijke zin 1\nMoeilijke zin 2\nMoeilijke zin 3");

            _sentenceService = new SentenceService(_mockFileProvider.Object);
        }

        private void SetupMockFileProvider(string fileName, string content)
        {
            // Maak de string tot een stream, dit gebeurt in de normale code automatisch
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            _mockFileProvider
                .Setup(x => x.OpenAppPackageFileAsync(fileName))
                .ReturnsAsync(stream);
        }

        [Test]
        public void GetRandomSentence_Makkelijk_ReturnsEasySentence()
        {
            // Act
            string result = _sentenceService.GetRandomSentence(Difficulty.Easy);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.StartWith("Makkelijke zin"));
        }

        [Test]
        public void GetRandomSentence_Middelmatig_ReturnsMediumSentence()
        {
            string result = _sentenceService.GetRandomSentence(Difficulty.Medium);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.StartWith("Middelmatige zin"));
        }

        [Test]
        public void GetRandomSentence_Moeilijk_ReturnsHardSentence()
        {
            string result = _sentenceService.GetRandomSentence(Difficulty.Hard);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.StartWith("Moeilijke zin"));
        }

        [Test]
        public void GetRandomSentence_CalledMultipleTimes_ReturnsSentences()
        {
            for (int i = 0; i < 10; i++)
            {
                string result = _sentenceService.GetRandomSentence(Difficulty.Easy);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Does.StartWith("Makkelijke zin"));
            }
        }
    }
}
