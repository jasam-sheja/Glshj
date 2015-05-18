using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Drag
{
    public class DragVertexer:Vertexer
    {
        public Vertexer source;
        public Vertexer destinaion;

        public DragVertexer(Vertexer source, Vertexer destinaion)
        {
            this.source = source;
            this.destinaion = destinaion;
        }

        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[Count];
            Vector4[] firstdata = source.getData();
            Vector4[] seconddata = destinaion.getData();
            firstdata.CopyTo(data, 0);
            seconddata.CopyTo(data, firstdata.Length);
            return data;
        }

        public override int Count
        {
            get { return source.Count + destinaion.Count; }
        }
    }
}
