﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Prototype.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PersonStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PersonStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Prototype.Common.Resources.PersonStrings", typeof(PersonStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Birth Date.
        /// </summary>
        public static string BirthDate_Name {
            get {
                return ResourceManager.GetString("BirthDate_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Full Name.
        /// </summary>
        public static string FullName_Name {
            get {
                return ResourceManager.GetString("FullName_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be between {1} and {2}.
        /// </summary>
        public static string Generic_Range {
            get {
                return ResourceManager.GetString("Generic_Range", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be filled in.
        /// </summary>
        public static string Generic_Required {
            get {
                return ResourceManager.GetString("Generic_Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be shorter than {1} characters and longer than {2} characters.
        /// </summary>
        public static string Generic_StringLength {
            get {
                return ResourceManager.GetString("Generic_StringLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to House Number.
        /// </summary>
        public static string HouseNumber_Name {
            get {
                return ResourceManager.GetString("HouseNumber_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registration Date.
        /// </summary>
        public static string RegistrationDate_Name {
            get {
                return ResourceManager.GetString("RegistrationDate_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UID.
        /// </summary>
        public static string UniqueId_Name {
            get {
                return ResourceManager.GetString("UniqueId_Name", resourceCulture);
            }
        }
    }
}
