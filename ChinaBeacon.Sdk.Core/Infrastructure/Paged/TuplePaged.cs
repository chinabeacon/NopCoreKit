using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaBeacon.Sdk.Core.Infrastructure.Paged
{
    public class TuplePaged<T> : Tuple<List<T>, int>
    {
        public TuplePaged(List<T> data, int count) : base(data, count)
        {
        }
    }
}
