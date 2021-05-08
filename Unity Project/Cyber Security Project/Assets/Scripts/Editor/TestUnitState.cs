using NUnit.Framework;
using Units;

namespace Editor
{
    public class TestUnitState
    {
        /*
         * Assert that unit - state changing methods are true
         */
        [Test]
        public void TestMissTurn()
        {
            var unit = new Unit();
            unit.ToggleMissTurn(true);
            Assert.That(unit.ShouldMissTurn, Is.EqualTo(true));
        }
        
        [Test]
        public void TestAttacked()
        {
            var unit = new Unit();
            unit.ToggleAttackedThisTurn(true);
            Assert.That(unit.AttackedThisTurn, Is.EqualTo(true));
        }
        
        [Test]
        public void TestMoved()
        {
            var unit = new Unit();
            unit.ToggleMovedThisTurn(true);
            Assert.That(unit.MovedThisTurn, Is.EqualTo(true));
        }
    }
}

