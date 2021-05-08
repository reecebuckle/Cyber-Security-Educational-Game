using NUnit.Framework;
using Units;
using UnityEngine;

namespace Editor
{
    public class TestBypassShields 
    {
        /*
        * Tests how 0 damage is handled
        */
        [Test]
        public void TestOne()
        {
            //Initialise setting
            var unit = new Unit();
            
            unit.CurrentHp = 0;
            unit.CurrentDef = 0 ;
            unit.MaxDefence = 0;
            
            const int damage = 0;
            const int expectedHealth = 0;
            const int expectedDefence = 0;
            
            //Facilitate act
            unit.CalculateBypassDefence(damage);
            
            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        
        }
    
    
        /*
        * Tests a standard damage taking (0 defence but positive HP)
        */
        [Test]
        public void TestTwo()
        {
            //Initialise setting
            var unit = new Unit();
            
            unit.CurrentHp = 2;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;
            
            const int damage = 1;
            const int expectedHealth = 1;
            const int expectedDefence = 1;
            
            //Facilitate act
            unit.CalculateBypassDefence(damage);
            
            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        
        }
        
        
        /*
         * 
         */
        [Test]
        public void TestThree()
        {
            //Initialise setting
            var unit = new Unit();
            
            unit.CurrentHp = 2;
            unit.CurrentDef = 2;
            unit.MaxDefence = 2;
            
            const int damage = 2;
            const int expectedHealth = 0;
            const int expectedDefence = 2;
            
            //Facilitate act
            unit.CalculateBypassDefence(damage);
            
            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        
        }
        
        /*
         * Tests that damage greater than current health doesn't leave health negative
         */
        [Test]
        public void TestFour()
        {
            //Initialise setting
            var unit = new Unit();
            
            unit.CurrentHp = 2;
            unit.CurrentDef = 3;
            unit.MaxDefence = 3;
            
            const int damage = 3;
            const int expectedHealth = 0;
            const int expectedDefence = 3;
            
            //Facilitate act
            unit.CalculateBypassDefence(damage);
            
            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        
        }
        
        /*
        * Tests how negative damage is handled
        */
        [Test]
        public void TestFive()
        {
            //Initialise setting
            var unit = new Unit();
            
            unit.CurrentHp = 2;
            unit.CurrentDef = 3;
            unit.MaxDefence = 3;
            
            const int damage = -1;
            const int expectedHealth = 2;
            const int expectedDefence = 3;
            
            //Facilitate act
            unit.CalculateBypassDefence(damage);
            
            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        
        }
    }
}
