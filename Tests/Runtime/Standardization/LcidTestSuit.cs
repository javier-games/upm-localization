using System.Globalization;
using NUnit.Framework;

namespace BricksBucket.Global.Standardization.Tests
{
    /// <!-- LcidTestSuit -->
    /// 
    /// <summary>
    /// Test Suit for the LCID standard related Methods.
    /// </summary>
    /// 
    /// <seealso cref="Lcid"/>
    /// <seealso cref="Standard"/>
    /// <seealso href=
    /// "https://docs.microsoft.com/dotnet/api/system.globalization">
    /// System.Globalization</seealso>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
	public class LcidTestSuit
	{
        /// <summary>
        /// Tests the response to a not valid values in the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Lcid)"/></item>
        /// <item><see cref="Standard.GetCode(Lcid)"/></item>
        /// <item><see cref="Standard.GetIso639(Lcid)"/></item>
        /// <item><see cref="Standard.GetUnm49(Lcid)"/></item>
        /// <item><see cref="Standard.GetIso3166(Lcid)"/></item>
        /// <item><see cref="Standard.GetCultureInfo(Lcid)"/></item>
        /// <item><see cref=
        /// "Standard.GetLcid(Iso639, Unm49, Iso15924, string)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void FakeValuesTest ()
        {
            const Lcid fakeLcid = (Lcid) (-1);
            const Iso639 fakeLanguage = (Iso639) (-1);
            const Unm49 fakeRegion = (Unm49) (-1);
            const Iso15924 fakeScript = (Iso15924) (-1);
            
            // Test Name for a non LCID enum member.
            var name = Standard.GetName (fakeLcid);
            Assert.IsTrue (
                string.IsNullOrEmpty (name),
                "Invalid code for a non enum member (-1): " + name
            );
            
            // Test Code for a non LCID enum member.
            var code = Standard.GetCode (fakeLcid);
            Assert.IsTrue (
                string.IsNullOrEmpty (code),
                "Invalid code for a non enum member (-1): " + code
            );
            
            // Test get ISO 639 from a non LCID enum member.
            Assert.IsTrue (
                Standard.GetIso639 (fakeLcid) == Iso639.NONE,
                "Wrong ISO 639 from a non LCID value."
            );
            
            // Test get UN M.49 from a non LCID enum member.
            Assert.IsTrue (
                Standard.GetUnm49 (fakeLcid) == Unm49.NONE,
                "Wrong UN M.49 from a non LCID value."
            );
            
            // Test get ISO 639 from a non LCID enum member.
            Assert.IsTrue (
                Standard.GetIso3166 (fakeLcid) == Iso3166.NONE,
                "Wrong ISO ISO 3166 from a non LCID value."
            );
            
            // Test get Culture Info for non enum member-
            Assert.IsTrue (
                Standard.GetCultureInfo (fakeLcid) == null,
                "Wrong get culture method for non enum members."
            );

            // Test get LCID from non enum members.
            Assert.IsTrue (
                Standard.GetLcid (fakeLanguage) == Lcid.NONE,
                "Wrong LCID for a non ISO 639 enum member."
            );
            Assert.IsTrue (
                Standard.GetLcid (fakeLanguage, fakeRegion) == Lcid.NONE,
                "Wrong LCID for a non UN M.49 enum member."
            );
            Assert.IsTrue (
                Standard.GetLcid (fakeLanguage, Unm49.NONE, fakeScript) ==
                Lcid.NONE,
                "Wrong LCID for a non ISO 15924 enum member."
            );
            Assert.IsTrue (
                Standard.GetLcid (fakeLanguage, fakeRegion, fakeScript) ==
                Lcid.NONE,
                "Wrong LCID for a non enum members."
            );
        }
		
        /// <summary>
        /// Tests each enum member from the <see cref="Lcid">LCID</see>
        /// enum for the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetName(Lcid)"/></item>
        /// <item><see cref="Standard.GetCode(Lcid)"/></item>
        /// <item><see cref="Standard.GetCultureInfo(Lcid)"/></item>
        /// <item><see cref="Standard.GetLcid(string)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void LcidEnumTest ()
        {
            var values = System.Enum.GetValues (typeof (Lcid));
            for (int i = 0; i < values.Length; i++)
            {
                var value = (Lcid)values.GetValue (i);
                
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
                if (value == Lcid.NONE || value == Lcid.INVARIANT) continue;
                
                // Test Culture Info.
                var cultureInfo = Standard.GetCultureInfo (value);
                Assert.IsTrue (
                    Standard.GetLcid (cultureInfo.Name) == value,
                    value + " different from " + cultureInfo.Name
                );
            }
        }

        /// <summary>
        /// Tests each culture from the <see href="
        /// http://docs.microsoft.com/dotnet/api/system.globalization.cultureinfo
        /// ">Culture Info</see> class for the following methods.
        /// <list type="bullet">
        /// <item><see cref="Standard.GetLcid(string)"/></item>
        /// <item><see cref="Standard.GetCultureInfo(Lcid)"/></item>
        /// </list>
        /// </summary>
        [Test]
        public void CultureInfoTest()
        {
            var cultures = CultureInfo.GetCultures (CultureTypes.AllCultures);
            for (int i = 0; i < cultures.Length; i++)
            {
                var lcid = Standard.GetLcid (cultures[i].Name);
                var culture = Standard.GetCultureInfo (lcid);
                var lcidFromCulture = Standard.GetLcid (culture.Name);

                Assert.IsTrue (
                    lcid == lcidFromCulture,
                    "Wrong parse method.\n" +
                    cultures[i].Name + " -> " + lcid + "\n" +
                    culture.Name + " -> " + lcidFromCulture
                );
            }
        }
	}
}