﻿#pragma checksum "C:\Users\csend\Documents\szakdolgozat\szd1\szd1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4301E593DE0FB3DB3726812BD483AF09"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace szd1
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_UIElement_Visibility(global::Windows.UI.Xaml.UIElement obj, global::Windows.UI.Xaml.Visibility value)
            {
                obj.Visibility = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class MainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::szd1.MainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Button obj3;
            private global::Windows.UI.Xaml.Controls.Button obj4;
            private global::Windows.UI.Xaml.Controls.Grid obj5;
            private global::Windows.UI.Xaml.Controls.ItemsControl obj6;
            private global::Windows.UI.Xaml.Controls.Button obj7;
            private global::Windows.UI.Xaml.Controls.Button obj8;

            private MainPage_obj1_BindingsTracking bindingsTracking;

            public MainPage_obj1_Bindings()
            {
                this.bindingsTracking = new MainPage_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3: // MainPage.xaml line 14
                        this.obj3 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    case 4: // MainPage.xaml line 15
                        this.obj4 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    case 5: // MainPage.xaml line 16
                        this.obj5 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 6: // MainPage.xaml line 17
                        this.obj6 = (global::Windows.UI.Xaml.Controls.ItemsControl)target;
                        break;
                    case 7: // MainPage.xaml line 24
                        this.obj7 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    case 8: // MainPage.xaml line 25
                        this.obj8 = (global::Windows.UI.Xaml.Controls.Button)target;
                        break;
                    default:
                        break;
                }
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::szd1.MainPage)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::szd1.MainPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_viewModel(obj.viewModel, phase);
                    }
                }
            }
            private void Update_viewModel(global::szd1.ViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_viewModel(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_viewModel_IsInMenu(obj.IsInMenu, phase);
                        this.Update_viewModel_IsInFillomino(obj.IsInFillomino, phase);
                        this.Update_viewModel_StickyArray(obj.StickyArray, phase);
                        this.Update_viewModel_IsInSticky(obj.IsInSticky, phase);
                    }
                }
            }
            private void Update_viewModel_IsInMenu(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainPage.xaml line 14
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj3, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    // MainPage.xaml line 15
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj4, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                }
            }
            private void Update_viewModel_IsInFillomino(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainPage.xaml line 16
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj5, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    // MainPage.xaml line 24
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj7, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                }
            }
            private void Update_viewModel_StickyArray(global::szd1.Classes.Unit[,] obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainPage.xaml line 17
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj6, (global::System.Object)this.LookupConverter("MapToGeometry").Convert(obj, typeof(global::System.Object), null, null), null);
                }
            }
            private void Update_viewModel_IsInSticky(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainPage.xaml line 17
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj6, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    // MainPage.xaml line 25
                    XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj8, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibility").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class MainPage_obj1_BindingsTracking
            {
                private global::System.WeakReference<MainPage_obj1_Bindings> weakRefToBindingObj; 

                public MainPage_obj1_BindingsTracking(MainPage_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<MainPage_obj1_Bindings>(obj);
                }

                public MainPage_obj1_Bindings TryGetBindingObject()
                {
                    MainPage_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_viewModel(null);
                }

                public void PropertyChanged_viewModel(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MainPage_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::szd1.ViewModel obj = sender as global::szd1.ViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_viewModel_IsInMenu(obj.IsInMenu, DATA_CHANGED);
                                bindings.Update_viewModel_IsInFillomino(obj.IsInFillomino, DATA_CHANGED);
                                bindings.Update_viewModel_StickyArray(obj.StickyArray, DATA_CHANGED);
                                bindings.Update_viewModel_IsInSticky(obj.IsInSticky, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "IsInMenu":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_viewModel_IsInMenu(obj.IsInMenu, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsInFillomino":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_viewModel_IsInFillomino(obj.IsInFillomino, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "StickyArray":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_viewModel_StickyArray(obj.StickyArray, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsInSticky":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_viewModel_IsInSticky(obj.IsInSticky, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::szd1.ViewModel cache_viewModel = null;
                public void UpdateChildListeners_viewModel(global::szd1.ViewModel obj)
                {
                    if (obj != cache_viewModel)
                    {
                        if (cache_viewModel != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_viewModel).PropertyChanged -= PropertyChanged_viewModel;
                            cache_viewModel = null;
                        }
                        if (obj != null)
                        {
                            cache_viewModel = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_viewModel;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 13
                {
                    this.mainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // MainPage.xaml line 14
                {
                    this.fillominoButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.fillominoButton).Click += this.FillominoClick;
                }
                break;
            case 4: // MainPage.xaml line 15
                {
                    this.stickyButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.stickyButton).Click += this.StickyBlocksClick;
                }
                break;
            case 5: // MainPage.xaml line 16
                {
                    this.gameGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 7: // MainPage.xaml line 24
                {
                    global::Windows.UI.Xaml.Controls.Button element7 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element7).Click += this.FillominoBackButtonClick;
                }
                break;
            case 8: // MainPage.xaml line 25
                {
                    global::Windows.UI.Xaml.Controls.Button element8 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element8).Click += this.StickyBackButtonClick;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    MainPage_obj1_Bindings bindings = new MainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    bindings.SetConverterLookupRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

