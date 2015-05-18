using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Combine
{
    public class CombineColorer:Colorer
    {
        public Colorer first, second;
        public CombineColorer(Colorer first, Colorer second)
        {
            this.first = first;
            this.second = second;
        }

        public override Vector4[] getData()
        {
            Vector4[] firstvertex = first.getData();
            Vector4[] secondvertex = second.getData();
            Vector4[] data = new Vector4[firstvertex.Length+secondvertex.Length];
            firstvertex.CopyTo(data, 0);
            secondvertex.CopyTo(data, firstvertex.Length);
            return data;
        }
    }
}
