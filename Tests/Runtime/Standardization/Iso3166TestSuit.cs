using NUnit.Framework;

namespace BricksBucket.Global.Standardization.Tests
{
    /// <!-- Iso3166TestSuit -->
    /// 
    /// <summary>
    /// Test Suit for the ISO 3166 standard related Methods.
    /// </summary>
    /// 
    /// <seealso cref="Iso3166"/>
    /// <seealso cref="Standard"/>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
	public class Iso3166TestSuit
	{
        /// <summary>
        /// Tests the response to a not valid values in the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Iso3166)"/></item>
        /// <item><see cref="Standard.GetCode(Iso3166)"/></item>
        /// <item><see cref="Standard.GetIso3166(string)"/></item>
        /// <item><see cref="Standard.GetUnm49(Iso3166)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void FakeValuesTest ()
        {
            const Iso3166 wrongValue = (Iso3166) (-1);
            
            // Test Name for a non ISO 3166 enum member.
            var name = Standard.GetName (wrongValue);
            Assert.IsTrue (
                string.IsNullOrEmpty (name),
                "Invalid code for not enum member (-1): " + name
            );
            
            // Test Code for a non ISO 3166 enum member.
            var code = Standard.GetCode (wrongValue);
            Assert.IsTrue (
                string.IsNullOrEmpty (code),
                "Invalid code for not enum member (-1): " + code
            );
            
            // Test Parse for not valid ISO 3166 enum member.
            Assert.IsTrue (
                Standard.GetIso3166 ("Invalid Code") == Iso3166.NONE,
                "Wrong parse method for not valid string."
            );
            
            // Test Parse for Lower Case code.
            Assert.IsTrue (
                Standard.GetIso3166 ("mx") == Iso3166.MX,
                "Wrong parse method for \"mx\"."
            );
            
            // Test Parse for Mixed Case code.
            Assert.IsTrue (
                Standard.GetIso3166 ("mX") == Iso3166.MX,
                "Wrong parse method for \"mX\"."
            );
            
            // Test Wrong ISO 15924 for not ISO 3166 enum member.
            Assert.IsTrue (
                Standard.GetUnm49 (wrongValue) == Unm49.NONE,
                "Wrong UN M.49 for not enum member (-1)."
            );
        }
        
        /// <summary>
        /// Tests each enum member from the <see cref="Iso3166">ISO 3166</see>
        /// enum for the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Iso3166)"/></item>
        /// <item><see cref="Standard.GetCode(Iso3166)"/></item>
        /// <item><see cref="Standard.GetIso3166(string)"/></item>
        /// <item><see cref="Standard.GetUnm49(Iso3166)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void Iso3166EnumTest ()
        {
            var values = System.Enum.GetValues (typeof (Iso3166));
            for (int i = 0; i < values.Length; i++)
            {
                var value = (Iso3166)values.GetValue (i);
                
                // Test UN M.49 value.
                // Every ISO 3166 MUST have a UN M.49 value.
                if(value != Iso3166.NONE)
                    Assert.IsTrue (
                        Standard.GetUnm49 (value) != Unm49.NONE,
                        value + " has not an UN M.49 standard value."
                    );
                
                // Test Name.
                var name = Standard.GetName (value);
                Assert.IsFalse (
                    string.IsNullOrEmpty (name),
                    value + " has not a name."
                );
                
                // Test Code.
                var code = Standard.GetCode (value);
                Assert.IsFalse (
                    string.IsNullOrEmpty (code),
                    value + " has not code."
                );
                
                // Test Parse.
                Assert.IsTrue (
                    value == Standard.GetIso3166 (code),
                    value + " wrong parse method."
                );
            }
        }
	}
}