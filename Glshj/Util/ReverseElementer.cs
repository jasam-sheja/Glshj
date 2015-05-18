using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glshj.Abstract;

namespace Glshj.Util
{
    public class ReverseElementer:Elementer
    {
        private Elementer base_;
        private int index, count;
        public ReverseElementer(Elementer base_):
            this(base_, -1,-1)
        {
        }

        public ReverseElementer(Elementer base_, int index, int count)
        {
            this.base_ = base_;
            this.index = index;
            this.count = count;
        }

        public override short[] getData()
        {
            short[] data = base_.getData();
            if(index < 0)
                index = 0;
            if(count<0)
                count = data.Length-index;
            Array.Reverse(data,index,count);
            return data;
        }
    }
}
