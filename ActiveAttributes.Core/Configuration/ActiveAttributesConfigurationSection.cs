﻿// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.

using System;
using System.Configuration;
using System.Xml;
using JetBrains.Annotations;

namespace ActiveAttributes.Configuration
{
  [UsedImplicitly]
  public sealed class ActiveAttributesConfigurationSection : ConfigurationSection
  {
    [ConfigurationProperty ("xmlns")]
    public string XmlNamespace
    {
      get { throw new NotSupportedException(); }
    }

    [ConfigurationProperty ("ignoreAssemblyDefinedRules")]
    public bool IgnoreAssemblyDefinedRules
    {
      get { return (bool) base["ignoreAssemblyDefinedRules"]; }
    }

    [ConfigurationProperty ("typeRules")]
    public TypeOrderingCollection TypeOrderings
    {
      get { return (TypeOrderingCollection) base["typeRules"]; }
    }

    protected override bool OnDeserializeUnrecognizedElement (string elementName, XmlReader reader)
    {
      var message = "Unknown element name: " + elementName + Environment.NewLine +
                    "Example configuration: " + Environment.NewLine +
                    "<aspects xmlns=\"http://activeattributes.codeplex.com/configuration/schema.xsd\">" + Environment.NewLine +
                    "  <typeRules>" + Environment.NewLine +
                    "    <add beforeType=\"MyAssembly.Type1, MyAssembly\"" + Environment.NewLine +
                    "         afterType=\"MyAssembly.Type2, MyAssembly\" />" + Environment.NewLine +
                    "  </typeRules>" + Environment.NewLine +
                    "</aspects>";
      throw new ConfigurationErrorsException (message);
    }
  }
}