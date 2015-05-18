using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Glshj.Abstract;
using Glshj.Curve;
using Glshj.Regural2D;
using Glshj.Complex2D;
using Glshj.Combine;
using Glshj.Drag;
using Glshj.Util;

namespace TestGljsh
{
    class Test2 :GameWindow
    {
        Vector3 pos = Vector3.Zero;
        Vector3 rot = Vector3.Zero;
        Vector4 lightpos = new Vector4(0, 0, 10, 1);
        Vector4 matspec = new Vector4(0.1f, 0.1f, 0.1f, 1);
        Vector4 matemission = new Vector4(0.3f, 0.2f, 0.2f, 0.0f);
        float matshin = 3;
        bool lightson = false;

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

        Shape drag;
        Shape drag_regural;

        #endregion

        public Test2() : base(800, 600) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeyDown += Keyboard_KeyDown;
            MouseMove += Mouse_Move;
            MouseDown += Mouse_ButtonDown;
            Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(Test1_Closing);

            Fib fib_param = new Fib(10);
            CurveElementer fib_elementer = new CurveElementer(fib_param);
            DragVertexer drag_v = new DragVertexer( fib_param, new VertexEditor( fib_param, new EditorParam(Matrix4.CreateTranslation(10,0,0))));
            drag = new Shape(drag_v, new Controller(null, new DragElementer(fib_elementer, fib_elementer, fib_param), new OneColorColorer(Color.Blue, drag_v), null), PrimitiveType.QuadStrip);
            addToDestroy(drag);

            Regular2DParam regural_param = new Regular2DParam(5, 4);
            Elementer regural_elm = new LoopElementer(new Regular2DElementer(regural_param), 2);
            Regular2DVertexer regural_v = new Regular2DVertexer(regural_param);
            DragVertexer drag_regural_v = new DragVertexer(regural_v, new VertexEditor(regural_v, new EditorParam(Matrix4.CreateTranslation(0, 0, 5))));
            drag_regural = new Shape(drag_regural_v, new Controller(null, new DragElementer(regural_elm, regural_elm, regural_v), new OneColorColorer(Color.Green, drag_regural_v), null), PrimitiveType.QuadStrip);
            addToDestroy(drag_regural);

            GL.ClearColor(System.Drawing.Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);
            if(lightson){
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            }
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.CullFace(CullFaceMode.Front);
            GL.ShadeModel(ShadingModel.Smooth);
        }

        #region control
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
            int extra = 1;
            if (e.Keyboard[Key.ShiftLeft] || e.Keyboard[Key.ShiftRight])
                extra = 3;

            if (e.Key == Key.Escape)
            {
                this.Exit();
            }
            else if (e.Key == Key.Up)
                pos.Y += 1 * extra;
            else if (e.Key == Key.Down)
                pos.Y -= 1 * extra;
            else if (e.Key == Key.Right)
                pos.X += 1 * extra;
            else if (e.Key == Key.Left)
                pos.X -= 1 * extra;
            else if (e.Key == Key.Keypad1)
                pos.Z += 1 * extra;
            else if (e.Key == Key.Keypad0)
                pos.Z -= 1 * extra;
            else if (e.Key == Key.Keypad4)
                rot.X += 5 * extra;
            else if (e.Key == Key.Keypad5)
                rot.Y += 5 * extra;
            else if (e.Key == Key.Keypad6)
                rot.Z += 5 * extra;
            else if (e.Key == Key.Keypad7)
                rot.X -= 5 * extra;
            else if (e.Key == Key.Keypad8)
                rot.Y -= 5 * extra;
            else if (e.Key == Key.Keypad9)
                rot.Z -= 5 * extra;
            else if (e.Key == Key.L)
            {
                lightson = !lightson;
                if (lightson)
                {
                    GL.Enable(EnableCap.Lighting);
                    GL.Enable(EnableCap.Light0);
                }
                else
                {
                    GL.Disable(EnableCap.Lighting);
                    GL.Disable(EnableCap.Light0);
                }
            }
            //System.Console.WriteLine(e.Key.ToString());
        }
        #endregion
        
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

            if (lightson)
            {
                GL.Light(LightName.Light0, LightParameter.Position, lightpos);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, matspec);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, matshin);
            }
            GL.PushMatrix();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(255, 0, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);
            GL.Color4(0, 255, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);
            GL.Color4(0, 0, 255, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);
            GL.End();
            if (lightson)
            {
                GL.Translate(-lightpos.Xyz);
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(255, 0, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(1, 0, 0);
                GL.Color4(0, 255, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 1, 0);
                GL.Color4(0, 0, 255, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, 1);
                GL.End();
            }
            GL.PopMatrix();
            GL.PushMatrix();
            GL.Translate(0, 4, 0);
            drag.Draw();
            GL.Translate(-15, 0, 0);
            drag.Draw(PrimitiveType.LineStrip);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, -4, 0);
            drag_regural.Draw();
            GL.Translate(-15, 0, 0);
            drag_regural.Draw(PrimitiveType.LineStrip);
            GL.PopMatrix();
            
            this.SwapBuffers();
        }
    }
}
