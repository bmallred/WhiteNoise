// 
// IRepository.cs
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
using System.Collections.Generic;

namespace WhiteNoise.Domain.Abstract
{
	/// <summary>
	/// Repository base interface.
	/// </summary>
	public interface IRepository<T>
	{
		/// <summary>
		/// Gets the collection.
		/// </summary>
		/// <value>
		/// The collection.
		/// </value>
		IList<T> Collection { get; }
		
		/// <summary>
		/// Add the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		T Add(T member);
		
		/// <summary>
		/// Find the specified id.
		/// </summary>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		T Find(int id);
		
		/// <summary>
		/// Remove the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		T Remove(T member);
		
		/// <summary>
		/// Update the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		T Update(T member);
	}
}

