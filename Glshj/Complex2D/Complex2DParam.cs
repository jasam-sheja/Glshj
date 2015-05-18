using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Complex2D
{
    public class Complex2DParam:CountParam
    {
        public Vertexer base_;
        public Complex2DParam(Vertexer base_)
        {
            this.base_ = base_;
        }

        public int Count
        {
            get { return base_.Count + 1; }
        }
    }
}
