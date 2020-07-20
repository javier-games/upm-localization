namespace BricksBucket.Global.Standardization
{
	/// <!-- ScriptDirection -->
	/// 
	/// <summary>
	/// Enumerates types of directions to write scripts.
	/// </summary>
	/// 
	/// <seealso cref="Standard" />
	/// <seealso cref="Iso15924"/>
	/// 
	/// <!-- By Javier García | @jvrgms | 2020 -->
	[System.Flags]
	public enum ScriptDirection
	{
		//	No direction.
		NONE,
		//	Left to Right.
		// ReSharper disable once InconsistentNaming
		L2R,
		//	Right to Left.
		// ReSharper disable once InconsistentNaming
		R2L,
		//	Top to Bottom.
		// ReSharper disable once InconsistentNaming
		T2B,
		//	Bottom to Top.
		// ReSharper disable once InconsistentNaming
		B2T,
		//	Various or Mixed.
		VAR = L2R | R2L | T2B | B2T
	}
}