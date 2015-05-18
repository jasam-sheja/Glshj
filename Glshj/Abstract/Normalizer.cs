using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public abstract class Normalizer : Destroyable
    {
        protected int buff = -1;

        public abstract Vector3[] getData();
        public virtual int Buffer { get { return buff; } }
        public int genBuffer()
        {
            Vector3[] data = getData();
            if (buff < 1)
                buff = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buff);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes), data, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (data.Length * Vector3.SizeInBytes != size)
                throw new ApplicationException("Normaliz data not uploaded correctly");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

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
