using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Glshj.Abstract;
using Glshj.Curve;
using Glshj.Regural2D;
using Glshj.Complex2D;
using Glshj.Combine;
using Glshj.Util;

namespace TestGljsh
{
    class Test1:GameWindow
    {
        Vector3 pos = Vector3.Zero;
        Vector3 rot = Vector3.Zero;

        #region shapes
        List<Destroyable> destroy = new List<Destroyable>();
        private void addToDestroy(Shape shape)
        {
            destroy.Add(shape.getVertexer());
            for (int i = 0; i < shape.getControlsCount(); i++)
            {
                Controller controller = shape.getController(i);
                destroy.Add(controller);
                if (controller.normalizer != null)
                    destroy.Add(controller.normalizer);
                if (controller.elementer != null)
                    destroy.Add(controller.elementer);
                if (controller.textrureer != null)
                    destroy.Add(controller.textrureer);
                if (controller.colorer != null)
                    destroy.Add(controller.colorer);
            }
        }
        

        Shape fib;
        Shape fibfib;
        Shape squar;
        Shape tear;
        Shape mousetrack;

        MouseTrackParam mousetrack_param;


        #endregion

        public Test1() : base(800, 600) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeyDown += Keyboard_KeyDown;
            MouseMove += Mouse_Move;
            MouseDown += Mouse_ButtonDown;
            Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(Test1_Closing);

            Regular2DParam squar_param = new Regular2DParam(10, 36, MathHelper.PiOver4);
            squar = new Shape(new VertexEditor(new Regular2DVertexer(squar_param), new EditorParam(Matrix4.CreateRotationX(MathHelper.PiOver4))), new Controller(null, new Regular2DElementer(squar_param), new OneColorColorer(new Vector4(0, 0, 1, 1), squar_param.count), null), PrimitiveType.Polygon);
            squar.addController(new Controller(null, squar.getController(0).elementer, null, new Regular2DTextureer(squar_param, "tex/sqr.jpg", -MathHelper.PiOver4)), PrimitiveType.Polygon);
            addToDestroy(squar);

            Fib fib_vertexer_param = new Fib(10);
            fib = new Shape(fib_vertexer_param, new Controller(null, new CurveElementer(fib_vertexer_param), new OneColorColorer(new Vector4(0.5f, 1, 1, 1), 10), null), PrimitiveType.LineStrip);
            addToDestroy(fib);

            fibfib = new Shape(new CombineVertexer(fib.getVertexer(), new VertexShifter(fib.getVertexer(), Vector3.UnitX * 2)),
                new Controller(null, new CombineElementer(fib.getController(0).elementer, new ReverseElementer(fib.getController(0).elementer)),
                                    new CombineColorer(fib.getController(0).colorer, fib.getController(0).colorer), null), PrimitiveType.LineStrip);
            addToDestroy(fibfib);

            Complex2DParam tear_param = new Complex2DParam(new TearDrop(6, 36));
            tear = new Shape(new Complex2DVertexer(tear_param), new Controller(null, new Complex2DElementer(tear_param), new OneColorColorer(new Vector4(1, 0, 0, 1), tear_param.base_.Count + 1), null), PrimitiveType.TriangleFan);
            addToDestroy(tear);
      
            mousetrack_param = new MouseTrackParam();
            MouseDown += mousetrack_param.Mouse_ButtonDown;
            MouseUp += mousetrack_param.Mouse_ButtonUp;
            MouseMove += mousetrack_param.Mouse_Move;
            mousetrack = new Shape(new MouseTrack(mousetrack_param), new Controller(null, new CurveElementer(mousetrack_param), new OneColorColorer(Color.White, mousetrack_param), null), PrimitiveType.LineStrip);
            addToDestroy(mousetrack);

            GL.ClearColor(System.Drawing.Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.FrontAndBack);
        }

        void Test1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Destroyable des in destroy)
                des.onDestroy();
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {

            }
        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (e.Mouse[MouseButton.Left])
            {

            }
        }
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Exit();
            }
            else if (e.Key == Key.Up)
                pos.Y += 1;
            else if (e.Key == Key.Down)
                pos.Y -= 1;
            else if (e.Key == Key.Right)
                pos.X += 1;
            else if (e.Key == Key.Left)
                pos.X -= 1;
            else if (e.Key == Key.Keypad1)
                pos.Z += 1;
            else if (e.Key == Key.Keypad0)
                pos.Z -= 1;
            else if (e.Key == Key.Keypad4)
                rot.X += 5;
            else if (e.Key == Key.Keypad5)
                rot.Y += 5;
            else if (e.Key == Key.Keypad6)
                rot.Z += 5;
            else if (e.Key == Key.Keypad7)
                rot.X -= 5;
            else if (e.Key == Key.Keypad8)
                rot.Y -= 5;
            else if (e.Key == Key.Keypad9)
                rot.Z -= 5;
            System.Console.WriteLine(e.Key.ToString());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, aspect_ratio, 1, 1000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            this.Title = " FPS: " + string.Format("{0:F}", 1.0 / e.Time);
            if (mousetrack_param.update)
                mousetrack.reBuffer();            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(0, 0, 35, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Translate(pos);
            GL.Rotate(rot.X, Vector3.UnitX);
            GL.Rotate(rot.Y, Vector3.UnitY);
            GL.Rotate(rot.Z, Vector3.UnitZ);
            //GL.Scale(2.5, 2.5, 2.5);
            //GL.Begin(PrimitiveType.Quads);

            //GL.Color3(Color.Green);
            //GL.Vertex2(-10.0f, 10.0f);
            //GL.Color3(Color.SpringGreen);
            //GL.Vertex2(-10.0f, -10.0f);
            //GL.Color3(Color.Ivory);
            //GL.Vertex2(10.0f, -10.0f);
            //GL.Color3(Color.Indigo);
            //GL.Vertex2(10.0f, 10.0f);

            //GL.End();
            
            fib.Draw();
            mousetrack.Draw();
            
            //GL.PushMatrix();
            //GL.Translate(-8, -1, 0);
            //fibfib.Draw();
            //GL.PopMatrix();

            //GL.PushMatrix();
            //GL.Translate(0, 0, -2);
            //GL.Scale(5, 5, 0);
            //squar.Draw(1);
            //GL.PopMatrix();

            //GL.PushMatrix();
            //GL.Translate(1.5, 0, 0);
            //tear.Draw();
            //GL.Translate(1, 0, 0);
            //tear.Draw(PrimitiveType.Points);
            //GL.Translate(1, 0, 0);
            //tear.Draw(PrimitiveType.LineStrip);
            //GL.Translate(1, 0, 0);
            //tear.Draw(0, PrimitiveType.LineStrip, -1, 1);
            //GL.PopMatrix();

            //GL.PushMatrix();
            //GL.Translate(-1.5, 0, 0);
            //tear.Draw(PrimitiveType.Lines);
            //GL.Translate(-1, 0, 0);
            //tear.Draw(PrimitiveType.Quads);
            //GL.Translate(-1, 0, 0);            
            //tear.Draw(PrimitiveType.Triangles);
            //GL.Translate(-1, 0, 0);
            //tear.Draw(PrimitiveType.TriangleStrip);
            //GL.PopMatrix();

            this.SwapBuffers();
        }
    }

    class Fib : Vertexer, CurveParam
    {
        int count;
        int offset;
        public Fib(int count, int offset=0)
        {
            this.count = count;
            this.offset = offset;
            genBuffer();
        }

        public override Vector4[] getData()
        {
            Vector4[] res = new Vector4[count+offset];
            res[0] = new Vector4(0, 0, 0, 1);
            res[1] = new Vector4(1, 1, 0, 1);
            for(int i=2;i<res.Length;i++)
                res[i] = new Vector4(i, res[i-1].Y + res[i-2].Y, 0, 1);
            return SubArray(res, offset, count);
        }

        public override int Count
        {
            get { return count; }
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    class TearDrop : Vertexer
    {
        private float m;
        private int count;
        public TearDrop(float m, int count)
        {
            this.m = m;
            this.count = count;
        }
        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[count];
            for(int i=0;i<count;i++){
                double t = MathHelper.TwoPi*i/(float)count;
                data[i] = new Vector4((float)(Math.Sin(t) * Math.Pow(Math.Sin(t / 2), m)), (float)Math.Cos(t), 0, 1);
            }
            return data;
        }

        public override int Count
        {
            get { return count; }
        }
    }

    #region mouseTrack

    class MouseTrackParam:CurveParam
    {
        public List<Vector4> pos = new List<Vector4>();
        public bool add = false;
        public bool update = false;

        public int Count
        {
            get { return pos.Count; }
        }

        public void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                add = true;
            }
        }

        public void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                add = false;
            }
        }

        private int modulo = 0;
        public int windowWidth = 800;
        public int windowHeight = 600;
        public void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (!e.Mouse[MouseButton.Left] || modulo++ % 3 > 0)
                return;

            Vector4 new_point = new Vector4((e.X - windowWidth / 2) / 15.0f, (-e.Y + windowHeight / 2) / 15.0f, 0, 1);
            if (pos.Count>0 && (new_point - pos.Last()).LengthFast < 0.1)
                return;
            pos.Add(new_point);
            update = true;
            Console.WriteLine(pos.Last().X + " " + pos.Last().Y);
        }
    }

    class MouseTrack : Vertexer
    {
        MouseTrackParam param;
        public MouseTrack(MouseTrackParam param)
        {
            this.param = param;
        }
        
        public override Vector4[] getData()
        {
            return param.pos.ToArray();
        }

        public override int genBuffer()
        {
            param.update = false;
            return base.genBuffer();
        }

        public override int Count
        {
            get { return param.Count; }
        }        
    }

    #endregion
}
