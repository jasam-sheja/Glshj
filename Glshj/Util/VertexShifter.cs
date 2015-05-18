using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class VertexShifter:Vertexer
    {
        private Vertexer base_;
        public Vector3 shift;
        public VertexShifter(Vertexer base_, Vector3 shift)
        {
            this.base_ = base_;
            this.shift = shift;
        }

        public override Vector4[] getData()
        {
            return Shift(base_.getData(), shift);
        }

        public override int Count
        {
            get { return base_.Count; }
        }

        public static Vector4[] Shift(Vector4[] data, Vector3 shift)
        {
            for (int i = 0; i < data.Length; i++)
                data[i].Xyz += shift;
            return data;
        }
    }
}
