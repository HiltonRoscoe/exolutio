﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exolutio.Model.PSM.Grammar.SchematronTranslation {
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
    internal class XPathTranslationLogMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal XPathTranslationLogMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Exolutio.Model.PSM.Grammar.SchematronTranslation.XPathTranslationLogMessages", typeof(XPathTranslationLogMessages).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Call of operation `{0}` (arity: {1}) is ambiguous. .
        /// </summary>
        internal static string OperationHelper_OperationCallAmbiguous_2 {
            get {
                return ResourceManager.GetString("OperationHelper_OperationCallAmbiguous_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation `{0}` not found. .
        /// </summary>
        internal static string OperationHelper_OperationNotFound_1 {
            get {
                return ResourceManager.GetString("OperationHelper_OperationNotFound_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation `{0} (arity: {1})` not found. .
        /// </summary>
        internal static string OperationHelper_OperationNotFound_2 {
            get {
                return ResourceManager.GetString("OperationHelper_OperationNotFound_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Navigations should end in attributes to translate correctly..
        /// </summary>
        internal static string UNSAFE_PROPERTY_CALL_EXP {
            get {
                return ResourceManager.GetString("UNSAFE_PROPERTY_CALL_EXP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsafe use of variable &apos;{0}&apos;. Variables of type derived from Class can not be translated to XPath. Access properties of &apos;{0}&apos; in the expression instead. .
        /// </summary>
        internal static string UNSAFE_VARIABLE_EXP {
            get {
                return ResourceManager.GetString("UNSAFE_VARIABLE_EXP", resourceCulture);
            }
        }
    }
}
