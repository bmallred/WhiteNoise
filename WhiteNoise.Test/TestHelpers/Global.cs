// 
// Global.cs
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

namespace WhiteNoise.Test.TestHelpers
{
	/// <summary>
	/// Global variables.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// Constant connection string.
		/// </summary>
		public const string ConnectionString = @"Server=localhost;Database=test;User ID=postgres;Password=password;"; //@"Data Source=file:test.db";
		
		/// <summary>
		/// Constant database provider.
		/// </summary>
		public const string DatabaseProvider = @"PostgreSQL";
	}
}

