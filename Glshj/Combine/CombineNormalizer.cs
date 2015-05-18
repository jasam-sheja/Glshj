using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Combine
{
    public class CombineNormalizer : Normalizer
    {
        public Normalizer first, second;
        public CombineNormalizer(Normalizer first, Normalizer second)
        {
            this.first = first;
            this.second = second;
        }

        public override Vector3[] getData()
        {
            Vector3[] firstvertex = first.getData();
            Vector3[] secondvertex = second.getData();
            Vector3[] data = new Vector3[firstvertex.Length + secondvertex.Length];
            firstvertex.CopyTo(data, 0);
            secondvertex.CopyTo(data, firstvertex.Length);
            return data;
        }
    }
}
