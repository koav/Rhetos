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
using Autofac.Features.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhetos.Extensibility.Test
{
    public class PluginsContainerAccessor<TPlugin> : PluginsContainer<TPlugin>
    {
        public PluginsContainerAccessor()
            : base (
                new Lazy<IEnumerable<TPlugin>> { },
                new Lazy<Autofac.Features.Indexed.IIndex<Type, IEnumerable<TPlugin>>> { },
                new PluginsMetadataCache<TPlugin>(new Lazy<IEnumerable<Meta<TPlugin>>> { }))
        {
        }

        public static List<Type> Access_GetTypeHierarchy(Type type)
        {
            return GetTypeHierarchy(type);
        }
    }
}
