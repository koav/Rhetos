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
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("AutoCodeForEach")]
    public class AutoCodeForEachInfo : AutoCodePropertyInfo, IMacroConcept, IValidationConcept
    {
        public PropertyInfo Group { get; set; }

        public IEnumerable<IConceptInfo> CreateNewConcepts(IEnumerable<IConceptInfo> existingConcepts)
        {
            var newConcepts = new List<IConceptInfo>
                             {
                                 new RequiredPropertyInfo {Property = Property},
                                 new UniquePropertiesInfo
                                     {DataStructure = Property.DataStructure, Property1 = Group, Property2 = Property}
                             };

            return newConcepts;
        }

        public void CheckSemantics(IEnumerable<IConceptInfo> concepts)
        {
            if (Group.DataStructure != Property.DataStructure)
                throw new DslSyntaxException("AutoCodeForEach grouping property " + Group.GetKeyProperties()
                    + " is not a member of " + Property.DataStructure.GetUserDescription() + ".");
        }
    }
}
