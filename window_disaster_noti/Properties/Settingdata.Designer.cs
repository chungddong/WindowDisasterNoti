﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace window_disaster_noti.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.3.0.0")]
    internal sealed partial class Settingdata : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settingdata defaultInstance = ((Settingdata)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settingdata())));
        
        public static Settingdata Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool cb_allowNoti {
            get {
                return ((bool)(this["cb_allowNoti"]));
            }
            set {
                this["cb_allowNoti"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool cb_usePopNoti {
            get {
                return ((bool)(this["cb_usePopNoti"]));
            }
            set {
                this["cb_usePopNoti"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool cb_runOnStartup {
            get {
                return ((bool)(this["cb_runOnStartup"]));
            }
            set {
                this["cb_runOnStartup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool cb_darkmode {
            get {
                return ((bool)(this["cb_darkmode"]));
            }
            set {
                this["cb_darkmode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool every_region {
            get {
                return ((bool)(this["every_region"]));
            }
            set {
                this["every_region"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("전라북도")]
        public string province {
            get {
                return ((string)(this["province"]));
            }
            set {
                this["province"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("전주시")]
        public string city {
            get {
                return ((string)(this["city"]));
            }
            set {
                this["city"] = value;
            }
        }
    }
}
