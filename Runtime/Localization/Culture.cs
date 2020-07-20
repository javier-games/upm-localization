using UnityEngine;
using BricksBucket.Global.Standardization;

namespace BricksBucket.Global.Localization
{
	/// <!-- Culture -->
	///
	/// <summary>
	/// The Culture struct contains the basic information to categorize
	/// localizations by its language, country and/or region with the facility
	/// to use standards by the <see href="https://www.iso.org">
	/// International Organization for Standardization</see>.
	/// </summary>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	[System.Serializable]
	public struct Culture
	{
		#region Fields

		/// <summary>
		/// Language code from the <see href=
		/// "https://www.iso.org/iso-639-language-codes.html">ISO 639-1</see>
		/// standard.
		/// </summary>
		[SerializeField]
		[Tooltip ("Language ISO-639 code.")]
		private Iso639 m_language;
		
		/// <summary>
		/// Specifies a different script from the default language script. 
		/// </summary>
		[SerializeField]
		[Tooltip ("Specifies a different script from the default script.")]
		private Iso15924 m_script;

		/// <summary>
		/// Area code used by the United Nations UN M49 standard.
		/// </summary>
		[SerializeField]
		[Tooltip ("Area UN M49 code.")]
		private Unm49 m_area;

		/// <summary>
		/// Specifies a region for the culture.
		/// </summary>
		[SerializeField]
		[Tooltip ("Specifies a region for the language.")]
		private string m_region;
		
		/// <summary>
		/// Custom plural form for the language.
		/// </summary>
		[SerializeField]
		[Tooltip ("Custom plural form for the language.")]
		private CustomPluralForm m_customPluralForm;
        
        #endregion
	}

	public enum CultureType
	{
		LCID,
		ISO639_UNM49,
		CUSTOM
	}
}