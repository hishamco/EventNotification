﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Framework.Internal;

namespace Microsoft.Framework.Notification.Internal
{
    public class ProxyList<TSourceElement, TTargetElement> : IReadOnlyList<TTargetElement>
    {
        private readonly IList<TSourceElement> _source;
        private readonly Type _proxyType;

        public ProxyList([NotNull] IList<TSourceElement> source)
            : this(source, null)
        {
        }

        protected ProxyList([NotNull] IList<TSourceElement> source, Type proxyType)
        {
            _source = source;
            _proxyType = proxyType;
        }

        public TTargetElement this[int index]
        {
            get
            {
                var element = _source[index];
                return MakeProxy(element);
            }
        }

        public int Count
        {
            get
            {
                return _source.Count;
            }
        }

        public IEnumerator<TTargetElement> GetEnumerator()
        {
            return new ProxyEnumerable<TSourceElement, TTargetElement>.ProxyEnumerator(_source.GetEnumerator(), _proxyType);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private TTargetElement MakeProxy(TSourceElement element)
        {
            if (_proxyType == null)
            {
                return (TTargetElement)(object)element;
            }
            else
            {
                return (TTargetElement)Activator.CreateInstance(
                    _proxyType,
                    new object[] { element });
            }
        }
    }
}
