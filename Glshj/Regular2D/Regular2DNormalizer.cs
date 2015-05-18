using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Regural2D
{
    public class Regular2DNormalizer:Normalizer
    {
        public Regular2DParam param;
        public Regular2DNormalizer(Regular2DParam param)
        {
            this.param = param;
        }

        public override Vector3[] getData()
        {
            Vector3[] data = new Vector3[param.Count];
            for (int i = 0; i < data.Length; i++)
                data[i] = Vector3.UnitZ;
            return data;
        }
    }
}
