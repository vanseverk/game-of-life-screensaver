using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Screensavers;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class MyCoolScreensaver : Screensaver
{
    private static bool[,] fields = new bool[475, 270];
    private static Random rand = new Random();
    private static DateTime last;
    public MyCoolScreensaver()
    {
        Initialize += new EventHandler(MyCoolScreensaver_Initialize);
        Update += new EventHandler(MyCoolScreensaver_Update);
        Exit += new EventHandler(MyCoolScreensaver_Exit);
    }

    void MyCoolScreensaver_Initialize(object sender, EventArgs e)
    {

        randomField();

        last = DateTime.Now;
    }

    private static void randomField()
    {
        for (int i = 0; i < 270; i++)
        {
            for (int j = 0; j < 475; j++)
            {
                fields[j, i] = false;
            }
        }

        fields[101, 102] = true;
        fields[102, 102] = true;
        fields[103, 102] = true;
        fields[103, 101] = true;
        fields[102, 100] = true;
       // fields[102, 100] = fields[102, 101] = fields[102, 102] = true;
        for (int i = 0; i < 270; i++)
        {
            for (int j = 0; j < 475; j++)
            {
                fields[j, i] = rand.NextDouble() > 0.85;
            }
        }

        //let's start a couple of gens further
        for (int i = 0; i < 10; i++)
        {
            nextGeneration();
        }
    }

    private static int numberNeighbours(int CellX, int CellY, bool[,] array)
    {
        int count = 0;

        int minX = Math.Max(CellX - 1, array.GetLowerBound(0));
        int maxX = Math.Min(CellX + 1, array.GetUpperBound(0));
        int minY = Math.Max(CellY - 1, array.GetLowerBound(1));
        int maxY = Math.Min(CellY + 1, array.GetUpperBound(1));

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (array[x, y] && (x != CellX || y != CellY))
                    count++;
            }
        }
        return count;
    }
    private static int nmbgens = 0;

    private static void nextGeneration()
    {
        bool[,] nextgenFields = new bool[475, 270];

        for (int i = 0; i < 270; i++)
        {
            for (int j = 0; j < 475; j++)
            {
                int nb = numberNeighbours(j, i, fields);
                if(nb == 3 || (fields[j,i] && nb == 2))
                    nextgenFields[j, i] = true;
            }
        }
        fields = nextgenFields;
        nmbgens++;
    }

    void MyCoolScreensaver_Update(object sender, EventArgs e)
    {
        Graphics0.Clear(Color.Black);

        Graphics0.DrawString(
           DateTime.Now.ToString() + "\n" + "Nb generations: " + nmbgens,
           SystemFonts.DefaultFont, Brushes.Blue,
           0, 0);
        
        DateTime now = DateTime.Now;
        if ((now - last).Milliseconds > 200)
        {
            last = now;
            nextGeneration();
        }

       // Graphics0.FillRectangle(Brushes.Black, 0, 0, 475, 270);

        for (int i = 0; i < 270; i++)
        {
            for (int j = 0; j < 475; j++)
            {
                if (fields[j, i])
                    Graphics0.FillRectangle(Brushes.White, j * 4, i * 4, 4, 4);
            }
        }
    }

    void MyCoolScreensaver_Exit(object sender, EventArgs e)
    {
    }

    [STAThread]
    static void Main()
    {
        Screensaver ss = new MyCoolScreensaver();
        ss.Run();
    }
}


/*namespace golscreensaver
{
    class MyCoolScreensaver : Screensavers.Screensaver
    {
        public MyCoolScreensaver()
            : base(FullscreenMode.MultipleWindows)
        {
            Initialize += new EventHandler(MyCoolScreensaver_Initialize);
            Update += new EventHandler(MyCoolScreensaver_Update);
            Exit += new EventHandler(MyCoolScreensaver_Exit);
        }

        Device device;

        static int Height = 0;
        static int Width = 0;

        Microsoft.DirectX.Direct3D.Font font;
        int updates;

        void MyCoolScreensaver_Initialize(object sender, EventArgs e)
        {
            PresentParameters pp = new PresentParameters();

            if (this.Mode != ScreensaverMode.Normal)
                pp.Windowed = true;
            else
            {
                pp.Windowed = false;
                pp.BackBufferCount = 1;
                pp.BackBufferWidth =
                   Manager.Adapters[Window0.DeviceIndex].CurrentDisplayMode.Width;
                pp.BackBufferHeight =
                   Manager.Adapters[Window0.DeviceIndex].CurrentDisplayMode.Height;
                pp.BackBufferFormat =
                   Manager.Adapters[Window0.DeviceIndex].CurrentDisplayMode.Format;
            }

            pp.SwapEffect = SwapEffect.Flip;
            device = new Device(
               Window0.DeviceIndex, DeviceType.Hardware,
               Window0.Handle, CreateFlags.HardwareVertexProcessing, pp);

            Window0.DoubleBuffer = false;
            font = new Microsoft.DirectX.Direct3D.Font(
               device, System.Drawing.SystemFonts.DefaultFont);
            Microsoft.DirectX.Direct3D

            Height = Manager.Adapters[Window0.DeviceIndex].CurrentDisplayMode.Height;
            Width = Manager.Adapters[Window0.DeviceIndex].CurrentDisplayMode.Width;
        }

        void MyCoolScreensaver_Update(object sender, EventArgs e)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            writer.WriteLine("Time: " + DateTime.Now);
            writer.WriteLine("Achieved framerate: " + this.AchievedFramerate);
            writer.WriteLine("Update count: " + updates++);
            writer.WriteLine("Device: " + Window0.DeviceIndex);
            writer.WriteLine("Height: " + Height);
            writer.WriteLine("Width: " + Width);

            device.Clear(ClearFlags.Target, System.Drawing.Color.Black, 0, 0);

            device.BeginScene();

            font.DrawText(null, writer.ToString(), 0, 0,
              System.Drawing.Color.Blue.ToArgb());

            device.DrawRectanglePatch(

            device.EndScene();
            device.Present();
        }

        void MyCoolScreensaver_Exit(object sender, EventArgs e)
        {
            device.Dispose();
        }

        [STAThread]
        static void Main()
        {
            Screensaver ss = new MyCoolScreensaver();
            ss.Run();
        }
    }
}
*/