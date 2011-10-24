using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UniTTT.XNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BrettDarsteller darsteller;
        Logik.Brett brett;
        MouseState MState;
        Logik.Player.AbstractPlayer player;
        HumanPlayer player1 = new HumanPlayer('X');
        Logik.Player.AbstractPlayer player2 = new HumanPlayer('O');
        Logik.GewinnPrüfer pruefer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SpielerTausch();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            darsteller = new BrettDarsteller(ref spriteBatch, graphics.GraphicsDevice, Content);
            brett = new Logik.Brett(3, 3);
            pruefer = new Logik.GewinnPrüfer(3);

            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth = 152;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MState = Mouse.GetState();

            if (MState.LeftButton == ButtonState.Pressed)
                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                        if (brett.IsFieldEmpty(x, y))
                            if (darsteller.Fields[x, y].Contains(new Point(MState.X, MState.Y)))
                            {
                                if (darsteller.AllowUpdate)
                                {
                                    brett.Setzen(player.Spieler, new Logik.Vector(x, y));
                                    darsteller.Update(brett.VarBrett);

                                    Logik.Brett.GameStates state = brett.GetGameState(brett.VarBrett, player.Spieler);
                                    if (state != Logik.Brett.GameStates.Laufend)
                                    {
                                        darsteller.Sperren();
                                    }
                                    else if (state == Logik.Brett.GameStates.Laufend)
                                        SpielerTausch();
                                }
                            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
            darsteller.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SpielerTausch()
        {
            player = player1 == player ? player2 : player1;
        }

        public override string ToString()
        {
            return (player1 is Logik.Player.KIPlayer) && (player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
    }
}
