// 
// DbPacketRepository.cs
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
using NHibernate;
using WhiteNoise.Domain.Abstract;
using WhiteNoise.Domain.Entities;

namespace WhiteNoise.Domain.Concrete
{
	/// <summary>
	/// Database packet repository.
	/// </summary>
	public class DbPacketRepository : DbRepository<Packet>, IPacketRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteNoise.Domain.Concrete.DbPacketRepository"/> class.
		/// </summary>
		/// <param name='session'>
		/// Session.
		/// </param>
		public DbPacketRepository(ISession session)
			: base(session)
		{
			// Stub.
		}
	}
}

