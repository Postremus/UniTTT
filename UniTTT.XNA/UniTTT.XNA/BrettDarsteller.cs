using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UniTTT.XNA
{
    class BrettDarsteller : Logik.IGraphicalBrettDarsteller
    {
        public BrettDarsteller(ref SpriteBatch b, GraphicsDevice graphic, ContentManager content)
        {
            Batch = b;
            Graphics = graphic;
            Width = Height = 3;
            BrettLines = new List<Rectangle>();
            FieldTextures = new Texture2D[Width, Height];
            Fields = new Rectangle[Width, Height];
            Content = content;
            AllowUpdate = true;
            BrettLineTextur = new Texture2D(Graphics, 1, 1);
            BrettLineTextur.SetData<Color>(new Color[] { Color.Gray });
            Create();
        }
        
        #region Fields
        public int Width { get; private set; }
        public int Height { get; private set; }
        private List<Rectangle> BrettLines { get; set; }
        private Texture2D BrettLineTextur { get; set; }
        private GraphicsDevice Graphics { get; set; }
        private SpriteBatch Batch { get; set; }
        private Texture2D[,] FieldTextures { get; set; }
        public Rectangle[,] Fields { get; private set; }
        private ContentManager Content { get; set; }
        public bool AllowUpdate { get; private set; }
        #endregion

        public void Update(char[,] brett)
        {
            if (AllowUpdate)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        FieldTextures[x, y] = brett[x, y] == 'X' ?
                            Content.Load<Texture2D>("PlayerSymbols\\X") :
                            brett[x, y] == 'O' ? Content.Load<Texture2D>("PlayerSymbols\\O") :
                            new Texture2D(Graphics, 50, 50);
                    }
                }
            }
        }

        public void Draw()
        {
            // Linien Zeichnen
            foreach (var line in BrettLines)
                Batch.Draw(BrettLineTextur, line, Color.Black);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Batch.Draw(FieldTextures[x, y], Fields[x, y], Color.White);
        }

        public void Create()
        {
            // Alle Linien Senkrecht
            for (int x = 0; x < Width-1; x++)
                BrettLines.Add(new Rectangle((x + 1) * 50, 0, 1, Height * 50 + Height - 1));

            // Alle Linen Horizontal
            for (int y = 0; y < Height-1; y++)
                BrettLines.Add(new Rectangle(0, (y + 1) * 50, Width * 50 + Width - 1, 1));

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Fields[x, y] = new Rectangle(x * 50 + x, y * 50 + y, 50, 50);
            Update(new char[3, 3]);
        }

        public void DeLock() 
        {
            AllowUpdate = true;
        }

        public void Lock() 
        {
            AllowUpdate = false;
        }
    }
}