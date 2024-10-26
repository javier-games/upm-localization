using NUnit.Framework;

namespace BricksBucket.Global.Standardization.Tests
{
	/// <!-- Iso639TestSuit -->
	/// 
	/// <summary>
	/// Test Suit for the ISO 639 standard related Methods.
	/// </summary>
	/// 
	/// <seealso cref="Iso639"/>
	/// <seealso cref="Standard"/>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public class Iso639TestSuit
	{
        /// <summary>
        /// Tests the response to a not valid values in the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Iso639)"/></item>
        /// <item><see cref="Standard.GetCode(Iso639)"/></item>
        /// <item><see cref="Standard.GetIso639(string)"/></item>
        /// <item><see cref="Standard.GetIso15924(Iso639)"/></item>
        /// <item><see cref="Standard.GetPluralForm(Iso639)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void FakeValuesTest()
        {
            // Test Name for a non ISO 639 enum member.
            const Iso639 fakeValue = (Iso639) (-1);
            var name = Standard.GetName (fakeValue);
            Assert.IsTrue (
                string.IsNullOrEmpty (name),
                "Invalid code for not enum member (-1): " + name
            );
            
            // Test Code for a non ISO 639 enum member.
            var code = Standard.GetCode (fakeValue);
            Assert.IsTrue (
                string.IsNullOrEmpty (code),
                "Invalid code for not enum member (-1): " + code
            );
            
            // Test Parse for not valid ISO 639 enum member.
            Assert.IsTrue (
                Standard.GetIso639 ("Invalid Code") == Iso639.NONE,
                "Wrong parse method for not valid string."
            );
            
            // Test Parse for Lower Case code.
            Assert.IsTrue (
                Standard.GetIso639 ("en") == Iso639.EN,
                "Wrong parse method for \"en\"."
            );
            
            // Test Parse for Mixed Case code.
            Assert.IsTrue (
                Standard.GetIso639 ("eN") == Iso639.EN,
                "Wrong parse method for \"eN\"."
            );
            
            // Test Wrong ISO 15924 for not ISO 639 enum member.
            Assert.IsTrue (
                Standard.GetIso15924 (fakeValue) == Iso15924.NONE,
                "Wrong ISO 15924 for not enum member (-1)."
            );
        }
        
        
        /// <summary>
        /// Tests each enum member from the <see cref="Iso639">ISO 639</see>
        /// enum for the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Iso639)"/></item>
        /// <item><see cref="Standard.GetCode(Iso639)"/></item>
        /// <item><see cref="Standard.GetIso639(string)"/></item>
        /// <item><see cref="Standard.GetPluralForm(Iso639)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void Iso639EnumTest()
        {
            // Test for all elements in ISO 639 Enum.
            var values = System.Enum.GetValues (typeof (Iso639));
            for (int i = 0; i < values.Length; i++)
            {
                var iso = (Iso639)values.GetValue (i);
                
                // Test Name.
                var name = Standard.GetName (iso);
                Assert.IsFalse (
                    string.IsNullOrEmpty (name),
                    iso + " has not a name."
                );
                
                // Test Code.
                var code = Standard.GetCode (iso);
                Assert.IsFalse (
                    string.IsNullOrEmpty (code),
                    iso + " has not code."
                );
                
                // Test Parse.
                Assert.IsTrue(
                    iso == Standard.GetIso639 (code),
                    iso + " wrong parse method."
                );

                // Test Plural Form.
                var pluralForm = Standard.GetPluralForm (iso);
                if (pluralForm != null)
                {
                    Assert.IsTrue (
                        pluralForm.Evaluate (0) >= 0,
                        "Wrong plural form option value."
                    );
                }
            }
        }
    }
}