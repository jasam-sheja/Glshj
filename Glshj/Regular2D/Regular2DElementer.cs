using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Regural2D
{
    public class Regular2DElementer:Elementer
    {
        public Regular2DParam param;
        public Regular2DElementer(Regular2DParam param)
        {
            this.param = param;
        }

        public override short[] getData()
        {
            short[] data = new short[param.Count];
            for (short i = 0; i < data.Length; i++)
                data[i] = i;
            return data;
        }
    }
}
