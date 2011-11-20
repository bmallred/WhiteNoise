// 
// RepositoryTests.cs
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
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using WhiteNoise.Domain.NHibernate;
using WhiteNoise.Test.TestHelpers;

namespace WhiteNoise.Test.Domain.Concrete
{
	/// <summary>
	/// Database repository tests.
	/// </summary>
	[TestFixture]
	public abstract class DbRepositoryTests<T>
	{
		/// <summary>
		/// The _configuration.
		/// </summary>
		private Configuration _configuration;
		
		/// <summary>
		/// Gets or sets the session factory.
		/// </summary>
		/// <value>
		/// The session factory.
		/// </value>
		protected ISessionFactory SessionFactory { get; set; }
		
		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>
		/// The items.
		/// </value>
		public ICollection<T> Items { get; set; }
		
		/// <summary>
		/// Setups the contex.
		/// </summary>
		[SetUp]
		public void SetupContex()
		{
			new SchemaExport(this._configuration).Execute(false, true, false);
			this.LoadData();
		}
		
		/// <summary>
		/// Tests the fixture set up.
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var rawConfig = new Configuration();
			
			// NOTE: Removed but left in place as a reminder.
            //rawConfig.SetNamingStrategy(new MsSqlNamingStrategy());

            this._configuration = Fluently.Configure(rawConfig)
                .Database(NHibernateConfiguration.CreateDatabaseConfiguration(Global.ConnectionString, Global.DatabaseProvider))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateConfiguration>())
                .BuildConfiguration();

            this.SessionFactory = this._configuration.BuildSessionFactory();
		}
		
		/// <summary>
		/// Loads the data.
		/// </summary>
		private void LoadData()
		{
			using (ISession session = this.SessionFactory.OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					foreach (var item in this.Items)
					{
						session.Save(item);
					}
					
					transaction.Commit();
				}
			}
		}
	}
}