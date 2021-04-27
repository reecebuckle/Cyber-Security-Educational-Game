using NUnit.Framework;
using Units;

namespace Editor
{
    public class TestAPChange 
    {
    
        /*
     * Tests a negative value - should return a log error and be set to 0
     */
        [Test]
        public void TestIncrementOne()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = -10;
            const int expectedAP = 3;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * Tests a negative value - should return a log error and be set to 0
     */
        [Test]
        public void TestIncrementTwo()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = -5;
            const int expectedAP = 3;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));;
        }
    
        /*
     * Standard positive tests
     */
        [Test]
        public void TestIncrementThree()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = 0;
            const int expectedAP = 3;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * 
     */
        [Test]
        public void TestIncrementFour()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = 2;
            const int expectedAP = 5;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        [Test]
        public void TestIncrementFive()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = 3;
            const int expectedAP = 6;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        [Test]
        public void TestIncrementSix()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = 10;
            const int expectedAP = 6;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * Tests a negative value - should return a log error and be set to 0
     */
        [Test]
        public void TestDecrementOne()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int decrement = -10;
            const int expectedAP = 3;
        
            // Carry out function
            unit.DecrementActionPoints(decrement);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * Tests a negative value - should return a log error and be set to 0
     */
        [Test]
        public void TestDecrementTwo()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int increment = -5;
            const int expectedAP = 3;
        
            // Carry out function
            unit.IncrementActionPoints(increment);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));;
        }
    
        /*
     * Standard positive tests
     */
        [Test]
        public void TestDecrementThree()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int decrement = 0;
            const int expectedAP = 3;
        
            // Carry out function
            unit.DecrementActionPoints(decrement);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * 
     */
        [Test]
        public void TestDecrementFour()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int decrement = 2;
            const int expectedAP = 1;
        
            // Carry out function
            unit.DecrementActionPoints(decrement);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        [Test]
        public void TestDecrementFive()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int decrement = 3;
            const int expectedAP = 0;
        
            // Carry out function
            unit.DecrementActionPoints(decrement);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    
        /*
     * Should not be possible - but these booleans are tested when a move is tested to have enough AP to begin with
     */
        [Test]
        public void TestDecrementSix()
        {
            //Instantiate variables
            var unit = new Unit();
            unit.ActionPoints = 3;
            unit.MaxActionPoints = 6;
        
            const int decrement = 10;
            const int expectedAP = 0;
        
            // Carry out function
            unit.DecrementActionPoints(decrement);
        
            // Assert result
            Assert.That(unit.ActionPoints, Is.EqualTo(expectedAP));
        }
    }
}
