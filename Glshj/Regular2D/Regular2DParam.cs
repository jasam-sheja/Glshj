using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glshj.Abstract;

namespace Glshj.Regural2D
{
    public class Regular2DParam:CountParam
    {
        public float radius;
        public int count;
        public float start;

        public Regular2DParam(float radius, int count, float start=0)
        {
            this.radius = radius;
            this.count = count;
            this.start = start;
        }

        public int Count
        {
            get { return count; }
        }
    }
}
