using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glshj.Abstract;

namespace Glshj.Util
{
    public class LoopElementer : Elementer
    {
        private Elementer base_;
        public short count;
        public LoopElementer(Elementer base_, short count)
        {
            this.base_ = base_;
            this.count = count;
        }

        public override short[] getData()
        {
            short[] base_data = base_.getData();
            short[] data = new short[base_data.Length + count];
            base_data.CopyTo(data, 0);
            Array.Copy(base_data, 0, data, base_data.Length, count);
            return data;
        }
    }
}
