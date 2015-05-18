using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glshj.Abstract;

namespace Glshj.Curve
{
    public class CurveElementer:Elementer
    {
        public CurveParam param;

        public CurveElementer(CurveParam param)
        {
            this.param = param;
        }

        public override short[] getData()
        {
            short[] res = new short[param.Count];
            for (short i = 0; i < res.Length; i++)
                res[i] = i;
            return res;
        }
    }
}
