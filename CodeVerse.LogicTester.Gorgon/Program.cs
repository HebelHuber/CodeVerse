using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DX = SharpDX;
using Gorgon.Core;
using Gorgon.Graphics;
using Gorgon.Graphics.Core;
using Gorgon.Graphics.Imaging.Codecs;
using Gorgon.Renderers;
using Gorgon.UI;
using Gorgon.Timing;
using CodeVerse.Logic;
using CodeVerse.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gorgon.Graphics.Fonts;

namespace CodeVerse.LogicTester.Gorgon
{
    static class Program
    {
        private static GorgonGraphics _graphics;
        private static GorgonSwapChain _screen;
        private static Gorgon2D _renderer;
        private static GorgonFont _font;

        private static bool RenderLoop()
        {
            // Before we render anything, let's update our sprite.

            _screen.RenderTargetView.Clear(GorgonColor.CornFlowerBlue);
            _graphics.SetRenderTarget(_screen.RenderTargetView);

            _renderer.Begin();

            GorgonDrawer.DrawEntities(_renderer, ents, mapsize, screenMin, _font);

            _renderer.End();

            _screen.Present(1);

            return true;
        }

        private static void Initialize(Form form)
        {
            IReadOnlyList<IGorgonVideoAdapterInfo> videoDevices = GorgonGraphics.EnumerateAdapters();
            _graphics = new GorgonGraphics(videoDevices[0]);
            _screen = new GorgonSwapChain(_graphics, form, new GorgonSwapChainInfo("Our Screen")
            {
                Width = form.ClientSize.Width,
                Height = form.ClientSize.Height,
                Format = BufferFormat.R8G8B8A8_UNorm
            });

            var FF = new GorgonFontFactory(_graphics);
            _font = FF.DefaultFont;
            _renderer = new Gorgon2D(_graphics);
        }

        private static DefaultSimulator sim;
        private static List<Entity> ents;
        private static float mapsize = 2000f;
        private static int screenMin;

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var form = new Form1();
                Initialize(form);

                screenMin = Math.Min(form.ClientSize.Height, form.ClientSize.Width);
                sim = new DefaultSimulator(20, mapsize, debugmode: true);
                ents = sim.GetDebugEntities();
                Task.Run(() => { KeepTickin(); });


                GorgonApplication.AllowBackground = true;
                GorgonApplication.Run(form, RenderLoop);
            }
            catch (Exception ex)
            {
                ex.Catch(_ => GorgonDialogs.ErrorBox(null, ex));
            }
            finally
            {
                _font?.Dispose();
                _renderer?.Dispose();
                _screen?.Dispose();
                _graphics?.Dispose();
            }
        }

        private static void KeepTickin(float simFps = 25)
        {
            Thread.Sleep(2500);

            int fpsAsMilliseconds = Convert.ToInt32(1000f / simFps);

            while (true)
            {
                sim.Simulate();
                ents = sim.GetDebugEntities();
                Thread.Sleep(fpsAsMilliseconds);
            }
        }
    }
}