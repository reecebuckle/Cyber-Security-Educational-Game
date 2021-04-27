using NUnit.Framework;
using Units;

namespace Editor
{
    public class TestTakeDamage
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
            unit.CurrentDef = 0;
            unit.MaxDefence = 0;

            const int damage = 0;
            const int expectedHealth = 0;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

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

            unit.CurrentHp = 1;
            unit.CurrentDef = 0;
            unit.MaxDefence = 0;

            const int damage = 0;
            const int expectedHealth = 1;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        [Test]
        public void TestThree()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 1;
            unit.CurrentDef = 0;
            unit.MaxDefence = 0;

            const int damage = 1;
            const int expectedHealth = 0;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        [Test]
        public void TestFour()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 2;
            unit.CurrentDef = 0;
            unit.MaxDefence = 0;

            const int damage = 1;
            const int expectedHealth = 1;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        /*
    * Tests how 0 hp but defence 1 or greater is handled
    */
        [Test]
        public void TestFive()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 0;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;

            const int damage = 1;
            const int expectedHealth = 0;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        [Test]
        public void TestSix()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 0;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;

            const int damage = 1;
            const int expectedHealth = 0;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        [Test]
        public void TestSeven()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 1;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;

            const int damage = 1;
            const int expectedHealth = 1;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        /*
     * Tests damage greater than positive shields, that health is deducted too
     */
        [Test]
        public void TestEight()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 2;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;

            const int damage = 2;
            const int expectedHealth = 1;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        /*
     * Tests 0 hp, and a strong damage (3), greater than defence (2)
     */
        [Test]
        public void TestNine()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 0;
            unit.CurrentDef = 2;
            unit.MaxDefence = 2;

            const int damage = 3;
            const int expectedHealth = 0;
            const int expectedDefence = 0;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }

        /*
     * Tests negative input
     */
        [Test]
        public void TestTen()
        {
            //Initialise setting
            var unit = new Unit();

            unit.CurrentHp = 1;
            unit.CurrentDef = 1;
            unit.MaxDefence = 1;

            const int damage = -1;
            const int expectedHealth = 1;
            const int expectedDefence = 1;

            //Facilitate act
            unit.CalculateDamage(damage);

            //Compare result
            Assert.That(unit.CurrentHp, Is.EqualTo((expectedHealth)));
            Assert.That(unit.CurrentDef, Is.EqualTo((expectedDefence)));
        }
    }
}