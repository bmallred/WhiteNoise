// 
// Packet.cs
//  
// Author:
//       Bryan Allred <bryan.allred@gmail.com>
// 
// Copyright (c) 2011 Bryan Allred
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

namespace WhiteNoise.Listen
{
	/// <summary>
	/// Device worker.
	/// </summary>
	public class DeviceWorker : BackgroundWorker
	{
		/// <summary>
		/// The file to dump information.
		/// </summary>
		private static FileInfo _file;
		
		/// <summary>
		/// The devices used for capturing packets.
		/// </summary>
		private CaptureDeviceList _devices;
		
		/// <summary>
		/// Gets or sets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		public ICollection<string> Devices { get; private set; }
		
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>
		/// The name of the file.
		/// </value>
		public string FileName 
		{ 
			get
			{
				return _file.FullName;
			}
			
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new NullReferenceException();
				}
				
				_file = new FileInfo(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the filter.
		/// </summary>
		/// <value>
		/// The filter.
		/// </value>
		public string Filter { get; set; }
		
		/// <summary>
		/// Gets or sets the timeout.
		/// </summary>
		/// <value>
		/// The timeout.
		/// </value>
		public int Timeout { get; set; }
		
		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		public string Version { get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteNoise.DeviceWorker"/> class.
		/// </summary>
		/// <param name='lazyLoad'>
		/// Bypass loading of device list on loading.
		/// </param>
		public DeviceWorker(bool lazyLoad = false)
		{
			_file = new FileInfo("results.pcap");
			
			this.Devices = new List<string>();
			this.Filter = "ip and tcp";
			this.Timeout = 1000;
			this.Version = SharpPcap.Version.VersionString;
			
			if (!lazyLoad)
			{
				this._devices = CaptureDeviceList.Instance;
				
				// Make a pretty list of devices.
				for (int i = 0; i < this._devices.Count; i++)
				{
					this.Devices.Add(
						string.Format("{1}. {2}{0}", 
							Environment.NewLine, 
							i + 1, 
							this._devices[i].Description)
						);
				}
			}
			
			this.WorkerReportsProgress = true;
			this.WorkerSupportsCancellation = true;
		}
		
		/// <summary>
		/// Captures the device.
		/// </summary>
		/// <param name='deviceNumber'>
		/// Device number.
		/// </param>
		public void CaptureDevice(int deviceNumber)
		{
			var device = this._devices[deviceNumber - 1];
			
			// Add event handler(s).
			device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
			
			// Open each device requested for scan.
			if (device is AirPcapDevice)
			{
				// NOTE: AirPcap devices cannot disable local capture.
				var airDevice = device as AirPcapDevice;
				airDevice.Open(OpenFlags.DataTransferUdp, this.Timeout);
			}
			else if (device is WinPcapDevice)
			{
				var winDevice = device as WinPcapDevice;
				winDevice.Open(OpenFlags.DataTransferUdp | OpenFlags.NoCaptureLocal, this.Timeout);
			}
			else
			{
				device.Open(DeviceMode.Promiscuous, this.Timeout);
			}
			
			// Apply the filter *only* after the device is open.
			device.Filter = this.Filter;
			
			// Start capturing.
			device.StartCapture();
		}
		
		/// <summary>
		/// Stop all listening devices.
		/// </summary>
		public void Stop()
		{
			foreach (ICaptureDevice device in this._devices)
			{
				if (device.Started)
				{
					device.StopCapture();
				}
			}
		}
		
		/// <summary>
		/// Refresh the device list.
		/// </summary>
		public void Refresh()
		{
			this._devices.Refresh();
			
			// Make a pretty list of devices.
			this.Devices.Clear();
			for (int i = 0; i < this._devices.Count; i++)
			{
				this.Devices.Add(
					string.Format("{1}. {2}{0}", 
						Environment.NewLine, 
						i + 1, 
						this._devices[i].Description)
					);
			}
		}
		
		/// <summary>
		/// Raises the do work event.
		/// </summary>
		/// <param name='e'>
		/// Work event arguments.
		/// </param>
		protected override void OnDoWork (DoWorkEventArgs e)
		{
			// TODO: Add the working portion (possibly pull from previous areas).
			base.OnDoWork (e);
		}
		
		/// <summary>
		/// Device_s the on packet arrival.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Capture event arguments.
		/// </param>
		private static void Device_OnPacketArrival(object sender, CaptureEventArgs e)
		{
			CaptureFileWriterDevice writer = null;
			Debug.WriteLine("Received {0} bytes.", e.Packet.Data.Length);
			
			try
			{
				// Dump to the file.
				writer = new CaptureFileWriterDevice(_file.FullName, FileMode.Append);
				writer.Write(e.Packet);
			}
			catch (Exception ex)
			{
				// Let the developer know!!!
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				// Clean up.
				if (writer != null)
				{
					writer.Close();
				}
			}
		}
	}
}