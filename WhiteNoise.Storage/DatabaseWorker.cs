// 
// DatabaseWorker.cs
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
using SharpPcap.LibPcap;
using WhiteNoise.Domain.Abstract;
using WhiteNoise.Domain.Entities;

namespace WhiteNoise.Storage
{
	/// <summary>
	/// Database worker.
	/// </summary>
	public class DatabaseWorker : BackgroundWorker
	{
		/// <summary>
		/// The packet repository.
		/// </summary>
		private static IPacketRepository _repository;
		
		/// <summary>
		/// Gets or sets the devices.
		/// </summary>
		/// <value>
		/// The devices.
		/// </value>
		private ICollection<ICaptureDevice> _deviceFiles { get; set; }
		
		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		public string Version { get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteNoise.Storage.DatabaseWorker"/> class.
		/// </summary>
		/// <param name='repository'>
		/// Repository.
		/// </param>
		public DatabaseWorker(IPacketRepository repository)
		{
			_repository = repository;
			this._deviceFiles = new List<ICaptureDevice>();
			this.Version = SharpPcap.Version.VersionString;
			
			this.WorkerReportsProgress = true;
			this.WorkerSupportsCancellation = true;
		}
		
		/// <summary>
		/// Captures the device file.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public void CaptureDeviceFile(string file)
		{
			this.CaptureDeviceFile(new FileInfo(file));
		}
		
		/// <summary>
		/// Captures the device file.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public void CaptureDeviceFile(FileInfo file)
		{
			if (file.Exists)
			{
				ICaptureDevice device = null;
				
				try
				{
					// Listen to the device file.
					device = new CaptureFileReaderDevice(file.FullName);
					
					// Add event handler(s).
					device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
					
					// Open device file and start capturing.
					device.Open();
					device.StartCapture();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
				finally
				{
					// Clean-up.
					if (device != null && device.Started)
					{
						this._deviceFiles.Add(device);
					}
				}
			}
		}
		
		/// <summary>
		/// Stop all listening devices.
		/// </summary>
		public void Stop()
		{
			foreach (ICaptureDevice device in this._deviceFiles)
			{
				if (device.Started)
				{
					device.StopCapture();
				}
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
			base.OnDoWork(e);
		}
		
		/// <summary>
		/// Device the on packet arrival.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Capture event arguments.
		/// </param>
		private static void Device_OnPacketArrival(object sender, CaptureEventArgs e)
		{
			Debug.WriteLine("Received {0} bytes.", e.Packet.Data.Length);
			
			try
			{
				// Dump to the file.
				_repository.Add(new WhiteNoise.Domain.Entities.Packet() 
				{ 
					Type = e.Packet.LinkLayerType.ToString(),
					Data = e.Packet.Data
				}); 
			}
			catch (Exception ex)
			{
				// Let the developer know!!!
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				// Clean up.
			}
		}
	}
}

