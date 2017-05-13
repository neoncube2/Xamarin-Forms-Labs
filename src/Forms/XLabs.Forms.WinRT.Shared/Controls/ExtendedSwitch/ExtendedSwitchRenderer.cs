// ***********************************************************************
// Assembly         : XLabs.Forms.WinRT.Shared
// Author           : XLabs Team
// Created          : 03-04-2017
// 
// Last Modified By : Eli Black
// Last Modified On : 03-04-2017
// ***********************************************************************
// <copyright file="ExtendedSwitchRenderer.cs" company="XLabs Team">
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

using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using XLabs.Forms.Controls;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#endif

using Switch = Windows.UI.Xaml.Controls.ToggleSwitch;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedSwitchRenderer.
	/// </summary>
	public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, Switch>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.Toggled -= ElementToggled;
			}

			if (e.NewElement != null)
			{
                if (this.Control == null)
                {
                    var toggle = new Switch();
                    toggle.Toggled += ControlValueChanged;

                    this.SetNativeControl(toggle);
                }
                this.Control.IsOn = e.NewElement.IsToggled;
				this.SetTintColor(this.Element.TintColor);
				this.Element.Toggled += ElementToggled;
			}
		}

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "TintColor")
			{
				this.SetTintColor(this.Element.TintColor);
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Control.Toggled -= this.ControlValueChanged;
				this.Element.Toggled -= ElementToggled;
			}

			base.Dispose(disposing);
		}

        private static byte doubleColorToByteColor(double doubleColor)
        {
            return (byte)(doubleColor * byte.MaxValue);
        }

        private static Windows.UI.Color GetWindowsColor(Xamarin.Forms.Color xamarinColor)
        {
            return Windows.UI.Color.FromArgb(
                byte.MaxValue,
                doubleColorToByteColor(xamarinColor.R),
                doubleColorToByteColor(xamarinColor.G),
                doubleColorToByteColor(xamarinColor.B)
            );
        }

        /// <summary>
        /// Sets the color of the tint.
        /// </summary>
        /// <param name="color">The color.</param>
        private void SetTintColor(Color color)
		{
            Windows.UI.Color windowsColor = GetWindowsColor(color);

            this.Control.Foreground = new SolidColorBrush(windowsColor);
		}

		/// <summary>
		/// Handles the Toggled event of the Element control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ToggledEventArgs"/> instance containing the event data.</param>
		private void ElementToggled(object sender, ToggledEventArgs e)
		{
			this.Control.IsOn = this.Element.IsToggled;
		}

		/// <summary>
		/// Handles the ValueChanged event of the Control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ControlValueChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.Element.IsToggled = this.Control.IsOn;
		}
	}
}