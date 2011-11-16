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
	public static class MainClass
	{
		public static string LIBRARY_VERSION = SharpPcap.Version.VersionString;
		public static CaptureFileWriterDevice OUTPUT_STREAM;
		
		public const string DEFAULT_FILE = @"results.pcap";
		public const string DEFAULT_FILTER = @"ip and tcp";
		public const int TIMEOUT = 1000;
		
		public static void Main (string[] args)
		{
			ICollection<ICaptureDevice> captureDevices = new List<ICaptureDevice>();
			
			try
			{
				CaptureDeviceList deviceList = CaptureDeviceList.New();
				
				if (deviceList.Count < 1)
				{
					Console.WriteLine("No devices found. Please press any key to continue.");
				}
				else
				{
					// Feed a dog a bone.
					foreach (ICaptureDevice dev in deviceList)
					{
						string.Format("{1}{0}", Environment.NewLine, dev.Description);
					}
					
					Console.Write ("Device(s) to be captured (comma separated): ");
					string input = Console.ReadLine();
					
					// Pull apart the request in an attempt to find the devices.
					foreach (string segment in input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						int deviceNumber;
						
						if (int.TryParse(segment.Trim(), out deviceNumber))
						{
							ICaptureDevice device = deviceList[deviceNumber];
							
							if (!captureDevices.Contains(device))
							{
								captureDevices.Add(device);
							}
						}
					}
					
					Console.Write("Filter [{0}]: ", DEFAULT_FILTER);
					string filter = Console.ReadLine().Trim();
					
					// Apply the default filter if none was given.
					if (!string.IsNullOrEmpty(filter))
					{
						filter = DEFAULT_FILTER;
					}
					
					// Open each device requested for scan.
					foreach (ICaptureDevice device in captureDevices)
					{
						// Apply filter and event handler(s).
						device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
						device.Filter = filter;
						
						if (device is AirPcapDevice)
						{
							// NOTE: AirPcap devices cannot disable local capture.
							var airDevice = device as AirPcapDevice;
							airDevice.Open(OpenFlags.DataTransferUdp, TIMEOUT);
						}
						else if (device is WinPcapDevice)
						{
							var winDevice = device as WinPcapDevice;
							winDevice.Open(OpenFlags.DataTransferUdp | OpenFlags.NoCaptureLocal, TIMEOUT);
						}
						else
						{
							device.Open(DeviceMode.Promiscuous, TIMEOUT);
						}
						
						// Start capturing.
						device.StartCapture();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Console.WriteLine("TcpDump or WinPcap is not installed! Press any key to continue.");
			}
			
			// Waiting for 1630.
			Console.ReadKey();
			
			// Close any open devices.
			foreach (ICaptureDevice device in captureDevices)
			{
				device.Close();
			}
		}
		
		private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
		{
			CaptureFileWriterDevice writer = null;
			
			try
			{
				// Dump to the file.
				writer = new CaptureFileWriterDevice(DEFAULT_FILE, FileMode.Append);
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
