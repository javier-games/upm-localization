using UnityEngine;

namespace BricksBucket.Localization.Standardization
{
	/// <!-- CustomPluralForm -->
	///
	/// <summary>
	/// Abstract <see href=
	/// "https://docs.unity3d.com/ScriptReference/ScriptableObject.html">
	/// Scriptable Object</see> Plural Form that gives the opportunity to
	/// create new plural forms by inherit from this class and override the
	/// <see cref="Count"/> property and the <see cref="Evaluate"/> method.
	/// </summary>
	///
	/// <seealso cref="BricksBucket.Localization.Standardization.PluralForm"/>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	public abstract class CustomPluralForm : ScriptableObject, IPluralForm
	{
		/// <inheritdoc cref="IPluralForm.Count"/>
		public virtual int Count => 0;
        
		/// <inheritdoc cref="IPluralForm.Evaluate(int)"/>
		public virtual int Evaluate (int n) => 0;
	}
}