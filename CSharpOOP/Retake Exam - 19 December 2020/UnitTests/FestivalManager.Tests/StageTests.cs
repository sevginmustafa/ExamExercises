// Use this file for your unit tests.
// When you are ready to submit, REMOVE all using statements to Festival Manager (entities/controllers/etc)
// Test ONLY the Stage class. 
namespace FestivalManager.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class StageTests
    {
        [Test]
        [Category("MethodAddPerformer")]
        public void MethodAddPerformerShouldThrowExceptionIfAgeIsUnder18()
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = new Performer("Pesho", "Peshov", 16);

            //Act - Assert
            Assert.Throws<ArgumentException>(()
                => stage.AddPerformer(performer));
        }

        [Test]
        [Category("MethodAddPerformer")]
        public void MethodAddPerformerShouldThrowExceptionIfValueIsNull()
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = null;

            //Act - Assert
            Assert.Throws<ArgumentNullException>(()
                => stage.AddPerformer(performer));
        }

        [Test]
        [Category("MethodAddSong")]
        public void MethodAddSongShouldThrowExceptionIfDurationIsLessThan1()
        {
            //Arrange
            Stage stage = new Stage();
            Song song = new Song("Song", TimeSpan.FromSeconds(50));

            //Act - Assert
            Assert.Throws<ArgumentException>(()
                => stage.AddSong(song));
        }

        [Test]
        [Category("MethodAddSong")]
        public void MethodAddSongShouldThrowExceptionIfValueIsNull()
        {
            //Arrange
            Stage stage = new Stage();
            Song song = null;

            //Act - Assert
            Assert.Throws<ArgumentNullException>(()
                => stage.AddSong(song));
        }

        [Test]
        [TestCase("Gosho Goshov", "Song")]
        [TestCase("Gosho Goshov", "Song")]
        [TestCase("Pesho Peshov", "Song2")]
        [TestCase("Gosho Goshov", "Song2")]
        [Category("MethodAddSongToPerformer")]
        public void MethodAddSongToPerformerThrowExceptionIfOneOfTheGivenValuesDoesNotExist(string performerName, string songName)
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = new Performer("Pesho", "Peshov", 19);
            Song song = new Song("Song", TimeSpan.FromSeconds(500));

            //Act
            stage.AddPerformer(performer);
            stage.AddSong(song);

            //Act - Assert
            Assert.Throws<ArgumentException>(()
                => stage.AddSongToPerformer(songName, performerName));
        }

        [Test]
        [TestCase(null, "Song")]
        [TestCase("Pesho Peshov", null)]
        [Category("MethodAddSongToPerformer")]
        public void MethodAddSongToPerformerShouldThrowExceptionIfValueIsNull(string performerName, string songName)
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = new Performer("Pesho", "Peshov", 19);
            Song song = new Song("Song", TimeSpan.FromSeconds(500));

            //Act
            stage.AddPerformer(performer);
            stage.AddSong(song);

            //Assert
            Assert.Throws<ArgumentNullException>(()
                => stage.AddSongToPerformer(songName, performerName));
        }

        [Test]
        [Category("MethodAddSongToPerformer")]
        public void MethodAddSongToPerformerShouldReturnTheCorrectString()
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = new Performer("Pesho", "Peshov", 19);
            Song song = new Song("Song", TimeSpan.FromSeconds(500));

            //Act
            stage.AddPerformer(performer);
            stage.AddSong(song);
            var expectedResult = $"{song} will be performed by {performer}";
            var actualResult = stage.AddSongToPerformer("Song", "Pesho Peshov");

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("MethodPlay")]
        public void MethodPlayShouldReturnTheCorrectString()
        {
            //Arrange
            Stage stage = new Stage();
            Performer performer = new Performer("Pesho", "Peshov", 19);
            Song song = new Song("Song", TimeSpan.FromSeconds(500));

            //Act
            stage.AddPerformer(performer);
            stage.AddSong(song);
            stage.AddSongToPerformer("Song", "Pesho Peshov");
            var expectedResult = "1 performers played 1 songs";
            var actualResult = stage.Play();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}