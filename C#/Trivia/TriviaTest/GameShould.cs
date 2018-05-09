using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using UglyTrivia;

namespace TriviaTest
{
    [TestFixture]
    class GameShould
    {
        private Game game;
        private StringBuilder fakeOutput;

        [SetUp]
        public void SetUp()
        {
            game = new Game();
            fakeOutput = new StringBuilder();
            Console.SetOut(new StringWriter(fakeOutput));
        }

        [Test]
        public void createRockQuestion()
        {
            Assert.AreEqual("Rock Question 1", game.createRockQuestion(1));
        }

        [Test]
        public void addANewPlayer()
        {
            game.add("My Player");

            Assert.AreEqual(1, game.howManyPlayers());
        }

        [Test]
        public void returnTrueWhenAddANewPlayer()
        {
            Assert.IsTrue(game.add(""));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void getHowManyPlayersAre(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                game.add("My Player");
            }

            Assert.AreEqual(numberOfPlayers, game.howManyPlayers());
        }

        [Test]
        public void logTheCurrentPlayerWhenRoll()
        {
            game.add("First player");

            game.roll(1);

            StringAssert.Contains("First player is the current player", fakeOutput.ToString());
        }

        [Test]
        public void logRollWhenRoll()
        {
            game.add("First player");

            game.roll(1);

            StringAssert.Contains("They have rolled a 1", fakeOutput.ToString());
        }

        [Test]
        public void shouldAdvanceLocationByRollAmountWhenThePlayerIsNotInPenaltyBox()
        {
            game.add("First player");

            game.roll(1);

            StringAssert.Contains("First player's new location is 1", fakeOutput.ToString());
        }

        [TestCase(0, "Pop")]
        [TestCase(1, "Science")]
        [TestCase(2, "Sports")]
        [TestCase(3, "Rock")]
        [TestCase(4, "Pop")]
        [TestCase(5, "Science")]
        [TestCase(6, "Sports")]
        [TestCase(7, "Rock")]
        [TestCase(8, "Pop")]
        [TestCase(9, "Science")]
        [TestCase(10, "Sports")]
        [TestCase(11, "Rock")]
        public void logCurrentCategoryBasedOnAdvancedLocation(int place, string expectedCategory)
        {
            game.add("First player");

            game.roll(place);

            StringAssert.Contains($"The category is {expectedCategory}", fakeOutput.ToString());
        }

        [Test]
        public void moveFromEleventhPositionToZero()
        {
            game.add("First player");

            game.roll(11);
            fakeOutput.Clear();
            game.roll(1);

            StringAssert.Contains("First player's new location is 0", fakeOutput.ToString());
        }

        [TestCase(0, "Pop")]
        [TestCase(1, "Science")]
        [TestCase(2, "Sports")]
        [TestCase(3, "Rock")]
        public void askAQuestionAboutTheCorrectCategoryWhenThePlayerRolls(int roll, string category)
        {
            game.add("First player");

            game.roll(roll);

            StringAssert.Contains($"{category} Question 0", fakeOutput.ToString());
        }

        [Test]
        public void askTwoQuestionsOnTheSameCategory()
        {
            game.add("First player");

            game.roll(1);
            game.roll(4);

            StringAssert.Contains("Science Question 0", fakeOutput.ToString());
            StringAssert.Contains("Science Question 1", fakeOutput.ToString());
        }

        [Test]
        public void sendTheCurrentPlayerToPenaltyBoxWhenTheAnswerIsWrong()
        {
            game.add("First player");

            game.wrongAnswer();

            StringAssert.Contains("First player was sent to the penalty box", fakeOutput.ToString());
        }

        [Test]
        public void changeToNextPlayerWhenAPlayerGivesAWrongAnswer()
        {
            game.add("First player");
            game.add("Second player");
            game.wrongAnswer();
            fakeOutput.Clear();
            game.roll(1);
            StringAssert.Contains("Second player is the current player", fakeOutput.ToString());
        }

        [Test]
        public void changeToFirstPlayerWhenLastPlayerGivesWrongAnswer()
        {
            game.add("First player");
            game.add("Second player");
            game.wrongAnswer();
            game.wrongAnswer();
            fakeOutput.Clear();
            game.roll(1);
            StringAssert.Contains("First player is the current player", fakeOutput.ToString());
        }
    }
}
