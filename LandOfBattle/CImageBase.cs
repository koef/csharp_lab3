using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CImageBase : IDisposable
    {
        Bitmap _bitmap;
        private int X;
        private int Y;

        public int Left { get { return X; }; set { X = value; } }
        public int Top { get { return Y; }; set { Y = value; } }

        public CImageBase(Bitmap _resource)
        {
            _bitmap = new Bitmap(_resource);
        }

        public void DrawImage(Graphics gfx)
        {
            gfx.DrawImage(_bitmap, X, Y);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bitmap.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
