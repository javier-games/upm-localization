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
		/// Code to identify localizations related to this culture.
		/// </summary>
		[SerializeField]
		[Tooltip ("Code to identify language category.")]
		private string m_code;

		/// <summary>
		/// Name of culture, useful to displays the culture instead fo its code.
		/// </summary>
		[SerializeField]
		[Tooltip ("Name for language category.")]
		private string m_name;

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
		/// Country code from the <see href=
		/// "https://www.iso.org/standard/63545.html">ISO 3166</see> standard.
		/// </summary>
		[SerializeField]
		[Tooltip ("Country ISO-3166")]
		private Iso3166 m_country;

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

		/// <summary>
		/// Specifies the direction from the script.
		/// </summary>
		[SerializeField]
		[Tooltip ("Specifies the direction of the text.")]
		private ScriptDirection m_direction;

		#endregion
	}

	public enum CultureType
	{
		LCID,
		ISO639_UNM49,
		CUSTOM
	}
}