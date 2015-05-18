using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Combine
{
    public class CombineVertexer:Vertexer
    {
        public Vertexer first, second;
        public CombineVertexer(Vertexer first, Vertexer second)
        {
            this.first = first;
            this.second = second;
        }

        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[Count];
            Vector4[] firstvertex = first.getData();
            Vector4[] secondvertex = second.getData();
            firstvertex.CopyTo(data, 0);
            secondvertex.CopyTo(data, firstvertex.Length);
            return data;
        }

        public override int Count
        {
            get { return first.Count+second.Count; }
        }
    }
}
