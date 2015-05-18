using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;
using Glshj.Util;

namespace Glshj.Combine
{
    public class CombineElementer:Elementer
    {
        public Elementer first, second;
        public CombineElementer(Elementer first, Elementer second)
        {
            this.first = first;
            this.second = second;
        }

        public override short[] getData()
        {
            short[] firstvertex = first.getData();
            short[] secondvertex = second.getData();
            short[] data = new short[firstvertex.Length + secondvertex.Length];
            firstvertex.CopyTo(data, 0);
            ElementShifter.Shift(secondvertex, (short)firstvertex.Length).CopyTo(data, firstvertex.Length);
            return data;
        }
    }
}
