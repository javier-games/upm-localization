/*using System.Collections.Generic;
using BricksBucket.Core;
using BricksBucket.Core.Attributes;
using UnityEngine;
using UnityEngine.Video;
//using Sirenix.OdinInspector;
using BricksBucket.Localization.Internal;

#if UNITY_EDITOR
//using BricksBucket.Core.Editor;
using UnityEditor;
#endif


// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable TailRecursiveCall
namespace BricksBucket.Localization
{
    using ScriptableSingleton = Core.ScriptableSingleton<LocalizationSettings>;
    
    /// <!-- LocalizationSettings -->
    /// 
    /// <summary>
    /// Class that administrates the cultures, books and its localizations
    /// within settings related to them. Access point for the localizations
    /// values only editable on inspector.
    /// </summary>
    /// 
    /// <seealso href=
    /// "https://docs.unity3d.com/ScriptReference/ScriptableObject.html">
    /// Scriptable Object.</seealso>
    /// <seealso cref="Core.ScriptableSingleton{T}"/>
    /// <seealso cref="BricksBucket.Localization.Book"/>
    /// <see cref="BricksBucket.Localization.Culture"/>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    public class LocalizationSettings : Core.ScriptableSingleton<LocalizationSettings>
    {

        
        #region Fields

        /// <summary>
        /// Name of the Project.
        /// </summary>
        [SerializeField]
        [Tooltip ("Name of the LocalizedObject Project.")]
        private string _projectName;

        /// <summary>
        /// Whether to use default culture for default values.
        /// </summary>
        [Tooltip ("Whether to use default culture for default values.")]
        [SerializeField]
        private bool _useDefaultCulture = true;

        /// <summary>
        /// Collection of categories of languages.
        /// </summary>
        [SerializeField, ReadOnly]
        //[ListDrawerSettings (IsReadOnly = true, NumberOfItemsPerPage = 4)]
        [Tooltip ("Collection of cultures.")]
        private List<Culture> _cultures;

        /// <summary>
        /// Names of cultures in the cultures collection. Serialized Field
        /// dedicated for the culture names to avoid garbage collection and
        /// speed up the process to get an array of names.
        /// </summary>
        [SerializeField, HideInInspector]
        private string[] _culturesNames;

        /// <summary>
        /// Codes of cultures in the cultures collection. Serialized Field
        /// dedicated for the culture codes to avoid garbage collection and
        /// speed up the process to get an array of codes.
        /// </summary>
        [SerializeField, HideInInspector]
        private string[] _culturesCodes;

        /// <summary>
        /// Dictionary of books by codes.
        /// </summary>
        [SerializeField, ReadOnly]
        //[DictionaryDrawerSettings (KeyLabel = "Code", ValueLabel = "Book",
        //    IsReadOnly = true)]
        [Tooltip ("Collection of books")]
        private BooksDictionary _books;

        /// <summary>
        /// Names of books in the books dictionary. Serialized Field
        /// dedicated for the books names to avoid garbage collection and
        /// speed up the process to get an array of names.
        /// </summary>
        [SerializeField, HideInInspector]
        private string[] _booksNames;
        
        /// <summary>
        /// Codes of books in the books dictionary. Serialized Field
        /// dedicated for the books codes to avoid garbage collection and
        /// speed up the process to get an array of codes.
        /// </summary>
        [SerializeField, HideInInspector]
        private string[] _booksCodes;

#if UNITY_EDITOR
        /// <summary>
        /// Mask for the Edit Localizations Window Filter.
        /// </summary>
        [SerializeField]
        internal int _windowDataTypeMask = 1;

        /// <summary>
        /// Mask for the Edit Localizations Window Filter.
        /// </summary>
        [SerializeField]
        internal int _windowCultureMask = ~0b0;

        /// <summary>
        /// Index of the Edit Localizations Window active Book.
        /// </summary>
        [SerializeField]
        internal int _windowActiveBook;
#endif
        #endregion

        
        #region Instance Properties

        /// <summary>
        /// List of cultures categories.
        /// </summary>
        private List<Culture> CulturesList =>
            _cultures ??
            (_cultures = new List<Culture> ());

        /// <summary>
        /// Dictionary of books.
        /// </summary>
        private BooksDictionary BooksDictionary =>
            _books ?? (_books = new BooksDictionary ());
        
        /// <summary>
        /// Names of cultures in the cultures collection. Serialized Field
        /// dedicated for the culture names to avoid garbage collection and
        /// speed up the process to get an array of names.
        /// </summary>
        private string[] CulturesNamesArray
        {
            get
            {
                if (_culturesNames == null) UpdateCultureArrays ();
                return _culturesNames;
            }
            set => _culturesNames = value;
        }

        /// <summary>
        /// Codes of cultures in the cultures collection. Serialized Field
        /// dedicated for the culture codes to avoid garbage collection and
        /// speed up the process to get an array of codes.
        /// </summary>
        private string[] CulturesCodesArray
        {
            get
            {
                if(_culturesCodes == null) UpdateCultureArrays ();
                return _culturesCodes;
            }
            set => _culturesCodes = value;
        }
        
        /// <summary>
        /// Names of books in the books dictionary. Serialized Field
        /// dedicated for the books names to avoid garbage collection and
        /// speed up the process to get an array of names.
        /// </summary>
        private string[] BooksNamesArray
        {
            get
            {
                if(_booksNames == null) UpdateBooksArrays ();
                return _booksNames;
            }
            set => _booksNames = value;
        }

        /// <summary>
        /// Codes of books in the books dictionary. Serialized Field
        /// dedicated for the books codes to avoid garbage collection and
        /// speed up the process to get an array of codes.
        /// </summary>
        private string[] BooksCodesArray
        {
            get
            {
                if(_booksCodes == null) UpdateBooksArrays ();
                return _booksCodes;
            }
            set => _booksCodes = value;
        }

        #endregion
        

        #region Static Properties

        /// <summary>
        /// Name of the LocalizedObject Project.
        /// </summary>
        /// <returns>Name of the project.</returns>
        public static string ProjectName => Instance._projectName;

        /// <summary>
        /// If it is set to <value>true</value> then when a localization
        /// is requested for a certain culture and the localization is
        /// equals or equivalent to null then returns the value of the default
        /// culture.
        /// </summary>
        /// <returns>Whether to use default culture for localizations that are
        /// equals or equivalent to null in a certain culture.
        /// </returns>
        public static bool UseDefaultCulture
        {
            get => Instance._useDefaultCulture;
            set => Instance._useDefaultCulture = value;
        }

        /// <summary>
        /// Default culture to use for a localization instead of a culture with
        /// an empty or null value for the localization.
        /// </summary>
        /// <returns>Returns an empty culture if the list of cultures is
        /// empty.</returns>
        public static Culture DefaultCulture =>
            Instance.CulturesList.Count > 0
                ? Instance.CulturesList[0]
                : default;

        /// <summary>
        /// Array of the cultures registered in the project.
        /// <div class="WARNING">
        /// <h5>WARNING</h5>
        /// <p>The usage of this property generates garbage collection. Consider
        /// use the properties <see cref="CulturesNames"/> and <see cref=
        /// "CulturesCodes"/> within the <see cref="GetCulture"/> method.</p>
        /// </div>
        /// </summary>
        /// <returns>Array of cultures.</returns>
        public static Culture[] Cultures => Instance.CulturesList.ToArray ();
        
        /// <summary>
        /// Array with the codes of the cultures in the localization settings.
        /// </summary>
        /// <returns>Array of codes.</returns>
        public static string[] CulturesCodes
        {
            get => Instance.CulturesCodesArray;
            private set => Instance.CulturesCodesArray = value;
        }
        
        /// <summary>
        /// Array with the names of the cultures in the localization settings.
        /// </summary>
        /// <returns>Array of names.</returns>
        public static string[] CulturesNames
        {
            get => Instance.CulturesNamesArray;
            private set => Instance.CulturesNamesArray = value;
        }

        /// <summary>
        /// Array of the books registered in the project.
        /// <div class="WARNING">
        /// <h5>WARNING</h5>
        /// <p>The usage of this property generates garbage collection. Consider
        /// use the properties <see cref="BooksNames"/> and <see cref=
        /// "BooksCodes"/> within the <see cref="GetBook"/> method.</p>
        /// </div>
        /// </summary>
        /// <returns>Array of cultures.</returns>
        public static Book[] Books
        {
            get
            {
                var booksDictionary = Instance.BooksDictionary;
                var books = new Book[booksDictionary.Count];

                var i = 0;
                foreach (var key in booksDictionary.Keys)
                {
                    books[i] = booksDictionary[key];
                    i++;
                }
                
                return books;
            }
        }

        /// <summary>
        /// Array with the codes of the books in the localization settings.
        /// </summary>
        /// <returns>Array of codes.</returns>
        public static string[] BooksCodes
        {
            get => Instance.BooksCodesArray;
            private set => Instance.BooksCodesArray = value;
        }

        /// <summary>
        /// Array with the names of the books in the localization settings.
        /// </summary>
        /// <returns>Array of names.</returns>
        public static string[] BooksNames
        {
            get => Instance.BooksNamesArray;
            private set => Instance.BooksNamesArray = value;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Mask for the Edit Localizations Window Filter.
        /// </summary>
        internal static int WindowDataTypeMask
        {
            get => Instance._windowDataTypeMask;
            set => Instance._windowDataTypeMask = value;
        }

        /// <summary>
        /// Mask for the Edit Localizations Window Filter.
        /// </summary>
        internal static int WindowCultureMask
        {
            get => Instance._windowCultureMask;
            set => Instance._windowCultureMask = value;
        }

        /// <summary>
        /// Index of the Edit Localizations Window active Book.
        /// </summary>
        internal static int WindowActiveBook
        {
            get
            {
                if (Instance._windowActiveBook >= BooksNames.Length)
                    Instance._windowActiveBook = BooksNames.Length - 1;
                return Instance._windowActiveBook;
            }
            set => Instance._windowActiveBook = value;
        }
#endif
        
        #endregion

        
        #region Instance Methods
        
        /// <summary>
        /// Evaluates if the language code is in settings.
        /// </summary>
        /// <param name="code">Language code to evaluate.</param>
        /// <returns>Whether the language code is in settings.</returns>
        internal bool ContainsCulture (string code) =>
            CulturesList.Exists (culture => culture.Code == code);

        /// <summary>
        /// Returns the count of cultures in settings.
        /// </summary>
        /// <returns></returns>
        internal int GetCulturesCount () => CulturesList.Count;
        
        /// <summary>
        /// Gets the culture at the given index.
        /// </summary>
        /// <param name="index">Index of the culture to return.</param>
        /// <param name="culture">Culture found at the given index.</param>
        /// <returns>Whether a culture was found.</returns>
        internal bool GetCultureByIndex (int index, out Culture culture)
        {
            if (index >= CulturesList.Count || index < 0)
            {
                culture = new Culture ();
                return false;
            }

            culture = CulturesList[index];
            return true;
        }

        /// <summary>
        /// Sets an existing culture by the new one.
        /// </summary>
        /// <param name="index">Index of the culture to set.</param>
        /// <param name="culture">Updated culture.</param>
        internal void SetCulture (int index, Culture culture)
        {
            if (index >= CulturesList.Count || index < 0) return;
            var oldCode = CulturesList[index].Code;
            CulturesList.RemoveAt (index);
            CulturesList.Insert (index, culture);

            foreach (var book in BooksDictionary.Values)
                book.UpdateCulture (oldCode, culture.Code);

            UpdateCultureArrays ();
        }

        /// <summary>
        /// Adds a new culture to the list of cultures.
        /// </summary>
        /// <param name="culture">Culture to Add.</param>
        internal void AddCulture (Culture culture)
        {
            CulturesList.Add (culture);
            foreach (var book in BooksDictionary.Values)
                book.AddNewCulture (culture.Code);
            UpdateCultureArrays ();
        }

        /// <summary>
        /// Removes a culture by its index.
        /// </summary>
        /// <param name="index">Index of the culture to remove.</param>
        internal void RemoveCulture (int index)
        {
            var culture = CulturesList[index];
            CulturesList.RemoveAt (index);
            foreach (var book in BooksDictionary.Values)
                book.RemoveCulture (culture.Code);
            UpdateCultureArrays ();
        }

        /// <summary>
        /// Sets the default culture.
        /// </summary>
        /// <param name="index">Index of the new default culture.</param>
        internal void SetDefaultCulture (int index)
        {
            var newOne = CulturesList[index];
            CulturesList.Remove (newOne);
            CulturesList.Insert (0, newOne);
            UpdateCultureArrays ();
        }

        /// <summary>
        /// Updates Culture Serialized Arrays.
        /// </summary>
        internal void UpdateCultureArrays ()
        {
            var cultures = CulturesList;

            var culturesNames = new string[cultures.Count];
            var culturesCodes = new string[cultures.Count];

            for (int i = 0; i < cultures.Count; i++)
            {
                culturesNames[i] = cultures[i].Name;
                culturesCodes[i] = cultures[i].Code;
            }

            CulturesNames = culturesNames;
            CulturesCodes = culturesCodes;
        }

        /// <summary>
        /// Gets the count of books in books dictionary.
        /// </summary>
        /// <returns>Count of books in books dictionary.</returns>
        internal int GetBooksCount () => BooksDictionary.Count;

        /// <summary>
        /// Evaluates if the dictionary of books contains the key code.
        /// </summary>
        /// <param name="code">Code to look for.</param>
        /// <returns>Whether the dictionary contains the book code.</returns>
        internal bool ContainsBook (string code) =>
            BooksDictionary.ContainsKey (code);

        /// <summary>
        /// Adds a new book by code.
        /// </summary>
        /// <param name="code">Code where to add book.</param>
        /// <param name="book">Book to add.</param>
        internal void AddBook (string code, Book book)
        {
            BooksDictionary.Add (code, book);
            UpdateBooksArrays ();
        }

        /// <summary>
        /// Removes a book by code.
        /// </summary>
        /// <param name="code">Code of the book to remove.</param>
        internal void RemoveBook (string code)
        {
            var book = BooksDictionary[code];
            BooksDictionary.Remove (code);
#if UNITY_EDITOR
            /*
            if(!BooksDictionary.ContainsValue (book))
                book.TryDestroyImmediate ();
            *
#endif
            UpdateBooksArrays ();
        }
        
        /// <summary>
        /// Updates Books Serialized Arrays.
        /// </summary>
        internal void UpdateBooksArrays ()
        {
            var books = BooksDictionary;

            var culturesNames = new string[books.Count];
            var culturesCodes = new string[books.Count];

            var i = 0;
            foreach (var key in BooksDictionary.Keys)
            {
                culturesNames[i] = books[key].Name;
                culturesCodes[i] = books[key].Code;
                i++;
            }

            BooksNames = culturesNames;
            BooksCodes = culturesCodes;
        }

        #endregion

        
        #region Static Methods

#if UNITY_EDITOR
        /// <summary>
        /// Creates a new instances of the localization settings into the
        /// Resources folder.
        /// </summary>
        [MenuItem ("Tools/Bricks Bucket/Localization/Create Asset")]
        public static void InitializeLocalization ()
        {
            if (Instance){}
        }
#endif

        /// <summary>
        /// Gets the book with the specified code.
        /// </summary>
        /// <param name="code">Code of the book to look for.</param>
        /// <param name="book">Book found in settings. <value>Null</value>
        ///  if the book was not found.</param>
        /// <returns>Whether the book was found.</returns>
        public static bool GetBook (string code, out Book book)
        {
            if (!Instance.ContainsBook (code))
            {
                book = null;
                return false;
            }
            
            book = Instance.BooksDictionary[code];
            return true;
        }

        /// <summary>
        /// Gets the culture for the specified code.
        /// </summary>
        /// <param name="code">Code of the culture to look for.</param>
        /// <param name="culture">Culture found in settings. <value>Default
        /// </value> if the culture was not found.</param>
        /// <returns>Whether the culture book was found.</returns>
        public static bool GetCulture (string code, out Culture culture)
        {
            for (int i = 0; i < Instance.CulturesList.Count; i++)
            {
                if (Instance.CulturesList[i].Code == code)
                {
                    culture = Instance.CulturesList[i];
                    return true;
                }
            }

            culture = default;
            return false;
        }

        /// <summary>
        /// Tries to get the localization for the given parameters.
        /// </summary>
        /// <param name="book">Book where to look for.</param>
        /// <param name="code">Code of the localized object.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="type">Type of the localization value.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found. Returns <value>
        /// <c>NULL</c></value> if the localization was found.</returns>
        private static bool GetLocalization (
            string book, string code, string culture, LocalizationType type,
            out object value
        )
        {
            if (!Instance.BooksDictionary.ContainsKey (book))
            {
                value = null;
                return false;
            }

            var bookObject = Instance.BooksDictionary[book];
            if (bookObject == null)
            {
                value = null;
                return false;
            }

            var group = bookObject.GroupsDictionary[type];
            if (!group.ContainsLocalizedObject (code))
            {
                value = null;
                return false;
            }

            var localizedObject = group.GetLocalized (code);
            if (localizedObject == null)
            {
                value = null;
                return false;
            }

            if (!localizedObject.ContainsCulture (culture))
            {
                value = null;
                return false;
            }
            
            var localization = localizedObject.GetLocalization (culture);

            var returnDefault =
                culture != DefaultCulture.Code &&
                localization == null &&
                UseDefaultCulture;

            if (returnDefault)
                return GetLocalization (
                    book, code, DefaultCulture.Code, type, out value
                );

            value = localization;
            return true;
        }

        /// <summary>
        /// Gets the text localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationText (
            string book, string code, string culture, out string value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.TEXT,
                out var tempValue
            );
            if (!found) return false;
            value = (string) tempValue;
            return true;
        }

        /// <summary>
        /// Gets the Texture localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationTexture (
            string book, string code, string culture, out Texture value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.TEXTURE,
                out var tempValue
            );
            if (!found) return false;
            value = (Texture) tempValue;
            return true;
        }

        /// <summary>
        /// Gets the Sprite localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationSprite (
            string book, string code, string culture, out Sprite value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.SPRITE,
                out var tempValue
            );
            if (!found) return false;
            value = (Sprite) tempValue;
            return true;
        }

        /// <summary>
        /// Gets the Audio Clip localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationAudio (
            string book, string code, string culture, out AudioClip value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.AUDIO,
                out var tempValue
            );
            if (!found) return false;
            value = (AudioClip) tempValue;
            return true;
        }

        /// <summary>
        /// Gets the Video Clip localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationVideo (
            string book, string code, string culture, out VideoClip value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.VIDEO,
                out var tempValue
            );
            if (!found) return false;
            value = (VideoClip) tempValue;
            return true;
        }

        /// <summary>
        /// Gets the <see href=
        /// "https://docs.unity3d.com/ScriptReference/Object.html">
        /// Unity Object</see> localization.
        /// </summary>
        /// <param name="book">Code of book with the localization.</param>
        /// <param name="code">Code of the localization.</param>
        /// <param name="culture">Culture for the localization.</param>
        /// <param name="value">Value of the localization.</param>
        /// <returns>Whether the localization was found.</returns>
        public static bool GetLocalizationObject (
            string book, string code, string culture, out Object value
        )
        {
            value = null;
            var found = GetLocalization (
                book,
                code,
                culture,
                LocalizationType.OBJECT,
                out var tempValue
            );
            if (!found) return false;
            value = (Object) tempValue;
            return true;
        }

        #endregion
    }
}
*/