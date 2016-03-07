﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Artemis.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Artemis.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-16&quot;?&gt;
        ///&lt;!-- Used by Artemis to get the active Sign --&gt;
        ///&lt;UserConfig&gt;
        ///	&lt;Group id=&quot;Artemis&quot; displayName=&quot;Artemis&quot;&gt;
        ///		&lt;VisibleVars&gt;
        ///			&lt;Var id=&quot;ActiveSign&quot; displayName=&quot;ActiveSign&quot; displayType=&quot;SLIDER:0:1:1000000&quot;/&gt;
        ///		&lt;/VisibleVars&gt;
        ///	&lt;/Group&gt;
        ///&lt;/UserConfig&gt;.
        /// </summary>
        internal static string artemisXml {
            get {
                return ResourceManager.GetString("artemisXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap bow {
            get {
                object obj = ResourceManager.GetObject("bow", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Artemis&quot;
        ///{
        /// &quot;uri&quot; &quot;http://localhost:{{port}}/csgo_game_event&quot;
        /// &quot;timeout&quot; &quot;5.0&quot;
        /// &quot;buffer&quot;  &quot;0.1&quot;
        /// &quot;throttle&quot; &quot;0.1&quot;
        /// &quot;heartbeat&quot; &quot;30.0&quot;
        /// &quot;data&quot;
        /// {
        ///   &quot;provider&quot;            &quot;1&quot;
        ///   &quot;map&quot;                 &quot;1&quot;
        ///   &quot;round&quot;               &quot;1&quot;
        ///   &quot;player_id&quot;           &quot;1&quot;
        ///   &quot;player_state&quot;        &quot;1&quot;
        ///   &quot;player_weapons&quot;      &quot;1&quot;
        ///   &quot;player_match_stats&quot;  &quot;1&quot;
        /// }
        ///}.
        /// </summary>
        internal static string gamestateConfiguration {
            get {
                return ResourceManager.GetString("gamestateConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon logo {
            get {
                object obj = ResourceManager.GetObject("logo", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon logo_disabled {
            get {
                object obj = ResourceManager.GetObject("logo_disabled", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /***********************************************************************/
        ////** 	© 2015 CD PROJEKT S.A. All rights reserved.
        ////** 	THE WITCHER® is a trademark of CD PROJEKT S. A.
        ////** 	The Witcher game is based on the prose of Andrzej Sapkowski.
        ////***********************************************************************/
        ///
        ///
        ///
        ///
        ///statemachine class W3PlayerWitcher extends CR4Player
        ///{	
        ///	
        ///	private saved var craftingSchematics				: array&lt;name&gt;; 					
        ///	
        ///	
        ///	private saved var alchemyRecipes 					: array&lt;name&gt;; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string playerWitcherWs {
            get {
                return ResourceManager.GetString("playerWitcherWs", resourceCulture);
            }
        }
    }
}
