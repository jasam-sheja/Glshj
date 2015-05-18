using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Regural2D
{
    public class Regular2DVertexer : Vertexer
    {
        public Regular2DParam param;
        public Regular2DVertexer(Regular2DParam param)
        {
            this.param = param;
        }

        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[param.count];
            int i = 0;
            for (double a = param.start; a < MathHelper.TwoPi + param.start; a += MathHelper.TwoPi / param.count)
                data[i++] = new Vector4((float)Math.Cos(a)*param.radius, (float)Math.Sin(a)*param.radius, 0, 1);
            return data;
        }

        public override int Count
        {
            get { return param.Count; }
        }
    }
}
