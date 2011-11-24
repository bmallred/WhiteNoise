// 
// DatabaseWorkerTests.cs
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
using WhiteNoise.Storage;
using WhiteNoise.Test.TestHelpers;

namespace WhiteNoise.Test.Storage
{
	/// <summary>
	/// Database worker tests.
	/// </summary>
	[TestFixture]
	public class DatabaseWorkerTests
	{
		/// <summary>
		/// The database worker.
		/// </summary>
		private DatabaseWorker _worker;
		
		/// <summary>
		/// Test fixture setup.
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			this._worker = new DatabaseWorker(null);
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
			Assert.That(!string.IsNullOrWhiteSpace(this._worker.Version));
		}
		
		/// <summary>
		/// Determines whether this instance can capture device file.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can capture device file; otherwise, <c>false</c>.
		/// </returns>
		[Test]
		public void CanCaptureDeviceFile()
		{
			bool started = true;
			
			try
			{
				this._worker.CaptureDeviceFile(Global.CaptureFile);
			}
			catch
			{
				started = false;
			}
			
			// Clean up if necessary.
			if (started)
			{
				this._worker.Stop();
			}
			
			Assert.That(started, Is.EqualTo(true));
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

