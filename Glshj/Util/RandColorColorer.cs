using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class RandColorColorer:Colorer
    {
        Vector3 colorlower, colorupper;
        CountParam count;

        public RandColorColorer(Vector3 colorlower, Vector3 colorupper, int count, bool gen = false) :
            this(colorlower, colorupper, new CountPram_color(count), gen)
        {
        }

        public RandColorColorer(Color colorlower,Color colorupper, int count, bool gen = false) :
            this(new Vector3(colorlower.R / 255f, colorlower.G / 255f, colorlower.B / 255f),
                new Vector3(colorupper.R / 255f, colorupper.G / 255f, colorupper.B / 255f), 
                new CountPram_color(count), gen)
        {
        }

        public RandColorColorer(Color colorlower,Color colorupper, CountParam count, bool gen = false):
            this(new Vector3(colorlower.R / 255f, colorlower.G / 255f, colorlower.B / 255f),
                new Vector3(colorupper.R / 255f, colorupper.G / 255f, colorupper.B / 255f), count, gen)
        {
        }

        public RandColorColorer(Vector3 colorlower, Vector3 colorupper, CountParam count, bool gen = false)
        {
            this.colorlower = colorlower;
            this.colorupper = colorupper;
            this.count = count;
            if (gen)
                genBuffer();
        }

        public override Vector4[] getData()
        {
            Random r = new Random();
            Vector4[] data = new Vector4[count.Count];
            for (int i = 0; i < data.Length; i++)
                data[i] = new Vector4(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()) * (colorupper - colorlower) + colorlower, 1);
            return data;
        }
    }
}
