// ***********************************************************************
// Assembly         : XLabs.Forms.WinRT.Forms
// Author           : XLabs Team
// Created          : 12-31-2014
// 
// Last Modified By : Eli Black
// Last Modified On : 02-17-2017
// ***********************************************************************
// <copyright file="RadioButtonRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Windows.UI.Xaml.Media;

#if WINDOWS_PHONE
using Xamarin.Forms.Platform.WinPhone;

#elif WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;

#elif NETFX_CORE
using Xamarin.Forms.Platform.UWP;
#endif

using NativeRadioButton = Windows.UI.Xaml.Controls.RadioButton;

[assembly: ExportRenderer(typeof(CustomRadioButton), typeof(RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, NativeRadioButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.CheckedChanged -= CheckedChanged;
                e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            if (e.NewElement == null)
            {
                return;
            }

            if (Control == null)
            {
                var radioButton = new NativeRadioButton();

                SetNativeControl(radioButton);

                radioButton.Checked += (s, args) =>
                {
                    CustomRadioButton element = Element;

                    if (element != null)
                    {
                        element.Checked = true;
                    }
                };

                radioButton.Unchecked += (s, args) =>
                {
                    CustomRadioButton element = Element;

                    if (element != null)
                    {
                        element.Checked = false;
                    }
                };
            }

            Control.Content = Element.Text;
            Control.IsChecked = Element.Checked;

            UpdateFont();

            Element.CheckedChanged += CheckedChanged;
            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NativeRadioButton control = Control;

            if (control == null)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                switch (propertyChangedEventArgs.PropertyName)
                {
                    case "Checked":
                        control.IsChecked = Element.Checked;
                        break;
                    case "TextColor":
                        control.Foreground = Element.TextColor.ToBrush();
                        break;
                    case "FontName":
                    case "FontSize":
                        UpdateFont();
                        break;
                    case "Text":
                        control.Content = Element.Text;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                        break;
                }
            });
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Control.Content = Element.Text;
                Control.IsChecked = eventArgs.Value;
            });
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty(Element.FontName))
            {
                Control.FontFamily = new FontFamily(Element.FontName);
            }
 
            Control.FontSize = (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f;
        }
    }
}