// 
// DbPacketRepositoryTests.cs
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
using System.Linq;
using NHibernate;
using NUnit.Framework;
using WhiteNoise.Domain.Abstract;
using WhiteNoise.Domain.Concrete;
using WhiteNoise.Domain.Entities;

namespace WhiteNoise.Test.Domain.Concrete
{
	/// <summary>
	/// Database packet repository tests.
	/// </summary>
	/// <exception cref='NotImplementedException'>
	/// Is thrown when a requested operation is not implemented for a given type.
	/// </exception>
	[TestFixture]
	public class DbPacketRepositoryTests : DbRepositoryTests<Packet>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteNoise.Test.Domain.Concrete.DbPacketRepositoryTests"/> class.
		/// </summary>
		public DbPacketRepositoryTests()
		{
			this.Items = new List<Packet>()
			{
				new Packet() { Type = "Type1", Data = new byte[] { } },	
				new Packet() { Type = "Type1", Data = new byte[] { } },
				new Packet() { Type = "Type2", Data = new byte[] { } },
				new Packet() { Type = "Type2", Data = new byte[] { } },
				new Packet() { Type = "Type3", Data = new byte[] { } },
				new Packet() { Type = "Type3", Data = new byte[] { } },
			};
		}
		
		[Test]
		public void CanAddPacket()
		{
			var item = new Packet() { Type = "Type4", Data = new byte[] { } };
			
			using (ISession session = this.SessionFactory.OpenSession())
			{
				IPacketRepository repository = new DbPacketRepository(session);
				repository.Add(item);
			}
			
			// Use a different session to properly test the transaction.
			using (ISession session = this.SessionFactory.OpenSession())
			{
				var fromDatabase = session.Get<Packet>(item.Id);
				
				Assert.That(fromDatabase, Is.Not.Null);
				Assert.That(fromDatabase, Is.Not.SameAs(item));
				Assert.That(fromDatabase, Is.EqualTo(item.Type));
			}
		}
		
		[Test]
		public void CanDetermineEqualityOfPackets()
		{
			throw new NotImplementedException();
		}
		
		[Test]
		public void CanFetchRepository()
		{
			using (ISession session = this.SessionFactory.OpenSession())
			{
				IPacketRepository repository = new DbPacketRepository(session);
				var fromDatabase = repository.Collection;
				
				Assert.That(fromDatabase.Count, Is.EqualTo(this.Items.Count));
				
				foreach (var item in this.Items)
				{
					Assert.That(fromDatabase.Any(x => x.Type == item.Type));
				}
			}
		}
		
		[Test]
		public void CanFindById()
		{
			var item = this.Items.First();
			
			using (ISession session = this.SessionFactory.OpenSession())
			{
				IPacketRepository repository = new DbPacketRepository(session);
				var fromDatabase = repository.Find(item.Id);
				
				Assert.That(fromDatabase, Is.Not.Null);
				Assert.That(fromDatabase, Is.Not.SameAs(item));
				Assert.That(fromDatabase.Id, Is.EqualTo(item.Id));
			}
		}
		
		[Test]
		public void CanRemovePacket()
		{
			var item = this.Items.First();
			
			using (ISession session = this.SessionFactory.OpenSession())
			{
				IPacketRepository repository = new DbPacketRepository(session);
				repository.Remove(item);
			}
			
			// Use a different session to properly test the transaction.
			using (ISession session = this.SessionFactory.OpenSession())
			{
				var fromDatabase = session.Get<Packet>(item.Id);
				Assert.That(fromDatabase, Is.Null);
			}
		}
		
		[Test]
		public void CanUpdatePacket()
		{
			var item = this.Items.First();
			item.Type = @"Type6";
			
			using (ISession session = this.SessionFactory.OpenSession())
			{
				IPacketRepository repository = new DbPacketRepository(session);
				repository.Update(item);
			}
			
			// Use a different session to properly test the transaction.
			using (ISession session = this.SessionFactory.OpenSession())
			{
				var fromDatabase = session.Get<Packet>(item.Id);
				
				Assert.That(fromDatabase, Is.Not.Null);
				Assert.That(fromDatabase.Type, Is.EqualTo(item.Type));
			}
		}
	}
}

