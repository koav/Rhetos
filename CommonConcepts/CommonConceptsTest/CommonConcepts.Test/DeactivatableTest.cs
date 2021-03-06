﻿/*
    Copyright (C) 2013 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TestDeactivatable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos.Utilities;
using Rhetos.TestCommon;
using Rhetos.Dom.DefaultConcepts;

namespace CommonConcepts.Test
{
    [TestClass]
    public class DeactivatableTest
    {
        [TestMethod]
        public void ActivePropertyValueDoesNotHaveToBeDefinedOnInsert()
        {
            using (var executionContext = new CommonTestExecutionContext())
            {
                executionContext.SqlExecuter.ExecuteSql(new[] {
                    "DELETE FROM TestDeactivatable.BasicEnt" });
                var repository = new Common.DomRepository(executionContext);
                var entity = new BasicEnt { Name = "ttt" };
                repository.TestDeactivatable.BasicEnt.Insert(new[] { entity });
                Assert.AreEqual(true, repository.TestDeactivatable.BasicEnt.All().Single().Active);
            }
        }

        [TestMethod]
        public void ActivePropertyValueDoesNotHaveToBeDefinedOnUpdate()
        {
            using (var executionContext = new CommonTestExecutionContext())
            {
                var id1 = Guid.NewGuid();
                var id2 = Guid.NewGuid();
                var id3 = Guid.NewGuid();
                executionContext.SqlExecuter.ExecuteSql(new[] {
                    "DELETE FROM TestDeactivatable.BasicEnt",
                    "INSERT INTO TestDeactivatable.BasicEnt (ID, Name) VALUES (" + SqlUtility.QuoteGuid(id1) + ", 'a')",
                    "INSERT INTO TestDeactivatable.BasicEnt (ID, Name, Active) VALUES (" + SqlUtility.QuoteGuid(id2) + ", 'b', 0)",
                    "INSERT INTO TestDeactivatable.BasicEnt (ID, Name) VALUES (" + SqlUtility.QuoteGuid(id3) + ", 'c')"
                });
                var repository = new Common.DomRepository(executionContext);
                var e1 = new BasicEnt { ID = id1, Name = "a2", Active = false };
                var e2 = new BasicEnt { ID = id2, Name = "b2" };
                var e3 = new BasicEnt { ID = id3, Name = "c2" };
                repository.TestDeactivatable.BasicEnt.Update(new[] { e1, e2, e3});

                var afterUpdate = repository.TestDeactivatable.BasicEnt.All();
                Assert.AreEqual(
                    "a2 False, b2 False, c2 True",
                    TestUtility.DumpSorted(afterUpdate, item => item.Name + " " + item.Active));
            }
        }

        [TestMethod]
        public void ItIsOkToInsertInactiveOrActiveItem()
        {
            using (var executionContext = new CommonTestExecutionContext())
            {
                var repository = new Common.DomRepository(executionContext);
                var entity = new BasicEnt { Name = "ttt", Active = false };
                var entity2 = new BasicEnt { Name = "ttt2", Active = true };
                repository.TestDeactivatable.BasicEnt.Insert(new[] { entity, entity2 });
            }
        }

        [TestMethod]
        public void TestForActiveItemsFilter()
        {
            using (var executionContext = new CommonTestExecutionContext())
            {
                executionContext.SqlExecuter.ExecuteSql(new[] { 
                    "DELETE FROM TestDeactivatable.BasicEnt;",
                });
                var repository = new Common.DomRepository(executionContext);
                var entity = new BasicEnt { Name = "ttt", Active = false };
                var entity2 = new BasicEnt { Name = "ttt2", Active = true };
                
                repository.TestDeactivatable.BasicEnt.Insert(new[] { entity, entity2 });
                Assert.AreEqual(1, repository.TestDeactivatable.BasicEnt.Filter(new ActiveItems()).Count());
            }
        }

        [TestMethod]
        public void TestForThisAndActiveItemsFilter()
        {
            using (var executionContext = new CommonTestExecutionContext())
            {
                executionContext.SqlExecuter.ExecuteSql(new[] { 
                    "DELETE FROM TestDeactivatable.BasicEnt;",
                });
                var e1ID = Guid.NewGuid();
                var repository = new Common.DomRepository(executionContext);
                var entity = new BasicEnt { ID = e1ID, Name = "ttt", Active = false };
                var entity2 = new BasicEnt { Name = "ttt2", Active = true };
                var entity3 = new BasicEnt { Name = "ttt3", Active = false };

                repository.TestDeactivatable.BasicEnt.Insert(new[] { entity, entity2, entity3 });
                Assert.AreEqual(1, repository.TestDeactivatable.BasicEnt.Filter(new ActiveItems()).Count());
                Assert.AreEqual(2, repository.TestDeactivatable.BasicEnt.Filter(new ActiveItems { ItemID = e1ID }).Count());
            }
        }
    }
}
