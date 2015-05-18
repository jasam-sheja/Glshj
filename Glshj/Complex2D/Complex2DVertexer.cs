using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Complex2D
{
    public class Complex2DVertexer:Vertexer
    {
        public Complex2DParam param;
        public Complex2DVertexer(Complex2DParam param)
        {
            this.param = param;
        }

        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[Count];
            param.base_.getData().CopyTo(data, 1);
            data[0] = new Vector4(0, 0, 0, 1);
            return data;
        }

        public override int Count
        {
            get { return param.Count; }
        }
    }
}
