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
using System.Diagnostics;
using WhiteNoise.Listen;

namespace WhiteNoise
{
	/// <summary>
	/// Main class.
	/// </summary>
	public static class MainClass
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			DeviceWorker devWorker = null;
			
			try
			{
				// Initialize a new worker (this will throw an exception if no PCAP libraries are found).
				devWorker = new DeviceWorker();
			
				if (devWorker.Devices.Count < 1)
				{
					Console.WriteLine("No devices found. Please press any key to continue.");
					return;
				}
				
				Console.WriteLine("Available devices:");
				Console.WriteLine();
				
				foreach (var item in devWorker.Devices)
				{
					Console.WriteLine(item);
				}
				
				Console.WriteLine();
				Console.Write ("Device(s) to be captured (comma separated): ");
				string inputDevices = Console.ReadLine();
				
				Console.Write("Filter [{0}]: ", devWorker.Filter);
				string filter = Console.ReadLine().Trim();
				
				if (!string.IsNullOrWhiteSpace(filter))
				{
					devWorker.Filter = filter;
				}
				
				Console.WriteLine();
				Console.WriteLine("Capturing selected devices. Press any key to exit.");
				
				// Pull apart the request in an attempt to find the devices.
				foreach (string segment in inputDevices.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					int deviceNumber;
					
					if (int.TryParse(segment.Trim(), out deviceNumber))
					{
						devWorker.CaptureDevice(deviceNumber);
					}
				}
				
				// Waiting for 1630.
				Console.ReadKey();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				
				// Inform the user where to get the prize.
				Console.WriteLine("TcpDump or WinPcap is not installed!");
				Console.WriteLine("Please download the appropriate libraries.");
				Console.WriteLine();
				Console.WriteLine("\t{0}{1}", "Windows:".PadRight(20), @"http://www.winpcap.org");
				Console.WriteLine("\t{0}{1}", "Linux or MacOS:".PadRight(20), @"http://www.tcpdump.org");
				Console.WriteLine();
			}
			finally
			{
				// Close any open devices.
				if (devWorker != null)
				{
					devWorker.Stop();
				}
			}
		}
	}
}
