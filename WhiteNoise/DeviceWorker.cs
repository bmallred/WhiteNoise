using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

namespace WhiteNoise
{
	/// <summary>
	/// Device worker.
	/// </summary>
	public class DeviceWorker
	{
		private static FileInfo _file;
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
			
			if (!lazyLoad)
			{
				this._devices = CaptureDeviceList.Instance;
			}
			
			this.Devices = new List<string>();
			this.Filter = "ip and tcp";
			this.Timeout = 1000;
			this.Version = SharpPcap.Version.VersionString;
			
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
		
		/// <summary>
		/// Captures the device.
		/// </summary>
		/// <param name='deviceNumber'>
		/// Device number.
		/// </param>
		public void CaptureDevice(int deviceNumber)
		{
			var device = this._devices[deviceNumber - 1];
			
			//// NOTE: This may need to be placed elsewhere.
			//if (!this._captureDevices.Contains(device))
			//{
			//	this._captureDevices.Add(device);
			//}
			
			// Apply filter and event handler(s).
			device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
			device.Filter = this.Filter;
						
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