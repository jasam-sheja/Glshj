using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public abstract class Elementer : Destroyable
    {
        protected int buff = -1;

        public abstract short[] getData();
        public virtual int Buffer { get { return buff; } }
        public int genBuffer()
        {
            short[] data = getData();
            if (buff < 1)
                buff = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buff);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(data.Length * sizeof(short)), data, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (data.Length * sizeof(short) != size)
                throw new ApplicationException("Element data not uploaded correctly");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            return buff;
        }

        public void onDestroy()
        {
            if (buff > 0)
                GL.DeleteBuffer(buff);
            buff = 0;
        }
    }
}
