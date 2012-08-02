// 
// DeviceWorkerTests.cs
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
using NUnit.Framework;
using WhiteNoise.Listen;

namespace WhiteNoise.Test.Listen
{
	/// <summary>
	/// Device worker tests.
	/// </summary>
	[TestFixture]
	public class DeviceWorkerTests
	{
		/// <summary>
		/// The device worker with lazy loading.
		/// </summary>
		private DeviceWorker _lazyWorker;
		
		/// <summary>
		/// The device worker.
		/// </summary>
		private DeviceWorker _worker;
		
		/// <summary>
		/// Test fixture setup.
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			this._lazyWorker = new DeviceWorker(lazyLoad: true);
			
			try
			{
				this._worker = new DeviceWorker();
			}
			catch
			{
				this._worker = null;
			}
		}
		
		/// <summary>
		/// Checks for the dependencies to be installed.
		/// </summary>
		[Test]
		public void AreDependeciesInstalled()
		{
			Assert.That(this._worker, Is.Not.Null, "TcpDump or WinPcap is not installed (run as root on *nix systems)!");
		}
		
		/// <summary>
		/// Determines whether this instance has devices.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has devices; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void HasDevices()
		{
			Assert.That(this._worker, Is.Not.Null);
			Assert.That(this._worker.Devices.Count, Is.GreaterThan(0));
		}
		
		/// <summary>
		/// Determines whether this instance has version.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has version; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void HasVersion()
		{
			Assert.That(!string.IsNullOrWhiteSpace(this._lazyWorker.Version));
		}
		
		/// <summary>
		/// Determines whether this instance has defaults.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has defaults; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void HasDefaults()
		{
			Assert.That(!string.IsNullOrWhiteSpace(this._lazyWorker.FileName));
			Assert.That(this._lazyWorker.Timeout, Is.EqualTo(1000));
			Assert.That(this._lazyWorker.Filter, Is.EqualTo("ip and tcp"));
		}
		
		/// <summary>
		/// Determines whether this instance can refresh.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can refresh; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void CanRefresh()
		{
			bool refreshed = true;
			
			try
			{
				this._worker.Refresh();
			}
			catch
			{
				refreshed = false;
			}
			
			Assert.That(refreshed, Is.EqualTo(true));
		}
		
		/// <summary>
		/// Determines whether this instance can capture device.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can capture device; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void CanCaptureDevice()
		{
			bool captured = true;
			
			try
			{
				this._worker.CaptureDevice(1);
			}
			catch
			{
				captured = false;
			}
			
			Assert.That(captured, Is.EqualTo(true));
		}
		
		/// <summary>
		/// Determines whether this instance can stop captures.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can stop captures; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void CanStopCaptures()
		{
			bool stopped = true;
			
			try
			{
				this._worker.Stop();
			}
			catch
			{
				stopped = false;
			}
			
			Assert.That(stopped, Is.EqualTo(true));
		}
	}
}

