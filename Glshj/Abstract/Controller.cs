using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public class Controller : Destroyable
    {
        protected int vao;
        protected int vbo;
        protected int[] ntc = new int[] { -1, -1, -1 };
        protected int ebo = -1;
        protected int tex = -1;

        public Controller(
            Normalizer normalizer,
            Elementer elementer,
            Colorer colorer,
            Textureer textrureer)
        {
            this.normalizer = normalizer;
            this.elementer = elementer;
            this.colorer = colorer;
            this.textrureer = textrureer;
        }

        public void genBuffers(int vbo=-1, bool rebuffer=false)
        {
            if (vbo > 0)
                setVbo(vbo);
            else
                vbo = this.vbo;
            clearBuffers();

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(4, VertexPointerType.Float, 0, 0);

            if (normalizer != null)
            {
                if (normalizer.Buffer < 1 || rebuffer)
                    ntc[0] = normalizer.genBuffer();
                else
                    ntc[0] = normalizer.Buffer;
                GL.BindBuffer(BufferTarget.ArrayBuffer, ntc[0]);
                GL.NormalPointer(NormalPointerType.Float, 0, 0);
                GL.EnableClientState(ArrayCap.NormalArray);                
            }

            if (textrureer != null)
            {
                if (textrureer.Buffer < 1 || rebuffer)
                    ntc[1] = textrureer.genBuffer();
                else
                    ntc[1] = textrureer.Buffer;
                GL.BindBuffer(BufferTarget.ArrayBuffer, ntc[1]);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);            
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                tex = textrureer.getTexture();
                GL.BindTexture(TextureTarget.Texture2D, tex);
            }

            if (colorer != null)
            {
                if (colorer.Buffer < 1 || rebuffer)
                    ntc[2] = colorer.genBuffer();
                else
                    ntc[2] = colorer.Buffer;
                GL.BindBuffer(BufferTarget.ArrayBuffer, ntc[2]);
                GL.ColorPointer(4, ColorPointerType.Float, 0, 0);
                GL.EnableClientState(ArrayCap.ColorArray);
            }

            if (elementer != null)
            {
                if (elementer.Buffer < 1 || rebuffer)
                    ebo = elementer.genBuffer();
                else
                    ebo = elementer.Buffer;
            }

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void setVbo(int vbo)
        {
            this.vbo = vbo;
        }

        public void onDraw()
        {
            GL.BindVertexArray(vao);
            if (haveTexture && haveTextcroods)
            {
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.Enable(EnableCap.Texture2D);
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        }
        public void afterDraw()
        {
            GL.BindVertexArray(0);
            if (haveTexture && haveTextcroods)
            {
                GL.Color3(Color.White);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Texture2D);
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        protected void clearBuffers()
        {
            if (vao > 0)
                GL.DeleteVertexArray(vao);
            vao = 0;
            ntc = new int[] { -1, -1, -1 };
            ebo = -1;
            tex = 0;
        }


        public bool haveTexture { get { return tex > 0; } }
        public bool haveTextcroods { get { return ntc[1] > 0; } }
        public bool haveNormals { get { return ntc[0] > 0; } }
        public bool haveColors { get { return ntc[2] > 0; } }

        public Elementer elementer;
        public Colorer colorer;
        public Textureer textrureer;
        public Normalizer normalizer;

        public void onDestroy()
        {
            if (vao > 0)
                GL.DeleteVertexArray(vao);
            vao = 0;
        }
    }
}
