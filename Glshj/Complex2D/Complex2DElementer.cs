using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Complex2D
{
    public class Complex2DElementer:Elementer
    {
        public Complex2DParam param;
        public Complex2DElementer(Complex2DParam param)
        {
            this.param = param;
        }

        public override short[] getData()
        {
            short[] data = new short[param.Count + 1];
            for (short i = 0; i < data.Length-1; i++)
                data[i] = i;
            data[data.Length - 1] = 1;
            return data;
        }
    }
}
