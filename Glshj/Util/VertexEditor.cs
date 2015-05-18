using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class VertexEditor:Vertexer
    {
        private Vertexer base_;
        public EditorParam param;
        public VertexEditor(Vertexer base_, EditorParam param)
        {
            this.base_ = base_;
            this.param = param;
        }

        public override Vector4[] getData()
        {
            return Shift(base_.getData(), param.Matrix);
        }

        public override int Count
        {
            get { return base_.Count; }
        }

        public static Vector4[] Shift(Vector4[] data, Matrix4 matrix)
        {
            for (int i = 0; i < data.Length; i++)
                data[i].Xyz = Vector3.Transform(data[i].Xyz, matrix); ;
            return data;
        }
    }
}
