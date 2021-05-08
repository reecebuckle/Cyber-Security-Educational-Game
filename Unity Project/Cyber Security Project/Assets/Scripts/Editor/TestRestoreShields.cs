using NUnit.Framework;
using Units;

namespace Editor
{
    public class TestRestoreDefence
    {
        /*
         * When there's no gain
         */
        [Test]
        public void TestOne()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 0;
            unit.MaxDefence = 3;
            const int defenceGain = 0;
            const int expected = 0;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        [Test]
        public void TestTwo()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 0;
            unit.MaxDefence = 3;
            const int defenceGain = 1;
            const int expected = 1;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        [Test]
        public void TestThree()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 1;
            unit.MaxDefence = 3;
            const int defenceGain = 2;
            const int expected = 3;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        /*
        * When we reach the max defence threshold
        */
        [Test]
        public void TestFour()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 2;
            unit.MaxDefence = 3;
            const int defenceGain = 3;
            const int expected = 3;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        /*
        * When the unit can't even have defence
        */
        [Test]
        public void TestFive()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 0;
            unit.MaxDefence = 0;
            const int defenceGain = 1;
            const int expected = 0;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        /*
        * Checking we handle input properly!
        */
        [Test]
        public void TestSix()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 3;
            unit.MaxDefence = 3;
            const int defenceGain = -1;
            const int expected = 3;
            
            //Facilitate act
            unit.CalculateBoostDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
    }
}