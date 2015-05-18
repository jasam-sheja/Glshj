using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class NormalEditor:Normalizer
    {
        private Normalizer base_;
        public EditorParam param;
        public NormalEditor(Normalizer base_, EditorParam param)
        {
            this.base_ = base_;
            this.param = param;
        }

        public override Vector3[] getData()
        {
            return Shift(base_.getData(), param.Matrix);
        }

        public static Vector3[] Shift(Vector3[] data, Matrix4 matrix)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = Vector3.Transform(data[i], matrix); ;
            return data;
        }
    }
}
