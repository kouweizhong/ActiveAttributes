// Copyright (c) rubicon IT GmbH, www.rubicon.eu
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
using System.Collections.Generic;
using System.Linq;
using ActiveAttributes.Core.Assembly.Configuration;
using ActiveAttributes.Core.Assembly.Configuration.Configurators;

namespace ActiveAttributes.Core.Configuration2
{
  public class ActiveAttributeConfigurationProvider : IActiveAttributeConfigurationProvider
  {
    private readonly IEnumerable<IActiveAttributesConfigurator> _configurators;

    public ActiveAttributeConfigurationProvider (IEnumerable<IActiveAttributesConfigurator> configurators)
    {
      _configurators = configurators;
    }

    public IActiveAttributesConfiguration GetConfiguration ()
    {
      var configuration = new ActiveAttributesConfiguration();

      var sortedConfigurators = _configurators.BringToFront (x => x is ApplicationConfigurationConfigurator);
      foreach (var configurator in sortedConfigurators)
      {
        configurator.Initialize (configuration);
        if (configuration.IsLocked)
          break;
      }

      return configuration;
    }
  }

  public static class Extensions
  {
    public static IEnumerable<T> BringToFront<T> (this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
      var list = enumerable.ToList();
      return list.Where (predicate).Concat (list.Where (x => !predicate (x)));
    }
  }
}