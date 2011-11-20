// 
// DbRepository.cs
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
using NHibernate;
using WhiteNoise.Domain.Abstract;

namespace WhiteNoise.Domain.Concrete
{
	/// <summary>
	/// Database repository.
	/// </summary>
	public abstract class DbRepository<T> : IRepository<T>
	{
		/// <summary>
		/// The NHibernate session.
		/// </summary>
		private readonly ISession _session;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteNoise.Domain.Concrete.DbRepository`1"/> class.
		/// </summary>
		/// <param name='session'>
		/// Session.
		/// </param>
		public DbRepository(ISession session)
		{
			this._session = session;
		}

		#region IRepository[T] implementation
		
		/// <summary>
		/// Add the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		public T Add(T member)
		{
			using (ITransaction transaction = this._session.BeginTransaction())
			{
				this._session.Save(member);
				transaction.Commit();
			}
			
			return member;
		}
		
		/// <summary>
		/// Find the specified id.
		/// </summary>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		public T Find(int id)
		{
			return this._session.Get<T>(id);
		}
		
		/// <summary>
		/// Remove the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		public T Remove(T member)
		{
			using (ITransaction transaction = this._session.BeginTransaction())
			{
				this._session.Delete(member);
				transaction.Commit();
			}
			
			return member;
		}
		
		/// <summary>
		/// Update the specified member.
		/// </summary>
		/// <param name='member'>
		/// Member.
		/// </param>
		public T Update(T member)
		{
			using (ITransaction transaction = this._session.BeginTransaction())
			{
				this._session.Update(member);
				transaction.Commit();
			}
			
			return member;
		}
		
		/// <summary>
		/// Gets the collection.
		/// </summary>
		/// <value>
		/// The collection.
		/// </value>
		public IList<T> Collection 
		{
			get 
			{
				throw new NotImplementedException();
				//return this._session.QueryOver<T>.QueryOver<T>().List();
			}
		}
		
		#endregion
	}
}

