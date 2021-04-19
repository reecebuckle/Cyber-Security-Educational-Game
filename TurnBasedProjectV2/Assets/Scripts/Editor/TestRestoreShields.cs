using NUnit.Framework;
using Units;

namespace Editor
{
    public class TestRestoreDefence
    {
        [Test]
        public void TestOne()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 0;
            unit.MaxDefence = 3;
            var defenceGain = 0;
            var expected = 0;
            
            //Facilitate act
            unit.CalculateNewDefence(defenceGain);
            
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
            var defenceGain = 1;
            var expected = 1;
            
            //Facilitate act
            unit.CalculateNewDefence(defenceGain);
            
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
            var defenceGain = 2;
            var expected = 3;
            
            //Facilitate act
            unit.CalculateNewDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
        [Test]
        public void TestFour()
        {
            //Initialise setting
            var unit = new Unit();
            unit.CurrentDef = 0;
            unit.MaxDefence = 3;
            var defenceGain = 2;
            var expected = 1;
            
            //Facilitate act
            unit.CalculateNewDefence(defenceGain);
            
            //Compare result
            Assert.That(unit.CurrentDef, Is.EqualTo((expected)));
        }
        
    }
}