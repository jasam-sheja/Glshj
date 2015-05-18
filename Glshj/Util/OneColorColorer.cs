using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class OneColorColorer:Colorer
    {
        private Vector4 color;
        private CountParam count;

        public OneColorColorer(Vector4 color, int count, bool gen=false):
            this(color, new CountPram_color(count), gen)
        {
        }

        public OneColorColorer(Color color, int count, bool gen = false) :
            this(new Vector4(color.R/255f, color.G/255f, color.B/255f, color.A/255f), new CountPram_color(count), gen)
        {
        }

        public OneColorColorer(Color color, CountParam count, bool gen = false):
            this(new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f), count, gen)
        {
        }

        public OneColorColorer(Vector4 color, CountParam count, bool gen = false)
        {
            this.color = color;
            this.count = count;
            if (gen)
                genBuffer();
        }

        public override Vector4[] getData()
        {
            Vector4[] res = new Vector4[count.Count];
            Fill(res, color);
            return res;
        }

        private static void Fill(Vector4[] outarray, Vector4 value)
        {
            for (int i = 0; i < outarray.Length; i++)
                outarray[i] = value;
        }
    }

    class CountPram_color:CountParam
    {
        int count;
        public CountPram_color(int count){
            this.count = count;
        }

        public int  Count
        {
	        get { return count; }
        }
    }
}
