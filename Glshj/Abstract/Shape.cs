using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public class Shape:Drawable
    {
        private Vertexer vertexer;
        private List<Controller> controls = new List<Controller>();
        private List<PrimitiveType> DefaultTypes = new List<PrimitiveType>();
        private int vbo = -1;
        
        public Shape(Vertexer vertexer, Controller controller, PrimitiveType defaultmode = PrimitiveType.Points)
        {
            setVertexer(vertexer);
            addController(controller, defaultmode);
        }

        public void addController(Controller controller, PrimitiveType defaultmode = PrimitiveType.Points)
        {
            controls.Add(controller);
            controller.genBuffers(vbo);
            DefaultTypes.Add(defaultmode);
        }

        public Controller getController(int i)
        {
            return controls.ElementAt(i) ;
        }

        public void reBuffer()
        {
            reBufferVertex();
            reBufferControl();
        }

        public void reBufferVertex()
        {
            vbo = vertexer.genBuffer();
        }
        public void reBufferControl(int i = -1)
        {
            if (i < 0)
                foreach (Controller controller in controls)
                    controller.genBuffers(vbo, true);
            else
                controls.ElementAt(i).genBuffers(vbo, true);
        }

        public int getControlsCount()
        {
            return controls.Count;
        }

        public void setVertexer(Vertexer vertexer)
        {
            this.vertexer = vertexer;
            if (vertexer.Buffer < 1)
            {
                vbo = vertexer.genBuffer();
            }
            else
            {
                vbo = vertexer.Buffer;
            }
        }

        public Vertexer getVertexer()
        {
            return vertexer;
        }

        public void Draw()
        {
            Draw(DefaultTypes.ToArray());
        }

        public void Draw(params PrimitiveType[] mode)
        {
            for (short i = 0; i < mode.Length; i++)
            {
                Draw(i, mode[i]);
            }
        }

        public void Draw(params short[] index)
        {
            foreach (short idx in index)
            {
                Draw(idx, DefaultTypes.ElementAt(idx));
            }
        }

        public void Draw(short index, PrimitiveType mode)
        {
            Draw(index, mode, -1, -1);
        }

        public void Draw(short[] index, PrimitiveType[] mode)
        {
            for (int i = 0; i < index.Length; i++)
            {
                Draw(index[i], mode[i], -1, -1);
            }
        }

        public void Draw(short index, PrimitiveType mode, int count, int indeces)
        {
            Controller controller = controls.ElementAt(index);
            controller.onDraw();
            int size;
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (indeces < 0)
                indeces = 0;
            if (count < 1)
                count = size / sizeof(short)-indeces;

            if ((indeces + count) * sizeof(short) > size)
                return;
            GL.DrawElements(mode, count, DrawElementsType.UnsignedShort, indeces * sizeof(short));
            controller.afterDraw();
        }

        public void Draw(short[] index, PrimitiveType[] mode, int[] count, int[] indeces)
        {
            for (int i = 0; i < index.Length; i++)
            {
                Draw(index[i], mode[i], count[i], indeces[i]);
            }
        }
    }
}
